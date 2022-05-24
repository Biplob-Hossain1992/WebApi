using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class DanaRepository : IDanaRepository
    {

        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        public DanaRepository(IOptions<ConnectionStringList> connectionStrings, SqlServerContext sqlServerContext)
        {
            _connectionStrings = connectionStrings.Value;
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }


        public async Task<JsonData> AddDanaJsonData(JsonBodyModel jsonBodyModel)
        {

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@json", value: jsonBodyModel.Json, dbType: DbType.String);
                parameter.Add(name: "@endpoint", value: jsonBodyModel.Endpoint, dbType: DbType.String);
                parameter.Add(name: "@courierUserId", value: jsonBodyModel.CourierUserId, dbType: DbType.Int32);

                var data =  await connection.QueryFirstOrDefaultAsync<JsonData>(
                        sql: @"[DT].[USP_AddDanaJsonData]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data;

                //var data =  await connection.ExecuteAsync(
                //        sql: @"[DT].[USP_AddDanaJsonData]",
                //        param: parameter,
                //        commandType: CommandType.StoredProcedure);

                //JsonData sd = new JsonData();


                //return sd;
            }
        }

        public async Task<PohScore> AddPohScore(PohScore pohScoreModel)
        {
            _sqlServerContext.PohScore.Add(pohScoreModel);
            await _sqlServerContext.SaveChangesAsync();
            return pohScoreModel;
        }
    }
}
