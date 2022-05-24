using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.Offer;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class GenerateLinkService : IGenerateLinkService
    {
        private readonly IGenerateLinkRepository _generateLinkRepository;
        public GenerateLinkService(IGenerateLinkRepository generateLinkRepository)
        {
            _generateLinkRepository = generateLinkRepository;
        }

        public async Task<GenerateLink> AddGenerateLink(GenerateLink generateLink)
        {
            return await _generateLinkRepository.AddGenerateLink(generateLink);
        }

        public async Task<IEnumerable<GenerateLink>> GetGenerateLinks()
        {
            return await _generateLinkRepository.GetGenerateLinks();
        }

        public async Task<GenerateLinkViewModel> GetOffer(string offerId)
        {
            return await _generateLinkRepository.GetOffer(offerId);
        }
        public async Task<bool> GetOfferByMerchant(int merchantId)
        {
            var data = await _generateLinkRepository.GetOfferByMerchant(merchantId);
            if (data > 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<CourierOrders> UpdateOffer(int id, CourierOrders courierOrders)
        {
            return await _generateLinkRepository.UpdateOffer(id, courierOrders);
        }
    }
}
