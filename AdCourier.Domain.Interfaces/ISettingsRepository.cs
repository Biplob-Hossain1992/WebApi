using AdCourier.Domain.Entities.DataModel;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface ISettingsRepository
    {
        Task<Settings> UpdateRegisterTermsConditions(int id, Settings settings);
        Task<Settings> UpdateTermsConditions(int id, Settings settings);
        Task<Settings> GetSettings();
        Task<dynamic> GetOfferCharge(int merchantId);
        Task<Settings> UpdateVoucherTermsConditions(int id, Settings settings);
    }
}
