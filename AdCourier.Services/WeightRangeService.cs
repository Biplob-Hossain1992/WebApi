using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using AdCourier.Domain.Entities.ViewModel.Offer;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using System.Net;

using AdCourier.Domain.Entities;
using Microsoft.Extensions.Options;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;

namespace AdCourier.Services
{
    public class WeightRangeService : IWeightRangeService
    {
        private readonly RedisModel _redisModel;
        private readonly IWeightRangeRepository _weightRangeRepository;
        private readonly IRedisCacheClient _redis;
        public WeightRangeService(IOptions<RedisModel> redisModel, IWeightRangeRepository weightRangeRepository, IRedisCacheClient redis)
        {
            _weightRangeRepository = weightRangeRepository;
            _redis = redis;
            _redisModel = redisModel.Value;
        }
        public async Task<DbActionBn> AddDbActionBn(DbActionBn dbActionBn)
        {
            return await _weightRangeRepository.AddDbActionBn(dbActionBn);
        }

        public async Task<OrderAssignTrack> AddOrderAssignTrack(OrderAssignTrack orderAssignTrack)
        {
            return await _weightRangeRepository.AddOrderAssignTrack(orderAssignTrack);
        }
        public async Task<CollectionTimeSlot> AddCollectionTimeSlot(CollectionTimeSlot collectionTimeSlot)
        {
            return await _weightRangeRepository.AddCollectionTimeSlot(collectionTimeSlot);
        }
        public async Task<PickupLocations> AddPickupLocations(PickupLocations pickupLocations)
        {
            return await _weightRangeRepository.AddPickupLocations(pickupLocations);
        }
        public async Task<WeightRange> AddWeightRange(WeightRange weightRange)
        {
            return await _weightRangeRepository.AddWeightRange(weightRange);
        }

        public async Task<IEnumerable<dynamic>> GetPickupLocations(int courierUserId)
        {
            return await _weightRangeRepository.GetPickupLocations(courierUserId);
        }
        public async Task<IEnumerable<dynamic>> GetPickupLocationsWithAcceptedOrderCount(int courierUserId)
        {
            return await _weightRangeRepository.GetPickupLocationsWithAcceptedOrderCount(courierUserId);
        }
        public async Task<List<PriceListViewModel>> GetPriceList(int districtId, int deliveryRangeId)
        {
            return await _weightRangeRepository.GetPriceList(districtId, deliveryRangeId);
        }
        public async Task<OfferInformationViewModel> GetOfferInformation(int orderid, string offercode)
        {
            return await _weightRangeRepository.GetOfferInformation(orderid, offercode);
        }
        public async Task<IEnumerable<Hub>> GetAllHubs()
        {
            if (_redisModel.IsActive)
            {
                string key = "GetAllHubs";

                if (await _redis.Db1.Database.KeyExistsAsync(key) == true)
                {
                    return await _redis.Db1.GetAsync<IEnumerable<Hub>>(key);
                }
                else
                {
                    var data = await _weightRangeRepository.GetAllHubs();
                    bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                    return data;
                }
            }
            else
            {
                return await _weightRangeRepository.GetAllHubs();
            }

        }
        public async Task<IEnumerable<DbActionBn>> GetDbActionBn()
        {
            return await _weightRangeRepository.GetDbActionBn();
        }
        public bool GetMerchantCredit(int id)
        {
            return _weightRangeRepository.GetMerchantCredit(id);
        }

        public async Task<int> GetMerchantCollectionCharge(int id)
        {

            if (_redisModel.IsActive)
            {
                string key = "GetMerchantCollectionCharge" + id.ToString();

                if (_redis.Db1.Database.KeyExists(key) == true)
                {
                    return await _redis.Db1.GetAsync<int>(key);
                }
                else
                {
                    var data = await _weightRangeRepository.GetMerchantCollectionCharge(id);
                    bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                    return data;
                }
            }
            else
            {
                return await _weightRangeRepository.GetMerchantCollectionCharge(id);
            }
        }


        public async Task<dynamic> GetReturnOrders(int merchantId, int index, int count)
        {

            var data = await _weightRangeRepository.GetReturnOrders(merchantId, index, count);
            return data;

        }

        public async Task<dynamic> GetSurveyQuestion()
        {
            var data = await _weightRangeRepository.GetSurveyQuestion();
            return data;
        }

        public async Task<dynamic> DanaMatchColumn(string CourierUserId)
        {
            return await _weightRangeRepository.DanaMatchColumn(CourierUserId);
        }

