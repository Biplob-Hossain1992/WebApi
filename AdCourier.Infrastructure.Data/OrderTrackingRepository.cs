using AdCourier.Context;
using AdCourier.Domain.Entities.BodyModel.OrderTracking;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CollectorAssign;
using AdCourier.Domain.Entities.ViewModel.OrderTracking;
using AdCourier.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using EFCore.BulkExtensions;
using AdCourier.Domain.Entities.ViewModel.DeliverManAssign;
using AdCourier.Domain.Entities.BodyModel.CollectorAssign;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using AdCourier.Domain.Entities;
using Microsoft.Extensions.Options;
using AdCourier.Domain.Entities.DapperDataModel;
using AdCourier.Domain.Entities.BodyModel;
using AdCourier.Domain.Entities.Utility;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using System.Net.Mail;

namespace AdCourier.Infrastructure.Data
{
    public class OrderTrackingRepository : IOrderTrackingRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        private SmtpClient _smtpClient;
        public OrderTrackingRepository(SmtpClient smtpClient, SqlServerContext sqlServerContext, IOptions<ConnectionStringList> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _smtpClient = smtpClient;
        }
        public async Task<StatusGroup> AddStatusGroup(StatusGroup statusGroup)
        {
            await _sqlServerContext.StatusGroup.AddAsync(statusGroup);
            await _sqlServerContext.SaveChangesAsync();
            return statusGroup;
        }
        public async Task<CourierOrderStatus> AddCourierOrderStatus(CourierOrderStatus courierOrderStatus)
        {
            await _sqlServerContext.CourierOrderStatus.AddAsync(courierOrderStatus);
            await _sqlServerContext.SaveChangesAsync();
            return courierOrderStatus;
        }

