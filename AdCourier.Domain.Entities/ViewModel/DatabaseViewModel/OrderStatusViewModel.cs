using System;
using AdCourier.Domain.Entities.DataModel;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class OrderStatusViewModel
    {
        public int StatusId { set; get; }
        public string StatusNameEng { set; get; } 
        public string StatusNameBng { set; get; } 
        public string StatusGroup { set; get; } 
        public string FulfillmentStatusGroup { set; get; } 
        public string OrderTrackStatusGroup { get; set; } 
        public string OrderTrackStatusPublicGroup { get; set; } 
        public string DashboardStatusGroup { get; set; }
        public string StatusType { get; set; } 
        public bool IsActive { set; get; } 
        public DateTime PostedOn { set; get; }
        public string Message { get; set; }
        public string Email { get; set; } 
        public string CustomerMessage { get; set; } 
        public string CustomerEmail { get; set; }
        public int NotificationType { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageLink { get; set; } = "";
        public string BigText { get; set; } = "";
        public string ServiceType { get; set; } = "";

        //public CourierOrderStatusViewModel(CourierOrderStatus v)
        //{
        //    StatusId = v.StatusId;
        //    StatusNameEng = v.StatusNameEng;
        //    StatusNameBng = v.StatusNameBng;
        //    StatusGroup = v.StatusGroup;
        //    FulfillmentStatusGroup = v.FulfillmentStatusGroup;
        //    OrderTrackStatusGroup = v.OrderTrackStatusGroup;
        //    OrderTrackStatusPublicGroup = v.OrderTrackStatusPublicGroup;
        //    DashboardStatusGroup = v.DashboardStatusGroup;
        //    StatusType = v.StatusType;
        //    IsActive = v.IsActive;
        //    PostedOn = v.PostedOn;
        //    Message = v.Message;
        //    Email = v.Email;
        //    CustomerMessage = v.CustomerMessage;
        //    CustomerEmail = v.CustomerEmail;
        //}
    }
}
