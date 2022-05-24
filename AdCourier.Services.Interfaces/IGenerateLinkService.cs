using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.Offer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IGenerateLinkService
    {
        Task<GenerateLink> AddGenerateLink(GenerateLink generateLink);
        Task<IEnumerable<GenerateLink>> GetGenerateLinks();
        Task<GenerateLinkViewModel> GetOffer(string offerId);
        Task<bool> GetOfferByMerchant(int merchantId);
        Task<CourierOrders> UpdateOffer(int id, CourierOrders courierOrders);
    }
}
