using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.OrderTracking;
using AdCourier.Domain.Entities.BodyModel.Permission;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ProcedureDataModel;
using AdCourier.Domain.Entities.Utility;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.DeliveryManAssign;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;


namespace AdCourier.Services
{
    public class OrderService : IOrderService
    {
        private readonly RedisModel _redisModel;
        private readonly IFirebaseCloudService _firebaseCloudService;
        private readonly IExcelService _excelService;
        private readonly IOrderGenericRepository _orderGenericRepository;
        private readonly ISmsEmailService _smsEmailService;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly IWeightRangeRepository _weightRangeRepository;
        private readonly IRedisCacheClient _redis;
        private readonly SqlServerContext _sqlServerContext;
        private readonly OtherService _otherService;
        //private SmtpClient _smtpClient;
        public OrderService(IOptions<RedisModel> redisModel, SqlServerContext sqlServerContext, IOrderRepository orderRepository,
            IOrderGenericRepository orderGenericRepository,
            IOrderHistoryRepository orderHistoryRepository,
            IWeightRangeRepository weightRangeRepository,
            ISmsEmailService smsEmailService,
            IRedisCacheClient redis,
            IExcelService excelService,
            IFirebaseCloudService firebaseCloudService,
            OtherService otherService
            //SmtpClient smtpClient
            )
        {
            _redisModel = redisModel.Value;
            _orderRepository = orderRepository;
            _orderGenericRepository = orderGenericRepository;
            _orderHistoryRepository = orderHistoryRepository;
            _weightRangeRepository = weightRangeRepository;
            _smsEmailService = smsEmailService;
            _redis = redis;
            _sqlServerContext = sqlServerContext;
            _excelService = excelService;
            _firebaseCloudService = firebaseCloudService;
            _otherService = otherService;
            //_smtpClient = smtpClient;
        }

        public async Task<IEnumerable<CourierOrders>> AddOrdersBulk(List<CourierOrders> courierOrders)
        {
            var orderHistory = new List<CourierOrderStatusHistory>();

            foreach (var item in courierOrders)
            {
                item.CollectionTime = DateTime.Now.Hour >= 21 ? DateTime.Now.AddDays(1) : DateTime.Now;
            }
            //courierOrders.CollectionTime = DateTime.Now.Hour >= 21 ? DateTime.Now.AddDays(1) : DateTime.Now;

            var data = await _orderRepository.AddOrdersBulk(courierOrders);

            foreach (var item in data)
            {
                orderHistory.Add(new CourierOrderStatusHistory
                {
                    OrderDate = item.OrderDate,
                    CourierOrderId = item.CourierOrdersId,
                    Status = item.Status,
                    PostedBy = item.UpdatedBy,
                    MerchantId = item.MerchantId,
                    Comment = item.Comment,
                    IsConfirmedBy = item.IsConfirmedBy
                });
            }

            var resOrderHistory = await _orderRepository.AddCourierOrderHistoryBulk(orderHistory);

            return data;
        }

        public async Task<CourierOrders> AddOrder(CourierOrders courierOrders)
        {
            if (courierOrders.ActualPackagePrice <= 0)
            {
                return null;
            }
            if (courierOrders.PaymentServiceType == 1)
            {
                courierOrders.CodCharge = 0;
            }
            
            if (courierOrders.OfficeDrop)
            {
                courierOrders.CollectionCharge = 0;
            }
            //else
            //{
            //    courierOrders.CollectionCharge = 5;
            //}

            //AutoProcess
            IQueryable<CourierUsers> courierUsers = _sqlServerContext.CourierUsers.Where(x => x.CourierUserId.Equals(courierOrders.MerchantId));

            if (courierUsers.FirstOrDefault().IsAutoProcess
                && courierOrders.CollectAddressDistrictId > 0
                && courierOrders.CollectAddressThanaId > 0
                && courierOrders.OfficeDrop.Equals(false)
                )
            {
                courierOrders.IsAutoProcess = true;
                courierOrders.Status = 40;
                courierOrders.UpdatedBy = 82;
                courierOrders.IsConfirmedBy = "autoprocess";
                courierOrders.Comment = "Order has been assigned Product will be collected from collection point";
                courierOrders.CourierId = 35;
                courierOrders.PodNumber = UniqueCodeGenerator.GetUniqueCode(isCharaterLowerCaseInCouponCode: true, minNumberForRandomNumberGenerator: 10, maxNumberForRandomNumberGenerator: 99) + "-" + "deliveybondhu";
            }

            if (courierUsers.FirstOrDefault().Verify.ToLower().Equals("poh") && courierUsers.FirstOrDefault().PaymentServiceType.Equals(1))
            {
                courierOrders.PaymentServiceTypeVerify = "verify";
                courierOrders.PaymentServiceTypeMerchantVerify = "verify";
            }

            //AutoProcess
            // temporary solution
            //if (courierOrders.OrderFrom.Equals("desktop site"))
            //{
            //    courierOrders.CollectAddress = _sqlServerContext.PickupLocations.Where(d => d.DistrictId.Equals(courierOrders.CollectAddressDistrictId)
            //    && d.ThanaId.Equals(courierOrders.CollectAddressThanaId) && d.IsActive.Equals(true) && d.CourierUserId.Equals(courierOrders.MerchantId)).FirstOrDefault().PickupAddress;
            //}

            //RemoveSpecialCharacters
            courierOrders.Address = SpecialCharacters.RemoveSpecialCharacters(courierOrders.Address);
            courierOrders.CustomerName = SpecialCharacters.RemoveSpecialCharacters(courierOrders.CustomerName);
            courierOrders.Note = SpecialCharacters.RemoveSpecialCharacters(courierOrders.Note);
            courierOrders.CollectAddress = SpecialCharacters.RemoveSpecialCharacters(courierOrders.CollectAddress);
            courierOrders.CollectionName = SpecialCharacters.RemoveSpecialCharacters(courierOrders.CollectionName);
            //RemoveSpecialCharacters

            // add default CollectionTimeSlotId 4 for old app
            if (courierOrders.CollectionTimeSlotId.Equals(0))
            {
                courierOrders.CollectionTimeSlotId = 4;
                courierOrders.CollectionTime = DateTime.Now.Hour >= 21 ? DateTime.Now.AddDays(1) : DateTime.Now;
            }
           

            // version check

            if (courierOrders.OrderFrom != "" && courierOrders.OrderFrom.Contains("android-"))
            {
                int startIndex = 8;
                int endIndex = courierOrders.OrderFrom.Length - 8;
                courierOrders.Version = courierOrders.OrderFrom.Substring(startIndex, endIndex);
                courierOrders.OrderFrom = "android";
            }
            // version check



            var data = await _orderRepository.AddOrder(courierOrders);
            Log.Information("Add Order {@data} on {Created}", data, DateTime.Now);

            //if (data.Id > 0)
            //{
            //    courierOrderStatusHistory.CourierOrderId = data.CourierOrdersId;
            ////    courierOrderStatusHistory.PostedBy = courierOrders.MerchantId;
            //    courierOrderStatusHistory.MerchantId = courierOrders.MerchantId;
            //    courierOrderStatusHistory.PodNumber = "";


            //    var responseOrderHistory = await _orderHistoryRepository.AddCourierOrderHistory(courierOrderStatusHistory);
            //    Log.Information("Add OrderHistory {@responseOrderHistory} on {Created}", responseOrderHistory, DateTime.Now);
            //}

            return data;

        }

        //private async Task<int> UpdateOrder(string VoucherCode, CourierOrders courierOrders)
        //{
        //    var order = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(x => x.CourierOrdersId == courierOrders.CourierOrdersId);
        //    var voucher = await _sqlServerContext.Vouchers.FirstOrDefaultAsync(x => x.VoucherCode == VoucherCode && x.CourierUserId == courierOrders.MerchantId && x.DeliveryRangeId == courierOrders.DeliveryRangeId);

        //    var updatedDeliveryCharge = order.DeliveryCharge - (order.DeliveryCharge * (voucher.VoucherValue / 100));
        //    order.DeliveryCharge = updatedDeliveryCharge;
        //    _sqlServerContext.CourierOrders.Update(order);
        //    await _sqlServerContext.SaveChangesAsync();

        //    voucher.ApplicableQuantity = voucher.ApplicableQuantity - 1;
        //    _sqlServerContext.Vouchers.Update(voucher);
        //    return await _sqlServerContext.SaveChangesAsync();
        //}

        public Task<List<CourierOrderViewModel>> CollectorWiseData(CollectorOrderBodyModel collectorOrderBodyModel)
        {
            return _orderRepository.CollectorWiseData(collectorOrderBodyModel);
        }

        public async Task<DeliveryChargeDetails> GetDeliveryChargeDetailsPrice(DeliveryChargeDetails deliveryChargeDetails)
        {
            return await _orderRepository.GetDeliveryChargeDetailsPrice(deliveryChargeDetails);
        }

        public async Task<dynamic> GetChangeDeliveryChargeDetailsLog(ChangeDeliveryChargeDetailsLog changeDeliveryChargeDetailsLog)
        {
            return await _orderRepository.GetChangeDeliveryChargeDetailsLog(changeDeliveryChargeDetailsLog);
        }

        public async Task<DeliveryChargeDetails_test> GetDeliveryChargeDetailsPrice_test(DeliveryChargeDetails_test deliveryChargeDetails)
        {
            return await _orderRepository.GetDeliveryChargeDetailsPrice_test(deliveryChargeDetails);
        }
        public async Task<List<CourierUsers>> ListOfCourierUser(int collectorId, int groupType)
        {
            return await _orderRepository.ListOfCourierUser(collectorId, groupType);
        }

        public async Task<CourierOrderDetailsViewModel> LoadCourierOrder(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            return await _orderRepository.LoadCourierOrder(loadCourierOrderBodyModel);
        }

        public async Task<CourierAmountDetailsResponse> LoadCourierOrderAmountDetails(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            return await _orderRepository.LoadCourierOrderAmountDetails(loadCourierOrderBodyModel);
        }
        public async Task<CourierOrderDetailsViewModel> GetAllOrders(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var editStatus = new int[] { 0, 1, 4, 5, 6, 40, 41 };
            var data = await _orderRepository.GetAllOrders(loadCourierOrderBodyModel);

            var res = data.CodCollectionDetails.Select(s => new CourierOrderViewModel
            {
                CourierOrdersId = s.CourierOrdersId,
                Id = s.Id,
                Comment = s.Comment,
                PodNumber = s.PodNumber,
                CourierId = s.CourierId,
                CourierName = s.CourierName,
                CustomerName = s.CustomerName,
                Status = getStatusName(s.CollectionAmount, s.Status, s.DashboardStatusGroup), //s.StatusNameBng,
                OrderTrackStatusGroup = s.OrderTrackStatusGroup,
                DashboardStatusGroup = s.DashboardStatusGroup,
                StatusId = s.Status,
                ButtonFlag = editStatus.Contains(s.Status) ? true : false,
                StatusEng = s.StatusNameEng,
                StatusType = s.StatusType,
                PaymentServiceCharge = s.PaymentServiceCharge,
                PaymentServiceType = s.PaymentServiceType,
                HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                {
                    Name = s.Name,
                    Value = s.Value,
                    HubAddress = s.HubAddress,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    HubMobile = s.HubMobile
                },
                CourierAddressContactInfo = new CourierAddressContactInfo
                {
                    Mobile = s.CustomerMobile,
                    OtherMobile = s.OtherMobile,
                    Address = s.CustomerAddress,
                    DistrictName = s.DistrictName,
                    ThanaName = s.ThanaName,
                    AreaName = s.AreaName,
                    DistrictNameEng = s.DistrictNameEng,
                    ThanaNameEng = s.ThanaNameEng,
                    AreaNameEng = s.AreaNameEng,
                    ThanaPostalCode = s.ThanaPostalCode,
                    AreaPostalCode = s.AreaPostalCode,
                    DistrictId = s.DistrictId,
                    ThanaId = s.ThanaId,
                    AreaId = s.AreaId,
                    CollectAddressDistrictId = s.CollectAddressDistrictId,
                    CollectAddressThanaId = s.CollectAddressThanaId,
                    CollectDistrictName = s.CollectDistrictName,
                    CollectThanaName = s.CollectThanaName
                },
                CourierOrderInfo = new CourierOrderInfo
                {
                    PaymentType = s.PaymentType,
                    OrderType = s.OrderType,
                    Weight = s.Weight,
                    CollectionName = s.CollectionName,
                    WeightRangeId = s.WeightRangeId
                },
                CourierPrice = new CourierPrice
                {
                    CollectionAmount = s.CollectionAmount,
                    DeliveryCharge = s.DeliveryCharge,
                    BreakableCharge = s.BreakableCharge,
                    CODCharge = s.CodCharge,
                    CollectionCharge = s.CollectionCharge,
                    ReturnCharge = s.ReturnCharge,
                    OfficeDrop = s.OfficeDrop,
                    PaymentServiceCharge = s.PaymentServiceCharge,
                },
                CourierOrderDateDetails = new CourierOrderDateDetails
                {
                    UpdatedOn = s.UpdatedOn,
                    ConfirmationFormatDate = s.ConfirmationDate,
                    OrderFormatDate = s.OrderDate,
                    PostedFormatDate = s.PostedOn
                },
                UserInfo = new UserInfo
                {
                    CourierUserId = s.MerchantId,
                    Mobile = s.Mobile,
                    UserName = s.UserName,
                    Address = s.Address,
                    EmailAddress = s.EmailAddress,
                    CollectAddress = s.CollectAddress

                },
                AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                {

                    IsEmail = s.IsEmail,
                    IsSms = s.IsSms
                }
            });

            var responseData = new CourierOrderDetailsViewModel
            {
                TotalCount = data.CodCollectionTotal.CodCollectionTotalCount,
                CourierOrderViewModel = res.ToList()
            };
            return responseData;
        }

