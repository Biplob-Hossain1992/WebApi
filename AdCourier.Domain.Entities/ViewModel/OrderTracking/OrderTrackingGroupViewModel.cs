using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.OrderTracking
{
    public class OrderTrackingGroupViewModel
    {
        public int StatusGroupId { get; set; }
        public string TrackingName { get; set; }
        public string TrackingColor { get; set; }
        public bool TrackingFlag { get; set; }
        public DateTime TrackingDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime ExpectedFirstDeliveryDate { get; set; }
        public decimal TrackingOrderBy { get; set; }
        public int DistrictId { get; set; }
        public int PickDistrictId { get; set; }
        public int Status { get; set; }
        public HubViewModel SubTrackingShipmentName { get; set; }
        public HubViewModel SubTrackingReturnName { get; set; }
        public CourierDeliveryManViewModel CourierDeliveryMan { get; set; }
    }

    public class CourierDeliveryManViewModel
    {
        public string CourierDeliveryManMobile { get; set; }
        public string CourierDeliveryManName { get; set; }
        public string CourierComment { get; set; }
        public string EDeshMobileNo { get; set; }
    }
    public class OrderTracking
    {
        public CourierOrdersViewModel courierOrdersViewModel { get; set; }
        public List<OrderTrackingGroupViewModel> orderTrackingGroupViewModel { get; set; }
    }
}
