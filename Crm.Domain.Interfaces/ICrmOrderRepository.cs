using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using Crm.Domain.Entities.DapperBodyModel;
using Crm.Domain.Entities.DapperDataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.Domain.Interfaces
{
    public interface ICrmOrderRepository
    {
        Task<CombineCrmOrderDataModel> GetOrders(SearchOrderBodyModel searchOrderBodyModel);
        Task<Deals> GetProductInformation(int dealId);
        Task<IEnumerable<OrderStatusHistoryDataModel>> GetOrderHistoryInformation(string orderId);
    }
}
