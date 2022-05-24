using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.BodyModel;
using AdCourier.Domain.Entities.BodyModel.CollectorAssign;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.OrderTracking;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.CollectorAssign;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.DeliverManAssign;
using AdCourier.Domain.Entities.ViewModel.OrderTracking;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class OrderTrackingService : IOrderTrackingService
    {
        private readonly RedisModel _redisModel;
        private readonly IOrderTrackingRepository _orderTrackingRepository;
        private readonly IRedisCacheClient _redis;
        private readonly SqlServerContext _sqlServerContext;
        public OrderTrackingService(IOptions<RedisModel> redisModel, SqlServerContext sqlServerContext, IOrderTrackingRepository orderTrackingRepository, IRedisCacheClient redis)
        {
            _redisModel = redisModel.Value;
            _orderTrackingRepository = orderTrackingRepository;
            _redis = redis;
            _sqlServerContext = sqlServerContext;
        }

        public async Task<CourierOrderStatus> AddCourierOrderStatus(CourierOrderStatus courierOrderStatus)
        {
            return await _orderTrackingRepository.AddCourierOrderStatus(courierOrderStatus);
        }
        public async Task<OrderTracking> GetOrderTrackingNew(OrderTrackingBodyModel orderTrackingBodyModel, string flag)
        {
            var orderTrackingModel = new OrderTracking();

            var shipmentGroup = new int[] { 4 };
            //var pickGroup = new int[] { 5 };
            var deliverdGroup = new int[] { 6 };
            var returnGroup = new int[] { 1, 11, 10, 8 };
            var removeGroup = new int[] { 3, 7 };
            var removeSingleGroup = new int[] { 3 };

            var data = await _orderTrackingRepository.GetOrderTrackingNew(orderTrackingBodyModel, flag);

            var SubTrackingShipmentShow = new int[] { 4 };


            if (flag == "private")
            {
                var courierOrdersViewModel = data.Select(e => new CourierOrdersViewModel
                {
                    CourierOrdersId = orderTrackingBodyModel.CourierOrderId,
                    CollectionName = e.CollectionName
                }).FirstOrDefault();

                var reponse = data.GroupBy(x => x.OrderTrackStatusGroup).Select(x => new OrderTrackingGroupViewModel
                {
                    StatusGroupId = x.FirstOrDefault().StatusGroupId,
                    TrackingName = OrderTrackStatusGroupCalculate(x.FirstOrDefault().OrderTrackStatusGroup, x.FirstOrDefault().PickDistrictBng, x.FirstOrDefault().DistrictBng, x.FirstOrDefault().StatusGroupId, x.FirstOrDefault().Status),
                    TrackingColor = x.FirstOrDefault().TrackingColor,
                    TrackingFlag = x.FirstOrDefault().TrackingFlag,
                    //TrackingDate = x.FirstOrDefault().PostedOn,
                    TrackingDate = x.FirstOrDefault().StatusGroupId.Equals(13) ? x.Min(s => s.PostedOn) : x.Max(s => s.PostedOn),
                    DistrictId = x.FirstOrDefault().DistrictId,
                    PickDistrictId = x.FirstOrDefault().PickDistrictId,
                    ExpectedFirstDeliveryDate = x.FirstOrDefault().ExpectedFirstDeliveryDate,
                    ExpectedDeliveryDate = x.FirstOrDefault().ExpectedDeliveryDate,
                    TrackingOrderBy = x.FirstOrDefault().TrackingOrderBy,
                    Status = x.FirstOrDefault().Status,
                    SubTrackingShipmentName = new HubViewModel
                    {
                        Name = SubTrackingShipmentShow.Contains(x.FirstOrDefault().StatusGroupId) ? x.FirstOrDefault().HubName + " ছেড়েছে" : "",
                        Latitude = "",
                        Longitude = ""
                    },
                    SubTrackingReturnName = new HubViewModel
                    {
                        Name = SubTrackingReturnShowCalculate(x.FirstOrDefault().StatusGroupId, x.FirstOrDefault().HubName, x.FirstOrDefault().Status, x.FirstOrDefault().StatusNameBng),
                        //Name = SubTrackingReturnShow.Contains(x.FirstOrDefault().StatusGroupId) ? x.FirstOrDefault().HubName : "",
                        Latitude = SubTrackingReturnShowLatitudeCalculate(x.FirstOrDefault().StatusGroupId, x.FirstOrDefault().HubName, x.FirstOrDefault().Status, x.FirstOrDefault().Latitude),
                        Longitude = SubTrackingReturnShowLongitudeCalculate(x.FirstOrDefault().StatusGroupId, x.FirstOrDefault().HubName, x.FirstOrDefault().Status, x.FirstOrDefault().Longitude)
                    },
                    CourierDeliveryMan = new CourierDeliveryManViewModel
                    {
                        CourierDeliveryManMobile = x.FirstOrDefault().CourierDeliveryManMobile,
                        CourierDeliveryManName = x.FirstOrDefault().CourierDeliveryManName,
                        CourierComment = x.FirstOrDefault().CourierComment,
                        EDeshMobileNo = x.FirstOrDefault().EDeshMobileNo
                    }

                }).Where(x => x.TrackingName != "").ToList();

                if (reponse.Where(x => returnGroup.Contains(x.StatusGroupId)).Count() == 0
                    && reponse.Where(x => shipmentGroup.Contains(x.StatusGroupId)).Count() > 0
                    && reponse.Where(x => deliverdGroup.Contains(x.StatusGroupId)).Count() == 0)
                {
                    reponse.Add(new OrderTrackingGroupViewModel
                    {
                        TrackingName = "সম্ভাব্য ডেলিভারি",
                        TrackingOrderBy = 100,
                        StatusGroupId = 0,
                        ExpectedFirstDeliveryDate = reponse.FirstOrDefault().ExpectedFirstDeliveryDate,
                        ExpectedDeliveryDate = reponse.FirstOrDefault().ExpectedDeliveryDate,
                    });
                }
                if (reponse.FirstOrDefault().DistrictId.Equals(reponse.FirstOrDefault().PickDistrictId))
                {
                    reponse.RemoveAll(r => removeGroup.Contains(r.StatusGroupId)
                    && reponse.FirstOrDefault().Status != 61);
                }
                if (reponse.FirstOrDefault().PickDistrictId == 0)
                {
                    reponse.RemoveAll(r => removeSingleGroup.Contains(r.StatusGroupId));
                }

                orderTrackingModel.courierOrdersViewModel = courierOrdersViewModel;
                orderTrackingModel.orderTrackingGroupViewModel = reponse.OrderByDescending(x => x.TrackingOrderBy).ToList();
                return orderTrackingModel;
            }

            return null;
        }

        private string SubTrackingReturnShowLatitudeCalculate(int statusGroupId, string hubName, int status, string latitude)
        {
            var SubTrackingReturnShow = new int[] { 8 };
            var SubTrackingReturnWireHouseHubShow = new int[] { 10 };
            var SubTrackingReturnNearHubShow = new int[] { 11 };
            var customeReturnStatus = new int[] { 26 };

            if (SubTrackingReturnShow.Contains(statusGroupId) && customeReturnStatus.Contains(status))
            {
                return "";
            }
            else if (SubTrackingReturnWireHouseHubShow.Contains(statusGroupId))
            {
                return "23.7501826";
            }
            else if (SubTrackingReturnNearHubShow.Contains(statusGroupId))
            {
                return latitude;
            }
            return "";
        }
        private string SubTrackingReturnShowLongitudeCalculate(int statusGroupId, string hubName, int status, string longitude)
        {
            var SubTrackingReturnShow = new int[] { 8 };
            var SubTrackingReturnWireHouseHubShow = new int[] { 10 };
            var SubTrackingReturnNearHubShow = new int[] { 11 };
            var customeReturnStatus = new int[] { 26 };

            if (SubTrackingReturnShow.Contains(statusGroupId) && customeReturnStatus.Contains(status))
            {
                return "";
            }
            else if (SubTrackingReturnWireHouseHubShow.Contains(statusGroupId))
            {
                return "90.3905834";
            }
            else if (SubTrackingReturnNearHubShow.Contains(statusGroupId))
            {
                return longitude;
            }
            return "";
        }
        private string SubTrackingReturnShowCalculate(int statusGroupId, string hubName, int status, string statusNameBng)
        {
            var SubTrackingReturnShow = new int[] { 8 };
            var SubTrackingReturnWireHouseHubShow = new int[] { 10 };
            var SubTrackingReturnNearHubShow = new int[] { 11 };
            var customeReturnStatus = new int[] { 26 };

            if (SubTrackingReturnShow.Contains(statusGroupId) && customeReturnStatus.Contains(status))
            {
                return statusNameBng;
            }
            else if (SubTrackingReturnWireHouseHubShow.Contains(statusGroupId))
            {
                return "লালমাটিয়া হাব";
            }
            else if (SubTrackingReturnNearHubShow.Contains(statusGroupId))
            {
                return hubName;
            }
            return "";
        }

        private string OrderTrackStatusGroupCalculate(string name, string pickDistrictBng, string districtBng, int statusGroupId, int status)
        {
            if (statusGroupId.Equals(3))
            {
                return pickDistrictBng + " " + name;
            }
            else if (statusGroupId.Equals(7) && status.Equals(61))
            {
                return "লাস্টমাইলে পৌঁছেছে";
            }
            else if (statusGroupId.Equals(7))
            {
                return districtBng + " " + name;
            }
            else
            {
                return name;
            }

            //switch (statusGroupId)
            //{
            //    case 3:
            //        return pickDistrictBng + " " + name;
            //    case 7:
            //        return districtBng + " " + name;
            //    default:
            //        return name;
            //}
        }

        public async Task<IEnumerable<OrderTrackingStatusViewModel>> GetOrderTracking(OrderTrackingBodyModel orderTrackingBodyModel, string flag)
        {
            return await _orderTrackingRepository.GetOrderTracking(orderTrackingBodyModel, flag);
        }
        public async Task<IEnumerable<CourierOrderStatusViewModel>> GetCourierOrderStatus()
        {
            return await _orderTrackingRepository.GetCourierOrderStatus();
        }
        public async Task<IEnumerable<CourierOrderStatus>> LoadCourierStatus()
        {
            List<CourierOrderStatus> defaultStatusList = new List<CourierOrderStatus>();

            defaultStatusList.Add(new CourierOrderStatus
            {
                StatusId = -1,
                StatusNameBng = "সিলেক্ট স্ট্যাটাস",
                StatusNameEng = "Select Status"
            });

            //var responseData = await _orderTrackingRepository.LoadCourierStatus();

            if (_redisModel.IsActive)
            {
                string key = "LoadCourierStatus";

                if (await _redis.Db1.Database.KeyExistsAsync(key) == true)
                {
                    var data = await _redis.Db1.GetAsync<IEnumerable<CourierOrderStatus>>(key);
                    return defaultStatusList.Concat(data.ToList()).OrderBy(l => l.StatusId);
                }
                else
                {
                    var data = await _orderTrackingRepository.LoadCourierStatus();
                    bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                    return defaultStatusList.Concat(data.ToList()).OrderBy(l => l.StatusId);
                }
            }
            else
            {
                var data = await _orderTrackingRepository.LoadCourierStatus();
                return defaultStatusList.Concat(data.ToList()).OrderBy(l => l.StatusId);
            }


        }

        public async Task<StatusGroup> AddStatusGroup(StatusGroup statusGroup)
        {
            return await _orderTrackingRepository.AddStatusGroup(statusGroup);
        }

        public async Task<IEnumerable<StatusGroup>> GetStatusGroup()
        {
            //return await _orderTrackingRepository.GetStatusGroup();


            if (_redisModel.IsActive)
            {
                string key = "GetStatusGroup";

                if (await _redis.Db1.Database.KeyExistsAsync(key) == true)
                {
                    return await _redis.Db1.GetAsync<IEnumerable<StatusGroup>>(key);
                }
                else
                {
                    var data = await _orderTrackingRepository.GetStatusGroup();
                    bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                    return data;
                }
            }
            else
            {
                return await _orderTrackingRepository.GetStatusGroup();
            }
        }

        public async Task<StatusGroup> UpdateStatusGroup(int id, StatusGroup statusGroup)
        {
            return await _orderTrackingRepository.UpdateStatusGroup(id, statusGroup);
        }

        public async Task<CourierOrderStatus> UpdateCourierOrderStatus(int id, CourierOrderStatus courierOrderStatus)
        {
            return await _orderTrackingRepository.UpdateCourierOrderStatus(id, courierOrderStatus);
        }

        public async Task<IEnumerable<CourierOrderTrackHistoryViewModel>> OrderUpdateHistory(string courierOrderId)
        {
            return await _orderTrackingRepository.OrderUpdateHistory(courierOrderId);
        }

        public async Task<IEnumerable<Collectors>> GetAllCollectors()
        {
            return await _orderTrackingRepository.GetAllCollectors();
        }

        public async Task<IEnumerable<DeliveryUsers>> GetAllDeliveryMan()
        {
            return await _orderTrackingRepository.GetAllDeliveryMan();
        }
        public async Task<IEnumerable<DeliveryUsers>> GetLocationAssignDeliveryMan()
        {
            return await _orderTrackingRepository.GetLocationAssignDeliveryMan();

        }
        public async Task<List<LocationAssign>> AddMultipleLocationAssign(List<LocationAssign> locationAssign)
        {
            var data = await _orderTrackingRepository.AddMultipleLocationAssign(locationAssign);

            var locationAssignHistory = new List<LocationAssignHistory>();

            foreach (var item in data)
            {
                locationAssignHistory.Add(new LocationAssignHistory
                {
                    DeliveryUserId = item.DeliveryUserId,
                    CollectorId = item.CollectorId,
                    DistrictId = item.DistrictId,
                    ThanaId = item.ThanaId,
                    AreaId = item.AreaId,
                    DtDefaultAssign = item.DtDefaultAssign,
                    AdDefaultAssign = item.AdDefaultAssign,
                    ZoneId = item.ZoneId,
                    InsertedBy = item.InsertedBy,
                    UpdatedBy = item.UpdatedBy
                });
            }

            var insertToHistory = await _orderTrackingRepository.AddMultipleLocationAssignHistory(locationAssignHistory);

            return data;
        }
        public async Task<List<CollectorAssign>> AddMultipleCollectorAssign(List<CollectorAssign> collectorAssign)
        {
            return await _orderTrackingRepository.AddMultipleCollectorAssign(collectorAssign);
        }
        public async Task<CollectorAssign> AddCollectorAssign(CollectorAssign collectorAssign)
        {
            return await _orderTrackingRepository.AddCollectorAssign(collectorAssign);
        }
        public async Task<DeliveryBonduAssign> AddDeliveryManAssign(DeliveryBonduAssign deliveryBonduAssign)
        {
            return await _orderTrackingRepository.AddDeliveryManAssign(deliveryBonduAssign);
        }
        public async Task<List<DeliveryBonduAssign>> AddDeliveryBonduAssignMultiple(List<DeliveryBonduAssign> deliveryBonduAssign)
        {
            return await _orderTrackingRepository.AddDeliveryBonduAssignMultiple(deliveryBonduAssign);
        }
        public async Task<CollectorAssign> UpdateCollectorAssign(int id, CollectorAssign collectorAssign)
        {
            return await _orderTrackingRepository.UpdateCollectorAssign(id, collectorAssign);
        }
        public async Task<CollectorAssign> UpdateCollectorAssignForLocation(int id, CollectorAssign collectorAssign)
        {
            return await _orderTrackingRepository.UpdateCollectorAssignForLocation(id, collectorAssign);
        }
        public async Task<int> UpdateMultipleCollectorAssignForLocation(MultipleCollectorAssign multipleCollectorAssign)
        {
            return await _orderTrackingRepository.UpdateMultipleCollectorAssignForLocation(multipleCollectorAssign);
        }
        public async Task<IEnumerable<dynamic>> GetAllCollectorsLocationAssign()
        {
            return await _orderTrackingRepository.GetAllCollectorsLocationAssign();
        }
        public async Task<List<dynamic>> GetAllLocationAssign()
        {
            return await _orderTrackingRepository.GetAllLocationAssign();
        }

        public async Task<IEnumerable<CollectorAssignViewModel>> GetAllCollectorsAssign()
        {
            return await _orderTrackingRepository.GetAllCollectorsAssign();
        }
        public async Task<IEnumerable<DeliveryZoneInfo>> GetDeliveryZoneInfo()
        {
            return await _orderTrackingRepository.GetDeliveryZoneInfo();
        }
        public async Task<IEnumerable<DeliveryZone>> GetDeliveryZone()
        {
            return await _orderTrackingRepository.GetDeliveryZone();
        }
        public async Task<IEnumerable<DeliveryManAssignViewModel>> GetAllDeliveryMansAssign()
        {
            return await _orderTrackingRepository.GetAllDeliveryMansAssign();
        }
        public async Task<CourierUsers> UpdateMerchantInformation(int id, CourierUsers courierUserInfo)
        {
            return await _orderTrackingRepository.UpdateMerchantInformation(id, courierUserInfo);
        }
        public async Task<CourierUsers> CustomerVoiceSmsLimit(int courierUserId, int customerVoiceSmsLimit)
        {
            return await _orderTrackingRepository.CustomerVoiceSmsLimit(courierUserId, customerVoiceSmsLimit);
        }
        public async Task<CourierUsers> UpdateCustomerSMSLimit(int courierUserId, int customerSMSLimit)
        {
            return await _orderTrackingRepository.UpdateCustomerSMSLimit(courierUserId, customerSMSLimit);
        }

        public async Task<int> DeleteCollectorAssign(int id)
        {
            return await _orderTrackingRepository.DeleteCollectorAssign(id);
        }

        public async Task<int> DeletePickupLocations(int id)
        {
            return await _orderTrackingRepository.DeletePickupLocations(id);
        }

        public async Task<CourierUsersViewModel> GetCourierUsersInformation(int courierUserId)
        {
            return await _orderTrackingRepository.GetCourierUsersInformation(courierUserId);
        }

        public Task<int> DeleteLocationAssign(int id)
        {
            return _orderTrackingRepository.DeleteLocationAssign(id);
        }

        public async Task<int> DeleteUserLocationAssign(int userLocationAssignId)
        {
            return await _orderTrackingRepository.DeleteUserLocationAssign(userLocationAssignId);
        }

        public Task<int> DeleteDeliveryChargeDetails(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            return _orderTrackingRepository.DeleteDeliveryChargeDetails(assignCouirerAndServiceBodyModel);
        }

        public Task<int> UpdateAssignmentFalse(RequestBodyModel requestBody)
        {
            return _orderTrackingRepository.UpdateAssignmentFalse(requestBody);
        }

        public Task<int> UpdateDeliveryChargeDetails(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            return _orderTrackingRepository.UpdateDeliveryChargeDetails(assignCouirerAndServiceBodyModel);
        }

        public Task<int> UpdateDeliveryChargeMerchantDetails(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            return _orderTrackingRepository.UpdateDeliveryChargeMerchantDetails(assignCouirerAndServiceBodyModel);
        }

        public Task<int> UpdateServiceTypeCourier(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            return _orderTrackingRepository.UpdateServiceTypeCourier(assignCouirerAndServiceBodyModel);
        }

        public Task<int> UpdateMerchantServiceTypeCourier(AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            return _orderTrackingRepository.UpdateMerchantServiceTypeCourier(assignCouirerAndServiceBodyModel);
        }
        public async Task<DeliveryUsers> UpdateDeliveryUsers(DeliveryUsers request, int userId)
        {
            return await _orderTrackingRepository.UpdateDeliveryUsers(request, userId);
        }
        public async Task<Vouchers> UpdateVoucher(Vouchers vouchers)
        {
            return await _orderTrackingRepository.UpdateVoucher(vouchers);
        }

        public async Task<int> BulkInsertRedxPopData(List<RedxPop> popData)
        {
            return await _orderTrackingRepository.BulkInsertRedxPopData(popData);

        }

        public async Task<int> AddMailContent(PaymentMail request)
        {
            return await _orderTrackingRepository.AddMailContent(request);

        }
    }
}
