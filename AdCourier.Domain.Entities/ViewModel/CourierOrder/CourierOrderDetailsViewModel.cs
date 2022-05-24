using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.CourierOrder
{
    public class RiderInformation
    {
        public int AssignOrderId { get; set; }
        public string RiderName { get; set; }
        public string RiderMobile { get; set; }
        public int DeliveryUserId { get; set; }
    }

    public class Referrerformation
    {
        public string OfferType { get; set; }
        public string Referrer { get; set; }
    }
    public class OfferInformation
    {
        public string OfferCode { get; set; }
        public decimal OfferCodDiscount { get; set; }
        public decimal OfferBkashDiscount { get; set; }
        public bool IsOfferCodActive { get; set; }
        public bool IsOfferBkashActive { get; set; }
        public int ClassifiedId { get; set; }
    }


    public class CourierOrderDetailsViewModel
    {

        public decimal TotalCount { set; get; }
        public decimal AdTotalCollectionAmount { set; get; }
        public AdCourierPaymentInfo AdCourierPaymentInfo { set; get; }
        public List<CourierOrderViewModel> CourierOrderViewModel { set; get; }
    }


    public class CourierOrderViewModel
    {
        private string StatusTypeText(string statusType)
        {
            string text = string.Empty;
            if (statusType.ToLower() == "paid".ToLower())
            {
                text = "পেইড";
            }
            else if (statusType.ToLower() == "unpaid".ToLower())
            {
                text = "আনপেইড";
            }
            else
            {
            }
            return text;
        }
        public string StatusTypeName { get { return StatusTypeText(this.StatusType); } }
        public bool IsDownloaded { get; set; }
        public bool ButtonFlag { get; set; } = false;
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CourierOrdersId { set; get; }
        public string Status { set; get; }
        public string DashboardStatusGroup { get; set; }
        public string OrderTrackStatusGroup { get; set; }
        public string StatusType { set; get; }
        public string StatusEng { set; get; }
        public int StatusId { set; get; }
        public int LogStatusId { set; get; }
        public string LogStatus { set; get; }

        public string DocumentUrl { get; set; }
        public string TransactionId { get; set; }
        public string CourierDeliveryManName { get; set; }
        public string CourierDeliveryManMobile { get; set; }
        public string DistrictCenter { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceCourier { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? CutOffTime { get; set; }
        public string SlotName { get; set; }
        public DateTime? CollectionTime { get; set; }
        public int CourierId { set; get; }
        public string CourierName { set; get; }
        public string Comment { set; get; }
        public string PodNumber { set; get; }
        public string HubName { set; get; }
        public string IsConfirmedBy { get; set; }
        public int UpdatedBy { get; set; }

        public byte PaymentServiceType { get; set; }
        public decimal PaymentServiceCharge { get; set; }
        public string PaymentServiceTypeVerify { get; set; }
        public string PaymentServiceTypeMerchantVerify { get; set; }
        public string QuickOrderImageUrl { get; set; }
        public string BookingMobile { get; set; }
        public HubViewModel HubViewModel { get; set; }
        public Referrerformation Referrerformation { get; set; }
        public OfferInformation OfferInformation { set; get; }
        public RiderInformation RiderInformation { set; get; }
        public CourierAddressContactInfo CourierAddressContactInfo { set; get; }
        public CourierOrderInfo CourierOrderInfo { set; get; }
        public CourierOrderDateDetails CourierOrderDateDetails { set; get; }
        public ActionUrl ActionUrl { get { return LoadUrl(this.CourierOrdersId); } }
        public UserInfo UserInfo { set; get; }
        public CourierPrice CourierPrice { set; get; }
        public AdCourierCommunicationInfo AdCourierCommunicationInfo { set; get; }
        public VouchersViewModel VouchersViewModel { get; set; }
        public virtual DeliveryRangeViewModel DeliveryRangeViewModel { get; set; }
        public string ClassNameCss { get { return ClassNameRetrive(this.StatusType); } }
        public string PaidUnpaidColor { get { return PaidUnpaidColorClass(this.StatusType); } }
        private string PaidUnpaidColorClass(string status)
        {
            string color = string.Empty;
            if (status.ToLower() == "paid")
            {
                color = "paidColor";
            }
            else if (status.ToLower() == "unpaid")
            {
                color = "unPaidColor";
            }
            else
            {
                color = "";
            }
            return color;
        }

        private string ClassNameRetrive(string input)
        {
            string color = string.Empty;
            if (input.ToLower() == "paid")
            {
                color = "success";
            }
            else if (input.ToLower() == "unpaid")
            {
                color = "danger";
            }
            else
            {
                color = "";
            }
            return color;
        }

        public ActionUrl LoadUrl(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return new ActionUrl
                {
                    ButtonName = "",
                    Url = ""
                };
            }
            else
            {
                return new ActionUrl
                {
                    ButtonName = "Update",
                    Url = UrlPath.CourierUrl + "api/Update/UpdateOrderHistory/" + orderId.Trim()
                };
            }

        }

    }
    public class AdCourierCommunicationInfo
    {
        public bool IsCustomerSms { set; get; }
        public bool IsCustomerEmail { set; get; }
        public bool IsSms { set; get; }
        public bool IsEmail { set; get; }
    }
    public class AdCourierPaymentInfo
    {
        public decimal PaymentInProcessing { set; get; }//15,24
        public decimal PaymentPaid { set; get; }//25
        public decimal PaymentReady { set; get; }//28

    }
    public class CourierOrderInfo
    {
        public string Version { get; set; }
        public string ServiceType { get; set; }
        public bool IsOpenBox { get; set; }
        public string PaymentType { get; set; }
        public int DeliveryRangeId { get; set; }
        public string OrderType { get; set; }
        public string Weight { get; set; }
        public int WeightRangeId { get; set; }
        public string CollectionName { get; set; }
        public string OrderFrom { get; set; }
        public string ProductType { get; set; }
        public string ActiveForCoronaMsgThana { get; set; }
        public string ActiveForCoronaMsgArea { get; set; }
        public string CouponIds { get; set; }
    }
    public class CourierAddressContactInfo
    {
        public string Mobile { get; set; }
        public string OtherMobile { get; set; }
        public string Address { get; set; }
        public string DistrictNameEng { get; set; }
        public string ThanaNameEng { get; set; }
        public string AreaNameEng { get; set; }
        public string DistrictName { get; set; }
        public string ThanaName { get; set; }
        public string AreaName { get; set; }
        public int DistrictId { set; get; }
        public int ThanaId { set; get; }
        public int AreaId { set; get; }
        public string ThanaPostalCode { set; get; }
        public string AreaPostalCode { set; get; }
        public DateTime? MerchantDeliveryDate { get; set; }
        public DateTime? MerchantCollectionDate { get; set; }
        public int RedxAreaId { get; set; }
        public string RedxAreaName { get; set; }
        public string RedxHubName { get; set; }

        //public int AssignedAreaExpressCourierId { get; set; }
        //public int AssignedAreaCourierId { get; set; }

        //public int AssignedThanaExpressCourierId { get; set; }
        //public int AssignedThanaCourierId { get; set; }
        public int AssignedExpressCourierId { get; set; }
        public int AssignedCourierId { get; set; }
        public int CollectAddressDistrictId { get; set; }
        public int CollectAddressThanaId { get; set; }
        public string CollectDistrictName { get; set; }
        public string CollectThanaName { get; set; }
        public int DTAdvanceCourierId { get; set; }
        public bool IsDtOwnSecondMileDelivery { get; set; }
        public bool IsDtOwnSecondMileDeliveryThana { get; set; }
        public bool IsDtOwnSecondMileDeliveryArea { get; set; }
        public string OwnSecondMileDelivery { get; set; }
        public string OwnSecondMileDeliveryThana { get; set; }
        public string OwnSecondMileDeliveryArea { get; set; }
        public string EDeshMobileNo { get; set; }
        public string EdeshThana { get; set; }
    }
    public class CourierOrderDateDetails
    {
        [JsonIgnore]
        public DateTime UpdatedOn { get; set; }
        public string UpdatedOnDate { get { return this.UpdatedOn.ToString("MM-dd-yyyy HH:mm:ss"); } }
        [JsonIgnore]
        public DateTime ConfirmationFormatDate { set; get; }
        public string ConfirmationDate { get { return this.ConfirmationFormatDate.ToString("MM-dd-yyyy HH:mm:ss"); } }
        [JsonIgnore]
        public DateTime OrderFormatDate { set; get; }
        public string OrderDate { get { return this.OrderFormatDate.ToString("MM-dd-yyyy HH:mm:ss"); } }
        [JsonIgnore]
        public DateTime PostedFormatDate { set; get; }
        public string PostedOn { get { return this.PostedFormatDate.ToString("MM-dd-yyyy HH:mm:ss"); } }
        [JsonIgnore]
        public DateTime LogPostedFormatDate { set; get; }
        public string LogPostedOn { get { return this.LogPostedFormatDate.ToString("MM-dd-yyyy HH:mm:ss"); } }

    }
    public class CourierPrice
    {
        public decimal ActualPackagePrice { get; set; }
        public decimal CollectionAmount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal BreakableCharge { get; set; }
        public decimal CODCharge { set; get; }

        public decimal CollectionCharge { set; get; }
        public decimal ReturnCharge { set; get; }
        public decimal CourierCharge { get; set; }

        public string PackagingName { set; get; }
        public decimal PackagingCharge { set; get; }
        public bool OfficeDrop { get; set; }
        public decimal PaymentServiceCharge { get; set; }

        public decimal TotalServiceCharge { get { return TotalServiceChargeCalculateAmount(this.BreakableCharge, this.DeliveryCharge, this.CODCharge, this.CollectionCharge, this.ReturnCharge, this.PackagingCharge, this.PaymentServiceCharge); } }

        private decimal TotalServiceChargeCalculateAmount(decimal breakableCharge, decimal deliveryCharge, decimal cODCharge, decimal collectionCharge, decimal returnCharge, decimal packagingCharge, decimal paymentServiceCharge)
        {
            return breakableCharge + deliveryCharge + cODCharge + collectionCharge + returnCharge + packagingCharge + paymentServiceCharge;
        }

        public decimal TotalAmount { get { return TotalCalculateAmount(this.BreakableCharge, this.DeliveryCharge, this.CODCharge, this.CollectionCharge, this.ReturnCharge, this.PackagingCharge); } }

        private decimal TotalCalculateAmount(decimal breakableCharge, decimal deliveryCharge, decimal codCharge, decimal collectionCharge, decimal returnCharge, decimal packagingCharge)
        {
            return breakableCharge + deliveryCharge + codCharge + collectionCharge + returnCharge + packagingCharge;
        }
    }
    public class UserInfo
    {
        public int CourierUserId { set; get; }
        public string CompanyName { get; set; }
        public string UserName { set; get; }
        public string Mobile { set; get; }
        public string Address { set; get; }
        public string EmailAddress { set; get; }
        public string CollectAddress { set; get; }
        public string BkashNumber { get; set; }
        public string RetentionUser { get; set; }
        public string RetentionUserMobile { get; set; }
        public DateTime? JoinDate { get; set; }
        public int OrderDateDiff { get; set; }
        public bool MerchantAssignActive { get; set; }
        public bool IsBreakAble { get; set; }
        public bool AutoDownload { get; set; }
    }
    public class ActionUrl
    {
        public string ButtonName { set; get; }
        public string Url { set; get; }
    }
}
