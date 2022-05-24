using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using Crm.Domain.Entities.DapperBodyModel;
using Crm.Domain.Entities.DapperDataModel;
using Crm.Domain.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cm.Infrastructure.Data
{
    public class CrmOrderRepository : ICrmOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionStringList _connectionStrings;
        private readonly SqlServerContext _sqlServerContext;

        public CrmOrderRepository(IConfiguration configuration, 
            IOptions<ConnectionStringList> connectionStrings, SqlServerContext sqlServerContext)
        {
            _configuration = configuration;
            _connectionStrings = connectionStrings.Value;
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<Deals> GetProductInformation(int dealId)
        {
            IQueryable<Deals> data = from w in _sqlServerContext.Deals.AsNoTracking()
                                         where w.DealId.Equals(dealId)
                                         select w;
            return await data.FirstOrDefaultAsync();
        }

        public async Task<CombineCrmOrderDataModel> GetOrders(SearchOrderBodyModel searchOrderBodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: searchOrderBodyModel.FromDate, dbType: DbType.String);
                parameter.Add(name: "@ToDate", value: searchOrderBodyModel.ToDate, dbType: DbType.String);
                parameter.Add(name: "@CouponId", value: searchOrderBodyModel.CouponId, dbType: DbType.String);
                parameter.Add(name: "@StatusId", value: searchOrderBodyModel.StatusId, dbType: DbType.String);
                parameter.Add(name: "@CardType", value: searchOrderBodyModel.CardType, dbType: DbType.String);
                parameter.Add(name: "@Mobile", value: searchOrderBodyModel.Mobile, dbType: DbType.String);
                parameter.Add(name: "@MobileType", value: searchOrderBodyModel.MobileType, dbType: DbType.String);
                parameter.Add(name: "@Index", value: searchOrderBodyModel.Index, dbType: DbType.String);
                parameter.Add(name: "@Count", value: searchOrderBodyModel.Count, dbType: DbType.String);

                var orderModel = new CombineCrmOrderDataModel();
                var multi = await connection.QueryMultipleAsync(sql: @"[Crm].[USP_GetAllOrders]",
                    param: parameter,
                    commandType: CommandType.StoredProcedure);
                {
                    var orderCrmDataModel = multi.Read<OrderCrmDataModel>().ToList();
                    var orderCrmCountDataModel = multi.Read<OrderCrmCountDataModel>().FirstOrDefault();


                    orderModel.OrderCrmDataModel = orderCrmDataModel;
                    orderModel.OrderCrmCountDataModel = orderCrmCountDataModel;


                }

                return orderModel;

            }
        }

        public async Task<IEnumerable<OrderStatusHistoryDataModel>> GetOrderHistoryInformation(string orderId)
        {

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@CouponId", value: orderId, dbType: DbType.Int32);

                return await connection.QueryAsync<OrderStatusHistoryDataModel>(
                        sql: @"[Crm].[USP_GetOrdersHistory]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);
            }
        }
    }
}
