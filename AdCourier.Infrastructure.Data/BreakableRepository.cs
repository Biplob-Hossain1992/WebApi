using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class BreakableRepository : IBreakableRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public BreakableRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<ExtraCharge> AddBreakableCharge(ExtraCharge breakable)
        {
            await _sqlServerContext.ExtraCharge.AddAsync(breakable);
            await _sqlServerContext.SaveChangesAsync();
            return breakable;
        }

        public async Task<ExtraCharge> GetBreakableCharge()
        {
            IQueryable<ExtraCharge> data = from w in _sqlServerContext.ExtraCharge.AsNoTracking()
                                         orderby w.Id descending
                                         select w;
            return await data.FirstOrDefaultAsync();
        }

        public async Task<ExtraCharge> UpdateBreakableCharge(int id, ExtraCharge breakable)
        {
            var entity = await _sqlServerContext.ExtraCharge.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                if (entity.BreakableCharge != breakable.BreakableCharge)
                {
                    entity.BreakableCharge = breakable.BreakableCharge;
                }
                if (entity.CodChargePercentage != breakable.CodChargePercentage)
                {
                    entity.CodChargePercentage = breakable.CodChargePercentage;
                }
                if (entity.CodChargeDhakaPercentage != breakable.CodChargeDhakaPercentage)
                {
                    entity.CodChargeDhakaPercentage = breakable.CodChargeDhakaPercentage;
                }
                if (entity.CodChargeMin != breakable.CodChargeMin)
                {
                    entity.CodChargeMin = breakable.CodChargeMin;
                }
                if (entity.BigProductCharge != breakable.BigProductCharge)
                {
                    entity.BigProductCharge = breakable.BigProductCharge;
                }
                // Update entity in DbSet
                _sqlServerContext.ExtraCharge.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }
    }
}
