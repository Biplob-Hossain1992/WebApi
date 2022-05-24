using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.OrderTracking
{
    public class OrderTrackingStatusViewModel: Districts
    {
        public string OrderTrackStatusGroup { get; set; } = "";
        //public string OrderTrackStatusPublicGroup { get; set; } = "";
        public DateTime DateTime { get; set; }
        public IEnumerable<StatusNameViewModel> Status { get; set; }
    }

    public class StatusNameViewModel
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
    }


    public class CourierOrderStatusViewModel
    {
        public string FulfillmentStatusGroup { get; set; }
        public DateTime DateTime { get; set; }
        public IEnumerable<CourierOrderStatusNameViewModel> Status { get; set; }
    }

    public class CourierOrderStatusNameViewModel
    {
        public int StatusId { get; set; }
        public string StatusNameBng { get; set; }
        public string StatusNameEng { get; set; }
        public DateTime DateTime { get; set; }
        public string MessageFormat { get; set; }
        public string EmailFormat { get; set; }
        public string CustomerMessageFormat { get; set; }
        public string CustomerEmailFormat { get; set; }
        public string RetentionMessageFormat { get; set; }
    }
    public class CourierOrderTrackHistoryViewModel
    {
        public string StatusNameBng { set; get; }
        public string StatusNameEng { set; get; }


        public string CourierOrderId { set; get; }
        public string IsConfirmedBy { set; get; }
        public String OrderDate { set; get; }
        public int Status { set; get; } = 0;
        public string PostedOn { set; get; }
        public string NamePostedBy { set; get; } = "";

        public string Comment { set; get; }
        public string PodNumber { set; get; } = "";
        public int CourierId { set; get; } = 0;
        public string HubName { get; set; }
        public string CourierDeliveryManName { get; set; } = "";
        public string CourierDeliveryManMobile { get; set; } = "";
        public virtual DistrictsViewModel DistrictsViewModel { get; set; } = new DistrictsViewModel();
    }
}
