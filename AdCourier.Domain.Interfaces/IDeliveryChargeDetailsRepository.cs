using AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IDeliveryChargeDetailsRepository
    {
        Task<DeliveryChargeDetails> AddDeliveryChargeDetails(DeliveryChargeDetails deliveryChargeDetails);
        Task<DeliveryChargeDetails> UpdateDeliveryChargeDetails(int id, DeliveryChargeDetails deliveryChargeDetails);
        Task<DeliveryChargeMerchantDetails> UpdateDeliveryChargeMerchantDetails(int id, DeliveryChargeMerchantDetails deliveryChargeMerchantDetails);
        Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> GetDeliveryChargeDetails();
        Task<IEnumerable<SACodChargesViewModel>> GetSACodChargeList();
        Task<List<GetDeliveryChargeDetailsViewModel>> DeliveryChargeDetailsAreaWise(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel);
        Task<List<GetDeliveryChargeDetailsViewModel>> DeliveryChargeMerchantDetailsAreaWise(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel);
        Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> DeliveryChargeDetailsAreaWise_test(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel);
        Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> DeliveryChargeDetailsSearchWise(DeliveryChargeDetailsSearchModel deliveryChargeDetailsSearch);
    }
}
