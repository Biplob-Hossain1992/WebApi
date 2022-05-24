using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DapperDataModel;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ProcedureDataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.Referrer;
using AdCourier.Domain.Entities.ViewModel.RegisteredUsers;
using AdCourier.Domain.Entities.ViewModel.Report;
using AdCourier.Domain.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class OrderReportRepository : IOrderReportRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        public OrderReportRepository(IOptions<ConnectionStringList> connectionStrings, SqlServerContext sqlServerContext)
        {
            _connectionStrings = connectionStrings.Value;
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<IEnumerable<AdCourier.Domain.Entities.ViewModel.CourierUsers.CourierUsersViewModel>> GetPreferredPaymentCycle(OrderBodyModel orderBodyModel)
        {
            //if (orderBodyModel.PreferredPaymentCycle.Trim() == "instant")
            //{
            //    var data = _sqlServerContext.CourierUsers.Where(x => x.PreferredPaymentCycle.Equals(orderBodyModel.PreferredPaymentCycle) && x.JoinDate.Date >= orderBodyModel.FromDate.Date && x.JoinDate.Date < orderBodyModel.ToDate.Date.AddDays(1));

            //    return await data.ToListAsync();
            //}
            //else if (orderBodyModel.PreferredPaymentCycle.Trim() == "week")
            //{
            //    var data = _sqlServerContext.CourierUsers.Where(x => x.PreferredPaymentCycle.Equals(orderBodyModel.PreferredPaymentCycle) && x.JoinDate.Date >= orderBodyModel.FromDate && x.JoinDate.Date < orderBodyModel.ToDate.AddDays(1));

            //    return await data.ToListAsync();
            //}

            //var instantPayment = new List<AdCourier.Domain.Entities.ViewModel.CourierUsers.CourierUsersViewModel>();

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: orderBodyModel.FromDate, dbType: DbType.String);
                parameter.Add(name: "@ToDate", value: orderBodyModel.ToDate.AddDays(1), dbType: DbType.String);
                parameter.Add(name: "@preferredPaymentCycle", value: orderBodyModel.PreferredPaymentCycle, dbType: DbType.String);

                return (await connection.QueryAsync<AdCourier.Domain.Entities.ViewModel.CourierUsers.CourierUsersViewModel>(
                        sql: @"[DT].[USP_GetInstantPaymentRelatedInfo]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task<IEnumerable<LastMileInformationDapperModel>> GetLastMileInformation(OrderBodyModel orderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ToDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);

                return (await connection.QueryAsync<LastMileInformationDapperModel>(
                    sql: @"[DT].[Usp_LastMileInformation]",
                    param: parameter,
                    commandType: CommandType.StoredProcedure
                    )).ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> RiderOrderHistory(OrderBodyModel orderBodyModel)
        {


            IQueryable<dynamic> data = (from history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                        join man in _sqlServerContext.DeliveryUsers.AsNoTracking()
                                        on history.PostedBy equals man.Id
                                        join status in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                                        on history.Status equals status.StatusId
                                        where history.PostedOn.Date >= orderBodyModel.FromDate
                                        && history.PostedOn.Date < orderBodyModel.ToDate.AddDays(1)
                                        && history.IsConfirmedBy.Equals("deliveryman")
                                        && status.FulfillmentStatusGroup.Equals("DeliveryBondu")
                                        select new
                                        {
                                            man.Name,
                                            history.CourierOrderId,
                                            history.PostedBy,
                                            status.StatusNameEng,
                                            status.StatusId
                                        }).GroupBy(x => x.Name)
                                        .Select(y => new
                                        {
                                            Name = y.Key,
                                            OrderCount = y.Select(s => s.CourierOrderId).ToList(),
                                            StatusNameEng = y.Select(s => s.StatusNameEng).ToList()
                                        }).OrderByDescending(x => x.Name);

            return await data.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> OrderAssign(OrderBodyModel orderBodyModel)
        {


            IQueryable<dynamic> data = (from bonduAssign in _sqlServerContext.DeliveryBonduAssign.AsNoTracking()
                                        join man in _sqlServerContext.DeliveryUsers.AsNoTracking()
                                        on bonduAssign.DeliveryManUserId equals man.Id
                                        join order in _sqlServerContext.CourierOrders.AsNoTracking()
                                        on bonduAssign.OrderId equals order.Id
                                        where bonduAssign.UpdatedOn.Date >= orderBodyModel.FromDate
                                        && bonduAssign.UpdatedOn.Date <= orderBodyModel.ToDate
                                        select new
                                        {
                                            man.Name,
                                            bonduAssign.OrderId,
                                            order.CourierOrdersId
                                        }).GroupBy(x => x.Name)
                                        .Select(y => new
                                        {
                                            Name = y.Key,
                                            AssginOrder = y.Select(s => s.OrderId).Distinct().Count(),
                                            AssginOrderStr = string.Join(",", y.Select(s => s.OrderId).Distinct()),
                                            AssginOrderStrDt = string.Join(",", y.Select(s => s.CourierOrdersId).Distinct())
                                        });

            return await data.ToListAsync();
        }

        public async Task<CourierConsignmentViewModel> CourierConsignment(OrderBodyModel orderBodyModel)
        {
            int[] array = new[] { 8, 9, 10 };
            var courierConsignmentViewModel = new CourierConsignmentViewModel();
            var courierGroup = new CourierGroup();

            IQueryable<CourierGroup> dataDhaka = (from orderh in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                                  join courier in _sqlServerContext.Couriers.AsNoTracking()
                                                  on orderh.CourierId equals courier.CourierId
                                                  join order in _sqlServerContext.CourierOrders.AsNoTracking()
                                                  on orderh.CourierOrderId equals order.CourierOrdersId
                                                  where array.Contains(orderh.Status)
                                                  && order.DistrictId.Equals(14)
                                                  && orderh.PostedOn.Date >= orderBodyModel.FromDate
                                                  && orderh.PostedOn.Date <= orderBodyModel.ToDate
                                                  select new CourierReportViewModel
                                                  {
                                                      CourierId = courier.CourierId,
                                                      CourierName = courier.CourierName,
                                                      PodNumber = orderh.PodNumber,
                                                      MerchantId = orderh.MerchantId
                                                  }).GroupBy(x => x.CourierName)
                                        .Select(y => new CourierGroup
                                        {
                                            CourierId = y.FirstOrDefault().CourierId,
                                            CourierName = y.Key,
                                            PodNumber = y.Select(s => s.PodNumber).Distinct().Count(),
                                            MerchantCount = y.Select(s => s.MerchantId).Distinct().Count()
                                        });

            IQueryable<CourierGroup> dataOutDhaka = (from orderh in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                                     join courier in _sqlServerContext.Couriers.AsNoTracking()
                                                     on orderh.CourierId equals courier.CourierId
                                                     join order in _sqlServerContext.CourierOrders.AsNoTracking()
                                                     on orderh.CourierOrderId equals order.CourierOrdersId
                                                     where array.Contains(orderh.Status)
                                                     && order.DistrictId != 14
                                                     && orderh.PostedOn.Date >= orderBodyModel.FromDate
                                                     && orderh.PostedOn.Date <= orderBodyModel.ToDate
                                                     select new CourierReportViewModel
                                                     {
                                                         CourierId = courier.CourierId,
                                                         CourierName = courier.CourierName,
                                                         PodNumber = orderh.PodNumber,
                                                         MerchantId = orderh.MerchantId
                                                     }).GroupBy(x => x.CourierName)
                            .Select(y => new CourierGroup
                            {
                                CourierId = y.FirstOrDefault().CourierId,
                                CourierName = y.Key,
                                PodNumber = y.Select(s => s.PodNumber).Distinct().Count(),
                                MerchantCount = y.Select(s => s.MerchantId).Distinct().Count()
                            });

            IQueryable<CourierReportViewModel> courierReportViewModel = from orderh in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                                                        join courier in _sqlServerContext.Couriers.AsNoTracking()
                                                                        on orderh.CourierId equals courier.CourierId
                                                                        join order in _sqlServerContext.CourierOrders.AsNoTracking()
                                                                        on orderh.CourierOrderId equals order.CourierOrdersId
                                                                        where array.Contains(orderh.Status)
                                                                        && orderh.PostedOn.Date >= orderBodyModel.FromDate
                                                                        && orderh.PostedOn.Date <= orderBodyModel.ToDate
                                                                        select new CourierReportViewModel
                                                                        {
                                                                            CourierName = courier.CourierName,
                                                                            PodNumber = orderh.PodNumber,
                                                                            MerchantId = orderh.MerchantId,
                                                                            DistrictId = order.DistrictId
                                                                        };

            var podNumberDhaka = courierReportViewModel.Where(x => x.DistrictId.Equals(14)).Select(x => x.PodNumber).Distinct().Count();
            var merchantIdDhaka = courierReportViewModel.Where(x => x.DistrictId.Equals(14)).Select(x => x.MerchantId).Distinct().Count();

            var podNumberOutSideDhaka = courierReportViewModel.Where(x => x.DistrictId != 14).Select(x => x.PodNumber).Distinct().Count();
            var merchantIdOutSideDhaka = courierReportViewModel.Where(x => x.DistrictId != 14).Select(x => x.MerchantId).Distinct().Count();


            courierGroup.PodNumber = podNumberDhaka;
            courierGroup.MerchantCount = merchantIdDhaka;

            courierGroup.PodNumberOutSide = podNumberOutSideDhaka;
            courierGroup.MerchantCountOutSide = merchantIdOutSideDhaka;

            courierConsignmentViewModel.courierDhakaGroupList = await dataDhaka.ToListAsync();
            courierConsignmentViewModel.courierOutSideDhakaGroupList = await dataOutDhaka.ToListAsync();
            courierConsignmentViewModel.courierGroup = courierGroup;

            return courierConsignmentViewModel;
        }

        public async Task<IEnumerable<CourierReportViewModel>> CourierConsignmentDetails(RequestBodyModel requestBodyModel)
        {
            int[] status = new[] { 8, 9, 10 };

            if (requestBodyModel.Type == "InsideDhaka")
            {
                IQueryable<CourierReportViewModel> data = (from orderh in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                                           join courier in _sqlServerContext.Couriers.AsNoTracking()
                                                           on orderh.CourierId equals courier.CourierId
                                                           join order in _sqlServerContext.CourierOrders.AsNoTracking()
                                                           on orderh.CourierOrderId equals order.CourierOrdersId
                                                           join courierusers in _sqlServerContext.CourierUsers.AsNoTracking()
                                                           on order.MerchantId equals courierusers.CourierUserId
                                                           where status.Contains(orderh.Status)
                                                           && order.DistrictId == 14
                                                           && orderh.CourierId == requestBodyModel.CourierId
                                                           && orderh.PostedOn.Date >= requestBodyModel.FromDate
                                                           && orderh.PostedOn.Date <= requestBodyModel.ToDate
                                                           select new CourierReportViewModel
                                                           {
                                                               CourierOrderId = order.CourierOrdersId,
                                                               PodNumber = orderh.PodNumber,
                                                               CompanyName = courierusers.CompanyName
                                                           }).Distinct();
                return await data.ToListAsync();
            }
            else if (requestBodyModel.Type == "OutsideDhaka")
            {
                IQueryable<CourierReportViewModel> data = (from orderh in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                                           join courier in _sqlServerContext.Couriers.AsNoTracking()
                                                           on orderh.CourierId equals courier.CourierId
                                                           join order in _sqlServerContext.CourierOrders.AsNoTracking()
                                                           on orderh.CourierOrderId equals order.CourierOrdersId
                                                           join courierusers in _sqlServerContext.CourierUsers.AsNoTracking()
                                                           on order.MerchantId equals courierusers.CourierUserId
                                                           where status.Contains(orderh.Status)
                                                           && order.DistrictId != 14
                                                           && orderh.CourierId == requestBodyModel.CourierId
                                                           && orderh.PostedOn.Date >= requestBodyModel.FromDate
                                                           && orderh.PostedOn.Date <= requestBodyModel.ToDate
                                                           select new CourierReportViewModel
                                                           {
                                                               CourierOrderId = order.CourierOrdersId,
                                                               PodNumber = orderh.PodNumber,
                                                               CompanyName = courierusers.CompanyName
                                                           }).Distinct();
                return await data.ToListAsync();
            }
            return null;
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> DeliveredOrderDetails(OrderBodyModel orderBodyModel)
        {
            //bool IsCourier;
            //if (orderBodyModel.CourierId == 0) IsCourier = false;
            //else IsCourier = true;

            var data = await (from h in _sqlServerContext.CourierOrderStatusHistory
                              join o in _sqlServerContext.CourierOrders on h.CourierOrderId equals o.CourierOrdersId
                              join m in _sqlServerContext.CourierUsers on o.MerchantId equals m.CourierUserId
                              join c in _sqlServerContext.Couriers on o.CourierId equals c.CourierId
                              where h.PostedOn >= orderBodyModel.FromDate.Date
                              && h.PostedOn < orderBodyModel.ToDate.Date.AddDays(1)
                              && h.Status == 15
                              && h.CourierId == orderBodyModel.CourierId
                              && o.IsDownloaded == true
                              select new CourierOrdersViewModel
                              {
                                  CourierOrdersId = o.CourierOrdersId,
                                  MerchantId = o.MerchantId,
                                  CourierUsers = new CourierUsersViewModel
                                  {
                                      CompanyName = m.CompanyName
                                  },
                                  CollectionAmount = o.CollectionAmount,
                                  PackagingCharge = o.PackagingCharge,
                                  DeliveryCharge = o.DeliveryCharge,
                                  CodCharge = o.CodCharge,
                                  BreakableCharge = o.BreakableCharge,
                                  CollectionCharge = o.CollectionCharge,
                                  ReturnCharge = o.ReturnCharge,
                                  TotalCharge = (o.CollectionAmount + o.PackagingCharge + o.DeliveryCharge + o.CodCharge + o.BreakableCharge + o.CollectionCharge + o.ReturnCharge),
                                  DeliveredDate = h.PostedOn,
                                  Couriers = new CouriersViewModel
                                  {
                                      CourierId = c.CourierId,
                                      CourierName = c.CourierName
                                  },
                                  PodNumber = o.PodNumber,
                                  IsDownloaded = o.IsDownloaded,
                                  DownloadedDate = o.DownloadedDate
                              })
                              //.Where(x => IsCourier ? x.Couriers.CourierId == orderBodyModel.CourierId : true)
                              .ToListAsync();

            var deliveredOrderDetailsData = data.OrderBy(o => o.CourierOrdersId).ThenBy(t => t.DeliveredDate).GroupBy(g => g.CourierOrdersId).Select(s => new CourierOrdersViewModel
            {
                CourierOrdersId = s.Key,
                MerchantId = s.FirstOrDefault().MerchantId,
                CourierUsers = new CourierUsersViewModel
                {
                    CompanyName = s.FirstOrDefault().CourierUsers.CompanyName
                },
                CollectionAmount = s.FirstOrDefault().CollectionAmount,
                PackagingCharge = s.FirstOrDefault().PackagingCharge,
                DeliveryCharge = s.FirstOrDefault().DeliveryCharge,
                CodCharge = s.FirstOrDefault().CodCharge,
                BreakableCharge = s.FirstOrDefault().BreakableCharge,
                CollectionCharge = s.FirstOrDefault().CollectionCharge,
                ReturnCharge = s.FirstOrDefault().ReturnCharge,
                TotalCharge = (s.FirstOrDefault().CollectionAmount + s.FirstOrDefault().PackagingCharge + s.FirstOrDefault().DeliveryCharge + s.FirstOrDefault().CodCharge + s.FirstOrDefault().BreakableCharge + s.FirstOrDefault().CollectionCharge + s.FirstOrDefault().ReturnCharge),
                DeliveredDate = s.FirstOrDefault().DeliveredDate,
                Couriers = new CouriersViewModel
                {
                    CourierName = s.FirstOrDefault().Couriers.CourierName
                },
                PodNumber = s.FirstOrDefault().PodNumber,
                IsDownloaded = s.FirstOrDefault().IsDownloaded,
                DownloadedDate = s.FirstOrDefault().DownloadedDate
            });

            return deliveredOrderDetailsData.ToList();
        }

        public async Task<IEnumerable<RetentionUserPerformanceDapperModel>> RetentionUsersPerformance(OrderBodyModel orderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add(name: "@FromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@ToDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@RetentionUserId", value: orderBodyModel.RetentionUserId, dbType: DbType.Int32);

                return (await connection.QueryAsync<RetentionUserPerformanceDapperModel>(
                    sql: @"[Reports].[Usp_RetentionUsersPerformance]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    )).ToList();
            }
        }

        public async Task<ServiceTypeNew> GetDistrictSpeedByService(OrderBodyModel orderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add(name: "@FromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@ToDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);

                var serviceType = new ServiceTypeNew();
                var multi = await connection.QueryMultipleAsync(sql: @"[DT].[DtDistrictSpeedByService]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
                {
                    var districtSpeedModel = multi.Read<DistrictSpeedModel>().ToList();
                    var districtSpeedDetailsModel = multi.Read<DistrictSpeedDetailsModel>().ToList();

                    serviceType.DistrictSpeedModels = districtSpeedModel;
                    serviceType.DistrictSpeedDetailsModels = districtSpeedDetailsModel;
                };
                return serviceType;
            }
        }

        public async Task<IEnumerable<RetentionUserPerformanceDapperModel>> SRAssignedInactiveMerchantList(int retentionUserId, int inactiveDuration)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add(name: "@RetentionUserId", value: retentionUserId, dbType: DbType.Int32);
                parameters.Add(name: "@InactiveDuration", value: inactiveDuration, dbType: DbType.Int32);

                return (await connection.QueryAsync<RetentionUserPerformanceDapperModel>(
                    sql: @"[Reports].[SRAssignedInactiveMerchantList]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    )).ToList();
            }
        }

        public async Task<IEnumerable<RetentionComplainDetailsDapperModel>> RetentionUserWiseComplainDetails(int courierUserId)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@MerchantId", value: courierUserId, dbType: DbType.Int32);

                return (await connection.QueryAsync<RetentionComplainDetailsDapperModel>(
                    sql: @"[Reports].[RetentionUserWiseComplainDetails]",
                    param: parameter,
                    commandType: CommandType.StoredProcedure
                    )).ToList();
            }
        }

        public async Task<IEnumerable<CourierReportViewModel>> CourierConsignmentDetailsOutsideDhaka(RequestBodyModel requestBodyModel)
        {
            int[] status = new[] { 8, 9, 10 };

            var data3 = (from orderh in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                         join courier in _sqlServerContext.Couriers.AsNoTracking()
                         on orderh.CourierId equals courier.CourierId
                         join order in _sqlServerContext.CourierOrders.AsNoTracking()
                         on orderh.CourierOrderId equals order.CourierOrdersId
                         where status.Contains(orderh.Status)
                         && order.DistrictId == 14
                         && orderh.CourierId == requestBodyModel.CourierId
                         && orderh.PostedOn.Date >= requestBodyModel.FromDate
                         && orderh.PostedOn.Date <= requestBodyModel.ToDate
                         select new CourierReportViewModel
                         {
                             CourierOrderId = order.CourierOrdersId,
                             CourierId = courier.CourierId,
                             CourierName = courier.CourierName,
                             PodNumber = orderh.PodNumber,
                             MerchantId = orderh.MerchantId
                         }).Distinct();

            return await data3.ToListAsync();
        }

        public async Task<IEnumerable<MerchantOrder>> GetMerchantOrders(OrderBodyModel orderBodyModel)
        {
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@fromDate",
                            SqlDbType =  System.Data.SqlDbType.SmallDateTime,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = orderBodyModel.FromDate.Date
                        },
                        new SqlParameter() {
                            ParameterName = "@toDate",
                            SqlDbType =  System.Data.SqlDbType.SmallDateTime,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = orderBodyModel.ToDate.Date
                        }};

            _sqlServerContext.Database.SetCommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds);
            return await _sqlServerContext.MerchantOrder.FromSql("[DT].[USP_GetMerchantOrders] @fromDate, @toDate"
                , param).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> CollectorReceivedReportDetails(OrderBodyModel orderBodyModel)
        {
            IQueryable<dynamic> data = (from orderhistory in _sqlServerContext.CourierOrderStatusHistory
                                        join merchant in _sqlServerContext.CourierUsers
                                        on orderhistory.MerchantId equals merchant.CourierUserId
                                        where orderhistory.Status.Equals(3)
                                        && orderhistory.PostedBy.Equals(orderBodyModel.CollectorId)
                                        && orderhistory.PostedOn.Date >= orderBodyModel.FromDate.Date
                                        && orderhistory.PostedOn.Date < orderBodyModel.ToDate.AddDays(1).Date
                                        && orderhistory.IsConfirmedBy.Trim().ToLower() == "collector"
                                        select new
                                        {
                                            orderhistory.MerchantId,
                                            merchant.CompanyName,
                                            orderhistory.CourierOrderId,
                                            orderhistory.Comment,
                                            orderhistory.Status
                                        }).Distinct();

            return await data.ToListAsync();
        }
        public async Task<IEnumerable<CollectorReceived>> CollectorReceivedReport(OrderBodyModel orderBodyModel)
        {
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@FromDate",
                            SqlDbType =  System.Data.SqlDbType.SmallDateTime,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = orderBodyModel.FromDate.Date
                        },
                        new SqlParameter() {
                            ParameterName = "@ToDate",
                            SqlDbType =  System.Data.SqlDbType.SmallDateTime,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = orderBodyModel.ToDate.Date
                        }};

            _sqlServerContext.Database.SetCommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds);
            return await _sqlServerContext.CollectorReceived.FromSql("[DT].[USP_CollectorReceivedReport] @FromDate, @ToDate"
                , param).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<CourierOrders>> GetOrders(OrderBodyModel orderBodyModel)
        {
            IQueryable<CourierOrders> data = from order in _sqlServerContext.CourierOrders
                                             where order.OrderDate.Date >= orderBodyModel.FromDate
                                             && order.OrderDate.Date <= orderBodyModel.ToDate
                                             select new CourierOrders
                                             {
                                                 Id = order.Id,
                                                 CourierOrdersId = order.CourierOrdersId,
                                                 OrderDate = order.OrderDate,
                                                 DistrictId = order.DistrictId,
                                                 MerchantId = order.MerchantId,
                                                 Status = order.Status,
                                                 OrderType = order.OrderType,
                                                 CollectionAmount = order.CollectionAmount,
                                                 ActualPackagePrice = order.ActualPackagePrice,
                                                 DeliveryCharge = order.DeliveryCharge,
                                                 CodCharge = order.CodCharge,
                                                 CollectionCharge = order.CollectionCharge,
                                                 DeliveryRangeId = order.DeliveryRangeId,
                                                 OfficeDrop = order.OfficeDrop
                                             };
            return await data.ToListAsync();
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetReceivedOrders(OrderBodyModel orderBodyModel)
        {
            _sqlServerContext.Database.SetCommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds);


            IQueryable<CourierOrdersViewModel> data = (from order in _sqlServerContext.CourierOrders.AsNoTracking()
                                                       join status in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                                                       on order.Status equals status.StatusId
                                                       join merchant in _sqlServerContext.CourierUsers.AsNoTracking()
                                                       on order.MerchantId equals merchant.CourierUserId
                                                       join couriers in _sqlServerContext.Couriers.AsNoTracking()
                                                       on order.CourierId equals couriers.CourierId
                                                       join history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                                       on order.CourierOrdersId equals history.CourierOrderId
                                                       where history.Status.Equals(7)
                                                       && history.OrderDate.Month.Equals(orderBodyModel.Month)
                                                       && history.OrderDate.Year.Equals(orderBodyModel.Year)

                                                       select new CourierOrdersViewModel
                                                       {
                                                           CourierOrdersId = order.CourierOrdersId,
                                                           PodNumber = order.PodNumber,
                                                           DeliveryCharge = order.DeliveryCharge,
                                                           CodCharge = order.CodCharge,
                                                           BreakableCharge = order.BreakableCharge,
                                                           ReturnCharge = order.ReturnCharge,
                                                           PackagingCharge = order.PackagingCharge,
                                                           CollectionAmount = order.CollectionAmount,
                                                           OrderType = order.OrderType,
                                                           OrderDate = order.OrderDate,
                                                           ConfirmationDate = order.ConfirmationDate,

                                                           CourierOrderStatus = new OrderStatusViewModel
                                                           {
                                                               StatusNameEng = status.StatusNameEng,
                                                               StatusType = status.StatusType,
                                                           },
                                                           CourierUsers = new CourierUsersViewModel
                                                           {
                                                               CompanyName = merchant.CompanyName
                                                           },

                                                           Couriers = new CouriersViewModel
                                                           {
                                                               CourierName = couriers.CourierName
                                                           }

                                                       }).Distinct();

            return await data.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> OrderAgeingReport(string statusList)
        {
            int[] Status = statusList.Split(',').Select(Int32.Parse).ToArray();
            //var totalOrderCount = await _sqlServerContext.CourierOrders.Where(d => Status.Contains(d.Status)).CountAsync();
            _sqlServerContext.Database.SetCommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds);


            IQueryable<CourierOrderAgeingDataModel> data = (from order in _sqlServerContext.CourierOrders.AsNoTracking()
                                                            join merchant in _sqlServerContext.CourierUsers
                                                            on order.MerchantId equals merchant.CourierUserId

                                                            where Status.Contains(order.Status)

                                                            select new CourierOrderAgeingDataModel
                                                            {
                                                                CourierOrdersId = order.CourierOrdersId,
                                                                Status = order.Status,
                                                                Comment = order.Comment,
                                                                CollectionName = order.CollectionName,
                                                                CompanyName = merchant.CompanyName,
                                                                IsAdvOrder = order.OrderType.Trim() == "Only Delivery" ? 1 : 0,
                                                                TotalHour = Convert.ToInt32(DateTime.Now.AddHours(6).Subtract(Convert.ToDateTime(order.UpdatedOn)).TotalHours)
                                                            }).Distinct();
            return await data.ToListAsync();
        }

        public async Task<dynamic> GetReferrerRefereeList(OrderBodyModel orderBodyModel)
        {
            var data = await _sqlServerContext.CourierUsers.Where(x => x.Referrer != ""
            && x.JoinDate.Date >= orderBodyModel.FromDate.Date
            && x.JoinDate.Date < orderBodyModel.ToDate.Date.AddDays(1)).ToListAsync();

            var referee = data.GroupBy(x => new { Year = x.JoinDate.Year, Month = x.JoinDate.Month, Day = x.JoinDate.Day }).Select(z => new
            {
                JoinDate = z.Key.Year + "-" + z.Key.Month + "-" + z.Key.Day,
                RefereeCount = z.Count(),
                RefereeOrder = z.Count() * _sqlServerContext.Referee.Select(x => x.RefereeOrder).FirstOrDefault(),
                RefereeOrderUse = z.Sum(c => c.RefereeOrder)
            }).OrderByDescending(o => o.JoinDate).ToList();

            var referrer = new List<ReferrerDapperModel>();

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: orderBodyModel.FromDate, dbType: DbType.String);
                parameter.Add(name: "@ToDate", value: orderBodyModel.ToDate.AddDays(1), dbType: DbType.String);

                referrer = (await connection.QueryAsync<ReferrerDapperModel>(
                        sql: @"[DT].[USP_GetReferrerInfo]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure)).ToList();
            }

            return new
            {
                Referee = referee,
                Referrer = referrer
            };
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetOrderFromWiseOrderCount(OrderBodyModel orderBodyModel)
        {
            int[] status = new int[] { 0, 2, 29 };

            var data = _sqlServerContext.CourierOrders.Where(o => o.PostedOn >= orderBodyModel.FromDate.Date
            && o.PostedOn < orderBodyModel.ToDate.Date.AddDays(1)
            && !status.Contains(o.Status)
            && o.MerchantId != 1)
            .GroupBy(g => g.OrderFrom)
            .Select(s => new CourierOrdersViewModel
            {
                OrderFrom = s.Key,
                TotalOrder = s.Count(),
                TotalMerchant = s.Select(m => m.MerchantId).Distinct().Count()
            });

            return await data.ToListAsync();
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetMerchantOrderFromDetails(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            int[] status = new int[] { 0, 2, 29 };
            var data = (from _courierOrders in _sqlServerContext.CourierOrders
                        join _merchant in _sqlServerContext.CourierUsers on _courierOrders.MerchantId equals _merchant.CourierUserId
                        where _courierOrders.PostedOn >= loadCourierOrderBodyModel.FromDate.Date
                        && _courierOrders.PostedOn < loadCourierOrderBodyModel.ToDate.Date.AddDays(1)
                        && !status.Contains(_courierOrders.Status)
                        && _courierOrders.OrderFrom == loadCourierOrderBodyModel.OrderFrom
                        && _merchant.CourierUserId != 1
                        orderby _merchant.CourierUserId

                        //select new CourierOrdersViewModel
                        //{
                        //    CourierUsers = new CourierUsersViewModel
                        //    {
                        //        CompanyName = _merchant.CompanyName
                        //    },
                        //    CourierOrdersId = _courierOrders.CourierOrdersId,
                        //    OrderDate = _courierOrders.OrderDate
                        //};
                        select new CourierUsersViewModel
                        {
                            CourierUserId = _merchant.CourierUserId,
                            CompanyName = _merchant.CompanyName
                        })
                        .Distinct()
                        .Select(x => new CourierUsersViewModel
                        {
                            CompanyName = x.CompanyName
                        });

            return await data.ToListAsync();
        }

        public async Task<IEnumerable<AdCourier.Domain.Entities.DapperDataModel.CourierOrdersDapperModel>> PackageDateDeliveryDateReport(OrderBodyModel orderBodyModel)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
                {
                    connection.Open();

                    var parameter = new DynamicParameters();
                    parameter.Add(name: "@FromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                    parameter.Add(name: "@ToDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);
                    parameter.Add(name: "@CourierId", value: orderBodyModel.CourierId, dbType: DbType.Int32);
                    parameter.Add(name: "@DeliveryRangeIds", value: orderBodyModel.DeliveryRangeIds, dbType: DbType.String);
                    parameter.Add(name: "@DistrictId", value: orderBodyModel.DistrictId, dbType: DbType.Int32);
                    parameter.Add(name: "@DateFlag", value: orderBodyModel.DateFlag, dbType: DbType.Int32);
                    parameter.Add(name: "@TigerStatusId", value: orderBodyModel.TigerStatusId, dbType: DbType.Int32);

                    var data = (await connection.QueryAsync<AdCourier.Domain.Entities.DapperDataModel.CourierOrdersDapperModel>(
                            sql: @"[DT].[USP_PackageDateDeliveryDateReport]",
                            param: parameter,
                            commandType: CommandType.StoredProcedure)).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<AdCourier.Domain.Entities.DapperDataModel.CourierOrdersDapperModel>> PickDateDeliveryDateReport(OrderBodyModel orderBodyModel)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
                {
                    connection.Open();

                    var parameter = new DynamicParameters();
                    parameter.Add(name: "@FromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                    parameter.Add(name: "@ToDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);
                    parameter.Add(name: "@CourierId", value: orderBodyModel.CourierId, dbType: DbType.Int32);
                    parameter.Add(name: "@DeliveryRangeIds", value: orderBodyModel.DeliveryRangeIds, dbType: DbType.String);
                    parameter.Add(name: "@DistrictId", value: orderBodyModel.DistrictId, dbType: DbType.Int32);
                    parameter.Add(name: "@DateFlag", value: orderBodyModel.DateFlag, dbType: DbType.Int32);
                    parameter.Add(name: "@TigerStatusId", value: orderBodyModel.TigerStatusId, dbType: DbType.Int32);

                    var data = (await connection.QueryAsync<AdCourier.Domain.Entities.DapperDataModel.CourierOrdersDapperModel>(
                            sql: @"[DT].[USP_PickDateDeliveryDateReport]",
                            param: parameter,
                            commandType: CommandType.StoredProcedure)).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IEnumerable<AdCourier.Domain.Entities.DapperDataModel.CourierOrdersDapperModel>> GetDeliveryRangeTypeWiseOrders(OrderBodyModel orderBodyModel)
        {

            try
            {
                using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
                {
                    connection.Open();

                    var parameter = new DynamicParameters();
                    parameter.Add(name: "@FromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                    parameter.Add(name: "@ToDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);
                    parameter.Add(name: "@DeliveryRangeIds", value: orderBodyModel.DeliveryRangeIds, dbType: DbType.String);
                    parameter.Add(name: "@Hours", value: orderBodyModel.Hours, dbType: DbType.Int32);
                    parameter.Add(name: "@DistrictId", value: orderBodyModel.DistrictId, dbType: DbType.Int32);
                    parameter.Add(name: "@CourierId", value: orderBodyModel.CourierId, dbType: DbType.Int32);
                    parameter.Add(name: "@OutsideDhaka", value: orderBodyModel.OutsideDhaka, dbType: DbType.Int32);
                    parameter.Add(name: "@MerchantId", value: orderBodyModel.MerchantId, dbType: DbType.Int32);
                    parameter.Add(name: "@DateFlag", value: orderBodyModel.DateFlag, dbType: DbType.Int32);
                    parameter.Add(name: "@TigerStatusId", value: orderBodyModel.TigerStatusId, dbType: DbType.Int32);


                    var data = (await connection.QueryAsync<AdCourier.Domain.Entities.DapperDataModel.CourierOrdersDapperModel>(
                            sql: @"[DT].[USP_DeliveryRangeTypeWiseOrders]",
                            param: parameter,
                            commandType: CommandType.StoredProcedure)).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<RegisteredUsersViewModel>> GetAllRegisteredUsers(string joinDate)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@JoinDate", value: joinDate, dbType: DbType.String);

                var data = await connection.QueryAsync<RegisteredUsersViewModel>(
                        sql: @"[DT].[USP_RegisteredUsers]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<dynamic> GetRetentionAcquisitionUsers(int userId)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@UserId", value: userId, dbType: DbType.Int32);

                var retAcquiUsersModel = new RetentionAcquisitionUsersModel();
                var multi = await connection.QueryMultipleAsync(sql: @"[DT].[USP_RetentionAcquisitionUsers]",
                    param: parameter,
                    commandType: CommandType.StoredProcedure);

                {
                    var retentionUserModel = multi.Read<RetentionAcquisitionUsersViewModel>().ToList();

                    var acquisitionUserModel = multi.Read<RetentionAcquisitionUsersViewModel>().ToList();

                    retAcquiUsersModel.RetentionUserModel = retentionUserModel;
                    retAcquiUsersModel.AcquisitionUserModel = acquisitionUserModel;

                }

                return retAcquiUsersModel;
            }
        }

        public async Task<IEnumerable<MerchantDetailsResponseModel>> MerchantDetailsWithOrders(string joinDate, int flag)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@JoinDate", value: joinDate, dbType: DbType.String);
                parameter.Add(name: "@Flag", value: flag, dbType: DbType.Int32);

                var data = await connection.QueryAsync<MerchantDetailsResponseModel>(
                        sql: @"[Reports].[USP_AcquisitionManagerWiseMerchantDetailsWithOrders]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<dynamic> AcquisitionManagerWiseOrderDetails(RequestBodyModel requestBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add(name: "@JoinDate", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@AcquisitionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                parameters.Add(name: "@flag", value: requestBodyModel.Flag, dbType: DbType.Int32);
                parameters.Add(name: "@courierUserId", value: requestBodyModel.MerchantId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[AcquisitionManagerWiseOrderDetails]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<MerchantDetailsResponseModel>> MerchantDetails(string joinDate, int flag)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@JoinDate", value: joinDate, dbType: DbType.String);
                parameter.Add(name: "@Flag", value: flag, dbType: DbType.Int32);

                var data = await connection.QueryAsync<MerchantDetailsResponseModel>(
                        sql: @"[Reports].[USP_AcquisitionManagerWiseMerchantDetails]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<IEnumerable<AdCourier.Domain.Entities.DapperDataModel.OrderResponseDapperModel>> GetTotalOrdersWithDateFlag(RequestBodyModel requestBody)
        {

            try
            {
                using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
                {
                    connection.Open();

                    var parameter = new DynamicParameters();

                    parameter.Add(name: "@FromDate", value: requestBody.FromDate, dbType: DbType.DateTime);
                    parameter.Add(name: "@ToDate", value: requestBody.ToDate, dbType: DbType.DateTime);
                    parameter.Add(name: "@DateFlag", value: requestBody.DateFlag, dbType: DbType.Int32);
                    parameter.Add(name: "@DistrictId", value: requestBody.DistrictId, dbType: DbType.Int32);
                    parameter.Add(name: "@CourierId", value: requestBody.CourierId, dbType: DbType.Int32);
                    parameter.Add(name: "@DeliveryRangeId", value: requestBody.DeliveryRangeId, dbType: DbType.Int32);
                    parameter.Add(name: "@StatusId", value: requestBody.StatusId, dbType: DbType.Int32);
                    parameter.Add(name: "@StatusIds", value: requestBody.StatusIds, dbType: DbType.String);


                    var data = (await connection.QueryAsync<AdCourier.Domain.Entities.DapperDataModel.OrderResponseDapperModel>(
                            sql: @"[DT].[USP_GetTotalOrdersWithDateFlag]",
                            param: parameter,
                            commandType: CommandType.StoredProcedure));
                    return data.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<dynamic>> GetCalledMerchantList(OrderBodyModel orderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[Usp_GetCalledMerchants]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> GetVisitedMerchantList(OrderBodyModel orderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[Usp_GetVisitedMerchants]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> DateWiseOrderPlacedCalledMerchantList(OrderBodyModel orderBodyModel)
        {
            DateTime calledDateOne = DateTime.Now, calledDateTwo = DateTime.Now;
            if (orderBodyModel.DateFormat == "daterange")
            {
                calledDateOne = orderBodyModel.FromDate;
                calledDateTwo = orderBodyModel.ToDate;
            }
            else if (orderBodyModel.DateFormat == "last7days")
            {
                calledDateOne = orderBodyModel.FromDate.AddDays(-6);
                calledDateTwo = orderBodyModel.FromDate;
            }

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@calledDateOne", value: calledDateOne, dbType: DbType.DateTime);
                parameters.Add(name: "@calledDateTwo", value: calledDateTwo, dbType: DbType.DateTime);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[DateWiseOrderedCalledMerchantList]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> DateWiseOrderPlacedVisitedMerchantList(OrderBodyModel orderBodyModel)
        {
            DateTime visitedDateOne = DateTime.Now, visitedDateTwo = DateTime.Now;
            if (orderBodyModel.DateFormat == "daterange")
            {
                visitedDateOne = orderBodyModel.FromDate;
                visitedDateTwo = orderBodyModel.ToDate;
            }
            else if (orderBodyModel.DateFormat == "last7days")
            {
                visitedDateOne = orderBodyModel.FromDate.AddDays(-6);
                visitedDateTwo = orderBodyModel.FromDate;
            }
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@visitedDateOne", value: visitedDateOne, dbType: DbType.DateTime);
                parameters.Add(name: "@visitedDateTwo", value: visitedDateTwo, dbType: DbType.DateTime);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[DateWiseOrderedVisitedMerchantList]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<OrderStatusCountView>> StatusWiseTotalOrder(OrderBodyModel orderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@inputDate", value: orderBodyModel.InputDateType, dbType: DbType.Int32);

                var data = await connection.QueryAsync<OrderStatusCountView>(
                    sql: @"[Reports].[StatusWiseTotalOrderCount]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> StatusWiseTotalOrderDetails(RequestBodyModel requestBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@statusId", value: requestBodyModel.StatusId, dbType: DbType.Int32);
                parameters.Add(name: "@offset_rows", value: requestBodyModel.Index, dbType: DbType.Int32);
                parameters.Add(name: "@fetch_rows", value: requestBodyModel.Count, dbType: DbType.Int32);
                parameters.Add(name: "@fromDate", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: requestBodyModel.ToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@inputDate", value: requestBodyModel.DateFlag, dbType: DbType.Int32);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[StatusWiseTotalOrderCountDetails]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );

                return data.ToList();
            }
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> CourierWiseReturnReport(OrderBodyModel orderBodyModel)
        {

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ToDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);
                parameter.Add(name: "@CourierId", value: orderBodyModel.CourierId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<CourierOrdersViewModel>(
                        sql: @"[Reports].[USP_CourierWiseReturnReport]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }
        public async Task<IEnumerable<CourierOrdersViewModel>> PendingShipmentReconciliation(string orderId, int flag)
        {
            string id;
            if (orderId.ToLower().Contains("dt"))
            {
                int startIndex = 3;
                int endIndex = orderId.Length - 3;
                id = orderId.Substring(startIndex, endIndex);
            }
            else
            {
                id = orderId;
            }

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@orderId", value: id, dbType: DbType.String);
                parameter.Add(name: "@flag", value: flag, dbType: DbType.Int32);

                var data = await connection.QueryAsync<CourierOrdersViewModel>(
                        sql: @"[Reports].[USP_PendingShipmentReconciliation]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<IEnumerable<HoursCalculationViewModel>> HoursCalculationReport(RequestBodyModel bodyModel)
        {

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: bodyModel.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ToDate", value: bodyModel.ToDate, dbType: DbType.DateTime);

                var data = await connection.QueryAsync<HoursCalculationViewModel>(
                        sql: @"[Reports].[USP_HoursCalculationReport]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<IEnumerable<HoursCalculationViewModel>> HoursCalculationReportForDelivery(RequestBodyModel bodyModel)
        {

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: bodyModel.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ToDate", value: bodyModel.ToDate, dbType: DbType.DateTime);

                var data = await connection.QueryAsync<HoursCalculationViewModel>(
                        sql: @"[Reports].[USP_HoursCalculationReportForDelivery]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
        }

        public async Task<dynamic> EatAnalysisOverCollectionReport(OrderBodyModel orderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: orderBodyModel.ToDate, dbType: DbType.DateTime);

                connection.Open();
                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[EatAnalysisOverCollectionReport]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<CourierBillReportViewModel>> GetCourierBillList(RequestBodyModel requestBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: requestBodyModel.ToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@filteredPod", value: requestBodyModel.Flag, dbType: DbType.Int32);

                connection.Open();
                var data = await connection.QueryAsync<CourierBillReportViewModel>(
                    sql: @"[Reports].[GetCourierBillList]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );

                return data.ToList();
            }
        }

        public async Task<IEnumerable<LoanSurveyViewModel>> GetLoanSurvey(OrderBodyModel orderBodyModel)
        {
            var allData = await (from _loan in _sqlServerContext.LoanSurvey
                                 join _merchant in _sqlServerContext.CourierUsers on _loan.CourierUserId equals _merchant.CourierUserId
                                 join _loanCourier in _sqlServerContext.CouriersWithLoanSurvey on _loan.LoanSurveyId equals _loanCourier.LoanSurveyId into lc
                                 from _courier in lc.DefaultIfEmpty()

                                 join _thana in _sqlServerContext.Districts
                                 on _merchant.ThanaId equals _thana.DistrictId into merchantThanaJoin
                                 from subMerchantThana in merchantThanaJoin.DefaultIfEmpty()

                                 join presentDistrict in _sqlServerContext.Districts
                                 on _loan.PresentAddDistrictId equals presentDistrict.DistrictId into presentDistrictJoin
                                 from subPresentDistrict in presentDistrictJoin.DefaultIfEmpty()

                                 join presentThana in _sqlServerContext.Districts
                                 on _loan.PresentAddThanaId equals presentThana.DistrictId into presentThanaJoin
                                 from subPresentThana in presentThanaJoin.DefaultIfEmpty()

                                 join permanentDistrict in _sqlServerContext.Districts
                                 on _loan.PermanentAddDistrictId equals permanentDistrict.DistrictId into permanentDistrictJoin
                                 from subPermanentDistrict in permanentDistrictJoin.DefaultIfEmpty()

                                 join permanentThana in _sqlServerContext.Districts
                                 on _loan.PermanentAddThanaId equals permanentThana.DistrictId into permanentThanaJoin
                                 from subPermanentThana in presentThanaJoin.DefaultIfEmpty()

                                 join _courierUser in _sqlServerContext.CourierUsers
                                 on _loan.CourierUserId equals _courierUser.CourierUserId

                                 join _loanStatus in _sqlServerContext.LoanStatus 
                                 on _loan.LoanSurveyId equals _loanStatus.LoanSurveyId into loanStatusJoin
                                 from subLoanStatus in loanStatusJoin.DefaultIfEmpty()

                                 where _loan.ApplicationDate >= orderBodyModel.FromDate.Date
                                 && _loan.ApplicationDate < orderBodyModel.ToDate.Date.AddDays(1)
                                 && _loan.CourierUserId != 1
                                 select new
                                 {
                                     CourierUserId = _loan.CourierUserId,
                                     UserName = _courierUser.UserName,
                                     LoanSurveyId = _loan.LoanSurveyId,
                                     MerchantName = _merchant.CompanyName,
                                     TradeLicenseImageUrl = _loan.TradeLicenseImageUrl,
                                     InterestedAmount = _loan.InterestedAmount,
                                     TransactionAmount = _loan.TransactionAmount,
                                     IsBankAccount = _loan.IsBankAccount,
                                     IsLocalShop = _loan.IsLocalShop,
                                     ApplicationDate = _loan.ApplicationDate,
                                     MonthlyTotalCodAmount = _loan.MonthlyTotalCodAmount,
                                     GuarantorName = _loan.GuarantorName,
                                     GuarantorMobile = _loan.GuarantorMobile,
                                     MonthlyTotalAverageSale = _loan.MonthlyTotalAverageSale,
                                     LoanAmount = _loan.LoanAmount,
                                     BankName = _loan.BankName,
                                     Gender = _loan.Gender,
                                     Age = _loan.Age,
                                     BasketValue = _loan.BasketValue,
                                     CardHolder = _loan.CardHolder,
                                     CardLimit = _loan.CardLimit,
                                     LoanEmi = _loan.LoanEmi,
                                     HasCreditCard = _loan.HasCreditCard,
                                     HasTin = _loan.HasTin,
                                     EduLevel = _loan.EduLevel,
                                     RepayType = _loan.RepayType,
                                     MonthlyOrder = _loan.MonthlyOrder,
                                     MonthlyExp = _loan.MonthlyExp,
                                     Recommend = _loan.Recommend,
                                     RelationMarchent = _loan.RelationMarchent,
                                     ShopOwnership = _loan.ShopOwnership,
                                     TinNumber = _loan.TinNumber,
                                     HomeOwnership = _loan.HomeOwnership,
                                     Married = _loan.Married,
                                     FamMem = _loan.FamMem,
                                     HasTradeLicense = _loan.HasTradeLicense,
                                     TradeLicenseNo = _loan.TradeLicenseNo,
                                     TradeLicenseExpireDate = _loan.TradeLicenseExpireDate,
                                     CompanyBankAccNo = _loan.CompanyBankAccNo,
                                     CompanyBankAccName = _loan.CompanyBankAccName,
                                     AnnualTotalIncome = _loan.AnnualTotalIncome,
                                     DateOfBirth = _loan.DateOfBirth,
                                     NidNo = _loan.NidNo,
                                     OthersIncome = _loan.OthersIncome,
                                     ReqTenorMonth = _loan.ReqTenorMonth,
                                     ResidenceLocation = _loan.ResidenceLocation,
                                     CollectionAmountAvg = 0,
                                     HasPreviousLoan = _loan.HasPreviousLoan,
                                     LenderType = _loan.LenderType,
                                     BankStatementUrl = _loan.BankStatementUrl,
                                     Comments = _loan.Comments,
                                     BusinessStartDate = _loan.BusinessStartDate,
                                     NidImageUrl = _loan.NidImageUrl,
                                     NidBackImageUrl = _loan.NidBackImageUrl,
                                     TinImageUrl = _loan.TinImageUrl,
                                     CibUploadedFormUrl = _loan.CibUploadedFormUrl,
                                     Address = _merchant.Address,
                                     ThanaId = _merchant.ThanaId,
                                     ThanaName = merchantThanaJoin == null ? "" : (subMerchantThana.District ?? ""),
                                     FatherName = _loan.FatherName,
                                     MotherName = _loan.MotherName,
                                     SpouseName = _loan.SpouseName,
                                     IsLoanDue = _loan.IsLoanDue,
                                     PresentAddHouseNo = _loan.PresentAddHouseNo,
                                     PresentAddRoadNo = _loan.PresentAddRoadNo,
                                     PresentAddRoadName = _loan.PresentAddRoadName,
                                     PresentAddArea = _loan.PresentAddArea,
                                     PresentAddPostOffice = _loan.PresentAddPostOffice,
                                     PresentAddDistrictId = _loan.PresentAddDistrictId,
                                     PresentDistrictName = presentDistrictJoin == null ? "" : (subPresentDistrict.District ?? ""),
                                     PresentAddThanaId = _loan.PresentAddThanaId,
                                     PresentThanaName = presentThanaJoin == null ? "" : (subPresentThana.District ?? ""),
                                     HouseOwner = _loan.HouseOwner,
                                     IsOwner = _loan.IsOwner,
                                     DurationOfLiving = _loan.DurationOfLiving,
                                     PermanentAddHouseNo = _loan.PermanentAddHouseNo,
                                     PermanentAddRoadNo = _loan.PermanentAddRoadNo,
                                     PermanentAddRoadName = _loan.PermanentAddRoadName,
                                     PermanentAddArea = _loan.PermanentAddArea,
                                     PermanentAddPostOffice = _loan.PermanentAddPostOffice,
                                     PermanentAddDistrictId = _loan.PermanentAddDistrictId,
                                     PermanentDistrictName = permanentDistrictJoin == null ? "" : (subPermanentDistrict.District ?? ""),
                                     PermanentAddThanaId = _loan.PermanentAddThanaId,
                                     PermanentThanaName = permanentThanaJoin == null ? "" : (subPermanentThana.District ?? ""),
                                     Experience = _loan.Experience,
                                     Occupation = _loan.Occupation,
                                     PermanentAddHouseOwnerName = _loan.PermanentAddHouseOwnerName,
                                     CreditCardNumber = _loan.CreditCardNumber,
                                     ApplicantPhotoUrl = _loan.ApplicantPhotoUrl,
                                     LoanRepayType = _loan.LoanRepayType,
                                     OtherIncomeSource = _loan.OtherIncomeSource,
                                     LoanStatus = new
                                     {
                                         LoanStatusId = loanStatusJoin == null ? 0 : (subLoanStatus == null ? 0 : subLoanStatus.LoanStatusId),
                                         LoanSurveyId = loanStatusJoin == null ? 0 : (subLoanStatus == null ? 0 : subLoanStatus.LoanSurveyId),
                                         StatusCode = loanStatusJoin == null ? "" : (subLoanStatus.StatusCode ?? ""),
                                         Status = loanStatusJoin == null ? "" : (subLoanStatus.Status ?? ""),
                                         Comment = loanStatusJoin == null ? "" : (subLoanStatus.Comment ?? ""),
                                         //CommentDate = loanStatusJoin == null ? DateTime.Now : (subLoanStatus == null ? DateTime.Now : subLoanStatus.CommentDate)
                                     },
                                     CouriersWithLoanSurveyViewModel = new
                                     {
                                         CouriersWithLoanSurveyId = _courier == null ? 0 : _courier.CouriersWithLoanSurveyId,
                                         CourierId = _courier == null ? 0 : _courier.CourierId,
                                         CourierName = _courier == null ? "" : (_courier.CourierName ?? ""),
                                         LoanSurveyId = _courier == null ? 0 : _courier.LoanSurveyId
                                     }
                                 }).ToListAsync();

            var loanSurveyData = allData.GroupBy(g => g.CourierUserId).Select(s => new LoanSurveyViewModel
            {
                LoanSurveyId = s.FirstOrDefault().LoanSurveyId,
                CourierUserId = s.FirstOrDefault().CourierUserId,
                UserName = s.FirstOrDefault().UserName,
                MerchantName = s.FirstOrDefault().MerchantName,
                TradeLicenseImageUrl = s.FirstOrDefault().TradeLicenseImageUrl,
                InterestedAmount = s.FirstOrDefault().InterestedAmount,
                TransactionAmount = s.FirstOrDefault().TransactionAmount,
                IsBankAccount = s.FirstOrDefault().IsBankAccount,
                IsLocalShop = s.FirstOrDefault().IsLocalShop,
                ApplicationDate = s.FirstOrDefault().ApplicationDate,
                MonthlyTotalCodAmount = s.FirstOrDefault().MonthlyTotalCodAmount,
                GuarantorName = s.FirstOrDefault().GuarantorName,
                GuarantorMobile = s.FirstOrDefault().GuarantorMobile,
                MonthlyTotalAverageSale = s.FirstOrDefault().MonthlyTotalAverageSale,
                LoanAmount = s.FirstOrDefault().LoanAmount,
                BankName = s.FirstOrDefault().BankName,
                Gender = s.FirstOrDefault().Gender,
                Age = s.FirstOrDefault().Age,
                BasketValue = s.FirstOrDefault().BasketValue,
                CardHolder = s.FirstOrDefault().CardHolder,
                CardLimit = s.FirstOrDefault().CardLimit,
                LoanEmi = s.FirstOrDefault().LoanEmi,
                HasCreditCard = s.FirstOrDefault().HasCreditCard,
                HasTin = s.FirstOrDefault().HasTin,
                EduLevel = s.FirstOrDefault().EduLevel,
                RepayType = s.FirstOrDefault().RepayType,
                MonthlyOrder = s.FirstOrDefault().MonthlyOrder,
                MonthlyExp = s.FirstOrDefault().MonthlyExp,
                Recommend = s.FirstOrDefault().Recommend,
                RelationMarchent = s.FirstOrDefault().RelationMarchent,
                ShopOwnership = s.FirstOrDefault().ShopOwnership,
                TinNumber = s.FirstOrDefault().TinNumber,
                HomeOwnership = s.FirstOrDefault().HomeOwnership,
                Married = s.FirstOrDefault().Married,
                FamMem = s.FirstOrDefault().FamMem,
                HasTradeLicense = s.FirstOrDefault().HasTradeLicense,
                TradeLicenseNo = s.FirstOrDefault().TradeLicenseNo,
                TradeLicenseExpireDate = s.FirstOrDefault().TradeLicenseExpireDate,
                CompanyBankAccNo = s.FirstOrDefault().CompanyBankAccNo,
                CompanyBankAccName = s.FirstOrDefault().CompanyBankAccName,
                AnnualTotalIncome = s.FirstOrDefault().AnnualTotalIncome,
                DateOfBirth = s.FirstOrDefault().DateOfBirth,
                NidNo = s.FirstOrDefault().NidNo,
                OthersIncome = s.FirstOrDefault().OthersIncome,
                ReqTenorMonth = s.FirstOrDefault().ReqTenorMonth,
                ResidenceLocation = s.FirstOrDefault().ResidenceLocation,
                CollectionAmountAvg = s.FirstOrDefault().CollectionAmountAvg,
                HasPreviousLoan = s.FirstOrDefault().HasPreviousLoan,
                LenderType = s.FirstOrDefault().LenderType,
                BankStatementUrl = s.FirstOrDefault().BankStatementUrl,
                Comments = s.FirstOrDefault().Comments,
                BusinessStartDate = s.FirstOrDefault().BusinessStartDate,
                NidImageUrl = s.FirstOrDefault().NidImageUrl,
                NidBackImageUrl = s.FirstOrDefault().NidBackImageUrl,
                TinImageUrl = s.FirstOrDefault().TinImageUrl,
                CibUploadedFormUrl = s.FirstOrDefault().CibUploadedFormUrl,
                Address = s.FirstOrDefault().Address,
                ThanaId = s.FirstOrDefault().ThanaId,
                ThanaName = s.FirstOrDefault().ThanaName,
                FatherName = s.FirstOrDefault().FatherName,
                MotherName = s.FirstOrDefault().MotherName,
                SpouseName = s.FirstOrDefault().SpouseName,
                IsLoanDue = s.FirstOrDefault().IsLoanDue,
                PresentAddHouseNo = s.FirstOrDefault().PresentAddHouseNo,
                PresentAddRoadNo = s.FirstOrDefault().PresentAddRoadNo,
                PresentAddRoadName = s.FirstOrDefault().PresentAddRoadName,
                PresentAddArea = s.FirstOrDefault().PresentAddArea,
                PresentAddPostOffice = s.FirstOrDefault().PresentAddPostOffice,
                PresentAddDistrictId = s.FirstOrDefault().PresentAddDistrictId,
                PresentDistrictName = s.FirstOrDefault().PresentDistrictName,
                PresentAddThanaId = s.FirstOrDefault().PresentAddThanaId,
                PresentThanaName = s.FirstOrDefault().PresentThanaName,
                HouseOwner = s.FirstOrDefault().HouseOwner,
                IsOwner = s.FirstOrDefault().IsOwner,
                DurationOfLiving = s.FirstOrDefault().DurationOfLiving,
                PermanentAddHouseNo = s.FirstOrDefault().PermanentAddHouseNo,
                PermanentAddRoadNo = s.FirstOrDefault().PermanentAddRoadNo,
                PermanentAddRoadName = s.FirstOrDefault().PermanentAddRoadName,
                PermanentAddArea = s.FirstOrDefault().PermanentAddArea,
                PermanentAddPostOffice = s.FirstOrDefault().PermanentAddPostOffice,
                PermanentAddDistrictId = s.FirstOrDefault().PermanentAddDistrictId,
                PermanentDistrictName = s.FirstOrDefault().PermanentDistrictName,
                PermanentAddThanaId = s.FirstOrDefault().PermanentAddThanaId,
                PermanentThanaName = s.FirstOrDefault().PermanentThanaName,
                Experience = s.FirstOrDefault().Experience,
                Occupation = s.FirstOrDefault().Occupation,
                PermanentAddHouseOwnerName = s.FirstOrDefault().PermanentAddHouseOwnerName,
                CreditCardNumber = s.FirstOrDefault().CreditCardNumber,
                ApplicantPhotoUrl = s.FirstOrDefault().ApplicantPhotoUrl,
                LoanRepayType = s.FirstOrDefault().LoanRepayType,
                OtherIncomeSource = s.FirstOrDefault().OtherIncomeSource,
                LoanApprovedStatusViewModel = s.Where(k => k.LoanStatus.LoanSurveyId == s.FirstOrDefault().LoanSurveyId)
                .GroupBy(g => g.LoanStatus.LoanStatusId)
                .Select(a => new LoanApprovedStatusViewModel
                {
                    LoanApplicationId = a.FirstOrDefault().LoanSurveyId,
                    Status = a.FirstOrDefault().LoanStatus.Status,
                    StatusCode = a.FirstOrDefault().LoanStatus.StatusCode,
                    Comment = a.FirstOrDefault().LoanStatus.Comment
                }).ToList(),
                CouriersWithLoanSurveyViewModel = s.GroupBy(g => g.CouriersWithLoanSurveyViewModel.CouriersWithLoanSurveyId)
                .Select(r => new CouriersWithLoanSurveyViewModel
                {
                    CouriersWithLoanSurveyId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CouriersWithLoanSurveyId,
                    CourierId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CourierId,
                    CourierName = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CourierName,
                    LoanSurveyId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.LoanSurveyId
                }).ToList()
            }).ToList();

            if (orderBodyModel.LastDymanicMonth > 0)
            {
                var array = loanSurveyData.Select(x => x.CourierUserId).ToArray();

                var data = _sqlServerContext.CourierOrders.Where(x => array.Contains(x.MerchantId)
                    && x.OrderDate.Date >= DateTime.Now.Date.AddMonths(-orderBodyModel.LastDymanicMonth)
                    && x.OrderDate.Date < DateTime.Now.Date.AddDays(1)
                    && x.IsDownloaded == true)
                    .GroupBy(g => g.MerchantId).Select(s => new
                    {
                        MerchantId = s.Key,
                        TotalCollectionAmount = s.Sum(c => c.CollectionAmount),
                        Months = s.GroupBy(g => g.OrderDate.Month).Select(grouped => new
                        {
                            Month = grouped.Key
                        }).Count()
                    }).ToList();

                foreach (var item in loanSurveyData)
                {
                    var totalCollectionAmount = data.Where(w => w.MerchantId.Equals(item.CourierUserId)).Count() > 0 ? data.Where(w => w.MerchantId.Equals(item.CourierUserId)).FirstOrDefault().TotalCollectionAmount : 0;

                    int month = data.Where(w => w.MerchantId.Equals(item.CourierUserId)).Count() > 0 ? data.Where(w => w.MerchantId.Equals(item.CourierUserId)).FirstOrDefault().Months : 0;

                    int dynamicMonth = month == 0 ? orderBodyModel.LastDymanicMonth : (month < orderBodyModel.LastDymanicMonth ? month : orderBodyModel.LastDymanicMonth);

                    item.CollectionAmountAvg = totalCollectionAmount / dynamicMonth;
                }

            }

            return loanSurveyData;
        }

        public async Task<IEnumerable<LoanSurveyViewModel>> GetLoanSurveyByLender(OrderBodyModel orderBodyModel)
        {
            var lenderUser = await _sqlServerContext.LenderUser.Where(l => l.LenderUserId.Equals(orderBodyModel.lenderUserId)).FirstOrDefaultAsync();
            List<LoanSurveyViewModel> loanSurveyData = new List<LoanSurveyViewModel>();

            if (lenderUser.RoleName.ToLower().Equals("admin"))
            {
                var allData = await (from _loan in _sqlServerContext.LoanSurvey
                                     join _merchant in _sqlServerContext.CourierUsers on _loan.CourierUserId equals _merchant.CourierUserId
                                     join _loanCourier in _sqlServerContext.CouriersWithLoanSurvey on _loan.LoanSurveyId equals _loanCourier.LoanSurveyId into lc
                                     from _courier in lc.DefaultIfEmpty()

                                     join _thana in _sqlServerContext.Districts
                                     on _merchant.ThanaId equals _thana.DistrictId into merchantThanaJoin
                                     from subMerchantThana in merchantThanaJoin.DefaultIfEmpty()

                                     join presentDistrict in _sqlServerContext.Districts
                                     on _loan.PresentAddDistrictId equals presentDistrict.DistrictId into presentDistrictJoin
                                     from subPresentDistrict in presentDistrictJoin.DefaultIfEmpty()

                                     join presentThana in _sqlServerContext.Districts
                                     on _loan.PresentAddThanaId equals presentThana.DistrictId into presentThanaJoin
                                     from subPresentThana in presentThanaJoin.DefaultIfEmpty()

                                     join permanentDistrict in _sqlServerContext.Districts
                                     on _loan.PermanentAddDistrictId equals permanentDistrict.DistrictId into permanentDistrictJoin
                                     from subPermanentDistrict in permanentDistrictJoin.DefaultIfEmpty()

                                     join permanentThana in _sqlServerContext.Districts
                                     on _loan.PermanentAddThanaId equals permanentThana.DistrictId into permanentThanaJoin
                                     from subPermanentThana in presentThanaJoin.DefaultIfEmpty()

                                     join _loanStatus in _sqlServerContext.LoanStatus
                                     on _loan.LoanSurveyId equals _loanStatus.LoanSurveyId into loanStatusJoin
                                     from subLoanStatus in loanStatusJoin.DefaultIfEmpty()

                                     where _loan.ApplicationDate >= orderBodyModel.FromDate.Date
                                     && _loan.ApplicationDate < orderBodyModel.ToDate.Date.AddDays(1)
                                     && _loan.CourierUserId != 1
                                     select new
                                     {
                                         CourierUserId = _loan.CourierUserId,
                                         LoanSurveyId = _loan.LoanSurveyId,
                                         MerchantName = _merchant.CompanyName,
                                         Mobile = _merchant.Mobile,
                                         TradeLicenseImageUrl = _loan.TradeLicenseImageUrl,
                                         InterestedAmount = _loan.InterestedAmount,
                                         TransactionAmount = _loan.TransactionAmount,
                                         IsBankAccount = _loan.IsBankAccount,
                                         IsLocalShop = _loan.IsLocalShop,
                                         ApplicationDate = _loan.ApplicationDate,
                                         MonthlyTotalCodAmount = _loan.MonthlyTotalCodAmount,
                                         GuarantorName = _loan.GuarantorName,
                                         GuarantorMobile = _loan.GuarantorMobile,
                                         MonthlyTotalAverageSale = _loan.MonthlyTotalAverageSale,
                                         LoanAmount = _loan.LoanAmount,
                                         BankName = _loan.BankName,
                                         Gender = _loan.Gender,
                                         Age = _loan.Age,
                                         BasketValue = _loan.BasketValue,
                                         CardHolder = _loan.CardHolder,
                                         CardLimit = _loan.CardLimit,
                                         LoanEmi = _loan.LoanEmi,
                                         HasCreditCard = _loan.HasCreditCard,
                                         HasTin = _loan.HasTin,
                                         EduLevel = _loan.EduLevel,
                                         RepayType = _loan.RepayType,
                                         MonthlyOrder = _loan.MonthlyOrder,
                                         MonthlyExp = _loan.MonthlyExp,
                                         Recommend = _loan.Recommend,
                                         RelationMarchent = _loan.RelationMarchent,
                                         ShopOwnership = _loan.ShopOwnership,
                                         TinNumber = _loan.TinNumber,
                                         HomeOwnership = _loan.HomeOwnership,
                                         Married = _loan.Married,
                                         FamMem = _loan.FamMem,
                                         HasTradeLicense = _loan.HasTradeLicense,
                                         TradeLicenseNo = _loan.TradeLicenseNo,
                                         TradeLicenseExpireDate = _loan.TradeLicenseExpireDate,
                                         CompanyBankAccNo = _loan.CompanyBankAccNo,
                                         CompanyBankAccName = _loan.CompanyBankAccName,
                                         AnnualTotalIncome = _loan.AnnualTotalIncome,
                                         DateOfBirth = _loan.DateOfBirth,
                                         NidNo = _loan.NidNo,
                                         OthersIncome = _loan.OthersIncome,
                                         ReqTenorMonth = _loan.ReqTenorMonth,
                                         ResidenceLocation = _loan.ResidenceLocation,
                                         CollectionAmountAvg = 0,
                                         HasPreviousLoan = _loan.HasPreviousLoan,
                                         LenderType = _loan.LenderType,
                                         BankStatementUrl = _loan.BankStatementUrl,
                                         Comments = _loan.Comments,
                                         BusinessStartDate = _loan.BusinessStartDate,
                                         NidImageUrl = _loan.NidImageUrl,
                                         NidBackImageUrl = _loan.NidBackImageUrl,
                                         TinImageUrl = _loan.TinImageUrl,
                                         CibUploadedFormUrl = _loan.CibUploadedFormUrl,
                                         Address = _merchant.Address,
                                         ThanaId = _merchant.ThanaId,
                                         ThanaName = merchantThanaJoin == null ? "" : (subMerchantThana.District ?? ""),
                                         FatherName = _loan.FatherName,
                                         MotherName = _loan.MotherName,
                                         SpouseName = _loan.SpouseName,
                                         IsLoanDue = _loan.IsLoanDue,
                                         PresentAddHouseNo = _loan.PresentAddHouseNo,
                                         PresentAddRoadNo = _loan.PresentAddRoadNo,
                                         PresentAddRoadName = _loan.PresentAddRoadName,
                                         PresentAddArea = _loan.PresentAddArea,
                                         PresentAddPostOffice = _loan.PresentAddPostOffice,
                                         PresentAddDistrictId = _loan.PresentAddDistrictId,
                                         PresentDistrictName = presentDistrictJoin == null ? "" : (subPresentDistrict.District ?? ""),
                                         PresentAddThanaId = _loan.PresentAddThanaId,
                                         PresentThanaName = presentThanaJoin == null ? "" : (subPresentThana.District ?? ""),
                                         HouseOwner = _loan.HouseOwner,
                                         IsOwner = _loan.IsOwner,
                                         DurationOfLiving = _loan.DurationOfLiving,
                                         PermanentAddHouseNo = _loan.PermanentAddHouseNo,
                                         PermanentAddRoadNo = _loan.PermanentAddRoadNo,
                                         PermanentAddRoadName = _loan.PermanentAddRoadName,
                                         PermanentAddArea = _loan.PermanentAddArea,
                                         PermanentAddPostOffice = _loan.PermanentAddPostOffice,
                                         PermanentAddDistrictId = _loan.PermanentAddDistrictId,
                                         PermanentDistrictName = permanentDistrictJoin == null ? "" : (subPermanentDistrict.District ?? ""),
                                         PermanentAddThanaId = _loan.PermanentAddThanaId,
                                         PermanentThanaName = permanentThanaJoin == null ? "" : (subPermanentThana.District ?? ""),
                                         Experience = _loan.Experience,
                                         Occupation = _loan.Occupation,
                                         PermanentAddHouseOwnerName = _loan.PermanentAddHouseOwnerName,
                                         CreditCardNumber = _loan.CreditCardNumber,
                                         ApplicantPhotoUrl = _loan.ApplicantPhotoUrl,
                                         LoanRepayType = _loan.LoanRepayType,
                                         OtherIncomeSource = _loan.OtherIncomeSource,
                                         LoanStatus = new
                                         {
                                             LoanStatusId = loanStatusJoin == null ? 0 : (subLoanStatus == null ? 0 : subLoanStatus.LoanStatusId),
                                             LoanSurveyId = loanStatusJoin == null ? 0 : (subLoanStatus == null ? 0 : subLoanStatus.LoanSurveyId),
                                             StatusCode = loanStatusJoin == null ? "" : (subLoanStatus.StatusCode ?? ""),
                                             Status = loanStatusJoin == null ? "" : (subLoanStatus.Status ?? ""),
                                             Comment = loanStatusJoin == null ? "" : (subLoanStatus.Comment ?? ""),
                                             //CommentDate = loanStatusJoin == null ? DateTime.Now : (subLoanStatus == null ? DateTime.Now : subLoanStatus.CommentDate)
                                         },
                                         CouriersWithLoanSurveyViewModel = new
                                         {
                                             CouriersWithLoanSurveyId = _courier == null ? 0 : _courier.CouriersWithLoanSurveyId,
                                             CourierId = _courier == null ? 0 : _courier.CourierId,
                                             CourierName = _courier == null ? "" : (_courier.CourierName ?? ""),
                                             LoanSurveyId = _courier == null ? 0 : _courier.LoanSurveyId
                                         }
                                     }).OrderByDescending(o => o.LoanSurveyId).ToListAsync();

                loanSurveyData = allData.GroupBy(g => g.CourierUserId).Select(s => new LoanSurveyViewModel
                {
                    LoanSurveyId = s.FirstOrDefault().LoanSurveyId,
                    CourierUserId = s.FirstOrDefault().CourierUserId,
                    MerchantName = s.FirstOrDefault().MerchantName,
                    Mobile = s.FirstOrDefault().Mobile,
                    TradeLicenseImageUrl = s.FirstOrDefault().TradeLicenseImageUrl,
                    InterestedAmount = s.FirstOrDefault().InterestedAmount,
                    TransactionAmount = s.FirstOrDefault().TransactionAmount,
                    IsBankAccount = s.FirstOrDefault().IsBankAccount,
                    IsLocalShop = s.FirstOrDefault().IsLocalShop,
                    ApplicationDate = s.FirstOrDefault().ApplicationDate,
                    MonthlyTotalCodAmount = s.FirstOrDefault().MonthlyTotalCodAmount,
                    GuarantorName = s.FirstOrDefault().GuarantorName,
                    GuarantorMobile = s.FirstOrDefault().GuarantorMobile,
                    MonthlyTotalAverageSale = s.FirstOrDefault().MonthlyTotalAverageSale,
                    LoanAmount = s.FirstOrDefault().LoanAmount,
                    BankName = s.FirstOrDefault().BankName,
                    Gender = s.FirstOrDefault().Gender,
                    Age = s.FirstOrDefault().Age,
                    BasketValue = s.FirstOrDefault().BasketValue,
                    CardHolder = s.FirstOrDefault().CardHolder,
                    CardLimit = s.FirstOrDefault().CardLimit,
                    LoanEmi = s.FirstOrDefault().LoanEmi,
                    HasCreditCard = s.FirstOrDefault().HasCreditCard,
                    HasTin = s.FirstOrDefault().HasTin,
                    EduLevel = s.FirstOrDefault().EduLevel,
                    RepayType = s.FirstOrDefault().RepayType,
                    MonthlyOrder = s.FirstOrDefault().MonthlyOrder,
                    MonthlyExp = s.FirstOrDefault().MonthlyExp,
                    Recommend = s.FirstOrDefault().Recommend,
                    RelationMarchent = s.FirstOrDefault().RelationMarchent,
                    ShopOwnership = s.FirstOrDefault().ShopOwnership,
                    TinNumber = s.FirstOrDefault().TinNumber,
                    HomeOwnership = s.FirstOrDefault().HomeOwnership,
                    Married = s.FirstOrDefault().Married,
                    FamMem = s.FirstOrDefault().FamMem,
                    HasTradeLicense = s.FirstOrDefault().HasTradeLicense,
                    TradeLicenseNo = s.FirstOrDefault().TradeLicenseNo,
                    TradeLicenseExpireDate = s.FirstOrDefault().TradeLicenseExpireDate,
                    CompanyBankAccNo = s.FirstOrDefault().CompanyBankAccNo,
                    CompanyBankAccName = s.FirstOrDefault().CompanyBankAccName,
                    AnnualTotalIncome = s.FirstOrDefault().AnnualTotalIncome,
                    DateOfBirth = s.FirstOrDefault().DateOfBirth,
                    NidNo = s.FirstOrDefault().NidNo,
                    OthersIncome = s.FirstOrDefault().OthersIncome,
                    ReqTenorMonth = s.FirstOrDefault().ReqTenorMonth,
                    ResidenceLocation = s.FirstOrDefault().ResidenceLocation,
                    CollectionAmountAvg = s.FirstOrDefault().CollectionAmountAvg,
                    HasPreviousLoan = s.FirstOrDefault().HasPreviousLoan,
                    LenderType = s.FirstOrDefault().LenderType,
                    BankStatementUrl = s.FirstOrDefault().BankStatementUrl,
                    Comments = s.FirstOrDefault().Comments,
                    BusinessStartDate = s.FirstOrDefault().BusinessStartDate,
                    NidImageUrl = s.FirstOrDefault().NidImageUrl,
                    NidBackImageUrl = s.FirstOrDefault().NidBackImageUrl,
                    TinImageUrl = s.FirstOrDefault().TinImageUrl,
                    CibUploadedFormUrl = s.FirstOrDefault().CibUploadedFormUrl,
                    Address = s.FirstOrDefault().Address,
                    ThanaId = s.FirstOrDefault().ThanaId,
                    ThanaName = s.FirstOrDefault().ThanaName,
                    FatherName = s.FirstOrDefault().FatherName,
                    MotherName = s.FirstOrDefault().MotherName,
                    SpouseName = s.FirstOrDefault().SpouseName,
                    IsLoanDue = s.FirstOrDefault().IsLoanDue,
                    PresentAddHouseNo = s.FirstOrDefault().PresentAddHouseNo,
                    PresentAddRoadNo = s.FirstOrDefault().PresentAddRoadNo,
                    PresentAddRoadName = s.FirstOrDefault().PresentAddRoadName,
                    PresentAddArea = s.FirstOrDefault().PresentAddArea,
                    PresentAddPostOffice = s.FirstOrDefault().PresentAddPostOffice,
                    PresentAddDistrictId = s.FirstOrDefault().PresentAddDistrictId,
                    PresentDistrictName = s.FirstOrDefault().PresentDistrictName,
                    PresentAddThanaId = s.FirstOrDefault().PresentAddThanaId,
                    PresentThanaName = s.FirstOrDefault().PresentThanaName,
                    HouseOwner = s.FirstOrDefault().HouseOwner,
                    IsOwner = s.FirstOrDefault().IsOwner,
                    DurationOfLiving = s.FirstOrDefault().DurationOfLiving,
                    PermanentAddHouseNo = s.FirstOrDefault().PermanentAddHouseNo,
                    PermanentAddRoadNo = s.FirstOrDefault().PermanentAddRoadNo,
                    PermanentAddRoadName = s.FirstOrDefault().PermanentAddRoadName,
                    PermanentAddArea = s.FirstOrDefault().PermanentAddArea,
                    PermanentAddPostOffice = s.FirstOrDefault().PermanentAddPostOffice,
                    PermanentAddDistrictId = s.FirstOrDefault().PermanentAddDistrictId,
                    PermanentDistrictName = s.FirstOrDefault().PermanentDistrictName,
                    PermanentAddThanaId = s.FirstOrDefault().PermanentAddThanaId,
                    PermanentThanaName = s.FirstOrDefault().PermanentThanaName,
                    Experience = s.FirstOrDefault().Experience,
                    Occupation = s.FirstOrDefault().Occupation,
                    PermanentAddHouseOwnerName = s.FirstOrDefault().PermanentAddHouseOwnerName,
                    CreditCardNumber = s.FirstOrDefault().CreditCardNumber,
                    ApplicantPhotoUrl = s.FirstOrDefault().ApplicantPhotoUrl,
                    LoanRepayType = s.FirstOrDefault().LoanRepayType,
                    OtherIncomeSource = s.FirstOrDefault().OtherIncomeSource,
                    LoanApprovedStatusViewModel = s.Where(k => k.LoanStatus.LoanSurveyId == s.FirstOrDefault().LoanSurveyId)
                    .GroupBy(g => g.LoanStatus.LoanStatusId)
                    .Select(a => new LoanApprovedStatusViewModel
                    {
                        LoanApplicationId = a.FirstOrDefault().LoanSurveyId,
                        Status = a.FirstOrDefault().LoanStatus.Status,
                        StatusCode = a.FirstOrDefault().LoanStatus.StatusCode,
                        Comment = a.FirstOrDefault().LoanStatus.Comment
                    }).ToList(),
                    CouriersWithLoanSurveyViewModel = s.Where(w => w.LoanSurveyId == s.FirstOrDefault().LoanSurveyId)
                    .GroupBy(g => g.CouriersWithLoanSurveyViewModel.CouriersWithLoanSurveyId)
                    .Select(r => new CouriersWithLoanSurveyViewModel
                    {
                        CouriersWithLoanSurveyId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CouriersWithLoanSurveyId,
                        CourierId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CourierId,
                        CourierName = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CourierName,
                        LoanSurveyId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.LoanSurveyId
                    }).ToList()
                }).ToList();

            }
            else if (lenderUser.RoleName.ToLower() == "user")
            {
                var assignedCourierUsersList = await _sqlServerContext.LenderCourierUserAssignment.Where(l => l.LenderUserId.Equals(orderBodyModel.lenderUserId) && !l.CourierUserId.Equals(1)).Select(s => s.CourierUserId).ToListAsync();

                var allData = await (from _loan in _sqlServerContext.LoanSurvey
                                     join _merchant in _sqlServerContext.CourierUsers on _loan.CourierUserId equals _merchant.CourierUserId
                                     join _loanCourier in _sqlServerContext.CouriersWithLoanSurvey on _loan.LoanSurveyId equals _loanCourier.LoanSurveyId into lc
                                     from _courier in lc.DefaultIfEmpty()

                                     join _thana in _sqlServerContext.Districts
                                     on _merchant.ThanaId equals _thana.DistrictId into merchantThanaJoin
                                     from subMerchantThana in merchantThanaJoin.DefaultIfEmpty()

                                     join presentDistrict in _sqlServerContext.Districts
                                     on _loan.PresentAddDistrictId equals presentDistrict.DistrictId into presentDistrictJoin
                                     from subPresentDistrict in presentDistrictJoin.DefaultIfEmpty()

                                     join presentThana in _sqlServerContext.Districts
                                     on _loan.PresentAddThanaId equals presentThana.DistrictId into presentThanaJoin
                                     from subPresentThana in presentThanaJoin.DefaultIfEmpty()

                                     join permanentDistrict in _sqlServerContext.Districts
                                     on _loan.PermanentAddDistrictId equals permanentDistrict.DistrictId into permanentDistrictJoin
                                     from subPermanentDistrict in permanentDistrictJoin.DefaultIfEmpty()

                                     join permanentThana in _sqlServerContext.Districts
                                     on _loan.PermanentAddThanaId equals permanentThana.DistrictId into permanentThanaJoin
                                     from subPermanentThana in presentThanaJoin.DefaultIfEmpty()

                                     join _loanStatus in _sqlServerContext.LoanStatus
                                     on _loan.LoanSurveyId equals _loanStatus.LoanSurveyId into loanStatusJoin
                                     from subLoanStatus in loanStatusJoin.DefaultIfEmpty()

                                     where assignedCourierUsersList.Contains(_loan.CourierUserId)
                                     && _loan.ApplicationDate >= orderBodyModel.FromDate.Date
                                     && _loan.ApplicationDate < orderBodyModel.ToDate.Date.AddDays(1)
                                     select new
                                     {
                                         CourierUserId = _loan.CourierUserId,
                                         LoanSurveyId = _loan.LoanSurveyId,
                                         MerchantName = _merchant.CompanyName,
                                         TradeLicenseImageUrl = _loan.TradeLicenseImageUrl,
                                         InterestedAmount = _loan.InterestedAmount,
                                         TransactionAmount = _loan.TransactionAmount,
                                         IsBankAccount = _loan.IsBankAccount,
                                         IsLocalShop = _loan.IsLocalShop,
                                         ApplicationDate = _loan.ApplicationDate,
                                         MonthlyTotalCodAmount = _loan.MonthlyTotalCodAmount,
                                         GuarantorName = _loan.GuarantorName,
                                         GuarantorMobile = _loan.GuarantorMobile,
                                         MonthlyTotalAverageSale = _loan.MonthlyTotalAverageSale,
                                         LoanAmount = _loan.LoanAmount,
                                         BankName = _loan.BankName,
                                         Gender = _loan.Gender,
                                         Age = _loan.Age,
                                         BasketValue = _loan.BasketValue,
                                         CardHolder = _loan.CardHolder,
                                         CardLimit = _loan.CardLimit,
                                         LoanEmi = _loan.LoanEmi,
                                         HasCreditCard = _loan.HasCreditCard,
                                         HasTin = _loan.HasTin,
                                         EduLevel = _loan.EduLevel,
                                         RepayType = _loan.RepayType,
                                         MonthlyOrder = _loan.MonthlyOrder,
                                         MonthlyExp = _loan.MonthlyExp,
                                         Recommend = _loan.Recommend,
                                         RelationMarchent = _loan.RelationMarchent,
                                         ShopOwnership = _loan.ShopOwnership,
                                         TinNumber = _loan.TinNumber,
                                         HomeOwnership = _loan.HomeOwnership,
                                         Married = _loan.Married,
                                         FamMem = _loan.FamMem,
                                         HasTradeLicense = _loan.HasTradeLicense,
                                         TradeLicenseNo = _loan.TradeLicenseNo,
                                         TradeLicenseExpireDate = _loan.TradeLicenseExpireDate,
                                         CompanyBankAccNo = _loan.CompanyBankAccNo,
                                         CompanyBankAccName = _loan.CompanyBankAccName,
                                         AnnualTotalIncome = _loan.AnnualTotalIncome,
                                         DateOfBirth = _loan.DateOfBirth,
                                         NidNo = _loan.NidNo,
                                         OthersIncome = _loan.OthersIncome,
                                         ReqTenorMonth = _loan.ReqTenorMonth,
                                         ResidenceLocation = _loan.ResidenceLocation,
                                         CollectionAmountAvg = 0,
                                         HasPreviousLoan = _loan.HasPreviousLoan,
                                         LenderType = _loan.LenderType,
                                         BankStatementUrl = _loan.BankStatementUrl,
                                         Comments = _loan.Comments,
                                         BusinessStartDate = _loan.BusinessStartDate,
                                         NidImageUrl = _loan.NidImageUrl,
                                         NidBackImageUrl = _loan.NidBackImageUrl,
                                         TinImageUrl = _loan.TinImageUrl,
                                         CibUploadedFormUrl = _loan.CibUploadedFormUrl,
                                         Address = _merchant.Address,
                                         ThanaId = _merchant.ThanaId,
                                         ThanaName = merchantThanaJoin == null ? "" : (subMerchantThana.District ?? ""),
                                         FatherName = _loan.FatherName,
                                         MotherName = _loan.MotherName,
                                         SpouseName = _loan.SpouseName,
                                         IsLoanDue = _loan.IsLoanDue,
                                         PresentAddHouseNo = _loan.PresentAddHouseNo,
                                         PresentAddRoadNo = _loan.PresentAddRoadNo,
                                         PresentAddRoadName = _loan.PresentAddRoadName,
                                         PresentAddArea = _loan.PresentAddArea,
                                         PresentAddPostOffice = _loan.PresentAddPostOffice,
                                         PresentAddDistrictId = _loan.PresentAddDistrictId,
                                         PresentDistrictName = presentDistrictJoin == null ? "" : (subPresentDistrict.District ?? ""),
                                         PresentAddThanaId = _loan.PresentAddThanaId,
                                         PresentThanaName = presentThanaJoin == null ? "" : (subPresentThana.District ?? ""),
                                         HouseOwner = _loan.HouseOwner,
                                         IsOwner = _loan.IsOwner,
                                         DurationOfLiving = _loan.DurationOfLiving,
                                         PermanentAddHouseNo = _loan.PermanentAddHouseNo,
                                         PermanentAddRoadNo = _loan.PermanentAddRoadNo,
                                         PermanentAddRoadName = _loan.PermanentAddRoadName,
                                         PermanentAddArea = _loan.PermanentAddArea,
                                         PermanentAddPostOffice = _loan.PermanentAddPostOffice,
                                         PermanentAddDistrictId = _loan.PermanentAddDistrictId,
                                         PermanentDistrictName = permanentDistrictJoin == null ? "" : (subPermanentDistrict.District ?? ""),
                                         PermanentAddThanaId = _loan.PermanentAddThanaId,
                                         PermanentThanaName = permanentThanaJoin == null ? "" : (subPermanentThana.District ?? ""),
                                         Experience = _loan.Experience,
                                         Occupation = _loan.Occupation,
                                         PermanentAddHouseOwnerName = _loan.PermanentAddHouseOwnerName,
                                         CreditCardNumber = _loan.CreditCardNumber,
                                         ApplicantPhotoUrl = _loan.ApplicantPhotoUrl,
                                         LoanRepayType = _loan.LoanRepayType,
                                         OtherIncomeSource = _loan.OtherIncomeSource,
                                         LoanStatus = new
                                         {
                                             LoanStatusId = loanStatusJoin == null ? 0 : (subLoanStatus == null ? 0 : subLoanStatus.LoanStatusId),
                                             LoanSurveyId = loanStatusJoin == null ? 0 : (subLoanStatus == null ? 0 : subLoanStatus.LoanSurveyId),
                                             StatusCode = loanStatusJoin == null ? "" : (subLoanStatus.StatusCode ?? ""),
                                             Status = loanStatusJoin == null ? "" : (subLoanStatus.Status ?? ""),
                                             Comment = loanStatusJoin == null ? "" : (subLoanStatus.Comment ?? ""),
                                             //CommentDate = loanStatusJoin == null ? DateTime.Now : (subLoanStatus == null ? DateTime.Now : subLoanStatus.CommentDate)
                                         },
                                         CouriersWithLoanSurveyViewModel = new
                                         {
                                             CouriersWithLoanSurveyId = _courier == null ? 0 : _courier.CouriersWithLoanSurveyId,
                                             CourierId = _courier == null ? 0 : _courier.CourierId,
                                             CourierName = _courier == null ? "" : (_courier.CourierName ?? ""),
                                             LoanSurveyId = _courier == null ? 0 : _courier.LoanSurveyId
                                         }
                                     }).OrderByDescending(o => o.LoanSurveyId).ToListAsync();

                loanSurveyData = allData.GroupBy(g => g.CourierUserId).Select(s => new LoanSurveyViewModel
                {
                    LoanSurveyId = s.FirstOrDefault().LoanSurveyId,
                    CourierUserId = s.FirstOrDefault().CourierUserId,
                    MerchantName = s.FirstOrDefault().MerchantName,
                    TradeLicenseImageUrl = s.FirstOrDefault().TradeLicenseImageUrl,
                    InterestedAmount = s.FirstOrDefault().InterestedAmount,
                    TransactionAmount = s.FirstOrDefault().TransactionAmount,
                    IsBankAccount = s.FirstOrDefault().IsBankAccount,
                    IsLocalShop = s.FirstOrDefault().IsLocalShop,
                    ApplicationDate = s.FirstOrDefault().ApplicationDate,
                    MonthlyTotalCodAmount = s.FirstOrDefault().MonthlyTotalCodAmount,
                    GuarantorName = s.FirstOrDefault().GuarantorName,
                    GuarantorMobile = s.FirstOrDefault().GuarantorMobile,
                    MonthlyTotalAverageSale = s.FirstOrDefault().MonthlyTotalAverageSale,
                    LoanAmount = s.FirstOrDefault().LoanAmount,
                    BankName = s.FirstOrDefault().BankName,
                    Gender = s.FirstOrDefault().Gender,
                    Age = s.FirstOrDefault().Age,
                    BasketValue = s.FirstOrDefault().BasketValue,
                    CardHolder = s.FirstOrDefault().CardHolder,
                    CardLimit = s.FirstOrDefault().CardLimit,
                    LoanEmi = s.FirstOrDefault().LoanEmi,
                    HasCreditCard = s.FirstOrDefault().HasCreditCard,
                    HasTin = s.FirstOrDefault().HasTin,
                    EduLevel = s.FirstOrDefault().EduLevel,
                    RepayType = s.FirstOrDefault().RepayType,
                    MonthlyOrder = s.FirstOrDefault().MonthlyOrder,
                    MonthlyExp = s.FirstOrDefault().MonthlyExp,
                    Recommend = s.FirstOrDefault().Recommend,
                    RelationMarchent = s.FirstOrDefault().RelationMarchent,
                    ShopOwnership = s.FirstOrDefault().ShopOwnership,
                    TinNumber = s.FirstOrDefault().TinNumber,
                    HomeOwnership = s.FirstOrDefault().HomeOwnership,
                    Married = s.FirstOrDefault().Married,
                    FamMem = s.FirstOrDefault().FamMem,
                    HasTradeLicense = s.FirstOrDefault().HasTradeLicense,
                    TradeLicenseNo = s.FirstOrDefault().TradeLicenseNo,
                    TradeLicenseExpireDate = s.FirstOrDefault().TradeLicenseExpireDate,
                    CompanyBankAccNo = s.FirstOrDefault().CompanyBankAccNo,
                    CompanyBankAccName = s.FirstOrDefault().CompanyBankAccName,
                    AnnualTotalIncome = s.FirstOrDefault().AnnualTotalIncome,
                    DateOfBirth = s.FirstOrDefault().DateOfBirth,
                    NidNo = s.FirstOrDefault().NidNo,
                    OthersIncome = s.FirstOrDefault().OthersIncome,
                    ReqTenorMonth = s.FirstOrDefault().ReqTenorMonth,
                    ResidenceLocation = s.FirstOrDefault().ResidenceLocation,
                    CollectionAmountAvg = s.FirstOrDefault().CollectionAmountAvg,
                    HasPreviousLoan = s.FirstOrDefault().HasPreviousLoan,
                    LenderType = s.FirstOrDefault().LenderType,
                    BankStatementUrl = s.FirstOrDefault().BankStatementUrl,
                    Comments = s.FirstOrDefault().Comments,
                    BusinessStartDate = s.FirstOrDefault().BusinessStartDate,
                    NidImageUrl = s.FirstOrDefault().NidImageUrl,
                    NidBackImageUrl = s.FirstOrDefault().NidBackImageUrl,
                    TinImageUrl = s.FirstOrDefault().TinImageUrl,
                    CibUploadedFormUrl = s.FirstOrDefault().CibUploadedFormUrl,
                    Address = s.FirstOrDefault().Address,
                    ThanaId = s.FirstOrDefault().ThanaId,
                    ThanaName = s.FirstOrDefault().ThanaName,
                    FatherName = s.FirstOrDefault().FatherName,
                    MotherName = s.FirstOrDefault().MotherName,
                    SpouseName = s.FirstOrDefault().SpouseName,
                    IsLoanDue = s.FirstOrDefault().IsLoanDue,
                    PresentAddHouseNo = s.FirstOrDefault().PresentAddHouseNo,
                    PresentAddRoadNo = s.FirstOrDefault().PresentAddRoadNo,
                    PresentAddRoadName = s.FirstOrDefault().PresentAddRoadName,
                    PresentAddArea = s.FirstOrDefault().PresentAddArea,
                    PresentAddPostOffice = s.FirstOrDefault().PresentAddPostOffice,
                    PresentAddDistrictId = s.FirstOrDefault().PresentAddDistrictId,
                    PresentDistrictName = s.FirstOrDefault().PresentDistrictName,
                    PresentAddThanaId = s.FirstOrDefault().PresentAddThanaId,
                    PresentThanaName = s.FirstOrDefault().PresentThanaName,
                    HouseOwner = s.FirstOrDefault().HouseOwner,
                    IsOwner = s.FirstOrDefault().IsOwner,
                    DurationOfLiving = s.FirstOrDefault().DurationOfLiving,
                    PermanentAddHouseNo = s.FirstOrDefault().PermanentAddHouseNo,
                    PermanentAddRoadNo = s.FirstOrDefault().PermanentAddRoadNo,
                    PermanentAddRoadName = s.FirstOrDefault().PermanentAddRoadName,
                    PermanentAddArea = s.FirstOrDefault().PermanentAddArea,
                    PermanentAddPostOffice = s.FirstOrDefault().PermanentAddPostOffice,
                    PermanentAddDistrictId = s.FirstOrDefault().PermanentAddDistrictId,
                    PermanentDistrictName = s.FirstOrDefault().PermanentDistrictName,
                    PermanentAddThanaId = s.FirstOrDefault().PermanentAddThanaId,
                    PermanentThanaName = s.FirstOrDefault().PermanentThanaName,
                    Experience = s.FirstOrDefault().Experience,
                    Occupation = s.FirstOrDefault().Occupation,
                    PermanentAddHouseOwnerName = s.FirstOrDefault().PermanentAddHouseOwnerName,
                    CreditCardNumber = s.FirstOrDefault().CreditCardNumber,
                    ApplicantPhotoUrl = s.FirstOrDefault().ApplicantPhotoUrl,
                    LoanRepayType = s.FirstOrDefault().LoanRepayType,
                    OtherIncomeSource = s.FirstOrDefault().OtherIncomeSource,
                    LoanApprovedStatusViewModel = s.Where(k => k.LoanStatus.LoanSurveyId == s.FirstOrDefault().LoanSurveyId)
                    .GroupBy(g => g.LoanStatus.LoanStatusId)
                    .Select(a => new LoanApprovedStatusViewModel
                    {
                        LoanApplicationId = a.FirstOrDefault().LoanSurveyId,
                        Status = a.FirstOrDefault().LoanStatus.Status,
                        StatusCode = a.FirstOrDefault().LoanStatus.StatusCode,
                        Comment = a.FirstOrDefault().LoanStatus.Comment
                    }).ToList(),
                    CouriersWithLoanSurveyViewModel = s.Where(w => w.LoanSurveyId == s.FirstOrDefault().LoanSurveyId)
                    .GroupBy(g => g.CouriersWithLoanSurveyViewModel.CouriersWithLoanSurveyId)
                    .Select(r => new CouriersWithLoanSurveyViewModel
                    {
                        CouriersWithLoanSurveyId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CouriersWithLoanSurveyId,
                        CourierId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CourierId,
                        CourierName = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CourierName,
                        LoanSurveyId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.LoanSurveyId
                    }).ToList()
                }).ToList();

            }

            if (orderBodyModel.LastDymanicMonth > 0)
            {
                var array = loanSurveyData.Select(x => x.CourierUserId).ToArray();

                var data = _sqlServerContext.CourierOrders.Where(x => array.Contains(x.MerchantId)
                    && x.OrderDate.Date >= DateTime.Now.Date.AddMonths(-orderBodyModel.LastDymanicMonth)
                    && x.OrderDate.Date < DateTime.Now.Date.AddDays(1)
                    && x.IsDownloaded == true)
                    .GroupBy(g => g.MerchantId).Select(s => new
                    {
                        MerchantId = s.Key,
                        TotalCollectionAmount = s.Sum(c => c.CollectionAmount),
                        Months = s.GroupBy(g => g.OrderDate.Month).Select(grouped => new
                        {
                            Month = grouped.Key
                        }).Count()
                    }).ToList();

                foreach (var item in loanSurveyData)
                {
                    var totalCollectionAmount = data.Where(w => w.MerchantId.Equals(item.CourierUserId)).Count() > 0 ? data.Where(w => w.MerchantId.Equals(item.CourierUserId)).FirstOrDefault().TotalCollectionAmount : 0;

                    int month = data.Where(w => w.MerchantId.Equals(item.CourierUserId)).Count() > 0 ? data.Where(w => w.MerchantId.Equals(item.CourierUserId)).FirstOrDefault().Months : 0;

                    int dynamicMonth = month == 0 ? orderBodyModel.LastDymanicMonth : (month < orderBodyModel.LastDymanicMonth ? month : orderBodyModel.LastDymanicMonth);

                    item.CollectionAmountAvg = totalCollectionAmount / dynamicMonth;
                }

            }

            return loanSurveyData;
        }

        public async Task<IEnumerable<dynamic>> MonthWiseTotalCollectionAmount(int CourierUserId)
        {
            var data = await (from orders in _sqlServerContext.CourierOrders
                              where orders.MerchantId == CourierUserId
                              && orders.IsDownloaded == true
                              group orders by new
                              {
                                  Month = orders.OrderDate.Month,
                                  Year = orders.OrderDate.Year
                              } into orderDateGroup
                              select new
                              {
                                  Month = orderDateGroup.Key.Month,
                                  MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(orderDateGroup.Key.Month),
                                  Year = orderDateGroup.Key.Year,
                                  TotalCollectionAmount = orderDateGroup.Sum(o => o.CollectionAmount)
                              }).OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<dynamic>> GetTelesalesActiveMerchantList(OrderBodyModel orderBodyModel)
        {
            var data = await _sqlServerContext.CourierUsers.Where(w => w.TeleSalesDate.Value.Date >= orderBodyModel.FromDate.Date
                            && w.TeleSalesDate.Value.Date < orderBodyModel.ToDate.Date.AddDays(1)
                            && w.TeleSales != 0).Select(s => new CourierUsers
                            {
                                CourierUserId = s.CourierUserId,
                                TeleSalesDate = s.TeleSalesDate,
                                TeleSales = s.TeleSales
                            }).ToListAsync();

            var res = data.GroupBy(g => new
            {
                Year = g.TeleSalesDate.Value.Year,
                Month = g.TeleSalesDate.Value.Month,
                Day = g.TeleSalesDate.Value.Day
            }).Select(s => new
            {
                Year = s.Key.Year,
                Month = s.Key.Month,
                Day = s.Key.Day,
                TeleSalesDate = s.Key.Year + "-" + s.Key.Month + "-" + s.Key.Day,
                Values = s.GroupBy(sg => sg.TeleSales).Select(x => new
                {
                    TeleSales = getTeleSalesStatusName(x.Key.Value.ToString()),
                    TotalCount = x.Count()
                })
            }).OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).ThenByDescending(o => o.Day).ToList();

            return res;
        }

        private string getTeleSalesStatusName(string value)
        {
            if (value == "1")
            {
                return "Recently Interested";
            }

            else if (value == "2")
            {
                return "Not Interested";
            }
            else if (value == "3")
            {
                return "Business Closed";
            }
            else if (value == "4")
            {
                return "Didn't Pick";
            }
            else if (value == "5")
            {
                return "Late Interested";
            }
            return "";
        }

        public async Task<IEnumerable<dynamic>> AcquisitionLeadManagement(OrderBodyModel orderBodyModel)
        {
            var data = await (from _leadManagement in _sqlServerContext.AcquisitionLeadManagement
                              join _user in _sqlServerContext.Users on _leadManagement.AcquisitionUserId equals
                              _user.UserId
                              where _leadManagement.AcquiredDate.Date >= orderBodyModel.FromDate.Date &&
                              _leadManagement.AcquiredDate.Date < orderBodyModel.ToDate.Date.AddDays(1)
                              select new
                              {
                                  CompanyName = _leadManagement.CompanyName,
                                  Mobile = _leadManagement.Mobile,
                                  AcquiredBy = _user.FullName,
                                  AcquiredDate = _leadManagement.AcquiredDate

                              }).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> ReAttemptOrdersList(OrderBodyModel orderBodyModel)
        {
            var data = from orders in _sqlServerContext.CourierOrders
                       join users in _sqlServerContext.CourierUsers on orders.MerchantId equals users.CourierUserId
                       where orders.PostedOn >= orderBodyModel.FromDate
                       && orders.PostedOn <= orderBodyModel.ToDate
                       && orders.ReAttemptCharge != 0
                       select new CourierOrdersViewModel
                       {
                           MerchantId = orders.MerchantId,
                           CompanyName = users.CompanyName,
                           CustomerName = orders.CustomerName,
                           Mobile = orders.Mobile,
                           OrderDate = orders.OrderDate,
                           ReAttemptCharge = orders.ReAttemptCharge
                       };
            return await data.ToListAsync();
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetVouchers(RequestBodyModel requestBodyModel)
        {
            IEnumerable<CourierUsersViewModel> data = new List<CourierUsersViewModel>();
            if (requestBodyModel.Flag == 1) //filter by courier user id
            {
                data = await (from _voucher in _sqlServerContext.Vouchers
                              join _courierUser in _sqlServerContext.CourierUsers on _voucher.CourierUserId equals _courierUser.CourierUserId
                              join _deliveryRange in _sqlServerContext.DeliveryRange on _voucher.DeliveryRangeId equals _deliveryRange.Id
                              join _user in _sqlServerContext.Users on _voucher.InsertBy equals _user.UserId
                              where _voucher.CourierUserId == requestBodyModel.MerchantId
                              && _voucher.IsActive == requestBodyModel.IsActive
                              select new CourierUsersViewModel
                              {
                                  CompanyName = _courierUser.CompanyName,
                                  DeliveryRange = new DeliveryRange
                                  {
                                      Id = _deliveryRange.Id,
                                      Name = _deliveryRange.Name
                                  },
                                  VouchersViewModel = new VouchersViewModel
                                  {
                                      MerchantMobile = _voucher.MerchantMobile,
                                      VoucherCode = _voucher.VoucherCode,
                                      ApplicableQuantity = _voucher.ApplicableQuantity,
                                      StartTime = _voucher.StartTime,
                                      EndTime = _voucher.EndTime,
                                      VoucherDiscount = _voucher.VoucherDiscount,
                                      CourierUserId = _courierUser.CourierUserId,
                                      IsActive = _voucher.IsActive,
                                      InsertBy = _voucher.InsertBy,
                                      InsertedOn = _voucher.InsertedOn,
                                      FullName = _user.FullName
                                  }
                              }).ToListAsync();
            }
            else if (requestBodyModel.Flag == 2) //filter by date range
            {
                data = await (from _voucher in _sqlServerContext.Vouchers
                              join _courierUser in _sqlServerContext.CourierUsers on _voucher.CourierUserId equals _courierUser.CourierUserId
                              join _deliveryRange in _sqlServerContext.DeliveryRange on _voucher.DeliveryRangeId equals _deliveryRange.Id
                              join _user in _sqlServerContext.Users on _voucher.InsertBy equals _user.UserId
                              where _voucher.StartTime.Date >= requestBodyModel.FromDate
                              && _voucher.EndTime.Date < requestBodyModel.ToDate.AddDays(1)
                              && _voucher.IsActive == requestBodyModel.IsActive
                              select new CourierUsersViewModel
                              {
                                  CompanyName = _courierUser.CompanyName,
                                  DeliveryRange = new DeliveryRange
                                  {
                                      Id = _deliveryRange.Id,
                                      Name = _deliveryRange.Name
                                  },
                                  VouchersViewModel = new VouchersViewModel
                                  {
                                      MerchantMobile = _voucher.MerchantMobile,
                                      VoucherCode = _voucher.VoucherCode,
                                      ApplicableQuantity = _voucher.ApplicableQuantity,
                                      StartTime = _voucher.StartTime,
                                      EndTime = _voucher.EndTime,
                                      VoucherDiscount = _voucher.VoucherDiscount,
                                      CourierUserId = _courierUser.CourierUserId,
                                      IsActive = _voucher.IsActive,
                                      InsertBy = _voucher.InsertBy,
                                      InsertedOn = _voucher.InsertedOn,
                                      FullName = _user.FullName
                                  }
                              }).ToListAsync();
            }
            else if (requestBodyModel.Flag == 3) //filter by courier user id and date range
            {
                data = await (from _voucher in _sqlServerContext.Vouchers
                              join _courierUser in _sqlServerContext.CourierUsers on _voucher.CourierUserId equals _courierUser.CourierUserId
                              join _deliveryRange in _sqlServerContext.DeliveryRange on _voucher.DeliveryRangeId equals _deliveryRange.Id
                              join _user in _sqlServerContext.Users on _voucher.InsertBy equals _user.UserId
                              where _voucher.StartTime.Date >= requestBodyModel.FromDate
                              && _voucher.EndTime.Date < requestBodyModel.ToDate.AddDays(1)
                              && _voucher.CourierUserId == requestBodyModel.MerchantId
                              && _voucher.IsActive == requestBodyModel.IsActive
                              select new CourierUsersViewModel
                              {
                                  CompanyName = _courierUser.CompanyName,
                                  DeliveryRange = new DeliveryRange
                                  {
                                      Id = _deliveryRange.Id,
                                      Name = _deliveryRange.Name
                                  },
                                  VouchersViewModel = new VouchersViewModel
                                  {
                                      MerchantMobile = _voucher.MerchantMobile,
                                      VoucherCode = _voucher.VoucherCode,
                                      ApplicableQuantity = _voucher.ApplicableQuantity,
                                      StartTime = _voucher.StartTime,
                                      EndTime = _voucher.EndTime,
                                      VoucherDiscount = _voucher.VoucherDiscount,
                                      CourierUserId = _courierUser.CourierUserId,
                                      IsActive = _voucher.IsActive,
                                      InsertBy = _voucher.InsertBy,
                                      InsertedOn = _voucher.InsertedOn,
                                      FullName = _user.FullName
                                  }
                              }).ToListAsync();
            }
            return data;
        }

        public async Task<IEnumerable<dynamic>> TelesalesDetails(int teleSalesStatus, DateTime date)
        {


            var data = await (from _user in _sqlServerContext.CourierUsers
                              join _order in _sqlServerContext.CourierOrders
                              on _user.CourierUserId equals _order.MerchantId
                              where _user.TeleSales == teleSalesStatus &&
                              _user.TeleSalesDate.Value.Date == date
                              select new
                              {
                                  _user.CourierUserId,
                                  _user.CompanyName,
                                  _user.Mobile,
                                  _order.Id,
                                  _order.OrderDate,
                                  _order.MerchantId,
                                  _order.CourierOrdersId,
                                  _user.AlterMobile,
                                  _user.BkashNumber
                              }).OrderByDescending(y => y.OrderDate).ToListAsync();

            var groupedData = data.GroupBy(x => x.MerchantId)
                .Select(item => new

                {
                    //TotalOrder = item.Select(o => o.Id).Count(),
                    //CourierOrdersId = item.Select(o => o.CourierOrdersId),
                    CourierUserId = item.Select(o => o.CourierUserId).FirstOrDefault(),
                    LastOrderDate = item.Select(o => o.OrderDate).FirstOrDefault(),
                    CompanyName = item.Select(o => o.CompanyName).FirstOrDefault(),
                    Mobile = item.Select(o => o.Mobile).FirstOrDefault(),
                    AlterMobile = item.Select(o => o.AlterMobile).FirstOrDefault(),
                    BkashNumber = item.Select(o => o.BkashNumber).FirstOrDefault()
                });
            return groupedData;
        }

        public async Task<IEnumerable<dynamic>> StatusWiseTelesalesDetails(RequestBodyModel requestBodyModel)
        {
            List<CourierOrdersViewModel> data = new List<CourierOrdersViewModel>();
            if (requestBodyModel.RetentionUserId == 0)
            {
                data = await (from _user in _sqlServerContext.CourierUsers
                              join _order in _sqlServerContext.CourierOrders
                              on _user.CourierUserId equals _order.MerchantId
                              where _user.TeleSales == requestBodyModel.StatusId &&
                              _user.TeleSalesDate.Value.Date == requestBodyModel.FromDate
                              select new CourierOrdersViewModel
                              {
                                  CompanyName = _user.CompanyName,
                                  Mobile = _user.Mobile,
                                  Id = _order.Id,
                                  OrderDate = _order.OrderDate,
                                  MerchantId = _order.MerchantId,
                                  CourierOrdersId = _order.CourierOrdersId,
                                  AlterMobile = _user.AlterMobile,
                                  CourierUsers = new CourierUsersViewModel
                                  {
                                      BkashNumber = _user.BkashNumber
                                  }
                              }).OrderByDescending(y => y.OrderDate).ToListAsync();
            }
            else if (requestBodyModel.RetentionUserId != 0)
            {
                data = await (from _user in _sqlServerContext.CourierUsers
                              join _order in _sqlServerContext.CourierOrders
                              on _user.CourierUserId equals _order.MerchantId
                              where _user.TeleSales == requestBodyModel.StatusId
                              && _user.TeleSalesDate.Value.Date == requestBodyModel.FromDate
                              && _user.RetentionUserId == requestBodyModel.RetentionUserId
                              select new CourierOrdersViewModel
                              {
                                  CompanyName = _user.CompanyName,
                                  Mobile = _user.Mobile,
                                  Id = _order.Id,
                                  OrderDate = _order.OrderDate,
                                  MerchantId = _order.MerchantId,
                                  CourierOrdersId = _order.CourierOrdersId,
                                  AlterMobile = _user.AlterMobile,
                                  CourierUsers = new CourierUsersViewModel
                                  {
                                      BkashNumber = _user.BkashNumber
                                  }
                              }).OrderByDescending(y => y.OrderDate).ToListAsync();
            }

            var groupedData = data.GroupBy(x => x.MerchantId)
                .Select(item => new
                {
                    //TotalOrder = item.Select(o => o.Id).Count(),
                    //CourierOrdersId = item.Select(o => o.CourierOrdersId),
                    CouirerUserId = item.Select(o => o.MerchantId).FirstOrDefault(),
                    LastOrderDate = item.Select(o => o.OrderDate).FirstOrDefault(),
                    CompanyName = item.Select(o => o.CompanyName).FirstOrDefault(),
                    Mobile = item.Select(o => o.Mobile).FirstOrDefault(),
                    AlterMobile = item.Select(o => o.AlterMobile).FirstOrDefault(),
                    BkashNumber = item.Select(o => o.CourierUsers.BkashNumber).FirstOrDefault()
                });
            return groupedData;
        }

        public async Task<IEnumerable<dynamic>> StatusWiseHistoryCount(RequestBodyModel bodyModel)
        {
            var statusArray = new int[] { };
            var statusReturnArray = new int[] { 19, 21, 38, 39, 59, 60 };
            var statusShipmentArray = new int[] { 7, 8, 9, 10, 36, 37 };

            statusArray = bodyModel.StatusId == 1 ? statusShipmentArray : statusReturnArray;

            var data = await (from _statusHistory in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                              join _status in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                              on _statusHistory.Status equals _status.StatusId

                              where statusArray.Contains(_statusHistory.Status)
                              && _statusHistory.PostedOn.Date >= bodyModel.FromDate.Date
                              && _statusHistory.PostedOn < bodyModel.ToDate.Date.AddDays(1)

                              select new
                              {
                                  _statusHistory.Status,
                                  _statusHistory.CourierId,
                                  _status.StatusNameEng
                              }).ToListAsync();

            if (bodyModel.CourierId != -1)
            {
                data = data.Where(c => c.CourierId == bodyModel.CourierId).ToList();
            }

            var countData = data.GroupBy(b => b.Status).Select(s => new
            {
                Status = s.Key,
                StatusNameEng = s.FirstOrDefault().StatusNameEng,
                TotalOrder = s.Count()
            }).OrderBy(s => s.Status);

            return countData;
        }

        public async Task<IEnumerable<dynamic>> StatusWiseHistoryCountDetails(RequestBodyModel bodyModel)
        {
            var allDistricts = await _sqlServerContext.Districts.Where(z => z.IsActive == true).Select(s => new
            {
                s.District,
                s.DistrictBng,
                s.DistrictId,
                s.ParentId,
                s.IsActive
            }).ToListAsync();

            var data = await (from _statusHistory in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                              join _orderStatus in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                              on _statusHistory.Status equals _orderStatus.StatusId
                              join _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                              on _statusHistory.CourierOrderId equals _courierOrders.CourierOrdersId
                              join _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                              on _statusHistory.MerchantId equals _courierUsers.CourierUserId
                              join _couriers in _sqlServerContext.Couriers.AsNoTracking()
                              on _statusHistory.CourierId equals _couriers.CourierId
                              join users in _sqlServerContext.Users.AsNoTracking()
                              on _statusHistory.PostedBy equals users.UserId
                              into user
                              from _users in user.DefaultIfEmpty()

                              where _statusHistory.Status.Equals(bodyModel.StatusId)
                              && _statusHistory.PostedOn.Date >= bodyModel.FromDate.Date
                              && _statusHistory.PostedOn.Date < bodyModel.ToDate.Date.AddDays(1)

                              select new
                              {
                                  OrderDate = _statusHistory.OrderDate,
                                  CourierOrderId = _statusHistory.CourierOrderId,
                                  PostedOn = _statusHistory.PostedOn,
                                  CompanyName = _courierUsers.CompanyName,
                                  Address = _courierUsers.Address,
                                  IsConfirmedBy = _statusHistory.IsConfirmedBy,
                                  PodNumber = _statusHistory.PodNumber,
                                  HubName = _statusHistory.HubName,
                                  CourierName = _couriers.CourierName,
                                  FullName = _users.FullName,
                                  CourierId = _statusHistory.CourierId,
                                  CustomerName = _courierOrders.CustomerName,
                                  Mobile = _courierOrders.Mobile,
                                  CustomerAddress = _courierOrders.Address,
                                  DistrictName = allDistricts.Where(d=> d.DistrictId == _courierOrders.DistrictId).Select(s=> s.District).FirstOrDefault(),
                                  MerchantDistrictName = allDistricts.Where(d=> d.DistrictId == _courierUsers.DistrictId).Select(s=> s.District).FirstOrDefault(),
                                  ThanaName = _courierOrders.ThanaId != 0 ? allDistricts.Where(d => d.DistrictId == _courierOrders.ThanaId).Select(s => s.District).FirstOrDefault() : null,
                                  AreaName = _courierOrders.AreaId != 0 ? allDistricts.Where(d => d.DistrictId == _courierOrders.AreaId).Select(s => s.District).FirstOrDefault() : null,
                                  CollectionAmount = _courierOrders.CollectionAmount,
                                  ActualPackagePrice = _courierOrders.ActualPackagePrice,
                                  PaymentType = _courierOrders.PaymentType,
                                  DeliveryCharge = _courierOrders.DeliveryCharge,
                                  Comment = _statusHistory.Comment
                              })
                              .OrderByDescending(b => b.CourierOrderId)
                              .OrderByDescending(b => b.PostedOn)
                              .ToListAsync();

            if (bodyModel.CourierId != -1)
            {
                data = data.Where(c => c.CourierId == bodyModel.CourierId).ToList();
            }

            return data;
        }

        public async Task<IEnumerable<dynamic>> GetReRoutedOrders(OrderBodyModel orderBodyModel)
        {
            var data = await (from history in _sqlServerContext.CourierOrderStatusHistory
                              join couriers in _sqlServerContext.Couriers
                              on history.CourierId equals couriers.CourierId
                              where history.OrderDate >= orderBodyModel.FromDate
                              && history.OrderDate <= orderBodyModel.ToDate
                              && history.Status == 8
                              select new
                              {
                                  CourierOrderId = history.CourierOrderId,
                                  PostedOn = history.PostedOn,
                                  Couriers = new
                                  {
                                      CourierId = history.CourierId,
                                      CourierName = couriers.CourierName,
                                      PostedOn = history.PostedOn
                                  }
                              }).ToListAsync();

            var groupedData = data.GroupBy(g => g.CourierOrderId)
                                    .Select(s => new
                                    {
                                        CourierOrderId = s.FirstOrDefault().CourierOrderId,
                                        PostedOn = s.FirstOrDefault().PostedOn,
                                        Couriers = s.Select(r => new
                                        {
                                            CourierId = r.Couriers.CourierId,
                                            CourierName = r.Couriers.CourierName,
                                            PostedOn = r.Couriers.PostedOn
                                        }).ToList()
                                    });

            var dataToReturn = groupedData.Where(x => x.Couriers.Count() > 1);

            return dataToReturn;
        }

        public async Task<IEnumerable<dynamic>> SlotBasedOrder(RequestBodyModel requestBody)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: requestBody.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: requestBody.ToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@slotId", value: requestBody.Flag, dbType: DbType.Int32);

                connection.Open();
                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[SlotWiseOrderCount]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );

                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> TigerReport(RequestBodyModel requestBodyModel)
        {
            var data = await (from order in _sqlServerContext.CourierOrders
                              join orderHistory in _sqlServerContext.CourierOrderStatusHistory
                              on order.CourierOrdersId equals orderHistory.CourierOrderId
                              join courier in _sqlServerContext.Couriers
                              on order.CourierId equals courier.CourierId
                              where order.PostedOn >= requestBodyModel.FromDate
                              && order.PostedOn < requestBodyModel.ToDate.AddDays(1)
                              && orderHistory.Status == 15
                              && order.ExpectedDeliveryDate != null
                              && courier.CourierName.ToLower().Contains("tiger")
                              select new
                              {
                                  CourierOrderId = order.CourierOrdersId,
                                  ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                                  DeliveryDate = orderHistory.PostedOn,
                                  CourierId = order.CourierId,
                                  CourierName = courier.CourierName
                              }).ToListAsync();

            var groupedData = data.GroupBy(g => g.CourierOrderId)
                .Select(s => new
                {
                    CourierOrderId = s.FirstOrDefault().CourierOrderId,
                    ExpectedDeliveryDate = s.FirstOrDefault().ExpectedDeliveryDate,
                    DeliveryDate = s.FirstOrDefault().DeliveryDate,
                    DeliveryFlag = s.FirstOrDefault().ExpectedDeliveryDate > s.FirstOrDefault().DeliveryDate ? 1 : 0,
                    CourierId = s.FirstOrDefault().CourierId,
                    CourierName = s.FirstOrDefault().CourierName
                });

            return groupedData;
        }
        public async Task<IEnumerable<dynamic>> ReturnCourierPendingStatusCount(RequestBodyModel requestBodyModel)
        {
            var data = await (from orders in _sqlServerContext.CourierOrders.AsNoTracking()
                              join orderHistory in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                              on orders.CourierOrdersId equals orderHistory.CourierOrderId
                              where orders.Status == requestBodyModel.StatusId
                              && orderHistory.Status == 59
                              && orders.UpdatedOn.Date >= requestBodyModel.FromDate.Date
                              && orders.UpdatedOn.Date < requestBodyModel.ToDate.Date.AddDays(1)
                              select new
                              {
                                  orderHistory.Comment,
                                  orders.Status
                              }).ToListAsync();
            var countData = data.GroupBy(b => b.Comment).Select(s => new
            {
                Id = s.Key,
                HubName = s.FirstOrDefault().Comment,
                Status = s.FirstOrDefault().Status,
                TotalHub = s.Count()
            });
            return countData;
        }
        public async Task<IEnumerable<dynamic>> ReturnCourierPendingStatusDetails(RequestBodyModel bodyModel)
        {
            //var statusArray = new int[] { };
            var statusReturnArray = new int[] { 16, 17, 26 };
            //statusArray = bodyModel.DateFlag == 1 ? statusReturnArray : new int[] { bodyModel.StatusId };
            var data = await (from orders in _sqlServerContext.CourierOrders.AsNoTracking()
                              join ordersStatus in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                              on orders.Status equals ordersStatus.StatusId
                              join courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                              on orders.MerchantId equals courierUsers.CourierUserId
                              join couriers in _sqlServerContext.Couriers.AsNoTracking()
                              on orders.CourierId equals couriers.CourierId
                              join users in _sqlServerContext.Users.AsNoTracking()
                              on orders.PostedBy equals users.UserId
                              into user
                              from _users in user.DefaultIfEmpty()

                              where statusReturnArray.Contains(orders.Status)
                              && orders.UpdatedOn.Date >= bodyModel.FromDate.Date
                              && orders.UpdatedOn.Date < bodyModel.ToDate.Date.AddDays(1)

                              select new
                              {
                                  OrderDate = orders.OrderDate,
                                  CourierOrdersId = orders.CourierOrdersId,
                                  PostedOn = orders.PostedOn,
                                  CompanyName = courierUsers.CompanyName,
                                  Address = courierUsers.Address,
                                  IsConfirmedBy = orders.IsConfirmedBy,
                                  PodNumber = orders.PodNumber,
                                  HubName = orders.HubName,
                                  CollectionAmount = orders.CollectionAmount,
                                  ActualPackagePrice = orders.ActualPackagePrice,
                                  StatusNameEng = ordersStatus.StatusNameEng,
                                  CourierName = couriers.CourierName,
                                  FullName = _users.FullName,
                                  CourierId = orders.CourierId,
                                  CourierDeliveryManName = orders.CourierDeliveryManName,
                                  Comment = orders.Comment,
                                  UpdatedOn = orders.UpdatedOn,
                                  Status = orders.Status,
                              }).OrderByDescending(b => b.CourierOrdersId).ToListAsync();

            if (bodyModel.CourierId != -1)
            {
                data = data.Where(c => c.CourierId == bodyModel.CourierId).ToList();
            }

            return data;
        }
        public async Task<IEnumerable<dynamic>> ReturnCourierPendingStatusCountDetails(RequestBodyModel bodyModel)
        {
            var data = await (from orders in _sqlServerContext.CourierOrders.AsNoTracking()
                              join ordersStatus in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                              on orders.Status equals ordersStatus.StatusId
                              join orderHistory in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                              on orders.CourierOrdersId equals orderHistory.CourierOrderId
                              join courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                              on orders.MerchantId equals courierUsers.CourierUserId
                              join couriers in _sqlServerContext.Couriers.AsNoTracking()
                              on orders.CourierId equals couriers.CourierId
                              join users in _sqlServerContext.Users.AsNoTracking()
                              on orders.PostedBy equals users.UserId
                              into user
                              from _users in user.DefaultIfEmpty()

                              where orders.Status == bodyModel.StatusId
                              && orderHistory.Status == 59
                              && orderHistory.Comment == bodyModel.Comment
                              && orders.UpdatedOn.Date >= bodyModel.FromDate.Date
                              && orders.UpdatedOn.Date < bodyModel.ToDate.Date.AddDays(1)

                              select new
                              {
                                  OrderDate = orders.OrderDate,
                                  CourierOrdersId = orders.CourierOrdersId,
                                  PostedOn = orders.PostedOn,
                                  CompanyName = courierUsers.CompanyName,
                                  Address = courierUsers.Address,
                                  IsConfirmedBy = orders.IsConfirmedBy,
                                  PodNumber = orders.PodNumber,
                                  HubName = orders.HubName,
                                  CollectionAmount = orders.CollectionAmount,
                                  ActualPackagePrice = orders.ActualPackagePrice,
                                  StatusNameEng = ordersStatus.StatusNameEng,
                                  CourierName = couriers.CourierName,
                                  FullName = _users.FullName,
                                  CourierId = orders.CourierId,
                                  CourierDeliveryManName = orders.CourierDeliveryManName,
                                  Comment = orders.Comment,
                                  UpdatedOn = orders.UpdatedOn,
                                  Status = orders.Status,
                              }).OrderByDescending(b => b.CourierOrdersId).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<RiderPaymentReportViewModel>> RiderPaymentReport(RequestBodyModel requestBody)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromdate", value: requestBody.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@todate", value: requestBody.ToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@IsDownloaded", value: 1, dbType: DbType.Int32);

                connection.Open();
                var data = await connection.QueryAsync<RiderPaymentReportViewModel>(
                    sql: @"[DT].[USP_RiderPaymentReport]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );

                return data.ToList();
            }
        }
    }
}
