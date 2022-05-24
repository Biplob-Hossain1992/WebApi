using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using Crm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cm.Infrastructure.Data
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public MerchantRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }
        public async Task<UserProfile> GetMerchantInformation(int merchantId)
        {
            IQueryable<UserProfile> data = from w in _sqlServerContext.UserProfile.AsNoTracking()
                                           where w.ProfileID.Equals(merchantId)
                                           select w;
            return await data.FirstOrDefaultAsync();
        }
    }
}
