using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class OrderRequestRepository : IOrderRequestRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ConnectionStringList _connectionStrings;
        public OrderRequestRepository(SqlServerContext sqlServerContext, IHttpContextAccessor httpContextAccessor,
            IOptions<ConnectionStringList> connectionStrings)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _httpContextAccessor = httpContextAccessor;
            _connectionStrings = connectionStrings.Value;
        }



        public async Task<OrderRequest> AddOrderRequest(OrderRequest requestModel)
        {
            await _sqlServerContext.OrderRequest.AddAsync(requestModel);
            await _sqlServerContext.SaveChangesAsync();
            return requestModel;
        }
        public async Task<Couriers> AddCourier(Couriers couriers)
        {
            await _sqlServerContext.Couriers.AddAsync(couriers);
            await _sqlServerContext.SaveChangesAsync();
            return couriers;
        }
    }
}
