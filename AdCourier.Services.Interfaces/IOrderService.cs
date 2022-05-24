using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.OrderTracking;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.CodCollection;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.DeliveryManAssign;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IOrderService
    {
        Task<CourierOrderDetailsViewModel> GetAllOrders(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<CourierOrderDetailsViewModel> GetCodCollections(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<IEnumerable<CourierOrderStatusHistory>> UpdateBulkOrders(List<CourierOrderStatusHistoryViewModel> courierOrders);
        Task<int> FixSpecialCharacter(string id);
        Task<int> UpdateOrderInformation(List<CourierOrders> courierOrders);
        Task<CourierOrders> GetOrderInformation(int orderId);
        Task<IEnumerable<OrderStatusHistoryViewModel>> GetOrderHistoryInformation(string orderId);
        Task<int> UpdateOrdersBulk(List<CourierOrders> courierOrders);
        Task<IEnumerable<CourierOrders>> AddOrdersBulk(List<CourierOrders> courierOrders);
        Task<CourierOrders> AddOrder(CourierOrders courierOrders);
        Task<DeliveryChargeDetails> GetDeliveryChargeDetailsPrice(DeliveryChargeDetails deliveryChargeDetails);
        Task<dynamic> GetChangeDeliveryChargeDetailsLog(ChangeDeliveryChargeDetailsLog changeDeliveryChargeDetailsLog);
        Task<DeliveryChargeDetails_test> GetDeliveryChargeDetailsPrice_test(DeliveryChargeDetails_test deliveryChargeDetails);
        Task<CourierOrderStatusHistory> UpdateOrderHistory(string courierOrderId, CourierOrderStatusHistoryViewModel courierOrderStatusHistory);
        Task<CourierOrderDetailsViewModel> LoadCourierOrder(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<CourierAmountDetailsResponse> LoadCourierOrderAmountDetails(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<CourierAmountDetailsResponse> LoadCourierOrderAmountDetailsV2(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<List<CourierOrderViewModel>> CollectorWiseData(CollectorOrderBodyModel collectorOrderBodyModel);
        Task<List<CourierUsers>> ListOfCourierUser(int collectorId, int groupType);
        Task<List<CourierOrderStatusHistory>> UpdateCourierOrderCollector(List<CourierOrderStatusHistoryViewModel> updateCourierOrderCollectorBodyModel);
        Task<CourierUsersInfoViewModel> GetCourierUsersInfo(SearchBodyModel searchBody);
        Task<dynamic> GetCourierUserInfo(int courierUserId);
        Task<CourierOrderDetailsViewModel> LoadCourierReport(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<IEnumerable<Districts>> LoadAllDistricts();
        Task<IEnumerable<Districts>> LoadAllDistrictsById(int id);
        Task<CourierOrderDetailsViewModel> LoadOrders(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<IEnumerable<CourierUsers>> GetAllCourierUsersList(string companyName);
        Task<IEnumerable<TeleSaleCourierUsers>> GetTeleSaleCourierUsersList(int courierUserId, int teleSales);
        Task<CourierUserPickupLocationModel> GetCourierUserListWithPickupLocations(CourierUsersLocationWiseSearchBodyModel merchant);
        Task<CourierOrderDetailsViewModel> GetCustomerOrders(string mobileNo);
        Task<IEnumerable<DeliveryUserAcceptedViewModel>> GetAcceptedRiders(PickupLocations pickupLocations);
        Task<DeliveryUsersViewModel> GetRidersOfficeInfo(RequestBodyModel requestBodyModel);
        Task<IEnumerable<CourierUsersViewModel>> GetDuplicatesCourierUsersInfo();

        Task<IEnumerable<dynamic>> GetAssignCouirerAndService(int districtId, int thanaId);
        Task<IEnumerable<dynamic>> GetMerchantAssignCouirerAndService(int districtId, int thanaId, int courierUserId);
        Task<CourierOrders> UpdateInvoiceNumber(CourierOrders courierOrders);
        Task<int> UpdateRangeInvoiceNumber(List<CourierOrders> request);
        Task<List<Districts>> LoadAllDistrictsByIds(List<Districts> request);
        Task<IEnumerable<OrderStatusHistoryViewModel>> GetQuickOfficeReceivedDetails(string courierOrdersId, int userId, string hubName);
        Task<int> QuickUpdateStatus(RequestBodyModel requestBody);
        Task<bool> SendPushNotification(int courierUserId, OrderStatusViewModel request);
        Task<IEnumerable<CourierOrderViewModel>> GetQuickUpdateStatusDetails(RequestBodyModel bodyModel);
        Task<DeliveryChargeMerchantDetails> GetDeliveryChargeMerchantDetailsCourier(DeliveryChargeMerchantDetails request);
        Task<List<DeliveryUsersViewModel>> GetLocationWiseRiders(List<LocationAssign> locationAssigns);
        Task<int> UpdateMultipleOrdersWithRider(List<CourierOrders> courierOrders);
        Task<IEnumerable<CourierOrdersViewModel>> GetFirstTimeOrderedMerchantList(RequestBodyModel request);
        Task<List<VouchersViewModel>> GetMerchantAssignedVoucher(List<VouchersViewModel> vouchers);
        Task<int> UpdatePriceWithOrderType(CourierOrders courierOrders);
        Task<dynamic> GetOrderDetails(int orderId);
        Task<int> UpdateServiceType(CourierOrders courierOrders);
        Task<IEnumerable<WeightRangeWiseData>> GetSpecialService(RequestBodyModel request);
        Task<int> UpdateMerchantReview(int CourierUserId, CourierUsers courierUsers);
        Task<IEnumerable<CouriersViewModel>> GetAllCouriers();
        Task<IEnumerable<dynamic>> SameDayCollectedPendingOrdersCount(RequestBodyModel requestBody);
        Task<CourierOrderDetailsViewModel> LoadPOHOrders(LoadCourierOrderBodyModel request);
    }
}
