using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IBondhuRepository
    {
        Task<List<SelfDeliveryOrderResponseModel>> LoadOrderForBondhuApp(SelfDeliveryOrderRequestNewModel model);
        Task<List<SelfDeliveryOrderResponseModel>> LoadOrderReturnForBondhuApp(SelfDeliveryOrderRequestNewModel model);
        Task<List<SelfDeliveryOrderResponseModel>> LoadOrderForBondhuAppByTimeSlot(SelfDeliveryOrderRequestNewModel model);
        Task<List<SelfDeliveryOrderResponseModel>> LoadOrderForBondhuAppByTimeSlotNew(SelfDeliveryOrderRequestNewModel model);
        Task<SelfDeliveryModel> DeliveryManRegistration(DeliveryBondhuRegistration bondhuRegistration);
        Task<bool> UpdateDeliveryManInfo(DeliveryManGeneralInfoUpdate infoUpdate);
        Task<IEnumerable<SelfDeliveryLoginResponseModel>> SelfDeliveryLogin(SelfDeliveryLoginModel model);
        Task<IEnumerable<OrderStatusCountView>> GetDeliveryBondhuOrderStatusHistoryCountDeliveryManWise(DeliveryBondhuOrderSearchModel searchModel);
        Task<IEnumerable<DtOrderDetailsDataModel>> GetDtOrderHistoryDetailsReportForDeliveryMan(DeliveryBondhuOrderSearchModel searchModel);
        Task<int> UpdateDocumentUrl(List<CourierOrders> orders);
        Task<int> AddLatLag(LatLagModel model);
        Task<int> GetDeliveryBondhuShowOrderAutomatic();
        Task<IEnumerable<CourierOrders>> GetUpdateTimeSlotAutomatic();
        Task<DeliveryUsers> UserAccess(int bondhuId, bool isNowOffline);
        Task<UserAccessResponseModel> GetBondhuInfo(int bondhuId);
        Task<CourierOrders> GetBondhuAcceptStatus(CourierOrders courierOrders);
        Task<int> UpdateSelfDeliveryUserPassword(SelfDeliveryUserPasswordUpdateModel updateModel);
        Task<dynamic> GetZoneWiseOrdersCount(RequestBodyModel requestBody);
        Task<dynamic> GetZoneWiseOrderDetails(RequestBodyModel requestBody);
        Task<IEnumerable<dynamic>> CollectedNotCollectedMerchantInfo(RequestBodyModel requestBody);
        Task<IEnumerable<dynamic>> DeliveredAndPendingCustomerInfo(RequestBodyModel requestBody);
        Task<dynamic> MerchantWiseOrder(RequestBodyModel requestBody);
        Task<IEnumerable<dynamic>> GetAllLocationAssignHistory(RequestBodyModel requestBody);
        Task<dynamic> GetCustomCommentsWithDateRange(RequestBodyModel requestBody);
        Task<dynamic> GetMerchantWiseRiderCountWithDetails(RequestBodyModel requestBody);
    }
}
