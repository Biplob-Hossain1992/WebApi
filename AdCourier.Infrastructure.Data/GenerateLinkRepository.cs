using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.Offer;
using AdCourier.Domain.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class GenerateLinkRepository : IGenerateLinkRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        public GenerateLinkRepository(SqlServerContext sqlServerContext, 
            IOptions<ConnectionStringList> connectionStrings,
            IOrderHistoryRepository orderHistoryRepository)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _connectionStrings = connectionStrings.Value;
            _orderHistoryRepository = orderHistoryRepository;
        }

        public async Task<GenerateLink> AddGenerateLink(GenerateLink generateLink)
        {
            await _sqlServerContext.GenerateLink.AddAsync(generateLink);
            await _sqlServerContext.SaveChangesAsync();
            return generateLink;
        }

        public async Task<IEnumerable<GenerateLink>> GetGenerateLinks()
        {
            IQueryable<GenerateLink> data = from w in _sqlServerContext.GenerateLink.AsNoTracking()
                                           select w;
            return await data.ToListAsync();
        }

        public async Task<GenerateLinkViewModel> GetOffer(string offerId)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@OfferCode", value: offerId, dbType: DbType.String);

                var data = await connection.QueryAsync<GenerateLinkViewModel>(
                        sql: @"[DT].[GetOffer]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.FirstOrDefault();
            }
        }

        public async Task<int> GetOfferByMerchant(int merchantId)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@MerchantId", value: merchantId, dbType: DbType.String);

                var data = await connection.QueryAsync<int>(
                        sql: @"[DT].[GetOfferByMerchant]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.FirstOrDefault();
            }
        }

        public async Task<CourierOrders> UpdateOffer(int id, CourierOrders courierOrders)
        {
            CourierOrderStatusHistory courierOrderStatusHistory = new CourierOrderStatusHistory();

            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                if (courierOrders.OfferCodDiscount > 0)
                {
                    entity.IsOfferCodActive = true;
                    entity.OfferCodDiscount = courierOrders.OfferCodDiscount;
                }
                if (courierOrders.OfferBkashDiscount > 0)
                {
                    entity.IsOfferCodActive = false;
                    entity.OfferBkashDiscount = courierOrders.OfferBkashDiscount;
                }
                entity.IsConfirmedBy = "merchant";
                entity.UpdatedBy = 82;
                entity.Status = 58;
                entity.Comment = "Offer status";
                entity.ClassifiedId = courierOrders.ClassifiedId;
                entity.OfferCode = UniqueCodeGenerator.GetUniqueCode(isCharaterLowerCaseInCouponCode: true, minNumberForRandomNumberGenerator: 10, maxNumberForRandomNumberGenerator: 99);

                // Update entity in DbSet
                _sqlServerContext.CourierOrders.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();

                courierOrderStatusHistory.CourierOrderId = entity.CourierOrdersId;
                courierOrderStatusHistory.IsConfirmedBy = entity.IsConfirmedBy;
                courierOrderStatusHistory.OrderDate = entity.OrderDate;
                courierOrderStatusHistory.Status = entity.Status;
                courierOrderStatusHistory.PostedBy = entity.MerchantId;
                courierOrderStatusHistory.MerchantId = entity.MerchantId;
                courierOrderStatusHistory.Comment = entity.Comment;

                var responseOrderHistory = await _orderHistoryRepository.AddCourierOrderHistory(courierOrderStatusHistory);
            }

            return entity;
        }
    }
}
