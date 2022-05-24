using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using Crm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cm.Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public CustomerRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }
        public async Task<Customers> GetCustomerInformation(int customerId)
        {
            IQueryable<Customers> data = from w in _sqlServerContext.Customers.AsNoTracking()
                                           where w.CustomerId.Equals(customerId)
                                           select w;
            return await data.FirstOrDefaultAsync();
        }
    }
}
