using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class CourierOrdersViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string OtherMobile { get; set; }
        public string Address { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public string PaymentType { get; set; }
        public string OrderType { get; set; }
        public int TotalOrder { get; set; }
        public decimal TotalCharge { get; set; }
        public string Weight { get; set; }
        public string CollectionName { get; set; }
        public DateTime? CollectionTime { get; set; }
        public decimal CollectionAmount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public bool IsActive { get; set; } = false;
        public int UpdatedBy { set; get; }
        public DateTime UpdatedOn { set; get; }
        public DateTime ConfirmationDate { set; get; }
        public string CourierOrdersId { set; get; }
        public DateTime OrderDate { set; get; } 
        public int Status { set; get; }
        public DateTime PostedOn { set; get; } 
        public int PostedBy { set; get; }
        public int MerchantId { set; get; }
        public int TotalMerchant { get; set; }
        public string Comment { set; get; }
        public string PodNumber { set; get; } 
        public decimal BreakableCharge { get; set; }
        public string Note { get; set; }
        public string IsConfirmedBy { get; set; } 
        public decimal CodCharge { get; set; }
        public int CourierId { set; get; } 
        public decimal CollectionCharge { get; set; }
        public decimal ReturnCharge { get; set; }
        public string PackagingName { get; set; }
        public decimal PackagingCharge { get; set; }
        public string CollectAddress { get; set; }
        public string HubName { get; set; } 
        public string OrderFrom { get; set; }
        public string ServiceType { get; set; } = "";
        public decimal CourierCharge { get; set; }
        public bool IsOpenBox { get; set; }
        public bool IsAutoProcess { get; set; } 
        public bool isTakaCollectionFromCourier { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceCourier { get; set; }
        public int CollectAddressDistrictId { get; set; }
        public int CollectAddressThanaId { get; set; }
        //[ForeignKey("Status")]
        public virtual OrderStatusViewModel CourierOrderStatus { get; set; } = null;
        public virtual CourierUsersViewModel CourierUsers { get; set; } = null;
        public virtual CouriersViewModel Couriers { get; set; } = null;
        public virtual DeliveryZoneViewModel DeliveryZoneViewModel { get; set; } = null;

        //Added for DeliveryRange Type Wise list Report

        public int DeliveryRangeId { get; set; }
        public int WeightRangeId { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public int Hours { get; set; }
        public string StatusNameEng { get; set; }
        public string CourierName { get; set; } = "";
        public bool? IsQuickOrder { get; set; } = false;
        public string QuickOrderImageUrl { get; set; }
        public int OrderRequestId { get; set; }
        public bool IsDownloaded { get; set; }
        public DateTime? DownloadedDate { get; set; }
        public string QuickOrderGenerateForHub { get; set; } = "";
        public string DocumentUrl { get; set; } = "";
        public DateTime RiderAcceptDate { get; set; }
        public string CompanyName { get; set; } = "";
        public string AlterMobile { get; set; } = "";
        public decimal ActualPackagePrice { get; set; } = 0;
        public DateTime JoinDate { get; set; }
        public decimal ReAttemptCharge { get; set; } = 0;

        //public virtual CouriersViewModel CouriersViewModel { get; set; } = new CouriersViewModel();

        public virtual DistrictsViewModel DistrictsViewModel { get; set; } = new DistrictsViewModel();

        public virtual DeliveryRange DeliveryRange { get; set; } = new DeliveryRange();

        public virtual DeliveryUsersViewModel DeliveryUsersViewModel { get; set; }
        public virtual List<DeliveryUsersViewModel> DeliveryUsersList { get; set; } = null;

        public virtual CollectionTimeSlotViewModel CollectionTimeSlot { get; set; }

        //public CourierOrdersViewModel(DataModel.CourierOrders c)
        //{
        //    Id = c.Id;
        //    CustomerName = c.CustomerName;
        //    Mobile = c.Mobile;
        //    OtherMobile = c.OtherMobile;
        //    Address = c.Address;
        //    DistrictId = c.DistrictId;
        //    ThanaId = c.ThanaId;
        //    AreaId = c.AreaId;
        //    PaymentType = c.PaymentType;
        //    OrderType = c.OrderType;
        //    Weight = c.Weight;
        //    CollectionName = c.CollectionName;
        //    CollectionAmount = c.CollectionAmount;
        //    DeliveryCharge = c.DeliveryCharge;
        //    IsActive = c.IsActive;
        //    UpdatedBy = c.UpdatedBy;
        //    UpdatedOn = c.UpdatedOn;
        //    ConfirmationDate = c.ConfirmationDate;
        //    CourierOrdersId = c.CourierOrdersId;
        //    OrderDate = c.OrderDate;
        //    Status = c.Status;
        //    PostedBy = c.PostedBy;
        //    PostedOn = c.PostedOn;
        //    MerchantId = c.MerchantId;
        //    Comment = c.Comment;
        //    PodNumber = c.PodNumber;
        //    BreakableCharge = c.BreakableCharge;
        //    Note = c.Note;
        //    IsConfirmedBy = c.IsConfirmedBy;
        //    CodCharge = c.CodCharge;
        //    CourierId = c.CourierId;
        //    CollectionCharge = c.CollectionCharge;
        //    ReturnCharge = c.ReturnCharge;
        //    PackagingName = c.PackagingName;
        //    PackagingCharge = c.PackagingCharge;
        //    CollectAddress = c.CollectAddress;
        //    HubName = c.HubName;
        //    OrderFrom = c.OrderFrom;
        //    CourierCharge = c.CourierCharge;
        //    IsOpenBox = c.IsOpenBox;
        //    IsAutoProcess = c.IsAutoProcess;
        //    isTakaCollectionFromCourier = c.isTakaCollectionFromCourier;

        //}
    }

}
