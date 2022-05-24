using AdCourier.Domain.Entities.DataModel;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface ISettingsService
    {

        Task<Settings> UpdateRegisterTermsConditions(int id, Settings settings);
        Task<Settings> UpdateSettings(int id, Settings settings);
        Task<Settings> GetSettings();
        Task<dynamic> GetOfferCharge(int merchantId);
        Task<Settings> UpdateVoucherTermsConditions(int id, Settings settings);
    }
}
