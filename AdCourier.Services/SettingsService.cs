using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;
        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<dynamic> GetOfferCharge(int merchantId)
        {
            return await _settingsRepository.GetOfferCharge(merchantId);
        }

        public async Task<Settings> GetSettings()
        {
            return await _settingsRepository.GetSettings();
        }

        public async Task<Settings> UpdateSettings(int id, Settings settings)
        {
            return await _settingsRepository.UpdateTermsConditions(id, settings);
        }
        public async Task<Settings> UpdateRegisterTermsConditions(int id, Settings settings)
        {
            return await _settingsRepository.UpdateRegisterTermsConditions(id, settings);
        }

        public async Task<Settings> UpdateVoucherTermsConditions(int id, Settings settings)
        {
            return await _settingsRepository.UpdateVoucherTermsConditions(id, settings);
        }
    }
}
