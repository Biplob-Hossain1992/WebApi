using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IOrderRequestService
    {
        Task<OrderRequest> AddOrderRequest(OrderRequest request);
        Task<Couriers> AddCourier(Couriers couriers);
    }
}
