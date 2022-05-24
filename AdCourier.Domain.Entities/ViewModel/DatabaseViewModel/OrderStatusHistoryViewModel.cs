using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using System;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class OrderStatusHistoryViewModel
    {
        public int Id { set; get; }
        public string CourierOrderId { set; get; }
        public string IsConfirmedBy { set; get; }
        public DateTime OrderDate { set; get; }
        public int Status { set; get; }
        public DateTime PostedOn { set; get; }
        public int PostedBy { set; get; }
        public int MerchantId { set; get; }
        public string Comment { set; get; }
        public string PodNumber { set; get; }
        public int CourierId { set; get; }
        public string HubName { get; set; }

        public virtual OrderStatusViewModel OrderStatus { get; set; } = null;
        public virtual CourierUsersViewModel CourierUsersView { get; set; } = null;

        //public CourierOrderStatusHistoryViewModel(DataModel.CourierOrderStatusHistory  c)
        //{
        //    Id = c.Id;
        //    CourierOrderId = c.CourierOrderId;
        //    IsConfirmedBy = c.IsConfirmedBy;
        //    OrderDate = c.OrderDate;
        //    Status = c.Status;
        //    PostedOn = c.PostedOn;
        //    PostedBy = c.PostedBy;
        //    MerchantId = c.MerchantId;
        //    Comment = c.Comment;
        //    PodNumber = c.PodNumber;
        //    CourierId = c.CourierId;
        //    HubName = c.HubName;
        //}
    }


}
