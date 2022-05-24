using System.Collections.Generic;

namespace AdCourier.Domain.Entities.ViewModel.Other
{
    public class StatusGroupViewModel
    {
        public int StatusGroupId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int DashboardSpanCount { get; set; }
        public string DashboardViewColorType { get; set; }
        public int DashboardViewOrderBy { get; set; }
        public string DashboardRouteUrl { get; set; }
        public string DashboardCountSumView { get; set; }
        public decimal TotalAmount { get; set; }
        public string DashboardStatusFilter { get; set; }
        public string DashboardImageUrl { get; set; }
    }

    public class DashboardViewModel
    {
        public IList<StatusGroupViewModel> PickDashboardViewModel { get; set; }
        public IList<StatusGroupViewModel> OrderDashboardViewModel { get; set; }
        public IList<StatusGroupViewModel> PaymentDashboardViewModel { get; set; }
    }
}
