using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.Utility;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.OrderRequest;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class QuickOrderService : IQuickOrderService
    {
        private readonly IQuickOrderRepository _quickOrderRepository;
        private readonly IOrderRepository _orderRepository;
        public QuickOrderService(IQuickOrderRepository quickOrderRepository, IOrderRepository orderReposiotry)
        {
            _quickOrderRepository = quickOrderRepository;
            _orderRepository = orderReposiotry;
        }
        public async Task<List<CourierOrders>> AddQuickOrders(SearchBodyModel searchBodyModel)
        {

            List<CourierOrders> courierOrders = new List<CourierOrders>();

            for (int i = 0; i < searchBodyModel.QuickOrderLimit; i++)
            {
                courierOrders.Add(new CourierOrders
                {
                    CustomerName = "",
                    Mobile = "",
                    OtherMobile = "",
                    Address = "",
                    DistrictId = 0,
                    ThanaId = 0,
                    PaymentType = "",
                    OrderType = "",
                    Weight = "",
                    CollectionName = "",
                    CollectionAmount = 0,
                    DeliveryCharge = 0,
                    IsActive = true,
                    Status = 0,
                    PostedBy = 0,
                    UpdatedBy = 0,
                    PodNumber = "",
                    MerchantId = 0,
                    Comment = "",
                    BreakableCharge = 0,
                    ThirdPartyCourierInfo = "",
                    IsConfirmedBy = "",
                    Note = "",
                    CodCharge = 0,
                    CourierId = 0,
                    CollectionCharge = 0,
                    ReturnCharge = 0,
                    PackagingName = "",
                    PackagingCharge = 0,
                    CollectAddress = "",
                    IsDownloaded = false,
                    HubName = "",
                    OrderFrom = "",
                    CourierCharge = 0,
                    IsOpenBox = false,
                    IsAutoProcess = false,
                    IsTakaCollectionFromCourier = false,
                    DeliveryRangeId = 0,
                    WeightRangeId = 0,
                    ProductType = "",
                    CollectAddressDistrictId = 0,
                    CollectAddressThanaId = 0,
                    DeliveryUserId = 0,
                    OfficeDrop = false,
                    OfferCode = "",
                    OfferBkashDiscount = 0,
                    OfferCodDiscount = 0,
                    IsOfferBkashActive = false,
                    IsOfferCodActive = false,
                    ClassifiedId = 0,
                    ActualPackagePrice = 0,
                    ValidationId = "",
                    TransactionId = "",
                    CollectionTimeSlotId = 0,
                    OfferType = "",
                    RelationType = "",
                    CourierDeliveryManName = "",
                    CourierDeliveryManMobile = "",
                    ServiceType = "",
                    Version = "",
                    InvoiceCourier = "",
                    InvoiceNumber = "",
                    IsQuickOrder = true,
                    QuickOrderGenerateBy = searchBodyModel.QuickOrderGenerateBy,
                    QuickOrderGenerateForHub = searchBodyModel.QuickOrderGenerateForHub
                });
            }

            //var courierOrders = Enumerable.Repeat(new CourierOrders(), searchBodyModel.QuickOrderLimit).ToList();

            return await _quickOrderRepository.AddQuickOrders(courierOrders);
        }

        public async Task<List<CourierOrders>> GetGenerateQuickOrders(RequestBodyModel requestBodyModel)
        {
            return await _quickOrderRepository.GetGenerateQuickOrders(requestBodyModel);
        }

        public async Task<bool> CheckIsQuickOrder(string orderId)
        {
            return await _quickOrderRepository.CheckIsQuickOrder(orderId);
        }
        public async Task<bool> IsAcceptedQuickOrder(int orderRequestId)
        {
            return await _quickOrderRepository.IsAcceptedQuickOrder(orderRequestId);
        }

        public async Task<List<CourierUsers>> GetMerchantByCompanyName(string companyName)
        {
            return await _quickOrderRepository.GetMerchantByCompanyName(companyName);
        }

        public async Task<IEnumerable<dynamic>> LoadQuickOrder(RequestBodyModel request)
        {
            var quickOrders = await _quickOrderRepository.LoadQuickOrder(request);
            return quickOrders;
        }

        public async Task<CourierOrders> UpdateOrderInfoForApp(CourierOrders orders)
        {
            return await _quickOrderRepository.UpdateOrderInfoForApp(orders);
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetQuickOrders(RequestBodyModel request)
        {
            return await _quickOrderRepository.GetQuickOrders(request);
        }

        public async Task<IEnumerable<OrderRequestViewModel>> GetMerchantQuickOrders(RequestBodyModel request)
        {
            return await _quickOrderRepository.GetMerchantQuickOrders(request);
        }

        public async Task<IEnumerable<OrderRequestViewModel>> GetMerchantWiseRequestOrders(RequestBodyModel requestBody)
        {
            return await _quickOrderRepository.GetMerchantWiseRequestOrders(requestBody);
        }

        public async Task<int> UpdateOrderRequests(List<OrderRequest> orderRequests)
        {
            return await _quickOrderRepository.UpdateOrderRequests(orderRequests);
        }

        public async Task<CourierOrders> QuickOrderProcess(CourierOrders request)
        {
            request.CustomerName = SpecialCharacters.RemoveSpecialCharacters(request.CustomerName);
            request.CollectionName = SpecialCharacters.RemoveSpecialCharacters(request.CollectionName);
            request.Mobile = SpecialCharacters.RemoveSpecialCharacters(request.Mobile);
            request.OtherMobile = SpecialCharacters.RemoveSpecialCharacters(request.OtherMobile);
            request.Address = SpecialCharacters.RemoveSpecialCharacters(request.Address);

            return await _quickOrderRepository.QuickOrderProcess(request);
        }
        public async Task<int> UpdateMultipleTimeSlot(List<OrderRequest> orderRequests)
        {
            return await _quickOrderRepository.UpdateMultipleTimeSlot(orderRequests);
        }
        public async Task<int> UpdateRider(OrderRequest orderRequest)
        {
            return await _quickOrderRepository.UpdateRider(orderRequest);
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetQuickOrderGenerateForHub(RequestBodyModel request)
        {
            return await _quickOrderRepository.GetQuickOrderGenerateForHub(request);
        }

        public async Task<int> DeleteOrderRequest(int orderRequestId)
        {
            return await _quickOrderRepository.DeleteOrderRequest(orderRequestId);
        }
        public async Task<int> UpdateCollectionTimeSlotIdManually(int flag)
        {
            return await _quickOrderRepository.UpdateCollectionTimeSlotIdManually(flag);
        }
    }
}
