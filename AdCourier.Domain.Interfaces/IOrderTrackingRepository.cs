using AdCourier.Domain.Entities.BodyModel;
using AdCourier.Domain.Entities.BodyModel.CollectorAssign;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.OrderTracking;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DapperDataModel;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CollectorAssign;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DeliverManAssign;
using AdCourier.Domain.Entities.ViewModel.OrderTracking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IOrderTrackingRepository
    {
        Task<IEnumerable<PickOrderDapperModel>> GetOrderTrackingNew(OrderTrackingBodyModel orderTrackingBodyModel, string flag);
        Task<IEnumerable<OrderTrackingStatusViewModel>> GetOrderTracking(OrderTrackingBodyModel orderTrackingBodyModel, string flag);
        Task<IEnumerable<CourierOrderStatusViewModel>> GetCourierOrderStatus();
        Task<IEnumerable<CourierOrderStatus>> LoadCourierStatus();
        Task<CourierOrderStatus> AddCourierOrderStatus(CourierOrderStatus courierOrderStatus);
        Task<StatusGroup> AddStatusGroup(StatusGroup statusGroup);
        Task<IEnumerable<StatusGroup>> GetStatusGroup();
        Task<IEnumerable<Collectors>> GetAllCollectors();
        Task<IEnumerable<DeliveryUsers>> GetAllDeliveryMan();
        Task<IEnumerable<DeliveryUsers>> GetLocationAssignDeliveryMan();
        Task<StatusGroup> UpdateStatusGroup(int id, StatusGroup statusGroup);
        Task<CourierOrderStatus> UpdateCourierOrderStatus(int id, CourierOrderStatus courierOrderStatus);
        Task<IEnumerable<CourierOrderTrackHistoryViewModel>> OrderUpdateHistory(string courierOrderId);
        Task<List<LocationAssign>> AddMultipleLocationAssign(List<LocationAssign> locationAssign);
        Task<List<LocationAssignHistory>> AddMultipleLocationAssignHistory(List<LocationAssignHistory> locationAssignHistory);
        Task<List<CollectorAssign>> AddMultipleCollectorAssign(List<CollectorAssign> collectorAssign);
        Task<CollectorAssign> AddCollectorAssign(CollectorAssign collectorAssign);
        Task<List<DeliveryBonduAssign>> AddDeliveryBonduAssignMultiple(List<DeliveryBonduAssign> deliveryBonduAssign);
        Task<DeliveryBonduAssign> AddDeliveryManAssign(DeliveryBonduAssign deliveryBonduAssign);
        Task<CollectorAssign> UpdateCollectorAssign(int id, CollectorAssign collectorAssign);
        Task<int> UpdateMultipleCollectorAssignForLocation(MultipleCollectorAssign multipleCollectorAssign);
        Task<CollectorAssign> UpdateCollectorAssignForLocation(int id, CollectorAssign collectorAssign);
        Task<IEnumerable<dynamic>> GetAllCollectorsLocationAssign();
        Task<List<dynamic>> GetAllLocationAssign();
        Task<IEnumerable<CollectorAssignViewModel>> GetAllCollectorsAssign();
        Task<IEnumerable<DeliveryZoneInfo>> GetDeliveryZoneInfo();
        Task<IEnumerable<DeliveryZone>> GetDeliveryZone();
        Task<IEnumerable<DeliveryManAssignViewModel>> GetAllDeliveryMansAssign();
        Task<CourierUsers> UpdateMerchantInformation(int id, CourierUsers courierUserInfo);
        Task<CourierUsers> UpdateViberSMSLimit(int courierUserId, int viberSMSLimit);
        Task<CourierUsers> UpdateCustomerSMSLimit(int courierUserId, int customerSMSLimit);
        Task<CourierUsers> CustomerVoiceSmsLimit(int courierUserId, int customerVoiceSmsLimit);
        Task<int> DeleteCollectorAssign(int id);
        Task<int> DeletePickupLocations(int id);
        Task<CourierUsersViewModel> GetCourierUsersInformation(int courierUserId);
        Task<int> DeleteLocationAssign(int id);
        Task<int> UpdateServiceTypeCourier(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel);
        Task<int> UpdateMerchantServiceTypeCourier(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel);
        Task<int> UpdateAssignmentFalse(RequestBodyModel requestBody);
        Task<int> UpdateDeliveryChargeDetails(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel);
        Task<int> UpdateDeliveryChargeMerchantDetails(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel);
        Task<int> DeleteUserLocationAssign(int userLocationAssignId);
        Task<int> DeleteDeliveryChargeDetails(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel);
        Task<ChangeDeliveryChargeDetailsLog> UpdateChangeDeliveryChargeDetailsLog(List<DeliveryChargeDetails> oldentity, AssignCouirerAndServiceBodyModel newentity);
        Task<DeliveryUsers> UpdateDeliveryUsers(DeliveryUsers request, int userId);
        Task<Vouchers> UpdateVoucher(Vouchers vouchers);
        Task<int> BulkInsertRedxPopData(List<RedxPop> popData);
        Task<int> AddMailContent(PaymentMail request);
    }
}
