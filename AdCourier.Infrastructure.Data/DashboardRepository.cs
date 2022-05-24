using AdCourier.Context;
using AdCourier.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdCourier.Domain.Entities.BodyModel.Dashboard;
using AdCourier.Domain.Entities.ViewModel.Other;
using AdCourier.Domain.Entities.DataModel;
using System.Data.SqlClient;
using AdCourier.Domain.Entities;
using Microsoft.Extensions.Options;
using Dapper;
using System.Data;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;

namespace AdCourier.Infrastructure.Data
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        public DashboardRepository(SqlServerContext sqlServerContext, IOptions<ConnectionStringList> connectionStrings)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<StatusGroupViewModel> GetCollection(int courierUserId)
        {
            int[] pickStatusArray = { 3, 44 };
            return new StatusGroupViewModel
            {
                StatusGroupId = 0,
                Name = "কালেকশন করা হয়েছে",
                Count = await _sqlServerContext.CourierOrderStatusHistory.Where(x => x.PostedOn.Date == DateTime.Today
                && pickStatusArray.Contains(x.Status)
                && x.MerchantId.Equals(courierUserId)).GroupBy(x=>x.CourierOrderId).Distinct().CountAsync(),
                DashboardSpanCount = 1,
                DashboardViewColorType = "",
                DashboardViewOrderBy = 1,
                DashboardRouteUrl = "",
                DashboardCountSumView = "",
                TotalAmount = 0,
                DashboardStatusFilter = "",
                DashboardImageUrl = ""
            };
        }

        public async Task<DashBoardOrderViewModel> GetDashBoardOrder(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: orderCountByStatusGroupBodyModel.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ToDate", value: orderCountByStatusGroupBodyModel.ToDate, dbType: DbType.DateTime);
                parameter.Add(name: "@CourierUserId", value: orderCountByStatusGroupBodyModel.CourierUserId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<DashBoardOrderViewModel>(
                        sql: @"[DT].[USP_GetDashBoardOrder]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.FirstOrDefault();
            }
        }
        public async Task<IEnumerable<CourierOrderStatusHistory>> GetCollectionHistory(int courierUserId)
        {
            int[] pickStatusArray = { 3, 44 };

            IQueryable<CourierOrderStatusHistory> data = _sqlServerContext.CourierOrderStatusHistory
                .Where(x => x.PostedOn.Date == DateTime.Today && pickStatusArray.Contains(x.Status)
                && x.MerchantId.Equals(courierUserId)).Distinct()
                .Select(s => s);
            data = data.GroupBy(x => x.CourierOrderId).Select(s => new CourierOrderStatusHistory
            {
                Id = s.FirstOrDefault().Id,
                CourierOrderId = s.FirstOrDefault().CourierOrderId,
                IsConfirmedBy = s.FirstOrDefault().IsConfirmedBy,
                OrderDate = s.FirstOrDefault().OrderDate,
                Status = s.FirstOrDefault().Status,
                PostedOn = s.FirstOrDefault().PostedOn,
                PostedBy = s.FirstOrDefault().PostedBy,
                MerchantId = s.FirstOrDefault().MerchantId,
                Comment = s.FirstOrDefault().Comment,
                PodNumber = s.FirstOrDefault().PodNumber,
                CourierId = s.FirstOrDefault().CourierId,
                HubName = s.FirstOrDefault().HubName,
                CourierDeliveryManName = s.FirstOrDefault().CourierDeliveryManName,
                CourierDeliveryManMobile = s.FirstOrDefault().CourierDeliveryManMobile,
            }); 
            return await data.ToListAsync();
        }

        public async Task<MerchantAdvanceBalanceInfo> GetMerchantBalanceInfo(int courierUserId, int totalAmount)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@courierUserId", value: courierUserId, dbType: DbType.Int32);
                parameter.Add(name: "@totalAmount", value: totalAmount, dbType: DbType.Int32);

                var data = await connection.QueryAsync<MerchantAdvanceBalanceInfo>(
                        sql: @"[DT].[USP_GetMerchantBalanceInfo]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<StatusGroupViewModel>> GetOrderCountByStatusGroup(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            IQueryable<StatusGroupViewModel> results2 = null;

            IQueryable<StatusGroupViewModel> results1 = (from co in _sqlServerContext.StatusGroup
                                                         where !co.DashboardStatusGroup.Equals("")
                                                         select new StatusGroupViewModel
                                                         {
                                                             StatusGroupId = co.StatusGroupId,
                                                             Name = co.DashboardStatusGroup,
                                                             Count = 0,
                                                             DashboardSpanCount = co.DashboardSpanCount,
                                                             DashboardViewColorType = co.DashboardViewColorType,
                                                             DashboardViewOrderBy = co.DashboardViewOrderBy,
                                                             DashboardRouteUrl = co.DashboardRouteUrl,
                                                             DashboardCountSumView = co.DashboardCountSumView,
                                                             TotalAmount = 0,
                                                             DashboardStatusFilter = co.DashboardStatusFilter,
                                                             DashboardImageUrl = co.DashboardImageUrl
                                                         }).Distinct();

            var data1 = await results1.ToListAsync();
            if (orderCountByStatusGroupBodyModel.Month > 0 && orderCountByStatusGroupBodyModel.Year > 0)
            {
                results2 = from cos in _sqlServerContext.CourierOrderStatus
                           join o in _sqlServerContext.CourierOrders on cos.StatusId equals o.Status
                           where o.MerchantId.Equals(orderCountByStatusGroupBodyModel.CourierUserId)
                           && o.OrderDate.Month.Equals(orderCountByStatusGroupBodyModel.Month)
                           && o.OrderDate.Year.Equals(orderCountByStatusGroupBodyModel.Year)
                           && !cos.DashboardStatusGroup.Equals("")
                           group new { cos, o } by new { cos.DashboardStatusGroup } into cosoGroup
                           let firstCosogroup = cosoGroup.FirstOrDefault()
                           let courierOrderStatus = firstCosogroup.cos
                           let courierOrders = firstCosogroup.o
                           let count = cosoGroup.Select(x => x.o.CourierOrdersId).Count()

                           let total = cosoGroup.Select(x => x.o.BreakableCharge
                           + x.o.DeliveryCharge + x.o.CodCharge + x.o.CollectionCharge
                           + x.o.ReturnCharge + x.o.PackagingCharge).Sum()

                           let collectionAmount = cosoGroup.Select(a => a.o.CollectionAmount).Sum()

                           select new StatusGroupViewModel
                           {
                               StatusGroupId = 0,
                               Name = courierOrderStatus.DashboardStatusGroup,
                               Count = count,
                               DashboardSpanCount = 0,
                               DashboardViewColorType = "",
                               DashboardViewOrderBy = 0,
                               DashboardRouteUrl = "",
                               DashboardCountSumView = "",
                               TotalAmount = collectionAmount > 0 ? (collectionAmount - total) : total,
                               DashboardStatusFilter = "",
                               DashboardImageUrl = ""
                           };
            }
            else
            {
                results2 = from cos in _sqlServerContext.CourierOrderStatus
                           join o in _sqlServerContext.CourierOrders on cos.StatusId equals o.Status
                           where o.MerchantId.Equals(orderCountByStatusGroupBodyModel.CourierUserId)
                           && o.OrderDate.Date >= orderCountByStatusGroupBodyModel.FromDate.Date
                           && o.OrderDate.Date < orderCountByStatusGroupBodyModel.ToDate.Date.AddDays(1)
                           && !cos.DashboardStatusGroup.Equals("")
                           group new { cos, o } by new { cos.DashboardStatusGroup } into cosoGroup
                           let firstCosogroup = cosoGroup.FirstOrDefault()
                           let courierOrderStatus = firstCosogroup.cos
                           let courierOrders = firstCosogroup.o
                           let count = cosoGroup.Select(x => x.o.CourierOrdersId).Count()

                           let total = cosoGroup.Select(x => x.o.BreakableCharge
                           + x.o.DeliveryCharge + x.o.CodCharge + x.o.CollectionCharge
                           + x.o.ReturnCharge + x.o.PackagingCharge).Sum()

                           let collectionAmount = cosoGroup.Select(a => a.o.CollectionAmount).Sum()

                           select new StatusGroupViewModel
                           {
                               StatusGroupId = 0,
                               Name = courierOrderStatus.DashboardStatusGroup,
                               Count = count,
                               DashboardSpanCount = 0,
                               DashboardViewColorType = "",
                               DashboardViewOrderBy = 0,
                               DashboardRouteUrl = "",
                               DashboardCountSumView = "",
                               TotalAmount = collectionAmount > 0 ? (collectionAmount - total) : total,
                               DashboardStatusFilter = "",
                               DashboardImageUrl = ""
                           };
            }

             

            var data2 = await results2.ToListAsync();

            var data = data1.Union(data2);
            var res = from d in data
                      group d by d.Name into statusGroup

                      let dashboardStatusFilter = statusGroup.Select(s => s.DashboardStatusFilter).FirstOrDefault()
                      let dashboardImageUrl = statusGroup.Select(s => s.DashboardImageUrl).FirstOrDefault()

                      select new StatusGroupViewModel
                      {
                          StatusGroupId = statusGroup.Select(x => x.StatusGroupId).Sum(),
                          Name = statusGroup.Key,
                          Count = statusGroup.Select(x => x.Count).Sum(),
                          DashboardSpanCount = statusGroup.Select(x => x.DashboardSpanCount).Sum(),
                          DashboardViewColorType = statusGroup.Select(s => s.DashboardViewColorType).FirstOrDefault(),
                          DashboardViewOrderBy = statusGroup.Select(x => x.DashboardViewOrderBy).Sum(),
                          DashboardRouteUrl = statusGroup.Select(s => s.DashboardRouteUrl).FirstOrDefault(),
                          DashboardCountSumView = statusGroup.Select(s => s.DashboardCountSumView).FirstOrDefault(),
                          TotalAmount = statusGroup.Select(x => x.TotalAmount).Sum(),
                          DashboardStatusFilter = dashboardStatusFilter,
                          DashboardImageUrl = dashboardImageUrl
                      };



            var response = (from r in res

                            let groupSum = (from e in res
                                      where r.DashboardStatusFilter.Split(',').Contains(e.Name)
                                      select e.Count).Sum()

                            let totalAmountSum = (from e in res
                                            where r.DashboardStatusFilter.Split(',').Contains(e.Name)
                                            select e.TotalAmount).Sum()

                            select new StatusGroupViewModel
                            {
                                StatusGroupId = r.StatusGroupId,
                                Name = r.Name,
                                Count = groupSum > 0 ? (r.Count + groupSum) : r.Count,
                                DashboardSpanCount = r.DashboardSpanCount,
                                DashboardViewColorType = r.DashboardViewColorType,
                                DashboardViewOrderBy = r.DashboardViewOrderBy,
                                DashboardRouteUrl = r.DashboardRouteUrl,
                                DashboardCountSumView = r.DashboardCountSumView,
                                TotalAmount = groupSum > 0 ? (r.TotalAmount + totalAmountSum) : r.TotalAmount,
                                DashboardStatusFilter = groupSum > 0 ? (r.DashboardStatusFilter + ',' + r.Name) : r.Name,
                                DashboardImageUrl = r.DashboardImageUrl
                            }).ToList();

            return response;
        }

        public async Task<IEnumerable<StatusGroupViewModel>> GetOrderCountByStatusGroup2(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            IQueryable<StatusGroupViewModel> results2 = null;

            IQueryable<StatusGroupViewModel> results1 = (from co in _sqlServerContext.StatusGroup
                                                         where !co.DashboardStatusGroup.Equals("")
                                                         && !co.DashboardStatusGroup.Trim().Equals("পেমেন্ট প্রসেসিং-এ আছে")
                                                         select new StatusGroupViewModel
                                                         {
                                                             StatusGroupId = co.StatusGroupId,
                                                             Name = co.DashboardStatusGroup,
                                                             Count = 0,
                                                             DashboardSpanCount = co.DashboardSpanCount,
                                                             DashboardViewColorType = co.DashboardViewColorType,
                                                             DashboardViewOrderBy = co.DashboardViewOrderBy,
                                                             DashboardRouteUrl = co.DashboardRouteUrl,
                                                             DashboardCountSumView = co.DashboardCountSumView,
                                                             TotalAmount = 0,
                                                             DashboardStatusFilter = co.DashboardStatusFilter,
                                                             DashboardImageUrl = co.DashboardImageUrl
                                                         }).Distinct();

            var data1 = await results1.ToListAsync();

            if (orderCountByStatusGroupBodyModel.Month > 0 && orderCountByStatusGroupBodyModel.Year > 0)
            {
                results2 = from cos in _sqlServerContext.CourierOrderStatus
                           join o in _sqlServerContext.CourierOrders on cos.StatusId equals o.Status
                           where o.MerchantId.Equals(orderCountByStatusGroupBodyModel.CourierUserId)
                           && o.OrderDate.Month.Equals(orderCountByStatusGroupBodyModel.Month)
                           && o.OrderDate.Year.Equals(orderCountByStatusGroupBodyModel.Year)
                           && !cos.DashboardStatusGroup.Equals("")
                           && !cos.DashboardStatusGroup.Trim().Equals("পেমেন্ট প্রসেসিং-এ আছে")
                           group new { cos, o } by new { cos.DashboardStatusGroup } into cosoGroup
                           let firstCosogroup = cosoGroup.FirstOrDefault()
                           let courierOrderStatus = firstCosogroup.cos
                           let courierOrders = firstCosogroup.o
                           let count = cosoGroup.Select(x => x.o.CourierOrdersId).Count()

                           let total = cosoGroup.Select(x => x.o.BreakableCharge
                           + x.o.DeliveryCharge + x.o.CodCharge + x.o.CollectionCharge
                           + x.o.ReturnCharge + x.o.PackagingCharge).Sum()

                           let collectionAmount = cosoGroup.Select(a => a.o.CollectionAmount).Sum()

                           select new StatusGroupViewModel
                           {
                               StatusGroupId = 0,
                               Name = courierOrderStatus.DashboardStatusGroup,
                               Count = count,
                               DashboardSpanCount = 0,
                               DashboardViewColorType = "",
                               DashboardViewOrderBy = 0,
                               DashboardRouteUrl = "",
                               DashboardCountSumView = "",
                               TotalAmount = collectionAmount > 0 ? (collectionAmount - total) : total,
                               DashboardStatusFilter = "",
                               DashboardImageUrl = ""
                           };
            }
            else
            {
                results2 = from cos in _sqlServerContext.CourierOrderStatus
                           join o in _sqlServerContext.CourierOrders on cos.StatusId equals o.Status
                           where o.MerchantId.Equals(orderCountByStatusGroupBodyModel.CourierUserId)
                           && o.OrderDate.Date >= orderCountByStatusGroupBodyModel.FromDate.Date
                           && o.OrderDate.Date < orderCountByStatusGroupBodyModel.ToDate.Date.AddDays(1)
                           && !cos.DashboardStatusGroup.Equals("")
                           && !cos.DashboardStatusGroup.Trim().Equals("পেমেন্ট প্রসেসিং-এ আছে")
                           group new { cos, o } by new { cos.DashboardStatusGroup } into cosoGroup
                           let firstCosogroup = cosoGroup.FirstOrDefault()
                           let courierOrderStatus = firstCosogroup.cos
                           let courierOrders = firstCosogroup.o
                           let count = cosoGroup.Select(x => x.o.CourierOrdersId).Count()

                           let total = cosoGroup.Select(x => x.o.BreakableCharge
                           + x.o.DeliveryCharge + x.o.CodCharge + x.o.CollectionCharge
                           + x.o.ReturnCharge + x.o.PackagingCharge).Sum()

                           let collectionAmount = cosoGroup.Select(a => a.o.CollectionAmount).Sum()

                           select new StatusGroupViewModel
                           {
                               StatusGroupId = 0,
                               Name = courierOrderStatus.DashboardStatusGroup,
                               Count = count,
                               DashboardSpanCount = 0,
                               DashboardViewColorType = "",
                               DashboardViewOrderBy = 0,
                               DashboardRouteUrl = "",
                               DashboardCountSumView = "",
                               TotalAmount = collectionAmount > 0 ? (collectionAmount - total) : total,
                               DashboardStatusFilter = "",
                               DashboardImageUrl = ""
                           };
            }

            

            var data2 = await results2.ToListAsync();

            var data = data1.Union(data2);
            var res = from d in data
                      group d by d.Name into statusGroup

                      let dashboardStatusFilter = statusGroup.Select(s => s.DashboardStatusFilter).FirstOrDefault()
                      let dashboardImageUrl = statusGroup.Select(s => s.DashboardImageUrl).FirstOrDefault()

                      select new StatusGroupViewModel
                      {
                          StatusGroupId = statusGroup.Select(x => x.StatusGroupId).Sum(),
                          Name = statusGroup.Key,
                          Count = statusGroup.Select(x => x.Count).Sum(),
                          DashboardSpanCount = statusGroup.Select(x => x.DashboardSpanCount).Sum(),
                          DashboardViewColorType = statusGroup.Select(s => s.DashboardViewColorType).FirstOrDefault(),
                          DashboardViewOrderBy = statusGroup.Select(x => x.DashboardViewOrderBy).Sum(),
                          DashboardRouteUrl = statusGroup.Select(s => s.DashboardRouteUrl).FirstOrDefault(),
                          DashboardCountSumView = statusGroup.Select(s => s.DashboardCountSumView).FirstOrDefault(),
                          TotalAmount = statusGroup.Select(x => x.TotalAmount).Sum(),
                          DashboardStatusFilter = dashboardStatusFilter,
                          DashboardImageUrl = dashboardImageUrl
                      };



            var response = (from r in res

                            let groupSum = (from e in res
                                            where r.DashboardStatusFilter.Split(',').Contains(e.Name)
                                            select e.Count).Sum()

                            let totalAmountSum = (from e in res
                                                  where r.DashboardStatusFilter.Split(',').Contains(e.Name)
                                                  select e.TotalAmount).Sum()

                            select new StatusGroupViewModel
                            {
                                StatusGroupId = r.StatusGroupId,
                                Name = r.Name,
                                Count = groupSum > 0 ? (r.Count + groupSum) : r.Count,
                                DashboardSpanCount = r.DashboardSpanCount,
                                DashboardViewColorType = r.DashboardViewColorType,
                                DashboardViewOrderBy = r.DashboardViewOrderBy,
                                DashboardRouteUrl = r.DashboardRouteUrl,
                                DashboardCountSumView = r.DashboardCountSumView,
                                TotalAmount = groupSum > 0 ? (r.TotalAmount + totalAmountSum) : r.TotalAmount,
                                DashboardStatusFilter = groupSum > 0 ? (r.DashboardStatusFilter + ',' + r.Name) : r.Name,
                                DashboardImageUrl = r.DashboardImageUrl
                            }).ToList();

            return response;
        }


        public async Task<DashboardViewModel> GetOrderCountByStatusGroupNew(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            var dashboardViewModel = new DashboardViewModel();

            var paymentDashboardViewModel = new List<StatusGroupViewModel>();
            var pickDashboardViewModel = new List<StatusGroupViewModel>();

            var orderDashboardViewModel = await GetOrderCountByStatusGroup2(orderCountByStatusGroupBodyModel);

            dashboardViewModel.OrderDashboardViewModel = orderDashboardViewModel.OrderBy(x => x.DashboardViewOrderBy).ToList();


            IQueryable<StatusGroupViewModel> results1 = (from co in _sqlServerContext.StatusGroup
                                                         where !co.DashboardStatusGroup.Equals("")
                                                         && co.DashboardStatusGroup.Trim().Equals("পেমেন্ট প্রসেসিং-এ আছে")
                                                         select new StatusGroupViewModel
                                                         {
                                                             StatusGroupId = 0,
                                                             Name = co.DashboardStatusGroup,
                                                             Count = 0,
                                                             DashboardSpanCount = co.DashboardSpanCount,
                                                             DashboardViewColorType = co.DashboardViewColorType,
                                                             DashboardViewOrderBy = co.DashboardViewOrderBy,
                                                             DashboardRouteUrl = co.DashboardRouteUrl,
                                                             DashboardCountSumView = co.DashboardCountSumView,
                                                             TotalAmount = 0,
                                                             DashboardStatusFilter = co.DashboardStatusFilter,
                                                             DashboardImageUrl = co.DashboardImageUrl
                                                         }).Distinct();

            var data1 = await results1.ToListAsync();

            IQueryable<StatusGroupViewModel> statusGroupViewModel = from cos in _sqlServerContext.CourierOrderStatus
                                                        join o in _sqlServerContext.CourierOrders on cos.StatusId equals o.Status
                                                        where o.MerchantId.Equals(orderCountByStatusGroupBodyModel.CourierUserId)
                                                        //&& o.OrderDate.Month.Equals(orderCountByStatusGroupBodyModel.Month)
                                                        //&& o.OrderDate.Year.Equals(orderCountByStatusGroupBodyModel.Year)
                                                        && cos.DashboardStatusGroup.Trim().Equals("পেমেন্ট প্রসেসিং-এ আছে")
                                                        group new { cos, o } by new { cos.DashboardStatusGroup } into cosoGroup
                                                        let firstCosogroup = cosoGroup.FirstOrDefault()
                                                        let courierOrderStatus = firstCosogroup.cos
                                                        let courierOrders = firstCosogroup.o
                                                        let count = cosoGroup.Select(x => x.o.CourierOrdersId).Count()

                                                        let total = cosoGroup.Select(x => x.o.BreakableCharge
                                                        + x.o.DeliveryCharge + x.o.CodCharge + x.o.CollectionCharge
                                                        + x.o.ReturnCharge + x.o.PackagingCharge).Sum()

                                                        let collectionAmount = cosoGroup.Select(a => a.o.CollectionAmount).Sum()

                                                        select new StatusGroupViewModel
                                                        {
                                                            StatusGroupId = 0,
                                                            Name = courierOrderStatus.DashboardStatusGroup,
                                                            Count = count,
                                                            DashboardSpanCount = 1,
                                                            DashboardViewColorType = "",
                                                            DashboardViewOrderBy = 1,
                                                            DashboardRouteUrl = "",
                                                            DashboardCountSumView = "countsum",
                                                            TotalAmount = collectionAmount > 0 ? (collectionAmount - total) : total,
                                                            DashboardStatusFilter = "",
                                                            DashboardImageUrl = ""
                                                        };

            int[] pickStatusArray = { 3,44 };
            pickDashboardViewModel.Add(new StatusGroupViewModel
            {
                StatusGroupId = 0,
                Name = "কালেকশন করা হয়েছে",
                Count = _sqlServerContext.CourierOrderStatusHistory.Where(x => x.PostedOn.Date == DateTime.Today
                && pickStatusArray.Contains(x.Status)
                && x.MerchantId.Equals(orderCountByStatusGroupBodyModel.CourierUserId)).Distinct().Count(),
                DashboardSpanCount = 1,
                DashboardViewColorType = "",
                DashboardViewOrderBy = 1,
                DashboardRouteUrl = "",
                DashboardCountSumView = "",
                TotalAmount = 0,
                DashboardStatusFilter = "",
                DashboardImageUrl = ""
            });

            paymentDashboardViewModel.Add(new StatusGroupViewModel
            {
                StatusGroupId = 0,
                Name = "পেমেন্ট স্টেটমেন্ট\n(পূর্বের সকল পেমেন্ট)",
                Count = 0,
                DashboardSpanCount = 1,
                DashboardViewColorType = "",
                DashboardViewOrderBy = 2,
                DashboardRouteUrl = "",
                DashboardCountSumView = "",
                TotalAmount = 0,
                DashboardStatusFilter = "",
                DashboardImageUrl = ""
            });

            var dataRes = data1.Union(statusGroupViewModel);
            var res = from d in dataRes
                      group d by d.Name into statusGroup

                      let dashboardStatusFilter = statusGroup.Select(s => s.DashboardStatusFilter).FirstOrDefault()
                      let dashboardImageUrl = statusGroup.Select(s => s.DashboardImageUrl).FirstOrDefault()

                      select new StatusGroupViewModel
                      {
                          StatusGroupId = statusGroup.Select(x => x.StatusGroupId).Sum(),
                          Name = statusGroup.Key,
                          Count = statusGroup.Select(x => x.Count).Sum(),
                          DashboardSpanCount = statusGroup.Select(x => x.DashboardSpanCount).Sum(),
                          DashboardViewColorType = statusGroup.Select(s => s.DashboardViewColorType).FirstOrDefault(),
                          DashboardViewOrderBy = statusGroup.Select(x => x.DashboardViewOrderBy).Sum(),
                          DashboardRouteUrl = statusGroup.Select(s => s.DashboardRouteUrl).FirstOrDefault(),
                          DashboardCountSumView = statusGroup.Select(s => s.DashboardCountSumView).FirstOrDefault(),
                          TotalAmount = statusGroup.Select(x => x.TotalAmount).Sum(),
                          DashboardStatusFilter = dashboardStatusFilter,
                          DashboardImageUrl = dashboardImageUrl
                      };

            //var statusGroupViewModel2 = await statusGroupViewModel.ToListAsync();

            //var data = data1.Union(statusGroupViewModel).Union(paymentDashboardViewModel);
            var data = res.Union(paymentDashboardViewModel);
            dashboardViewModel.PickDashboardViewModel = pickDashboardViewModel;
            dashboardViewModel.PaymentDashboardViewModel = data.ToList();
            return dashboardViewModel;
        }

        public async Task<StatusGroup> UpdateSummary(int id, StatusGroupViewModel statusGroup)
        {
            var entity = await _sqlServerContext.StatusGroup.FirstOrDefaultAsync(item => item.StatusGroupId == id);
            if (entity != null)
            {
                if (entity.DashboardSpanCount != statusGroup.DashboardSpanCount)
                {
                    entity.DashboardSpanCount = statusGroup.DashboardSpanCount;
                }
                if (entity.DashboardViewColorType != statusGroup.DashboardViewColorType)
                {
                    entity.DashboardViewColorType = statusGroup.DashboardViewColorType;
                }
                if (entity.DashboardViewOrderBy != statusGroup.DashboardViewOrderBy)
                {
                    entity.DashboardViewOrderBy = statusGroup.DashboardViewOrderBy;
                }

                if (entity.DashboardRouteUrl != statusGroup.DashboardRouteUrl)
                {
                    entity.DashboardRouteUrl = statusGroup.DashboardRouteUrl;
                }
                if (entity.DashboardCountSumView != statusGroup.DashboardCountSumView)
                {
                    entity.DashboardCountSumView = statusGroup.DashboardCountSumView;
                }
                if (entity.DashboardStatusFilter != statusGroup.DashboardStatusFilter)
                {
                    entity.DashboardStatusFilter = statusGroup.DashboardStatusFilter;
                }
                if (entity.DashboardImageUrl != statusGroup.DashboardImageUrl)
                {
                    entity.DashboardImageUrl = statusGroup.DashboardImageUrl;
                }
                


                // Update entity in DbSet
                _sqlServerContext.StatusGroup.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<dynamic> GetCustomerInfoByMobile(string mobile)
        {
            return  await _sqlServerContext.CourierOrders.Where(w => w.Mobile.Equals(mobile)).OrderByDescending(o => o.Id).Select(s => new 
            {

                s.CustomerName,
                s.OtherMobile,
                s.Address,
                s.DistrictId,
                s.ThanaId,
                s.AreaId

            }).FirstOrDefaultAsync();
        }
    }
}
