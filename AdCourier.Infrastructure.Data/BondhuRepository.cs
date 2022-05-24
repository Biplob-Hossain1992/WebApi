using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using EFCore.BulkExtensions;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;

namespace AdCourier.Infrastructure.Data
{
    public class BondhuRepository: IBondhuRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        public BondhuRepository(SqlServerContext sqlServerContext, IOptions<ConnectionStringList> connectionStrings)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<List<SelfDeliveryOrderResponseModel>> LoadOrderForBondhuApp(SelfDeliveryOrderRequestNewModel model)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@DeliveryUserId", value: model.DeliveryUserId, dbType: DbType.Int32);
                parameter.Add(name: "@ProductTitle", value: model.ProductTitle, dbType: DbType.String);
                parameter.Add(name: "@OrderId", value: model.OrderId, dbType: DbType.String);
                parameter.Add(name: "@StatusId", value: model.StatusId, dbType: DbType.String);
                parameter.Add(name: "@DtStatusId", value: model.DtStatusId, dbType: DbType.String);
                parameter.Add(name: "@Index", value: model.Index, dbType: DbType.String);
                parameter.Add(name: "@Count", value: model.Count, dbType: DbType.String);
                parameter.Add(name: "@Flag", value: model.Flag, dbType: DbType.String);
                parameter.Add(name: "@CustomType", value: model.CustomType, dbType: DbType.String);
                parameter.Add(name: "@Type", value: model.Type, dbType: DbType.String);

                var data = await connection.QueryAsync<SelfDeliveryOrderResponseModel>(
                        sql: @"[DT].[USP_GetAllDeliveryLocationsOrdersNew]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        public async Task<List<SelfDeliveryOrderResponseModel>> LoadOrderReturnForBondhuApp(SelfDeliveryOrderRequestNewModel model)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@DeliveryUserId", value: model.DeliveryUserId, dbType: DbType.Int32);
                parameter.Add(name: "@ProductTitle", value: model.ProductTitle, dbType: DbType.String);
                parameter.Add(name: "@StatusId", value: model.StatusId, dbType: DbType.String);
                parameter.Add(name: "@DtStatusId", value: model.DtStatusId, dbType: DbType.String);
                parameter.Add(name: "@RiderType", value: model.RiderType, dbType: DbType.String);
                parameter.Add(name: "@OrderId", value: model.OrderId, dbType: DbType.String);

                var data = await connection.QueryAsync<SelfDeliveryOrderResponseModel>(
                        sql: @"[DT].[USP_GetAllDeliveryLocationsReturn]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        public async Task<List<SelfDeliveryOrderResponseModel>> LoadOrderForBondhuAppByTimeSlot(SelfDeliveryOrderRequestNewModel model)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@DeliveryUserId", value: model.DeliveryUserId, dbType: DbType.Int32);
                parameter.Add(name: "@ProductTitle", value: model.ProductTitle, dbType: DbType.String);
                parameter.Add(name: "@OrderId", value: model.OrderId, dbType: DbType.String);
                parameter.Add(name: "@StatusId", value: model.StatusId, dbType: DbType.String);
                parameter.Add(name: "@DtStatusId", value: model.DtStatusId, dbType: DbType.String);
                parameter.Add(name: "@Index", value: model.Index, dbType: DbType.String);
                parameter.Add(name: "@Count", value: model.Count, dbType: DbType.String);
                parameter.Add(name: "@Flag", value: model.Flag, dbType: DbType.String);
                parameter.Add(name: "@CustomType", value: model.CustomType, dbType: DbType.String);
                parameter.Add(name: "@Type", value: model.Type, dbType: DbType.String);

