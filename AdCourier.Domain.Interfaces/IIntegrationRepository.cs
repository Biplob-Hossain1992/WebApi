using AdCourier.Domain.Entities.IntegrationBody;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IIntegrationRepository
    {
        Task<string> order(IntegrationOrderBodyModel request);
        Task<bool> UpdateOrderHistory(string orderId, UpdateStatusBodyModel request);
    }
}
