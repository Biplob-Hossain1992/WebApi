using AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IDeliveryChargeDetailsService
    {
        Task<DeliveryChargeDetails> AddDeliveryChargeDetails(DeliveryChargeDetails deliveryChargeDetails);
        Task<DeliveryChargeDetails> UpdateDeliveryChargeDetails(int id, DeliveryChargeDetails deliveryChargeDetails);
        Task<DeliveryChargeMerchantDetails> UpdateDeliveryChargeMerchantDetails(int id, DeliveryChargeMerchantDetails deliveryChargeMerchantDetails);
        Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> GetDeliveryChargeDetails();
        Task<IEnumerable<SACodChargesViewModel>> GetSACodChargeList();
        Task<IEnumerable<AreaWiseChargeDetailsViewModel>> DeliveryChargeDetailsAreaWise(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel);
        Task<IEnumerable<AreaWiseChargeDetailsViewModel>> DeliveryChargeMerchantDetailsAreaWise(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel);
        Task<IEnumerable<AreaWiseChargeDetailsViewModel>> DeliveryChargeDetailsAreaWise_test(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel);
        Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> DeliveryChargeDetailsSearchWise(DeliveryChargeDetailsSearchModel deliveryChargeDetailsSearch);
    }
}
