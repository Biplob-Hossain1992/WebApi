using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.ProcedureDataModel
{
    [Table("USV_LoadOrders", Schema = "DT")]
    public class OrderModel
    {
        public int Id { get; set; }
        public string CourierOrdersId { get; set; }
        public decimal ActualPackagePrice { get; set; }
        public string CustomerName { get; set; }
        public int Status { get; set; }
        public string HubName { get; set; }
        public string Mobile { get; set; }
        public string OtherMobile { get; set; }
        public string Address { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public bool IsOpenBox { get; set; }
        public string PaymentType { get; set; }
        public string OrderType { get; set; }
        public string Weight { get; set; }
        public string CollectionName { get; set; }
        public string OrderFrom { get; set; }
        public decimal CollectionAmount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal BreakableCharge { get; set; }
        public decimal CodCharge { set; get; }
        public decimal CourierCharge { get; set; }
        public decimal CollectionCharge { get; set; }
        public string PackagingName { get; set; }
        public decimal PackagingCharge { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Comment { get; set; }
        public string PodNumber { get; set; }
        public int MerchantId { get; set; }
        public string CollectAddress { get; set; }
        public decimal ReturnCharge { get; set; }
        public string StatusNameBng { get; set; }
        public string StatusNameEng { get; set; }
        public string StatusType { get; set; }
        public string StatusGroup { get; set; }
        public int CourierId { get; set; }
        public string CourierName { get; set; }

        public string MerchantMobile { get; set; }
        public bool IsBreakAble { get; set; }
        public bool MerchantAssignActive { get; set; }
        public string BkashNumber { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string MerchantAddress { get; set; }
        public string EmailAddress { get; set; }
        public bool IsEmail { get; set; }
        public bool IsSms { get; set; }
        public bool IsCustomerSms { get; set; }
        public bool IsCustomerEmail { get; set; }
        public string DistrictName { get; set; }
        public string ThanaName { get; set; }
        public string AreaName { get; set; }
        public string DistrictNameEng { get; set; }
        public string ThanaNameEng { get; set; }
        public string AreaNameEng { get; set; }
        public string ThanaPostalCode { get; set; }
        public string AreaPostalCode { get; set; }
        public int AssignedExpressCourierId { get; set; }
        public int AssignedCourierId { get; set; }
        public int DeliveryRangeId { get; set; }
        public int WeightRangeId { get; set; }
        public string ProductType { get; set; }
        public string RetentionUser { get; set; }
        public string ActiveForCoronaMsgThana { get; set; }
        public string ActiveForCoronaMsgArea { get; set; }
        public int AssignOrderId { get; set; }
        public string RiderName { get; set; }
        public string RiderMobile { get; set; }
        public int DeliveryUserId { get; set; }
        public int CollectAddressDistrictId { get; set; }
        public int CollectAddressThanaId { get; set; }
        public string CollectDistrictName { get; set; }
        public string CollectThanaName { get; set; }
        public DateTime? MerchantDeliveryDate { get; set; }
        public DateTime? MerchantCollectionDate { get; set; }
        public int DTAdvanceCourierId { get; set; }
        public bool OfficeDrop { get; set; }
        public string RetentionUserMobile { get; set; }

        public string OfferCode { get; set; }
        public decimal OfferCodDiscount { get; set; }
        public decimal OfferBkashDiscount { get; set; }
        public bool IsOfferCodActive { get; set; }
        public bool IsOfferBkashActive { get; set; }
        public int ClassifiedId { get; set; }
        public bool IsDownloaded { get; set; }
        public string DocumentUrl { get; set; }
        public string TransactionId { get; set; }
        public DateTime? JoinDate { get; set; }
        public int OrderDateDiff { get; set; }
        public string Referrer { get; set; }
        public string OfferType { get; set; }
        public int RedxAreaId { get; set; }
        public string RedxAreaName { get; set; }
        public string ServiceType { get; set; }
        public string Version { get; set; }
        public bool IsDtOwnSecondMileDelivery { get; set; }
        public bool IsDtOwnSecondMileDeliveryThana { get; set; }
        public bool IsDtOwnSecondMileDeliveryArea { get; set; }
        public string CourierDeliveryManName { get; set; }
        public string CourierDeliveryManMobile { get; set; }
        public string DistrictCenter { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceCourier { get; set; }
        public string EDeshMobileNo { get; set; }
        public string EdeshThana { get; set; }
        public string OwnSecondMileDelivery { get; set; }
        public string OwnSecondMileDeliveryThana { get; set; }
        public string OwnSecondMileDeliveryArea { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? CutOffTime { get; set; }
        public string SlotName { get; set; }
        public int WeightNumber { get; set; }
        public string CouponIds { get; set; }
        public string VoucherCode { get; set; }
        public decimal VoucherDiscount { get; set; }
        public DateTime? CollectionTime { get; set; }
        public string IsConfirmedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string DayType { get; set; } = "";
        public string OnImageLink { get; set; } = "";
        public string QuickOrderImageUrl { get; set; }
        public byte PaymentServiceType { get; set; }
        public decimal PaymentServiceCharge { get; set; }
        public string PaymentServiceTypeVerify { get; set; }
        public string PaymentServiceTypeMerchantVerify { get; set; }
        public bool AutoDownload { get; set; }
        public string BookingMobile { get; set; }
    }
}
