using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class DistrictInfoRepository : IDistrictInfoRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ConnectionStringList _connectionStrings;
        public DistrictInfoRepository(SqlServerContext sqlServerContext, IHttpContextAccessor httpContextAccessor,
            IOptions<ConnectionStringList> connectionStrings)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _httpContextAccessor = httpContextAccessor;
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<Districts> AddDistrict(Districts districts)
        {
            await _sqlServerContext.Districts.AddAsync(districts);
            await _sqlServerContext.SaveChangesAsync();
            return districts;
        }

        public async Task<Districts> UpdateDistrict(Districts districts)
        {
            var entity = await _sqlServerContext.Districts.FirstOrDefaultAsync(item => item.DistrictId == districts.DistrictId);
            if (entity != null)
            {
                entity.UpdatedBy = districts.UpdatedBy;
                entity.IsDtOwnSecondMileDelivery = districts.IsDtOwnSecondMileDelivery;
                entity.EDeshMobileNo = districts.EDeshMobileNo;
                entity.TigerMobileNo = districts.TigerMobileNo;
                entity.OwnSecondMileDelivery = districts.OwnSecondMileDelivery;
                entity.IsActiveForCorona = districts.IsActiveForCorona;
                entity.DistrictPriority = districts.DistrictPriority;
                entity.DistrictBng = districts.DistrictBng;
                entity.District = districts.District;
                entity.IsActive = districts.IsActive;
                entity.PostalCode = districts.PostalCode;
                entity.RedxAreaId = districts.RedxAreaId;
                entity.RedxAreaName = districts.RedxAreaName;
                entity.CollectionTimeSlotId = districts.CollectionTimeSlotId;
                entity.NextDayAlertMessage = districts.NextDayAlertMessage;
                entity.EdeshThana = districts.EdeshThana;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return entity;
            }
            return null;
        }

        public async Task<Hub> AddHub(Hub hub)
        {
            await _sqlServerContext.Hub.AddAsync(hub);
            await _sqlServerContext.SaveChangesAsync();
            return hub;
        }

        public async Task<Hub> UpdateHub(Hub hub)
        {
            var entity = await _sqlServerContext.Hub.FirstOrDefaultAsync(item => item.Id == hub.Id);
            if (entity != null)
            {
                entity.Name = hub.Name;
                entity.Value = hub.Value;
                entity.HubAddress = hub.HubAddress;
                entity.IsActive = hub.IsActive;
                entity.Longitude = hub.Longitude;
                entity.Latitude = hub.Latitude;
                entity.HubMobile = hub.HubMobile;
                entity.RedxPickUpStoreId = hub.RedxPickUpStoreId;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return entity;
            }
            return null;
        }

        public async Task<int> UpdateTimeSlot(CollectionTimeSlot timeSlot)
        {
            var entity = await _sqlServerContext.CollectionTimeSlot.FirstOrDefaultAsync(item => item.CollectionTimeSlotId == timeSlot.CollectionTimeSlotId);
            if (entity != null)
            {
                entity.StartTime = timeSlot.StartTime;
                entity.EndTime = timeSlot.EndTime;
                entity.CutOffTime = timeSlot.CutOffTime;
                entity.Ordering = timeSlot.Ordering;
                entity.OrderLimit = timeSlot.OrderLimit;
                entity.IsActive = timeSlot.IsActive;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return 1;
            }
            return 0;
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetCutomerListForApp(RequestBodyModel request)
        {
            int[] statusArray = { 15, 24, 25, 56 };

            var data = await (from _orders in _sqlServerContext.CourierOrders.AsNoTracking()
                              join _districts in _sqlServerContext.Districts on _orders.DistrictId equals _districts.DistrictId
                              where statusArray.Contains(_orders.Status)
                              && _orders.MerchantId.Equals(request.MerchantId)
                              select new
                              {
                                  _orders.Mobile,
                                  _orders.CustomerName,
                                  _districts.DistrictId,
                                  _districts.District,
                                  //_orders.Status
                                  _orders.Id

                              }).Skip(request.Index).Take(request.Count).Distinct().ToListAsync();

            var courierOrdersViewModel = data.GroupBy(g => g.Mobile).Select(x => new CourierOrdersViewModel
            {

                Mobile = x.FirstOrDefault().Mobile,
                CustomerName = x.FirstOrDefault().CustomerName,
                //Status = x.FirstOrDefault().Status,
                TotalOrder = x.Select(item => item.Id).Count(),
                DistrictsViewModel = new DistrictsViewModel
                {
                    DistrictId = x.FirstOrDefault().DistrictId,
                    District = x.FirstOrDefault().District,
                }

            }).OrderByDescending(x => x.TotalOrder);

            var allCustomers = courierOrdersViewModel.Union(
                _sqlServerContext.OwnPhoneBook.Where(p => p.CourierUserId == request.MerchantId)
                .Skip(request.Index).Take(request.Count)
                .Select(s => new CourierOrdersViewModel
                {
                    Mobile = s.Mobile,
                    CustomerName = s.CustomerName,
                    TotalOrder = 0,
                    DistrictsViewModel = new DistrictsViewModel
                    {
                        DistrictId = 0,
                        District = "",
                    }
                })
                );

            return allCustomers.ToList();
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetCutomerWiseOrdersDetailsForApp(RequestBodyModel request)
        {
            int[] statusArray = { 15, 24, 25, 56 };

            var data = await (from _orders in _sqlServerContext.CourierOrders.AsNoTracking()
                              where _orders.Mobile.Equals(request.Mobile)
                              && statusArray.Contains(_orders.Status)
                              orderby _orders.Id descending
                              select new
                              {
                                  _orders.Id,
                                  _orders.OrderDate,
                                  Address = _orders.Address == null ? "" : _orders.Address,
                                  OtherMobile = _orders.OtherMobile == null ? "" : _orders.OtherMobile,
                                  _orders.PodNumber

                              }).ToListAsync();

            var courierOrdersViewModel = data.Select(x => new CourierOrdersViewModel
            {

                CourierOrdersId = "DT-" + x.Id,
                OrderDate = x.OrderDate,
                Address = x.Address,
                OtherMobile = x.OtherMobile,
                PodNumber = x.PodNumber

            });

            return courierOrdersViewModel.ToList();
        }

        public async Task<MerchantInfoUpdate> AddMerchantInfoUpdateLog(MerchantInfoUpdate merchantInfoUpdate)
        {
            await _sqlServerContext.MerchantInfoUpdate.AddAsync(merchantInfoUpdate);
            await _sqlServerContext.SaveChangesAsync();
            return merchantInfoUpdate;
        }

        public async Task<int> UpdateCourier(Couriers couriers)
        {
            var entity = await _sqlServerContext.Couriers.FirstOrDefaultAsync(c => c.CourierId == couriers.CourierId);

            if (entity != null)
            {
                entity.CourierName = couriers.CourierName;
                entity.ContactNo = couriers.ContactNo;
                entity.ContactAddress = couriers.ContactAddress;
                entity.IsActive = couriers.IsActive;
                entity.UpdatedBy = couriers.UpdatedBy;
                entity.UpdatedOn = DateTime.Now;
                entity.UserName = couriers.UserName;
                entity.Password = couriers.Password;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return 1;
            }
            return 0;
        }

        public async Task<CustomComment> AddCustomComment(CustomComment customComment)
        {
            await _sqlServerContext.CustomComment.AddAsync(customComment);
            await _sqlServerContext.SaveChangesAsync();
            return customComment;
        }

        public async Task<IEnumerable<dynamic>> GetAllCustomComment(int orderId)
        {
            var data = await (from _customComment in _sqlServerContext.CustomComment.AsNoTracking()
                              join _users in _sqlServerContext.Users.AsNoTracking()
                              on _customComment.PostedBy equals _users.UserId
                              join _orderStatus in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                              on _customComment.StatusId equals _orderStatus.StatusId

                              where _customComment.OrderId.Equals(orderId)

                              select new
                              {
                                  OrderId = "DT-" + _customComment.OrderId,
                                  PostedOn = _customComment.PostedOn.ToString("dddd, dd MMMM yyyy hh:mm tt"),
                                  Comment = _customComment.Comment,
                                  IsConfirmedBy = _customComment.IsConfirmedBy,
                                  StatusNameEng = _orderStatus.StatusNameEng,
                                  FullName = _users.FullName
                              }).OrderByDescending(cc => cc.PostedOn).ToListAsync();

            return data;
        }
    }
}
