using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.OrderRequest;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Services;
using EFCore.BulkExtensions;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace AdCourier.Infrastructure.Data
{
    public class QuickOrderRepository : IQuickOrderRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        public QuickOrderRepository(SqlServerContext sqlServerContext,
            IOptions<ConnectionStringList> connectionStrings)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<List<CourierOrders>> AddQuickOrders(List<CourierOrders> courierOrders)
        {
            //await _sqlServerContext.BulkInsertAsync(courierOrders, new BulkConfig { SetOutputIdentity = true });
            //await _sqlServerContext.BulkInsertAsync(courierOrders);

            //return courierOrders;


            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.CourierOrders.AddRangeAsync(courierOrders);
            await _sqlServerContext.SaveChangesAsync();
            return courierOrders;
        }

        public async Task<List<CourierOrders>> GetGenerateQuickOrders(RequestBodyModel requestBodyModel)
        {
            return await _sqlServerContext.CourierOrders.Where(q => q.QuickOrderGenerateDate.HasValue && q.QuickOrderGenerateDate.Value.Date >= requestBodyModel.FromDate
                                && q.QuickOrderGenerateDate.Value.Date < requestBodyModel.ToDate.AddDays(1)
                                && q.QuickOrderGenerateForHub == requestBodyModel.HubName
                                && q.IsQuickOrder.Equals(true)
                                && q.OrderFrom.Equals(string.Empty)).ToListAsync();

        }

        public async Task<bool> CheckIsQuickOrder(string orderId)
        {
            int startIndex = 3;
            int endIndex = orderId.Length - 3;
            int id = Convert.ToInt32(orderId.Substring(startIndex, endIndex));

            var isQuickOrder = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(x => x.Id.Equals(id)
                && x.IsQuickOrder.Equals(true)
                && x.OrderFrom.Equals("")
            );

            if (isQuickOrder != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> IsAcceptedQuickOrder(int orderRequestId)
        {
            
            var isAccepted = await _sqlServerContext.OrderRequest.FirstOrDefaultAsync(x => x.OrderRequestId.Equals(orderRequestId)
                && x.Status.Equals(41)
            );

            if (isAccepted != null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<CourierUsers>> GetMerchantByCompanyName(string companyName)
        {
            return await _sqlServerContext.CourierUsers
                                .Where(x => x.CompanyName.ToLower().Contains(companyName.ToLower())).Select(s => new CourierUsers
                                {
                                    CompanyName = s.CompanyName,
                                    CourierUserId = s.CourierUserId
                                }).ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> LoadQuickOrder(RequestBodyModel request)
        {

            var allDistricts = await _sqlServerContext.Districts.Where(z => z.ParentId == 0 && z.IsActive == true).OrderBy(x => x.DistrictPriority).ToListAsync();
            //var allDeliveryRange = await _sqlServerContext.DeliveryRange.Where(z => z.IsActive == true).Select(x => new { DeliveryRangeId = x.Id, DeliveryType = x.Name + " " + x.Day }).ToListAsync();
            //var allDeliveryRange = await _sqlServerContext.DeliveryRange.Where(z => z.IsActive == true).Select(x => new { DeliveryRangeId = x.Id, OnImageLink = x.OnImageLink, OffImageLink = x.OffImageLink}).ToListAsync();
            var allWeightRange = await _sqlServerContext.WeightRange.Select(x => new { WeightRangeId = x.Id, Weight = x.Weight }).ToListAsync();

            var quickOrders = await(from data in _sqlServerContext.CourierOrders.AsNoTracking()
                              join _deliveryChargeInfo in _sqlServerContext.DeliveryChargeDetails on new { data.DistrictId, data.ThanaId,data.AreaId, data.ServiceType } equals new { _deliveryChargeInfo.DistrictId, _deliveryChargeInfo.ThanaId, _deliveryChargeInfo.AreaId, _deliveryChargeInfo.ServiceType } 
                              join _deliverRange in _sqlServerContext.DeliveryRange on _deliveryChargeInfo.DeliveryRangeId equals _deliverRange.Id   
                              join _courierUsers in _sqlServerContext.CourierUsers on data.MerchantId equals _courierUsers.CourierUserId
                              where data.IsQuickOrder == true
                              && data.IsActive == true
                              && data.OrderFrom.Equals("quick")
                              && data.Mobile ==""
                              && data.Status == 44
                              && data.OrderDate >= request.FromDate && data.OrderDate < request.ToDate.AddDays(1)
                              orderby data.Id descending
                              select new
                              {
                                  Id = data.Id,
                                  CustomerName = data.CustomerName,
                                  Mobile = data.Mobile,
                                  OtherMobile = data.OtherMobile,
                                  Address = data.Address,
                                  DistrictId = data.DistrictId,
                                  ThanaId = data.ThanaId,
                                  AreaId = data.AreaId,
                                  DeliveryRangeId = data.DeliveryRangeId,
                                  WeightRangeId = data.WeightRangeId,
                                  ServiceType = data.ServiceType,
                                  PaymentType = data.PaymentType,
                                  DeliveryCharge = data.DeliveryCharge,
                                  PackagingName = data.PackagingName,
                                  PackagingCharge = data.PackagingCharge,
                                  BreakableCharge = data.BreakableCharge > 0? true:false,
                                  ActualPackagePrice = data.ActualPackagePrice,
                                  CollectionAmount = data.CollectionAmount,
                                  Weight = data.Weight,
                                  MerchantMobile = _courierUsers.Mobile,
                                  CollectionName = data.CollectionName,
                                  IsQuickOrder = data.IsQuickOrder,
                                  QuickOrderImageUrl = data.QuickOrderImageUrl,
                                  CollectAddressDistrictId = data.CollectAddressDistrictId,
                                  DeliveryRangeInfos = data.DeliveryRangeId == 0 ? null : new 
                                  {
                                      DeliveryRangeId = _deliverRange.Id,
                                      OnImageLink = _deliverRange.OnImageLink,
                                      OffImageLink = _deliverRange.OffImageLink
                                  },
                                  WeightRangeInfos = data.WeightRangeId != 0 ? allWeightRange : null,
                                  Districts = allDistricts,
                                  Thanas = data.ThanaId != 0 ? _sqlServerContext.Districts.Where(z => z.ParentId == data.DistrictId && z.IsActive == true) : null,
                                  Areas = data.AreaId != 0 ? _sqlServerContext.Districts.Where(z => z.ParentId == data.ThanaId && z.IsActive == true) : null
                              }).Distinct().ToListAsync();


            var response = quickOrders.GroupBy(y => new { y.Id, y.DistrictId, y.ThanaId, y.AreaId, y.ServiceType })
                           .Select(x => new
                           {
                               Id = x.Key.Id,
                               CourierOrdersId = "DT-" + x.Key.Id,
                               CustomerName = x.FirstOrDefault().CustomerName,
                               Mobile = x.FirstOrDefault().Mobile,
                               OtherMobile = x.FirstOrDefault().OtherMobile,
                               Address = x.FirstOrDefault().Address,
                               DistrictId = x.Key.DistrictId,
                               ThanaId = x.Key.ThanaId,
                               AreaId = x.Key.AreaId,
                               DeliveryRangeId = x.FirstOrDefault().DeliveryRangeId,
                               WeightRangeId = x.FirstOrDefault().WeightRangeId,
                               ServiceType = x.Key.ServiceType,
                               PaymentType = x.FirstOrDefault().PaymentType,
                               DeliveryCharge = x.FirstOrDefault().DeliveryCharge,
                               PackagingName = x.FirstOrDefault().PackagingName,
                               PackagingCharge = x.FirstOrDefault().PackagingCharge,
                               BreakableCharge = x.FirstOrDefault().BreakableCharge,
                               ActualPackagePrice = x.FirstOrDefault().ActualPackagePrice,
                               CollectionAmount = x.FirstOrDefault().CollectionAmount,
                               Weight = x.FirstOrDefault().Weight,
                               MerchantMobile = x.FirstOrDefault().MerchantMobile,
                               CollectionName = x.FirstOrDefault().CollectionName,
                               IsQuickOrder = x.FirstOrDefault().IsQuickOrder,
                               QuickOrderImageUrl = x.FirstOrDefault().QuickOrderImageUrl,
                               CollectAddressDistrictId = x.FirstOrDefault().CollectAddressDistrictId,
                               DeliveryRangeInfos = x.GroupBy(z=> z.DeliveryRangeInfos.DeliveryRangeId).Select(q=> q.FirstOrDefault().DeliveryRangeInfos).ToList(),
                               WeightRangeInfos = x.FirstOrDefault().WeightRangeInfos,
                               Districts = x.FirstOrDefault().Districts,
                               Thanas = x.FirstOrDefault().Thanas,
                               Areas = x.FirstOrDefault().Areas
                           });

            return response;
        }

        public async Task<CourierOrders> UpdateOrderInfoForApp(CourierOrders orders)
        {
            int startIndex = 3;
            int endIndex = orders.CourierOrdersId.Length - 3;
            int id = Convert.ToInt32(orders.CourierOrdersId.Substring(startIndex, endIndex));

            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.DistrictId = orders.DistrictId;
                entity.ThanaId = orders.ThanaId;
                entity.AreaId = orders.AreaId;
                entity.CollectAddressDistrictId = orders.CollectAddressDistrictId;
                entity.CollectAddressThanaId = orders.CollectAddressThanaId;
                entity.PaymentType = orders.PaymentType;
                entity.DeliveryRangeId = orders.DeliveryRangeId;
                entity.WeightRangeId = orders.WeightRangeId;
                entity.Weight = orders.Weight;
                entity.DeliveryCharge = orders.DeliveryCharge;
                entity.OrderFrom = "quick";
                entity.CollectionTimeSlotId = orders.CollectionTimeSlotId;
                entity.CollectionTime = orders.CollectionTime;
                entity.QuickOrderImageUrl = orders.QuickOrderImageUrl;
                entity.ServiceType = orders.ServiceType;
                entity.MerchantId = orders.MerchantId;
                entity.OrderRequestId = orders.OrderRequestId;
                entity.Status = orders.Status;
                entity.OrderDate = DateTime.Now;
                entity.DeliveryUserId = orders.DeliveryUserId;
                entity.CollectionAmount = orders.CollectionAmount;
                entity.ActualPackagePrice = orders.ActualPackagePrice;
                entity.CodCharge = orders.CodCharge;
                entity.OrderType = orders.OrderType;
                entity.UpdatedBy = orders.UpdatedBy;
                entity.UpdatedOn = DateTime.Now;
                entity.Comment = orders.Comment;
                entity.IsConfirmedBy = orders.IsConfirmedBy;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    orders = _sqlServerContext.Update(entity).Entity;
                    await _sqlServerContext.SaveChangesAsync();
                }

                var orderStatusHistory = new CourierOrderStatusHistory()
                {
                    CourierOrderId = orders.CourierOrdersId,
                    IsConfirmedBy = orders.IsConfirmedBy,
                    OrderDate = orders.OrderDate,
                    Status = orders.Status,
                    PostedBy = orders.UpdatedBy,
                    PostedOn = orders.UpdatedOn,
                    MerchantId = orders.MerchantId,
                    Comment = orders.Comment,
                    PodNumber = orders.PodNumber,
                    CourierId = orders.CourierId,
                    HubName = orders.HubName,
                    CourierDeliveryManName = orders.CourierDeliveryManName,
                    CourierDeliveryManMobile = orders.CourierDeliveryManMobile
                };

                 _sqlServerContext.CourierOrderStatusHistory.AddRange(orderStatusHistory);
                await _sqlServerContext.SaveChangesAsync();
                return entity;
            }
            return null;
        }
        public async Task<IEnumerable<OrderRequestViewModel>> GetMerchantQuickOrders(RequestBodyModel request)
        {
            var requestOrders = await(from _orderRequest in _sqlServerContext.OrderRequest.AsNoTracking()
                                       join o in _sqlServerContext.CourierOrders.AsNoTracking() on _orderRequest.OrderRequestId equals o.OrderRequestId into orders from _courierOrders in orders.DefaultIfEmpty()
                                       join district in _sqlServerContext.Districts.AsNoTracking() on _orderRequest.DistrictId equals district.DistrictId
                                       join thana in _sqlServerContext.Districts.AsNoTracking() on _orderRequest.ThanaId equals thana.DistrictId

                                       where _orderRequest.CourierUserId.Equals(request.MerchantId)
                                       && _orderRequest.RequestDate >= request.FromDate
                                       && _orderRequest.RequestDate < request.ToDate.AddDays(1)
                                       orderby _orderRequest.OrderRequestId descending
                                       select new 
                                       {
                                           OrderRequestId = _orderRequest.OrderRequestId,
                                           RequestOrderAmount = _orderRequest.RequestOrderAmount,
                                           CollectionDate = _orderRequest.CollectionDate,
                                           Status = _orderRequest.Status,
                                           CollectionTimeSlotId = _orderRequest.CollectionTimeSlotId,
                                           DistrictsViewModel = new DistrictsViewModel
                                           {
                                               DistrictId = district.DistrictId,
                                               District = district.DistrictBng,
                                               ThanaId = thana.DistrictId,
                                               Thana = thana.DistrictBng
                                           },
                                           ActionViewModel = new SelfDeliveryService().GetActionModel(_orderRequest.Status)
                                       }).ToListAsync();


            var response = requestOrders.GroupBy(x => x.OrderRequestId).Select(y => new OrderRequestViewModel
            {
                OrderRequestId = y.Key,
                RequestOrderAmount = y.FirstOrDefault().RequestOrderAmount,
                CollectionDate = y.FirstOrDefault().CollectionDate,
                Status = y.FirstOrDefault().Status,
                TotalOrder = y.Where(z=> z.Status == 44).Count(),
                DistrictsViewModel = y.FirstOrDefault().DistrictsViewModel,
                ActionViewModel = y.FirstOrDefault().ActionViewModel,
                CollectionTimeSlotId = y.FirstOrDefault().CollectionTimeSlotId
            });

            return response;
        }


        public async Task<IEnumerable<CourierUsersViewModel>> GetQuickOrders(RequestBodyModel request)
        {
            //DateTime currendDate = new DateTime(2021, 06, 14, 8, 0, 15);

            if (request.DeliveryRiderId > 0 && request.StatusId.Equals(0))
            {
                var quickOrders = await (from _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                                         join _orderRequest in _sqlServerContext.OrderRequest on _courierUsers.CourierUserId equals _orderRequest.CourierUserId
                                         join _locationAssign in _sqlServerContext.LocationAssign on new { _orderRequest.DistrictId, _orderRequest.ThanaId } equals new { _locationAssign.DistrictId, _locationAssign.ThanaId }
                                         join _districts in _sqlServerContext.Districts on _orderRequest.DistrictId equals _districts.DistrictId
                                         join _thana in _sqlServerContext.Districts on _orderRequest.ThanaId equals _thana.DistrictId
                                         join _collectionTimeSlot in _sqlServerContext.CollectionTimeSlot on _orderRequest.CollectionTimeSlotId equals _collectionTimeSlot.CollectionTimeSlotId
                                         where _locationAssign.DeliveryUserId.Equals(request.DeliveryRiderId)
                                         && _orderRequest.Status.Equals(0)
                                         && _orderRequest.CollectionDate.Date == DateTime.Now.Date
                                         && DateTime.Now.TimeOfDay >= _collectionTimeSlot.StartTime.Value
                                         && DateTime.Now.TimeOfDay <= _collectionTimeSlot.EndTime.Value
                                         select new
                                         {
                                             OrderRequestId = _orderRequest.OrderRequestId,
                                             RequestOrderAmount = _orderRequest.RequestOrderAmount,
                                             _orderRequest.CollectionDate,
                                             _collectionTimeSlot.CollectionTimeSlotId,
                                             _collectionTimeSlot.StartTime,
                                             _collectionTimeSlot.EndTime,
                                             _orderRequest.Status,
                                             _courierUsers.CompanyName,
                                             _courierUsers.CourierUserId,
                                             _districts.DistrictBng,
                                             DistrictId = _districts.DistrictId,
                                             ThanaBng = _thana.DistrictBng,
                                             ThanaId = _thana.DistrictId,
                                             Mobile = _courierUsers.Mobile,
                                             AlterMobile = _courierUsers.AlterMobile,
                                             Address = _courierUsers.Address
                                         }).ToListAsync();

                var data = quickOrders.GroupBy(g => new { Year = g.CollectionDate.Year, Month = g.CollectionDate.Month, Day = g.CollectionDate.Day, g.CourierUserId, g.ThanaId, g.CollectionTimeSlotId }).Select(x => new CourierUsersViewModel
                {
                    CourierUserId = x.Key.CourierUserId,
                    CompanyName = x.FirstOrDefault().CompanyName,
                    Mobile = x.FirstOrDefault().Mobile,
                    AlterMobile = x.FirstOrDefault().AlterMobile,
                    Address = x.FirstOrDefault().Address,
                    DistrictsViewModel = new DistrictsViewModel
                    {
                        DistrictId = x.FirstOrDefault().DistrictId,
                        ThanaId = x.FirstOrDefault().ThanaId,
                        District = x.FirstOrDefault().DistrictBng,
                        Thana = x.FirstOrDefault().ThanaBng,
                    },
                    ActionModel = new SelfDeliveryService().SetActionModel(x.FirstOrDefault().Status),
                    OrderRequestList = x.GroupBy(gr => new { Year = gr.CollectionDate.Year, Month = gr.CollectionDate.Month, Day = gr.CollectionDate.Day, gr.CollectionTimeSlotId, gr.CourierUserId }).Select(r => new OrderRequestViewModel
                    {
                        OrderRequestSelfList = r.GroupBy(oa => new { oa.OrderRequestId, oa.RequestOrderAmount }).Select(item => new OrderRequestViewModel
                        {
                            OrderRequestId = item.Key.OrderRequestId,
                            RequestOrderAmount = item.Key.RequestOrderAmount
                        }).Distinct().ToList(),
                        RequestOrderAmount = r.GroupBy(j => j.OrderRequestId).Sum(y => y.FirstOrDefault().RequestOrderAmount), //r.Sum(s => s.RequestOrderAmount),
                        Status = r.FirstOrDefault().Status,
                        CollectionDate = r.FirstOrDefault().CollectionDate,
                        CollectionTimeSlot = new CollectionTimeSlotViewModel
                        {
                            CollectionTimeSlotId = r.Key.CollectionTimeSlotId,
                            StartTime = r.FirstOrDefault().StartTime,
                            EndTime = r.FirstOrDefault().EndTime,
                        }
                    }).ToList()
                });

                return data;
            }

            else if (request.DeliveryRiderId > 0 && request.StatusId > 0)
            {
                var quickOrders = await (from _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                                         join _orderRequest in _sqlServerContext.OrderRequest on _courierUsers.CourierUserId equals _orderRequest.CourierUserId
                                         join orders in _sqlServerContext.CourierOrders.AsNoTracking() on _orderRequest.OrderRequestId equals orders.OrderRequestId into order from _courierOrders in order.DefaultIfEmpty()
                                         join _locationAssign in _sqlServerContext.LocationAssign on new { _orderRequest.DistrictId, _orderRequest.ThanaId } equals new { _locationAssign.DistrictId, _locationAssign.ThanaId }
                                         join _districts in _sqlServerContext.Districts on _orderRequest.DistrictId equals _districts.DistrictId
                                         join _thana in _sqlServerContext.Districts on _orderRequest.ThanaId equals _thana.DistrictId
                                         join _collectionTimeSlot in _sqlServerContext.CollectionTimeSlot on _orderRequest.CollectionTimeSlotId equals _collectionTimeSlot.CollectionTimeSlotId
                                         where _orderRequest.DeliveryUserId.Equals(request.DeliveryRiderId)
                                         && _orderRequest.Status.Equals(request.StatusId)
                                         && _orderRequest.CollectionDate.ToShortDateString() == DateTime.Now.ToShortDateString()
                                         && DateTime.Now.TimeOfDay >= _collectionTimeSlot.StartTime.Value
                                         && DateTime.Now.TimeOfDay <= _collectionTimeSlot.EndTime.Value
                                         select new
                                         {
                                             OrderRequestId = _orderRequest.OrderRequestId,
                                             RequestOrderAmount = _orderRequest.RequestOrderAmount,
                                             OrderId = "DT-"+_courierOrders.Id == null? 0 :_courierOrders.Id,
                                             _orderRequest.CollectionDate,
                                             _collectionTimeSlot.StartTime,
                                             _collectionTimeSlot.EndTime,
                                             _collectionTimeSlot.CollectionTimeSlotId,
                                             _orderRequest.Status,
                                             _courierUsers.CompanyName,
                                             _courierUsers.CourierUserId,
                                             _districts.DistrictBng,
                                             DistrictId = _districts.DistrictId,
                                             ThanaBng = _thana.DistrictBng,
                                             ThanaId = _thana.DistrictId,
                                             Mobile = _courierUsers.Mobile,
                                             AlterMobile = _courierUsers.AlterMobile,
                                             Address = _courierUsers.Address
                                         }).ToListAsync();

                var data = quickOrders.GroupBy(g => new { Year = g.CollectionDate.Year, Month = g.CollectionDate.Month, Day = g.CollectionDate.Day, g.CourierUserId, g.ThanaId, g.CollectionTimeSlotId }).Select(x => new CourierUsersViewModel
                {
                    CourierUserId = x.Key.CourierUserId,
                    CompanyName = x.FirstOrDefault().CompanyName,
                    Mobile = x.FirstOrDefault().Mobile,
                    AlterMobile = x.FirstOrDefault().AlterMobile,
                    Address = x.FirstOrDefault().Address,
                    DistrictsViewModel = new DistrictsViewModel
                    {
                        DistrictId = x.FirstOrDefault().DistrictId,
                        ThanaId = x.FirstOrDefault().ThanaId,
                        District = x.FirstOrDefault().DistrictBng,
                        Thana = x.FirstOrDefault().ThanaBng,
                    },
                    ActionModel = new SelfDeliveryService().SetActionModel(x.FirstOrDefault().Status),
                    OrderRequestList = x.GroupBy(gr => new { Year = gr.CollectionDate.Year, Month = gr.CollectionDate.Month, Day = gr.CollectionDate.Day, gr.CollectionTimeSlotId, gr.CourierUserId }).Select(r => new OrderRequestViewModel
                    {
                        OrderRequestSelfList = r.GroupBy(oa => new { oa.OrderRequestId,oa.RequestOrderAmount }).Select(item => new OrderRequestViewModel {
                            OrderRequestId = item.Key.OrderRequestId,
                            RequestOrderAmount = item.Key.RequestOrderAmount
                        }).Distinct().ToList(),
                        RequestOrderAmount = r.GroupBy( j => j.OrderRequestId).Sum(y=> y.FirstOrDefault().RequestOrderAmount),  //r.Sum(s => s.RequestOrderAmount),
                        TotalOrder = r.Where(j => j.Status == 44 && j.OrderId > 0).GroupBy(e => e.OrderId).Count(),
                        Status = r.FirstOrDefault().Status,
                        CollectionDate = r.FirstOrDefault().CollectionDate,
                        CollectionTimeSlot = new CollectionTimeSlotViewModel
                        {
                            CollectionTimeSlotId = r.Key.CollectionTimeSlotId,
                            StartTime = r.FirstOrDefault().StartTime,
                            EndTime = r.FirstOrDefault().EndTime,
                        }
                    }).ToList()
                });

                return data;
            }

            return null;
        }

        public async Task<IEnumerable<OrderRequestViewModel>> GetMerchantWiseRequestOrders(RequestBodyModel requestBody)
        {

            var data = await (from request in _sqlServerContext.OrderRequest.AsNoTracking()

                        join order in _sqlServerContext.CourierOrders.AsNoTracking() 
                        on request.OrderRequestId equals order.OrderRequestId into orders from _courierOrders in orders.DefaultIfEmpty()

                        join users in _sqlServerContext.CourierUsers.AsNoTracking()
                        on request.CourierUserId equals users.CourierUserId
                        join timeSlot in _sqlServerContext.CollectionTimeSlot.AsNoTracking()
                        on request.CollectionTimeSlotId equals timeSlot.CollectionTimeSlotId
                        join locationAssign in _sqlServerContext.LocationAssign
                        on new { request.DistrictId, request.ThanaId } equals new { locationAssign.DistrictId, locationAssign.ThanaId }
                        join deliveryusers in _sqlServerContext.DeliveryUsers.AsNoTracking()
                        on locationAssign.DeliveryUserId equals deliveryusers.Id

                        where request.RequestDate.Date >= requestBody.FromDate.Date
                        && request.RequestDate.Date < requestBody.ToDate.Date.AddDays(1)
                        //&& orders.FirstOrDefault().OrderFrom.Equals("quick")
                        && request.CollectionTimeSlotId == (requestBody.CollectionTimeSlotId == 0 ? request.CollectionTimeSlotId : requestBody.CollectionTimeSlotId)
                        select new
                        {
                            request.OrderRequestId,
                            request.RequestOrderAmount,
                            request.ThanaId,
                            //_courierOrders.Id, null value does not accept int data type
                            _courierOrders.CourierOrdersId,
                            //_courierOrders.Status,
                            users.CourierUserId,
                            users.CompanyName,
                            locationAssign.DeliveryUserId,
                            deliveryusers.Name,
                            timeSlot.StartTime,
                            timeSlot.EndTime,
                            DeliveryUsers = new
                            {
                                DeliveryUserId = request.DeliveryUserId
                            },

                        }).Distinct().ToListAsync();

            var orderRequestViewModel = data.GroupBy(g => g.OrderRequestId).Select(s => new OrderRequestViewModel
            {

                OrderRequestId = s.Key,
                RequestOrderAmount = s.FirstOrDefault().RequestOrderAmount,
                ThanaId = s.FirstOrDefault().ThanaId,
                DeliveryUserId = s.FirstOrDefault().DeliveryUsers.DeliveryUserId, // it will get from the OrderRequest table
                //TotalOrder = s.Select(item => item.Id).Distinct().Count(),
                TotalOrder = s.Where(x=>x.CourierOrdersId != null).Select(item => item.CourierOrdersId).Distinct().Count(),
                DeliveryUsersList = s.Select(r => new DeliveryUsersViewModel
                {
                    Id = r.DeliveryUserId,
                    Name = r.Name
                }).ToList(),
                CourierUsersView = new CourierUsersViewModel
                {
                    CompanyName = s.FirstOrDefault().CompanyName,
                    CourierUserId = s.FirstOrDefault().CourierUserId
                },
                CollectionTimeSlot = new CollectionTimeSlotViewModel
                {
                    FormattingStartTime = new DateTime(s.FirstOrDefault().StartTime.Value.Ticks).ToString("hh:mm tt"),
                    FormattingEndTime = new DateTime(s.FirstOrDefault().EndTime.Value.Ticks).ToString("hh:mm tt"),
                    StartTime = s.FirstOrDefault().StartTime,
                    EndTime = s.FirstOrDefault().EndTime
                }
            });
           
            return orderRequestViewModel.ToList();
        }

        public async Task<int> UpdateOrderRequests(List<OrderRequest> orderRequests)
        {

            var orderRequestIds = (from d in orderRequests
                            select d.OrderRequestId).ToArray();

            var entity = await _sqlServerContext.OrderRequest.AsNoTracking()
                .Where(x => orderRequestIds.Contains(x.OrderRequestId))
                .BatchUpdateAsync(x => new OrderRequest
                {
                    DeliveryUserId = orderRequests.FirstOrDefault().DeliveryUserId,
                    DeliveryUserActionDate = DateTime.Now,
                    Status = orderRequests.FirstOrDefault().Status
                });

            return entity;
        }

        public async Task<CourierOrders> QuickOrderProcess(CourierOrders request)
        {
            var entity = await _sqlServerContext.CourierOrders.Where(x => x.CourierOrdersId == request.CourierOrdersId && x.IsQuickOrder == true).FirstOrDefaultAsync();
          
            if(entity != null)
            {
                entity.ActualPackagePrice = request.ActualPackagePrice;
                entity.DistrictId = request.DistrictId;
                entity.ThanaId = request.ThanaId;
                entity.AreaId = request.AreaId;
                entity.CodCharge = request.CodCharge;
                entity.CollectionAmount = request.CollectionAmount;
                entity.CollectionCharge = request.CollectionCharge;

                entity.DeliveryCharge = request.DeliveryCharge;
                entity.DeliveryRangeId = request.DeliveryRangeId;
                entity.WeightRangeId = request.WeightRangeId;
                entity.Weight = request.Weight;
                entity.PaymentType = request.PaymentType;
                entity.CustomerName = request.CustomerName;
                entity.Address = request.Address;
                entity.Mobile = request.Mobile;
                entity.CollectionName = request.CollectionName;
                entity.OrderType = request.OrderType;
                
                if(request.OtherMobile != "" )
                {
                    entity.OtherMobile = request.OtherMobile;
                }

                entity.BreakableCharge = request.BreakableCharge;
                if (request.PackagingCharge > 0)
                {
                    entity.PackagingName = request.PackagingName;
                    entity.PackagingCharge = request.PackagingCharge;
                }



                _sqlServerContext.Update(entity);

                await _sqlServerContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<int> UpdateMultipleTimeSlot(List<OrderRequest> orderRequests)
        {

            var orderRequestIds = (from d in orderRequests
                                    select d.OrderRequestId).ToArray();

            var entity = await _sqlServerContext.OrderRequest.AsNoTracking().Where(x => orderRequestIds.Contains(x.OrderRequestId))
               .BatchUpdateAsync(x => new OrderRequest { CollectionTimeSlotId = orderRequests.FirstOrDefault().CollectionTimeSlotId });

            return entity;
        }

        public async Task<int> UpdateRider(OrderRequest orderRequest)
        {
            
            var entity = await _sqlServerContext.OrderRequest.FirstOrDefaultAsync(item => item.OrderRequestId == orderRequest.OrderRequestId);
            if (entity != null)
            {
                if(orderRequest.ThanaId > 0)
                {
                    entity.ThanaId = orderRequest.ThanaId;
                }
                else if(orderRequest.DeliveryUserId > 0)
                {
                    entity.DeliveryUserId = orderRequest.DeliveryUserId;
                }
                
                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return 1;
            }
            return 0;
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetQuickOrderGenerateForHub(RequestBodyModel request)
        {

            var data = await (from _orders in _sqlServerContext.CourierOrders.AsNoTracking()

                              where _orders.QuickOrderGenerateForHub != ""
                              && _orders.QuickOrderGenerateDate >= request.FromDate
                              && _orders.QuickOrderGenerateDate < request.ToDate.AddDays(1)
                              select new
                              {
                                  _orders.QuickOrderGenerateForHub,
                                  _orders.Id

                              }).ToListAsync();

            var courierOrdersViewModel = data.GroupBy(g => g.QuickOrderGenerateForHub).Select(s => new CourierOrdersViewModel
            {

                QuickOrderGenerateForHub = s.FirstOrDefault().QuickOrderGenerateForHub,
                TotalOrder = s.Select(item => item.Id).Count(),
                
            });

            return courierOrdersViewModel.ToList();
        }

        public async Task<int> DeleteOrderRequest(int orderRequestId)
        {
            var entity = await _sqlServerContext.OrderRequest.Where(x => x.OrderRequestId == orderRequestId && x.Status == 0).FirstOrDefaultAsync();

            if(entity != null)
            {
                _sqlServerContext.OrderRequest.Remove(entity);
                return await _sqlServerContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> UpdateCollectionTimeSlotIdManually(int flag)
        {

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@flag", value: flag, dbType: DbType.Int32);

                var data = await connection.ExecuteAsync(
                        sql: @"[DT].[UpdateCollectionTimeSlotIdManually]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data;
            }
        }
    }
}
