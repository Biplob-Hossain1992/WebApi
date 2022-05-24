
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CourierOrders", Schema = "DT")]
    public class CourierOrders
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [MaxLength(100)]
        //[Required]
        public string CustomerName { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        [MaxLength(20)]
        public string OtherMobile { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }

        [MaxLength(50)]
        public string PaymentType { get; set; } = "";
        [MaxLength(50)]
        public string OrderType { get; set; }
        [MaxLength(50)]
        public string Weight { get; set; } = "";
        [MaxLength(200)]
        public string CollectionName { get; set; }
        public decimal CollectionAmount { get; set; }
        public decimal DeliveryCharge { get; set; }
        //[Required]
        public bool IsActive { get; set; } = false;

        public int UpdatedBy { set; get; }
        public DateTime UpdatedOn { set; get; } = DateTime.Now;

        public DateTime ConfirmationDate { set; get; } = DateTime.Now;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string CourierOrdersId { set; get; }

        public DateTime OrderDate { set; get; } = DateTime.Now;
        public int Status { set; get; } = 0;
        public DateTime PostedOn { set; get; } = DateTime.Now;
        public int PostedBy { set; get; }
        //public int UserId { set; get; } = 0;
        public int MerchantId { set; get; } = 0;
        public string Comment { set; get; } = "";
        public string PodNumber { set; get; } = "";
        public decimal BreakableCharge { get; set; }
        public string ThirdPartyCourierInfo { get; set; } = "";
        public string Note { get; set; }
        public string IsConfirmedBy { get; set; } = "";
        public decimal CodCharge { get; set; }
        public int CourierId { set; get; } = 0;
        public decimal CollectionCharge { get; set; }
        public decimal ReturnCharge { get; set; }
        public string PackagingName { get; set; }
        public decimal PackagingCharge { get; set; }
        public string CollectAddress { get; set; }
        public bool IsDownloaded { get; set; } = false;
        public string HubName { get; set; } = "";
        public string OrderFrom { get; set; } = "";
        public decimal CourierCharge { get; set; } = 0;
        public bool IsOpenBox { get; set; }
        public bool IsAutoProcess { get; set; } = false;
        public bool IsTakaCollectionFromCourier { get; set; } = false;
        public int DeliveryRangeId { get; set; }
        public int WeightRangeId { get; set; }
        [MaxLength(50)]
        public string ProductType { get; set; } = "";
        public int CollectAddressDistrictId { get; set; } = 0;
        public int CollectAddressThanaId { get; set; } = 0;
        public int DeliveryUserId { get; set; } = 0;
        public DateTime RiderAcceptDate { get; set; } = DateTime.Now;
        public DateTime RiderDeliveredDate { get; set; } = DateTime.Now;
        public DateTime? MerchantDeliveryDate { get; set; }
        public DateTime? MerchantCollectionDate { get; set; }
        public bool OfficeDrop { get; set; } = false;
        //public int DeliveryZoneId { get; set; } = 0;
        //public int CollectZoneId { get; set; } = 0;
        public string OfferCode { get; set; } = "";
        public decimal OfferCodDiscount { get; set; } = 0;
        public decimal OfferBkashDiscount { get; set; } = 0;
        public bool IsOfferCodActive { get; set; } = false;
        public bool IsOfferBkashActive { get; set; } = false;
        public int ClassifiedId { get; set; } = 0;
        public decimal ActualPackagePrice { get; set; } = 0;
        public int CollectionTimeSlotId { get; set; } = 4;
        public DateTime? CollectionTime { get; set; }
        public string OfferType { get; set; } = "";
        public string RelationType { get; set; } = "";
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string CourierDeliveryManName { get; set; } = "";
        public string CourierDeliveryManMobile { get; set; } = "";
        public string TransactionId { get; set; } = "";
        public string ValidationId { get; set; } = "";
        public string ServiceType { get; set; } = "alltoall";
        public string Version { get; set; } = "";
        public string InvoiceNumber { get; set; } = "";
        public string InvoiceCourier { get; set; } = "";
        public bool? IsQuickOrder { get; set; } = false;
        public DateTime? QuickOrderGenerateDate { get; set; } = DateTime.Now;
        public DateTime? DownloadedDate { get; set; }
        public string QuickOrderImageUrl { get; set; } = "";
        public int OrderRequestId { get; set; } = 0;
        public int QuickOrderGenerateBy { get; set; } = 0;
        public string QuickOrderGenerateForHub { get; set; } = "";
        public string DocumentUrl { get; set; } = "";
        public decimal ReAttemptCharge { get; set; } = 0;
        public bool IsHeavyWeight { get; set; } = false;
        public string CouponIds { get; set; } = "";
        public decimal VoucherDiscount { get; set; } = 0;
        public string VoucherCode { get; set; } = "";
        public int VoucherDeliveryRangeId { get; set; } = 0;
        public string DistinctCenter { get; set; } = "";

        public byte? PaymentServiceType { get; set; } = 0;
        public decimal? PaymentServiceCharge { get; set; } = 0;
        public string PaymentServiceTypeVerify { get; set; } = string.Empty;
        public string PaymentServiceTypeMerchantVerify { get; set; } = string.Empty;
        public int CourierLocationId { get; set; } = 0;
        public string BookingMobile { get; set; } = "";
        public decimal OpenBoxCharge { get; set; } = 0;
    }

}