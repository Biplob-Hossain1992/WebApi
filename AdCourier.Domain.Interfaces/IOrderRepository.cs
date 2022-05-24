using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.OrderTracking;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ProcedureDataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.CodCollection;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.DeliveryManAssign;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IOrderRepository
    {
        bool GetDistrictInfo(int id);
        Task<IEnumerable<CourierOrderStatusHistory>> UpdateBulkOrders(List<CourierOrderStatusHistoryViewModel> courierOrders);
        Task<int> FixSpecialCharacter(string courierOrdersId);
        Task<List<CourierOrders>> UpdateOrderInformation(List<CourierOrders> courierOrders);
        Task<int> UpdateOrdersBulk(List<CourierOrders> courierOrders);
        Task<IEnumerable<CourierOrderStatusHistory>> AddCourierOrderHistoryBulk(List<CourierOrderStatusHistory> courierOrderStatusHistory);
        Task<IEnumerable<CourierOrders>> AddOrdersBulk(List<CourierOrders> courierOrders);
        Task<CourierOrders> AddOrder(CourierOrders courierOrders);
        Task<DeliveryChargeDetails_test> GetDeliveryChargeDetailsPrice_test(DeliveryChargeDetails_test deliveryChargeDetails);
        Task<DeliveryChargeDetails> GetDeliveryChargeDetailsPrice(DeliveryChargeDetails deliveryChargeDetails);
        Task<dynamic> GetChangeDeliveryChargeDetailsLog(ChangeDeliveryChargeDetailsLog changeLog);
        Task<int> UpdateReferrer(string referrerMobile);
        int UpdateOrderHistory(string courierOrderId, CourierOrderStatusHistoryViewModel courierOrderStatusHistory);
        Task<CourierOrderDetailsViewModel> LoadCourierOrder(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<CourierOrderDetailsViewModel> RetriveOrderList_Admin(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<CourierAmountDetailsResponse> LoadCourierOrderAmountDetails(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<ServiceAmount> LoadCourierOrderAmountDetailsV2(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<CodCollection> GetAllOrders(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<CodCollection> GetCodCollections(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<List<OrderModel>> GetPriorityOrders();
        Task<List<CourierOrderViewModel>> CollectorWiseData(CollectorOrderBodyModel collectorOrderBodyModel);
        Task<List<CourierUsers>> ListOfCourierUser(int collectorId,int groupType);
        Task<int> UpdateCourierOrderCollector(List<CourierOrderStatusHistoryViewModel> updateCourierOrderCollectorBodyModel);
        Task<CourierUsersInfoViewModel> GetCourierUsersInfo(SearchBodyModel searchBody);
        Task<dynamic> GetCourierUserInfo(int courierUserId);
        Task<CourierOrderDetailsViewModel> LoadCourierReport(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<IEnumerable<Districts>> LoadAllDistricts();
        Task<IEnumerable<Districts>> LoadAllDistrictsById(int id);
        Task<CourierUserPickupLocationModel> GetCourierUserListWithPickupLocations(CourierUsersLocationWiseSearchBodyModel merchant);
        Task<IEnumerable<DeliveryUserAcceptedSingleViewModel>> GetAcceptedRiders(PickupLocations pickupLocations);
        Task<DeliveryUsersViewModel> GetRidersOfficeInfo(RequestBodyModel requestBodyModel);
        Task<IEnumerable<CourierUsersViewModel>> GetDuplicatesCourierUsersInfo();

        Task<List<AssignCouirerAndServiceViewModel>> GetAssignCouirerAndService(int districtId);
        Task<List<AssignCouirerAndServiceViewModel>> GetAssignCouirerAndServiceArea(int thanaId);
        Task<List<AssignCouirerAndServiceViewModel>> GetMerchantAssignCouirerAndService(int districtId, int courierUserId);
        Task<List<AssignCouirerAndServiceViewModel>> GetMerchantAssignCouirerAndServiceArea(int thanaId, int courierUserId);
        Task<List<AssignCouirerAndServiceViewModel>> GetMerchantAssignCouirerAndServiceByCourierUserId(int courierUserId);
        Task<CourierOrders> UpdateInvoiceNumber(CourierOrders courierOrders);
        Task<int> UpdateRangeInvoiceNumber(List<CourierOrders> request);
        Task<List<Districts>> LoadAllDistrictsByIds(List<Districts> request);
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
        Task<CourierUsers> UpdateCourierUserBankInformation(int CourierUserId, CourierUsers courierUsers);
        Task<IEnumerable<CouriersViewModel>> GetAllCouriers();
        Task<IEnumerable<dynamic>> SameDayCollectedPendingOrdersCount(RequestBodyModel requestBody);
        Task<IEnumerable<TeleSaleCourierUsers>> GetTeleSaleCourierUsersList(int courierUserId, int teleSales);
    }
}
