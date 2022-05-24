using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IOrderHistoryService
    {
        Task<CourierOrderStatusHistory> AddCourierOrderHistory(CourierOrderStatusHistory courierOrderStatusHistory);
    }
}
