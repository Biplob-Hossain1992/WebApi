using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IDistrictInfoRepository
    {
        Task<Districts> AddDistrict(Districts districts);
        Task<Districts> UpdateDistrict(Districts districts);
        Task<Hub> AddHub(Hub hub);
        Task<Hub> UpdateHub(Hub hub);
        Task<int> UpdateTimeSlot(CollectionTimeSlot timeSlot);
        Task<IEnumerable<CourierOrdersViewModel>> GetCutomerListForApp(RequestBodyModel request);
        Task<IEnumerable<CourierOrdersViewModel>> GetCutomerWiseOrdersDetailsForApp(RequestBodyModel request);
        Task<MerchantInfoUpdate> AddMerchantInfoUpdateLog(MerchantInfoUpdate merchantInfoUpdate);
        Task<int> UpdateCourier(Couriers couriers);
        Task<CustomComment> AddCustomComment(CustomComment customComment);
        Task<IEnumerable<dynamic>> GetAllCustomComment(int orderId);
    }
}
