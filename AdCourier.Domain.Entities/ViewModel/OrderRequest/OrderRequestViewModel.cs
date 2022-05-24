using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.OrderRequest
{
    public class OrderRequestViewModel
    {
        public int OrderRequestId { get; set; }
        public int CourierUserId { get; set; }
        public int RequestOrderAmount { get; set; } = 0;
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public int DistrictId { get; set; } = 0;
        public int ThanaId { get; set; } = 0;
        public DateTime CollectionDate { get; set; } = DateTime.Now;
        public int CollectionTimeSlotId { get; set; } = 0;
        public int DeliveryUserId { get; set; } = 0;
        public int Status { get; set; } = 0;
        public int TotalOrder { get; set; } = 0;
        public virtual CollectionTimeSlotViewModel CollectionTimeSlot { get; set; } = null;
        public virtual DistrictsViewModel DistrictsViewModel { get; set; } = new DistrictsViewModel();
        public List<ActionModel> ActionModel { set; get; }
        public virtual ActionModel ActionViewModel { set; get; }
        public virtual CourierUsersViewModel CourierUsersView  { get; set; } = null;
        //public virtual CourierOrders OrdersModel { get; set; } = null;
        public virtual DeliveryUsersViewModel DeliveryUsersViewModel { get; set; } = null;
        public virtual List<DeliveryUsersViewModel> DeliveryUsersList { get; set; } = null;
        public virtual LocationAssignViewModel LocationAssignViewModel { get; set; } = null;
        public List<OrderRequestViewModel> OrderRequestSelfList { get; set; }
    }
}
