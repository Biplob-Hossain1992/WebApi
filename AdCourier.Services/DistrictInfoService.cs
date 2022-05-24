using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class DistrictInfoService: IDistrictInfoService
    {
        private readonly IDistrictInfoRepository _districtInfoRepository;

        public DistrictInfoService(IDistrictInfoRepository districtInfoRepository)
        {
            _districtInfoRepository = districtInfoRepository;
        }

        public async Task<Districts> AddDistrict(Districts districts)
        {
            return await _districtInfoRepository.AddDistrict(districts);
        }
        public async Task<Districts> UpdateDistrict(Districts districts)
        {
            return await _districtInfoRepository.UpdateDistrict(districts);
        }
        public async Task<Hub> AddHub(Hub hub)
        {
            return await _districtInfoRepository.AddHub(hub);
        }
        public async Task<Hub> UpdateHub(Hub hub)
        {
            return await _districtInfoRepository.UpdateHub(hub);
        }
        public async Task<int> UpdateTimeSlot(CollectionTimeSlot timeSlot)
        {
            return await _districtInfoRepository.UpdateTimeSlot(timeSlot);
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetCutomerListForApp(RequestBodyModel request)
        {
            return await _districtInfoRepository.GetCutomerListForApp(request);
        }
        public async Task<IEnumerable<CourierOrdersViewModel>> GetCutomerWiseOrdersDetailsForApp(RequestBodyModel request)
        {
            return await _districtInfoRepository.GetCutomerWiseOrdersDetailsForApp(request);
        }

        public async Task<MerchantInfoUpdate> AddMerchantInfoUpdateLog(MerchantInfoUpdate merchantInfoUpdate)
        {
            return await _districtInfoRepository.AddMerchantInfoUpdateLog(merchantInfoUpdate);
        }

        public async Task<int> UpdateCourier(Couriers couriers)
        {
            return await _districtInfoRepository.UpdateCourier(couriers);
        }

        public async Task<CustomComment> AddCustomComment(CustomComment customComment)
        {
            return await _districtInfoRepository.AddCustomComment(customComment);
        }

        public async Task<IEnumerable<dynamic>> GetAllCustomComment(int orderId)
        {
            return await _districtInfoRepository.GetAllCustomComment(orderId);
        }
    }
}