                var data = await connection.QueryAsync<SelfDeliveryOrderResponseModel>(
                        sql: @"[DT].[USP_LoadOrderForBondhuAppByTimeSlot]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        public async Task<List<SelfDeliveryOrderResponseModel>> LoadOrderForBondhuAppByTimeSlotNew(SelfDeliveryOrderRequestNewModel model)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@DeliveryUserId", value: model.DeliveryUserId, dbType: DbType.Int32);
                parameter.Add(name: "@ProductTitle", value: model.ProductTitle, dbType: DbType.String);
                parameter.Add(name: "@OrderId", value: model.OrderId, dbType: DbType.String);
                parameter.Add(name: "@StatusId", value: model.StatusId, dbType: DbType.String);
                parameter.Add(name: "@DtStatusId", value: model.DtStatusId, dbType: DbType.String);
                parameter.Add(name: "@Index", value: model.Index, dbType: DbType.String);
                parameter.Add(name: "@Count", value: model.Count, dbType: DbType.String);
                parameter.Add(name: "@Flag", value: model.Flag, dbType: DbType.String);
                parameter.Add(name: "@CustomType", value: model.CustomType, dbType: DbType.String);
                parameter.Add(name: "@Type", value: model.Type, dbType: DbType.String);
                parameter.Add(name: "@CollectionSlotId", value: model.CollectionSlotId, dbType: DbType.Int32);
                parameter.Add(name: "@FromDate", value: model.FromDate, dbType: DbType.String);
                parameter.Add(name: "@ToDate", value: model.ToDate , dbType: DbType.String);

                var data = await connection.QueryAsync<SelfDeliveryOrderResponseModel>(
                        sql: @"[DT].[USP_LoadOrderForBondhuAppByTimeSlotNew]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        public async Task<SelfDeliveryModel> DeliveryManRegistration(DeliveryBondhuRegistration bondhuRegistration)
        {
            
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@Name", value: bondhuRegistration.Name, dbType: DbType.String);
                parameter.Add(name: "@Mobile", value: bondhuRegistration.Mobile, dbType: DbType.String);
                parameter.Add(name: "@AlternativeMobile", value: bondhuRegistration.AlternativeMobile, dbType: DbType.String);
                parameter.Add(name: "@BkashMobileNumber", value: bondhuRegistration.BkashMobileNumber, dbType: DbType.String);
                parameter.Add(name: "@Password", value: bondhuRegistration.Password, dbType: DbType.String);
                parameter.Add(name: "@Address", value: bondhuRegistration.Address, dbType: DbType.String);
                parameter.Add(name: "@DistrictId", value: bondhuRegistration.DistrictId, dbType: DbType.Int32);
                parameter.Add(name: "@ThanaId", value: bondhuRegistration.ThanaId, dbType: DbType.Int32);
                parameter.Add(name: "@PostCode", value: bondhuRegistration.PostCode, dbType: DbType.Int32);
                parameter.Add(name: "@DistrictName", value: bondhuRegistration.DistrictName, dbType: DbType.String);
                parameter.Add(name: "@ThanaName", value: bondhuRegistration.ThanaName, dbType: DbType.String);
                parameter.Add(name: "@IsActive", value: bondhuRegistration.IsActive, dbType: DbType.Int32);
                parameter.Add(name: "@RegistrationFrom", value: bondhuRegistration.RegistrationFrom, dbType: DbType.String);
                parameter.Add(name: "@Version", value: bondhuRegistration.Version, dbType: DbType.String);
                


                var data = await connection.QueryAsync<SelfDeliveryModel>(
                    sql: @"[DT].[USP_InsertDeliveryUserInfo]",
                    commandType: CommandType.StoredProcedure,
                    param: parameter
                    );

                return data.FirstOrDefault();

            }
        }

