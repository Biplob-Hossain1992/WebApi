using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.OrderRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IQuickOrderRepository
    {
        Task<List<CourierOrders>> AddQuickOrders(List<CourierOrders> courierOrders);
        Task<List<CourierOrders>> GetGenerateQuickOrders(RequestBodyModel requestBodyModel);
        Task<bool> CheckIsQuickOrder(string orderId);
        Task<bool> IsAcceptedQuickOrder(int orderRequestId);
        Task<List<CourierUsers>> GetMerchantByCompanyName(string companyName);
        Task<IEnumerable<dynamic>> LoadQuickOrder(RequestBodyModel request);
        Task<CourierOrders> UpdateOrderInfoForApp(CourierOrders orders);
        Task<IEnumerable<CourierUsersViewModel>> GetQuickOrders(RequestBodyModel request);
         Task<IEnumerable<OrderRequestViewModel>> GetMerchantQuickOrders(RequestBodyModel request);
        Task<IEnumerable<OrderRequestViewModel>> GetMerchantWiseRequestOrders(RequestBodyModel requestBody);
        Task<CourierOrders> QuickOrderProcess(CourierOrders request);
        Task<int> UpdateOrderRequests(List<OrderRequest> orderRequests);
        Task<int> UpdateMultipleTimeSlot(List<OrderRequest> orderRequests);
        Task<int> UpdateRider(OrderRequest orderRequest);
        Task<IEnumerable<CourierOrdersViewModel>> GetQuickOrderGenerateForHub(RequestBodyModel request);
        Task<int> DeleteOrderRequest(int orderRequestId);
        Task<int> UpdateCollectionTimeSlotIdManually(int flag);
    }
}
