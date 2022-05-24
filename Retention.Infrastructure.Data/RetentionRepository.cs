using AdCourier.Context;
using Retention.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using System.Data.SqlClient;
using AdCourier.Domain.Entities;
using Microsoft.Extensions.Options;
using Dapper;
using System.Data;
using AdCourier.Domain.Entities.BodyModel.Report;

namespace Retention.Infrastructure.Data
{
    public class RetentionRepository : IRetentionRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        public RetentionRepository(SqlServerContext sqlServerContext, IOptions<ConnectionStringList> connectionStrings)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetRetentionMerchantList(SearchBodyModel searchBodyModel)
        {
            var status = new List<int> { 0, 2, 29 };
            var merchantList = new List<CourierUsersViewModel>();
            IQueryable<CourierUsersViewModel> query;

            if (searchBodyModel.Search == "")
            {
                query = (from _courierUsers in _sqlServerContext.CourierUsers
                         where _courierUsers.RetentionUserId == searchBodyModel.RetentionUserId
                         select new CourierUsersViewModel
                         {
                             CourierUserId = _courierUsers.CourierUserId,
                             CompanyName = _courierUsers.CompanyName,
                             Mobile = _courierUsers.Mobile,
                             AlterMobile = _courierUsers.AlterMobile,
                             Address = _courierUsers.Address,
                             BkashNumber = _courierUsers.BkashNumber
                         }).Skip(searchBodyModel.Index).Take(searchBodyModel.Count);

                merchantList = await (from q in query
                                      join _courierOrders in _sqlServerContext.CourierOrders on q.CourierUserId equals _courierOrders.MerchantId
                                      where !status.Contains(_courierOrders.Status)
                                      orderby _courierOrders.OrderDate descending
                                      select new CourierUsersViewModel
                                      {
                                          CourierUserId = q.CourierUserId,
                                          CompanyName = q.CompanyName,
                                          Mobile = q.Mobile,
                                          AlterMobile = q.AlterMobile,
                                          Address = q.Address,
                                          BkashNumber = q.BkashNumber,
                                          CourierOrders = new CourierOrdersViewModel
                                          {
                                              Id = _courierOrders.Id,
                                              MerchantId = _courierOrders.MerchantId,
                                              OrderDate = _courierOrders.OrderDate
                                          }
                                      }).ToListAsync();

            }
            else
            {
                query = (from _courierUsers in _sqlServerContext.CourierUsers
                         join _pickupLocations in _sqlServerContext.PickupLocations on _courierUsers.CourierUserId equals _pickupLocations.CourierUserId
                         join _district in _sqlServerContext.Districts on _pickupLocations.DistrictId equals _district.DistrictId
                         join _thana in _sqlServerContext.Districts on _pickupLocations.ThanaId equals _thana.DistrictId
                         where _courierUsers.RetentionUserId == searchBodyModel.RetentionUserId
                         && _thana.ParentId == _district.DistrictId
                         && (_courierUsers.CompanyName + _courierUsers.UserName + 
                         _pickupLocations.PickupAddress + _district.District + _thana.District).Contains(searchBodyModel.Search)
                         select new CourierUsersViewModel
                         {
                             CourierUserId = _courierUsers.CourierUserId,
                             CompanyName = _courierUsers.CompanyName,
                             Mobile = _courierUsers.Mobile,
                             AlterMobile = _courierUsers.AlterMobile,
                             Address = _courierUsers.Address,
                             BkashNumber = _courierUsers.BkashNumber
                         }).Skip(searchBodyModel.Index).Take(searchBodyModel.Count);

                merchantList = await (from q in query
                                      join _courierOrders in _sqlServerContext.CourierOrders on q.CourierUserId equals _courierOrders.MerchantId
                                      where !status.Contains(_courierOrders.Status)
                                      orderby _courierOrders.OrderDate descending
                                      select new CourierUsersViewModel
                                      {
                                          CourierUserId = q.CourierUserId,
                                          CompanyName = q.CompanyName,
                                          Mobile = q.Mobile,
                                          AlterMobile = q.AlterMobile,
                                          Address = q.Address,
                                          BkashNumber = q.BkashNumber,
                                          CourierOrders = new CourierOrdersViewModel
                                          {
                                              Id = _courierOrders.Id,
                                              MerchantId = _courierOrders.MerchantId,
                                              OrderDate = _courierOrders.OrderDate
                                          }
                                      }).ToListAsync();
            }