        public async Task<IEnumerable<WeightRange>> GetWeightRange()
        {

            if (_redisModel.IsActive)
            {
                string key = "GetWeightRange";

                if (await _redis.Db1.Database.KeyExistsAsync(key) == true)
                {
                    return await _redis.Db1.GetAsync<IEnumerable<WeightRange>>(key);
                }
                else
                {
                    var data = await _weightRangeRepository.GetWeightRange();
                    bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                    return data;
                }
            }
            else
            {
                return await _weightRangeRepository.GetWeightRange();
            }
        }
        public async Task<DbActionBn> UpdateDbActionBn(int id, DbActionBn dbActionBn)
        {
            return await _weightRangeRepository.UpdateDbActionBn(id, dbActionBn);
        }
        public async Task<LocationAssign> UpdateLocationAssign(int id, LocationAssign locationAssign)
        {
            return await _weightRangeRepository.UpdateLocationAssign(id, locationAssign);
        }
        public async Task<PickupLocations> UpdatePickupLocations(int id, PickupLocations pickupLocations)
        {
            return await _weightRangeRepository.UpdatePickupLocations(id, pickupLocations);
        }

        public async Task<CourierUsers> UpdatePaymentCycle(CourierUsers users)
        {
            return await _weightRangeRepository.UpdatePaymentCycle(users);
        }

        public async Task<DeliveryRange> UpdateImageLink(DeliveryRange deliveryRange)
        {
            return await _weightRangeRepository.UpdateImageLink(deliveryRange);
        }

        public async Task<WeightRange> UpdateWeightRange(int id, WeightRange weightRange)
        {
            return await _weightRangeRepository.UpdateWeightRange(id, weightRange);
        }
        public async Task<Collectors> UpdateTemporaryCollectors(int id, Collectors collectors)
        {
            return await _weightRangeRepository.UpdateTemporaryCollectors(id, collectors);
        }
        public async Task<DeliveryUsers> UpdateNowOfflineRiders(int id, DeliveryUsers rider)
        {
            return await _weightRangeRepository.UpdateNowOfflineRiders(id, rider);
        }
        public async Task<DeliveryUsers> UpdateIsPermanentRider(int id, DeliveryUsers rider)
        {
            return await _weightRangeRepository.UpdateIsPermanentRider(id, rider);
        }
        public async Task<DeliveryUsers> UpdateRiderTypeOfDeliveryBondhu(int id, DeliveryUsers rider)
        {
            return await _weightRangeRepository.UpdateRiderTypeOfDeliveryBondhu(id, rider);
        }
        public async Task<int> UpdateDeliveryBonduAssignMultiple(List<DeliveryBonduAssign> deliveryBonduAssign)
        {
            return await _weightRangeRepository.UpdateDeliveryBonduAssignMultiple(deliveryBonduAssign);
        }

        public async Task<Hub> GetHubsByPickupLocation(PickupLocations pickupLocation)
        {
            return await _weightRangeRepository.GetHubsByPickupLocation(pickupLocation);
        }

        public async Task<PickupLocations> UpdatePickupLocationsForLatLong(PickupLocations pickupLocations)
        {
            return await _weightRangeRepository.UpdatePickupLocationsForLatLong(pickupLocations);
        }
        public async Task<int> UpdatePriceWithWeight(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            return await _weightRangeRepository.UpdatePriceWithWeight(deliveryChargeDetailsBodyModel);
        }

        public async Task<List<Category>> GetAssignCourierUserCategory(AssignCourierUserCategory assignCourierUserCategory)
        {
            return await _weightRangeRepository.GetAssignCourierUserCategory(assignCourierUserCategory);
        }

        public async Task<List<Category>> GetDtCategories(bool isActive)
        {
            return await _weightRangeRepository.GetDtCategories(isActive);
        }

        public async Task<List<Category>> GetCategoriesForAdmin(bool isActive)
        {
            return await _weightRangeRepository.GetCategoriesForAdmin(isActive);
        }
        public async Task<List<SubCategory>> GetSubCategories(bool isActive)
        {
            return await _weightRangeRepository.GetSubCategories(isActive);
        }

        public async Task<List<SubCategory>> GetSubCategoryById(bool isActive, int categoryId)
        {
            return await _weightRangeRepository.GetSubCategoryById(isActive, categoryId);
        }
        public async Task<List<Districts>> GetServiceDistricts(int deliveryRangeId)
        {
            return await _weightRangeRepository.GetServiceDistricts(deliveryRangeId);
        }

