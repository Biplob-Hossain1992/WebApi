using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IOrderHistoryRepository
    {
        Task<CourierOrders> GetOrderInformation(int orderId);
        Task<IEnumerable<OrderStatusHistoryViewModel>> GetOrderHistoryInformation(string orderId);
        Task<CourierOrderStatusHistory> AddCourierOrderHistory(CourierOrderStatusHistory courierOrderStatusHistory);
        Task<List<CourierOrderStatusHistory>> AddListCourierOrderHistory(List<CourierOrderStatusHistory> courierOrderStatusHistory);
        Task<IEnumerable<CourierUsers>> GetAllCourierUsersList(string companyName);
        Task<IEnumerable<OrderStatusHistoryViewModel>> GetQuickOfficeReceivedDetails(int userId, string hubName);
    }
}
