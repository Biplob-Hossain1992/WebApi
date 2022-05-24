using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using EFCore.BulkExtensions;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System.Data.SqlClient;
using AdCourier.Domain.Entities.ViewModel.CollectorAssign;
using AdCourier.Domain.Entities;
using Microsoft.Extensions.Options;
using Dapper;
using System.Data;
using AdCourier.Domain.Entities.BodyModel.OrderTracking;
using AdCourier.Domain.Entities.ViewModel.DeliveryManAssign;
using AdCourier.Domain.Entities.ViewModel.CodCollection;
using AdCourier.Domain.Entities.ProcedureDataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.Utility;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;

namespace AdCourier.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository, IOrderHistoryRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ConnectionStringList _connectionStrings;
        public OrderRepository(SqlServerContext sqlServerContext, IHttpContextAccessor httpContextAccessor,
            IOptions<ConnectionStringList> connectionStrings)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _httpContextAccessor = httpContextAccessor;
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<CourierOrderStatusHistory> AddCourierOrderHistory(CourierOrderStatusHistory courierOrderStatusHistory)
        {
            await _sqlServerContext.CourierOrderStatusHistory.AddAsync(courierOrderStatusHistory);
            await _sqlServerContext.SaveChangesAsync();
            return courierOrderStatusHistory;
        }

        public async Task<IEnumerable<CourierOrderStatusHistory>> AddCourierOrderHistoryBulk(List<CourierOrderStatusHistory> courierOrderStatusHistory)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.CourierOrderStatusHistory.AddRangeAsync(courierOrderStatusHistory);
            await _sqlServerContext.SaveChangesAsync();
            return courierOrderStatusHistory;
        }

        public async Task<IEnumerable<CourierOrders>> AddOrdersBulk(List<CourierOrders> courierOrders)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.CourierOrders.AddRangeAsync(courierOrders);
            await _sqlServerContext.SaveChangesAsync();
            return courierOrders;

            //var bulkConfig = new BulkConfig { PreserveInsertOrder = false, SetOutputIdentity = false };


            //await _sqlServerContext.BulkInsertAsync(courierOrders);

        }

        public bool GetDistrictInfo(int id)
        {

            var data = from d in _sqlServerContext.Districts
                       where d.DistrictId.Equals(id)
                       select d.IsActiveForCorona;
            return data.FirstOrDefault();

        }

        public async Task<CourierOrders> AddOrder(CourierOrders courierOrders)
        {

            //await _sqlServerContext.CourierOrders.AddAsync(courierOrders);
            //await _sqlServerContext.SaveChangesAsync();
            //return courierOrders;



            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@CustomerName", value: courierOrders.CustomerName, dbType: DbType.String);
                parameter.Add(name: "@Mobile", value: courierOrders.Mobile, dbType: DbType.String);
                parameter.Add(name: "@OtherMobile", value: courierOrders.OtherMobile, dbType: DbType.String);
                parameter.Add(name: "@Address", value: courierOrders.Address, dbType: DbType.String);
                parameter.Add(name: "@DistrictId", value: courierOrders.DistrictId, dbType: DbType.Int32);
                parameter.Add(name: "@ThanaId", value: courierOrders.ThanaId, dbType: DbType.Int32);
                parameter.Add(name: "@AreaId", value: courierOrders.AreaId, dbType: DbType.Int32);
                parameter.Add(name: "@PaymentType", value: courierOrders.PaymentType, dbType: DbType.String);
                parameter.Add(name: "@OrderType", value: courierOrders.OrderType, dbType: DbType.String);
                parameter.Add(name: "@Weight", value: courierOrders.Weight, dbType: DbType.String);
                parameter.Add(name: "@CollectionName", value: courierOrders.CollectionName, dbType: DbType.String);
                parameter.Add(name: "@CollectionAmount", value: courierOrders.CollectionAmount, dbType: DbType.Decimal);
                parameter.Add(name: "@DeliveryCharge", value: courierOrders.DeliveryCharge, dbType: DbType.Decimal);
                parameter.Add(name: "@IsActive", value: courierOrders.IsActive, dbType: DbType.Boolean);
                parameter.Add(name: "@Status", value: courierOrders.Status, dbType: DbType.Int32);
                parameter.Add(name: "@PostedOn", value: courierOrders.PostedOn, dbType: DbType.DateTime);
                parameter.Add(name: "@PostedBy", value: courierOrders.PostedBy, dbType: DbType.Int32);
                parameter.Add(name: "@UpdatedBy", value: courierOrders.UpdatedBy, dbType: DbType.Int32);
                parameter.Add(name: "@UpdatedOn", value: courierOrders.UpdatedOn, dbType: DbType.DateTime);
                parameter.Add(name: "@PodNumber", value: courierOrders.PodNumber, dbType: DbType.String);
                parameter.Add(name: "@MerchantId", value: courierOrders.MerchantId, dbType: DbType.Int32);
                parameter.Add(name: "@Comment", value: courierOrders.Comment, dbType: DbType.String);
                parameter.Add(name: "@OrderDate", value: courierOrders.OrderDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ConfirmationDate", value: courierOrders.ConfirmationDate, dbType: DbType.DateTime);
                parameter.Add(name: "@BreakableCharge", value: courierOrders.BreakableCharge, dbType: DbType.Decimal);
                parameter.Add(name: "@ThirdPartyCourierInfo", value: courierOrders.ThirdPartyCourierInfo, dbType: DbType.String);
                parameter.Add(name: "@IsConfirmedBy", value: courierOrders.IsConfirmedBy, dbType: DbType.String);
                parameter.Add(name: "@Note", value: courierOrders.Note, dbType: DbType.String);
                parameter.Add(name: "@CodCharge", value: courierOrders.CodCharge, dbType: DbType.Decimal);
                parameter.Add(name: "@CourierId", value: courierOrders.CourierId, dbType: DbType.Int32);
                parameter.Add(name: "@CollectionCharge", value: courierOrders.CollectionCharge, dbType: DbType.Decimal);
                parameter.Add(name: "@ReturnCharge", value: courierOrders.ReturnCharge, dbType: DbType.Decimal);
                parameter.Add(name: "@PackagingName", value: courierOrders.PackagingName, dbType: DbType.String);
                parameter.Add(name: "@PackagingCharge", value: courierOrders.PackagingCharge, dbType: DbType.Decimal);
                parameter.Add(name: "@CollectAddress", value: courierOrders.CollectAddress, dbType: DbType.String);
                parameter.Add(name: "@IsDownloaded", value: courierOrders.IsDownloaded, dbType: DbType.Boolean);
                parameter.Add(name: "@HubName", value: courierOrders.HubName, dbType: DbType.String);
                parameter.Add(name: "@OrderFrom", value: courierOrders.OrderFrom, dbType: DbType.String);
                parameter.Add(name: "@CourierCharge", value: courierOrders.CourierCharge, dbType: DbType.Decimal);
                parameter.Add(name: "@IsOpenBox", value: courierOrders.IsOpenBox, dbType: DbType.Boolean);
                parameter.Add(name: "@IsAutoProcess", value: courierOrders.IsAutoProcess, dbType: DbType.Boolean);
                parameter.Add(name: "@IsTakaCollectionFromCourier", value: courierOrders.IsTakaCollectionFromCourier, dbType: DbType.Boolean);
                parameter.Add(name: "@DeliveryRangeId", value: courierOrders.DeliveryRangeId, dbType: DbType.Int32);
                parameter.Add(name: "@WeightRangeId", value: courierOrders.WeightRangeId, dbType: DbType.Int32);
                parameter.Add(name: "@ProductType", value: courierOrders.ProductType, dbType: DbType.String);
                parameter.Add(name: "@CollectAddressDistrictId", value: courierOrders.CollectAddressDistrictId, dbType: DbType.Int32);
                parameter.Add(name: "@CollectAddressThanaId", value: courierOrders.CollectAddressThanaId, dbType: DbType.Int32);

                parameter.Add(name: "@MerchantDeliveryDate", value: courierOrders.MerchantDeliveryDate, dbType: DbType.DateTime);
                parameter.Add(name: "@MerchantCollectionDate", value: courierOrders.MerchantCollectionDate, dbType: DbType.DateTime);
                parameter.Add(name: "@OfficeDrop", value: courierOrders.OfficeDrop, dbType: DbType.Boolean);
                parameter.Add(name: "@ActualPackagePrice", value: courierOrders.ActualPackagePrice, dbType: DbType.Decimal);
                parameter.Add(name: "@CollectionTimeSlotId", value: courierOrders.CollectionTimeSlotId, dbType: DbType.Int32);
                parameter.Add(name: "@CollectionTime", value: courierOrders.CollectionTime, dbType: DbType.DateTime);
                parameter.Add(name: "@OfferType", value: courierOrders.OfferType, dbType: DbType.String);
                parameter.Add(name: "@RelationType", value: courierOrders.RelationType, dbType: DbType.String);
                parameter.Add(name: "@ServiceType", value: courierOrders.ServiceType, dbType: DbType.String);
                parameter.Add(name: "@Version", value: courierOrders.Version, dbType: DbType.String);
                parameter.Add(name: "@IsHeavyWeight", value: courierOrders.IsHeavyWeight, dbType: DbType.Boolean);
                parameter.Add(name: "@CouponIds", value: courierOrders.CouponIds, dbType: DbType.String);
                parameter.Add(name: "@VoucherDiscount", value: courierOrders.VoucherDiscount, dbType: DbType.Decimal);
                parameter.Add(name: "@VoucherCode", value: courierOrders.VoucherCode, dbType: DbType.String);
                parameter.Add(name: "@VoucherDeliveryRangeId", value: courierOrders.VoucherDeliveryRangeId, dbType: DbType.Int32);
                parameter.Add(name: "@PaymentServiceType", value: courierOrders.PaymentServiceType, dbType: DbType.Byte);
                parameter.Add(name: "@PaymentServiceCharge", value: courierOrders.PaymentServiceCharge, dbType: DbType.Decimal);

                parameter.Add(name: "@PaymentServiceTypeVerify", value: courierOrders.PaymentServiceTypeVerify, dbType: DbType.String);
                parameter.Add(name: "@PaymentServiceTypeMerchantVerify", value: courierOrders.PaymentServiceTypeMerchantVerify, dbType: DbType.String);
                parameter.Add(name: "@OpenBoxCharge", value: courierOrders.OpenBoxCharge, dbType: DbType.Decimal);

                var data = await connection.QueryAsync<CourierOrders>(
                    sql: @"[Dt].[USP_InsertCourierOrders]",
                    commandType: CommandType.StoredProcedure,
                    param: parameter
                    );

                return data.FirstOrDefault();

            }
        }

        public async Task<DeliveryChargeDetails_test> GetDeliveryChargeDetailsPrice_test(DeliveryChargeDetails_test deliveryChargeDetails)
        {
            IQueryable<DeliveryChargeDetails_test> data = from d in _sqlServerContext.DeliveryChargeDetails_test
                                                          where d.DistrictId.Equals(deliveryChargeDetails.DistrictId)
                                                     && d.ThanaId.Equals(deliveryChargeDetails.ThanaId)
                                                     && d.AreaId.Equals(deliveryChargeDetails.AreaId)
                                                     && d.WeightRangeId.Equals(deliveryChargeDetails.WeightRangeId)
                                                     && d.DeliveryRangeId.Equals(deliveryChargeDetails.DeliveryRangeId)
                                                          select d;
            return await data.FirstOrDefaultAsync();
        }
        public async Task<DeliveryChargeDetails> GetDeliveryChargeDetailsPrice(DeliveryChargeDetails deliveryChargeDetails)
        {

            IQueryable<DeliveryChargeDetails> data = from d in _sqlServerContext.DeliveryChargeDetails
                                                     where d.DistrictId.Equals(deliveryChargeDetails.DistrictId)
                                                     && d.ThanaId.Equals(deliveryChargeDetails.ThanaId)
                                                     && d.AreaId.Equals(deliveryChargeDetails.AreaId)
                                                     && d.WeightRangeId.Equals(2)
                                                     && d.DeliveryRangeId.Equals(deliveryChargeDetails.DeliveryRangeId)
                                                     && d.ServiceType.Equals(deliveryChargeDetails.ServiceType)
                                                     select new DeliveryChargeDetails
                                                     {
                                                         CourierId = d.CourierId
                                                     };

            return await data.FirstOrDefaultAsync();
        }

        public async Task<dynamic> GetChangeDeliveryChargeDetailsLog(ChangeDeliveryChargeDetailsLog changeLog)
        {
            IQueryable<dynamic> data = (from d in _sqlServerContext.ChangeDeliveryChargeDetailsLog.Where(c => c.DistrictId == changeLog.DistrictId && c.ThanaId == changeLog.ThanaId && c.AreaId == changeLog.AreaId && c.ServiceType == changeLog.ServiceType.ToLower())
                                       join r in _sqlServerContext.DeliveryRange on d.NewDeliveryRangeId equals r.Id
                                       join n in _sqlServerContext.Couriers on d.NewCourierId equals n.CourierId
                                       join o in _sqlServerContext.Couriers on d.OldCourierId equals o.CourierId
                                       join u in _sqlServerContext.Users on d.UserId equals u.UserId
                                       select new
                                       {
                                           DeliveryServiceType = r.Name,
                                           ChangedDate = d.ChangedDate,
                                           OldCourierId = d.OldCourierId,
                                           OldCourierName = o.CourierName,
                                           NewCourierId = d.NewCourierId,
                                           NewCourierName = n.CourierName,
                                           OldIsActive = d.OldIsActive,
                                           NewIsActive = d.NewIsActive,
                                           UserName = u.FullName
                                       }).OrderByDescending(o => o.ChangedDate);
            return await data.ToListAsync();
        }

        public async Task<CourierOrderDetailsViewModel> LoadCourierOrder(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {

            CourierOrderDetailsViewModel responseData = new CourierOrderDetailsViewModel();

            //string merchantId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            string role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            if (role.ToLower() == "DeliveryTiger".ToLower())
            {
                responseData = await RetriveOrderList(loadCourierOrderBodyModel);
            }
            if (role.ToLower() == "DeliveryTigerAdmin".ToLower())
            {
                responseData = await RetriveOrderList_Admin(loadCourierOrderBodyModel);
            }
            //responseData = await RetriveOrderList(loadCourierOrderBodyModel);

            return responseData;


        }

        public async Task<List<OrderModel>> GetPriorityOrders()
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var data = await connection.QueryAsync<OrderModel>(
                        sql: @"[DT].[USP_GetPriorityOrders]",
                        param: null,
                        commandType: CommandType.StoredProcedure);

                return data.ToList();

            }
        }

        public async Task<CodCollection> GetAllOrders(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@CourierUserId", value: loadCourierOrderBodyModel.CourierUserId, dbType: DbType.String);
                parameter.Add(name: "@FromDate", value: loadCourierOrderBodyModel.FromDate.ToString("yyyy-MM-dd"), dbType: DbType.String);
                parameter.Add(name: "@ToDate", value: loadCourierOrderBodyModel.ToDate.ToString("yyyy-MM-dd"), dbType: DbType.String);
                parameter.Add(name: "@StatusGroup", value: loadCourierOrderBodyModel.StatusGroup.FirstOrDefault().ToString(), dbType: DbType.String);
                parameter.Add(name: "@OrderIds", value: loadCourierOrderBodyModel.OrderIds, dbType: DbType.String);
                parameter.Add(name: "@Mobile", value: loadCourierOrderBodyModel.Mobile, dbType: DbType.String);
                parameter.Add(name: "@CollectionName", value: loadCourierOrderBodyModel.CollectionName, dbType: DbType.String);
                parameter.Add(name: "@OrderType", value: loadCourierOrderBodyModel.OrderType, dbType: DbType.String);
                parameter.Add(name: "@Index", value: loadCourierOrderBodyModel.Index, dbType: DbType.String);
                parameter.Add(name: "@Count", value: loadCourierOrderBodyModel.Count, dbType: DbType.String);

                var codCollection = new CodCollection();
                var multi = await connection.QueryMultipleAsync(sql: @"DT.USP_GetAllOrders",
                    param: parameter,
                    commandType: CommandType.StoredProcedure);
                {
                    var codCollectionDetails = multi.Read<CodCollectionDetails>().ToList();
                    var codCollectionTotal = multi.Read<CodCollectionTotal>().FirstOrDefault();

                    codCollection.CodCollectionDetails = codCollectionDetails;
                    codCollection.CodCollectionTotal = codCollectionTotal;
                }
                return codCollection;
            }
        }
        public async Task<CodCollection> GetCodCollections(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@CourierUserId", value: loadCourierOrderBodyModel.CourierUserId, dbType: DbType.String);
                parameter.Add(name: "@FromDate", value: loadCourierOrderBodyModel.FromDate.ToString("yyyy-MM-dd"), dbType: DbType.String);
                parameter.Add(name: "@ToDate", value: loadCourierOrderBodyModel.ToDate.ToString("yyyy-MM-dd"), dbType: DbType.String);
                parameter.Add(name: "@Status", value: loadCourierOrderBodyModel.Status, dbType: DbType.String);
                parameter.Add(name: "@StatusGroup", value: loadCourierOrderBodyModel.StatusGroup.FirstOrDefault().ToString(), dbType: DbType.String);
                parameter.Add(name: "@OrderIds", value: loadCourierOrderBodyModel.OrderIds, dbType: DbType.String);
                parameter.Add(name: "@Mobile", value: loadCourierOrderBodyModel.Mobile, dbType: DbType.String);
                parameter.Add(name: "@CollectionName", value: loadCourierOrderBodyModel.CollectionName, dbType: DbType.String);
                parameter.Add(name: "@Index", value: loadCourierOrderBodyModel.Index, dbType: DbType.String);
                parameter.Add(name: "@Count", value: loadCourierOrderBodyModel.Count, dbType: DbType.String);

                var codCollection = new CodCollection();
                var multi = await connection.QueryMultipleAsync(sql: @"DT.USP_GetCodCollectionDetails",
                    param: parameter,
                    commandType: CommandType.StoredProcedure);
                {
                    var codCollectionDetails = multi.Read<CodCollectionDetails>().ToList();
                    var codCollectionTotal = multi.Read<CodCollectionTotal>().FirstOrDefault();

                    codCollection.CodCollectionDetails = codCollectionDetails;
                    codCollection.CodCollectionTotal = codCollectionTotal;
                }
                return codCollection;
            }
        }
        public async Task<ServiceAmount> LoadCourierOrderAmountDetailsV2(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@CourierUserId", value: loadCourierOrderBodyModel.CourierUserId, dbType: DbType.String);
                parameter.Add(name: "@FromDate", value: loadCourierOrderBodyModel.FromDate.ToString("yyyy-MM-dd"), dbType: DbType.String);
                parameter.Add(name: "@ToDate", value: loadCourierOrderBodyModel.ToDate.ToString("yyyy-MM-dd"), dbType: DbType.String);
                parameter.Add(name: "@StatusGroup", value: loadCourierOrderBodyModel.StatusGroup.FirstOrDefault().ToString(), dbType: DbType.String);
                parameter.Add(name: "@OrderIds", value: loadCourierOrderBodyModel.OrderIds, dbType: DbType.String);
                parameter.Add(name: "@Mobile", value: loadCourierOrderBodyModel.Mobile, dbType: DbType.String);
                parameter.Add(name: "@CollectionName", value: loadCourierOrderBodyModel.CollectionName, dbType: DbType.String);
                parameter.Add(name: "@Index", value: loadCourierOrderBodyModel.Index, dbType: DbType.String);
                parameter.Add(name: "@Count", value: loadCourierOrderBodyModel.Count, dbType: DbType.String);

                var serviceAmount = new ServiceAmount();
                var multi = await connection.QueryMultipleAsync(sql: @"DT.USP_LoadCourierOrderAmountDetails",
                    param: parameter,
                    commandType: CommandType.StoredProcedure);
                {
                    var serviceAmountDetails = multi.Read<ServiceAmountDetails>().ToList();
                    var serviceAmountTotal = multi.Read<ServiceAmountTotal>().FirstOrDefault();

                    serviceAmount.ServiceAmountDetails = serviceAmountDetails;
                    serviceAmount.ServiceAmountTotal = serviceAmountTotal;

                }
                return serviceAmount;
            }
        }
        public async Task<CourierAmountDetailsResponse> LoadCourierOrderAmountDetails(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            string merchantId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            int merchantIdValue = Convert.ToInt32(merchantId);
            string role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            IQueryable<CourierOrderAmountDetails> courierOrdersAMount;


            // var amountResponse = await _sqlServerContext.CourierOrders.Where(g => g.MerchantId == merchantIdValue).Select(x => new { x.BreakableCharge, x.CodCharge, x.DeliveryCharge }).ToListAsync();
            int totalData = (from courier in _sqlServerContext.CourierOrders
                             where courier.MerchantId == merchantIdValue
                             select courier
                           ).Count();

            var amountResponse = await (from status in _sqlServerContext.CourierOrderStatus.Where(p => p.StatusType.ToLower() == "unpaid")
                                        join courierOrder in _sqlServerContext.CourierOrders.Distinct().Where(j => j.MerchantId == merchantIdValue)
                                        on status.StatusId equals courierOrder.Status
                                        //      join courierUsers in _sqlServerContext.CourierUsers on courierOrder.MerchantId equals courierUsers.CourierUserId
                                        //, courierUsers.CollectionCharge, courierUsers.ReturnCharge 
                                        let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? courierOrder.ReturnCharge : 0
                                        select new { courierOrder.CourierOrdersId, courierOrder.BreakableCharge, courierOrder.CodCharge, courierOrder.DeliveryCharge, returnCharge, courierOrder.CollectionCharge }).Distinct().ToListAsync();



            var sumOfTotalAdAmount = amountResponse.Sum(x => x.BreakableCharge + x.CodCharge + x.DeliveryCharge + x.returnCharge + x.CollectionCharge);
            var totalDataCount = amountResponse.Count();
            if (loadCourierOrderBodyModel.OrderIds.Trim() == "")
            {
                courierOrdersAMount = (from courierOrder in _sqlServerContext.CourierOrders.OrderByDescending(y => y.PostedOn)
                                       join status in _sqlServerContext.CourierOrderStatus on courierOrder.Status equals status.StatusId
                                       join courierUsers in _sqlServerContext.CourierUsers on courierOrder.MerchantId equals courierUsers.CourierUserId
                                       where courierOrder.MerchantId == merchantIdValue
                                       //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? courierUsers.CollectionCharge : 0
                                       let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? courierOrder.ReturnCharge : 0


                                       select new CourierOrderAmountDetails
                                       {
                                           CustomerName = courierOrder.CustomerName,
                                           CourierOrdersId = courierOrder.CourierOrdersId,
                                           BreakableCharge = courierOrder.BreakableCharge,
                                           CollectionAmount = courierOrder.CollectionAmount,
                                           DeliveryCharge = courierOrder.DeliveryCharge,
                                           CODCharge = courierOrder.CodCharge,
                                           Status = status.StatusType,
                                           CollectionCharge = courierOrder.CollectionCharge,
                                           ReturnCharge = returnCharge
                                       }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
            }
            else
            {
                courierOrdersAMount = (from courierOrder in _sqlServerContext.CourierOrders.OrderByDescending(y => y.PostedOn)
                                       join status in _sqlServerContext.CourierOrderStatus on courierOrder.Status equals status.StatusId
                                       join courierUsers in _sqlServerContext.CourierUsers on courierOrder.MerchantId equals courierUsers.CourierUserId
                                       where courierOrder.CourierOrdersId.Trim().ToLower() == loadCourierOrderBodyModel.OrderIds.Trim().ToLower() && courierOrder.MerchantId == merchantIdValue
                                       // let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? courierUsers.CollectionCharge : 0
                                       let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? courierOrder.ReturnCharge : 0



                                       select new CourierOrderAmountDetails
                                       {
                                           CustomerName = courierOrder.CustomerName,
                                           CourierOrdersId = courierOrder.CourierOrdersId,
                                           BreakableCharge = courierOrder.BreakableCharge,
                                           CollectionAmount = courierOrder.CollectionAmount,
                                           DeliveryCharge = courierOrder.DeliveryCharge,
                                           CODCharge = courierOrder.CodCharge,
                                           Status = status.StatusType,

                                           CollectionCharge = courierOrder.CollectionCharge,
                                           ReturnCharge = returnCharge
                                       });
            }
            return new CourierAmountDetailsResponse
            {
                TotalData = totalData,
                TotalAmount = sumOfTotalAdAmount,
                TotalDataCount = totalDataCount,
                CourierOrderAmountDetails = courierOrdersAMount.ToList()
            };
        }

        public async Task<int> UpdateReferrer(string referrerMobile)
        {
            var referrerInfo = await _sqlServerContext.CourierUsers.Where(x => x.Mobile.Equals(referrerMobile) && x.ReferrerIsActive.Equals(false)).FirstOrDefaultAsync();

            if (referrerInfo != null)
            {
                referrerInfo.ReferrerIsActive = true;
                var data = _sqlServerContext.CourierUsers.Update(referrerInfo);
                if (data != null)
                {
                    return 1;
                }
            }
            return 0;
        }

        public int UpdateOrderHistory(string courierOrderId, CourierOrderStatusHistoryViewModel courierOrderStatusHistory)
        {
            int[] courierArray = { 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 69};

            int startIndex = 3;
            int endIndex = courierOrderId.Length - 3;
            int id = Convert.ToInt32(courierOrderId.Substring(startIndex, endIndex));

            CourierOrders courierOrder = (from x in _sqlServerContext.CourierOrders
                                          where x.Id.Equals(id)
                                          select x).FirstOrDefault();

            //CourierOrders courierOrder = (from x in _sqlServerContext.CourierOrders
            //                              where x.CourierOrdersId.Trim().ToLower() == courierOrderId.Trim().ToLower()
            //                              select x).FirstOrDefault();
            if (courierOrder != null)
            {
                if (courierArray.Contains(courierOrderStatusHistory.CourierId))
                {
                    courierOrder.PodNumber = courierOrderStatusHistory.PodNumber;
                }

                courierOrder.Status = courierOrderStatusHistory.Status;
                courierOrder.UpdatedBy = courierOrderStatusHistory.PostedBy;
                courierOrder.MerchantId = courierOrderStatusHistory.MerchantId;
                courierOrder.UpdatedOn = courierOrderStatusHistory.PostedOn;
                courierOrder.IsConfirmedBy = courierOrderStatusHistory.IsConfirmedBy;

                if (courierOrderStatusHistory.CourierId != 35)
                {
                    courierOrder.CourierId = courierOrderStatusHistory.CourierId;
                }

                courierOrder.HubName = courierOrderStatusHistory.HubName;
                courierOrder.Comment = courierOrderStatusHistory.Comment;
                courierOrder.CourierCharge = courierOrderStatusHistory.CourierCharge;
                if (courierOrderStatusHistory.IsConfirmedBy.Trim().ToLower() == "deliveryman")
                {
                    courierOrder.DeliveryUserId = courierOrderStatusHistory.PostedBy;
                }
                if (courierOrderStatusHistory.Status == 40) //&& courierOrderStatusHistory.IsConfirmedBy.Trim().ToLower() == "deliveryman"
                {
                    courierOrder.DeliveryUserId = 0;
                }
                if (courierOrderStatusHistory.Status == 41 && courierOrderStatusHistory.IsConfirmedBy.Trim().ToLower() == "deliveryman")
                {
                    courierOrder.RiderAcceptDate = DateTime.Now;
                }
                if (courierOrderStatusHistory.Status == 45 && courierOrderStatusHistory.IsConfirmedBy.Trim().ToLower() == "deliveryman")
                {
                    courierOrder.RiderDeliveredDate = DateTime.Now;
                }
                if (courierOrderStatusHistory.Status == 53) //&& courierOrderStatusHistory.IsConfirmedBy.Trim().ToLower() == "deliveryman"
                {
                    courierOrder.DeliveryUserId = 0;
                }
                if (courierOrderStatusHistory.Status.Equals(7))
                {
                    courierOrder.ExpectedDeliveryDate = expectedDeliveryDateCalculator(courierOrder.DeliveryRangeId);
                }

            }
            var resposne = _sqlServerContext.SaveChanges();
            return resposne;
        }

        private DateTime? expectedDeliveryDateCalculator(int deliveryRangeId)
        {
            var deliveryRange = _sqlServerContext.DeliveryRange.Where(z => z.Id.Equals(deliveryRangeId)).FirstOrDefault();
            if (deliveryRange != null)
            {
                return DateTime.Now.AddHours(deliveryRange.AddCddDate);
            }
            else
            {
                return null;
            }


            //switch (deliveryRangeId)
            //{
            //    case 2:
            //        return DateTime.Now.AddHours(54);
            //    case 7:
            //        return DateTime.Now.AddHours(30);
            //    case 4:
            //        return DateTime.Now.AddHours(130);
            //    case 3:
            //        return DateTime.Now.AddHours(106);
            //    case 10:
            //        return DateTime.Now.AddHours(106);
            //    case 11:
            //        return DateTime.Now.AddHours(150);
            //    case 9:
            //        return DateTime.Now.AddHours(150);
            //    default:
            //        return null;
            //}
        }

        public async Task<CourierOrderDetailsViewModel> RetriveOrderList_Admin(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {

            //IQueryable<CourierOrderViewModel> courierOrderViewModel2;

            IQueryable<CourierAssign> courierAssignData2 = from assign in _sqlServerContext.CourierAssign.AsNoTracking()
                                                           where assign.IsActive.Equals(true)
                                                           select assign;
            var courierAssignData = courierAssignData2.ToList();


            int totalOrderCount = 0;
            if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001" && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001" && loadCourierOrderBodyModel.Status != -1 && loadCourierOrderBodyModel.CourierUserId != -1)
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date && d.Status == loadCourierOrderBodyModel.Status && d.MerchantId == loadCourierOrderBodyModel.CourierUserId).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     where d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date && d.Status == loadCourierOrderBodyModel.Status && d.MerchantId == loadCourierOrderBodyModel.CourierUserId

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()


                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0

                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusEng = status.StatusNameEng,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CourierCharge = d.CourierCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001" && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001" && loadCourierOrderBodyModel.Status != -1)
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date && d.Status == loadCourierOrderBodyModel.Status).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     where d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date && d.Status == loadCourierOrderBodyModel.Status

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()


                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //  let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0

                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusEng = status.StatusNameEng,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001" && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001" && loadCourierOrderBodyModel.CourierUserId != -1)
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date && d.MerchantId == loadCourierOrderBodyModel.CourierUserId).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     where d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date && d.MerchantId == loadCourierOrderBodyModel.CourierUserId

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()


                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //  let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0

                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusEng = status.StatusNameEng,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001" && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001" && loadCourierOrderBodyModel.StatusList != new int[] { 0 })
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => loadCourierOrderBodyModel.StatusList.Contains(d.Status) && d.UpdatedOn.Date >= loadCourierOrderBodyModel.FromDate.Date && d.UpdatedOn.Date <= loadCourierOrderBodyModel.ToDate.Date).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()

                                     where loadCourierOrderBodyModel.StatusList.Contains(d.Status) && d.UpdatedOn.Date >= loadCourierOrderBodyModel.FromDate.Date && d.UpdatedOn.Date <= loadCourierOrderBodyModel.ToDate.Date

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()


                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()

                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     // let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,

                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001" && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001")
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     where d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()


                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //  let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0

                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusEng = status.StatusNameEng,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             //RedxHubName = districtList.Select(y => y.RedxHubName).FirstOrDefault() ?? ""
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.Status != -1 && loadCourierOrderBodyModel.CourierUserId != -1)
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.Status == loadCourierOrderBodyModel.Status && d.MerchantId == loadCourierOrderBodyModel.CourierUserId).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()

                                     where d.Status == loadCourierOrderBodyModel.Status && d.MerchantId == loadCourierOrderBodyModel.CourierUserId

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()


                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     // let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,

                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.OrderIds != "")
            {

                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.CourierOrdersId.ToLower().Trim() == loadCourierOrderBodyModel.OrderIds.ToLower().Trim()).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId

                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     where d.CourierOrdersId == loadCourierOrderBodyModel.OrderIds

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()

                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //        let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,

                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",

                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,

                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,

                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName

                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.Status != -1)
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.Status == loadCourierOrderBodyModel.Status).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()

                                     where d.Status == loadCourierOrderBodyModel.Status

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()


                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()

                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     // let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,

                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (!loadCourierOrderBodyModel.StatusList.Contains(-1))
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => loadCourierOrderBodyModel.StatusList.Contains(d.Status)).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()

                                     where loadCourierOrderBodyModel.StatusList.Contains(d.Status)

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()


                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()

                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     // let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,

                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.CourierUserId != -1)
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.MerchantId == loadCourierOrderBodyModel.CourierUserId).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()

                                     where d.MerchantId == loadCourierOrderBodyModel.CourierUserId

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()


                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     // let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,

                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.PodNumber != "")
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.PodNumber == loadCourierOrderBodyModel.PodNumber).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()

                                     where d.PodNumber == loadCourierOrderBodyModel.PodNumber

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()


                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     // let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,

                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (loadCourierOrderBodyModel.CollectionName != "")
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.CollectionName == loadCourierOrderBodyModel.CollectionName).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()

                                     where d.CollectionName == loadCourierOrderBodyModel.CollectionName

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()


                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     // let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName
                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {
                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,

                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else
            {
                totalOrderCount = await _sqlServerContext.CourierOrders.CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId).AsNoTracking().AsQueryable()
                                     let thanaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.ThanaId).AsNoTracking().AsQueryable()
                                     let areaList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.AreaId).AsNoTracking().AsQueryable()

                                     let AreaCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.CourierId).FirstOrDefault()
                                     let AreaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.AreaId).Select(s => s.PremiumCourierId).FirstOrDefault()
                                     let ThanaCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.CourierId).FirstOrDefault()
                                     let ThanaPremiumCourierId = courierAssignData.Where(x => x.DistrictId == d.ThanaId).Select(s => s.PremiumCourierId).FirstOrDefault()


                                     //let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         StatusType = status.StatusType,
                                         HubName = d.HubName,
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaPostalCode = thanaList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = areaList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId,
                                             AssignedExpressCourierId = AreaPremiumCourierId == 0 ? ThanaPremiumCourierId : AreaPremiumCourierId,
                                             AssignedCourierId = AreaCourierId == 0 ? ThanaCourierId : AreaCourierId,
                                             RedxHubName = thanaList.FirstOrDefault(x => x.RedxHubName != "").RedxHubName

                                             //AssignedThanaExpressCourierId = ThanaPremiumCourierId,
                                             //AssignedThanaCourierId = ThanaCourierId,
                                             //AssignedAreaExpressCourierId = AreaPremiumCourierId,
                                             //AssignedAreaCourierId = AreaCourierId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             IsOpenBox = d.IsOpenBox,
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName,
                                             OrderFrom = d.OrderFrom
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CourierCharge = d.CourierCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge,// users.ReturnCharge
                                             PackagingName = d.PackagingName,
                                             PackagingCharge = d.PackagingCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             BkashNumber = users.BkashNumber,
                                             CompanyName = users.CompanyName,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress
                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms,
                                             IsCustomerSms = users.IsCustomerSms,
                                             IsCustomerEmail = users.IsCustomerEmail
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
        }
        public async Task<CourierOrderDetailsViewModel> RetriveOrderList(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var editStatus = new int[] { 0, 1, 4, 5, 6 };

            string merchantId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            int merchantValue = Convert.ToInt32(merchantId);

            var amountResponse = await _sqlServerContext.CourierOrders.Where(h => h.MerchantId == merchantValue).Select(b => new { b.CollectionAmount, b.Status }).ToListAsync();

            decimal totalCollectionAmount = amountResponse.Select(c => c.CollectionAmount).Sum();
            decimal totalOrderCount = 0;
            if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId > 0)
            {

                totalOrderCount = await _sqlServerContext.CourierOrders
                    .Where(d => d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                    && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                    && d.Status != 2
                    && d.MerchantId == merchantValue).CountAsync();

                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                                     && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                                     && d.MerchantId == merchantValue
                                     && d.Status != 2

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0

                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         StatusEng = status.StatusNameEng,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {
                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId

                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;

            }
            else if (loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.CourierUserId > 0)
            {


                totalOrderCount = await _sqlServerContext.CourierOrders
                    .Where(d => d.Status == loadCourierOrderBodyModel.Status
                    && d.Status != 2
                    && d.MerchantId == merchantValue).CountAsync();

                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where d.Status == loadCourierOrderBodyModel.Status
                                     && d.MerchantId == merchantValue
                                     && d.Status != 2

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         //ButtonFlag = d.Status.Equals(0) ? true : false,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {
                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }


            else if (loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.CourierUserId > 0)
            {


                totalOrderCount = await _sqlServerContext.CourierOrders
                    .Where(d => d.Status == loadCourierOrderBodyModel.Status
                && d.MerchantId == merchantValue
                && d.Status != 2
                && d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date).CountAsync();

                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where d.MerchantId == merchantValue
                                     && d.Status != 2
                                     && d.Status == loadCourierOrderBodyModel.Status
                                     && d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                                     && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date

                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         //ButtonFlag = d.Status.Equals(0) ? true : false,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {
                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }


            else if (!loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId > 0)
            {


                totalOrderCount = await (from o in _sqlServerContext.CourierOrders
                                         join s in _sqlServerContext.CourierOrderStatus on o.Status equals s.StatusId
                                         where o.MerchantId.Equals(merchantValue)
                                         && loadCourierOrderBodyModel.StatusGroup.Contains(s.DashboardStatusGroup)
                                         && o.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                                         && o.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                                         && o.Status != 2
                                         select o.CourierOrdersId).CountAsync();

                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where loadCourierOrderBodyModel.StatusGroup.Contains(status.DashboardStatusGroup)
                                     && d.MerchantId == merchantValue
                                     && d.Status != 2
                                     && d.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                                     && d.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         //ButtonFlag = d.Status.Equals(0) ? true : false,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {
                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }

            else if (!loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId > 0)
            {


                totalOrderCount = await (from o in _sqlServerContext.CourierOrders
                                         join s in _sqlServerContext.CourierOrderStatus
                                         on o.Status equals s.StatusId
                                         where o.MerchantId.Equals(merchantValue)
                                         && o.Status != 2
                                         && loadCourierOrderBodyModel.StatusGroup.Contains(s.DashboardStatusGroup)
                                         select o.CourierOrdersId).CountAsync();

                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where loadCourierOrderBodyModel.StatusGroup.Contains(status.DashboardStatusGroup)
                                     && d.MerchantId == merchantValue
                                     && d.Status != 2
                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         //ButtonFlag = d.Status.Equals(0) ? true : false,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {
                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }

            else if (loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId > 0
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.OrderIds == "")
            {


                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.MerchantId == merchantValue && d.Status != 2).CountAsync();

                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where d.MerchantId == merchantValue
                                     && d.Status != 2
                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         //ButtonFlag = d.Status.Equals(0) ? true : false,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {
                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }

            else if (
                    loadCourierOrderBodyModel.OrderIds != ""
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.CourierUserId > 0)
            {
                totalOrderCount = await (from o in _sqlServerContext.CourierOrders
                                         where o.MerchantId.Equals(merchantValue)
                                         && o.Status != 2
                                         && o.CourierOrdersId.Equals(loadCourierOrderBodyModel.OrderIds)
                                         select o.CourierOrdersId).CountAsync();

                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where d.CourierOrdersId.Equals(loadCourierOrderBodyModel.OrderIds)
                                     && d.MerchantId == merchantValue
                                     && d.Status != 2
                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         //ButtonFlag = d.Status.Equals(0) ? true : false,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {

                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (
                    loadCourierOrderBodyModel.CollectionName != ""
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.CourierUserId > 0)
            {
                totalOrderCount = await (from o in _sqlServerContext.CourierOrders
                                         where o.MerchantId.Equals(merchantValue)
                                         && o.Status != 2
                                         && o.CollectionName.Contains(loadCourierOrderBodyModel.CollectionName)
                                         select o.CourierOrdersId).CountAsync();

                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where d.CollectionName.Contains(loadCourierOrderBodyModel.CollectionName)
                                     && d.MerchantId == merchantValue
                                     && d.Status != 2
                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         //ButtonFlag = d.Status.Equals(0) ? true : false,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {

                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else if (
                    loadCourierOrderBodyModel.Mobile != ""
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.CourierUserId > 0)
            {
                totalOrderCount = await (from o in _sqlServerContext.CourierOrders
                                         where o.MerchantId.Equals(merchantValue)
                                         && o.Status != 2
                                         && o.Mobile.Equals(loadCourierOrderBodyModel.Mobile.Trim())
                                         select o.CourierOrdersId).CountAsync();

                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where d.Mobile.Equals(loadCourierOrderBodyModel.Mobile.Trim())
                                     && d.MerchantId == merchantValue
                                     && d.Status != 2
                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //    let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         //ButtonFlag = d.Status.Equals(0) ? true : false,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {

                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {
                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;
            }
            else
            {

                totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => d.MerchantId == merchantValue && d.Status != 2).CountAsync();
                var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                     join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                     join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId
                                     join courierInfo in _sqlServerContext.Couriers on d.CourierId equals courierInfo.CourierId into responseAlldata
                                     from output in responseAlldata.DefaultIfEmpty()
                                     join hub in _sqlServerContext.Hub
                                        on d.HubName equals hub.Value into hubGroup
                                     from h in hubGroup.DefaultIfEmpty()
                                     where d.MerchantId == merchantValue
                                     && d.Status != 2
                                     let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == d.DistrictId || x.ParentId == d.DistrictId).AsNoTracking().AsQueryable()
                                     //   let collectionCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? users.CollectionCharge : 0
                                     let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0
                                     select new CourierOrderViewModel
                                     {
                                         CourierId = d.CourierId,
                                         CourierName = output.CourierName ?? "",
                                         CustomerName = d.CustomerName,
                                         Status = status.StatusNameBng,
                                         StatusId = d.Status,
                                         ButtonFlag = editStatus.Contains(d.Status) ? true : false,
                                         //ButtonFlag = d.Status.Equals(0) ? true : false,
                                         StatusType = status.StatusType,
                                         HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                                         {

                                             Name = h.Name,
                                             Value = h.Value,
                                             HubAddress = h.HubAddress,
                                             Latitude = h.Latitude,
                                             Longitude = h.Longitude,
                                             HubMobile = h.HubMobile
                                         },
                                         CourierAddressContactInfo = new CourierAddressContactInfo
                                         {
                                             Mobile = d.Mobile,
                                             OtherMobile = d.OtherMobile,
                                             Address = d.Address,
                                             DistrictName = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             ThanaName = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             AreaName = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                             DistrictNameEng = districtList.Where(x => x.DistrictId == d.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                             ThanaNameEng = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                             AreaNameEng = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                             ThanaPostalCode = districtList.Where(x => x.DistrictId == d.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             AreaPostalCode = districtList.Where(x => x.DistrictId == d.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                             DistrictId = d.DistrictId,
                                             ThanaId = d.ThanaId,
                                             AreaId = d.AreaId
                                         },
                                         CourierOrderInfo = new CourierOrderInfo
                                         {
                                             PaymentType = d.PaymentType,
                                             OrderType = d.OrderType,
                                             Weight = d.Weight,
                                             CollectionName = d.CollectionName
                                         },
                                         CourierPrice = new CourierPrice
                                         {
                                             CollectionAmount = d.CollectionAmount,
                                             DeliveryCharge = d.DeliveryCharge,
                                             BreakableCharge = d.BreakableCharge,
                                             CODCharge = d.CodCharge,
                                             CollectionCharge = d.CollectionCharge,// users.CollectionCharge,
                                             ReturnCharge = returnCharge// users.ReturnCharge
                                         },
                                         CourierOrderDateDetails = new CourierOrderDateDetails
                                         {
                                             UpdatedOn = d.UpdatedOn,
                                             ConfirmationFormatDate = d.ConfirmationDate,
                                             OrderFormatDate = d.OrderDate,
                                             PostedFormatDate = d.PostedOn
                                         },
                                         CourierOrdersId = d.CourierOrdersId,
                                         Id = d.Id,
                                         Comment = d.Comment,
                                         PodNumber = d.PodNumber,
                                         UserInfo = new UserInfo
                                         {
                                             CourierUserId = d.MerchantId,
                                             Mobile = users.Mobile,
                                             UserName = users.UserName,
                                             Address = users.Address,
                                             EmailAddress = users.EmailAddress,
                                             CollectAddress = d.CollectAddress

                                         },
                                         AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                         {

                                             IsEmail = users.IsEmail,
                                             IsSms = users.IsSms
                                         }
                                     }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
                var responseData = new CourierOrderDetailsViewModel
                {

                    TotalCount = totalOrderCount,
                    AdTotalCollectionAmount = totalCollectionAmount,
                    AdCourierPaymentInfo = new AdCourierPaymentInfo
                    {
                        PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                        PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                        PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                    },
                    CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
                };
                return responseData;

            }
        }

        public async Task<List<CourierOrderViewModel>> CollectorWiseData(CollectorOrderBodyModel collectorOrderBodyModel)
        {
            var response = (from courierOrder in _sqlServerContext.CourierOrders


                            join courierUser in _sqlServerContext.CourierUsers.AsNoTracking()
                            on courierOrder.MerchantId equals courierUser.CourierUserId

                            join courierStatus in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                            on courierOrder.Status equals courierStatus.StatusId

                            join collectorAssign in _sqlServerContext.CollectorAssign.AsNoTracking()
                            on courierUser.CourierUserId equals collectorAssign.CourierUserId into totalCourierCollectorData
                            from output in totalCourierCollectorData.DefaultIfEmpty()

                                //join collectorAssign in _sqlServerContext.CollectorAssign
                                //on courierUser.CourierUserId equals collectorAssign.CourierUserId


                                //join collector in _sqlServerContext.Collectors
                                //on collectorAssign.CollectorId equals collector.CollectorId

                            join collector in _sqlServerContext.Collectors
                            on output.CollectorId equals collector.CollectorId into responseJoinData
                            from result in responseJoinData.DefaultIfEmpty()


                            where courierOrder.MerchantId == collectorOrderBodyModel.CourierUserId
                            && courierOrder.OfficeDrop == false
                            && courierStatus.StatusGroup.Trim().ToLower() == collectorOrderBodyModel.CollectionType.Trim().ToLower()
                            && output.CollectorId == collectorOrderBodyModel.CollectorId
                            //&& collectorAssign.CollectorId == collectorOrderBodyModel.CollectorId
                            && courierStatus.StatusGroup.ToLower().Trim() == collectorOrderBodyModel.CollectionType.Trim().ToLower()
                            //&& courierOrder.DistrictId.Equals(14)

                            let districtList = _sqlServerContext.Districts.Where(x => x.DistrictId == courierOrder.DistrictId || x.ParentId == courierOrder.DistrictId).AsNoTracking().AsQueryable()

                            let returnCharge = courierStatus.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? courierOrder.ReturnCharge : 0
                            select new CourierOrderViewModel
                            {
                                CourierId = courierOrder.CourierId,
                                //    CourierName = output.CourierName ?? "",
                                CustomerName = courierOrder.CustomerName,
                                Status = courierStatus.StatusNameBng,
                                StatusId = courierOrder.Status,
                                StatusType = courierStatus.StatusType,
                                CourierAddressContactInfo = new CourierAddressContactInfo
                                {
                                    Mobile = courierOrder.Mobile,
                                    OtherMobile = courierOrder.OtherMobile,
                                    Address = courierOrder.Address,
                                    DistrictName = districtList.Where(x => x.DistrictId == courierOrder.DistrictId && x.AreaType == 2).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                    ThanaName = districtList.Where(x => x.DistrictId == courierOrder.ThanaId && x.AreaType == 3).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                    AreaName = districtList.Where(x => x.DistrictId == courierOrder.AreaId && x.AreaType == 4).Select(y => y.DistrictBng).FirstOrDefault() ?? "",
                                    DistrictNameEng = districtList.Where(x => x.DistrictId == courierOrder.DistrictId && x.AreaType == 2).Select(y => y.District).FirstOrDefault() ?? "",
                                    ThanaNameEng = districtList.Where(x => x.DistrictId == courierOrder.ThanaId && x.AreaType == 3).Select(y => y.District).FirstOrDefault() ?? "",
                                    AreaNameEng = districtList.Where(x => x.DistrictId == courierOrder.AreaId && x.AreaType == 4).Select(y => y.District).FirstOrDefault() ?? "",

                                    ThanaPostalCode = districtList.Where(x => x.DistrictId == courierOrder.ThanaId && x.AreaType == 3).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                    AreaPostalCode = districtList.Where(x => x.DistrictId == courierOrder.AreaId && x.AreaType == 4).Select(y => y.PostalCode).FirstOrDefault() ?? "",
                                    DistrictId = courierOrder.DistrictId,
                                    ThanaId = courierOrder.ThanaId,
                                    AreaId = courierOrder.AreaId
                                },
                                CourierOrderInfo = new CourierOrderInfo
                                {
                                    PaymentType = courierOrder.PaymentType,
                                    OrderType = courierOrder.OrderType,
                                    Weight = courierOrder.Weight,

                                    CollectionName = courierOrder.CollectionName
                                },
                                CourierPrice = new CourierPrice
                                {
                                    CollectionAmount = courierOrder.CollectionAmount,
                                    DeliveryCharge = courierOrder.DeliveryCharge,
                                    BreakableCharge = courierOrder.BreakableCharge,
                                    CODCharge = courierOrder.CodCharge,
                                    CollectionCharge = courierOrder.CollectionCharge,// users.CollectionCharge,
                                    ReturnCharge = returnCharge// users.ReturnCharge
                                },
                                CourierOrderDateDetails = new CourierOrderDateDetails
                                {
                                    ConfirmationFormatDate = courierOrder.ConfirmationDate,
                                    OrderFormatDate = courierOrder.OrderDate,
                                    PostedFormatDate = courierOrder.PostedOn
                                },
                                CourierOrdersId = courierOrder.CourierOrdersId,
                                Id = courierOrder.Id,
                                Comment = courierOrder.Comment,
                                PodNumber = courierOrder.PodNumber,
                                UserInfo = new UserInfo
                                {
                                    CourierUserId = courierOrder.MerchantId,
                                    CompanyName = courierUser.CompanyName,
                                    Mobile = courierUser.Mobile,
                                    UserName = courierUser.UserName,
                                    Address = courierUser.Address,
                                    EmailAddress = courierUser.EmailAddress


                                },
                                AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                {

                                    IsEmail = courierUser.IsEmail,
                                    IsSms = courierUser.IsSms
                                }
                            }).Skip(collectorOrderBodyModel.Index).Take(collectorOrderBodyModel.Count);

            var responseData = await Task.FromResult(response.ToList());
            return responseData;
        }

        public async Task<List<CourierUsers>> ListOfCourierUser(int collectorId, int groupType)
        {
            List<CourierUsers> courierUsers = new List<CourierUsers>();
            string groupTypeStatus = string.Empty;

            var status = new int[] { 1, 5, 6, 35 };

            if (groupType == 1)
            {
                groupTypeStatus = "Collection";
            }
            else if (groupType == 2)
            {
                groupTypeStatus = "return";
            }
            if (groupType == 0)
            {

                courierUsers = await (from courierUser in _sqlServerContext.CourierUsers
                                      join collectorAssign in _sqlServerContext.CollectorAssign
                                      on courierUser.CourierUserId equals collectorAssign.CourierUserId
                                      //join order in _sqlServerContext.CourierOrders
                                      //on courierUser.CourierUserId equals order.MerchantId
                                      where collectorAssign.CollectorId == collectorId
                                      //&& status.Contains(order.Status) 
                                      select new CourierUsers
                                      {
                                          CourierUserId = courierUser.CourierUserId,
                                          CompanyName = courierUser.CompanyName,
                                          UserName = courierUser.UserName,
                                          Mobile = courierUser.Mobile,
                                          AlterMobile = courierUser.AlterMobile,
                                          Address = courierUser.Address,
                                          EmailAddress = courierUser.EmailAddress

                                      }
                               ).Distinct().ToListAsync();
            }
            else
            {


                //var districtList = (from d1 in _sqlServerContext.Districts.AsNoTracking()
                //                    join d2 in _sqlServerContext.Districts.AsNoTracking().Where(d2 => d2.IsActive.Equals(true))
                //                    on d1.DistrictId equals d2.ParentId
                //                    join d3 in _sqlServerContext.Districts.AsNoTracking().Where(d3 => d3.IsActive.Equals(true)) on d2.DistrictId equals d3.ParentId into d3Left
                //                    from d3 in d3Left.DefaultIfEmpty()

                //                    where (d1.IsActiveForCorona.Equals(true) || d2.IsActiveForCorona.Equals(true) || d3.IsActiveForCorona.Equals(true))
                //                    select new 
                //                    {
                //                        DistrictId = d1.DistrictId,
                //                        DistrictBng = d1.DistrictBng,
                //                        ThanaId = d2.DistrictId,
                //                        Thana = d2.DistrictBng,
                //                        AreaId = d3.DistrictId == null ? 0 : d3.DistrictId,
                //                        Area = d3.DistrictBng == null ? "" : d3.DistrictBng
                //                    }).Distinct().ToList();


                var courierUsersData = await (from courierUser in _sqlServerContext.CourierUsers.AsNoTracking()
                                              join collectorAssign in _sqlServerContext.CollectorAssign.AsNoTracking()
                                              on courierUser.CourierUserId equals collectorAssign.CourierUserId
                                              join order in _sqlServerContext.CourierOrders.AsNoTracking()
                                              on courierUser.CourierUserId equals order.MerchantId


                                              //join district in districtList
                                              //on 
                                              //   new { order.DistrictId, order.ThanaId, order.AreaId }
                                              //equals
                                              //   new { district.DistrictId, district.ThanaId, district.AreaId }


                                              where collectorAssign.CollectorId == collectorId
                                              //&& order.ThanaId == district.ThanaId
                                              // && order.AreaId == district.AreaId


                                              && collectorAssign.AssignType.ToLower().Trim() == groupTypeStatus.ToLower().Trim()
                                              && status.Contains(order.Status)
                                              && order.OfficeDrop == false
                                              //&& order.DistrictId.Equals(14)
                                              select new CourierUsers
                                              {
                                                  CourierUserId = courierUser.CourierUserId,
                                                  CompanyName = courierUser.CompanyName,
                                                  UserName = courierUser.UserName,
                                                  Mobile = courierUser.Mobile,
                                                  AlterMobile = courierUser.AlterMobile,
                                                  Address = courierUser.Address,
                                                  EmailAddress = courierUser.EmailAddress

                                              }
                        ).Distinct().ToListAsync();


                courierUsers = courierUsersData
                      .GroupBy(p => p.CourierUserId)
                      .Select(g => g.First())
                      .ToList();
            }



            return courierUsers;
        }

        public async Task<int> UpdateCourierOrderCollector(List<CourierOrderStatusHistoryViewModel> updateCourierOrderCollectorBodyModel)
        {
            int responseReturn = 0;

            for (int loop = 0; loop < updateCourierOrderCollectorBodyModel.Count; loop++)
            {

                CourierOrders courierOrder = (from x in _sqlServerContext.CourierOrders
                                              where x.CourierOrdersId.Trim().ToLower() == updateCourierOrderCollectorBodyModel[loop].CourierOrderId.Trim().ToLower()
                                              select x).FirstOrDefault();
                if (courierOrder != null)
                {
                    courierOrder.Status = updateCourierOrderCollectorBodyModel[loop].Status;
                    courierOrder.UpdatedBy = updateCourierOrderCollectorBodyModel[loop].PostedBy;
                    courierOrder.MerchantId = updateCourierOrderCollectorBodyModel[loop].MerchantId;
                    courierOrder.UpdatedOn = updateCourierOrderCollectorBodyModel[loop].PostedOn;
                    courierOrder.IsConfirmedBy = updateCourierOrderCollectorBodyModel[loop].IsConfirmedBy;
                    courierOrder.CourierId = updateCourierOrderCollectorBodyModel[loop].CourierId;

                }
                responseReturn = await _sqlServerContext.SaveChangesAsync();
            }




            return responseReturn;
        }

        public async Task<List<CourierOrderStatusHistory>> AddListCourierOrderHistory(List<CourierOrderStatusHistory> courierOrderStatusHistory)
        {

            await _sqlServerContext.CourierOrderStatusHistory.AddRangeAsync(courierOrderStatusHistory);
            await _sqlServerContext.SaveChangesAsync();
            return courierOrderStatusHistory;
        }

        public async Task<CourierUsersInfoViewModel> GetCourierUsersInfo(SearchBodyModel searchBody)
        {
            int totalUserCount = 0;
            var courierUsersList = new List<CourierUsers>();

            if (searchBody.Search == "")
            {
                totalUserCount = await _sqlServerContext.CourierUsers.CountAsync();
                courierUsersList = await (from courierUsers in _sqlServerContext.CourierUsers
                                          orderby courierUsers.CourierUserId
                                          select courierUsers).Skip(searchBody.Index).Take(searchBody.Count).ToListAsync();

            }
            else
            {
                totalUserCount = await _sqlServerContext.CourierUsers.CountAsync();
                courierUsersList = await (from courierUsers in _sqlServerContext.CourierUsers
                                          orderby courierUsers.CourierUserId
                                          where (courierUsers.UserName + courierUsers.Mobile + courierUsers.CompanyName).Contains(searchBody.Search)
                                          select courierUsers).Skip(searchBody.Index).Take(searchBody.Count).ToListAsync();
            }
            var result = new CourierUsersInfoViewModel
            {
                TotalUsers = totalUserCount,
                CourierUsers = courierUsersList
            };
            return result;
        }

        public async Task<dynamic> GetCourierUserInfo(int courierUserId)
        {
            var user = await (from courierUser in _sqlServerContext.CourierUsers
                              join category in _sqlServerContext.Category
                              on courierUser.CategoryId equals category.CategoryId into categoryJoin

                              from j1 in categoryJoin.DefaultIfEmpty()

                              join subCategory in _sqlServerContext.SubCategory
                              on courierUser.SubCategoryId equals subCategory.SubCategoryId into subCategoryJoin
                              from j2 in subCategoryJoin.DefaultIfEmpty()

                              where courierUser.CourierUserId.Equals(courierUserId)
                              select new
                              {
                                  CourierUserId = courierUser.CourierUserId,
                                  UserName = courierUser.UserName,
                                  Mobile = courierUser.Mobile,
                                  IsActive = courierUser.IsActive,
                                  Address = courierUser.Address,
                                  SmsCharge = courierUser.SmsCharge,
                                  MailCharge = courierUser.MailCharge,
                                  ReturnCharge = courierUser.ReturnCharge,
                                  CollectionCharge = courierUser.CollectionCharge,
                                  IsSms = courierUser.IsSms,
                                  IsEmail = courierUser.IsEmail,
                                  EmailAddress = courierUser.EmailAddress,
                                  BkashNumber = courierUser.BkashNumber,
                                  AlterMobile = courierUser.AlterMobile,
                                  SourceType = courierUser.SourceType,
                                  RetentionUserId = courierUser.RetentionUserId,
                                  AcquisitionUserId = courierUser.AcquisitionUserId,
                                  JoinDate = courierUser.JoinDate,
                                  IsDocument = courierUser.IsDocument,
                                  Remarks = courierUser.Remarks,
                                  CompanyName = courierUser.CompanyName,
                                  IsCustomerSms = courierUser.IsCustomerSms,
                                  IsCustomerEmail = courierUser.IsCustomerEmail,
                                  MaxCodCharge = courierUser.MaxCodCharge,
                                  Refreshtoken = courierUser.Refreshtoken,
                                  IsAutoProcess = courierUser.IsAutoProcess,
                                  FirebaseToken = courierUser.FirebaseToken,
                                  Credit = courierUser.Credit,
                                  FBURL = courierUser.FBURL,
                                  WebURL = courierUser.WebURL,
                                  IsOfferActive = courierUser.IsOfferActive,
                                  OfferCodDiscount = courierUser.OfferCodDiscount,
                                  OfferType = courierUser.OfferType,
                                  OfferBkashDiscountDhaka = courierUser.OfferBkashDiscountDhaka,
                                  OfferBkashDiscountOutSideDhaka = courierUser.OfferBkashDiscountOutSideDhaka,
                                  AdvancePayment = courierUser.AdvancePayment,
                                  KnowingSource = courierUser.KnowingSource,
                                  Priority = courierUser.Priority,
                                  Referrer = courierUser.Referrer,
                                  ReferrerOrder = courierUser.ReferrerOrder,
                                  ReferrerStartTime = courierUser.RefereeStartTime,
                                  ReferrerEndTime = courierUser.RefereeEndTime,
                                  OrderType = courierUser.OrderType,
                                  RefereeOrder = courierUser.RefereeOrder,
                                  ReferrerIsActive = courierUser.ReferrerIsActive,
                                  RefereeStartTime = courierUser.RefereeStartTime,
                                  RefereeEndTime = courierUser.RefereeEndTime,
                                  PreferredPaymentCycle = courierUser.PreferredPaymentCycle,
                                  RegistrationFrom = courierUser.RegistrationFrom,
                                  IsBlock = courierUser.IsBlock,
                                  BlockReason = courierUser.BlockReason,
                                  WeightRangeId = courierUser.WeightRangeId,
                                  DeliveryRangeIdInside = courierUser.DeliveryRangeIdInside,
                                  DeliveryRangeIdIOutside = courierUser.DeliveryRangeIdIOutside,
                                  DistrictId = courierUser.DistrictId,
                                  ThanaId = courierUser.ThanaId,
                                  PreferredPaymentCycleDate = courierUser.PreferredPaymentCycleDate,
                                  AccountName = courierUser.AccountName,
                                  AccountNumber = courierUser.AccountNumber,
                                  BankName = courierUser.BankName,
                                  BranchName = courierUser.BranchName,
                                  RoutingNumber = courierUser.RoutingNumber,
                                  IsQuickOrderActive = courierUser.IsQuickOrderActive,
                                  CustomerSMSLimit = courierUser.CustomerSMSLimit,
                                  CustomerVoiceSmsLimit = courierUser.CustomerVoiceSmsLimit,
                                  IsLoanActive = courierUser.IsLoanActive,
                                  LoanCompany = courierUser.LoanCompany,
                                  Gender = courierUser.Gender,
                                  CategoryId = courierUser.CategoryId,
                                  CategoryName = j1 == null ? "" : (j1.CategoryNameEng ?? ""),
                                  SubCategoryId = courierUser.SubCategoryId,
                                  SubCategoryName = j2 == null ? "" : (j2.SubCategoryNameEng ?? ""),
                                  IsBreakAble = courierUser.IsBreakAble,
                                  IsTeleSales = courierUser.IsTeleSales,
                                  TeleSalesDate = courierUser.TeleSalesDate,
                                  IsHeavyWeight = courierUser.IsHeavyWeight,
                                  TeleSales = courierUser.TeleSales,
                                  MerchantAssignActive = courierUser.MerchantAssignActive,
                                  HashPassword = courierUser.HashPassword,
                                  MerchantReview = courierUser.MerchantReview,
                                  Recommendation = courierUser.Recommendation,
                                  PaymentServiceType = courierUser.PaymentServiceType,
                                  PaymentServiceCharge = courierUser.PaymentServiceCharge,
                                  IsDana = courierUser.IsDana

                              }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<CourierOrderDetailsViewModel> LoadCourierReport(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            //Need Review
            CourierOrderDetailsViewModel responseData = new CourierOrderDetailsViewModel();

            if (loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001" && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001")
            {
                responseData = await RetriveOrderCurrentList_Admin(loadCourierOrderBodyModel);
            }
            else
            {
                responseData = await RetriveLogReport_Admin(loadCourierOrderBodyModel);
            }
            return responseData;
        }

        public async Task<CourierOrderDetailsViewModel> RetriveOrderCurrentList_Admin(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => loadCourierOrderBodyModel.StatusList.Contains(d.Status)).CountAsync();
            var courierOrders = (from d in _sqlServerContext.CourierOrders.OrderByDescending(v => v.PostedOn)
                                 join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                 join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId

                                 where loadCourierOrderBodyModel.StatusList.Contains(d.Status)
                                 let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0

                                 select new CourierOrderViewModel
                                 {
                                     CourierId = d.CourierId,
                                     //CourierName = output.CourierName ?? "",
                                     CustomerName = d.CustomerName,
                                     Status = status.StatusNameBng,
                                     StatusId = d.Status,
                                     StatusEng = status.StatusNameEng,
                                     StatusType = status.StatusType,
                                     HubName = d.HubName,
                                     CourierAddressContactInfo = new CourierAddressContactInfo
                                     {
                                         Mobile = d.Mobile,
                                         OtherMobile = d.OtherMobile,
                                         Address = d.Address,
                                         DistrictId = d.DistrictId,
                                         ThanaId = d.ThanaId,
                                         AreaId = d.AreaId,
                                     },
                                     CourierOrderInfo = new CourierOrderInfo
                                     {
                                         PaymentType = d.PaymentType,
                                         OrderType = d.OrderType,
                                         Weight = d.Weight,
                                         CollectionName = d.CollectionName
                                     },
                                     CourierPrice = new CourierPrice
                                     {
                                         CollectionAmount = d.CollectionAmount,
                                         DeliveryCharge = d.DeliveryCharge,
                                         BreakableCharge = d.BreakableCharge,
                                         CODCharge = d.CodCharge,
                                         CollectionCharge = d.CollectionCharge,
                                         ReturnCharge = returnCharge,
                                         PackagingName = d.PackagingName,
                                         PackagingCharge = d.PackagingCharge,
                                         CourierCharge = d.CourierCharge
                                     },
                                     CourierOrderDateDetails = new CourierOrderDateDetails
                                     {
                                         ConfirmationFormatDate = d.ConfirmationDate,
                                         OrderFormatDate = d.OrderDate,
                                         PostedFormatDate = d.PostedOn
                                     },
                                     CourierOrdersId = d.CourierOrdersId,
                                     Id = d.Id,
                                     Comment = d.Comment,
                                     PodNumber = d.PodNumber,
                                     UserInfo = new UserInfo
                                     {
                                         CourierUserId = d.MerchantId,
                                         Mobile = users.Mobile,
                                         BkashNumber = users.BkashNumber,
                                         CompanyName = users.CompanyName,
                                         UserName = users.UserName,
                                         Address = users.Address,
                                         EmailAddress = users.EmailAddress,
                                         CollectAddress = d.CollectAddress
                                     },
                                     AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                     {
                                         IsEmail = users.IsEmail,
                                         IsSms = users.IsSms,
                                         IsCustomerSms = users.IsCustomerSms,
                                         IsCustomerEmail = users.IsCustomerEmail
                                     }
                                 }).Skip(loadCourierOrderBodyModel.Index).Take(loadCourierOrderBodyModel.Count);
            var responseData = new CourierOrderDetailsViewModel
            {
                TotalCount = totalOrderCount,

                CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
            };
            return responseData;
        }

        public async Task<CourierOrderDetailsViewModel> RetriveLogReport_Admin(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            int totalOrderCount = 0;
            totalOrderCount = await _sqlServerContext.CourierOrderStatusHistory.Where(d => loadCourierOrderBodyModel.StatusList.Contains(d.Status) && d.PostedOn.Date >= loadCourierOrderBodyModel.FromDate.Date && d.PostedOn <= loadCourierOrderBodyModel.ToDate.Date).Distinct().CountAsync();
            var courierOrders = (from s in _sqlServerContext.CourierOrderStatusHistory.OrderByDescending(s => s.PostedOn)
                                 join d in _sqlServerContext.CourierOrders on s.CourierOrderId equals d.CourierOrdersId
                                 join status in _sqlServerContext.CourierOrderStatus on d.Status equals status.StatusId
                                 join logStatus in _sqlServerContext.CourierOrderStatus on s.Status equals logStatus.StatusId
                                 join users in _sqlServerContext.CourierUsers on d.MerchantId equals users.CourierUserId

                                 where loadCourierOrderBodyModel.StatusList.Contains(s.Status) && s.PostedOn.Date >= loadCourierOrderBodyModel.FromDate.Date && s.PostedOn <= loadCourierOrderBodyModel.ToDate.Date

                                 let returnCharge = status.StatusGroup.ToLower().Trim() == "Return".ToLower().Trim() ? d.ReturnCharge : 0

                                 select new CourierOrderViewModel
                                 {
                                     CourierId = d.CourierId,
                                     //CourierName = output.CourierName ?? "",
                                     CustomerName = d.CustomerName,
                                     Status = status.StatusNameBng,
                                     StatusId = d.Status,
                                     LogStatusId = s.Status,
                                     LogStatus = logStatus.StatusNameEng,
                                     StatusEng = status.StatusNameEng,
                                     StatusType = status.StatusType,
                                     CourierOrdersId = d.CourierOrdersId,
                                     Id = d.Id,
                                     Comment = d.Comment,
                                     PodNumber = d.PodNumber,
                                     HubName = d.HubName,
                                     CourierAddressContactInfo = new CourierAddressContactInfo
                                     {
                                         Mobile = d.Mobile,
                                         OtherMobile = d.OtherMobile,
                                         Address = d.Address,
                                         DistrictId = d.DistrictId,
                                         ThanaId = d.ThanaId,
                                         AreaId = d.AreaId
                                     },
                                     CourierOrderInfo = new CourierOrderInfo
                                     {
                                         PaymentType = d.PaymentType,
                                         OrderType = d.OrderType,
                                         Weight = d.Weight,
                                         CollectionName = d.CollectionName
                                     },
                                     CourierPrice = new CourierPrice
                                     {
                                         CollectionAmount = d.CollectionAmount,
                                         DeliveryCharge = d.DeliveryCharge,
                                         BreakableCharge = d.BreakableCharge,
                                         CODCharge = d.CodCharge,
                                         CollectionCharge = d.CollectionCharge,
                                         ReturnCharge = returnCharge,
                                         PackagingName = d.PackagingName,
                                         PackagingCharge = d.PackagingCharge,
                                         CourierCharge = d.CourierCharge
                                     },
                                     CourierOrderDateDetails = new CourierOrderDateDetails
                                     {
                                         ConfirmationFormatDate = d.ConfirmationDate,
                                         OrderFormatDate = d.OrderDate,
                                         PostedFormatDate = d.PostedOn,
                                         LogPostedFormatDate = s.PostedOn
                                     },
                                     UserInfo = new UserInfo
                                     {
                                         CourierUserId = d.MerchantId,
                                         Mobile = users.Mobile,
                                         BkashNumber = users.BkashNumber,
                                         CompanyName = users.CompanyName,
                                         UserName = users.UserName,
                                         Address = users.Address,
                                         EmailAddress = users.EmailAddress,
                                         CollectAddress = d.CollectAddress
                                     },
                                     AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                                     {
                                         IsEmail = users.IsEmail,
                                         IsSms = users.IsSms,
                                         IsCustomerSms = users.IsCustomerSms,
                                         IsCustomerEmail = users.IsCustomerEmail
                                     }
                                 });
            var responseData = new CourierOrderDetailsViewModel
            {
                TotalCount = totalOrderCount,
                CourierOrderViewModel = await Task.FromResult(courierOrders.ToList())
            };
            return responseData;
        }

        public async Task<IEnumerable<Districts>> LoadAllDistricts()
        {
            //return await _sqlServerContext.Districts.Where(d => d.IsActive == true && d.DistrictId < 20000).OrderBy(s => s.DistrictPriority).ToListAsync();
            return await _sqlServerContext.Districts.Where(d => d.IsActive == true).OrderBy(s => s.DistrictPriority).Select(s => new Districts
            {
                DistrictId = s.DistrictId,
                DistrictBng = s.DistrictBng,
                District = s.District,
                IsCity = s.IsCity,
                IsActiveForCorona = s.IsActiveForCorona,
                ParentId = s.ParentId,
                PostalCode = s.PostalCode,
                IsActive = s.IsActive,
                IsPickupLocation = s.IsPickupLocation,
                CollectionTimeSlotId = s.CollectionTimeSlotId,
                NextDayAlertMessage = s.NextDayAlertMessage
            }).ToListAsync();
        }

        public async Task<IEnumerable<Districts>> LoadAllDistrictsById(int id)
        {
            return await _sqlServerContext.Districts.Where(d => d.IsActive == true && d.ParentId.Equals(id)).OrderBy(s => s.DistrictPriority).ToListAsync();
        }

        public async Task<int> UpdateOrdersBulk(List<CourierOrders> courierOrders)
        {
            var courierOrdersIds = (from d in courierOrders
                                    select d.CourierOrdersId).ToArray();

            var entity = await _sqlServerContext.CourierOrders.AsNoTracking()
                .Where(x => courierOrdersIds.Contains(x.CourierOrdersId))
                .BatchUpdateAsync(x => new CourierOrders { IsTakaCollectionFromCourier = true });

            return entity;
        }

        public async Task<List<CourierOrders>> UpdateOrderInformation(List<CourierOrders> courierOrders)
        {
            var paymentStatus = new int[] { 24, 25, 28, 30, 31 };

            var courierOrdersIds = (from d in courierOrders
                                    select d.CourierOrdersId).ToArray();


            //string courierName = await _sqlServerContext.Couriers.Where(x => x.CourierId.Equals(courierOrders.FirstOrDefault().CourierId)).Select(x => x.CourierName).FirstOrDefaultAsync();


            var entity = await _sqlServerContext.CourierOrders.AsNoTracking()
                .Where(x => courierOrdersIds.Contains(x.CourierOrdersId)
                && !paymentStatus.Contains(x.Status))
                .BatchUpdateAsync(x => new CourierOrders
                {
                    CourierId = courierOrders.FirstOrDefault().CourierId,
                    Status = 15,
                    UpdatedBy = courierOrders.FirstOrDefault().UpdatedBy,
                    UpdatedOn = courierOrders.FirstOrDefault().PostedOn.ToLocalTime(),
                    Comment = "কুরিয়ার পণ্যটি কাস্টমারকে ডেলিভারি করেছে",
                    IsConfirmedBy = "admin"
                });

            var data = from o in _sqlServerContext.CourierOrders
                .Where(x => courierOrdersIds.Contains(x.CourierOrdersId)
                && !paymentStatus.Contains(x.Status))
                       select o;


            return await data.ToListAsync();

        }

        public async Task<IEnumerable<OrderStatusHistoryViewModel>> GetOrderHistoryInformation(string orderId)
        {

            IQueryable<OrderStatusHistoryViewModel> data =
                from history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                join status in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                on history.Status equals status.StatusId
                where history.CourierOrderId.Equals(orderId)
                select new OrderStatusHistoryViewModel
                {
                    CourierOrderId = history.CourierOrderId,
                    OrderDate = history.OrderDate,
                    PostedOn = history.PostedOn,
                    IsConfirmedBy = history.IsConfirmedBy,
                    PodNumber = history.PodNumber,
                    PostedBy = history.PostedBy,
                    Comment = history.Comment,
                    CourierId = history.CourierId,
                    HubName = history.HubName,
                    MerchantId = history.MerchantId,
                    Status = history.Status,
                    Id = history.Id,
                    OrderStatus = new OrderStatusViewModel
                    {
                        StatusId = status.StatusId,
                        StatusNameBng = status.StatusNameBng,
                        StatusNameEng = status.StatusNameEng
                    },
                };

            return await data.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<IEnumerable<CourierUsers>> GetAllCourierUsersList(string companyName)
        {
            if (companyName == "0")
            {
                IQueryable<CourierUsers> result = from courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                                                  where courierUsers.IsActive == true
                                                  select new CourierUsers
                                                  {
                                                      CourierUserId = courierUsers.CourierUserId,
                                                      CompanyName = courierUsers.CompanyName,
                                                      Mobile = courierUsers.Mobile,
                                                      TeleSales = courierUsers.TeleSales
                                                  };
                return await result.ToListAsync();
            }
            else
            {
                IQueryable<CourierUsers> result = from courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                                                  where courierUsers.IsActive == true
                                                  && EF.Functions.Like(courierUsers.CompanyName, "%" + companyName + "%")
                                                  select new CourierUsers
                                                  {
                                                      CourierUserId = courierUsers.CourierUserId,
                                                      CompanyName = courierUsers.CompanyName,
                                                      Mobile = courierUsers.Mobile,
                                                      TeleSales = courierUsers.TeleSales
                                                  };
                return await result.ToListAsync();
            }

        }

        public async Task<IEnumerable<TeleSaleCourierUsers>> GetTeleSaleCourierUsersList(int courierUserId, int teleSales)
        {
            return await _sqlServerContext.TeleSaleCourierUsers.Where(d => d.CourierUserId.Equals(courierUserId) && d.TeleSales.Equals(teleSales)).ToListAsync();
        }

        public async Task<int> FixSpecialCharacter(string courierOrdersId)
        {

            int startIndex = 3;
            int endIndex = courierOrdersId.Length - 3;
            int id = Convert.ToInt32(courierOrdersId.Substring(startIndex, endIndex));


            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == id);

            if (entity != null)
            {
                entity.CustomerName = SpecialCharacters.RemoveSpecialCharacters(entity.CustomerName);
                entity.CollectAddress = SpecialCharacters.RemoveSpecialCharacters(entity.CollectAddress);
                entity.CollectionName = SpecialCharacters.RemoveSpecialCharacters(entity.CollectionName);
                entity.Address = SpecialCharacters.RemoveSpecialCharacters(entity.Address);
                entity.Comment = SpecialCharacters.RemoveSpecialCharacters(entity.Comment);

                _sqlServerContext.CourierOrders.Update(entity);

                await _sqlServerContext.SaveChangesAsync();
            }


            var parameter = new DynamicParameters();
            parameter.Add(name: "@Id", value: id, dbType: DbType.Int32);

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                await connection.ExecuteAsync(
                   sql: @"[DT].[USP_FixSpecialCharacter]",
                   param: parameter,
                   commandType: CommandType.StoredProcedure);
            }

            return 1;
        }

        public async Task<IEnumerable<CourierOrderStatusHistory>> UpdateBulkOrders(List<CourierOrderStatusHistoryViewModel> courierOrderStatusHistory)
        {

            var courierOrderIds = (from a in courierOrderStatusHistory
                                   select a.CourierOrderId).ToArray();
            var comment = courierOrderStatusHistory.Select(x => x.Comment).FirstOrDefault();
            var status = courierOrderStatusHistory.Select(x => x.Status).FirstOrDefault();
            var isConfirmberBy = courierOrderStatusHistory.Select(x => x.IsConfirmedBy).FirstOrDefault();
            var hubName = courierOrderStatusHistory.Select(x => x.HubName).FirstOrDefault();
            var updatedBy = courierOrderStatusHistory.Select(x => x.PostedBy).FirstOrDefault();


            var updateEntity = await _sqlServerContext.CourierOrders.AsNoTracking().Where(x => courierOrderIds.Contains(x.CourierOrdersId))
                                .BatchUpdateAsync(x => new CourierOrders
                                {
                                    Status = status,
                                    Comment = comment,
                                    IsConfirmedBy = isConfirmberBy,
                                    HubName = hubName,
                                    UpdatedBy = updatedBy,
                                    UpdatedOn = DateTime.Now
                                });


            var insertEntity = _sqlServerContext.CourierOrders.AsNoTracking().Where(x => courierOrderIds.Contains(x.CourierOrdersId));


            var orderStatusHistory = insertEntity.Select(item => new CourierOrderStatusHistory()
            {
                CourierOrderId = item.CourierOrdersId,
                IsConfirmedBy = item.IsConfirmedBy,
                OrderDate = item.OrderDate,
                Status = item.Status,
                PostedBy = item.UpdatedBy,
                PostedOn = item.UpdatedOn,
                MerchantId = item.MerchantId,
                Comment = item.Comment,
                PodNumber = item.PodNumber,
                CourierId = item.CourierId,
                HubName = item.HubName,
                CourierDeliveryManName = item.CourierDeliveryManName,
                CourierDeliveryManMobile = item.CourierDeliveryManMobile
            }).ToList();

            await _sqlServerContext.BulkInsertAsync(orderStatusHistory);


            return orderStatusHistory;



            ////List<CourierOrders> courierOrdersList = new List<CourierOrders>();
            //List<CourierOrderStatusHistory> courierOrderStatusHistoryList = new List<CourierOrderStatusHistory>();

            ////var courierOrdersIds = (from d in courierOrderStatusHistory
            ////                        select d.CourierOrderId).ToArray();

            ////CourierOrders courierOrder = (from x in _sqlServerContext.CourierOrders
            ////                              where courierOrdersIds.Contains(x.CourierOrdersId)
            ////                              select x);


            ////var courierOrders = _sqlServerContext.CourierOrders.AsNoTracking()
            ////    .Where(x => courierOrdersIds.Contains(x.CourierOrdersId)).ToList();


            //int startIndex = 3;
            //int endIndex = 0;
            //foreach (var item in courierOrderStatusHistory)
            //{
            //    endIndex = item.CourierOrderId.Length - 3;
            //    item.OrderId = Convert.ToInt32(item.CourierOrderId.Substring(startIndex, endIndex));
            //}


            //IQueryable<CourierOrders> courierOrdersList = (from x in _sqlServerContext.CourierOrders.AsNoTracking()
            //                                               join y in courierOrderStatusHistory
            //                                               on x.Id equals y.OrderId
            //                                               //on x.CourierOrdersId equals y.CourierOrderId
            //                                               select new CourierOrders
            //                                               {
            //                                                   Id = x.Id,
            //                                                   Status = y.Status,
            //                                                   UpdatedBy = y.PostedBy,
            //                                                   UpdatedOn = y.PostedOn,
            //                                                   IsConfirmedBy = y.IsConfirmedBy,
            //                                                   CourierId = y.CourierId,
            //                                                   HubName = y.HubName,
            //                                                   Comment = y.Comment,
            //                                                   CourierCharge = y.CourierCharge,

            //                                                   CustomerName = x.CustomerName,
            //                                                   Mobile = x.Mobile,
            //                                                   OtherMobile = x.OtherMobile,
            //                                                   Address = x.Address,
            //                                                   DistrictId = x.DistrictId,
            //                                                   ThanaId = x.ThanaId,
            //                                                   AreaId = x.AreaId,
            //                                                   PaymentType = x.PaymentType,
            //                                                   OrderType = x.OrderType,
            //                                                   Weight = x.Weight,
            //                                                   CollectionName = x.CollectionName,
            //                                                   CollectionAmount = x.CollectionAmount,
            //                                                   DeliveryCharge = x.DeliveryCharge,
            //                                                   IsActive = x.IsActive,

            //                                                   ConfirmationDate = x.ConfirmationDate,
            //                                                   OrderDate = x.OrderDate,

            //                                                   PostedOn = x.PostedOn,
            //                                                   PostedBy = x.PostedBy,
            //                                                   MerchantId = x.MerchantId,

            //                                                   PodNumber = y.PodNumber,
            //                                                   BreakableCharge = x.BreakableCharge,
            //                                                   CodCharge = x.CodCharge,
            //                                                   Note = x.Note,
            //                                                   CollectionCharge = x.CollectionCharge,
            //                                                   ReturnCharge = x.ReturnCharge,
            //                                                   PackagingName = x.PackagingName,
            //                                                   PackagingCharge = x.PackagingCharge,
            //                                                   CollectAddress = x.CollectAddress,
            //                                                   OrderFrom = x.OrderFrom,
            //                                                   IsOpenBox = x.IsOpenBox,
            //                                                   IsAutoProcess = x.IsAutoProcess,
            //                                                   IsTakaCollectionFromCourier = x.IsTakaCollectionFromCourier,
            //                                                   DeliveryRangeId = x.DeliveryRangeId,
            //                                                   WeightRangeId = x.WeightRangeId,
            //                                                   ProductType = x.ProductType,
            //                                                   CollectAddressDistrictId = x.CollectAddressDistrictId,
            //                                                   CollectAddressThanaId = x.CollectAddressThanaId,
            //                                                   DeliveryUserId = x.DeliveryUserId,
            //                                                   RiderAcceptDate = x.RiderAcceptDate,
            //                                                   RiderDeliveredDate = x.RiderDeliveredDate,
            //                                                   MerchantDeliveryDate = x.MerchantDeliveryDate,
            //                                                   MerchantCollectionDate = x.MerchantCollectionDate,
            //                                                   OfficeDrop = x.OfficeDrop,
            //                                                   OfferCode = x.OfferCode,
            //                                                   OfferCodDiscount = x.OfferCodDiscount,
            //                                                   OfferBkashDiscount = x.OfferBkashDiscount,
            //                                                   IsOfferCodActive = x.IsOfferCodActive,
            //                                                   IsOfferBkashActive = x.IsOfferBkashActive,
            //                                                   ClassifiedId = x.ClassifiedId,
            //                                                   ActualPackagePrice = x.ActualPackagePrice,
            //                                                   CollectionTimeSlotId = x.CollectionTimeSlotId,
            //                                                   CollectionTime = x.CollectionTime,
            //                                                   OfferType = x.OfferType,
            //                                                   RelationType = x.RelationType,
            //                                                   ExpectedDeliveryDate = x.ExpectedDeliveryDate,
            //                                                   CourierDeliveryManName = x.CourierDeliveryManName,
            //                                                   CourierDeliveryManMobile = x.CourierDeliveryManMobile,
            //                                                   TransactionId = x.TransactionId,
            //                                                   ValidationId = x.ValidationId,
            //                                                   ServiceType = x.ServiceType,
            //                                                   Version = x.Version,
            //                                                   InvoiceNumber = x.InvoiceNumber,
            //                                                   InvoiceCourier = x.InvoiceCourier,
            //                                                   IsQuickOrder = x.IsQuickOrder,
            //                                                   QuickOrderGenerateDate = x.QuickOrderGenerateDate,
            //                                                   DownloadedDate = x.DownloadedDate,
            //                                                   QuickOrderImageUrl = x.QuickOrderImageUrl
            //                                               });



            ////spublic string CourierOrdersId { set; get; }




            ////foreach (var item in courierOrders)
            ////{
            ////    courierOrdersList.Add(new CourierOrders
            ////    {
            ////        //Id = item.Id,
            ////        //Status = item.Status,
            ////        //UpdatedBy = item.PostedBy,
            ////        //MerchantId = item.MerchantId,
            ////        //UpdatedOn = item.PostedOn,
            ////        //IsConfirmedBy = item.IsConfirmedBy,
            ////        //CourierId = item.CourierId,
            ////        //HubName = item.HubName,
            ////        //Comment = item.Comment,
            ////        //CourierCharge = item.CourierCharge


            ////    });
            ////}

            //foreach (var item in courierOrderStatusHistory)
            //{
            //    courierOrderStatusHistoryList.Add(new CourierOrderStatusHistory
            //    {
            //        CourierOrderId = item.CourierOrderId,
            //        IsConfirmedBy = item.IsConfirmedBy,
            //        OrderDate = item.OrderDate,
            //        Status = item.Status,
            //        PostedBy = item.PostedBy,
            //        MerchantId = item.MerchantId,
            //        Comment = item.Comment,
            //        PodNumber = item.PodNumber,
            //        CourierId = item.CourierId,
            //        HubName = item.HubName,
            //        CourierDeliveryManName = item.CourierDeliveryManName,
            //        CourierDeliveryManMobile = item.CourierDeliveryManMobile
            //    });
            //}

            //_sqlServerContext.CourierOrders.UpdateRange(courierOrdersList.ToList());
            //await _sqlServerContext.CourierOrderStatusHistory.AddRangeAsync(courierOrderStatusHistoryList);
            //_sqlServerContext.SaveChanges();

            //return courierOrderStatusHistory;
        }

        public async Task<CourierUserPickupLocationModel> GetCourierUserListWithPickupLocations(CourierUsersLocationWiseSearchBodyModel merchant)
        {
            if (merchant.Search == "" && merchant.DistrictId != 0 && merchant.ThanaId != 0)
            {
                int totalCount = 0;
                var merchantList = _sqlServerContext.CourierOrders.Where(x => x.OrderDate > DateTime.Now.AddMonths(-3)).Select(p => p.MerchantId).Distinct().ToList();
                totalCount = await _sqlServerContext.PickupLocations.Where(x => merchantList.Contains(x.CourierUserId) && x.DistrictId.Equals(merchant.DistrictId) && x.ThanaId.Equals(merchant.ThanaId)).Distinct().CountAsync();
                List<CourierUserPickupLocations> result = (from courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                                                           where courierUsers.IsActive == true
                                                           && merchantList.Contains(courierUsers.CourierUserId)
                                                           join pickupLocations in _sqlServerContext.PickupLocations on courierUsers.CourierUserId equals pickupLocations.CourierUserId
                                                           where pickupLocations.DistrictId.Equals(merchant.DistrictId) && pickupLocations.ThanaId.Equals(merchant.ThanaId)
                                                           select new CourierUserPickupLocations
                                                           {
                                                               CourierUserId = courierUsers.CourierUserId,
                                                               UserName = courierUsers.UserName,
                                                               CompanyName = courierUsers.CompanyName,
                                                               Address = courierUsers.Address,
                                                               Mobile = courierUsers.Mobile,
                                                               Id = pickupLocations.Id,
                                                               DistrictId = pickupLocations.DistrictId,
                                                               ThanaId = pickupLocations.ThanaId,
                                                               AreaId = pickupLocations.AreaId,
                                                               PickupAddress = pickupLocations.PickupAddress,
                                                               Latitude = pickupLocations.Latitude,
                                                               Longitude = pickupLocations.Longitude
                                                           }).Skip(merchant.Index).Take(merchant.Count).ToList();
                CourierUserPickupLocationModel data = new CourierUserPickupLocationModel
                {
                    TotalCount = totalCount,
                    PickUpLocations = result.ToList()
                };
                return data;
            }
            else if (merchant.Search != "" && merchant.DistrictId == 0 && merchant.ThanaId == 0)
            {
                int totalCount = 0;
                totalCount = await _sqlServerContext.CourierUsers.Where(x => x.IsActive.Equals(true) && EF.Functions.Like(x.CompanyName, "%" + merchant.Search + "%")).Distinct().CountAsync();
                List<CourierUserPickupLocations> result = (from courierUsers in _sqlServerContext.CourierUsers
                                                           where courierUsers.IsActive == true
                                                           && EF.Functions.Like(courierUsers.CompanyName, "%" + merchant.Search + "%")
                                                           join pickupLocations in _sqlServerContext.PickupLocations on courierUsers.CourierUserId equals pickupLocations.CourierUserId
                                                           //let pickupLocations = _sqlServerContext.PickupLocations.Where(x => x.CourierUserId.Equals(courierUsers.CourierUserId)).ToList()
                                                           select new CourierUserPickupLocations
                                                           {
                                                               CourierUserId = courierUsers.CourierUserId,
                                                               UserName = courierUsers.UserName,
                                                               CompanyName = courierUsers.CompanyName,
                                                               Address = courierUsers.Address,
                                                               Mobile = courierUsers.Mobile,
                                                               Id = pickupLocations.Id,
                                                               DistrictId = pickupLocations.DistrictId,
                                                               ThanaId = pickupLocations.ThanaId,
                                                               AreaId = pickupLocations.AreaId,
                                                               PickupAddress = pickupLocations.PickupAddress,
                                                               Latitude = pickupLocations.Latitude,
                                                               Longitude = pickupLocations.Longitude
                                                           }).Skip(merchant.Index).Take(merchant.Count).ToList();
                CourierUserPickupLocationModel data = new CourierUserPickupLocationModel
                {
                    TotalCount = totalCount,
                    PickUpLocations = result.ToList()
                };
                return data;
            }
            else // if (merchant.Search == "" && merchant.DistrictId == 0 && merchant.ThanaId == 0)
            {
                int totalCount = 0;
                var merchantList = _sqlServerContext.CourierOrders.Where(x => x.OrderDate > DateTime.Now.AddMonths(-3)).Select(p => p.MerchantId).Distinct().ToList();
                totalCount = await _sqlServerContext.PickupLocations.Where(x => merchantList.Contains(x.CourierUserId)).Distinct().CountAsync();
                List<CourierUserPickupLocations> result = (from courierUsers in _sqlServerContext.CourierUsers
                                                           where courierUsers.IsActive == true
                                                           && merchantList.Contains(courierUsers.CourierUserId)

                                                           join pickupLocations in _sqlServerContext.PickupLocations on courierUsers.CourierUserId equals pickupLocations.CourierUserId

                                                           select new CourierUserPickupLocations
                                                           {
                                                               CourierUserId = courierUsers.CourierUserId,
                                                               UserName = courierUsers.UserName,
                                                               CompanyName = courierUsers.CompanyName,
                                                               Address = courierUsers.Address,
                                                               Mobile = courierUsers.Mobile,
                                                               Id = pickupLocations.Id,
                                                               DistrictId = pickupLocations.DistrictId,
                                                               ThanaId = pickupLocations.ThanaId,
                                                               AreaId = pickupLocations.AreaId,
                                                               PickupAddress = pickupLocations.PickupAddress,
                                                               Latitude = pickupLocations.Latitude,
                                                               Longitude = pickupLocations.Longitude
                                                           }).Skip(merchant.Index).Take(merchant.Count).ToList();
                CourierUserPickupLocationModel data = new CourierUserPickupLocationModel
                {
                    TotalCount = totalCount,
                    PickUpLocations = result.ToList()
                };
                return data;
            }
        }

        public async Task<DeliveryUsersViewModel> GetRidersOfficeInfo(RequestBodyModel requestBodyModel)
        {
            var rider = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                               join _deliveryUsers in _sqlServerContext.DeliveryUsers.AsNoTracking()
                               on _courierOrders.DeliveryUserId equals _deliveryUsers.Id
                               where _courierOrders.MerchantId.Equals(requestBodyModel.MerchantId)
                               && _courierOrders.Status.Equals(41)
                               orderby _courierOrders.Id descending
                               select new DeliveryUsersViewModel
                               {
                                   Id = _deliveryUsers.Id,
                                   Name = _deliveryUsers.Name,
                                   Mobile = _deliveryUsers.Mobile
                               }).FirstOrDefaultAsync();

            if (rider == null)
            {
                return new DeliveryUsersViewModel
                {
                    Mobile = "01844487627" //01894804833
                };
            }

            return rider;
        }
        public async Task<IEnumerable<DeliveryUserAcceptedSingleViewModel>> GetAcceptedRiders(PickupLocations pickupLocations)
        {
            IQueryable<DeliveryUserAcceptedSingleViewModel> data = from 
                                        acceptedOrders in _sqlServerContext.CourierOrders.AsNoTracking() 
                                        where  acceptedOrders.MerchantId.Equals(pickupLocations.CourierUserId) 
                                        && acceptedOrders.CollectAddressDistrictId.Equals(pickupLocations.DistrictId) 
                                        && acceptedOrders.CollectAddressThanaId.Equals(pickupLocations.ThanaId)
                                        && acceptedOrders.Status.Equals(41)
                                        join deliveryUser in _sqlServerContext.DeliveryUsers on acceptedOrders.DeliveryUserId equals deliveryUser.Id
                                        select new DeliveryUserAcceptedSingleViewModel
                                        {
                                            Id = deliveryUser.Id,
                                            Latitude = deliveryUser.Latitude,
                                            Longitude = deliveryUser.Longitude,
                                            Mobile = deliveryUser.Mobile,
                                            Name = deliveryUser.Name,
                                            CourierOrderId = acceptedOrders.CourierOrdersId
                                        };
            return await data.ToListAsync();
        }

        public async Task<CourierOrders> GetOrderInformation(int orderId)
        {
            return await _sqlServerContext.CourierOrders.Where(x => x.Id.Equals(orderId)).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetDuplicatesCourierUsersInfo()
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var data = await connection.QueryAsync<CourierUsersViewModel>(
                        sql: @"[DT].[USP_DuplicatesCourierUsersInfo]",
                        param: null,
                        commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<List<AssignCouirerAndServiceViewModel>> GetAssignCouirerAndService(int id)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@Id", value: id, dbType: DbType.Int32);
                var data = await connection.QueryAsync<AssignCouirerAndServiceViewModel>(
                        sql: @"[DT].[USP_AssignCouirerAndService]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<List<AssignCouirerAndServiceViewModel>> GetAssignCouirerAndServiceArea(int id)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@Id", value: id, dbType: DbType.Int32);
                var data = await connection.QueryAsync<AssignCouirerAndServiceViewModel>(
                        sql: @"[DT].[USP_AssignCouirerAndServiceWithArea]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<List<AssignCouirerAndServiceViewModel>> GetMerchantAssignCouirerAndService(int id, int courierUserId)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@Id", value: id, dbType: DbType.Int32);
                parameter.Add(name: "courierUserId", value: courierUserId, dbType: DbType.Int32);
                var data = await connection.QueryAsync<AssignCouirerAndServiceViewModel>(
                        sql: @"[DT].[USP_MerchantAssignCouirerAndService]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<List<AssignCouirerAndServiceViewModel>> GetMerchantAssignCouirerAndServiceArea(int id, int courierUserId)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@Id", value: id, dbType: DbType.Int32);
                parameter.Add(name: "courierUserId", value: courierUserId, dbType: DbType.Int32);
                var data = await connection.QueryAsync<AssignCouirerAndServiceViewModel>(
                        sql: @"[DT].[USP_MerchantAssignCouirerAndServiceWithArea]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<List<AssignCouirerAndServiceViewModel>> GetMerchantAssignCouirerAndServiceByCourierUserId(int courierUserId)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "courierUserId", value: courierUserId, dbType: DbType.Int32);
                var data = await connection.QueryAsync<AssignCouirerAndServiceViewModel>(
                        sql: @"[DT].[USP_MerchantAssignCouirerAndServiceBycourierUserId]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<CourierOrders> UpdateInvoiceNumber(CourierOrders courierOrders)
        {
            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == courierOrders.Id);
            if (entity != null)
            {
                entity.InvoiceNumber = courierOrders.InvoiceNumber;
                entity.InvoiceCourier = courierOrders.InvoiceCourier;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return entity;
            }
            return null;
        }

        public async Task<int> UpdateRangeInvoiceNumber(List<CourierOrders> courierOrders)
        {
            
            var courierOrdersIds = (from d in courierOrders
                                    select d.Id).ToArray();

            var entity = await _sqlServerContext.CourierOrders.AsNoTracking().Where(x => courierOrdersIds.Contains(x.Id))
               .BatchUpdateAsync(x => new CourierOrders { 
                   InvoiceNumber = courierOrders.FirstOrDefault().InvoiceNumber, 
                   InvoiceCourier = courierOrders.FirstOrDefault().InvoiceCourier });

            return entity;
        }

        public async Task<List<Districts>> LoadAllDistrictsByIds(List<Districts> request)
        {
            var districtIds = (from data in request
                               select data.DistrictId).ToArray();

            var districtInfo = await _sqlServerContext.Districts.AsNoTracking().Where(x => districtIds.Contains(x.DistrictId) && x.IsActive == true).ToListAsync();

            return districtInfo;
        }

        public async Task<IEnumerable<OrderStatusHistoryViewModel>> GetQuickOfficeReceivedDetails(int userId, string hubName)
        {

            IQueryable<OrderStatusHistoryViewModel> data =
                from history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                join courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                on history.MerchantId equals courierUsers.CourierUserId
                where history.PostedOn.Date >= DateTime.Now.Date
                && history.PostedOn.Date <DateTime.Now.Date.AddDays(1)
                && history.Status.Equals(7)
                && history.IsConfirmedBy.Equals("admin")
                && history.PostedBy.Equals(userId)
                select new OrderStatusHistoryViewModel
                {
                    CourierOrderId = history.CourierOrderId,
                    PostedOn = history.PostedOn,
                    PodNumber = history.PodNumber,
                    Comment = history.Comment,
                    HubName = history.HubName,
                    CourierUsersView = new  CourierUsersViewModel
                    {
                        CompanyName = courierUsers.CompanyName
                    }
                };

            return await data.OrderByDescending(x => x.CourierOrderId).ToListAsync();
        }

        public async Task<IEnumerable<CourierOrderViewModel>> GetQuickUpdateStatusDetails(RequestBodyModel bodyModel)
        {
            //var statusArray = new int[] { 9, 10, 36, 37 };

            var stringArray = bodyModel.StatusIds.Split(new[] { ',' });
            int[] statusArray = stringArray.Select(int.Parse).ToArray();
            //int[] myInts1 = Array.ConvertAll(stringArray, int.Parse);
            //int[] myInts2 = Array.ConvertAll(stringArray, s => int.Parse(s));

            IQueryable<CourierOrderViewModel> data =
                from orders in _sqlServerContext.CourierOrders.AsNoTracking()
                join _district in _sqlServerContext.Districts.AsNoTracking()
                on orders.DistrictId equals _district.DistrictId into d from district in d.DefaultIfEmpty()
                join _thana in _sqlServerContext.Districts.AsNoTracking()
                on orders.ThanaId equals _thana.DistrictId into t from thana in t.DefaultIfEmpty()
                join _area in _sqlServerContext.Districts.AsNoTracking()
                on orders.AreaId equals _area.DistrictId into a from area in a.DefaultIfEmpty()
                where orders.UpdatedOn.Date >= bodyModel.FromDate.Date
                && orders.UpdatedOn.Date < bodyModel.ToDate.Date.AddDays(1)
                && orders.HubName.Equals(bodyModel.HubName)
                && statusArray.Contains(orders.Status)
                && orders.CourierId == (bodyModel.CourierId == 0 ? orders.CourierId : bodyModel.CourierId)

                select new CourierOrderViewModel
                {
                    Id = orders.Id,
                    CustomerName = orders.CustomerName,
                    PodNumber = orders.PodNumber,
                    HubName = orders.HubName,
                    StatusType = "",
                    Comment = orders.Comment == null ? "": orders.Comment,
                    CourierAddressContactInfo = new CourierAddressContactInfo
                    {
                        Mobile = orders.Mobile,
                        OtherMobile = orders.OtherMobile == null ? "": orders.OtherMobile,
                        Address = orders.Address == null ? "": orders.Address,
                        DistrictNameEng = district.District,
                        RedxHubName = thana.RedxHubName == null ? "" : thana.RedxHubName,
                        ThanaPostalCode = thana.PostalCode == null ? "" : thana.PostalCode,
                        AreaPostalCode = area.PostalCode == null ? "" : area.PostalCode
                    },
                    CourierPrice =  new CourierPrice
                    {
                        CollectionAmount = orders.CollectionAmount,
                        CourierCharge = orders.CourierCharge
                    }
                };

            return await data.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<DeliveryChargeMerchantDetails> GetDeliveryChargeMerchantDetailsCourier(DeliveryChargeMerchantDetails request)
        {
            var response = await (from _deliveryChargeMerchantDetails in _sqlServerContext.DeliveryChargeMerchantDetails.AsNoTracking()

                                  where _deliveryChargeMerchantDetails.DistrictId.Equals(request.DistrictId)
                                  && _deliveryChargeMerchantDetails.ThanaId.Equals(request.ThanaId)
                                  && _deliveryChargeMerchantDetails.AreaId.Equals(request.AreaId)
                                  && _deliveryChargeMerchantDetails.WeightRangeId.Equals(2)
                                  && _deliveryChargeMerchantDetails.DeliveryRangeId.Equals(request.DeliveryRangeId)
                                  && _deliveryChargeMerchantDetails.ServiceType.Equals(request.ServiceType)
                                  && _deliveryChargeMerchantDetails.CourierUserId.Equals(request.CourierUserId)
                                  && _deliveryChargeMerchantDetails.IsActive == true

                                  select _deliveryChargeMerchantDetails).FirstOrDefaultAsync();
            return response;
        }
        public async Task<List<DeliveryUsersViewModel>> GetLocationWiseRiders(List<LocationAssign> locationAssigns)
        {
            var thanaIds = (from d in locationAssigns
                                   select d.ThanaId).ToArray();

            var data = from _locationAssign in _sqlServerContext.LocationAssign.AsNoTracking()
                                join _deliveryUsers in _sqlServerContext.DeliveryUsers on _locationAssign.DeliveryUserId equals _deliveryUsers.Id
                                

                                where thanaIds.Contains(_locationAssign.ThanaId)
                                && _deliveryUsers.IsActive.Equals(1)
                                select new DeliveryUsersViewModel
                                {
                                    Id = _deliveryUsers.Id,
                                    Name = _deliveryUsers.Name
                                };


            return await data.Distinct().ToListAsync();
        }

        public async Task<int> UpdateMultipleOrdersWithRider(List<CourierOrders> courierOrders)
        {

            var orderIds = (from d in courierOrders
                                   select d.Id).ToArray();
            
            var comment = courierOrders.Select(x => x.Comment).FirstOrDefault();
            var status = courierOrders.Select(x => x.Status).FirstOrDefault();
            //var isConfirmberBy = courierOrders.Select(x => x.IsConfirmedBy).FirstOrDefault();
            var hubName = courierOrders.Select(x => x.HubName).FirstOrDefault();
            var updatedBy = courierOrders.Select(x => x.UpdatedBy).FirstOrDefault();

            var entity = await _sqlServerContext.CourierOrders.AsNoTracking().Where(x => orderIds.Contains(x.Id))
               .BatchUpdateAsync(x => new CourierOrders
               { 
                   DeliveryUserId = courierOrders.FirstOrDefault().DeliveryUserId,
                   Status = status,
                   Comment = comment,
                   IsConfirmedBy = "admin",
                   HubName = hubName,
                   UpdatedBy = updatedBy,
                   UpdatedOn = DateTime.Now
               });

            var entry = _sqlServerContext.CourierOrders.AsNoTracking().Where(x => orderIds.Contains(x.Id));


            var courierOrderStatusHistory = entry.Select(x => new CourierOrderStatusHistory()
            {
                CourierOrderId = x.CourierOrdersId,
                IsConfirmedBy = x.IsConfirmedBy,
                OrderDate = x.OrderDate,
                Status = x.Status,
                PostedBy = x.UpdatedBy,
                MerchantId = x.MerchantId,
                Comment = x.Comment,
                PodNumber = x.PodNumber,
                CourierId = x.CourierId,
                HubName = x.HubName,
                CourierDeliveryManName = x.CourierDeliveryManName,
                CourierDeliveryManMobile = x.CourierDeliveryManMobile,
                PostedOn = x.UpdatedOn
            }).ToList();

            await _sqlServerContext.BulkInsertAsync(courierOrderStatusHistory);

            return entity;
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetFirstTimeOrderedMerchantList(RequestBodyModel request)
        {

            try
            {
                using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
                {
                    connection.Open();

                    var parameter = new DynamicParameters();
                    parameter.Add(name: "@FromDate", value: request.FromDate, dbType: DbType.DateTime);
                    parameter.Add(name: "@ToDate", value: request.ToDate, dbType: DbType.DateTime);

                    var data = (await connection.QueryAsync<CourierOrdersViewModel>(
                            sql: @"[DT].[USP_GetFirstTimeOrderedMerchantList]",
                            param: parameter,
                            commandType: CommandType.StoredProcedure)).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //TODO
            //var data = await (from _orders in _sqlServerContext.CourierOrders.AsNoTracking()

            //                  join _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
            //                  on   _orders.MerchantId equals _courierUsers.CourierUserId

            //                  where _orders.OrderDate.Date >= request.FromDate.Date
            //                  && _orders.OrderDate.Date < request.ToDate.Date.AddDays(1)
            //                  select new
            //                  {
            //                      _orders.Id,
            //                      _orders.OrderDate,
            //                      _orders.OrderType,
            //                      _orders.MerchantId,
            //                      _orders.ActualPackagePrice,
            //                      _courierUsers.CompanyName,
            //                      _courierUsers.JoinDate,
            //                      _courierUsers.Mobile,
            //                      _courierUsers.AlterMobile,
            //                      _courierUsers.Address,
            //                      _courierUsers.EmailAddress

            //                  }).Distinct().ToListAsync();
            //var courierOrdersViewModel = data.GroupBy(g => g.MerchantId).Select(s => new CourierOrdersViewModel
            //{

            //    MerchantId = s.Key,
            //    Id = s.FirstOrDefault().Id,
            //    OrderDate = s.FirstOrDefault().OrderDate,
            //    OrderType = s.FirstOrDefault().OrderType,
            //    ActualPackagePrice = s.FirstOrDefault().ActualPackagePrice,
            //    CourierUsers = new CourierUsersViewModel
            //    {
            //        CompanyName = s.FirstOrDefault().CompanyName,
            //        JoinDate = s.FirstOrDefault().JoinDate,
            //        Mobile = s.FirstOrDefault().Mobile,
            //        AlterMobile = s.FirstOrDefault().AlterMobile,
            //        Address = s.FirstOrDefault().Address,
            //        EmailAddress = s.FirstOrDefault().EmailAddress
            //    },
            //    TotalOrder = s.Select(item => item.MerchantId).Distinct().Count()

            //});

            //return courierOrdersViewModel.ToList();
        }

        public async Task<List<VouchersViewModel>> GetMerchantAssignedVoucher(List<VouchersViewModel> vouchers)
        {
            var voucher = await (from _vouchers in _sqlServerContext.Vouchers.Where(x => x.CourierUserId == vouchers.FirstOrDefault().CourierUserId && x.IsActive == true)
                                 join _deliveryRange in _sqlServerContext.DeliveryRange
                                 on _vouchers.DeliveryRangeId equals _deliveryRange.Id
                                 select new VouchersViewModel
                                 {
                                     MerchantMobile = _vouchers.MerchantMobile,
                                     VoucherCode = _vouchers.VoucherCode,
                                     StartTime = _vouchers.StartTime,
                                     EndTime = _vouchers.EndTime,
                                     ApplicableQuantity = _vouchers.ApplicableQuantity,
                                     VoucherDiscount = _vouchers.VoucherDiscount,
                                     CourierUserId = _vouchers.CourierUserId,
                                     IsActive = _vouchers.IsActive,
                                     DeliveryRangeId = _vouchers.DeliveryRangeId,
                                     DeliveryRangeName = _deliveryRange.Name
                                 }).ToListAsync();

            if (voucher.Count() > 0)
            {
                List<VouchersViewModel> voucherToReturn = new List<VouchersViewModel>();

                if (voucher.FirstOrDefault().VoucherCode == vouchers.FirstOrDefault().VoucherCode
                    && voucher.FirstOrDefault().CourierUserId == vouchers.FirstOrDefault().CourierUserId
                    //&& voucher.DeliveryRangeId == vouchers.DeliveryRangeId
                    )
                {
                    foreach (var item1 in vouchers)
                    {
                        foreach (var item2 in voucher)
                        {
                            if (item2.DeliveryRangeId == item1.DeliveryRangeId)
                            {
                                voucherToReturn.Add(item2);
                            }
                        }
                    }

                    if(voucherToReturn.Count() != 0) return voucherToReturn;
                }

                if (voucher.FirstOrDefault().CourierUserId == vouchers.FirstOrDefault().CourierUserId
                    //&& voucher.DeliveryRangeId == vouchers.DeliveryRangeId
                    )
                {
                    foreach (var item1 in vouchers)
                    {
                        foreach (var item2 in voucher)
                        {
                            if (item2.DeliveryRangeId == item1.DeliveryRangeId)
                            {
                                voucherToReturn.Add(item2);
                            }
                        }
                    }
                    if (voucherToReturn.Count() != 0) return voucherToReturn;
                }
                
                    return null;
            }

            return null;
        }

        public async Task<int> UpdatePriceWithOrderType(CourierOrders courierOrders)
        {
            int startIndex = 3;
            decimal codCharge = 0;
            int endIndex = courierOrders.CourierOrdersId.Length - 3;
            int id = Convert.ToInt32(courierOrders.CourierOrdersId.Substring(startIndex, endIndex));

            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == id);

            if (entity != null)
            {
                if (entity.PaymentServiceType != 1)
                {
                    if (courierOrders.CollectionAmount != 0)
                    {
                        if (entity.DistrictId.Equals(14))
                        {
                            codCharge = 5;
                        }
                        else
                        {
                            codCharge = courierOrders.CollectionAmount <= 1000 ? 10 : (courierOrders.CollectionAmount * 1) / 100;
                        }
                    }
                    else
                    {
                        codCharge = 0;
                    }
                }
                else
                {
                    codCharge = 0;
                }

                entity.OrderType = courierOrders.OrderType;
                entity.CollectionAmount = courierOrders.CollectionAmount;
                if (courierOrders.CollectionAmount > 0)
                {
                    entity.ActualPackagePrice = courierOrders.CollectionAmount;
                }
                entity.CodCharge = codCharge;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }

                var priceAndOrderTypeHistory = new PriceAndOrderTypeHistory()
                {
                    Id = id,
                    OrderType = courierOrders.OrderType,
                    CollectionAmount = courierOrders.CollectionAmount,
                    CodCharge = codCharge,
                    PostedOn = DateTime.Now,
                    PostedBy = courierOrders.UpdatedBy
                };

                await _sqlServerContext.PriceAndOrderTypeHistory.AddAsync(priceAndOrderTypeHistory);
                await _sqlServerContext.SaveChangesAsync();

                return 1;
            }
            return 0;
        }

        public async Task<dynamic> GetOrderDetails(int orderId)
        {

            var data = from _order in _sqlServerContext.CourierOrders.AsNoTracking()
                        join _couriers in _sqlServerContext.Couriers.AsNoTracking()
                        on _order.CourierId equals _couriers.CourierId
                        where _order.Id.Equals(orderId)
                        select new
                        {
                            Id = _order.Id,
                            CourierOrdersId = _order.CourierOrdersId,
                            PodNumber = _order.PodNumber,
                            CourierId = _couriers.CourierId,
                            CourierName = _couriers.CourierName
                        };

            return  await data.ToListAsync();
        }

        public async Task<int> UpdateServiceType(CourierOrders courierOrders)
        {
            int startIndex = 3;
            int endIndex = courierOrders.CourierOrdersId.Length - 3;
            int id = Convert.ToInt32(courierOrders.CourierOrdersId.Substring(startIndex, endIndex));

            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.ServiceType = courierOrders.ServiceType;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return 1;
            }
            return 0;
        }

        public async Task<IEnumerable<WeightRangeWiseData>> GetSpecialService(RequestBodyModel request)
        {
            var query =
                       from m in _sqlServerContext.DeliveryChargeMerchantDetails
                       join dr in _sqlServerContext.DeliveryRange
                       on m.DeliveryRangeId equals dr.Id
                       where m.CourierUserId.Equals(request.MerchantId)
                       && m.DistrictId.Equals(request.DistrictId)
                       && m.ThanaId.Equals(request.ThanaId)
                       && m.AreaId.Equals(request.AreaId)
                       && m.WeightRangeId.Equals(2)
                        && m.IsActive.Equals(true)
                       select new WeightRangeWiseData
                       {

                           DeliveryRangeId = m.DeliveryRangeId,
                           WeightRangeId = m.WeightRangeId,
                           ChargeAmount = dr.CourierDeliveryCharge,
                           Type = dr.Type,
                           Days = dr.Day,
                           DeliveryType = dr.Name,
                           Ranking = dr.Ranking,
                           DayType = dr.DayType,
                           OnImageLink = dr.OnImageLink,
                           OffImageLink = dr.OffImageLink,
                           ShowHide = dr.ShowHide,
                           DeliveryAlertMessage = dr.DeliveryAlertMessage,
                           LoginHours = dr.LoginHours,
                           DateAdvance = dr.DateAdvance,
                           DeliveryCharge = dr.CourierDeliveryCharge,
                           //DeliveryCharge = m.CourierDeliveryCharge,
                           ExtraDeliveryCharge = 0,
                           ExtraCollectionCharge = 0
                       };

            return await query.ToListAsync();
        }

        public async Task<int> UpdateMerchantReview(int CourierUserId, CourierUsers courierUsers)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(u => u.CourierUserId == CourierUserId);

            if(entity != null)
            {
                entity.MerchantReview = courierUsers.MerchantReview;
                entity.Recommendation = courierUsers.Recommendation;

                _sqlServerContext.CourierUsers.Update(entity);
                return await _sqlServerContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<CourierUsers> UpdateCourierUserBankInformation(int CourierUserId, CourierUsers courierUsers)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(u => u.CourierUserId == CourierUserId);

            entity.AccountNumber = courierUsers.AccountNumber;
            entity.AccountName = courierUsers.AccountName;
            entity.RoutingNumber = courierUsers.RoutingNumber;
            entity.BkashNumber = courierUsers.BkashNumber;

            _sqlServerContext.CourierUsers.Update(entity);
            await _sqlServerContext.SaveChangesAsync();

            return courierUsers;
        } 

        public async Task<IEnumerable<CouriersViewModel>> GetAllCouriers()
        {
            var data = from couriers in _sqlServerContext.Couriers.AsNoTracking()
                       where couriers.IsActive.Equals(true)
                       select new CouriersViewModel
                       {
                           CourierId = couriers.CourierId,
                           CourierName = couriers.CourierName,
                           ContactNo = couriers.ContactNo,
                           ContactAddress = couriers.ContactAddress,
                           IsActive = couriers.IsActive,
                           UserName = couriers.UserName,
                           Password = couriers.Password,
                           IsPresent = couriers.IsPresent,
                           IsOwnTiger = couriers.IsOwnTiger
                       };

            return await data.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> SameDayCollectedPendingOrdersCount(RequestBodyModel requestBody)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: requestBody.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ToDate", value: requestBody.ToDate, dbType: DbType.DateTime);
                var data = await connection.QueryAsync<dynamic>(
                        sql: @"[DT].[USP_SameDayCollectedPendingOrdersCount]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data;

            }
        }
    }
}