        public async Task<List<Districts>> GetServiceDistricts(int[] rangeId)
        {
            return await _weightRangeRepository.GetServiceDistricts(rangeId);
        }

        public async Task<List<CourierOrdersViewModel>> GetSABookingReport(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetSABookingReport(request);
        }

        public async Task<List<ServiceType>> GetDTServices()
        {
            var services = new List<ServiceType>()
            {
                new ServiceType()
                {
                    ServiceId = 1,
                    ServiceTypeName = "<font color='#00844A'>নেক্সট ডে</font>",
                    ServiceInfo = "<font color='#ed1c24'>২৪ ঘন্টায়</font><font color='#000000'> (৯০ টাকা)</font><font color='#ed1c24'> ঢাকা </font><font color='#000000'>(৩৫ টাকা)</font>",
                    DeliveryRangeId = new int[] { 14, 17 },
                    ServiceTypeShow = new string[] { "nextday" }
                },
                new ServiceType()
                {
                    ServiceId = 2,
                    ServiceTypeName = "<font color='#00844A'>সদর এক্সপ্রেস</font>",
                    ServiceInfo = "<font color='#ed1c24'>৪৮ ঘন্টায়</font><font color='#000000'> (৮০ টাকা)</font>",
                    DeliveryRangeId = new int[] { 18 }
                },
                new ServiceType()
                {
                    ServiceId = 3,
                    ServiceTypeName = "<font color='#00844A'>রেগুলার</font>",
                    ServiceInfo = "<font color='#ed1c24'>৩-৫ দিনে</font><font color='#000000'> (৭০ টাকা)</font>",
                    DeliveryRangeId = new int[]{ },
                    ServiceTypeShow = new string[] { "nextday" }
                },
            };

            return await Task.FromResult(services);

        }

        public async Task<IEnumerable<DeliveredReturnedCountModel>> GetDeliveredReturnedCount(LoadCourierOrderBodyModel bodyModel)
        {
            return await _weightRangeRepository.GetDeliveredReturnedCount(bodyModel);
        }

        public async Task<IEnumerable<DeliveredReturnedDetailsViewModel>> GetDeliveredReturnedCountWiseDetails(RequestBodyModel bodyModel)
        {
            return await _weightRangeRepository.GetDeliveredReturnedCountWiseDetails(bodyModel);
        }
        public async Task<IEnumerable<BondhuAppMismatchDataViewModel>> BondhuAppMismatchData(RequestBodyModel bodyModel)
        {
            return await _weightRangeRepository.BondhuAppMismatchData(bodyModel);
        }

        public async Task<int> UpdateBulkStatus(List<CourierOrders> request)
        {
            return await _weightRangeRepository.UpdateBulkStatus(request);
        }

        public async Task<IEnumerable<dynamic>> GetDetailedSAReport(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetDetailedSAReport(request);
        }

        public async Task<CourierOrdersViewModel> GetAcceptedCourierOrders(int courierUserId)
        {
            return await _weightRangeRepository.GetAcceptedCourierOrders(courierUserId);
        }

        public async Task<bool> SlotChangeSMSandNotification(List<CourierOrders> orders)
        {
            return await _weightRangeRepository.SlotChangeSMSandNotification(orders);
        }

        public async Task<List<CourierOrdersViewModel>> GetCollectionSlotWiseOrders(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetCollectionSlotWiseOrders(request);
        }

        public async Task<IEnumerable<dynamic>> GetRiderWiseCollectionReport(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetRiderWiseCollectionReport(request);
        }

        public async Task<IEnumerable<dynamic>> GetPackagedWiseOrders(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetPackagedWiseOrders(request);
        }



        public async Task<List<Vouchers>> AddVoucher(List<Vouchers> vouchers)
        {
            return await _weightRangeRepository.AddVoucher(vouchers);
        }

        public async Task<IEnumerable<Vouchers>> GetAllVouchers()
        {
            return await _weightRangeRepository.GetAllVouchers();
        }

        public async Task<List<PhoneBookGroup>> GetMyPhoneBookGroup(int courierUserId)
        {
            return await _weightRangeRepository.GetMyPhoneBookGroup(courierUserId);
        }
        public async Task<CourierUsers> UpdateTelesalesStatus(int courierUserId, CourierUsersViewModel courierUsers)
        {
            return await _weightRangeRepository.UpdateTelesalesStatus(courierUserId, courierUsers);
        }

