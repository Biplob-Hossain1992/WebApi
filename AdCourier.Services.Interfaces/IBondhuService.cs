using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IBondhuService
    {
        Task<SelfDeliveryAllDataResponseModel> LoadOrderForBondhuApp(SelfDeliveryOrderRequestNewModel model);
        Task<SelfDeliveryAllDataResponseModel> LoadOrderForBondhuAppByTimeSlot(SelfDeliveryOrderRequestNewModel model);
        Task<SelfDeliveryAllDataResponseModel> LoadOrderForBondhuAppByTimeSlotNew(SelfDeliveryOrderRequestNewModel model);
        Task<bool> UpdateBondhuOrder(List<CourierOrders> courierOrders);
        Task<SelfDeliveryModel> DeliveryManRegistration(DeliveryBondhuRegistration bondhuRegistration);
        Task<bool> UpdateDeliveryManInfo(DeliveryManGeneralInfoUpdate infoUpdate);
        Task<SelfDeliveryLoginResponseModel> SelfDeliveryLogin(SelfDeliveryLoginModel model);
        Task<IEnumerable<OrderStatusCountDeliveryManWise>> GetDeliveryBondhuOrderStatusHistoryCountDeliveryManWise(DeliveryBondhuOrderSearchModel searchModel);
        Task<IEnumerable<DtOrderDetailsDataModel>> GetDtOrderHistoryDetailsReportForDeliveryMan(DeliveryBondhuOrderSearchModel searchModel);
        Task<int> UpdateDocumentUrl(List<CourierOrders> orders);
        Task<int> AddLatLag(LatLagModel model);
        Task<int> GetDeliveryBondhuShowOrderAutomatic();
        Task<IEnumerable<CourierOrders>> GetUpdateTimeSlotAutomatic();
        Task<DeliveryUsers> UserAccess(int bondhuId, bool isNowOffline);
        Task<UserAccessResponseModel> GetBondhuInfo(int bondhuId);
        Task<bool> GetBondhuAcceptStatus(CourierOrders courierOrders);
        Task<IEnumerable<dynamic>> GetBondhuAcceptStatus_Test(List<CourierOrders> courierOrders);
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
