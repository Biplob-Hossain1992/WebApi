using AdCourier.Domain.Entities.BodyModel.Dashboard;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.Other;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashBoardOrderViewModel> GetDashBoardOrder(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel);
        Task<IEnumerable<StatusGroupViewModel>> GetOrderCountByStatusGroup(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel);
        Task<StatusGroup> UpdateSummary(int id, StatusGroupViewModel statusGroup);
        Task<dynamic> GetCustomerInfoByMobile(string mobileNo);
        Task<DashboardViewModel> GetOrderCountByStatusGroupNew(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel);
        Task<StatusGroupViewModel> GetCollection(int courierUserId);
        Task<IEnumerable<CourierOrderStatusHistory>> GetCollectionHistory(int courierUserId);
        Task<MerchantAdvanceBalanceInfo> GetMerchantBalanceInfo(int courierUserId, int totalAmount);
    }
}