        private string getStatusName(decimal collectionAmount, int status, string dashboardStatusGroup)
        {
            if (status.Equals(15) && collectionAmount > 0)
            {
                return dashboardStatusGroup + " (Cod)";
            }
            else if (status.Equals(15) && collectionAmount.Equals(0))
            {
                return dashboardStatusGroup + " (Paid)";
            }
            else
            {
                return dashboardStatusGroup;
            }
            
        }

        public async Task<CourierOrderDetailsViewModel> GetCodCollections(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var data = await _orderRepository.GetCodCollections(loadCourierOrderBodyModel);

            var editStatus = new int[] { 0, 1, 4, 5, 6 };

            var res = data.CodCollectionDetails.Select(s => new CourierOrderViewModel
            {
                CourierOrdersId = s.CourierOrdersId,
                Id = s.Id,
                Comment = s.Comment,
                PodNumber = s.PodNumber,
                CourierId = s.CourierId,
                CourierName = s.CourierName,
                CustomerName = s.CustomerName,
                Status = s.StatusNameBng,
                StatusId = s.Status,
                ButtonFlag = editStatus.Contains(s.Status) ? true : false,
                StatusEng = s.StatusNameEng,
                StatusType = s.StatusType,
                HubViewModel = new Domain.Entities.ViewModel.HubViewModel
                {
                    Name = s.Name,
                    Value = s.Value,
                    HubAddress = s.HubAddress,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    HubMobile = s.HubMobile
                },
                CourierAddressContactInfo = new CourierAddressContactInfo
                {
                    Mobile = s.CustomerMobile,
                    OtherMobile = s.OtherMobile,
                    Address = s.Address,
                    DistrictName = s.DistrictName,
                    ThanaName = s.ThanaName,
                    AreaName = s.AreaName,
                    DistrictNameEng = s.DistrictNameEng,
                    ThanaNameEng = s.ThanaNameEng,
                    AreaNameEng = s.AreaNameEng,
                    ThanaPostalCode = s.ThanaPostalCode,
                    AreaPostalCode = s.AreaPostalCode,
                    DistrictId = s.DistrictId,
                    ThanaId = s.ThanaId,
                    AreaId = s.AreaId
                },
                CourierOrderInfo = new CourierOrderInfo
                {
                    PaymentType = s.PaymentType,
                    OrderType = s.OrderType,
                    Weight = s.Weight,
                    CollectionName = s.CollectionName
                },
                CourierPrice = new CourierPrice
                {
                    CollectionAmount = s.CollectionAmount,
                    DeliveryCharge = s.DeliveryCharge,
                    BreakableCharge = s.BreakableCharge,
                    CODCharge = s.CodCharge,
                    CollectionCharge = s.CollectionCharge,
                    ReturnCharge = s.ReturnCharge
                },
                CourierOrderDateDetails = new CourierOrderDateDetails
                {
                    UpdatedOn = s.UpdatedOn,
                    ConfirmationFormatDate = s.ConfirmationDate,
                    OrderFormatDate = s.OrderDate,
                    PostedFormatDate = s.PostedOn
                },
                UserInfo = new UserInfo
                {
                    CourierUserId = s.MerchantId,
                    Mobile = s.Mobile,
                    UserName = s.UserName,
                    Address = s.Address,
                    EmailAddress = s.EmailAddress,
                    CollectAddress = s.CollectAddress

                },
                AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                {

                    IsEmail = s.IsEmail,
                    IsSms = s.IsSms
                }
            });
            var amountResponse = await _sqlServerContext.CourierOrders.Where(h => h.MerchantId == loadCourierOrderBodyModel.CourierUserId).Select(b => new { b.CollectionAmount, b.Status }).ToListAsync();

            var responseData = new CourierOrderDetailsViewModel
            {
                TotalCount = data.CodCollectionTotal.CodCollectionTotalCount,
                AdTotalCollectionAmount = data.CodCollectionDetails.Select(c => c.CollectionAmount).Sum(),
                AdCourierPaymentInfo = new AdCourierPaymentInfo
                {
                    PaymentInProcessing = amountResponse.Where(n => n.Status == 15 || n.Status == 24).Select(v => v.CollectionAmount).Sum(),
                    PaymentPaid = amountResponse.Where(n => n.Status == 25).Select(v => v.CollectionAmount).Sum(),
                    PaymentReady = amountResponse.Where(n => n.Status == 28).Select(v => v.CollectionAmount).Sum()
                },
                CourierOrderViewModel = res.ToList()
            };
            return responseData;
        }
        public async Task<CourierAmountDetailsResponse> LoadCourierOrderAmountDetailsV2(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var data =  await _orderRepository.LoadCourierOrderAmountDetailsV2(loadCourierOrderBodyModel);

            var res = data.ServiceAmountDetails.Select(s => new CourierOrderAmountDetails
            {
                CustomerName = s.CustomerName,
                CourierOrdersId = s.CourierOrdersId,
                BreakableCharge = s.BreakableCharge,
                CollectionAmount = s.CollectionAmount,
                DeliveryCharge = s.DeliveryCharge,
                CODCharge = s.CodCharge,
                Status = s.StatusType,
                CollectionCharge = s.CollectionCharge,
                ReturnCharge = s.ReturnCharge
            }).ToList();

            var totalAmountOnlyDelivery = data.ServiceAmountDetails.Where(s => s.CollectionAmount.Equals(0)).Sum(x => x.BreakableCharge + x.CodCharge + x.DeliveryCharge + x.ReturnCharge + x.CollectionCharge);
            var totalAmountDeliveryTakaCollection = data.ServiceAmountDetails.Where(s => s.CollectionAmount != 0).Sum(x => x.BreakableCharge + x.CodCharge + x.DeliveryCharge + x.ReturnCharge + x.CollectionCharge);

            var sumOfTotalAdAmount = data.ServiceAmountDetails.Sum(x => x.BreakableCharge + x.CodCharge + x.DeliveryCharge + x.ReturnCharge + x.CollectionCharge);

            return new CourierAmountDetailsResponse
            {
                TotalData = data.ServiceAmountTotal.TotalCount,
                TotalAmountOnlyDelivery = totalAmountOnlyDelivery,
                TotalAmountDeliveryTakaCollection = totalAmountDeliveryTakaCollection,
                TotalAmount = sumOfTotalAdAmount,
                TotalDataCount = data.ServiceAmountDetails.Count(),
                CourierOrderAmountDetails = res
            };
        }
        

        public async Task<List<CourierOrderStatusHistory>> UpdateCourierOrderCollector(List<CourierOrderStatusHistoryViewModel> updateCourierOrderCollectorBodyModel)
        {
            var responseData = new List<CourierOrderStatusHistory>();
            int updateOrderStatus = 0;
            if (updateCourierOrderCollectorBodyModel.Count() > 0)
            {
                updateOrderStatus = await _orderRepository.UpdateCourierOrderCollector(updateCourierOrderCollectorBodyModel);
            }
            if (updateOrderStatus > 0)
            {
                responseData = updateCourierOrderCollectorBodyModel.Select(x => new CourierOrderStatusHistory()
                {
                    CourierOrderId = x.CourierOrderId,
                    IsConfirmedBy = x.IsConfirmedBy,
                    OrderDate = x.OrderDate,
                    Status = x.Status,
                    PostedBy = x.PostedBy,
                    MerchantId = x.MerchantId,
                    Comment = x.Comment,
                    PodNumber = x.PodNumber,
                    CourierId = x.CourierId
                }).ToList();

                responseData = await _orderHistoryRepository.AddListCourierOrderHistory(responseData);
            }
            return responseData;
        }

