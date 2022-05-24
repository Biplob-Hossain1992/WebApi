using AdCourier.Domain.Entities.BodyModel.Dashboard;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.Other;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashBoardOrderViewModel> GetDashBoardOrder(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel);
        Task<IEnumerable<StatusGroupViewModel>> GetOrderCountByStatusGroupV3(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel);
        Task<IEnumerable<StatusGroupViewModel>> GetOrderCountByStatusGroup(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel);
        Task<StatusGroup> UpdateSummary(int id, StatusGroupViewModel statusGroup);
        Task<dynamic> GetCustomerInfoByMobile(string mobileNo);
        Task<StatusGroupViewModel> GetCollection(int courierUserId);
        Task<IEnumerable<CourierOrderStatusHistory>> GetCollectionHistory(int courierUserId);
        Task<MerchantAdvanceBalanceInfo> GetMerchantBalanceInfo(int courierUserId, int totalAmount);
        Task<DashboardViewModel> GetOrderCountByStatusGroupNew(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel);
    }
}
