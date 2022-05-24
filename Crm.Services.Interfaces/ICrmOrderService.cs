using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using Crm.Domain.Entities.DapperBodyModel;
using Crm.Domain.Entities.DapperViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.Services.Interfaces
{
    public interface ICrmOrderService
    {
        Task<CombineCrmOrderViewModel> GetOrders(SearchOrderBodyModel searchOrderBodyModel);
        Task<Deals> GetProductInformation(int dealId);
        Task<IEnumerable<Domain.Entities.DapperViewModel.DatabaseViewModel.OrderStatusHistoryViewModel>> GetOrderHistoryInformation(string orderId);
    }
}
