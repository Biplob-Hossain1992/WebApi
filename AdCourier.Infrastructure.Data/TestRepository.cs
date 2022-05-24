using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.TestModel;
using AdCourier.Domain.Interfaces;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class TestRepository : ITestRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        public TestRepository(IOptions<ConnectionStringList> connectionStrings, SqlServerContext sqlServerContext)
        {
            _connectionStrings = connectionStrings.Value;
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<JsonModel> GetJsonData()
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();

                return (await connection.QueryFirstOrDefaultAsync<JsonModel>(
                        sql: @"[DT].[USP_Json]",
                        param: null,
                        commandType: CommandType.StoredProcedure));
            }
        }
    }
}