        public async Task<AcquisitionLeadManagement> AddAcquisitionLead(AcquisitionLeadManagement acquisitionLead)
        {
            return await _weightRangeRepository.AddAcquisitionLead(acquisitionLead);
        }

        public async Task<List<CourierUsersViewModel>> GetDistrictwiseCourierUserInfo(bool isInsideDhaka, string companyName)
        {
            if( _redisModel.IsActive)
            {
                string key = companyName;

                if (await _redis.Db1.Database.KeyExistsAsync(key) == true)
                {
                    return await _redis.Db1.GetAsync<List<CourierUsersViewModel>>(key);
                }
                else
                {
                    var data = await _weightRangeRepository.GetDistrictwiseCourierUserInfo(isInsideDhaka, companyName);
                    bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                    return data;
                }
            }
            else
            {
                return await _weightRangeRepository.GetDistrictwiseCourierUserInfo(isInsideDhaka, companyName);
            }

        }

        public async Task<int> UpdateUserProfile(int userId, Users users)
        {
            return await _weightRangeRepository.UpdateUserProfile(userId, users);
        }

        public async Task<AdminUsersViewModel> GetUser(int userId)
        {
            return await _weightRangeRepository.GetUser(userId);
        }

        public async Task<List<DeliveryRange>> GetDTDeliveryChargeInfo(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetDTDeliveryChargeInfo(request);
        }

        public async Task<SMSPurchase> AddSmsPurchase(SMSPurchase request)
        {
            return await _weightRangeRepository.AddSmsPurchase(request);
        }

        public async Task<List<GetPurchasedSMSInfoViewModel>> GetPurchasedSMSInfo(int courierUserId)
        {
            return await _weightRangeRepository.GetPurchasedSMSInfo(courierUserId);
        }

        public async Task<Users> UpdateAdUserSalaryAmount(Users user)
        {
            return await _weightRangeRepository.UpdateAdUserSalaryAmount(user);
        }

        public async Task<IEnumerable<dynamic>> GetDatewiseVoucherInfo(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetDatewiseVoucherInfo(request);
        }

        public async Task<int> AddLenderUser(LenderUser lenderUser)
        {
            return await _weightRangeRepository.AddLenderUser(lenderUser);
        }

        public async Task<int> AddUserLocationAssign(UserLocationAssign userLocationAssign)
        {
            return await _weightRangeRepository.AddUserLocationAssign(userLocationAssign);
        }

        public async Task<List<LenderCourierUserAssignment>> AssignLenderUserToCourierUser(List<LenderCourierUserAssignment> lenderCourierUserAssignments)
        {
            return await _weightRangeRepository.AssignLenderUserToCourierUser(lenderCourierUserAssignments);
        }

        public async Task<List<LenderCourierUserAssignment>> UnAssignLenderUserToCourierUser(List<LenderCourierUserAssignment> lenderCourierUserAssignments)
        {
            return await _weightRangeRepository.UnAssignLenderUserToCourierUser(lenderCourierUserAssignments);
        }

        public async Task<List<LenderUser>> GetLenderUsers()
        {
            return await _weightRangeRepository.GetLenderUsers();
        }

        public async Task<IEnumerable<dynamic>> GetLenderWiseAssignedCourierUsers(int lenderUserId)
        {
            return await _weightRangeRepository.GetLenderWiseAssignedCourierUsers(lenderUserId);
        }

        public async Task<int> UpdatePoHOrders(CourierOrders request, string type)
        {
            return await _weightRangeRepository.UpdatePoHOrders(request, type);
        }

        public async Task<int> UpdateUserLocationAssign(int userLocationAssignId, UserLocationAssign userLocationAssign)
        {
            return await _weightRangeRepository.UpdateUserLocationAssign(userLocationAssignId, userLocationAssign);
        }

        public async Task<dynamic> UpdateReferencewisePayment(PaymentReference request, string paymentFrom)
        {
            return await _weightRangeRepository.UpdateReferencewisePayment(request, paymentFrom);
        }

        public async Task<dynamic> GetPaymentReferenceReport(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetPaymentReferenceReport(request);
        }

        public async Task<List<UserLocationAssignViewModel>> GetUserLocationAssign()
        {
            return await _weightRangeRepository.GetUserLocationAssign();
        }

        public async Task<dynamic> GetPohOrderStatuswise(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetPohOrderStatuswise(request);
        }

        public async Task<dynamic> GetPohOrderwiseReport(RequestBodyModel request)
        {
            return await _weightRangeRepository.GetPohOrderwiseReport(request);
        }
    }
}
