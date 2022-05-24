using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using AdCourier.Domain.Entities.ViewModel.Offer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IWeightRangeService
    {
        Task<List<PhoneBookGroup>> GetMyPhoneBookGroup(int courierUserId);
        Task<DbActionBn> AddDbActionBn(DbActionBn dbActionBn);
        Task<OrderAssignTrack> AddOrderAssignTrack(OrderAssignTrack orderAssignTrack);
        Task<CollectionTimeSlot> AddCollectionTimeSlot(CollectionTimeSlot collectionTimeSlot);
        Task<LocationAssign> UpdateLocationAssign(int id, LocationAssign locationAssign);
        Task<DbActionBn> UpdateDbActionBn(int id, DbActionBn dbActionBn);
        Task<PickupLocations> UpdatePickupLocations(int id, PickupLocations pickupLocations);
        Task<CourierUsers> UpdatePaymentCycle(CourierUsers users);
        Task<PickupLocations> AddPickupLocations(PickupLocations pickupLocations);
        Task<IEnumerable<dynamic>> GetPickupLocations(int courierUserId);
        Task<IEnumerable<dynamic>> GetPickupLocationsWithAcceptedOrderCount(int courierUserId);
        Task<WeightRange> AddWeightRange(WeightRange weightRange);
        Task<IEnumerable<DbActionBn>> GetDbActionBn();
        Task<IEnumerable<WeightRange>> GetWeightRange();
        Task<dynamic> GetReturnOrders(int merchantId, int index, int count);
        bool GetMerchantCredit(int id);
        Task<int> GetMerchantCollectionCharge(int id);
        Task<IEnumerable<Hub>> GetAllHubs();
        Task<WeightRange> UpdateWeightRange(int id, WeightRange weightRange);
        Task<Collectors> UpdateTemporaryCollectors(int id, Collectors collectors);
        Task<DeliveryUsers> UpdateNowOfflineRiders(int id, DeliveryUsers rider);
        Task<DeliveryUsers> UpdateIsPermanentRider(int id, DeliveryUsers rider);
        Task<DeliveryUsers> UpdateRiderTypeOfDeliveryBondhu(int id, DeliveryUsers rider);
        Task<int> UpdateDeliveryBonduAssignMultiple(List<DeliveryBonduAssign> deliveryBonduAssign);

        Task<List<PriceListViewModel>> GetPriceList(int districtId, int deliveryRangeId);
        Task<OfferInformationViewModel> GetOfferInformation(int orderid, string offercode);
        Task<Hub> GetHubsByPickupLocation(PickupLocations pickupLocation);
        Task<dynamic> GetSurveyQuestion();
        Task<dynamic> DanaMatchColumn(string CourierUserId);
        Task<PickupLocations> UpdatePickupLocationsForLatLong(PickupLocations pickupLocations);
        Task<int> UpdatePriceWithWeight(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel);
        Task<List<Category>> GetAssignCourierUserCategory(AssignCourierUserCategory assignCourierUserCategory);
        Task<List<Category>> GetDtCategories(bool isActive);
        Task<List<Category>> GetCategoriesForAdmin(bool isActive);
        Task<List<SubCategory>> GetSubCategories(bool isActive);
        Task<List<SubCategory>> GetSubCategoryById(bool isActive, int categoryId);
        Task<IEnumerable<DeliveredReturnedCountModel>> GetDeliveredReturnedCount(LoadCourierOrderBodyModel bodyModel);
        Task<IEnumerable<DeliveredReturnedDetailsViewModel>> GetDeliveredReturnedCountWiseDetails(RequestBodyModel bodyModel);
        Task<IEnumerable<BondhuAppMismatchDataViewModel>> BondhuAppMismatchData(RequestBodyModel bodyModel);
        Task<DeliveryRange> UpdateImageLink(DeliveryRange deliveryRange);

        Task<List<Districts>> GetServiceDistricts(int deliveryRangeId);
        Task<List<ServiceType>> GetDTServices();
        Task<List<Districts>> GetServiceDistricts(int[] rangeId);
        Task<List<CourierOrdersViewModel>> GetSABookingReport(RequestBodyModel request);
        Task<int> UpdateBulkStatus(List<CourierOrders> request);
        Task<IEnumerable<dynamic>> GetDetailedSAReport(RequestBodyModel request);
        Task<CourierOrdersViewModel> GetAcceptedCourierOrders(int courierUserId);
        Task<bool> SlotChangeSMSandNotification(List<CourierOrders> orders);
        Task<List<CourierOrdersViewModel>> GetCollectionSlotWiseOrders(RequestBodyModel request);
        Task<IEnumerable<dynamic>> GetRiderWiseCollectionReport(RequestBodyModel request);
        Task<IEnumerable<dynamic>> GetPackagedWiseOrders(RequestBodyModel request);
        Task<List<Vouchers>> AddVoucher(List<Vouchers> vouchers);
        Task<IEnumerable<Vouchers>> GetAllVouchers();
        Task<CourierUsers> UpdateTelesalesStatus(int courierUserId, CourierUsersViewModel courierUsers);

        Task<AcquisitionLeadManagement> AddAcquisitionLead(AcquisitionLeadManagement acquisitionLead);
        Task<List<CourierUsersViewModel>> GetDistrictwiseCourierUserInfo(bool isInsideDhaka, string companyName);
        Task<int> UpdateUserProfile(int userId, Users users);
        Task<AdminUsersViewModel> GetUser(int userId);
        Task<List<DeliveryRange>> GetDTDeliveryChargeInfo(RequestBodyModel request);
        Task<SMSPurchase> AddSmsPurchase(SMSPurchase request);
        Task<List<GetPurchasedSMSInfoViewModel>> GetPurchasedSMSInfo(int courierUserId);
        Task<Users> UpdateAdUserSalaryAmount(Users user);
        Task<IEnumerable<dynamic>> GetDatewiseVoucherInfo(RequestBodyModel request);
        Task<int> AddLenderUser(LenderUser lenderUser);
        Task<int> AddUserLocationAssign(UserLocationAssign userLocationAssign);
        Task<List<LenderCourierUserAssignment>> AssignLenderUserToCourierUser(List<LenderCourierUserAssignment> lenderCourierUserAssignments);
        Task<List<LenderCourierUserAssignment>> UnAssignLenderUserToCourierUser(List<LenderCourierUserAssignment> lenderCourierUserAssignments);
        Task<List<LenderUser>> GetLenderUsers();
        Task<IEnumerable<dynamic>> GetLenderWiseAssignedCourierUsers(int lenderUserId);
        Task<int> UpdatePoHOrders(CourierOrders request, string type);
        Task<int> UpdateUserLocationAssign(int userLocationAssignId, UserLocationAssign userLocationAssign);
        Task<dynamic> UpdateReferencewisePayment(PaymentReference request, string paymentFrom);
        Task<dynamic> GetPaymentReferenceReport(RequestBodyModel request);
        Task<List<UserLocationAssignViewModel>> GetUserLocationAssign();
        Task<dynamic> GetPohOrderStatuswise(RequestBodyModel request);
        Task<dynamic> GetPohOrderwiseReport(RequestBodyModel request);
    }
}
