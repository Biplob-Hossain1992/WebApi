using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.BondhuApp
{
    public class DtOrderDetailsDataModel
    {
        public int Id { get; set; }
        public bool OfficeDrop { get; set; } = false;
        public string CourierOrdersId { get; set; }
        public string CollectionName { get; set; }
        public string ShipmentCharge { get; set; }
        public string CodCharge { get; set; }
        public string BreakableCharge { get; set; }
        public string ReturnCharge { get; set; }
        public string PackagingCharge { get; set; }
        public int CollectionCharge { get; set; }
        public string TotalServiceCharge { get; set; }
        public string CollectionAmount { get; set; }
        public string ActualPackagePrice { get; set; }
        public string Status { get; set; }
        public string StatusNameEng { get; set; }
        public string StatusType { get; set; }
        public string OrderType { get; set; }
        public string PaymentType { get; set; }
        public string CourierUserId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantAddress { get; set; }
        public string MerchantMobile { get; set; }
        public string CourierName { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string District { get; set; }
        public string ThanaName { get; set; }
        public string ThanaId { get; set; }
        public string AreaName { get; set; }
        public string AreaId { get; set; }
        public DateTime? PickDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public string SourceType { get; set; }
        public string PickedToDelivered { get; set; }
        public string PodNumber { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string OtherMobile { get; set; }
        public string Address { get; set; }
        public string BkashNumber { get; set; }
        public int AssignOrderId { get; set; }
        public string RiderName { get; set; }
        public int PickDistrictId { get; set; }
        public int PickThanaId { get; set; }
        public string PickThanaName { get; set; }
        public int PickAreaId { get; set; }
        public int DeliveryBondhuId { get; set; }
        public string DeliveryBondhuName { get; set; }
        public string DeliveryBondhuMobile { get; set; }
        public int OtherDeliveryBondhuId { get; set; }
        public string OtherDeliveryBondhuName { get; set; }
        public string OtherDeliveryBondhuMobile { get; set; }
        public int OfficeReceivedMerchantId { get; set; }
        public int MerchantId { get; set; }
        public string DocumentUrl { get; set; }
    }
}