        private async Task<bool> SendNotification(CourierOrderStatusHistoryViewModel courierOrderStatusHistory)
        {
            bool res = false;
            var order =  _sqlServerContext.CourierOrders.Where(x => x.CourierOrdersId.Equals(courierOrderStatusHistory.CourierOrderId)).FirstOrDefault();
            if (order.DeliveryUserId > 0)
            {
                var status = _sqlServerContext.CourierOrderStatus.Where(x => x.StatusId.Equals(courierOrderStatusHistory.Status)).FirstOrDefault();
                if (status.FulfillmentStatusGroup.ToLower().Trim() == "deliverybondu" && status.IsActiveNotification)
                {
                    var deliveryUsers = _sqlServerContext.DeliveryUsers.Where(x => x.Id.Equals(order.DeliveryUserId)).FirstOrDefault();
                    res = await _firebaseCloudService.SendNotificationDeliveryBondhu(deliveryUsers.FirebaseToken, status);
                }
            }
            return res;
        }
        public async Task<CourierOrderStatusHistory> UpdateOrderHistory(string courierOrderId, CourierOrderStatusHistoryViewModel courierOrderStatusHistory)
        {
            int startIndex = 3;
            int endIndex = courierOrderId.Length - 3;
            int OrderId = Convert.ToInt32(courierOrderId.Substring(startIndex, endIndex));


            if (courierOrderStatusHistory.Status == 10)
            {
                var orderData = await _sqlServerContext.CourierOrders.Where(o => o.Id.Equals(OrderId)).Select(s => new CourierOrders
                {
                    Id = s.Id,
                    PaymentServiceType = s.PaymentServiceType,
                    PaymentServiceTypeVerify = s.PaymentServiceTypeVerify,
                    PaymentServiceTypeMerchantVerify = s.PaymentServiceTypeMerchantVerify
                }).FirstOrDefaultAsync();

                if (orderData.PaymentServiceType == 1 && string.IsNullOrEmpty(orderData.PaymentServiceTypeMerchantVerify) && string.IsNullOrEmpty(orderData.PaymentServiceTypeVerify))
                {
                    return new CourierOrderStatusHistory();

                }
                else if (orderData.PaymentServiceType == 1 && (string.IsNullOrEmpty(orderData.PaymentServiceTypeMerchantVerify) || string.IsNullOrEmpty(orderData.PaymentServiceTypeVerify)))
                {
                    return new CourierOrderStatusHistory();
                }
            }


            int updateOrderId = 0;
            if (!string.IsNullOrEmpty(courierOrderId))
            {
                updateOrderId = _orderRepository.UpdateOrderHistory(courierOrderId, courierOrderStatusHistory);
            }
            if (updateOrderId > 0)
            {
                if (courierOrderStatusHistory.Status.Equals(7) && courierOrderStatusHistory.ReferrerMobile != "")
                {
                    int referrerUpdate = await _orderRepository.UpdateReferrer(courierOrderStatusHistory.ReferrerMobile);
                }

                CourierOrderStatusHistory courierOrderStatusHistoryObj = new CourierOrderStatusHistory();
                courierOrderStatusHistoryObj.CourierOrderId = courierOrderStatusHistory.CourierOrderId;
                courierOrderStatusHistoryObj.IsConfirmedBy = courierOrderStatusHistory.IsConfirmedBy;
                courierOrderStatusHistoryObj.OrderDate = courierOrderStatusHistory.OrderDate;
                courierOrderStatusHistoryObj.Status = courierOrderStatusHistory.Status;
                courierOrderStatusHistoryObj.PostedBy = courierOrderStatusHistory.PostedBy;
                courierOrderStatusHistoryObj.MerchantId = courierOrderStatusHistory.MerchantId;
                courierOrderStatusHistoryObj.Comment = courierOrderStatusHistory.Comment;
                courierOrderStatusHistoryObj.PodNumber = courierOrderStatusHistory.PodNumber;
                courierOrderStatusHistoryObj.CourierId = courierOrderStatusHistory.CourierId;
                courierOrderStatusHistoryObj.HubName = courierOrderStatusHistory.HubName;

                var responseOrderHistory = await _orderHistoryRepository.AddCourierOrderHistory(courierOrderStatusHistoryObj);

                var user = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(x => x.CourierUserId.Equals(courierOrderStatusHistory.MerchantId));
                var order = await _sqlServerContext.CourierOrders.Where(o => o.Id.Equals(OrderId)).Select(s => new CourierOrders
                {
                    Id = s.Id,
                    ExpectedDeliveryDate = s.ExpectedDeliveryDate,
                    PaymentServiceType = s.PaymentServiceType,
                    CollectionAmount = s.CollectionAmount,
                    PaymentServiceTypeVerify = s.PaymentServiceTypeVerify,
                    PaymentServiceTypeMerchantVerify = s.PaymentServiceTypeMerchantVerify
                }).FirstOrDefaultAsync();


                if (courierOrderStatusHistory.Status.Equals(15) && user.AutoDownload)
                {
                    if (user.PaymentServiceType.Value.Equals(1) && order.CollectionAmount > 0)
                    {
                        //await _otherService.AutoDownload(courierOrderId);
                    }
                    else
                    {
                        await _otherService.AutoDownload(courierOrderId);
                    }
                }

                //<< ---Accounting Part(Poh Download)--Start--->>
                if (order.PaymentServiceType.Value.Equals(1) && order.PaymentServiceTypeVerify == "verify" && order.PaymentServiceTypeMerchantVerify == "verify")
                {
                    try
                    {
                        var accountingHit = new AccountingHit
                        {
                            OrderId = courierOrderId
                        };
                        await _sqlServerContext.AccountingHit.AddAsync(accountingHit);
                        await _sqlServerContext.SaveChangesAsync();

                        var status = user.AutoDownloadPohStatus.Split(',');
                        var statusArr = Array.ConvertAll(status, s => int.TryParse(s, out var i) ? i : 0);

                        if (statusArr.Contains(courierOrderStatusHistory.Status) && courierOrderStatusHistory.Status > 0)
                        {
                            bool responsePoh = await _otherService.DownloadPoh(courierOrderId);
                            if (responsePoh.Equals(true))
                            {
                                await _otherService.BlockCodCodesForPoh(courierOrderStatusHistory.MerchantId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                //<<---poh download---Ends---->>

                //var res = await SendNotification(courierOrderStatusHistory);

                //it will be used with abin's bhaiya confirmation
                //await GetSmsAndMailFormat(courierOrderId, courierOrderStatusHistory);
                int[] districtIds = { 4, 6, 12, 13, 22, 23, 40, 49, 56, 59, 63, 77, 78, 79, 80 };

                if (courierOrderStatusHistory.IsSms && courierOrderStatusHistory.MessageFormat.Trim() != "")
                {
                    if (districtIds.Contains(courierOrderStatusHistory.DistrictId) 
                        && courierOrderStatusHistory.CourierId == 32
                        && courierOrderStatusHistoryObj.Status == 37)
                    {
                        string dtorderid = "dt-orderid", dtmerchantname = "dt-merchantname",
                        dtcompanyname = "dt-companyname", dtpodnumber = "dt-podnumber",
                         dtcouriername = "dt-couriername", dtcustomername = "dt-customername", dtcustomermobile = "dt-customermobile",
                         dtdeliverymanname = "dt-courier-deliveryman-name", dtdeliverymanmobile = "dt-courier-deliveryman-mobile",
                         dtbkashnumber = "dt-bkashnumber", dtmerchantpaymentamount = "dt-merchantpaymentamount",
                         dtdistrictname = "dt-districtname", dtthananame = "dt-thananame",
                         dtareaname = "dt-areaname", dtridermobile = "dt-ridermobile",
                         dtcdddate = "dt-cdddate",
                         dtedeshmobileno = "dt-edeshmobileno";

                        SendMobileBodyModel merchantSmsBody = new SendMobileBodyModel
                        {
                            numbers = new string[] { courierOrderStatusHistory.MerchantMobile.Trim() },
                            text = courierOrderStatusHistory.MessageFormat.Trim()
                            .Replace(dtorderid, courierOrderStatusHistory.CourierOrderId)
                            .Replace(dtmerchantname, courierOrderStatusHistory.MerchantName)
                            .Replace(dtcompanyname, courierOrderStatusHistory.CompanyName)
                            .Replace(dtpodnumber, courierOrderStatusHistory.PodNumber)
                            .Replace(dtcouriername, courierOrderStatusHistory.CourierName)
                            .Replace(dtcustomername, courierOrderStatusHistory.CustomerName)
                            .Replace(dtcustomermobile, courierOrderStatusHistory.CustomerMobile)
                            .Replace(dtdeliverymanname, courierOrderStatusHistory.CourierDeliveryManName)
                            .Replace(dtdeliverymanmobile, courierOrderStatusHistory.CourierDeliveryManMobile)
                            .Replace(dtbkashnumber, courierOrderStatusHistory.BkashNumber)
                            .Replace(dtmerchantpaymentamount, courierOrderStatusHistory.MerchantPaymentAmount)
                            .Replace(dtdistrictname, courierOrderStatusHistory.DistrictName)
                            .Replace(dtthananame, courierOrderStatusHistory.ThanaName)
                            .Replace(dtareaname, courierOrderStatusHistory.AreaName)
                            .Replace(dtridermobile, courierOrderStatusHistory.RiderMobile)
                            .Replace(dtcdddate, order.ExpectedDeliveryDate.HasValue ? order.ExpectedDeliveryDate.Value.ToString("MM/dd/yyyy") : string.Empty)
                            .Replace(dtedeshmobileno, courierOrderStatusHistory.EDeshMobileNo),
                            datacoding = 0,
                            type = 0
                        };
                        var listOfData = new List<SendMobileBodyModel>();
                        listOfData.Add(merchantSmsBody);
                        bool isActive = await _smsEmailService.SmsSend(listOfData);
                    }
                    else
                    {
                        string dtorderid = "dt-orderid", dtmerchantname = "dt-merchantname",
                        dtcompanyname = "dt-companyname", dtpodnumber = "dt-podnumber",
                         dtcouriername = "dt-couriername", dtcustomername = "dt-customername", dtcustomermobile = "dt-customermobile",
                         dtdeliverymanname = "dt-courier-deliveryman-name", dtdeliverymanmobile = "dt-courier-deliveryman-mobile",
                         dtbkashnumber = "dt-bkashnumber", dtmerchantpaymentamount = "dt-merchantpaymentamount",
                         dtdistrictname = "dt-districtname", dtthananame = "dt-thananame",
                         dtareaname = "dt-areaname", dtridermobile = "dt-ridermobile",
                         dtcdddate = "dt-cdddate";

                        SendMobileBodyModel merchantSmsBody = new SendMobileBodyModel
                        {
                            numbers = new string[] { courierOrderStatusHistory.MerchantMobile.Trim() },
                            text = courierOrderStatusHistory.MessageFormat.Trim()
                            .Replace(dtorderid, courierOrderStatusHistory.CourierOrderId)
                            .Replace(dtmerchantname, courierOrderStatusHistory.MerchantName)
                            .Replace(dtcompanyname, courierOrderStatusHistory.CompanyName)
                            .Replace(dtpodnumber, courierOrderStatusHistory.PodNumber)
                            .Replace(dtcouriername, courierOrderStatusHistory.CourierName)
                            .Replace(dtcustomername, courierOrderStatusHistory.CustomerName)
                            .Replace(dtcustomermobile, courierOrderStatusHistory.CustomerMobile)
                            .Replace(dtdeliverymanname, courierOrderStatusHistory.CourierDeliveryManName)
                            .Replace(dtdeliverymanmobile, courierOrderStatusHistory.CourierDeliveryManMobile)
                            .Replace(dtbkashnumber, courierOrderStatusHistory.BkashNumber)
                            .Replace(dtmerchantpaymentamount, courierOrderStatusHistory.MerchantPaymentAmount)
                            .Replace(dtdistrictname, courierOrderStatusHistory.DistrictName)
                            .Replace(dtthananame, courierOrderStatusHistory.ThanaName)
                            .Replace(dtareaname, courierOrderStatusHistory.AreaName)
                            .Replace(dtridermobile, courierOrderStatusHistory.RiderMobile)
                            .Replace(dtcdddate, order.ExpectedDeliveryDate.HasValue ? order.ExpectedDeliveryDate.Value.ToString("MM/dd/yyyy") : string.Empty),
                            datacoding = 0,
                            type = 0
                        };
                        var listOfData = new List<SendMobileBodyModel>();
                        listOfData.Add(merchantSmsBody);
                        bool isActive = await _smsEmailService.SmsSend(listOfData);
                    }

                    
                }

                if (courierOrderStatusHistory.RetentionUserMobile.Trim() != "" 
                    && courierOrderStatusHistory.RetentionMessageFormat.Trim() != "")
                {
                    string dtorderid = "dt-orderid", dtmerchantname = "dt-merchantname",
                        dtcompanyname = "dt-companyname", dtpodnumber = "dt-podnumber",
                         dtcouriername = "dt-couriername", dtcustomername = "dt-customername",
                         dtbkashnumber = "dt-bkashnumber", dtmerchantpaymentamount = "dt-merchantpaymentamount",
                         dtdistrictname = "dt-districtname", dtthananame = "dt-thananame",
                         dtareaname = "dt-areaname", dtridermobile = "dt-ridermobile",
                         dtcdddate = "dt-cdddate";

                    SendMobileBodyModel retentionSmsBody = new SendMobileBodyModel
                    {
                        numbers = new string[] { courierOrderStatusHistory.RetentionUserMobile.Trim() },
                        text = courierOrderStatusHistory.RetentionMessageFormat.Trim()
                        .Replace(dtorderid, courierOrderStatusHistory.CourierOrderId)
                        .Replace(dtmerchantname, courierOrderStatusHistory.MerchantName)
                        .Replace(dtcompanyname, courierOrderStatusHistory.CompanyName)
                        .Replace(dtpodnumber, courierOrderStatusHistory.PodNumber)
                        .Replace(dtcouriername, courierOrderStatusHistory.CourierName)
                        .Replace(dtcustomername, courierOrderStatusHistory.CustomerName)
                        .Replace(dtbkashnumber, courierOrderStatusHistory.BkashNumber)
                        .Replace(dtmerchantpaymentamount, courierOrderStatusHistory.MerchantPaymentAmount)
                        .Replace(dtdistrictname, courierOrderStatusHistory.DistrictName)
                        .Replace(dtthananame, courierOrderStatusHistory.ThanaName)
                        .Replace(dtareaname, courierOrderStatusHistory.AreaName)
                        .Replace(dtridermobile, courierOrderStatusHistory.RiderMobile)
                        .Replace(dtcdddate, order.ExpectedDeliveryDate.HasValue ? order.ExpectedDeliveryDate.Value.ToString("MM/dd/yyyy") : string.Empty),
                        datacoding = 0,
                        type = 0
                    };
                    var listOfData = new List<SendMobileBodyModel>();
                    listOfData.Add(retentionSmsBody);
                    bool isActive = await _smsEmailService.SmsSend(listOfData);
                }

                if (courierOrderStatusHistory.IsCustomerSms && courierOrderStatusHistory.CustomerMessageFormat.Trim()
                    != "")
                {
                    if (districtIds.Contains(courierOrderStatusHistory.DistrictId) 
                        && courierOrderStatusHistory.CourierId == 32
                        && courierOrderStatusHistoryObj.Status == 37)
                    {
                        string dtorderid = "dt-orderid", dtmerchantname = "dt-merchantname",
                        dtcompanyname = "dt-companyname", dtpodnumber = "dt-podnumber",
                         dtcouriername = "dt-couriername", dtcustomername = "dt-customername", dtcustomermobile = "dt-customermobile",
                         dtdeliverymanname = "dt-courier-deliveryman-name", dtdeliverymanmobile = "dt-courier-deliveryman-mobile",
                         dtbkashnumber = "dt-bkashnumber", dtmerchantpaymentamount = "dt-merchantpaymentamount",
                         dtdistrictname = "dt-districtname", dtthananame = "dt-thananame",
                         dtareaname = "dt-areaname", dtridermobile = "dt-ridermobile",
                         dtcdddate = "dt-cdddate",
                         dtedeshmobileno = "dt-edeshmobileno";

                        SendMobileBodyModel customerSmsBody = new SendMobileBodyModel
                        {
                            numbers = new string[] { courierOrderStatusHistory.CustomerMobile.Trim() },
                            text = courierOrderStatusHistory.CustomerMessageFormat.Trim()
                            .Replace(dtorderid, courierOrderStatusHistory.CourierOrderId)
                            .Replace(dtmerchantname, courierOrderStatusHistory.MerchantName)
                            .Replace(dtcompanyname, courierOrderStatusHistory.CompanyName)
                            .Replace(dtpodnumber, courierOrderStatusHistory.PodNumber)
                            .Replace(dtcouriername, courierOrderStatusHistory.CourierName)
                            .Replace(dtcustomername, courierOrderStatusHistory.CustomerName)
                            .Replace(dtcustomermobile, courierOrderStatusHistory.CustomerMobile)
                            .Replace(dtdeliverymanname, courierOrderStatusHistory.CourierDeliveryManName)
                            .Replace(dtdeliverymanmobile, courierOrderStatusHistory.CourierDeliveryManMobile)
                            .Replace(dtbkashnumber, courierOrderStatusHistory.BkashNumber)
                            .Replace(dtmerchantpaymentamount, courierOrderStatusHistory.MerchantPaymentAmount)
                            .Replace(dtdistrictname, courierOrderStatusHistory.DistrictName)
                            .Replace(dtthananame, courierOrderStatusHistory.ThanaName)
                            .Replace(dtareaname, courierOrderStatusHistory.AreaName)
                            .Replace(dtridermobile, courierOrderStatusHistory.RiderMobile)
                            .Replace(dtcdddate, order.ExpectedDeliveryDate.HasValue ? order.ExpectedDeliveryDate.Value.ToString("MM/dd/yyyy") : string.Empty)
                            .Replace(dtedeshmobileno, courierOrderStatusHistory.EDeshMobileNo),
                            datacoding = 0,
                            type = 0
                        };
                        var listOfData = new List<SendMobileBodyModel>();
                        listOfData.Add(customerSmsBody);
                        bool isActive = await _smsEmailService.SmsSend(listOfData);
                    }
                    else
                    {
                        string dtorderid = "dt-orderid", dtmerchantname = "dt-merchantname",
                        dtcompanyname = "dt-companyname", dtpodnumber = "dt-podnumber",
                         dtcouriername = "dt-couriername", dtcustomername = "dt-customername", dtcustomermobile = "dt-customermobile",
                         dtdeliverymanname = "dt-courier-deliveryman-name", dtdeliverymanmobile = "dt-courier-deliveryman-mobile",
                         dtbkashnumber = "dt-bkashnumber", dtmerchantpaymentamount = "dt-merchantpaymentamount",
                         dtdistrictname = "dt-districtname", dtthananame = "dt-thananame",
                         dtareaname = "dt-areaname", dtridermobile = "dt-ridermobile",
                         dtcdddate = "dt-cdddate";

                        SendMobileBodyModel customerSmsBody = new SendMobileBodyModel
                        {
                            numbers = new string[] { courierOrderStatusHistory.CustomerMobile.Trim() },
                            text = courierOrderStatusHistory.CustomerMessageFormat.Trim()
                            .Replace(dtorderid, courierOrderStatusHistory.CourierOrderId)
                            .Replace(dtmerchantname, courierOrderStatusHistory.MerchantName)
                            .Replace(dtcompanyname, courierOrderStatusHistory.CompanyName)
                            .Replace(dtpodnumber, courierOrderStatusHistory.PodNumber)
                            .Replace(dtcouriername, courierOrderStatusHistory.CourierName)
                            .Replace(dtcustomername, courierOrderStatusHistory.CustomerName)
                            .Replace(dtcustomermobile, courierOrderStatusHistory.CustomerMobile)
                            .Replace(dtdeliverymanname, courierOrderStatusHistory.CourierDeliveryManName)
                            .Replace(dtdeliverymanmobile, courierOrderStatusHistory.CourierDeliveryManMobile)
                            .Replace(dtbkashnumber, courierOrderStatusHistory.BkashNumber)
                            .Replace(dtmerchantpaymentamount, courierOrderStatusHistory.MerchantPaymentAmount)
                            .Replace(dtdistrictname, courierOrderStatusHistory.DistrictName)
                            .Replace(dtthananame, courierOrderStatusHistory.ThanaName)
                            .Replace(dtareaname, courierOrderStatusHistory.AreaName)
                            .Replace(dtridermobile, courierOrderStatusHistory.RiderMobile)
                            .Replace(dtcdddate, order.ExpectedDeliveryDate.HasValue ? order.ExpectedDeliveryDate.Value.ToString("MM/dd/yyyy") : string.Empty),
                            datacoding = 0,
                            type = 0
                        };
                        var listOfData = new List<SendMobileBodyModel>();
                        listOfData.Add(customerSmsBody);
                        bool isActive = await _smsEmailService.SmsSend(listOfData);
                    }
                        
                }

                //if (courierOrderStatusHistory.IsEmail && courierOrderStatusHistory.EmailFormat.Trim() != ""
                //    && courierOrderStatusHistory.MerchantEmail != "")
                //{

                //    string dtorderid = "dt-orderid", dtmerchantname = "dt-merchantname",
                //        dtcompanyname = "dt-companyname", dtpodnumber = "dt-podnumber",
                //         dtcouriername = "dt-couriername", dtcustomername = "dt-customername",
                //         dtbkashnumber = "dt-bkashnumber", dtmerchantpaymentamount = "dt-merchantpaymentamount",
                //         dtdistrictname = "dt-districtname", dtthananame = "dt-thananame",
                //         dtareaname = "dt-areaname";

                //    string mailBody = courierOrderStatusHistory.EmailFormat.Trim()
                //        .Replace(dtorderid, courierOrderStatusHistory.CourierOrderId)
                //        .Replace(dtmerchantname, courierOrderStatusHistory.MerchantName)
                //        .Replace(dtcompanyname, courierOrderStatusHistory.CompanyName)
                //        .Replace(dtpodnumber, courierOrderStatusHistory.PodNumber)
                //        .Replace(dtcouriername, courierOrderStatusHistory.CourierName)
                //        .Replace(dtcustomername, courierOrderStatusHistory.CustomerName)
                //        .Replace(dtbkashnumber, courierOrderStatusHistory.BkashNumber)
                //        .Replace(dtmerchantpaymentamount, courierOrderStatusHistory.MerchantPaymentAmount)
                //        .Replace(dtdistrictname, courierOrderStatusHistory.DistrictName)
                //        .Replace(dtthananame, courierOrderStatusHistory.ThanaName)
                //        .Replace(dtareaname, courierOrderStatusHistory.AreaName);

                //    //await _smtpClient.SendMailAsync(new MailMessage(
                //    //    from: "info@deliverytiger.com.bd",
                //    //    to: courierOrderStatusHistory.MerchantEmail,
                //    //    subject: "Test Delivery Tiger",
                //    //    body: mailBody
                //    //    ));

                //    try
                //    {
                //        var mail = new MailMessage()
                //        {
                //            From = new MailAddress("info@deliverytiger.com.bd"),
                //            Subject = "Delivery Tiger",
                //            Body = mailBody
                //        };
                //        mail.IsBodyHtml = true;
                //        mail.To.Add(new MailAddress(courierOrderStatusHistory.MerchantEmail));
                //        await _smtpClient.SendMailAsync(mail);

                //    }
                //    catch (Exception ex)
                //    {
                //        //Log.Error(ex, "merchant mail exception");
                //        //Log.CloseAndFlush();
                //    }
                //}

                //if (courierOrderStatusHistory.IsCustomerEmail && courierOrderStatusHistory.CustomerEmailFormat.Trim() != ""
                //    && courierOrderStatusHistory.CustomerEmail != "")
                //{

                //    string dtorderid = "dt-orderid", dtmerchantname = "dt-merchantname",
                //        dtcompanyname = "dt-companyname", dtpodnumber = "dt-podnumber",
                //         dtcouriername = "dt-couriername", dtcustomername = "dt-customername",
                //         dtbkashnumber = "dt-bkashnumber", dtmerchantpaymentamount = "dt-merchantpaymentamount",
                //         dtdistrictname = "dt-districtname", dtthananame = "dt-thananame",
                //         dtareaname = "dt-areaname";

                //    string mailBody = courierOrderStatusHistory.CustomerEmailFormat.Trim()
                //        .Replace(dtorderid, courierOrderStatusHistory.CourierOrderId)
                //        .Replace(dtmerchantname, courierOrderStatusHistory.MerchantName)
                //        .Replace(dtcompanyname, courierOrderStatusHistory.CompanyName)
                //        .Replace(dtpodnumber, courierOrderStatusHistory.PodNumber)
                //        .Replace(dtcouriername, courierOrderStatusHistory.CourierName)
                //        .Replace(dtcustomername, courierOrderStatusHistory.CustomerName)
                //        .Replace(dtbkashnumber, courierOrderStatusHistory.BkashNumber)
                //        .Replace(dtmerchantpaymentamount, courierOrderStatusHistory.MerchantPaymentAmount)
                //        .Replace(dtdistrictname, courierOrderStatusHistory.DistrictName)
                //        .Replace(dtthananame, courierOrderStatusHistory.ThanaName)
                //        .Replace(dtareaname, courierOrderStatusHistory.AreaName);

                //    try
                //    {
                //        var mail = new MailMessage()
                //        {
                //            From = new MailAddress("info@deliverytiger.com.bd"),
                //            Subject = "Delivery Tiger",
                //            Body = mailBody
                //        };
                //        mail.IsBodyHtml = true;
                //        mail.To.Add(new MailAddress(courierOrderStatusHistory.CustomerEmail));
                //        await _smtpClient.SendMailAsync(mail);

                //    }
                //    catch (Exception ex)
                //    {
                //        Log.Error(ex, "mail exception");
                //        Log.CloseAndFlush();
                //    }
                //}
            }

            return courierOrderStatusHistory;
        }
        public async Task<CourierUsersInfoViewModel> GetCourierUsersInfo(SearchBodyModel searchBody)
        {
            return await _orderRepository.GetCourierUsersInfo(searchBody);
        }

        public async Task<dynamic> GetCourierUserInfo(int courierUserId)
        {
            return await _orderRepository.GetCourierUserInfo(courierUserId);
        }

        public async Task<CourierOrderDetailsViewModel> LoadCourierReport(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            return await _orderRepository.LoadCourierReport(loadCourierOrderBodyModel);
        }

        public async Task<IEnumerable<Districts>> LoadAllDistricts()
        {
            //if (_redisModel.IsActive)
            //{
            //    string key = "LoadAllDistricts";

            //    if (await _redis.Db1.Database.KeyExistsAsync(key) == true)
            //    {
            //        return await _redis.Db1.GetAsync<IEnumerable<Districts>>(key);
            //    }
            //    else
            //    {
            //        var data = await _orderRepository.LoadAllDistricts();
            //        bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddDays(7));
            //        return data;
            //    }
            //}
            //else
            //{
            //    return await _orderRepository.LoadAllDistricts();
            //}

            return await _orderRepository.LoadAllDistricts();
        }


        public async Task<IEnumerable<Districts>> LoadAllDistrictsById(int id)
        {
            return await _orderRepository.LoadAllDistrictsById(id);
        }

        public async Task<int> UpdateOrdersBulk(List<CourierOrders> courierOrders)
        {
            return await _orderRepository.UpdateOrdersBulk(courierOrders);
        }

        public async Task<int> UpdateOrderInformation(List<CourierOrders> courierOrders)
        {
            var data =  await _orderRepository.UpdateOrderInformation(courierOrders);

            var orderHistory = new List<CourierOrderStatusHistory>();

            foreach (var item in data)
            {
                orderHistory.Add(new CourierOrderStatusHistory
                {
                    OrderDate = item.OrderDate,
                    CourierOrderId = item.CourierOrdersId,
                    Status = item.Status,
                    PostedBy = item.UpdatedBy,
                    MerchantId = item.MerchantId,
                    Comment = item.Comment,
                    IsConfirmedBy = item.IsConfirmedBy,
                    CourierId = item.CourierId,
                    PodNumber = item.PodNumber,
                    HubName = item.HubName,
                    PostedOn = item.UpdatedOn
                });
            }

            var resOrderHistory = await _orderRepository.AddCourierOrderHistoryBulk(orderHistory);

            return resOrderHistory.Count();
        }

        public async Task<CourierOrderDetailsViewModel> LoadOrders(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var orderModel = new List<OrderModel>().AsQueryable();
            int totalCount = 0;
            
            var orderIdsArr = loadCourierOrderBodyModel.OrderIds.Trim().Split(',');

            if (loadCourierOrderBodyModel.Priority.ToLower() == "priority")
            {
                var data = await _orderRepository.GetPriorityOrders();
                orderModel = data.AsQueryable();
            }

            // Date PaymentType
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType != "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.PaymentType.Equals(loadCourierOrderBodyModel.PaymentType)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.PaymentType.Equals(loadCourierOrderBodyModel.PaymentType)

                );
            }
            // Date Status PaymentType
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType != "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.PaymentType.Equals(loadCourierOrderBodyModel.PaymentType)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.PaymentType.Equals(loadCourierOrderBodyModel.PaymentType)

                );
            }


            // Date Status OrderFrom
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom != "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.OrderFrom.Equals(loadCourierOrderBodyModel.OrderFrom)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.OrderFrom.Equals(loadCourierOrderBodyModel.OrderFrom)

                );
            }


