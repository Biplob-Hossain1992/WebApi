using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;

namespace AdCourier.Services
{
    public class OrderRequestService : IOrderRequestService
    {
        private readonly IOrderRequestRepository _orderRequestRepository;

        public OrderRequestService(IOrderRequestRepository orderRequestRepository)
        {
            _orderRequestRepository = orderRequestRepository;
        }

        public async Task<OrderRequest> AddOrderRequest(OrderRequest requestModel)
        {
            return await _orderRequestRepository.AddOrderRequest(requestModel);
        }
        
        public async Task<Couriers> AddCourier(Couriers couriers)
        {
            return await _orderRequestRepository.AddCourier(couriers);
        }
    }
}