        public async Task<bool> UpdateDeliveryManInfo(DeliveryManGeneralInfoUpdate infoUpdate)
        {

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@Id", value: infoUpdate.BondhuId, dbType: DbType.Int32);
                parameter.Add(name: "@Name", value: infoUpdate.Name, dbType: DbType.String);
                parameter.Add(name: "@Mobile", value: infoUpdate.Mobile, dbType: DbType.String);
                parameter.Add(name: "@AlternativeMobile", value: infoUpdate.AlternativeMobile, dbType: DbType.String);
                parameter.Add(name: "@BkashMobileNumber", value: infoUpdate.BkashMobileNumber, dbType: DbType.String);
                parameter.Add(name: "@Address", value: infoUpdate.Address, dbType: DbType.String);
                parameter.Add(name: "@DistrictId", value: infoUpdate.AreaInfo.FirstOrDefault().DistrictId, dbType: DbType.Int32);
                parameter.Add(name: "@DistrictName", value: infoUpdate.AreaInfo.FirstOrDefault().DistrictName, dbType: DbType.String);
                parameter.Add(name: "@ThanaId", value: infoUpdate.AreaInfo.FirstOrDefault().ThanaId, dbType: DbType.Int32);
                parameter.Add(name: "@ThanaName", value: infoUpdate.AreaInfo.FirstOrDefault().ThanaName, dbType: DbType.String);
                parameter.Add(name: "@PostCode", value: infoUpdate.AreaInfo.FirstOrDefault().PostCode, dbType: DbType.Int32);
                parameter.Add(name: "@AreaId", value: infoUpdate.AreaInfo.FirstOrDefault().AreaId, dbType: DbType.Int32);
                parameter.Add(name: "@AreaName", value: infoUpdate.AreaInfo.FirstOrDefault().AreaName, dbType: DbType.String);
                
                
                var data = await connection.QueryAsync<bool>(
                    sql: @"[DT].[USP_UpdateDeliveryBondhuInfo]",
                    commandType: CommandType.StoredProcedure,
                    param: parameter
                    );

                return data.FirstOrDefault();

            }
        }

        public async Task<IEnumerable<SelfDeliveryLoginResponseModel>> SelfDeliveryLogin(SelfDeliveryLoginModel model)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var data = await connection.QueryAsync<SelfDeliveryLoginResponseModel>(
                        sql: @"[DT].[USP_LoginDeliveryUser]",
                        param: model,
                        commandType: CommandType.StoredProcedure);

                return data;
            }
        }
        public async Task<IEnumerable<OrderStatusCountView>> GetDeliveryBondhuOrderStatusHistoryCountDeliveryManWise(DeliveryBondhuOrderSearchModel searchModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: searchModel.FromDate, dbType: DbType.String);
                parameter.Add(name: "@ToDate", value: searchModel.ToDate, dbType: DbType.String);
                parameter.Add(name: "@DeliveryManId", value: searchModel.DeliveryManId, dbType: DbType.Int32);
                parameter.Add(name: "@CollectionTimeSlotId", value: searchModel.CollectionTimeSlotId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<OrderStatusCountView>(
                        sql: @"[DT].[DeliveryBondhuOrderStatusHistoryCountDeliveryManWise]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public async Task<IEnumerable<DtOrderDetailsDataModel>> GetDtOrderHistoryDetailsReportForDeliveryMan(DeliveryBondhuOrderSearchModel searchModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: searchModel.FromDate, dbType: DbType.String);
                parameter.Add(name: "@ToDate", value: searchModel.ToDate, dbType: DbType.String);
                parameter.Add(name: "@DeliveryManId", value: searchModel.DeliveryManId, dbType: DbType.Int32);
                parameter.Add(name: "@Status", value: searchModel.Status, dbType: DbType.String);
                parameter.Add(name: "@CollectionTimeSlotId", value: searchModel.CollectionTimeSlotId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<DtOrderDetailsDataModel>(
                        sql: @"[Reports].[USP_DtOrderHistoryDetailsReport_ForDeliveryMan]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public async Task<int> UpdateDocumentUrl(List<CourierOrders> orders)
        {

            var orderIds = (from d in orders
                            select d.CourierOrdersId).ToArray();

            var entity = await _sqlServerContext.CourierOrders.AsNoTracking().Where(x => orderIds.Contains(x.CourierOrdersId))
                .BatchUpdateAsync(x => new CourierOrders
                { 
                    DocumentUrl = orders.FirstOrDefault().DocumentUrl
                });

            return entity;
        }

        public async Task<int> AddLatLag(LatLagModel model)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var data = await connection.ExecuteAsync(
                        sql: @"[DT].[USP_InsertLatitudeLongitudeInfo]",
                        param: model,
                        commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public async Task<int> GetDeliveryBondhuShowOrderAutomatic()
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                return await connection.ExecuteAsync(
                        sql: @"[DT].[DeliveryBondhuShowOrderAutomatic]",
                        commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<CourierOrders>> GetUpdateTimeSlotAutomatic()
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                return await connection.QueryAsync<CourierOrders>(
                        sql: @"[DT].[Usp_UpdateTimeSlotWithSms]",
                        commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<DeliveryUsers> UserAccess(int bondhuId, bool isNowOffline)
        {
            var acceptedOrders = await _sqlServerContext.CourierOrders.Where(x => x.UpdatedBy == bondhuId && x.IsConfirmedBy == "deliveryman" && x.Status == 41).
                Select(y => new CourierOrders
                    {
                        Id = y.Id,
                        Status = 40,
                        UpdatedBy = y.DeliveryUserId,
                        Comment = "Order has been assigned Product will be collected from collection point-40",
                        IsConfirmedBy = "deliveryman"
                }).ToListAsync();



            if(acceptedOrders.Count != 0)
            {
                var orderIds = (from data in acceptedOrders
                                select data.Id).ToArray();


                var entity = await _sqlServerContext.CourierOrders.Where(x => orderIds.Contains(x.Id))
                                .BatchUpdateAsync(y => new CourierOrders
                                {
                                    Status = acceptedOrders.FirstOrDefault().Status,
                                    UpdatedBy = acceptedOrders.FirstOrDefault().UpdatedBy,
                                    Comment = acceptedOrders.FirstOrDefault().Comment,
                                    IsConfirmedBy = acceptedOrders.FirstOrDefault().IsConfirmedBy
                                });


                var courierOrderStatusHistory = await _sqlServerContext.CourierOrders.Where(x => orderIds.Contains(x.Id)).Select(y => new CourierOrderStatusHistory
                {
                    CourierOrderId = y.CourierOrdersId,
                    IsConfirmedBy = y.IsConfirmedBy,
                    OrderDate = y.OrderDate,
                    Status = y.Status,
                    PostedBy = y.UpdatedBy,
                    MerchantId = y.MerchantId,
                    Comment = y.Comment,
                    PodNumber = y.PodNumber,
                    CourierId = y.CourierId,
                    HubName = y.HubName,
                    CourierDeliveryManName = y.CourierDeliveryManName,
                    CourierDeliveryManMobile = y.CourierDeliveryManMobile,
                    PostedOn = y.UpdatedOn
                }).ToListAsync();

                await _sqlServerContext.BulkInsertAsync(courierOrderStatusHistory);
            }


            return null;
        }

        public async Task<UserAccessResponseModel> GetBondhuInfo(int bondhuId)
        {
            var deliveryUser = await _sqlServerContext.DeliveryUsers.Where(x => x.Id == bondhuId).FirstOrDefaultAsync();

            var response = new UserAccessResponseModel()
            {
                RiderType = deliveryUser.RiderType,
                UserType = deliveryUser.UserType,
                IsNowOffline = deliveryUser.IsNowOffline,
                ProfileImage = deliveryUser.IsProfileImage == true ? BaseUrlModel.ImageBaseUrl + "images/bondhuprofileimage/" + deliveryUser.Id + "/profileimage.jpg" : "",
                IsProfileImage = deliveryUser.IsProfileImage == null?false: deliveryUser.IsProfileImage,
                IsDrivingLicense = deliveryUser.IsDrivingLicense == null? false: deliveryUser.IsDrivingLicense,
                IsNID = deliveryUser.IsNID == null? false: deliveryUser.IsNID,
                Name = deliveryUser.Name,
                Mobile = deliveryUser.Mobile
            };


            return response;
        }

        public async Task<CourierOrders> GetBondhuAcceptStatus(CourierOrders courierOrders)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@OrderId", value: courierOrders.Id, dbType: DbType.Int32);
                parameter.Add(name: "@CollectionTimeSlotId", value: courierOrders.CollectionTimeSlotId, dbType: DbType.Int32);
                
                var data = await connection.QueryAsync<CourierOrders>(
                        sql: @"[DT].[USP_CheckDTStatus]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.FirstOrDefault();

            }
        }

        public async Task<int> UpdateSelfDeliveryUserPassword(SelfDeliveryUserPasswordUpdateModel updateModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@Password", value: updateModel.Password, dbType: DbType.String);
                parameter.Add(name: "@CustomerId", value: updateModel.CustomerId, dbType: DbType.Int32);
                parameter.Add(name: "@Mobile", value: updateModel.Mobile, dbType: DbType.String);

                return await connection.ExecuteAsync(
                        sql: @"[DT].[UpdateSelfDeliveryUserPassword]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

            }
        }

        public async Task<dynamic> GetZoneWiseOrdersCount(RequestBodyModel requestBody)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@fromDate", value: requestBody.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@toDate", value: requestBody.ToDate, dbType: DbType.DateTime);
                parameter.Add(name: "@zoneId", value: requestBody.ZoneId, dbType: DbType.Int32);
                parameter.Add(name: "@type", value: requestBody.Type, dbType: DbType.String);

                var data = await connection.QueryAsync<dynamic>(
                        sql: @"[DT].[USP_GetZoneWiseOrdersCount]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data;

            }
        }

        public async Task<dynamic> GetZoneWiseOrderDetails(RequestBodyModel requestBody)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@fromDate", value: requestBody.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@toDate", value: requestBody.ToDate, dbType: DbType.DateTime);
                parameter.Add(name: "@zoneId", value: requestBody.ZoneId, dbType: DbType.Int32);
                parameter.Add(name: "@thanaId", value: requestBody.ThanaId, dbType: DbType.Int32);
                parameter.Add(name: "@type", value: requestBody.Type, dbType: DbType.String);

                var data = await connection.QueryAsync<dynamic>(
                        sql: @"[DT].[USP_GetZoneWiseOrdersCountDetails]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data;

            }
        }

        public async Task<IEnumerable<dynamic>> CollectedNotCollectedMerchantInfo(RequestBodyModel requestBody)
        {
            int[] totalMerchant = new int[] { 40, 41, 43, 44, 47, 55, 57 };
            int[] pendingMerchant = new int[] { 40, 41 };
            int[] attemptedMerchant = new int[] { 43, 47, 55 };

            var data = new List<CollectedNotCollectedMerchantWithCustomerInfo>();

            if (requestBody.Flag == 1)
            {
                data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new {key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId} equals new {key1 = _order.CollectAddressDistrictId,key2 = _order.CollectAddressThanaId}
                                  into order from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.CollectAddressThanaId == (requestBody.ThanaId == 0 ? _orders.CollectAddressThanaId : requestBody.ThanaId)
                                  && totalMerchant.Contains(_history.Status)
                                  && _orders.MerchantId != 1
                                  select new CollectedNotCollectedMerchantWithCustomerInfo
                                  {
                                      CompanyName = _users.CompanyName,
                                      Mobile = _users.Mobile,
                                      Address = _users.Address,
                                      Id = _orders.Id,
                                      CollectAddressThanaId = _orders.CollectAddressThanaId,
                                      MerchantId = _orders.MerchantId
                                  }).Distinct().ToListAsync();

            }
            else if(requestBody.Flag == 2)
            {
                data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new { key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId } equals new { key1 = _order.CollectAddressDistrictId, key2 = _order.CollectAddressThanaId }
                                  into order
                                  from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.CollectAddressThanaId == (requestBody.ThanaId == 0 ? _orders.CollectAddressThanaId : requestBody.ThanaId)
                                  && _history.Status.Equals(44)
                                  && _orders.MerchantId != 1
                                  select new CollectedNotCollectedMerchantWithCustomerInfo
                                  {
                                      CompanyName = _users.CompanyName,
                                      Mobile = _users.Mobile,
                                      Address = _users.Address,
                                      Id = _orders.Id,
                                      CollectAddressThanaId = _orders.CollectAddressThanaId,
                                      MerchantId = _orders.MerchantId
                                  }).Distinct().ToListAsync();

            }
            else if (requestBody.Flag == 3)
            {
                data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new { key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId } equals new { key1 = _order.CollectAddressDistrictId, key2 = _order.CollectAddressThanaId }
                                  into order from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.CollectAddressThanaId == (requestBody.ThanaId == 0 ? _orders.CollectAddressThanaId : requestBody.ThanaId)
                                  && pendingMerchant.Contains(_orders.Status)
                                  && _orders.MerchantId != 1
                                  select new CollectedNotCollectedMerchantWithCustomerInfo
                                  {
                                      CompanyName = _users.CompanyName,
                                      Mobile = _users.Mobile,
                                      Address = _users.Address,
                                      Id = _orders.Id,
                                      CollectAddressThanaId = _orders.CollectAddressThanaId,
                                      MerchantId = _orders.MerchantId
                                  }).Distinct().ToListAsync();

            }
            else if (requestBody.Flag == 4)
            {
                data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new { key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId } equals new { key1 = _order.CollectAddressDistrictId, key2 = _order.CollectAddressThanaId }
                                  into order from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.CollectAddressThanaId == (requestBody.ThanaId == 0 ? _orders.CollectAddressThanaId : requestBody.ThanaId)
                                  && attemptedMerchant.Contains(_orders.Status)
                                  && _orders.MerchantId != 1
                                  select new CollectedNotCollectedMerchantWithCustomerInfo
                                  {
                                      CompanyName = _users.CompanyName,
                                      Mobile = _users.Mobile,
                                      Address = _users.Address,
                                      Id = _orders.Id,
                                      CollectAddressThanaId = _orders.CollectAddressThanaId,
                                      MerchantId = _orders.MerchantId
                                  }).Distinct().ToListAsync();

            }

            var responseModel = data.GroupBy(g => g.MerchantId).Select(s => new
            {

                MerchantId = s.Key,
                CompanyName = s.FirstOrDefault().CompanyName,
                Mobile = s.FirstOrDefault().Mobile,
                Address = s.FirstOrDefault().Address,
                CollectAddressThanaId = s.FirstOrDefault().CollectAddressThanaId,
                TotalOrder = s.Select(item => item.Id).Distinct().Count()

            });

            return responseModel;

        }

        public async Task<IEnumerable<dynamic>> DeliveredAndPendingCustomerInfo(RequestBodyModel requestBody)
        {
            int[] totalCustomers = new int[] { 8, 9, 10, 33, 42, 45, 49, 47, 52, 53, 54, 64 };
            int[] pendingCustomer = new int[] { 33, 42, 47, 52, 53, 54, 64 };
            int[] courierArray = new int[] { 28, 30, 32, 34, 49 };

            var data = new List<CollectedNotCollectedMerchantWithCustomerInfo>();

            if (requestBody.Flag == 1)
            {
                data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new { key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId } equals new { key1 = _order.DistrictId, key2 = _order.ThanaId }
                                  into order from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.ThanaId == 0
                                  && totalCustomers.Contains(_history.Status)
                                  && _orders.MerchantId != 1
                                  && !courierArray.Contains(_orders.CourierId)
                                  select new CollectedNotCollectedMerchantWithCustomerInfo
                                  {
                                      CustomerName = _orders.CustomerName,
                                      Mobile = _orders.Mobile,
                                      Address = _orders.Address,
                                      Id = _orders.Id
                                  }).Distinct().ToListAsync();

            }
            else if (requestBody.Flag == 2)
            {
                data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new { key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId } equals new { key1 = _order.DistrictId, key2 = _order.ThanaId }
                                  into order from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.ThanaId == (requestBody.ThanaId == 0 ? _orders.ThanaId : requestBody.ThanaId)
                                  && _history.Status.Equals(45)
                                  && _orders.MerchantId != 1
                                  && !courierArray.Contains(_orders.CourierId)
                                  select new CollectedNotCollectedMerchantWithCustomerInfo
                                  {
                                      CustomerName = _orders.CustomerName,
                                      Mobile = _orders.Mobile,
                                      Address = _orders.Address,
                                      Id = _orders.Id
                                  }).Distinct().ToListAsync();

            }
            else if (requestBody.Flag == 3)
            {
                data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new { key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId } equals new { key1 = _order.DistrictId, key2 = _order.ThanaId }
                                  into order from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.ThanaId == (requestBody.ThanaId == 0 ? _orders.ThanaId : requestBody.ThanaId)
                                  && pendingCustomer.Contains(_orders.Status)
                                  && _orders.MerchantId != 1
                                  && !courierArray.Contains(_orders.CourierId)
                                  select new CollectedNotCollectedMerchantWithCustomerInfo
                                  {
                                      CustomerName = _orders.CustomerName,
                                      Mobile = _orders.Mobile,
                                      Address = _orders.Address,
                                      Id = _orders.Id
                                  }).Distinct().ToListAsync();

            }

            var responseModel = data.GroupBy(g => g.Mobile).Select(s => new
            {

                Mobile = s.Key,
                CustomerName = s.FirstOrDefault().CustomerName,
                Address = s.FirstOrDefault().Address,
                TotalOrder = s.Select(item => item.Id).Distinct().Count()

            });

            return responseModel;

        }

        public async Task<dynamic> MerchantWiseOrder(RequestBodyModel requestBody)
        {
            //int[] totalMerchant = new int[] { 40, 41, 43, 44, 47, 55, 57 };
            int[] pendingMerchant = new int[] { 40, 41 };
            int[] attemptedMerchant = new int[] { 43, 47, 55 };

            if (requestBody.Flag == 1)
            {
                var data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new { key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId } equals new { key1 = _order.CollectAddressDistrictId, key2 = _order.CollectAddressThanaId }
                                  into order from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId
                                  join _orderStatus in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                                  on _orders.Status equals _orderStatus.StatusId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.CollectAddressThanaId == requestBody.ThanaId
                                  && _history.Status.Equals(44)
                                  && _orders.MerchantId.Equals(requestBody.MerchantId)
                                  select new
                                  {
                                      Id = _orders.Id,
                                      StatusNameEng = _orderStatus.StatusNameEng,
                                      Status = _orderStatus.StatusId
                                  }).Distinct().ToListAsync();

               
                return data;
            }
            else if (requestBody.Flag == 2)
            {
                var data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new { key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId } equals new { key1 = _order.CollectAddressDistrictId, key2 = _order.CollectAddressThanaId }
                                  into order from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId
                                  join _orderStatus in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                                  on _orders.Status equals _orderStatus.StatusId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.CollectAddressThanaId == requestBody.ThanaId
                                  && pendingMerchant.Contains(_orders.Status)
                                  && _orders.MerchantId.Equals(requestBody.MerchantId)
                                  select new
                                  {
                                      Id = _orders.Id,
                                      StatusNameEng = _orderStatus.StatusNameEng,
                                      Status = _orderStatus.StatusId
                                  }).Distinct().ToListAsync();


                return data;
            }
            else if (requestBody.Flag == 3)
            {
                var data = await (from _zoneLocation in _sqlServerContext.DeliveryZoneLocation.AsNoTracking()
                                  join _order in _sqlServerContext.CourierOrders.AsNoTracking()
                                  on new { key1 = _zoneLocation.DistrictId, key2 = _zoneLocation.ThanaId } equals new { key1 = _order.CollectAddressDistrictId, key2 = _order.CollectAddressThanaId }
                                  into order from _orders in order.DefaultIfEmpty()
                                  join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                  on _orders.CourierOrdersId equals _history.CourierOrderId
                                  join _users in _sqlServerContext.CourierUsers.AsNoTracking()
                                  on _orders.MerchantId equals _users.CourierUserId
                                  join _orderStatus in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                                  on _orders.Status equals _orderStatus.StatusId

                                  where _history.OrderDate.Date >= requestBody.FromDate.Date
                                  && _history.OrderDate.Date < requestBody.ToDate.Date.AddDays(1)
                                  && _zoneLocation.ZoneId == (requestBody.ZoneId == 0 ? _zoneLocation.ZoneId : requestBody.ZoneId)
                                  && _orders.CollectAddressThanaId == requestBody.ThanaId
                                  && attemptedMerchant.Contains(_orders.Status)
                                  && _orders.MerchantId.Equals(requestBody.MerchantId)
                                  select new
                                  {
                                      Id = _orders.Id,
                                      StatusNameEng = _orderStatus.StatusNameEng,
                                      Status = _orderStatus.StatusId
                                  }).Distinct().ToListAsync();


                return data;
            }
            
            return null;

        }

        public async Task<IEnumerable<dynamic>> GetAllLocationAssignHistory(RequestBodyModel requestBody)
        {
            var data = await (from _assignHistory in _sqlServerContext.LocationAssignHistory.AsNoTracking()

                              join users in _sqlServerContext.Users.AsNoTracking()
                              on _assignHistory.UpdatedBy equals users.UserId into user from _users in user.DefaultIfEmpty()
                              join _zone in _sqlServerContext.DeliveryZone.AsNoTracking()
                              on _assignHistory.ZoneId equals _zone.ZoneId
                              join _thana in _sqlServerContext.Districts.AsNoTracking()
                              on _assignHistory.ThanaId equals _thana.DistrictId
                              join _deliveryUsers in _sqlServerContext.DeliveryUsers.AsNoTracking()
                              on _assignHistory.DeliveryUserId equals _deliveryUsers.Id

                              where _assignHistory.DeliveryUserId.Equals(requestBody.DeliveryRiderId)
                              //&& _assignHistory.ThanaId.Equals(requestBody.ThanaId)
                              //&& _assignHistory.AreaId == (requestBody.AreaId == 0 ? _assignHistory.AreaId : requestBody.AreaId)
                              && _assignHistory.ZoneId.Equals(requestBody.ZoneId)

                              select new
                              {
                                  ZoneName = _zone.ZoneName,
                                  Thana = _thana.District,
                                  UpdatedBy = _users.FullName,
                                  UpdatedOn = _assignHistory.UpdatedOn
                              }).OrderByDescending(h => h.UpdatedOn).ToListAsync();

            return data;
        }

        public async Task<dynamic> GetCustomCommentsWithDateRange(RequestBodyModel requestBody)
        {
            var data = await (from _cc in _sqlServerContext.CustomComment.AsNoTracking()

                              join _status in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                              on _cc.StatusId equals _status.StatusId
                              join _orders in _sqlServerContext.CourierOrders.AsNoTracking()
                              on _cc.OrderId equals _orders.Id
                              join _courierusers in _sqlServerContext.CourierUsers.AsNoTracking()
                              on _orders.MerchantId equals _courierusers.CourierUserId
                              join district in _sqlServerContext.Districts.AsNoTracking()
                              on _orders.CollectAddressThanaId equals district.DistrictId into districts
                              from _district in districts.DefaultIfEmpty()
                              join _users in _sqlServerContext.Users.AsNoTracking()
                              on _cc.PostedBy equals _users.UserId

                              where _cc.PostedOn.Date >= requestBody.FromDate.Date
                              && _cc.PostedOn.Date < requestBody.ToDate.Date.AddDays(1)

                              select new
                              {
                                  OrderId = _cc.OrderId,
                                  PostedOn = _cc.PostedOn,
                                  Comment = _cc.Comment,
                                  StatusNameEng = _status.StatusNameEng,
                                  CompanyName = _courierusers.CompanyName,
                                  District = _district.District,
                                  DistrictId = _orders.DistrictId,
                                  PaymentType = _orders.PaymentType,
                                  FullName = _users.FullName,
                                  UserId = _users.UserId

                              }).ToListAsync();

            var groupedData = data.GroupBy(c => c.FullName).Select(s => new
            {
                FullName = s.FirstOrDefault().FullName,
                UserId = s.FirstOrDefault().UserId,
                TotalOrder = s.Select(o => o.OrderId).Count()
            }).ToList();

            var responseModel = new
            {
                CountData = groupedData,
                ActualData = data
            };

            return responseModel;
        }

        public async Task<dynamic> GetMerchantWiseRiderCountWithDetails(RequestBodyModel requestBody)
        {
            var totalData = await (from _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()

                                   join _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                                   on _history.MerchantId equals _courierUsers.CourierUserId
                                   join _deliveryUsers in _sqlServerContext.DeliveryUsers.AsNoTracking()
                                   on _history.PostedBy equals _deliveryUsers.Id
                                   join _orders in _sqlServerContext.CourierOrders.AsNoTracking()
                                   on _history.CourierOrderId equals _orders.CourierOrdersId
                                   join _collectionSlot in _sqlServerContext.CollectionTimeSlot.AsNoTracking()
                                   on _orders.CollectionTimeSlotId equals _collectionSlot.CollectionTimeSlotId
                                   join _district in _sqlServerContext.Districts.AsNoTracking()
                                   on _orders.CollectAddressThanaId equals _district.DistrictId

                                   where _history.PostedOn.Date >= requestBody.FromDate.Date
                                   && _history.PostedOn.Date < requestBody.ToDate.Date.AddDays(1)
                                   && _history.Status.Equals(44)

                                   select new
                                   {
                                       OrderId = _orders.Id,
                                       OrderDate = _orders.OrderDate,
                                       PostedOn = _history.PostedOn,
                                       MerchantId = _history.MerchantId,
                                       CompanyName = _courierUsers.CompanyName,
                                       Name = _deliveryUsers.Name,
                                       DeliveryUserId = _deliveryUsers.Id,
                                       SlotName = _collectionSlot.SlotName,
                                       District = _district.District
                                   }).ToListAsync();

            var groupedData = totalData.GroupBy(b => b.MerchantId).Select(b => new
            {
                MerchantId = b.Key,
                CompanyName = b.FirstOrDefault().CompanyName,
                TotalRider = b.Select(r => r.DeliveryUserId).Distinct().Count()
            }).OrderByDescending(o => o.TotalRider).ToList();

            var responseModel = new
            {
                CountData = groupedData,
                DetailsData = totalData
            };

            return responseModel;
        }
    }
}
