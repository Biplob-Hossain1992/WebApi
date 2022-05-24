using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IOrderRequestRepository
    {
        Task<OrderRequest> AddOrderRequest(OrderRequest requestModel);
        Task<Couriers> AddCourier(Couriers couriers);
    }
}