            var retentionMerchantList = from r in merchantList
                                        group r by r.CourierOrders.MerchantId into g
                                        select new CourierUsersViewModel
                                        {
                                            CourierUserId = g.FirstOrDefault().CourierOrders.MerchantId,
                                            CompanyName = g.FirstOrDefault().CompanyName,
                                            Mobile = g.FirstOrDefault().Mobile,
                                            AlterMobile = g.FirstOrDefault().AlterMobile,
                                            Address = g.FirstOrDefault().Address,
                                            BkashNumber = g.FirstOrDefault().BkashNumber,
                                            RetentionMerchantOrder = new Domain.Entities.ViewModel.RetentionMerchantOrderViewModel
                                            {
                                                TotalOrder = g.Count(),
                                                LastOrderDate = g.FirstOrDefault().CourierOrders.OrderDate.Date
                                            }
                                        };

            return retentionMerchantList;

        }

        public async Task<IEnumerable<dynamic>> GetRetentionMerchantListV1(SearchBodyModel searchBodyModel)
        {
            using (var sqlconnection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                int isSearch = 0;
                if (searchBodyModel.Search != "") isSearch = 1;

                var parameters = new DynamicParameters();
                parameters.Add(name: "@retentionUserId", value: searchBodyModel.RetentionUserId, dbType: DbType.Int32);
                parameters.Add(name: "@offset_rows", value: searchBodyModel.Index, dbType: DbType.Int32);
                parameters.Add(name: "@fetch_rows", value: searchBodyModel.Count, dbType: DbType.Int32);
                parameters.Add(name: "@isSearch", value: isSearch, dbType: DbType.Int32);
                parameters.Add(name: "@search_string", value: searchBodyModel.Search, dbType: DbType.String);

                sqlconnection.Open();

                var data = await sqlconnection.QueryAsync<dynamic>(
                    sql: @"[Retention].[GetRetentionMerchantList]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetOrderWiseRetentionMerchantList(RequestBodyModel requestBodyModel)
        {
            var status = new List<int> { 0, 2, 29 };
            //IEnumerable<CourierUsersViewModel> retentionMerchantList;
            IQueryable<CourierUsersViewModel> allOrderedMerchantList;
            var merchantList = new List<CourierUsersViewModel>();
            if (requestBodyModel.Flag != 0)
            {
                allOrderedMerchantList = (from _courierOrders in _sqlServerContext.CourierOrders
                                          join _courierUsers in _sqlServerContext.CourierUsers.Where(u => u.RetentionUserId == requestBodyModel.RetentionUserId)
                                          on _courierOrders.MerchantId equals _courierUsers.CourierUserId
                                          join _pickUpLocations in _sqlServerContext.PickupLocations on _courierUsers.CourierUserId equals _pickUpLocations.CourierUserId
                                          join _district in _sqlServerContext.Districts on _courierUsers.DistrictId equals _district.DistrictId
                                          join _thana in _sqlServerContext.Districts on _courierUsers.ThanaId equals _thana.DistrictId
                                          where _courierOrders.OrderDate >= DateTime.Now.AddDays(-requestBodyModel.Flag)
                                          && _courierOrders.OrderDate <= DateTime.Now
                                          && !status.Contains(_courierOrders.Status)
                                          && _thana.ParentId == _district.DistrictId
                                          && (_courierUsers.CompanyName + _courierUsers.UserName +
                                          _pickUpLocations.PickupAddress + _district.District + _thana.District).Contains(requestBodyModel.Search)
                                          orderby _courierOrders.OrderDate descending
                                          select new CourierUsersViewModel
                                          {
                                              CourierUserId = _courierUsers.CourierUserId,
                                              CompanyName = _courierUsers.CompanyName,
                                              Mobile = _courierUsers.Mobile,
                                              AlterMobile = _courierUsers.AlterMobile,
                                              Address = _courierUsers.Address,
                                              BkashNumber = _courierUsers.BkashNumber,
                                              CourierOrders = new CourierOrdersViewModel
                                              {
                                                  Id = _courierOrders.Id,
                                                  MerchantId = _courierOrders.MerchantId,
                                                  OrderDate = _courierOrders.OrderDate
                                              }
                                          }).GroupBy(g => g.CourierUserId)
                                         .Select(s => new CourierUsersViewModel
                                         {
                                             CourierUserId = s.FirstOrDefault().CourierOrders.MerchantId,
                                             CompanyName = s.FirstOrDefault().CompanyName,
                                             Mobile = s.FirstOrDefault().Mobile,
                                             AlterMobile = s.FirstOrDefault().AlterMobile,
                                             Address = s.FirstOrDefault().Address,
                                             BkashNumber = s.FirstOrDefault().BkashNumber,
                                             RetentionMerchantOrder = new Domain.Entities.ViewModel.RetentionMerchantOrderViewModel
                                             {
                                                 TotalOrder = s.Count(),
                                                 LastOrderDate = s.FirstOrDefault().CourierOrders.OrderDate.Date
                                             }
                                         }).OrderBy(o => o.RetentionMerchantOrder.TotalOrder)
                                            .Skip(requestBodyModel.Index)
                                            .Take(requestBodyModel.Count);

                merchantList = await allOrderedMerchantList.ToListAsync();
            }
            else
            {
                allOrderedMerchantList = (from _courierOrders in _sqlServerContext.CourierOrders
                                          join _courierUsers in _sqlServerContext.CourierUsers.Where(u => u.RetentionUserId == requestBodyModel.RetentionUserId)
                                          on _courierOrders.MerchantId equals _courierUsers.CourierUserId
                                          join _pickUpLocations in _sqlServerContext.PickupLocations on _courierUsers.CourierUserId equals _pickUpLocations.CourierUserId
                                          join _district in _sqlServerContext.Districts on _courierUsers.DistrictId equals _district.DistrictId
                                          join _thana in _sqlServerContext.Districts on _courierUsers.ThanaId equals _thana.DistrictId
                                          where !status.Contains(_courierOrders.Status)
                                          && _thana.ParentId == _district.DistrictId
                                          && (_courierUsers.CompanyName + _courierUsers.UserName +
                                          _pickUpLocations.PickupAddress + _district.District + _thana.District).Contains(requestBodyModel.Search)
                                          orderby _courierOrders.OrderDate descending
                                          select new CourierUsersViewModel
                                          {
                                              CourierUserId = _courierUsers.CourierUserId,
                                              CompanyName = _courierUsers.CompanyName,
                                              Mobile = _courierUsers.Mobile,
                                              AlterMobile = _courierUsers.AlterMobile,
                                              Address = _courierUsers.Address,
                                              BkashNumber = _courierUsers.BkashNumber,
                                              CourierOrders = new CourierOrdersViewModel
                                              {
                                                  Id = _courierOrders.Id,
                                                  MerchantId = _courierOrders.MerchantId,
                                                  OrderDate = _courierOrders.OrderDate
                                              }
                                          }).GroupBy(g => g.CourierUserId)
                                         .Select(s => new CourierUsersViewModel
                                         {
                                             CourierUserId = s.FirstOrDefault().CourierOrders.MerchantId,
                                             CompanyName = s.FirstOrDefault().CompanyName,
                                             Mobile = s.FirstOrDefault().Mobile,
                                             AlterMobile = s.FirstOrDefault().AlterMobile,
                                             Address = s.FirstOrDefault().Address,
                                             BkashNumber = s.FirstOrDefault().BkashNumber,
                                             RetentionMerchantOrder = new Domain.Entities.ViewModel.RetentionMerchantOrderViewModel
                                             {
                                                 TotalOrder = s.Count(),
                                                 LastOrderDate = s.FirstOrDefault().CourierOrders.OrderDate.Date
                                             }
                                         }).OrderByDescending(o => o.RetentionMerchantOrder.TotalOrder)
                                            .Skip(requestBodyModel.Index)
                                            .Take(requestBodyModel.Count);

                merchantList = await allOrderedMerchantList.ToListAsync();
            }

            return merchantList;
        }

        public async Task<IEnumerable<dynamic>> GetOrderedRetentionMerchantList(RequestBodyModel requestBodyModel)
        {
            int isSearch = 0;
            if (requestBodyModel.Search != "") isSearch = 1;

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                parameters.Add(name: "@flag", value: requestBodyModel.Flag, dbType: DbType.Int32);
                parameters.Add(name: "@search_string", value: requestBodyModel.Search, dbType: DbType.String);
                parameters.Add(name: "@offset_rows", value: requestBodyModel.Index, dbType: DbType.Int32);
                parameters.Add(name: "@fetch_rows", value: requestBodyModel.Count, dbType: DbType.Int32);
                parameters.Add(name: "@isSearch", value: isSearch, dbType: DbType.Int32);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Retention].[GetOrderedRetentionMerchantList]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<MerchantVisited> AddVisitedMerchant(MerchantVisited merchantVisited)
        {
            await _sqlServerContext.MerchantVisited.AddAsync(merchantVisited);
            await _sqlServerContext.SaveChangesAsync();
            return merchantVisited;
        }

        public async Task<MerchantCalled> AddCalledMerchant(MerchantCalled merchantCalled)
        {
            await _sqlServerContext.MerchantCalled.AddAsync(merchantCalled);
            await _sqlServerContext.SaveChangesAsync();
            return merchantCalled;
        }

        public async Task<dynamic> NewRetentionMerchantFollowUpReport(RequestBodyModel requestBodyModel)
        {
            DateTime backFromDate = requestBodyModel.FromDate.AddDays(-requestBodyModel.Index);
            DateTime backToDate = requestBodyModel.FromDate.AddDays(-1);
            DateTime futureFromDate = requestBodyModel.FromDate.AddDays(1);
            DateTime futureToDate = requestBodyModel.FromDate.AddDays(requestBodyModel.Count);

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@tempFromDate", value: backFromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@tempToDate", value: backToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@futureFromDate", value: futureFromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@futureToDate", value: futureToDate, dbType: DbType.DateTime);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[NewRetentionMerchantFollowUpReport]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<dynamic> NewRetentionMerchantFollowUpReportDetails(RequestBodyModel requestBodyModel)
        {
            using (var conneciton = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                var parameters = new DynamicParameters();

                if (requestBodyModel.DateFlag == 1)
                {
                    parameters.Add(name: "@detailsColumnNo", value: requestBodyModel.DateFlag, dbType: DbType.Int32);
                    parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                }
                else if (requestBodyModel.DateFlag == 2)
                {
                    parameters.Add(name: "@detailsColumnNo", value: requestBodyModel.DateFlag, dbType: DbType.Int32);
                    parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                }
                else if (requestBodyModel.DateFlag == 3)
                {
                    parameters.Add(name: "@detailsColumnNo", value: requestBodyModel.DateFlag, dbType: DbType.Int32);
                    parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                }
                else if (requestBodyModel.DateFlag == 4)
                {
                    DateTime backFromDate = requestBodyModel.FromDate.AddDays(-requestBodyModel.Index);
                    DateTime backToDate = requestBodyModel.FromDate.AddDays(-1);
                    parameters.Add(name: "@detailsColumnNo", value: requestBodyModel.DateFlag, dbType: DbType.Int32);
                    parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                    parameters.Add(name: "@tempFromDate", backFromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@tempToDate", backToDate, dbType: DbType.DateTime);
                }
                else if (requestBodyModel.DateFlag == 5)
                {
                    DateTime backFromDate = requestBodyModel.FromDate.AddDays(-requestBodyModel.Index);
                    DateTime backToDate = requestBodyModel.FromDate.AddDays(-1);
                    DateTime futureFromDate = requestBodyModel.FromDate.AddDays(1);
                    DateTime futureToDate = requestBodyModel.FromDate.AddDays(requestBodyModel.Count);
                    parameters.Add(name: "@detailsColumnNo", value: requestBodyModel.DateFlag, dbType: DbType.Int32);
                    parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                    parameters.Add(name: "@tempFromDate", backFromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@tempToDate", backToDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@futureFromDate", futureFromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@futureToDate", futureToDate, dbType: DbType.DateTime);
                }

                conneciton.Open();
                var data = await conneciton.QueryAsync<dynamic>(
                    sql: @"[Reports].[NewRetentionMerchantFollowUpDetails]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> SrWiseRetentionMerchantFollowUp(OrderBodyModel orderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@dateRange", value: orderBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@retentionUserId", value: orderBodyModel.RetentionUserId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Retention].[SrWiseRetentionMerchantFollowUp]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> SrWiseRetentionMerchantFollowUpDetails(RequestBodyModel requestBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();

                if (requestBodyModel.DateFlag == 1)
                {
                    parameters.Add(name: "@detailsColumnNo", value: requestBodyModel.DateFlag, dbType: DbType.Int32);
                    parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                }
                else if (requestBodyModel.DateFlag == 2)
                {
                    parameters.Add(name: "@detailsColumnNo", value: requestBodyModel.DateFlag, dbType: DbType.Int32);
                    parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                }
                else if (requestBodyModel.DateFlag == 3)
                {
                    parameters.Add(name: "@detailsColumnNo", value: requestBodyModel.DateFlag, dbType: DbType.Int32);
                    parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                    parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);
                }

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[SrWiseRetentionMerchantFollowUpDetails]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> MonthWiseUniqueOrderdMerchantList(RequestBodyModel requestBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: requestBodyModel.ToDate, dbType: DbType.DateTime);
                parameters.Add(name: "@retentionUserId", value: requestBodyModel.RetentionUserId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[MonthWiseUniqueOrderedMerchants]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> FullMonthUniqueOrderedMerchantList(RequestBodyModel requestBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@fromDate", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@toDate", value: requestBodyModel.ToDate, dbType: DbType.DateTime);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[FullMonthUniqueOrderedMerchantList]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );
                return data.ToList();
            }
        }

        public async Task<IEnumerable<dynamic>> SrWiseRegularOrderedMerchantList(RequestBodyModel requestBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add(name: "@daterange", value: requestBodyModel.FromDate, dbType: DbType.DateTime);
                parameters.Add(name: "@retentionUserList", value: requestBodyModel.RetentionUsers, dbType: DbType.String);

                var data = await connection.QueryAsync<dynamic>(
                    sql: @"[Reports].[RegularOrderedMerchantList]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                    );

                return data.ToList();
            }
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetTelesalesActiveMerchantList(RequestBodyModel requestBodyModel)
        {
            var merchantList = (from _users in _sqlServerContext.CourierUsers
                                where _users.TeleSalesDate.Value.Date == requestBodyModel.RequestDate
                                && _users.TeleSales == requestBodyModel.Flag
                                && _users.RetentionUserId == requestBodyModel.RetentionUserId
                                select new CourierUsersViewModel
                                {
                                    CompanyName = _users.CompanyName,
                                    TeleSalesDate = _users.TeleSalesDate,
                                    Mobile = _users.Mobile,
                                    AlterMobile = _users.AlterMobile,
                                    BkashNumber = _users.BkashNumber,
                                    JoinDate = _users.JoinDate,
                                    Address = _users.Address
                                }).Skip(requestBodyModel.Index).Take(requestBodyModel.Count);

            return await merchantList.ToListAsync();
        }

        public async Task<CourierUsersInfoViewModel> GetSrWiseCourierUsersInfo(SearchBodyModel searchBodyModel)
        {
            int totalUserCount = 0;
            var courierUsers = new List<CourierUsers>();

            totalUserCount = await _sqlServerContext.CourierUsers.Where(u => u.RetentionUserId == searchBodyModel.RetentionUserId).CountAsync();

            if (searchBodyModel.Search == "")
            {
                courierUsers = await _sqlServerContext.CourierUsers.Where(u => u.RetentionUserId == searchBodyModel.RetentionUserId)
                                    .OrderBy(o => o.CourierUserId).Skip(searchBodyModel.Index).Take(searchBodyModel.Count)
                                    .ToListAsync();
            }
            else
            {
                courierUsers = await _sqlServerContext.CourierUsers.Where(u => u.RetentionUserId == searchBodyModel.RetentionUserId
                                     && (u.UserName + u.Mobile + u.CompanyName).Contains(searchBodyModel.Search))
                                     .OrderBy(o => o.CourierUserId).Skip(searchBodyModel.Index)
                                     .Take(searchBodyModel.Count).ToListAsync();
            }

            var result = new CourierUsersInfoViewModel
            {
                TotalUsers = totalUserCount,
                CourierUsers = courierUsers
            };
            return result;
        }
    }
}
