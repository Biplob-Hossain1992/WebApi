using AdCourier.Domain.Entities.BodyModel.Dashboard;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.Other;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<StatusGroupViewModel> GetCollection(int courierUserId)
        {
            return await _dashboardRepository.GetCollection(courierUserId);
        }
        public async Task<MerchantAdvanceBalanceInfo> GetMerchantBalanceInfo(int courierUserId, int totalAmount)
        {
            return await _dashboardRepository.GetMerchantBalanceInfo(courierUserId,totalAmount);
        }

        public async Task<IEnumerable<StatusGroupViewModel>> GetOrderCountByStatusGroup(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            var statusGroup = new int[] { 4, 6 };

            var data = await _dashboardRepository.GetOrderCountByStatusGroup(orderCountByStatusGroupBodyModel);

            return data.Where(x => statusGroup.Contains(x.StatusGroupId)).OrderBy(x => x.DashboardViewOrderBy);
        }

        public async Task<IEnumerable<StatusGroupViewModel>> GetOrderCountByStatusGroupV3(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            var statusGroup = new int[] { 4, 6 };

            var data = await _dashboardRepository.GetOrderCountByStatusGroup(orderCountByStatusGroupBodyModel);

            return data.OrderBy(x => x.DashboardViewOrderBy);
        }

        public async Task<DashBoardOrderViewModel> GetDashBoardOrder(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {

            var data = await _dashboardRepository.GetDashBoardOrder(orderCountByStatusGroupBodyModel);

            return data;

        }
        public async Task<DashboardViewModel> GetOrderCountByStatusGroupNew(OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {

            var data = await _dashboardRepository.GetOrderCountByStatusGroupNew(orderCountByStatusGroupBodyModel);

            return data;

        }

        public async Task<StatusGroup> UpdateSummary(int id, StatusGroupViewModel statusGroup)
        {
            return await _dashboardRepository.UpdateSummary(id, statusGroup);
        }

        public async Task<dynamic> GetCustomerInfoByMobile(string mobileNo)
        {
            return await _dashboardRepository.GetCustomerInfoByMobile(mobileNo);
        }

        public async Task<IEnumerable<CourierOrderStatusHistory>> GetCollectionHistory(int courierUserId)
        {
            return await _dashboardRepository.GetCollectionHistory(courierUserId);
        }
    }
}
