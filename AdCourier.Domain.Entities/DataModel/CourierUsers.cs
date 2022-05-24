using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CourierUsers", Schema = "DT")]
    public class CourierUsers
    {
        [Key]
        [Column("CourierUserId")]
        public int CourierUserId { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Mobile { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public string Address { set; get; } = "";
        public decimal SmsCharge { get; set; } = 0;
        public decimal MailCharge { get; set; } = 0;
        public decimal ReturnCharge { get; set; } = 0;
        public decimal CollectionCharge { get; set; } = 5;
        public decimal FirstCollectionCharge { get; set; } = 10;
        public bool IsSms { get; set; } = true;
        public bool IsEmail { get; set; } = true;
        public string EmailAddress { get; set; } = "";
        public string BkashNumber { get; set; } = "";
        public string AlterMobile { get; set; } = "";
        public string SourceType { get; set; } = "";
        public int RetentionUserId { get; set; } = 0;
        public int AcquisitionUserId { get; set; } = 0;
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public bool IsDocument { get; set; } = false;
        public string Remarks { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public bool IsCustomerSms { get; set; } = true;
        public bool IsCustomerEmail { get; set; } = true;
        public decimal MaxCodCharge { get; set; }
        public string Refreshtoken { get; set; }
        public bool IsAutoProcess { get; set; } = true;
        public string FirebaseToken { get; set; } = "";
        public decimal Credit { get; set; } = 100;
        public string FBURL { get; set; } = "";
        public string WebURL { get; set; } = "";
        public bool IsOfferActive { get; set; } = false;
        //public decimal OfferBkashDiscount { get; set; } = 10;
        public decimal OfferCodDiscount { get; set; } = 5;
        public int OfferType { get; set; } = 3;
        public decimal OfferBkashDiscountDhaka { get; set; } = 15;
        public decimal OfferBkashDiscountOutSideDhaka { get; set; } = 15;
        public decimal AdvancePayment { get; set; }
        public string KnowingSource { get; set; } = "";
        public string Priority { get; set; } = "";
        public string Referrer { get; set; } = "";
        public int ReferrerOrder { get; set; } = 0;
        public DateTime? ReferrerStartTime { get; set; }
        public DateTime? ReferrerEndTime { get; set; }
        public string OrderType { get; set; } = "";
        public int RefereeOrder { get; set; } = 0;
        public bool ReferrerIsActive { get; set; } = false;
        public DateTime? RefereeStartTime { get; set; }
        public DateTime? RefereeEndTime { get; set; }
        public string PreferredPaymentCycle { get; set; } = "";
        public string RegistrationFrom { get; set; } = "";
        public bool? IsBlock { get; set; } = false;
        public string BlockReason { get; set; } = "";
        public int WeightRangeId { get; set; } = 2;
        public int DeliveryRangeIdInside { get; set; } = 16;
        public int DeliveryRangeIdIOutside { get; set; } = 21;
        public int DistrictId { get; set; } = 0;
        public int ThanaId { get; set; } = 0;
        public DateTime? PreferredPaymentCycleDate { get; set; } = DateTime.Now;
        public string AccountName { get; set; } = "";
        public string AccountNumber { get; set; } = "";
        public string BankName { get; set; } = "";
        public string BranchName { get; set; } = "";
        public string RoutingNumber { get; set; } = "";
        public bool? IsQuickOrderActive { get; set; } = false;
        public int? CustomerSMSLimit { get; set; } = 0;
        public int? CustomerVoiceSmsLimit { get; set; } = 0;
        public int? ViberSMSLimit { get; set; } = 0;
        public bool? IsLoanActive { get; set; } = false;
        public string LoanCompany { get; set; } = "";
        public string Gender { get; set; } = "";
        public int CategoryId { get; set; } = 0;
        public int SubCategoryId { get; set; } = 0;
        public bool? IsBreakAble { get; set; } = false;
        public bool IsTeleSales { get; set; }
        public DateTime? TeleSalesDate { get; set; }
        public bool? IsHeavyWeight { get; set; } = false;
        public int? TeleSales { get; set; } = 0;
        public bool MerchantAssignActive { get; set; } = false;
        [Column(TypeName = "nvarchar(max)")]
        public string HashPassword { get; set; }
        public int MerchantReview { get; set; } = 0;
        public int Recommendation { get; set; } = 0;
        public byte? PaymentServiceType { get; set; }
        public decimal? PaymentServiceCharge { get; set; }
        public decimal? CollectionAmountLimt { get; set; }
        public decimal? CollectionAmountMInLimt { get; set; }
        public string Verify { get; set; } = "";
        public decimal? CodChargeDhaka { get; set; } = 5;
        public decimal? CodChargeOutsideDhaka { get; set; } = 15;
        public decimal? CodChargePercentageDhaka { get; set; } = 1;
        public decimal? CodChargePercentageOutsideDhaka { get; set; } = 1;
        public byte? CodChargeTypeFlag { get; set; } = 1;
        public byte? CodChargeTypeOutsideFlag { get; set; } = 2;
        public bool AutoDownload { get; set; } = true;
        public string AutoDownloadPohStatus { get; set; } = "10";
        public bool IsInstaCod { get; set; } = false;
        public bool IsDana { get; set; } = false;
        public bool IsOpenBox { get; set; } = false;
    }
}