        public async Task<IEnumerable<PickOrderDapperModel>> GetOrderTrackingNew(OrderTrackingBodyModel orderTrackingBodyModel, string flag)
        {

            if (!(orderTrackingBodyModel.CourierOrderId.Trim().ToLower().Contains("dt")))
            {
                orderTrackingBodyModel.CourierOrderId = "dt-" + orderTrackingBodyModel.CourierOrderId;
            }

            int startIndex = 3;
            int endIndex = orderTrackingBodyModel.CourierOrderId.Length - 3;
            int id = Convert.ToInt32(orderTrackingBodyModel.CourierOrderId.Substring(startIndex, endIndex));

            try
            {
                using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
                {
                    connection.Open();

                    var parameter = new DynamicParameters();
                    parameter.Add(name: "@Id", value: id, dbType: DbType.Int32);
                    //parameter.Add(name: "@courierUserId", value: orderTrackingBodyModel.CourierUserId, dbType: DbType.Int32);

                    var data = await connection.QueryAsync<PickOrderDapperModel>(
                            sql: @"[DT].[OrderTracking]",
                            param: parameter,
                            commandType: CommandType.StoredProcedure);
                    return data.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<OrderTrackingStatusViewModel>> GetOrderTracking(OrderTrackingBodyModel orderTrackingBodyModel, string flag)
        {

            if (!(orderTrackingBodyModel.CourierOrderId.Trim().ToLower().Contains("dt")))
            {
                orderTrackingBodyModel.CourierOrderId = "dt-" + orderTrackingBodyModel.CourierOrderId;
            }


            var data = await (from csh in _sqlServerContext.CourierOrderStatusHistory
                              join cs in _sqlServerContext.CourierOrderStatus
                              on csh.Status equals cs.StatusId
                              join order in _sqlServerContext.CourierOrders
                              on csh.CourierOrderId equals order.CourierOrdersId
                              join dis in _sqlServerContext.Districts
                              on order.ThanaId equals dis.DistrictId
                              where csh.CourierOrderId == orderTrackingBodyModel.CourierOrderId.Trim()
                              orderby csh.Id ascending
                              select new
                              {
                                  OrderTrackStatusGroup = cs.OrderTrackStatusGroup,
                                  OrderTrackStatusPublicGroup = cs.OrderTrackStatusPublicGroup,
                                  StatusNameEng = cs.StatusNameEng,
                                  StatusNameBng = cs.StatusNameBng,
                                  PostedOn = csh.PostedOn,
                                  District = dis.District
                              }).ToListAsync();

            var reponse = new List<OrderTrackingStatusViewModel>();

            if (flag == "public")
            {
                var OrderTrackStatusPublicGroup = data.Select(x => x.OrderTrackStatusPublicGroup).Distinct();
                foreach (var item in OrderTrackStatusPublicGroup)
                {
                    var district = data.Where(x => x.OrderTrackStatusPublicGroup == item).Select(x => x.District).FirstOrDefault();

                    var orderTrackStatusPublicGroup = data.Where(x => x.OrderTrackStatusPublicGroup == item).Select(x => x.OrderTrackStatusPublicGroup).FirstOrDefault();

                    reponse.Add(new OrderTrackingStatusViewModel
                    {

                        OrderTrackStatusGroup = orderTrackStatusPublicGroup,
                        DateTime = data.Where(x => x.OrderTrackStatusPublicGroup == item).Select(x => x.PostedOn).FirstOrDefault(),
                        Status = data.Where(x => x.OrderTrackStatusPublicGroup == item).Select(y => new StatusNameViewModel
                        {
                            Name = y.StatusNameEng,
                            DateTime = y.PostedOn
                        }),
                        District = orderTrackStatusPublicGroup.ToLower() == "Product Reached to Last Hub".ToLower() ? ", " + district : ""
                    });
                }

                return reponse.Where(x => x.OrderTrackStatusGroup != "").ToList();
            }
            else if (flag == "private")
            {
                var OrderTrackStatusGroup = data.Select(x => x.OrderTrackStatusPublicGroup).Distinct();

                foreach (var item in OrderTrackStatusGroup)
                {
                    var district = data.Where(x => x.OrderTrackStatusPublicGroup == item).Select(x => x.District).FirstOrDefault();

                    var orderTrackStatusGroup = data.Where(x => x.OrderTrackStatusPublicGroup == item).Select(x => x.OrderTrackStatusGroup).FirstOrDefault();

                    reponse.Add(new OrderTrackingStatusViewModel
                    {

                        OrderTrackStatusGroup = orderTrackStatusGroup,
                        DateTime = data.Where(x => x.OrderTrackStatusGroup == item).Select(x => x.PostedOn).FirstOrDefault(),
                        Status = data.Where(x => x.OrderTrackStatusGroup == item).Select(y => new StatusNameViewModel
                        {
                            Name = y.StatusNameBng,
                            DateTime = y.PostedOn
                        }),
                        District = orderTrackStatusGroup.ToLower() == "Product Reached to Last Hub".ToLower() ? ", " + district : ""
                    });
                }

                return reponse.Where(x => x.OrderTrackStatusGroup != "").ToList();
            }

            return reponse; //.OrderByDescending(o => o.OrderTrackStatusGroup);
        }

        public async Task<IEnumerable<CourierOrderStatusViewModel>> GetCourierOrderStatus()
        {

            var data = await (from csh in _sqlServerContext.CourierOrderStatus
                              select csh).ToListAsync();

            var FulfillmentStatusGroup = data.Select(x => x.FulfillmentStatusGroup).Distinct();
            var reponse = new List<CourierOrderStatusViewModel>();
            foreach (var item in FulfillmentStatusGroup)
            {
                reponse.Add(new CourierOrderStatusViewModel
                {
                    FulfillmentStatusGroup = data.Where(x => x.FulfillmentStatusGroup == item).Select(x => x.FulfillmentStatusGroup).FirstOrDefault(),
                    DateTime = data.Where(x => x.FulfillmentStatusGroup == item).Select(x => x.PostedOn).FirstOrDefault(),
                    Status = data.Where(x => x.FulfillmentStatusGroup == item).Select(y => new CourierOrderStatusNameViewModel
                    {
                        StatusId = y.StatusId,
                        StatusNameEng = y.StatusNameEng,
                        StatusNameBng = y.StatusNameBng,
                        MessageFormat = y.Message,
                        EmailFormat = y.Email,
                        CustomerMessageFormat = y.CustomerMessage,
                        CustomerEmailFormat = y.CustomerEmail,
                        RetentionMessageFormat = y.RetentionMessage,
                        DateTime = y.PostedOn

                    })
                });
            }
            return reponse;
        }

        public async Task<IEnumerable<CourierOrderStatus>> LoadCourierStatus()
        {
            var data = await _sqlServerContext.CourierOrderStatus.Where(h => h.IsActive == true).ToListAsync();
            data.ToList().ForEach(i => i.StatusNameEng = i.StatusNameEng + "-" + i.StatusId);
            return data;

        }

        public async Task<IEnumerable<StatusGroup>> GetStatusGroup()
        {
            try
            {
                return await _sqlServerContext.StatusGroup.AsNoTracking().OrderBy(x => x.DashboardViewOrderBy).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StatusGroup> UpdateStatusGroup(int id, StatusGroup statusGroup)
        {
            var entity = await _sqlServerContext.StatusGroup.FirstOrDefaultAsync(item => item.StatusGroupId == id);

            string reportStatusGroup = entity.ReportStatusGroup;
            string fulfillmentStatusGroup = entity.FulfillmentStatusGroup;
            string orderTrackStatusGroup = entity.OrderTrackStatusGroup;
            string orderTrackStatusPublicGroup = entity.OrderTrackStatusPublicGroup;
            string dashboardStatusGroup = entity.DashboardStatusGroup;


            if (entity != null)
            {
                if (entity.ReportStatusGroup != statusGroup.ReportStatusGroup)
                {
                    entity.ReportStatusGroup = statusGroup.ReportStatusGroup;

                    var reportStatus = await _sqlServerContext.CourierOrderStatus
                    .Where(item => item.StatusGroup.Trim() == reportStatusGroup.Trim()).ToListAsync();

                    foreach (var d in reportStatus)
                    {
                        d.StatusGroup = statusGroup.ReportStatusGroup;
                    }
                }
                if (entity.FulfillmentStatusGroup != statusGroup.FulfillmentStatusGroup)
                {
                    entity.FulfillmentStatusGroup = statusGroup.FulfillmentStatusGroup;

                    var fulfillmentStatus = await _sqlServerContext.CourierOrderStatus
                    .Where(item => item.FulfillmentStatusGroup.Trim() == fulfillmentStatusGroup.Trim()).ToListAsync();

                    foreach (var d in fulfillmentStatus)
                    {
                        d.FulfillmentStatusGroup = statusGroup.FulfillmentStatusGroup;
                    }
                }
                if (entity.OrderTrackStatusGroup != statusGroup.OrderTrackStatusGroup)
                {
                    entity.OrderTrackStatusGroup = statusGroup.OrderTrackStatusGroup;

                    var orderTrackStatus = await _sqlServerContext.CourierOrderStatus
                    .Where(item => item.OrderTrackStatusGroup.Trim() == orderTrackStatusGroup.Trim()).ToListAsync();

                    foreach (var d in orderTrackStatus)
                    {
                        d.OrderTrackStatusGroup = statusGroup.OrderTrackStatusGroup;
                    }
                }

                if (entity.OrderTrackStatusPublicGroup != statusGroup.OrderTrackStatusPublicGroup)
                {
                    entity.OrderTrackStatusPublicGroup = statusGroup.OrderTrackStatusPublicGroup;

                    var orderTrackPublicStatus = await _sqlServerContext.CourierOrderStatus
                    .Where(item => item.OrderTrackStatusPublicGroup.Trim() == orderTrackStatusPublicGroup.Trim()).ToListAsync();

                    foreach (var d in orderTrackPublicStatus)
                    {
                        d.OrderTrackStatusPublicGroup = statusGroup.OrderTrackStatusPublicGroup;
                    }
                }

                if (entity.DashboardStatusGroup != statusGroup.DashboardStatusGroup)
                {
                    entity.DashboardStatusGroup = statusGroup.DashboardStatusGroup;

                    var dashboardStatus = await _sqlServerContext.CourierOrderStatus
                    .Where(item => item.DashboardStatusGroup.Trim() == dashboardStatusGroup.Trim()).ToListAsync();

                    foreach (var d in dashboardStatus)
                    {
                        d.DashboardStatusGroup = statusGroup.DashboardStatusGroup;
                    }
                }

                // Update entity in DbSet
                _sqlServerContext.StatusGroup.Update(entity);

                // Save changes in database
                //await _sqlServerContext.SaveChangesAsync();
            }

            await _sqlServerContext.SaveChangesAsync();

            return entity;
        }

        public async Task<CourierOrderStatus> UpdateCourierOrderStatus(int id, CourierOrderStatus courierOrderStatus)
        {
            var entity = await _sqlServerContext.CourierOrderStatus.FirstOrDefaultAsync(item => item.StatusId == id);
            if (entity != null)
            {
                if (entity.StatusNameBng != courierOrderStatus.StatusNameBng)
                {
                    entity.StatusNameBng = courierOrderStatus.StatusNameBng;
                }
                if (entity.StatusNameEng != courierOrderStatus.StatusNameEng)
                {
                    entity.StatusNameEng = courierOrderStatus.StatusNameEng;
                }
                if (entity.StatusType != courierOrderStatus.StatusType)
                {
                    entity.StatusType = courierOrderStatus.StatusType;
                }
                if (entity.StatusGroup != courierOrderStatus.StatusGroup)
                {
                    entity.StatusGroup = courierOrderStatus.StatusGroup;
                }
                if (entity.FulfillmentStatusGroup != courierOrderStatus.FulfillmentStatusGroup)
                {
                    entity.FulfillmentStatusGroup = courierOrderStatus.FulfillmentStatusGroup;
                }
                if (entity.OrderTrackStatusGroup != courierOrderStatus.OrderTrackStatusGroup)
                {
                    entity.OrderTrackStatusGroup = courierOrderStatus.OrderTrackStatusGroup;
                }

                if (entity.OrderTrackStatusPublicGroup != courierOrderStatus.OrderTrackStatusPublicGroup)
                {
                    entity.OrderTrackStatusPublicGroup = courierOrderStatus.OrderTrackStatusPublicGroup;
                }

                if (entity.DashboardStatusGroup != courierOrderStatus.DashboardStatusGroup)
                {
                    entity.DashboardStatusGroup = courierOrderStatus.DashboardStatusGroup;
                }

                // Update entity in DbSet
                _sqlServerContext.CourierOrderStatus.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<IEnumerable<CourierOrderTrackHistoryViewModel>> OrderUpdateHistory(string courierOrderId)
        {
            var response = await (from history in _sqlServerContext.CourierOrderStatusHistory
                                  join status in _sqlServerContext.CourierOrderStatus
                                  on history.Status equals status.StatusId
                                  join _orders in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on history.CourierOrderId equals _orders.CourierOrdersId
                                  join district in _sqlServerContext.Districts.AsNoTracking()
                                  on _orders.DistrictId equals district.DistrictId into districts
                                  from _districts in districts.DefaultIfEmpty()
                                      //left join
                                      //join courierUser in _sqlServerContext.CourierUsers on history.PostedBy equals courierUser.CourierUserId into ordertracking
                                      //from courierUser in ordertracking.DefaultIfEmpty()
                                      ////left join
                                      //join adUser in _sqlServerContext.Users on history.PostedBy equals adUser.UserId into orderTrackingDetails
                                      //from adUser in orderTrackingDetails.DefaultIfEmpty()
                                      ////left join
                                      //join adCollector in _sqlServerContext.Collectors on history.PostedBy equals adCollector.CollectorId into orderTrackingInfo
                                      //from adCollector in orderTrackingInfo.DefaultIfEmpty()

                                  where history.CourierOrderId.ToLower().Trim() == courierOrderId.ToLower().Trim()
                                  orderby history.Id descending

                                  select new CourierOrderTrackHistoryViewModel
                                  {
                                      StatusNameBng = status.StatusNameBng,
                                      StatusNameEng = status.StatusNameEng,
                                      CourierOrderId = history.CourierOrderId,
                                      IsConfirmedBy = history.IsConfirmedBy,
                                      OrderDate = history.OrderDate.ToString("dd-MM-yyyy HH:mm:ss"),
                                      Status = history.Status,
                                      PostedOn = history.PostedOn.ToString("dd-MM-yyyy HH:mm:ss"),
                                      //NamePostedBy = history.IsConfirmedBy == "admin" ? adUser.FullName : history.IsConfirmedBy == "collector"? adCollector.CollectorName: courierUser.UserName,
                                      Comment = history.Comment,
                                      PodNumber = history.PodNumber,
                                      CourierId = history.CourierId,
                                      HubName = history.HubName,
                                      CourierDeliveryManName = history.CourierDeliveryManName,
                                      CourierDeliveryManMobile = history.CourierDeliveryManMobile,
                                      DistrictsViewModel = new DistrictsViewModel
                                      {
                                          EdeshMobileNo = _districts.EDeshMobileNo == null ? "" : _districts.EDeshMobileNo
                                      },
                                      NamePostedBy =
                                                (
                                                    history.IsConfirmedBy.ToLower() == "admin" ? _sqlServerContext.Users.AsNoTracking().Where(x => x.UserId.Equals(history.PostedBy)).Select(x => x.FullName).FirstOrDefault() :
                                                    history.IsConfirmedBy.ToLower() == "merchant" ? _sqlServerContext.CourierUsers.AsNoTracking().Where(x => x.CourierUserId.Equals(history.PostedBy)).Select(x => x.CompanyName + "(" + x.UserName + ")").FirstOrDefault() :
                                                    history.IsConfirmedBy.ToLower() == "account" ? _sqlServerContext.Users.AsNoTracking().Where(x => x.UserId.Equals(history.PostedBy)).Select(x => x.FullName).FirstOrDefault() :
                                                    history.IsConfirmedBy.ToLower() == "collector" ? _sqlServerContext.Collectors.AsNoTracking().Where(x => x.CollectorId.Equals(history.PostedBy)).Select(x => x.CollectorName).FirstOrDefault() :
                                                    history.IsConfirmedBy.ToLower() == "courier" ? _sqlServerContext.Couriers.AsNoTracking().Where(x => x.CourierId.Equals(history.PostedBy)).Select(x => x.CourierName).FirstOrDefault() :
                                                    history.IsConfirmedBy.ToLower() == "deliveryman" ? _sqlServerContext.DeliveryUsers.AsNoTracking().Where(x => x.Id.Equals(history.PostedBy)).Select(x => x.Name).FirstOrDefault() :
                                                    history.IsConfirmedBy
                                                )
                                  }
                            ).ToListAsync();
            return response;
        }

        public async Task<IEnumerable<Collectors>> GetAllCollectors()
        {
            return await _sqlServerContext.Collectors.Where(c => c.IsActive.Equals(true)).ToListAsync();
        }

        public async Task<IEnumerable<DeliveryUsers>> GetAllDeliveryMan()
        {
            return await _sqlServerContext.DeliveryUsers.Where(x => x.IsActive.Equals(1) && x.Name != "").OrderBy(x => x.Name).ToListAsync();
        }
        public async Task<IEnumerable<DeliveryUsers>> GetLocationAssignDeliveryMan()
        {
            //return await _sqlServerContext.DeliveryUsers.Where(x => x.IsActive.Equals(1) && x.Name != "").OrderBy(x => x.Name).ToListAsync();
            //IQueryable<DeliveryUsers> data = (from m in _sqlServerContext.LocationAssign.AsNoTracking()
            //                                            join c in _sqlServerContext.DeliveryUsers on  m.DeliveryUserId equals c.Id
            //                                            select new DeliveryUsers
            //                                            {
            //                                                Id = c.Id,
            //                                                Name = c.Name,
            //                                                Mobile = c.Mobile,
            //                                                IsActive = c.IsActive,
            //                                                IsNowOffline = c.IsNowOffline,
            //                                                Latitude = c == null ? "" : c.Latitude,
            //                                                Longitude = c == null ? "" : c.Longitude,
            //                                                UpdatedOn = c.UpdatedOn,
            //                                                IsPermanentRider = c.IsPermanentRider,
            //                                                RiderType = c == null ? "" : c.RiderType
            //                                            }).Distinct(); 
            IQueryable<DeliveryUsers> data = (from c in _sqlServerContext.DeliveryUsers.AsNoTracking()
                                              where c.Name != ""
                                              orderby c.IsNowOffline, c.IsActive descending
                                              select new DeliveryUsers
                                              {
                                                  Id = c.Id,
                                                  Name = c.Name,
                                                  Mobile = c.Mobile,
                                                  IsActive = c.IsActive,
                                                  IsNowOffline = c.IsNowOffline,
                                                  Latitude = c == null ? "" : c.Latitude,
                                                  Longitude = c == null ? "" : c.Longitude,
                                                  UpdatedOn = c.UpdatedOn,
                                                  IsPermanentRider = c.IsPermanentRider,
                                                  RiderType = c == null ? "" : c.RiderType,
                                                  HubName = c.HubName
                                              }).Distinct();


            return await data.ToListAsync();
        }
        public async Task<List<LocationAssign>> AddMultipleLocationAssign(List<LocationAssign> locationAssign)
        {
            await _sqlServerContext.LocationAssign.AddRangeAsync(locationAssign);
            await _sqlServerContext.SaveChangesAsync();
            return locationAssign;
        }

        public async Task<List<LocationAssignHistory>> AddMultipleLocationAssignHistory(List<LocationAssignHistory> locationAssignHistory)
        {
            await _sqlServerContext.LocationAssignHistory.AddRangeAsync(locationAssignHistory);
            await _sqlServerContext.SaveChangesAsync();
            return locationAssignHistory;
        }

        public async Task<List<CollectorAssign>> AddMultipleCollectorAssign(List<CollectorAssign> collectorAssign)
        {
            await _sqlServerContext.CollectorAssign.AddRangeAsync(collectorAssign);
            await _sqlServerContext.SaveChangesAsync();
            return collectorAssign;
        }
        public async Task<CollectorAssign> AddCollectorAssign(CollectorAssign collectorAssign)
        {
            await _sqlServerContext.CollectorAssign.AddAsync(collectorAssign);
            await _sqlServerContext.SaveChangesAsync();
            return collectorAssign;
        }

        public async Task<DeliveryBonduAssign> AddDeliveryManAssign(DeliveryBonduAssign deliveryBonduAssign)
        {
            await _sqlServerContext.DeliveryBonduAssign.AddAsync(deliveryBonduAssign);
            await _sqlServerContext.SaveChangesAsync();
            return deliveryBonduAssign;
        }

        public async Task<List<DeliveryBonduAssign>> AddDeliveryBonduAssignMultiple(List<DeliveryBonduAssign> deliveryBonduAssign)
        {

            var entryOrderIdList = new List<DeliveryBonduAssign>();

            string assignType = (from d in deliveryBonduAssign
                                 select d.AssignType).FirstOrDefault();

            if (assignType == "")
            {

                var merchantTocustomerData = (from order in _sqlServerContext.CourierOrders.AsNoTracking()
                                              join zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                              on
                                                 new { order.DistrictId, order.ThanaId, order.AreaId }
                                              equals
                                                 new { zoneLocation.DistrictId, zoneLocation.ThanaId, zoneLocation.AreaId }
                                              join assign in deliveryBonduAssign
                                              on order.Id equals assign.OrderId
                                              where zoneLocation.IsActive.Equals(1)
                                              select assign).ToList();

                merchantTocustomerData.ToList().ForEach(i => i.AssignType = "merchantTocustomer");

                if (merchantTocustomerData.Count() > 0)
                {
                    var orderIds = (from d in merchantTocustomerData
                                    select d.OrderId).ToArray();

                    assignType = (from d in merchantTocustomerData
                                  select d.AssignType).FirstOrDefault();

                    var existOrderIds = _sqlServerContext.DeliveryBonduAssign.AsNoTracking()
                        .Where(x => orderIds.Contains(x.OrderId) && x.AssignType.Equals(assignType)).Select(x => x.OrderId).ToList();


                    var entryOrderIds =
                        (from ba in merchantTocustomerData
                         where !(from o in existOrderIds
                                 select o)
                            .Contains(ba.OrderId)
                         select ba).ToList();

                    if (entryOrderIds.Count() > 0)
                    {
                        await _sqlServerContext.DeliveryBonduAssign.AddRangeAsync(entryOrderIds);
                        await _sqlServerContext.SaveChangesAsync();
                        entryOrderIdList.AddRange(entryOrderIds);
                    }
                }


                var merchantTohubData =
                    (from ba in deliveryBonduAssign
                     where !(from o in merchantTocustomerData
                             select o.OrderId)
                        .Contains(ba.OrderId)
                     select ba).ToList();

                merchantTohubData.ToList().ForEach(i => i.AssignType = "merchantTohub");

                if (merchantTohubData.Count() > 0)
                {
                    var orderIds = (from d in merchantTohubData
                                    select d.OrderId).ToArray();

                    assignType = (from d in merchantTohubData
                                  select d.AssignType).FirstOrDefault();

                    var existOrderIds = _sqlServerContext.DeliveryBonduAssign.AsNoTracking()
                        .Where(x => orderIds.Contains(x.OrderId) && x.AssignType.Equals(assignType)).Select(x => x.OrderId).ToList();


                    var entryOrderIds =
                        (from ba in merchantTohubData
                         where !(from o in existOrderIds
                                 select o)
                            .Contains(ba.OrderId)
                         select ba).ToList();

                    if (entryOrderIds.Count() > 0)
                    {
                        await _sqlServerContext.DeliveryBonduAssign.AddRangeAsync(entryOrderIds);
                        await _sqlServerContext.SaveChangesAsync();
                        entryOrderIdList.AddRange(entryOrderIds);
                    }
                }

                return entryOrderIdList;
            }
            else
            {


                var orderIds = (from d in deliveryBonduAssign
                                select d.OrderId).ToArray();

                var existOrderIds = _sqlServerContext.DeliveryBonduAssign.AsNoTracking()
                    .Where(x => orderIds.Contains(x.OrderId) && x.AssignType.Equals(assignType)).Select(x => x.OrderId).ToList();

                var entryOrderIds =
                    (from ba in deliveryBonduAssign
                     where !(from o in existOrderIds
                             select o)
                        .Contains(ba.OrderId)
                     select ba).ToList();

                if (entryOrderIds.Count() > 0)
                {
                    await _sqlServerContext.DeliveryBonduAssign.AddRangeAsync(entryOrderIds);
                    await _sqlServerContext.SaveChangesAsync();
                    return entryOrderIds;
                }
            }
            return new List<DeliveryBonduAssign>();
        }

        public async Task<CollectorAssign> UpdateCollectorAssign(int id, CollectorAssign collectorAssign)
        {
            var entity = await _sqlServerContext.CollectorAssign.FirstOrDefaultAsync(item => item.CollectorAssignId == id);
            if (entity != null)
            {
                entity.CourierUserId = collectorAssign.CourierUserId;
                entity.CollectorId = collectorAssign.CollectorId;
                entity.AssignType = collectorAssign.AssignType;
                entity.UpdatedBy = collectorAssign.UpdatedBy;
                entity.UpdatedOn = collectorAssign.UpdatedOn;

                // Update entity in DbSet
                _sqlServerContext.CollectorAssign.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<int> UpdateMultipleCollectorAssignForLocation(MultipleCollectorAssign multipleCollectorAssign)
        {
            var entity = await _sqlServerContext.CollectorAssign.AsNoTracking()
                .Where(x => multipleCollectorAssign.CollectorAssignId.Contains(x.CollectorAssignId))
                .BatchUpdateAsync(x => new CollectorAssign
                {
                    CollectorId = multipleCollectorAssign.CollectorId,
                    UpdatedBy = multipleCollectorAssign.UpdatedBy
                });
            return entity;
        }

        public async Task<CollectorAssign> UpdateCollectorAssignForLocation(int id, CollectorAssign collectorAssign)
        {
            var entity = await _sqlServerContext.CollectorAssign.FirstOrDefaultAsync(item => item.CollectorAssignId == id);
            if (entity != null)
            {
                entity.CollectorId = collectorAssign.CollectorId;
                entity.UpdatedBy = collectorAssign.UpdatedBy;
                entity.UpdatedOn = collectorAssign.UpdatedOn;

                // Update entity in DbSet
                _sqlServerContext.CollectorAssign.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<List<dynamic>> GetAllLocationAssign()
        {
            //IQueryable<dynamic> res = _sqlServerContext.LocationAssign.AsNoTracking()
            //    .GroupBy(g => g.DistrictId)
            //    .Select(x => new
            //    {
            //        x.FirstOrDefault().DistrictId,
            //        x.FirstOrDefault().CollectorId,
            //        x.FirstOrDefault().DeliveryUserId,
            //        _sqlServerContext.Districts.Where(z => z.DistrictId.Equals(x.FirstOrDefault().DistrictId)).FirstOrDefault().District,
            //        thana = _sqlServerContext.Districts.Where(s => (x.Select(a => a.ThanaId)).Contains(s.DistrictId)).Select(e => e),
            //        area = _sqlServerContext.Districts.Where(s => (x.Select(a => a.AreaId)).Contains(s.DistrictId)).Select(e => e),

            //        thanas = _sqlServerContext.Districts.Where(s => s.ParentId.Equals(x.FirstOrDefault().DistrictId)).Select(e => e),
            //        areas = _sqlServerContext.Districts.Where(s => (x.Select(a => a.AreaId)).Contains(s.DistrictId)).Select(e => e)
            //    });


            IQueryable<dynamic> res = _sqlServerContext.LocationAssign.AsNoTracking()
                .Select(x => new
                {
                    IsNowOffline = _sqlServerContext.DeliveryUsers.Where(d => d.Id.Equals(x.DeliveryUserId)).Select(e => e.IsNowOffline).FirstOrDefault(),
                    isTemporary = _sqlServerContext.Collectors.Where(d => d.CollectorId.Equals(x.CollectorId)).Select(e => e.IsTemporary).FirstOrDefault(),
                    x.DtDefaultAssign,
                    x.AdDefaultAssign,
                    x.LocationAssignId,
                    x.CollectorId,
                    x.DeliveryUserId,
                    x.ZoneId,
                    x.DistrictId,
                    district = _sqlServerContext.Districts.Where(d => d.DistrictId.Equals(x.DistrictId)).Select(e => e.District).FirstOrDefault(),
                    x.ThanaId,
                    x.AreaId,
                    thana = _sqlServerContext.Districts.Where(c => c.ParentId.Equals(x.DistrictId)).Select(e => new
                    {
                        e.AreaType,
                        e.District,
                        DistrictBng = e.DistrictBng + "-" + e.District,
                        e.DistrictId,
                        e.IsActive,
                        e.ParentId,
                        e.PostalCode
                    }),
                    area = _sqlServerContext.Districts.Where(c => c.ParentId.Equals(x.ThanaId)).Select(e => new
                    {
                        e.AreaType,
                        e.District,
                        DistrictBng = e.DistrictBng + "-" + e.District,
                        e.DistrictId,
                        e.IsActive,
                        e.ParentId,
                        e.PostalCode
                    })
                });


            return await res.ToListAsync();
        }
        public async Task<IEnumerable<dynamic>> GetAllCollectorsLocationAssign()
        {

            //IQueryable<dynamic> res = _sqlServerContext.Collectors.AsNoTracking()
            //        .Join(_sqlServerContext.CollectorAssign.AsNoTracking(), collector => collector.CollectorId, ca => ca.CollectorId, (collector, ca) => new { collector, ca })
            //        .Join(_sqlServerContext.Districts.AsNoTracking(), dca => dca.ca.ThanaId, d => d.DistrictId, (dca, d) => new { dca, d })
            //        .Where(m => m.dca.ca.DistrictId > 0)
            //        .Select(m => new
            //        {
            //            CollectorId = m.dca.collector.CollectorId,
            //            CollectorName = m.dca.collector.CollectorName,
            //            AssignType = m.dca.ca.AssignType,
            //            DistrictId = m.d.DistrictId,
            //            District = m.d.District,
            //            DistrictBng = m.d.DistrictBng
            //        }).GroupBy(g => g.CollectorId)
            //        .Select(x=> new {
            //            x.FirstOrDefault().CollectorId,
            //            x.FirstOrDefault().CollectorName,
            //            x.FirstOrDefault().AssignType,
            //            districtName = "Dhaka",
            //            districtId = 14,
            //            thanas = x.Select(n => new
            //            {
            //                n.District,
            //                n.DistrictBng,
            //                n.DistrictId
            //            })
            //        });


            IQueryable<dynamic> res = _sqlServerContext.Collectors.AsNoTracking()
                    .Join(_sqlServerContext.CollectorAssign.AsNoTracking(), collector => collector.CollectorId, ca => ca.CollectorId, (collector, ca) => new { collector, ca })
                    .Join(_sqlServerContext.Districts.AsNoTracking(), dca => dca.ca.ThanaId, d => d.DistrictId, (dca, d) => new { dca, d })
                    .Where(m => m.dca.ca.DistrictId > 0 && m.dca.collector.IsActive.Equals(true))
                    .Select(m => new
                    {
                        CollectorAssignId = m.dca.ca.CollectorAssignId,
                        CollectorId = m.dca.collector.CollectorId,
                        CollectorName = m.dca.collector.CollectorName,
                        AssignType = m.dca.ca.AssignType,
                        DistrictId = m.d.DistrictId,
                        District = m.d.District,
                        DistrictBng = m.d.DistrictBng,
                        IsTemporary = m.dca.collector.IsTemporary
                    });
            return await res.ToListAsync();

        }
        public async Task<IEnumerable<CollectorAssignViewModel>> GetAllCollectorsAssign()
        {
            IQueryable<CollectorAssignViewModel> data = from c in _sqlServerContext.CollectorAssign.AsNoTracking()
                                                        join m in _sqlServerContext.CourierUsers on c.CourierUserId equals m.CourierUserId
                                                        join co in _sqlServerContext.Collectors on c.CollectorId equals co.CollectorId
                                                        select new CollectorAssignViewModel
                                                        {
                                                            CollectorAssignId = c.CollectorAssignId,
                                                            CourierUserId = m.CourierUserId,
                                                            CompanyName = m.CompanyName,
                                                            CourierUserName = m.UserName,
                                                            CourierUserMobile = m.Mobile,
                                                            CollectorId = co.CollectorId,
                                                            CollectorName = co.CollectorName,
                                                            AssignType = c.AssignType
                                                        };
            return await data.ToListAsync();
        }

        public async Task<IEnumerable<DeliveryZoneInfo>> GetDeliveryZoneInfo()
        {
            IQueryable<DeliveryZoneInfo> data = from c in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                                join dc in _sqlServerContext.Districts on c.ThanaId equals dc.DistrictId into pc
                                                from thana in pc.DefaultIfEmpty()
                                                join dd in _sqlServerContext.Districts on c.DistrictId equals dd.DistrictId into pd
                                                from dist in pd.DefaultIfEmpty()
                                                join da in _sqlServerContext.Districts on c.AreaId equals da.DistrictId into pa
                                                from area in pa.DefaultIfEmpty()
                                                join z in _sqlServerContext.DeliveryZone on c.ZoneId equals z.ZoneId into pz
                                                from zone in pz.DefaultIfEmpty()
                                                select new DeliveryZoneInfo
                                                {
                                                    ZoneId = c.ZoneId,
                                                    ZoneName = zone.ZoneName,
                                                    DistrictId = dist == null ? 0 : dist.DistrictId,
                                                    District = dist == null ? "" : dist.District,
                                                    DistrictBng = dist == null ? "" : dist.DistrictBng,
                                                    ThanaId = thana == null ? 0 : thana.DistrictId,
                                                    Thana = thana == null ? "" : thana.District,
                                                    ThanaBng = thana == null ? "" : thana.DistrictBng,
                                                    AreaId = area == null ? 0 : area.DistrictId,
                                                    Area = area == null ? "" : area.District,
                                                    AreaBng = area == null ? "" : area.DistrictBng,
                                                    ParentId = area == null ? 0 : area.ParentId

                                                };

            return await data.ToListAsync();

        }
        public async Task<IEnumerable<DeliveryZone>> GetDeliveryZone()
        {
            IQueryable<DeliveryZone> data = from c in _sqlServerContext.DeliveryZone.AsNoTracking()
                                            select c;

            //new DeliveryZone
            //{
            //    ZoneId = c.ZoneId,
            //    ZoneName = c.ZoneName

            //};

            return await data.ToListAsync();

        }

        public async Task<IEnumerable<DeliveryManAssignViewModel>> GetAllDeliveryMansAssign()
        {
            IQueryable<DeliveryManAssignViewModel> data = from c in _sqlServerContext.DeliveryBonduAssign.AsNoTracking()
                                                          join m in _sqlServerContext.CourierUsers.AsNoTracking() on c.CourierUserId equals m.CourierUserId
                                                          join co in _sqlServerContext.DeliveryUsers.AsNoTracking() on c.DeliveryManUserId equals co.Id
                                                          select new DeliveryManAssignViewModel
                                                          {
                                                              Id = c.Id,
                                                              CourierUserId = m.CourierUserId,
                                                              CompanyName = m.CompanyName,
                                                              CourierUserName = m.UserName,
                                                              CourierUserMobile = m.Mobile,
                                                              DeliveryManUserId = co.Id,
                                                              Name = co.Name,
                                                              AssignType = c.AssignType
                                                          };
            return await data.ToListAsync();
        }
        public async Task<CourierUsers> UpdateMerchantInformation(int id, CourierUsers courierUserInfo)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == id);
            //var pickUpEntity = await _sqlServerContext.PickupLocations.FirstOrDefaultAsync(item => item.CourierUserId == id);
            if (entity != null)
            {
                if (courierUserInfo.UserName != "")
                {
                    //entity.UserName = Regex.Replace(courierUserInfo.UserName, @"[^0-9a-zA-Z]+", " ").Trim();
                    entity.UserName = SpecialCharacters.RemoveSpecialCharacters(courierUserInfo.UserName);
                }
                if (courierUserInfo.Mobile != "")
                {
                    entity.Mobile = courierUserInfo.Mobile;
                }
                if (courierUserInfo.Address != "")
                {
                    //entity.Address = Regex.Replace(courierUserInfo.Address, @"[^0-9a-zA-Z,]+", " ").Trim();
                    entity.Address = SpecialCharacters.RemoveSpecialCharacters(courierUserInfo.Address);
                }

                if (courierUserInfo.EmailAddress != "")
                {
                    entity.EmailAddress = courierUserInfo.EmailAddress;
                }
                if (courierUserInfo.BkashNumber != "")
                {
                    //entity.BkashNumber = Regex.Replace(courierUserInfo.BkashNumber, @"[^0-9]+", " ").Trim();
                    entity.BkashNumber = SpecialCharacters.RemoveSpecialCharacters(courierUserInfo.BkashNumber);
                }
                if (courierUserInfo.AlterMobile != "")
                {
                    //entity.AlterMobile = Regex.Replace(courierUserInfo.AlterMobile, @"[^0-9]+", " ").Trim();
                    entity.AlterMobile = SpecialCharacters.RemoveSpecialCharacters(courierUserInfo.AlterMobile);
                }
                entity.FBURL = courierUserInfo.FBURL;
                entity.WebURL = courierUserInfo.WebURL;

                entity.IsSms = courierUserInfo.IsSms;
                entity.IsEmail = courierUserInfo.IsEmail;
                entity.MaxCodCharge = courierUserInfo.MaxCodCharge;
                entity.CollectionCharge = courierUserInfo.CollectionCharge;
                entity.Credit = courierUserInfo.Credit;
                //if (courierUserInfo.IsSms != false)
                //{
                //    entity.IsSms = courierUserInfo.IsSms;
                //}
                //if (courierUserInfo.IsEmail != false)
                //{
                //    entity.IsEmail = courierUserInfo.IsEmail;
                //}
                if (courierUserInfo.SourceType != "")
                {
                    entity.SourceType = courierUserInfo.SourceType;
                }
                if (courierUserInfo.RetentionUserId != 0)
                {
                    entity.RetentionUserId = courierUserInfo.RetentionUserId;
                }
                if (courierUserInfo.AcquisitionUserId != 0)
                {
                    entity.AcquisitionUserId = courierUserInfo.AcquisitionUserId;
                }
                if (courierUserInfo.Remarks != "")
                {
                    //entity.Remarks = Regex.Replace(courierUserInfo.Remarks, @"[^0-9a-zA-Z,]+", " ").Trim();
                    entity.Remarks = SpecialCharacters.RemoveSpecialCharacters(courierUserInfo.Remarks);
                }
                if (courierUserInfo.CompanyName != "")
                {
                    entity.CompanyName = SpecialCharacters.RemoveSpecialCharacters(courierUserInfo.CompanyName);
                    //entity.CompanyName = Regex.Replace(courierUserInfo.CompanyName, @"[^0-9a-zA-Z,]+", " ").Trim();
                }
                //if (courierUserInfo.IsDocument != false)
                //{
                entity.IsDocument = courierUserInfo.IsDocument;
                entity.IsAutoProcess = courierUserInfo.IsAutoProcess;
                entity.IsQuickOrderActive = courierUserInfo.IsQuickOrderActive;
                entity.IsOfferActive = courierUserInfo.IsOfferActive;
                entity.OfferType = courierUserInfo.OfferType;
                entity.OfferCodDiscount = courierUserInfo.OfferCodDiscount;
                entity.OfferBkashDiscountDhaka = courierUserInfo.OfferBkashDiscountDhaka;
                entity.OfferBkashDiscountOutSideDhaka = courierUserInfo.OfferBkashDiscountOutSideDhaka;
                //}
                entity.IsBreakAble = courierUserInfo.IsBreakAble;
                entity.IsHeavyWeight = courierUserInfo.IsHeavyWeight;
                entity.MerchantAssignActive = courierUserInfo.MerchantAssignActive;

                if (courierUserInfo.KnowingSource != "")
                {
                    entity.KnowingSource = courierUserInfo.KnowingSource;
                }
                if (courierUserInfo.Priority != "")
                {
                    entity.Priority = courierUserInfo.Priority;
                }


                if (courierUserInfo.DistrictId != 0)
                {
                    entity.DistrictId = courierUserInfo.DistrictId;
                    entity.ThanaId = courierUserInfo.ThanaId;
                }
                if (courierUserInfo.CategoryId != 0)
                {
                    entity.CategoryId = courierUserInfo.CategoryId;
                }
                if (courierUserInfo.SubCategoryId != 0)
                {
                    entity.SubCategoryId = courierUserInfo.SubCategoryId;
                }

                if (courierUserInfo.PaymentServiceType != null)
                {
                    entity.PaymentServiceType = courierUserInfo.PaymentServiceType;
                }
                if (courierUserInfo.PaymentServiceCharge != null)
                {
                    entity.PaymentServiceCharge = courierUserInfo.PaymentServiceCharge;
                }
                if (courierUserInfo.CollectionAmountLimt != null)
                {
                    entity.CollectionAmountLimt = courierUserInfo.CollectionAmountLimt;
                }

                if (courierUserInfo.Verify != null)
                {
                    entity.Verify = courierUserInfo.Verify;
                }


                entity.CodChargeDhaka = courierUserInfo.CodChargeDhaka;
                entity.CodChargeOutsideDhaka = courierUserInfo.CodChargeOutsideDhaka;
                entity.CodChargePercentageDhaka = courierUserInfo.CodChargePercentageDhaka;
                entity.CodChargePercentageOutsideDhaka = courierUserInfo.CodChargePercentageOutsideDhaka;
                entity.CodChargeTypeFlag = courierUserInfo.CodChargeTypeFlag;
                entity.CodChargeTypeOutsideFlag = courierUserInfo.CodChargeTypeOutsideFlag;
                entity.FirstCollectionCharge = courierUserInfo.FirstCollectionCharge;
                entity.AutoDownload = courierUserInfo.AutoDownload;
                entity.IsInstaCod = courierUserInfo.IsInstaCod;
                entity.IsDana = courierUserInfo.IsDana;

                // Update entity in DbSet
                _sqlServerContext.CourierUsers.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<CourierUsers> CustomerVoiceSmsLimit(int courierUserId, int customerVoiceSmsLimit)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == courierUserId);

            if (entity != null)
            {
                if (entity.CustomerVoiceSmsLimit > 0)
                {
                    entity.CustomerVoiceSmsLimit = entity.CustomerVoiceSmsLimit - customerVoiceSmsLimit;
                    _sqlServerContext.CourierUsers.Update(entity);

                    await _sqlServerContext.SaveChangesAsync();
                }


            }
            return entity;
        }
        public async Task<CourierUsers> UpdateCustomerSMSLimit(int courierUserId, int customerSMSLimit)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == courierUserId);

            if (entity != null)
            {
                if (entity.CustomerSMSLimit > 0)
                {
                    entity.CustomerSMSLimit = entity.CustomerSMSLimit - customerSMSLimit;
                    _sqlServerContext.CourierUsers.Update(entity);

                    await _sqlServerContext.SaveChangesAsync();
                }


            }
            return entity;
        }

        public async Task<CourierUsers> UpdateViberSMSLimit(int courierUserId, int viberSMSLimit)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == courierUserId);

            if (entity != null)
            {
                if (entity.ViberSMSLimit > 0)
                {
                    entity.ViberSMSLimit = entity.ViberSMSLimit - viberSMSLimit;
                    _sqlServerContext.CourierUsers.Update(entity);

                    await _sqlServerContext.SaveChangesAsync();
                }


            }
            return entity;
        }

        public async Task<int> DeleteCollectorAssign(int id)
        {
            var entity = await _sqlServerContext.CollectorAssign.FirstOrDefaultAsync(item => item.CollectorAssignId == id);

            _sqlServerContext.CollectorAssign.Remove(entity);
            return await _sqlServerContext.SaveChangesAsync();
        }

        public async Task<int> DeletePickupLocations(int id)
        {

            // temporary solutions 
            int res = 0;
            var entity = await _sqlServerContext.PickupLocations.FirstOrDefaultAsync(item => item.Id == id);

            if (entity != null)
            {
                entity.IsActive = false;
                _sqlServerContext.PickupLocations.Update(entity);
                res = await _sqlServerContext.SaveChangesAsync();
                return res;
            }
            return res;


            //_sqlServerContext.PickupLocations.Remove(entity);
            //return await _sqlServerContext.SaveChangesAsync();
        }

        public async Task<CourierUsersViewModel> GetCourierUsersInformation(int courierUserId)
        {
            //return await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == courierUserId);


            IQueryable<CourierUsersViewModel> courierUsersList = from _courierUsers in _sqlServerContext.CourierUsers
                                                                     //join _users in _sqlServerContext.Users on _courierUsers.RetentionUserId equals _users.UserId
                                                                 join _usersTemp in _sqlServerContext.Users.AsNoTracking() on _courierUsers.RetentionUserId equals _usersTemp.UserId into _adminUsers
                                                                 from _users in _adminUsers.DefaultIfEmpty()
                                                                 where _courierUsers.CourierUserId == courierUserId
                                                                 select new CourierUsersViewModel
                                                                 {
                                                                     CourierUserId = _courierUsers.CourierUserId,
                                                                     UserName = _courierUsers.UserName,
                                                                     Password = _courierUsers.Password,
                                                                     Mobile = _courierUsers.Mobile,
                                                                     IsActive = _courierUsers.IsActive,
                                                                     Address = _courierUsers.Address,
                                                                     SmsCharge = _courierUsers.SmsCharge,
                                                                     MailCharge = _courierUsers.MailCharge,
                                                                     ReturnCharge = _courierUsers.ReturnCharge,
                                                                     CollectionCharge = _courierUsers.CollectionCharge,
                                                                     IsSms = _courierUsers.IsSms,
                                                                     IsEmail = _courierUsers.IsEmail,
                                                                     EmailAddress = _courierUsers.EmailAddress,
                                                                     BkashNumber = _courierUsers.BkashNumber,
                                                                     AlterMobile = _courierUsers.AlterMobile,
                                                                     SourceType = _courierUsers.SourceType,
                                                                     RetentionUserId = _courierUsers.RetentionUserId,
                                                                     AcquisitionUserId = _courierUsers.AcquisitionUserId,
                                                                     JoinDate = _courierUsers.JoinDate,
                                                                     IsDocument = _courierUsers.IsDocument,
                                                                     Remarks = _courierUsers.Remarks,
                                                                     CompanyName = _courierUsers.CompanyName,
                                                                     IsCustomerSms = _courierUsers.IsCustomerSms,
                                                                     IsCustomerEmail = _courierUsers.IsCustomerEmail,
                                                                     MaxCodCharge = _courierUsers.MaxCodCharge,
                                                                     Refreshtoken = _courierUsers.Refreshtoken,
                                                                     IsAutoProcess = _courierUsers.IsAutoProcess,
                                                                     FirebaseToken = _courierUsers.FirebaseToken,
                                                                     Credit = _courierUsers.Credit,
                                                                     FBURL = _courierUsers.FBURL,
                                                                     WebURL = _courierUsers.WebURL,
                                                                     IsOfferActive = _courierUsers.IsOfferActive,
                                                                     OfferCodDiscount = _courierUsers.OfferCodDiscount,
                                                                     OfferType = _courierUsers.OfferType,
                                                                     OfferBkashDiscountDhaka = _courierUsers.OfferBkashDiscountDhaka,
                                                                     OfferBkashDiscountOutSideDhaka = _courierUsers.OfferBkashDiscountOutSideDhaka,
                                                                     AdvancePayment = _courierUsers.AdvancePayment,
                                                                     KnowingSource = _courierUsers.KnowingSource,
                                                                     Priority = _courierUsers.Priority,
                                                                     IsInstaCod = _courierUsers.IsInstaCod,
                                                                     Referrer = _courierUsers.Referrer,
                                                                     ReferrerOrder = _courierUsers.ReferrerOrder,
                                                                     ReferrerStartTime = _courierUsers.ReferrerStartTime,
                                                                     ReferrerEndTime = _courierUsers.ReferrerEndTime,
                                                                     OrderType = _courierUsers.OrderType,
                                                                     RefereeOrder = _courierUsers.RefereeOrder,
                                                                     ReferrerIsActive = _courierUsers.ReferrerIsActive,
                                                                     RefereeStartTime = _courierUsers.RefereeStartTime,
                                                                     RefereeEndTime = _courierUsers.RefereeEndTime,
                                                                     PreferredPaymentCycle = _courierUsers.PreferredPaymentCycle,
                                                                     RegistrationFrom = _courierUsers.RegistrationFrom,
                                                                     IsBlock = _courierUsers.IsBlock,
                                                                     BlockReason = _courierUsers.BlockReason,
                                                                     WeightRangeId = _courierUsers.WeightRangeId,
                                                                     DeliveryRangeIdInside = _courierUsers.DeliveryRangeIdInside,
                                                                     DeliveryRangeIdIOutside = _courierUsers.DeliveryRangeIdIOutside,
                                                                     DistrictId = _courierUsers.DistrictId,
                                                                     ThanaId = _courierUsers.ThanaId,
                                                                     PreferredPaymentCycleDate = _courierUsers.PreferredPaymentCycleDate,
                                                                     IsQuickOrderActive = _courierUsers.IsQuickOrderActive,
                                                                     CustomerSMSLimit = _courierUsers.CustomerSMSLimit,
                                                                     CustomerVoiceSmsLimit = _courierUsers.CustomerVoiceSmsLimit,
                                                                     ViberSMSLimit = _courierUsers.ViberSMSLimit,
                                                                     IsLoanActive = _courierUsers.IsLoanActive,
                                                                     LoanCompany = _courierUsers.LoanCompany,
                                                                     Gender = _courierUsers.Gender,
                                                                     CategoryId = _courierUsers.CategoryId,
                                                                     SubCategoryId = _courierUsers.SubCategoryId,
                                                                     IsBreakAble = _courierUsers.IsBreakAble,
                                                                     IsHeavyWeight = _courierUsers.IsHeavyWeight,
                                                                     PaymentServiceType = _courierUsers.PaymentServiceType,
                                                                     PaymentServiceCharge = _courierUsers.PaymentServiceCharge,
                                                                     CollectionAmountLimt = _courierUsers.CollectionAmountLimt,
                                                                     CollectionAmountMInLimt = _courierUsers.CollectionAmountLimt,
                                                                     MerchantAssignActive = _courierUsers.MerchantAssignActive,
                                                                     CodChargeDhaka = _courierUsers.CodChargeDhaka,
                                                                     CodChargeOutsideDhaka = _courierUsers.CodChargeOutsideDhaka,
                                                                     CodChargePercentageDhaka = _courierUsers.CodChargePercentageDhaka,
                                                                     CodChargePercentageOutsideDhaka = _courierUsers.CodChargePercentageOutsideDhaka,
                                                                     CodChargeTypeFlag = _courierUsers.CodChargeTypeFlag,
                                                                     CodChargeTypeOutsideFlag = _courierUsers.CodChargeTypeOutsideFlag,
                                                                     IsOpenBox =_courierUsers.IsOpenBox,
                                                                     AdminUsers = new AdminUsersViewModel
                                                                     {
                                                                         UserId = _users.UserId == null ? 0 : _users.UserId,
                                                                         FullName = _users.UserName == null ? "" : _users.FullName,
                                                                         Mobile = _users.Mobile == null ? "" : _users.Mobile
                                                                     },
                                                                 };

            return await courierUsersList.FirstOrDefaultAsync();
        }

        public async Task<int> DeleteLocationAssign(int id)
        {
            var entity = await _sqlServerContext.LocationAssign.FirstOrDefaultAsync(item => item.LocationAssignId == id);
            _sqlServerContext.LocationAssign.Remove(entity);
            return await _sqlServerContext.SaveChangesAsync();
        }

        public async Task<int> DeleteUserLocationAssign(int userLocationAssignId)
        {
            var entity = await _sqlServerContext.UserLocationAssign.FirstOrDefaultAsync(e => e.Id == userLocationAssignId);

            if (entity != null)
            {
                _sqlServerContext.UserLocationAssign.Remove(entity);
                return await _sqlServerContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> DeleteDeliveryChargeDetails(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var entity = _sqlServerContext.DeliveryChargeDetails.Where(item => item.ThanaId.Equals(assignCouirerAndServiceBodyModel.ThanaId)
            && item.AreaId.Equals(assignCouirerAndServiceBodyModel.AreaId)
            && item.DeliveryRangeId.Equals(assignCouirerAndServiceBodyModel.DeliveryRangeId)
            && item.ServiceType.Equals(assignCouirerAndServiceBodyModel.ServiceType));

            _sqlServerContext.DeliveryChargeDetails.RemoveRange(entity);
            return await _sqlServerContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAssignmentFalse(RequestBodyModel requestBody)
        {
            List<DeliveryChargeDetails> entity = new List<DeliveryChargeDetails>();
            List<ChangeDeliveryChargeDetailsLog> deliveryChargeDetailsLog = new List<ChangeDeliveryChargeDetailsLog>();

            entity = _sqlServerContext.DeliveryChargeDetails.Where(item => item.DistrictId == requestBody.DistrictId
                        && item.DeliveryRangeId == requestBody.DeliveryRangeId
                        && item.IsActive == (requestBody.IsActive == false ? true : false)).ToList();

            if (entity.Count() == 0) return 0;

            foreach (var item in entity)
            {
                if (item.WeightRangeId == 2)
                {
                    deliveryChargeDetailsLog.Add(new ChangeDeliveryChargeDetailsLog
                    {
                        DistrictId = item.DistrictId,
                        ThanaId = item.ThanaId,
                        AreaId = item.AreaId,
                        OldDeliveryRangeId = item.DeliveryRangeId,
                        NewDeliveryRangeId = item.DeliveryRangeId,
                        CourierDeliveryCharge = item.CourierDeliveryCharge,
                        OldCourierId = item.CourierId,
                        NewCourierId = item.CourierId,
                        ServiceType = item.ServiceType,
                        OldIsActive = item.IsActive,
                        NewIsActive = requestBody.IsActive,
                        ChangedDate = DateTime.Now,
                        UserId = requestBody.UserId
                    });
                }

                item.IsActive = requestBody.IsActive == true ? true : false;
            }

            await _sqlServerContext.BulkUpdateAsync(entity);
            await _sqlServerContext.BulkInsertAsync(deliveryChargeDetailsLog);

            return 1;
        }

        public async Task<int> UpdateDeliveryChargeDetails(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var entity = new List<DeliveryChargeDetails>();
            var oldentity = new List<DeliveryChargeDetails>();

            if (assignCouirerAndServiceBodyModel.IsActive)
            {
                entity = _sqlServerContext.DeliveryChargeDetails.Where(item => item.ThanaId.Equals(assignCouirerAndServiceBodyModel.ThanaId)
                && item.AreaId.Equals(assignCouirerAndServiceBodyModel.AreaId)
                && item.DeliveryRangeId.Equals(assignCouirerAndServiceBodyModel.DeliveryRangeId)
                && item.ServiceType.Equals(assignCouirerAndServiceBodyModel.ServiceType)
                && item.CourierId.Equals(assignCouirerAndServiceBodyModel.CourierId)
                && item.IsActive.Equals(false)).ToList();
            }

            else if (assignCouirerAndServiceBodyModel.IsActive.Equals(false))
            {
                entity = _sqlServerContext.DeliveryChargeDetails.Where(item => item.ThanaId.Equals(assignCouirerAndServiceBodyModel.ThanaId)
                && item.AreaId.Equals(assignCouirerAndServiceBodyModel.AreaId)
                && item.DeliveryRangeId.Equals(assignCouirerAndServiceBodyModel.DeliveryRangeId)
                && item.ServiceType.Equals(assignCouirerAndServiceBodyModel.ServiceType)
                && item.CourierId.Equals(assignCouirerAndServiceBodyModel.CourierId)
                && item.IsActive.Equals(true)).ToList();
            }

            foreach (var item in entity)
            {
                oldentity.Add(new DeliveryChargeDetails
                {
                    Id = item.Id,
                    DistrictId = item.DistrictId,
                    ThanaId = item.ThanaId,
                    AreaId = item.AreaId,
                    WeightRangeId = item.WeightRangeId,
                    DeliveryRangeId = item.DeliveryRangeId,
                    CourierDeliveryCharge = item.CourierDeliveryCharge,
                    IsOpenBox = item.IsOpenBox,
                    CourierId = item.CourierId,
                    ServiceType = item.ServiceType,
                    IsActive = item.IsActive
                });

                item.IsActive = assignCouirerAndServiceBodyModel.IsActive;
            }

            await _sqlServerContext.BulkUpdateAsync(entity);

            await UpdateChangeDeliveryChargeDetailsLog(oldentity, assignCouirerAndServiceBodyModel);

            return 1;
        }

        public async Task<int> UpdateDeliveryChargeMerchantDetails(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var entity = new List<DeliveryChargeMerchantDetails>();

            if (assignCouirerAndServiceBodyModel.IsActive)
            {
                entity = _sqlServerContext.DeliveryChargeMerchantDetails.Where(item => item.ThanaId.Equals(assignCouirerAndServiceBodyModel.ThanaId)
                && item.AreaId.Equals(assignCouirerAndServiceBodyModel.AreaId)
                && item.DeliveryRangeId.Equals(assignCouirerAndServiceBodyModel.DeliveryRangeId)
                && item.ServiceType.Equals(assignCouirerAndServiceBodyModel.ServiceType)
                && item.CourierId.Equals(assignCouirerAndServiceBodyModel.CourierId)
                && item.CourierUserId.Equals(assignCouirerAndServiceBodyModel.CourierUserId)
                && item.IsActive.Equals(false)).ToList();
            }

            else if (assignCouirerAndServiceBodyModel.IsActive.Equals(false))
            {
                entity = _sqlServerContext.DeliveryChargeMerchantDetails.Where(item => item.ThanaId.Equals(assignCouirerAndServiceBodyModel.ThanaId)
                && item.AreaId.Equals(assignCouirerAndServiceBodyModel.AreaId)
                && item.DeliveryRangeId.Equals(assignCouirerAndServiceBodyModel.DeliveryRangeId)
                && item.ServiceType.Equals(assignCouirerAndServiceBodyModel.ServiceType)
                && item.CourierId.Equals(assignCouirerAndServiceBodyModel.CourierId)
                && item.CourierUserId.Equals(assignCouirerAndServiceBodyModel.CourierUserId)
                && item.IsActive.Equals(true)).ToList();
            }

            foreach (var item in entity)
            {
                item.IsActive = assignCouirerAndServiceBodyModel.IsActive;
            }

            await _sqlServerContext.BulkUpdateAsync(entity);

            return 1;
        }

        public async Task<int> UpdateServiceTypeCourier(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var entity = new List<DeliveryChargeDetails>();
            var oldentity = new List<DeliveryChargeDetails>();

            entity = _sqlServerContext.DeliveryChargeDetails.Where(item => item.ThanaId.Equals(assignCouirerAndServiceBodyModel.ThanaId)
                && item.AreaId.Equals(assignCouirerAndServiceBodyModel.AreaId)
                && item.DeliveryRangeId.Equals(assignCouirerAndServiceBodyModel.DeliveryRangeId)
                && item.ServiceType.Equals(assignCouirerAndServiceBodyModel.ServiceType)
                && item.IsActive.Equals(assignCouirerAndServiceBodyModel.IsActive)).ToList();

            foreach (var item in entity)
            {
                oldentity.Add(new DeliveryChargeDetails
                {
                    Id = item.Id,
                    DistrictId = item.DistrictId,
                    ThanaId = item.ThanaId,
                    AreaId = item.AreaId,
                    WeightRangeId = item.WeightRangeId,
                    DeliveryRangeId = item.DeliveryRangeId,
                    CourierDeliveryCharge = item.CourierDeliveryCharge,
                    IsOpenBox = item.IsOpenBox,
                    CourierId = item.CourierId,
                    ServiceType = item.ServiceType,
                    IsActive = item.IsActive
                });

                item.CourierId = assignCouirerAndServiceBodyModel.CourierId;
            }

            await _sqlServerContext.BulkUpdateAsync(entity);

            await UpdateChangeDeliveryChargeDetailsLog(oldentity, assignCouirerAndServiceBodyModel);

            return 1;
        }

        public async Task<int> UpdateMerchantServiceTypeCourier(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var entity = new List<DeliveryChargeMerchantDetails>();

            entity = _sqlServerContext.DeliveryChargeMerchantDetails.Where(item => item.ThanaId.Equals(assignCouirerAndServiceBodyModel.ThanaId)
                && item.AreaId.Equals(assignCouirerAndServiceBodyModel.AreaId)
                && item.DeliveryRangeId.Equals(assignCouirerAndServiceBodyModel.DeliveryRangeId)
                && item.ServiceType.Equals(assignCouirerAndServiceBodyModel.ServiceType)
                && item.IsActive.Equals(assignCouirerAndServiceBodyModel.IsActive)
                && item.CourierUserId.Equals(assignCouirerAndServiceBodyModel.CourierUserId)).ToList();

            foreach (var item in entity)
            {
                item.CourierId = assignCouirerAndServiceBodyModel.CourierId;
            }

            await _sqlServerContext.BulkUpdateAsync(entity);

            return 1;
        }

        public async Task<ChangeDeliveryChargeDetailsLog> UpdateChangeDeliveryChargeDetailsLog(List<DeliveryChargeDetails> oldentity, AssignCouirerAndServiceBodyModel newentity)
        {
            var entity = oldentity.FirstOrDefault(x => x.WeightRangeId == 2);
            var deliveryChargeDetailsLog = new ChangeDeliveryChargeDetailsLog
            {
                DistrictId = entity.DistrictId,
                ThanaId = entity.ThanaId,
                AreaId = entity.AreaId,
                OldDeliveryRangeId = entity.DeliveryRangeId,
                NewDeliveryRangeId = entity.DeliveryRangeId,
                CourierDeliveryCharge = entity.CourierDeliveryCharge,
                OldCourierId = entity.CourierId,
                NewCourierId = newentity.CourierId,
                ServiceType = entity.ServiceType,
                OldIsActive = entity.IsActive,
                NewIsActive = newentity.IsActive,
                ChangedDate = DateTime.Now,
                UserId = newentity.UserId
            };

            await _sqlServerContext.ChangeDeliveryChargeDetailsLog.AddAsync(deliveryChargeDetailsLog);
            await _sqlServerContext.SaveChangesAsync();
            return deliveryChargeDetailsLog;
        }

        public async Task<DeliveryUsers> UpdateDeliveryUsers(DeliveryUsers request, int userId)
        {
            var entity = await _sqlServerContext.DeliveryUsers.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (entity != null)
            {
                if (request.Name != null)
                {
                    entity.Name = request.Name;
                }
                if (request.Mobile != null)
                {
                    entity.Mobile = request.Mobile;
                }
                if (request.RiderType != null)
                {
                    entity.RiderType = request.RiderType;
                }
                if (entity.IsActive != 0)
                {
                    entity.IsActive = request.IsActive;
                }
                entity.HubName = request.HubName;
                entity.UpdatedOn = DateTime.Now;
                entity.UpdatedBy = userId;

                _sqlServerContext.DeliveryUsers.Update(entity);

                await _sqlServerContext.SaveChangesAsync();

            }
            return entity;
        }

        public async Task<Vouchers> UpdateVoucher(Vouchers vouchers)
        {
            var entity = await _sqlServerContext.Vouchers
                .FirstOrDefaultAsync(x => x.CourierUserId == vouchers.CourierUserId
                && x.VoucherCode == vouchers.VoucherCode
                && x.DeliveryRangeId == vouchers.DeliveryRangeId);
            if (entity != null)
            {
                if (entity.IsActive)
                {
                    entity.IsActive = false;
                }
                else
                {
                    entity.IsActive = true;
                }
                _sqlServerContext.Vouchers.Update(entity);
                await _sqlServerContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<int> BulkInsertRedxPopData(List<RedxPop> popData)
        {
            await _sqlServerContext.RedxPop.AddRangeAsync(popData);
            var res = await _sqlServerContext.SaveChangesAsync();
            return res;
        }

        public async Task<int> AddMailContent(PaymentMail request)
        {

            await _smtpClient.SendMailAsync(new MailMessage(
                        from: "info@deliverytiger.com.bd",
                        to: "payment@deliverytiger.com.bd",
                        subject: "Bank payment information",
                        body: $"চলে আসলো ব্যাংক ট্রান্সফার সুবিধা। এখন থেকে ডেলিভারির মাত্র ২৪ ঘণ্টায়(চার্জ প্রযোজ্য)। ব্যাংক ট্রান্সফার এক্টিভেট করতে হলে আপনার নিম্নাক্ত তথ্যসহ ডেলিভারি টাইগারের রেজিস্টার্ড মেইল থেকে দ্রুত ইমেইল করুন।{Environment.NewLine}একাউন্ট নামঃ {request.AccountHolderName} {Environment.NewLine}একাউন্ট নম্বরঃ {request.AccountNo} {Environment.NewLine}রাউটিং নম্বরঃ {request.RoutingNo} {Environment.NewLine}ব্যাংক নামঃ {request.BankName} {Environment.NewLine}ব্রাঞ্চ নামঃ {request.BranchName} {Environment.NewLine}ব্যাংকে এ প্রদেয় মোবাইল নম্বরঃ {request.RegistrationPhoneNo}"
                     ));

            await _sqlServerContext.PaymentMail.AddRangeAsync(request);
            var res = await _sqlServerContext.SaveChangesAsync();
            return res;
        }
    }
}
