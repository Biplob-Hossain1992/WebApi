using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public SettingsRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<dynamic> GetOfferCharge(int merchantId)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == merchantId);

            if (!entity.ReferrerIsActive)
            {
                if (entity.RefereeOrder > 0 && DateTime.Now >= entity.RefereeStartTime && DateTime.Now <= entity.RefereeEndTime)
                {
                    return new
                    {
                        IsDeliveryCharge = true,
                        RelationType = "referee"
                    };
                    
                }
            }
            else
            {
                if (entity.ReferrerOrder > 0 && DateTime.Now >= entity.ReferrerStartTime && DateTime.Now <= entity.ReferrerEndTime)
                {
                    return new
                    {
                        IsDeliveryCharge = true,
                        RelationType = "referrer"
                    };
                }
            }

            return new
            {
                IsDeliveryCharge = false,
                RelationType = ""
            };
        }

        public async Task<Settings> GetSettings()
        {
            return await _sqlServerContext.Settings.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Settings> UpdateTermsConditions(int id, Settings settings)
        {
            var entity = await _sqlServerContext.Settings.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.TermsConditions = settings.TermsConditions;
                // Update entity in DbSet
                _sqlServerContext.Settings.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<Settings> UpdateRegisterTermsConditions(int id, Settings settings)
        {
            var entity = await _sqlServerContext.Settings.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.RegisterTermsConditions = settings.RegisterTermsConditions;
                // Update entity in DbSet
                _sqlServerContext.Settings.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<Settings> UpdateVoucherTermsConditions(int id, Settings settings)
        {
            var entity = await _sqlServerContext.Settings.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.VoucherTermsConditions = settings.VoucherTermsConditions;
                // Update entity in DbSet
                _sqlServerContext.Settings.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }
    }
}