            // Date and OrderFrom
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom != "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.OrderFrom.Equals(loadCourierOrderBodyModel.OrderFrom)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.OrderFrom.Equals(loadCourierOrderBodyModel.OrderFrom)

                );
            }

            // Date District status
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.PodNumber == ""
               && loadCourierOrderBodyModel.Mobile == ""
               && loadCourierOrderBodyModel.Status != -1
               && loadCourierOrderBodyModel.CourierUserId < 0
               && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
               && loadCourierOrderBodyModel.StatusList.Contains(-1)
               && loadCourierOrderBodyModel.OrderIds == ""
               && loadCourierOrderBodyModel.CollectionName == ""
               && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
               && loadCourierOrderBodyModel.DistrictGroupName != ""
               && loadCourierOrderBodyModel.CourierId == 0
               && loadCourierOrderBodyModel.DistrictId != 0
               && loadCourierOrderBodyModel.ThanaId == 0
               && loadCourierOrderBodyModel.AreaId == 0
               && loadCourierOrderBodyModel.PaymentType == "-1"
               && loadCourierOrderBodyModel.OrderFrom == "-1"
               && loadCourierOrderBodyModel.LowPrice == 0
               && loadCourierOrderBodyModel.HighPrice == 0
               && loadCourierOrderBodyModel.MinWeight == 0
               && loadCourierOrderBodyModel.MaxWeight == 0
               )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.Status.Equals(loadCourierOrderBodyModel.Status)

                );
            }


            // Date District thana status
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.PodNumber == ""
               && loadCourierOrderBodyModel.Mobile == ""
               && loadCourierOrderBodyModel.Status != -1
               && loadCourierOrderBodyModel.CourierUserId < 0
               && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
               && loadCourierOrderBodyModel.StatusList.Contains(-1)
               && loadCourierOrderBodyModel.OrderIds == ""
               && loadCourierOrderBodyModel.CollectionName == ""
               && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
               && loadCourierOrderBodyModel.DistrictGroupName != ""
               && loadCourierOrderBodyModel.CourierId == 0
               && loadCourierOrderBodyModel.DistrictId != 0
               && loadCourierOrderBodyModel.ThanaId != 0
               && loadCourierOrderBodyModel.AreaId == 0
               && loadCourierOrderBodyModel.PaymentType == "-1"
               && loadCourierOrderBodyModel.OrderFrom == "-1"
               && loadCourierOrderBodyModel.LowPrice == 0
               && loadCourierOrderBodyModel.HighPrice == 0
               && loadCourierOrderBodyModel.MinWeight == 0
               && loadCourierOrderBodyModel.MaxWeight == 0
               )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)
                && x.Status.Equals(loadCourierOrderBodyModel.Status)

                );
            }

            // Date District
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.PodNumber == ""
               && loadCourierOrderBodyModel.Mobile == ""
               && loadCourierOrderBodyModel.Status == -1
               && loadCourierOrderBodyModel.CourierUserId < 0
               && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
               && loadCourierOrderBodyModel.StatusList.Contains(-1)
               && loadCourierOrderBodyModel.OrderIds == ""
               && loadCourierOrderBodyModel.CollectionName == ""
               && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
               && loadCourierOrderBodyModel.DistrictGroupName != ""
               && loadCourierOrderBodyModel.CourierId == 0
               && loadCourierOrderBodyModel.DistrictId != 0
               && loadCourierOrderBodyModel.ThanaId == 0
               && loadCourierOrderBodyModel.AreaId == 0
               && loadCourierOrderBodyModel.PaymentType == "-1"
               && loadCourierOrderBodyModel.OrderFrom == "-1"
               && loadCourierOrderBodyModel.LowPrice == 0
               && loadCourierOrderBodyModel.HighPrice == 0
               && loadCourierOrderBodyModel.MinWeight == 0
               && loadCourierOrderBodyModel.MaxWeight == 0
               )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)

                );
            }

            // Date District Thana
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.PodNumber == ""
               && loadCourierOrderBodyModel.Mobile == ""
               && loadCourierOrderBodyModel.Status == -1
               && loadCourierOrderBodyModel.CourierUserId < 0
               && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
               && loadCourierOrderBodyModel.StatusList.Contains(-1)
               && loadCourierOrderBodyModel.OrderIds == ""
               && loadCourierOrderBodyModel.CollectionName == ""
               && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
               && loadCourierOrderBodyModel.DistrictGroupName != ""
               && loadCourierOrderBodyModel.CourierId == 0
               && loadCourierOrderBodyModel.DistrictId != 0
               && loadCourierOrderBodyModel.ThanaId != 0
               && loadCourierOrderBodyModel.AreaId == 0
               && loadCourierOrderBodyModel.PaymentType == "-1"
               && loadCourierOrderBodyModel.OrderFrom == "-1"
               && loadCourierOrderBodyModel.LowPrice == 0
               && loadCourierOrderBodyModel.HighPrice == 0
               && loadCourierOrderBodyModel.MinWeight == 0
               && loadCourierOrderBodyModel.MaxWeight == 0
               )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)

                );
            }

            // Date District Thana Area
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.PodNumber == ""
               && loadCourierOrderBodyModel.Mobile == ""
               && loadCourierOrderBodyModel.Status == -1
               && loadCourierOrderBodyModel.CourierUserId < 0
               && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
               && loadCourierOrderBodyModel.StatusList.Contains(-1)
               && loadCourierOrderBodyModel.OrderIds == ""
               && loadCourierOrderBodyModel.CollectionName == ""
               && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
               && loadCourierOrderBodyModel.DistrictGroupName != ""
               && loadCourierOrderBodyModel.CourierId == 0
               && loadCourierOrderBodyModel.DistrictId != 0
               && loadCourierOrderBodyModel.ThanaId != 0
               && loadCourierOrderBodyModel.AreaId != 0
               && loadCourierOrderBodyModel.PaymentType == "-1"
               && loadCourierOrderBodyModel.OrderFrom == "-1"
               && loadCourierOrderBodyModel.LowPrice == 0
               && loadCourierOrderBodyModel.HighPrice == 0
               && loadCourierOrderBodyModel.MinWeight == 0
               && loadCourierOrderBodyModel.MaxWeight == 0
               )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)
                && x.AreaId.Equals(loadCourierOrderBodyModel.AreaId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)
                && x.AreaId.Equals(loadCourierOrderBodyModel.AreaId)

                );
            }

            // Date Status District
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId != 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)

                );
            }

            // Date Status District Thana
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.PodNumber == ""
               && loadCourierOrderBodyModel.Mobile == ""
               && loadCourierOrderBodyModel.Status != -1
               && loadCourierOrderBodyModel.CourierUserId < 0
               && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
               && loadCourierOrderBodyModel.StatusList.Contains(-1)
               && loadCourierOrderBodyModel.OrderIds == ""
               && loadCourierOrderBodyModel.CollectionName == ""
               && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
               && loadCourierOrderBodyModel.DistrictGroupName != ""
               && loadCourierOrderBodyModel.CourierId == 0
               && loadCourierOrderBodyModel.DistrictId != 0
               && loadCourierOrderBodyModel.ThanaId != 0
               && loadCourierOrderBodyModel.AreaId == 0
               && loadCourierOrderBodyModel.PaymentType == "-1"
               && loadCourierOrderBodyModel.OrderFrom == "-1"
               && loadCourierOrderBodyModel.LowPrice == 0
               && loadCourierOrderBodyModel.HighPrice == 0
               && loadCourierOrderBodyModel.MinWeight == 0
               && loadCourierOrderBodyModel.MaxWeight == 0
               )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)

                );
            }

            // Date Status District Thana Area
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
               && loadCourierOrderBodyModel.PodNumber == ""
               && loadCourierOrderBodyModel.Mobile == ""
               && loadCourierOrderBodyModel.Status != -1
               && loadCourierOrderBodyModel.CourierUserId < 0
               && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
               && loadCourierOrderBodyModel.StatusList.Contains(-1)
               && loadCourierOrderBodyModel.OrderIds == ""
               && loadCourierOrderBodyModel.CollectionName == ""
               && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
               && loadCourierOrderBodyModel.DistrictGroupName != ""
               && loadCourierOrderBodyModel.CourierId == 0
               && loadCourierOrderBodyModel.DistrictId != 0
               && loadCourierOrderBodyModel.ThanaId != 0
               && loadCourierOrderBodyModel.AreaId != 0
               && loadCourierOrderBodyModel.PaymentType == "-1"
               && loadCourierOrderBodyModel.OrderFrom == "-1"
               && loadCourierOrderBodyModel.LowPrice == 0
               && loadCourierOrderBodyModel.HighPrice == 0
               && loadCourierOrderBodyModel.MinWeight == 0
               && loadCourierOrderBodyModel.MaxWeight == 0
               )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)
                && x.AreaId.Equals(loadCourierOrderBodyModel.AreaId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.DistrictId.Equals(loadCourierOrderBodyModel.DistrictId)
                && x.ThanaId.Equals(loadCourierOrderBodyModel.ThanaId)
                && x.AreaId.Equals(loadCourierOrderBodyModel.AreaId)

                );
            }

            // 
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001" 
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001" 
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date);
            }

            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                
                );
            }

            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId > 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.MerchantId.Equals(loadCourierOrderBodyModel.CourierUserId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.MerchantId.Equals(loadCourierOrderBodyModel.CourierUserId)
                );
            }

            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.CourierUserId > 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.MerchantId.Equals(loadCourierOrderBodyModel.CourierUserId)

                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                && x.MerchantId.Equals(loadCourierOrderBodyModel.CourierUserId)
                );
            }

            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.CourierUserId > 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(
                x => x.MerchantId.Equals(loadCourierOrderBodyModel.CourierUserId)
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(
                 x => x.MerchantId.Equals(loadCourierOrderBodyModel.CourierUserId)
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                );
            }

            // customer Mobile
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile != ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {

                orderModel = await _orderGenericRepository.FindByAsyc(
                    x => x.Mobile.Equals(loadCourierOrderBodyModel.Mobile.Trim())
                , null,
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(
                    x => x.Mobile.Equals(loadCourierOrderBodyModel.Mobile.Trim())
                );
            }

            // OrderIds
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds != ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                List<int> orderIdsArrayList = new List<int>();

                int startIndex = 3;
                int endIndex = 0;
                foreach (var item in orderIdsArr)
                {
                    endIndex = item.Length - 3;
                    orderIdsArrayList.Add(Convert.ToInt32(item.Substring(startIndex, endIndex)));
                }

                orderModel = await _orderGenericRepository.FindByAsyc(
                      //x => x.CourierOrdersId.Equals(loadCourierOrderBodyModel.OrderIds)
                      x => orderIdsArrayList.Contains(x.Id)
                //x => orderIdsArr.Contains(x.CourierOrdersId)
                , null,
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(
                    x => orderIdsArrayList.Contains(x.Id)
                //x => x.CourierOrdersId.Equals(loadCourierOrderBodyModel.OrderIds)
                //x => orderIdsArr.Contains(x.CourierOrdersId)
                );
            }

            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber != ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(
                    x => x.PodNumber.Equals(loadCourierOrderBodyModel.PodNumber)
                , null,
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(
                    x => x.PodNumber.Equals(loadCourierOrderBodyModel.PodNumber)
                );
            }

            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() == "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName != ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(
                    x => x.CollectionName.Equals(loadCourierOrderBodyModel.CollectionName)
                , null,
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(
                    x => x.CollectionName.Equals(loadCourierOrderBodyModel.CollectionName)
                );
            }


            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && !loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && (loadCourierOrderBodyModel.DistrictGroupName.ToLower() == "dhaka city"
                    || loadCourierOrderBodyModel.DistrictGroupName.ToLower() == "other city"
                    )
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && loadCourierOrderBodyModel.DistrictIds.Contains(x.DistrictId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && loadCourierOrderBodyModel.DistrictIds.Contains(x.DistrictId));
            }

            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && !loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName.ToLower() == "all districts shador"
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && loadCourierOrderBodyModel.DistrictIds.Contains(x.ThanaId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && loadCourierOrderBodyModel.DistrictIds.Contains(x.ThanaId));
            }

            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && !loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName.ToLower() == "all upozilla shador"
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && loadCourierOrderBodyModel.DistrictIds.Contains(x.AreaId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && loadCourierOrderBodyModel.DistrictIds.Contains(x.AreaId));
            }

            ///////////////////////CourierId and Date Filter/////////////////
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.CourierId != 0
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.CourierId.Equals(loadCourierOrderBodyModel.CourierId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.CourierId.Equals(loadCourierOrderBodyModel.CourierId));
            }

            ///////////////////////CourierId and MerchantId and Date Filter/////////////////
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.CourierId != 0
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId > 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.CourierId.Equals(loadCourierOrderBodyModel.CourierId)
                && x.MerchantId.Equals(loadCourierOrderBodyModel.CourierUserId)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.CourierId.Equals(loadCourierOrderBodyModel.CourierId)
                && x.MerchantId.Equals(loadCourierOrderBodyModel.CourierUserId));
            }


            ///////////////////////CourierId and Status and Date Filter/////////////////
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.CourierId != 0
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.CourierId.Equals(loadCourierOrderBodyModel.CourierId)
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.CourierId.Equals(loadCourierOrderBodyModel.CourierId)
                && x.Status.Equals(loadCourierOrderBodyModel.Status));
            }

            //Low and High price and Status and Date Filter
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice > 0
                && loadCourierOrderBodyModel.HighPrice > 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.ActualPackagePrice >= loadCourierOrderBodyModel.LowPrice
                && x.ActualPackagePrice <= loadCourierOrderBodyModel.HighPrice
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.ActualPackagePrice >= loadCourierOrderBodyModel.LowPrice
                && x.ActualPackagePrice <= loadCourierOrderBodyModel.HighPrice
                && x.Status.Equals(loadCourierOrderBodyModel.Status));
            }

            //Weight Range and Status and Date Filter
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status != -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice == 0
                && loadCourierOrderBodyModel.HighPrice == 0
                && loadCourierOrderBodyModel.MinWeight > 0
                && loadCourierOrderBodyModel.MaxWeight > 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.WeightNumber >= loadCourierOrderBodyModel.MinWeight
                && x.WeightNumber <= loadCourierOrderBodyModel.MaxWeight
                && x.Status.Equals(loadCourierOrderBodyModel.Status)
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.WeightNumber >= loadCourierOrderBodyModel.MinWeight
                && x.WeightNumber <= loadCourierOrderBodyModel.MaxWeight
                && x.Status.Equals(loadCourierOrderBodyModel.Status));
            }

            //Low and High price and Date Filter
            else if (loadCourierOrderBodyModel.FromDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.ToDate.ToShortDateString() != "1/1/2001"
                && loadCourierOrderBodyModel.CourierId == 0
                && loadCourierOrderBodyModel.PodNumber == ""
                && loadCourierOrderBodyModel.Mobile == ""
                && loadCourierOrderBodyModel.Status == -1
                && loadCourierOrderBodyModel.CourierUserId < 0
                && loadCourierOrderBodyModel.StatusGroup.Contains("-1")
                && loadCourierOrderBodyModel.StatusList.Contains(-1)
                && loadCourierOrderBodyModel.OrderIds == ""
                && loadCourierOrderBodyModel.CollectionName == ""
                && loadCourierOrderBodyModel.DistrictIds.Contains(-1)
                && loadCourierOrderBodyModel.DistrictGroupName != ""
                && loadCourierOrderBodyModel.DistrictId == 0
                && loadCourierOrderBodyModel.ThanaId == 0
                && loadCourierOrderBodyModel.AreaId == 0
                && loadCourierOrderBodyModel.PaymentType == "-1"
                && loadCourierOrderBodyModel.OrderFrom == "-1"
                && loadCourierOrderBodyModel.LowPrice > 0
                && loadCourierOrderBodyModel.HighPrice > 0
                && loadCourierOrderBodyModel.MinWeight == 0
                && loadCourierOrderBodyModel.MaxWeight == 0
                )
            {
                orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.ActualPackagePrice >= loadCourierOrderBodyModel.LowPrice
                && x.ActualPackagePrice <= loadCourierOrderBodyModel.HighPrice
                , p => p.OrderByDescending(o => o.OrderDate),
                loadCourierOrderBodyModel.Index, loadCourierOrderBodyModel.Count);

                totalCount = await _orderGenericRepository.FindByCountAsyc(x => x.OrderDate.Date >= loadCourierOrderBodyModel.FromDate.Date
                && x.OrderDate.Date <= loadCourierOrderBodyModel.ToDate.Date
                && x.ActualPackagePrice >= loadCourierOrderBodyModel.LowPrice
                && x.ActualPackagePrice <= loadCourierOrderBodyModel.HighPrice);
            }

            //var dd = _excelService.ExportExcel(orderModel);

            var res = from d in orderModel
                      select new CourierOrderViewModel
                      {
                          CourierId = d.CourierId,
                          CourierName = d.CourierName ?? "",
                          CustomerName = d.CustomerName,
                          Status = d.StatusNameBng,
                          StatusId = d.Status,
                          StatusEng = d.StatusNameEng,
                          StatusType = d.StatusType,
                          HubName = d.HubName,
                          IsDownloaded = d.IsDownloaded,
                          DocumentUrl = d.DocumentUrl,
                          TransactionId = d.TransactionId,
                          CourierDeliveryManName = d.CourierDeliveryManName,
                          CourierDeliveryManMobile = d.CourierDeliveryManMobile,
                          DistrictCenter = d.DistrictCenter,
                          InvoiceNumber = d.InvoiceNumber,
                          InvoiceCourier = d.InvoiceCourier,
                          StartTime = d.StartTime,
                          EndTime = d.EndTime,
                          CutOffTime = d.CutOffTime,
                          SlotName = d.SlotName,
                          CollectionTime = d.CollectionTime,
                          IsConfirmedBy = d.IsConfirmedBy,
                          UpdatedBy = d.UpdatedBy,
                          PaymentServiceType = d.PaymentServiceType,
                          PaymentServiceCharge = d.PaymentServiceCharge,
                          PaymentServiceTypeVerify = d.PaymentServiceTypeVerify,
                          PaymentServiceTypeMerchantVerify = d.PaymentServiceTypeMerchantVerify,
                          QuickOrderImageUrl = d.QuickOrderImageUrl,
                          BookingMobile = d.BookingMobile,
                          Referrerformation = new Referrerformation
                          {
                              OfferType = d.OfferType,
                              Referrer = d.Referrer
                          },
                          OfferInformation = new OfferInformation
                          {
                              OfferCode = d.OfferCode,
                              OfferCodDiscount = d.OfferCodDiscount,
                              OfferBkashDiscount = d.OfferBkashDiscount,
                              IsOfferCodActive = d.IsOfferCodActive,
                              IsOfferBkashActive = d.IsOfferBkashActive,
                              ClassifiedId = d.ClassifiedId
                          },
                          RiderInformation = new RiderInformation
                          {
                              AssignOrderId = d.AssignOrderId,
                              RiderName = d.RiderName,
                              RiderMobile = d.RiderMobile,
                              DeliveryUserId= d.DeliveryUserId
                          },
                          CourierAddressContactInfo = new CourierAddressContactInfo
                          {
                              Mobile = d.Mobile,
                              OtherMobile = d.OtherMobile,
                              Address = d.Address,
                              DistrictName = d.DistrictName,
                              ThanaName = d.ThanaName,
                              AreaName = d.AreaName,
                              DistrictNameEng = d.DistrictNameEng,
                              ThanaNameEng = d.ThanaNameEng,
                              AreaNameEng = d.AreaNameEng,
                              ThanaPostalCode = d.ThanaPostalCode,
                              AreaPostalCode = d.AreaPostalCode,
                              DistrictId = d.DistrictId,
                              ThanaId = d.ThanaId,
                              AreaId = d.AreaId,
                              AssignedExpressCourierId = d.AssignedExpressCourierId,
                              AssignedCourierId = d.AssignedCourierId,
                              DTAdvanceCourierId = d.DTAdvanceCourierId,
                              CollectAddressDistrictId = d.CollectAddressDistrictId,
                              CollectAddressThanaId = d.CollectAddressThanaId,
                              CollectDistrictName = d.CollectDistrictName,
                              CollectThanaName = d.CollectThanaName,
                              MerchantCollectionDate = d.MerchantCollectionDate,
                              MerchantDeliveryDate = d.MerchantDeliveryDate,
                              RedxAreaId = d.RedxAreaId,
                              RedxAreaName = d.RedxAreaName,
                              IsDtOwnSecondMileDelivery = d.IsDtOwnSecondMileDelivery,
                              IsDtOwnSecondMileDeliveryThana = d.IsDtOwnSecondMileDeliveryThana,
                              IsDtOwnSecondMileDeliveryArea = d.IsDtOwnSecondMileDeliveryArea,
                              OwnSecondMileDelivery = d.OwnSecondMileDelivery,
                              OwnSecondMileDeliveryThana = d.OwnSecondMileDeliveryThana,
                              OwnSecondMileDeliveryArea = d.OwnSecondMileDeliveryArea,
                              EDeshMobileNo = d.EDeshMobileNo,
                              EdeshThana = d.EdeshThana
                          },
                          CourierOrderInfo = new CourierOrderInfo
                          {
                              ServiceType = d.ServiceType,
                              IsOpenBox = d.IsOpenBox,
                              PaymentType = d.PaymentType,
                              DeliveryRangeId = d.DeliveryRangeId,
                              OrderType = d.OrderType,
                              Weight = d.Weight,
                              WeightRangeId =d.WeightRangeId,
                              CollectionName = d.CollectionName,
                              OrderFrom = d.OrderFrom,
                              Version = d.Version,
                              ProductType = d.ProductType,
                              ActiveForCoronaMsgArea = d.ActiveForCoronaMsgArea,
                              ActiveForCoronaMsgThana = d.ActiveForCoronaMsgThana,
                              CouponIds = d.CouponIds
                          },
                          CourierPrice = new CourierPrice
                          {
                              ActualPackagePrice = d.ActualPackagePrice,
                              CollectionAmount = d.CollectionAmount,
                              DeliveryCharge = d.DeliveryCharge,
                              BreakableCharge = d.BreakableCharge,
                              CourierCharge = d.CourierCharge,
                              CODCharge = d.CodCharge,
                              CollectionCharge = d.CollectionCharge,
                              ReturnCharge = d.StatusGroup.ToLower().Trim() == "return" ? d.ReturnCharge : 0,
                              PackagingName = d.PackagingName,
                              PackagingCharge = d.PackagingCharge,
                              OfficeDrop = d.OfficeDrop
                          },
                          CourierOrderDateDetails = new CourierOrderDateDetails
                          {
                              ConfirmationFormatDate = d.ConfirmationDate,
                              OrderFormatDate = d.OrderDate,
                              PostedFormatDate = d.PostedOn,
                              UpdatedOn = d.UpdatedOn
                          },
                          CourierOrdersId = d.CourierOrdersId,
                          Id = d.Id,
                          Comment = d.Comment,
                          PodNumber = d.PodNumber,
                          UserInfo = new UserInfo
                          {
                              CourierUserId = d.MerchantId,
                              Mobile = d.MerchantMobile,
                              BkashNumber = d.BkashNumber,
                              CompanyName = d.CompanyName,
                              UserName = d.UserName,
                              Address = d.MerchantAddress,
                              EmailAddress = d.EmailAddress,
                              CollectAddress = d.CollectAddress,
                              RetentionUser = d.RetentionUser,
                              RetentionUserMobile = d.RetentionUserMobile.Trim(),
                              JoinDate = d.JoinDate,
                              OrderDateDiff = d.OrderDateDiff,
                              MerchantAssignActive = d.MerchantAssignActive,
                              IsBreakAble = d.IsBreakAble,
                              AutoDownload = d.AutoDownload
                          },
                          AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                          {
                              IsEmail = d.IsEmail,
                              IsSms = d.IsSms,
                              IsCustomerSms = d.IsCustomerSms,
                              IsCustomerEmail = d.IsCustomerEmail
                          },
                          VouchersViewModel = new VouchersViewModel
                          {
                              VoucherCode = d.VoucherCode,
                              VoucherDiscount = d.VoucherDiscount
                          },
                          DeliveryRangeViewModel = new DeliveryRangeViewModel
                          {
                              Name = d.Name,
                              Day = d.Day,
                              DayType = d.DayType,
                              OnImageLink = d.OnImageLink
                          }
                      };

            var responseData = new CourierOrderDetailsViewModel
            {
                TotalCount = totalCount,
                CourierOrderViewModel = res.ToList()
            };
            return responseData;
        }

        public async Task<IEnumerable<OrderStatusHistoryViewModel>> GetOrderHistoryInformation(string orderId)
        {
            return await _orderHistoryRepository.GetOrderHistoryInformation(orderId);
        }

        public async Task<IEnumerable<CourierUsers>> GetAllCourierUsersList(string companyName)
        {
            if (companyName == "0")
            {
                if (_redisModel.IsActive)
                {
                    string key = "GetAllCourierUsersList";

                    if (await _redis.Db1.Database.KeyExistsAsync(key) == true)
                    {
                        return await _redis.Db1.GetAsync<IEnumerable<CourierUsers>>(key);
                    }
                    else
                    {
                        var data = await _orderHistoryRepository.GetAllCourierUsersList(companyName);
                        bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                        return data;
                    } 
                }
                else
                {
                    return await _orderHistoryRepository.GetAllCourierUsersList(companyName);
                }
            }
            else
            {
                if (_redisModel.IsActive)
                {
                    string key = companyName;

                    if (await _redis.Db1.Database.KeyExistsAsync(key) == true)
                    {
                        return await _redis.Db1.GetAsync<IEnumerable<CourierUsers>>(key);
                    }
                    else
                    {
                        var data = await _orderHistoryRepository.GetAllCourierUsersList(companyName);
                        bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                        return data;
                    } 
                }
                else
                {
                    return await _orderHistoryRepository.GetAllCourierUsersList(companyName);
                }
            }
        }

        public async Task<IEnumerable<TeleSaleCourierUsers>> GetTeleSaleCourierUsersList(int courierUserId, int teleSales)
        {
            return await _orderRepository.GetTeleSaleCourierUsersList(courierUserId, teleSales);
        }

        public async Task<int> FixSpecialCharacter(string courierOrdersId)
        {
            return await _orderRepository.FixSpecialCharacter(courierOrdersId);
        }

        public async Task<IEnumerable<CourierOrderStatusHistory>> UpdateBulkOrders(List<CourierOrderStatusHistoryViewModel> courierOrderStatusHistory)
        {

            return await _orderRepository.UpdateBulkOrders(courierOrderStatusHistory);
        }

        public async Task<CourierUserPickupLocationModel> GetCourierUserListWithPickupLocations(CourierUsersLocationWiseSearchBodyModel merchant)
        {
            return await _orderRepository.GetCourierUserListWithPickupLocations(merchant);
        }

        public async Task<CourierOrderDetailsViewModel> GetCustomerOrders(string mobileNo)
        {
            var orderModel = new List<OrderModel>().AsQueryable();
            orderModel = await _orderGenericRepository.FindByAsyc(x => x.Mobile.Equals(mobileNo) || x.OtherMobile.Equals(mobileNo)
                , p => p.OrderByDescending(o => o.OrderDate),
                0, 5);
            var res = from d in orderModel
                      select new CourierOrderViewModel
                      {
                          CourierId = d.CourierId,
                          CourierName = d.CourierName ?? "",
                          CustomerName = d.CustomerName,
                          Status = d.StatusNameBng,
                          StatusId = d.Status,
                          StatusEng = d.StatusNameEng,
                          StatusType = d.StatusType,
                          HubName = d.HubName,
                          IsDownloaded = d.IsDownloaded,
                          OfferInformation = new OfferInformation
                          {
                              OfferCode = d.OfferCode,
                              OfferCodDiscount = d.OfferCodDiscount,
                              OfferBkashDiscount = d.OfferBkashDiscount,
                              IsOfferCodActive = d.IsOfferCodActive,
                              IsOfferBkashActive = d.IsOfferBkashActive,
                              ClassifiedId = d.ClassifiedId
                          },
                          RiderInformation = new RiderInformation
                          {
                              AssignOrderId = d.AssignOrderId,
                              RiderName = d.RiderName,
                              RiderMobile = d.RiderMobile,
                              DeliveryUserId = d.DeliveryUserId
                          },
                          CourierAddressContactInfo = new CourierAddressContactInfo
                          {
                              Mobile = d.Mobile,
                              OtherMobile = d.OtherMobile,
                              Address = d.Address,
                              DistrictName = d.DistrictName,
                              ThanaName = d.ThanaName,
                              AreaName = d.AreaName,
                              DistrictNameEng = d.DistrictNameEng,
                              ThanaNameEng = d.ThanaNameEng,
                              AreaNameEng = d.AreaNameEng,
                              ThanaPostalCode = d.ThanaPostalCode,
                              AreaPostalCode = d.AreaPostalCode,
                              DistrictId = d.DistrictId,
                              ThanaId = d.ThanaId,
                              AreaId = d.AreaId,
                              AssignedExpressCourierId = d.AssignedExpressCourierId,
                              AssignedCourierId = d.AssignedCourierId,
                              DTAdvanceCourierId = d.DTAdvanceCourierId,
                              CollectAddressDistrictId = d.CollectAddressDistrictId,
                              CollectAddressThanaId = d.CollectAddressThanaId,
                              CollectDistrictName = d.CollectDistrictName,
                              CollectThanaName = d.CollectThanaName,
                              MerchantCollectionDate = d.MerchantCollectionDate,
                              MerchantDeliveryDate = d.MerchantDeliveryDate
                          },
                          CourierOrderInfo = new CourierOrderInfo
                          {
                              ServiceType = d.ServiceType,
                              IsOpenBox = d.IsOpenBox,
                              PaymentType = d.PaymentType,
                              DeliveryRangeId = d.DeliveryRangeId,
                              OrderType = d.OrderType,
                              Weight = d.Weight,
                              WeightRangeId = d.WeightRangeId,
                              CollectionName = d.CollectionName,
                              OrderFrom = d.OrderFrom,
                              Version = d.Version,
                              ProductType = d.ProductType,
                              ActiveForCoronaMsgArea = d.ActiveForCoronaMsgArea,
                              ActiveForCoronaMsgThana = d.ActiveForCoronaMsgThana
                          },
                          CourierPrice = new CourierPrice
                          {
                              CollectionAmount = d.CollectionAmount,
                              DeliveryCharge = d.DeliveryCharge,
                              BreakableCharge = d.BreakableCharge,
                              CourierCharge = d.CourierCharge,
                              CODCharge = d.CodCharge,
                              CollectionCharge = d.CollectionCharge,
                              ReturnCharge = d.StatusGroup.ToLower().Trim() == "return" ? d.ReturnCharge : 0,
                              PackagingName = d.PackagingName,
                              PackagingCharge = d.PackagingCharge,
                              OfficeDrop = d.OfficeDrop
                          },
                          CourierOrderDateDetails = new CourierOrderDateDetails
                          {
                              ConfirmationFormatDate = d.ConfirmationDate,
                              OrderFormatDate = d.OrderDate,
                              PostedFormatDate = d.PostedOn
                          },
                          CourierOrdersId = d.CourierOrdersId,
                          Id = d.Id,
                          Comment = d.Comment,
                          PodNumber = d.PodNumber,
                          UserInfo = new UserInfo
                          {
                              CourierUserId = d.MerchantId,
                              Mobile = d.MerchantMobile,
                              BkashNumber = d.BkashNumber,
                              CompanyName = d.CompanyName,
                              UserName = d.UserName,
                              Address = d.MerchantAddress,
                              EmailAddress = d.EmailAddress,
                              CollectAddress = d.CollectAddress,
                              RetentionUser = d.RetentionUser,
                              RetentionUserMobile = d.RetentionUserMobile.Trim()
                          },
                          AdCourierCommunicationInfo = new AdCourierCommunicationInfo
                          {
                              IsEmail = d.IsEmail,
                              IsSms = d.IsSms,
                              IsCustomerSms = d.IsCustomerSms,
                              IsCustomerEmail = d.IsCustomerEmail
                          }
                      };

            var responseData = new CourierOrderDetailsViewModel
            {
                TotalCount = res.ToList().Count(),
                CourierOrderViewModel = res.ToList()
            };
            return responseData;
        }

        public async Task<IEnumerable<DeliveryUserAcceptedViewModel>> GetAcceptedRiders(PickupLocations pickupLocations)
        {
            var data =  await _orderRepository.GetAcceptedRiders(pickupLocations);
            var riderList = data.GroupBy(x => x.Id).Select(p => new DeliveryUserAcceptedViewModel
            {
                Id = p.FirstOrDefault().Id,
                Name = p.FirstOrDefault().Name,
                Mobile = p.FirstOrDefault().Mobile,
                Latitude = p.FirstOrDefault().Latitude,
                Longitude = p.FirstOrDefault().Longitude,
                CourierOrderIds = p.Select(y => y.CourierOrderId).Distinct().ToArray()
            }).ToList();
            return riderList;
        }

        public async Task<DeliveryUsersViewModel> GetRidersOfficeInfo(RequestBodyModel requestBodyModel)
        {
            return await _orderRepository.GetRidersOfficeInfo(requestBodyModel);
        }

        public async Task<CourierOrders> GetOrderInformation(int orderId)
        {
            return await _orderHistoryRepository.GetOrderInformation(orderId);
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetDuplicatesCourierUsersInfo()
        {
            return await _orderRepository.GetDuplicatesCourierUsersInfo();
        }

        public async Task<IEnumerable<dynamic>> GetAssignCouirerAndService(int districtId, int thanaId)
        {

            var assignCouirerAndServiceViewModel = new List<AssignCouirerAndServiceViewModel>();

            if (districtId > 0 && thanaId > 0)
            {
                assignCouirerAndServiceViewModel = await _orderRepository.GetAssignCouirerAndServiceArea(thanaId);
            }
            else if (districtId > 0 && thanaId == 0)
            {
                assignCouirerAndServiceViewModel = await _orderRepository.GetAssignCouirerAndService(districtId);
            }

            var results = assignCouirerAndServiceViewModel.GroupBy(n => new { n.DistrictId, n.DistrictEng })
                .Select(g => new {
                    DistrictId = g.Key.DistrictId,
                    DistrictEng = g.Key.DistrictEng,
                    DeliveryInfo = g.ToList()});

            return results;
        }
        public async Task<IEnumerable<dynamic>> GetMerchantAssignCouirerAndService(int districtId, int thanaId, int courierUserId)
        {

            var assignCouirerAndServiceViewModel = new List<AssignCouirerAndServiceViewModel>();

            if (districtId > 0 && thanaId > 0)
            {
                assignCouirerAndServiceViewModel = await _orderRepository.GetMerchantAssignCouirerAndServiceArea(thanaId, courierUserId);
            }
            else if (districtId > 0 && thanaId == 0)
            {
                assignCouirerAndServiceViewModel = await _orderRepository.GetMerchantAssignCouirerAndService(districtId, courierUserId);
            }
            else if(districtId == 0 && thanaId == 0)
            {
                assignCouirerAndServiceViewModel = await _orderRepository.GetMerchantAssignCouirerAndServiceByCourierUserId(courierUserId);
            }

            var results = assignCouirerAndServiceViewModel.GroupBy(n => new { n.DistrictId, n.DistrictEng })
                .Select(g => new {
                    DistrictId = g.Key.DistrictId,
                    DistrictEng = g.Key.DistrictEng,
                    DeliveryInfo = g.ToList()
                });

            return results;
        }
        public async Task<CourierOrders> UpdateInvoiceNumber(CourierOrders courierOrders)
        {
            return await _orderRepository.UpdateInvoiceNumber(courierOrders);
        }

        public async Task<int> UpdateRangeInvoiceNumber(List<CourierOrders> request)
        {
            return await _orderRepository.UpdateRangeInvoiceNumber(request);
        }

        public async Task<List<Districts>> LoadAllDistrictsByIds(List<Districts> request)
        {
            return await _orderRepository.LoadAllDistrictsByIds(request);
        }


        //private async Task<string> GetSmsAndMailFormat(string courierOrderId, CourierOrderStatusHistoryViewModel courierOrderStatusHistory)
        //{
        //    int startIndex = 3;
        //    int endIndex = courierOrderId.Length - 3;
        //    int OrderId = Convert.ToInt32(courierOrderId.Substring(startIndex, endIndex));

        //    var order = _sqlServerContext.CourierOrders.Where(o => o.Id.Equals(OrderId)).Select(s => new CourierOrders
        //    {
        //        Id = s.Id,
        //        ExpectedDeliveryDate = s.ExpectedDeliveryDate
        //    }).FirstOrDefault();

        //    string dtorderid = "dt-orderid", dtmerchantname = "dt-merchantname",
        //                dtcompanyname = "dt-companyname", dtpodnumber = "dt-podnumber",
        //                 dtcouriername = "dt-couriername", dtcustomername = "dt-customername", dtcustomermobile = "dt-customermobile",
        //                 dtdeliverymanname = "dt-courier-deliveryman-name", dtdeliverymanmobile = "dt-courier-deliveryman-mobile",
        //                 dtbkashnumber = "dt-bkashnumber", dtmerchantpaymentamount = "dt-merchantpaymentamount",
        //                 dtdistrictname = "dt-districtname", dtthananame = "dt-thananame",
        //                 dtareaname = "dt-areaname", dtridermobile = "dt-ridermobile",
        //                 dtcdddate = "dt-cdddate",
        //                 dtedeshmobileno = "dt-edeshmobileno";

        //    SendMobileBodyModel merchantSmsBody = new SendMobileBodyModel
        //    {
        //        numbers = new string[] { courierOrderStatusHistory.MerchantMobile.Trim() },
        //        text = courierOrderStatusHistory.MessageFormat.Trim()
        //        .Replace(dtorderid, courierOrderStatusHistory.CourierOrderId)
        //        .Replace(dtmerchantname, courierOrderStatusHistory.MerchantName)
        //        .Replace(dtcompanyname, courierOrderStatusHistory.CompanyName)
        //        .Replace(dtpodnumber, courierOrderStatusHistory.PodNumber)
        //        .Replace(dtcouriername, courierOrderStatusHistory.CourierName)
        //        .Replace(dtcustomername, courierOrderStatusHistory.CustomerName)
        //        .Replace(dtcustomermobile, courierOrderStatusHistory.CustomerMobile)
        //        .Replace(dtdeliverymanname, courierOrderStatusHistory.CourierDeliveryManName)
        //        .Replace(dtdeliverymanmobile, courierOrderStatusHistory.CourierDeliveryManMobile)
        //        .Replace(dtbkashnumber, courierOrderStatusHistory.BkashNumber)
        //        .Replace(dtmerchantpaymentamount, courierOrderStatusHistory.MerchantPaymentAmount)
        //        .Replace(dtdistrictname, courierOrderStatusHistory.DistrictName)
        //        .Replace(dtthananame, courierOrderStatusHistory.ThanaName)
        //        .Replace(dtareaname, courierOrderStatusHistory.AreaName)
        //        .Replace(dtridermobile, courierOrderStatusHistory.RiderMobile)
        //        .Replace(dtcdddate, order.ExpectedDeliveryDate.HasValue ? order.ExpectedDeliveryDate.Value.ToString("MM/dd/yyyy") : string.Empty)
        //        .Replace(dtedeshmobileno, courierOrderStatusHistory.EDeshMobileNo),
        //        datacoding = 0,
        //        type = 0
        //    };
        //    var listOfData = new List<SendMobileBodyModel>();
        //    listOfData.Add(merchantSmsBody);
        //    bool isActive = await _smsEmailService.SmsSend(listOfData);

        //    return merchantSmsBody.ToString();
        //}

        public async Task<IEnumerable<OrderStatusHistoryViewModel>> GetQuickOfficeReceivedDetails(string courierOrdersId, int userId, string hubName)
        {
            var orders = new List<CourierOrders>();

            if (courierOrdersId != "0")
            {
                bool entity = await _sqlServerContext.CourierOrderStatusHistory.AnyAsync(h => h.CourierOrderId == courierOrdersId && h.Status == 7);

                if (!entity)
                {
                    orders.Add(new CourierOrders
                    {
                        CourierOrdersId = courierOrdersId,
                        Status = 7,
                        UpdatedBy = userId,
                        IsConfirmedBy = "admin",
                        HubName = hubName,
                        Comment = "Order received by DT head office-7"
                    });

                    var updateOrder = await _weightRangeRepository.UpdateBulkStatus(orders);
                }
                
            }
            
            return await _orderHistoryRepository.GetQuickOfficeReceivedDetails(userId, hubName);
        }

        public async Task<int> QuickUpdateStatus(RequestBodyModel requestBody)
        {
            var orders = new List<CourierOrders>();

            if (requestBody.CourierOrdersId != "0")
            {
                var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(o => o.CourierOrdersId == requestBody.CourierOrdersId);

                if (entity != null)
                {
                    if(requestBody.Flag == 1 && entity.CourierId == requestBody.CourierId)
                    {
                        if (entity.OrderType == "Only Delivery")
                        {
                            orders.Add(new CourierOrders
                            {
                                CourierOrdersId = requestBody.CourierOrdersId,
                                Status = 9,
                                UpdatedBy = requestBody.UserId,
                                IsConfirmedBy = "admin",
                                HubName = requestBody.HubName,
                                Comment = "Paid Order Given to courier-9"
                            });
                        }
                        else if (entity.OrderType == "Delivery Taka Collection")
                        {


                            if (entity.PaymentServiceType == 1 && string.IsNullOrEmpty(entity.PaymentServiceTypeMerchantVerify) && string.IsNullOrEmpty(entity.PaymentServiceTypeVerify))
                            {
                                return 10;
                            }
                            else if (entity.PaymentServiceType == 1 && (string.IsNullOrEmpty(entity.PaymentServiceTypeMerchantVerify) || string.IsNullOrEmpty(entity.PaymentServiceTypeVerify)))
                            {
                                return 10;
                            }


                            orders.Add(new CourierOrders
                            {
                                CourierOrdersId = requestBody.CourierOrdersId,
                                Status = 10,
                                UpdatedBy = requestBody.UserId,
                                IsConfirmedBy = "admin",
                                HubName = requestBody.HubName,
                                Comment = "UnPaid Order Given to courier-10"
                            });
                        }
                        var updateOrder = await _weightRangeRepository.UpdateBulkStatus(orders);
                        return 1;
                    }
                    else if(requestBody.Flag == 2)
                    {
                        orders.Add(new CourierOrders
                        {
                            CourierOrdersId = requestBody.CourierOrdersId,
                            Status = 36,
                            UpdatedBy = requestBody.UserId,
                            IsConfirmedBy = "admin",
                            HubName = requestBody.HubName,
                            Comment = "Sent To Hub-36-" + requestBody.SentToHubName
                        });
                        var updateOrder = await _weightRangeRepository.UpdateBulkStatus(orders);
                        return 1;
                    }
                    else if (requestBody.Flag == 3)
                    {
                        orders.Add(new CourierOrders
                        {
                            CourierOrdersId = requestBody.CourierOrdersId,
                            Status = 37,
                            UpdatedBy = requestBody.UserId,
                            IsConfirmedBy = "admin",
                            HubName = requestBody.HubName,
                            Comment = "Received From Hub-37-" + requestBody.HubName
                        });
                        var updateOrder = await _weightRangeRepository.UpdateBulkStatus(orders);
                        return 1;
                    }
                    return 0; 
                    
                }
                return 0;
            }
            return 0;
        }

        public async Task<bool> SendPushNotification(int courierUserId, OrderStatusViewModel request)
        {
            var fireBaseToken = await _sqlServerContext.CourierUsers.AsNoTracking().Where(x=> x.CourierUserId == courierUserId)
                .Select(y=> y.FirebaseToken).FirstOrDefaultAsync();

            if(fireBaseToken != "" && fireBaseToken != null)
            {
                var notification = new CourierOrderStatus()
                {
                    NotificationType = request.NotificationType,
                    BigText = request.BigText,
                    Description = request.Description,
                    ImageLink = request.ImageLink,
                    ServiceType = request.ServiceType,
                    Title = request.Title

                };

                return await _firebaseCloudService.SendNotificationDeliveryBondhu(fireBaseToken, notification);
            }

            return false;
        }
        public async Task<IEnumerable<CourierOrderViewModel>> GetQuickUpdateStatusDetails(RequestBodyModel bodyModel)
        {
            return await _orderRepository.GetQuickUpdateStatusDetails(bodyModel);
        }

        public async Task<DeliveryChargeMerchantDetails> GetDeliveryChargeMerchantDetailsCourier(DeliveryChargeMerchantDetails request)
        {
            return await _orderRepository.GetDeliveryChargeMerchantDetailsCourier(request);
        }

        public async Task<List<DeliveryUsersViewModel>> GetLocationWiseRiders(List<LocationAssign> locationAssigns)
        {
            return await _orderRepository.GetLocationWiseRiders(locationAssigns);
        }
        public async Task<int> UpdateMultipleOrdersWithRider(List<CourierOrders> courierOrders)
        {
            return await _orderRepository.UpdateMultipleOrdersWithRider(courierOrders);
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetFirstTimeOrderedMerchantList(RequestBodyModel request)
        {
            var data = await _orderRepository.GetFirstTimeOrderedMerchantList(request);

            return data.Select(x => new CourierOrdersViewModel
            {
                Id = x.Id,
                OrderType = x.OrderType,
                OrderDate = x.OrderDate,
                ActualPackagePrice = x.ActualPackagePrice,
                DistrictId = x.DistrictId,
                CourierUsers = new CourierUsersViewModel
                {
                    CompanyName = x.CompanyName,
                    Mobile = x.Mobile,
                    AlterMobile = x.AlterMobile,
                    Address = x.Address,
                    JoinDate = x.JoinDate
                }
            });
        }

        public async Task<List<VouchersViewModel>> GetMerchantAssignedVoucher(List<VouchersViewModel> vouchers)
        {
            return await _orderRepository.GetMerchantAssignedVoucher(vouchers);
        }

        public async Task<int> UpdatePriceWithOrderType(CourierOrders courierOrders)
        {
            return await _orderRepository.UpdatePriceWithOrderType(courierOrders);
        }
        public async Task<dynamic> GetOrderDetails(int orderId)
        {
            return await _orderRepository.GetOrderDetails(orderId);
        }

        public async Task<int> UpdateServiceType(CourierOrders courierOrders)
        {
            return await _orderRepository.UpdateServiceType(courierOrders);
        }

        public async Task<IEnumerable<WeightRangeWiseData>> GetSpecialService(RequestBodyModel request)
        {
            return await _orderRepository.GetSpecialService(request); ;
        }

        public async Task<int> UpdateMerchantReview(int CourierUserId, CourierUsers courierUsers)
        {
            return await _orderRepository.UpdateMerchantReview(CourierUserId, courierUsers);
        }

        public async Task<IEnumerable<CouriersViewModel>> GetAllCouriers()
        {
            return await _orderRepository.GetAllCouriers();
        }

        public async Task<CourierOrderDetailsViewModel> LoadPOHOrders(LoadCourierOrderBodyModel request)
        {
            var orderModel = new List<OrderModel>().AsQueryable();

            orderModel = await _orderGenericRepository.FindByAsyc(x => x.OrderDate.Date >= request.FromDate.Date
                        && x.OrderDate.Date <= request.ToDate.Date
                        && x.PaymentServiceType == 1);

            //var data = orderModel.ToList();

            var res = from d in orderModel
                      select new CourierOrderViewModel
                      {
                          CourierId = d.CourierId,
                          Status = d.StatusNameBng,
                          StatusId = d.Status,
                          CustomerName = d.CustomerName,
                          StatusEng = d.StatusNameEng,
                          StatusType = d.StatusType,
                          IsConfirmedBy = d.IsConfirmedBy,
                          UpdatedBy = d.UpdatedBy,
                          PaymentServiceType = d.PaymentServiceType,
                          PaymentServiceCharge = d.PaymentServiceCharge,
                          PaymentServiceTypeVerify = d.PaymentServiceTypeVerify,
                          PaymentServiceTypeMerchantVerify = d.PaymentServiceTypeMerchantVerify,
                          CourierAddressContactInfo = new CourierAddressContactInfo
                          {
                              Mobile = d.Mobile,
                              OtherMobile = d.OtherMobile,
                              Address = d.Address
                          },
                          CourierOrderInfo = new CourierOrderInfo
                          {
                              ServiceType = d.ServiceType,
                              IsOpenBox = d.IsOpenBox,
                              PaymentType = d.PaymentType,
                              DeliveryRangeId = d.DeliveryRangeId,
                              OrderType = d.OrderType,
                              Weight = d.Weight,
                              WeightRangeId = d.WeightRangeId,
                              CollectionName = d.CollectionName,
                              OrderFrom = d.OrderFrom,
                              Version = d.Version,
                              ProductType = d.ProductType,
                              ActiveForCoronaMsgArea = d.ActiveForCoronaMsgArea,
                              ActiveForCoronaMsgThana = d.ActiveForCoronaMsgThana,
                              CouponIds = d.CouponIds
                          },
                          CourierPrice = new CourierPrice
                          {
                              ActualPackagePrice = d.ActualPackagePrice,
                              DeliveryCharge = d.DeliveryCharge,
                              PackagingName = d.PackagingName,
                              PackagingCharge = d.PackagingCharge,
                              OfficeDrop = d.OfficeDrop
                          },
                          CourierOrderDateDetails = new CourierOrderDateDetails
                          {
                              ConfirmationFormatDate = d.ConfirmationDate,
                              OrderFormatDate = d.OrderDate,
                              PostedFormatDate = d.PostedOn,
                              UpdatedOn = d.UpdatedOn
                          },
                          CourierOrdersId = d.CourierOrdersId,
                          Id = d.Id,
                          Comment = d.Comment,
                          PodNumber = d.PodNumber,
                          UserInfo = new UserInfo
                          {
                              CourierUserId = d.MerchantId,
                              Mobile = d.MerchantMobile,
                              BkashNumber = d.BkashNumber,
                              CompanyName = d.CompanyName,
                              UserName = d.UserName,
                              Address = d.MerchantAddress,
                              EmailAddress = d.EmailAddress,
                              CollectAddress = d.CollectAddress,
                              RetentionUser = d.RetentionUser,
                              RetentionUserMobile = d.RetentionUserMobile.Trim(),
                              JoinDate = d.JoinDate,
                              OrderDateDiff = d.OrderDateDiff,
                              MerchantAssignActive = d.MerchantAssignActive,
                              IsBreakAble = d.IsBreakAble
                          }
                      };



            var responseData = new CourierOrderDetailsViewModel
            {
                TotalCount = res.Count(),
                CourierOrderViewModel = res.ToList()
            };

            return responseData;
        }
        public async Task<IEnumerable<dynamic>> SameDayCollectedPendingOrdersCount(RequestBodyModel requestBody)
        {
            return await _orderRepository.SameDayCollectedPendingOrdersCount(requestBody);
        }
    }
}
