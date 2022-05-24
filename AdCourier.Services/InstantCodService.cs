using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class InstantCodService : IInstantCodService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly IInstantCodRepository _instantCodRepository;
        public InstantCodService(IInstantCodRepository instantCodRepository, 
            IOrderHistoryRepository orderHistoryRepository,
            IOrderRepository orderRepository)
        {
            _instantCodRepository = instantCodRepository;
            _orderHistoryRepository = orderHistoryRepository;
            _orderRepository = orderRepository;
        }
        public async Task<IEnumerable<CourierOrders>> AddInstantCodOrder(List<CourierOrders> courierOrders)
        {
            var orderHistory = new List<CourierOrderStatusHistory>();

            var res = await _orderRepository.AddOrdersBulk(courierOrders);


            foreach (var item in res)
            {
                orderHistory.Add(new CourierOrderStatusHistory
                {
                    OrderDate = item.OrderDate,
                    CourierOrderId = item.CourierOrdersId,
                    Status = item.Status,
                    PostedOn = item.UpdatedOn,
                    PostedBy = item.UpdatedBy,
                    MerchantId = item.MerchantId,
                    Comment = item.Comment,
                    IsConfirmedBy = item.IsConfirmedBy,
                    PodNumber = item.PodNumber
                });
            }

            var resOrderHistory = await _orderRepository.AddCourierOrderHistoryBulk(orderHistory);

            return res;
        }

        public async Task<bool> CheckInstaCod(RequestBodyModel request)
        {
            return await _instantCodRepository.CheckInstaCod(request);
        }

        public async Task<dynamic> GetInstantCodCollectionDetails(RequestBodyModel request)
        {
            return await _instantCodRepository.GetInstantCodCollectionDetails(request);
        }
    }
}
