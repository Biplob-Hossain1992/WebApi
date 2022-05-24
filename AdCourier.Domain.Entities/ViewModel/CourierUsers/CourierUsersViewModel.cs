using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.OrderRequest;
using Retention.Domain.Entities.ViewModel;
using System;
using System.Collections.Generic;

namespace AdCourier.Domain.Entities.ViewModel.CourierUsers
{
    public class CourierUsersViewModel
    {
        public int CourierUserId { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; } = true;
        public bool? IsBlock { get; set; } = false;
        public string Address { set; get; } = "";
        public decimal CollectionCharge { get; set; }
        public decimal ReturnCharge { get; set; }
        public decimal SmsCharge { get; set; }
        public decimal MailCharge { get; set; }
        public bool Sms { get; set; }
        public bool Email { get; set; }
        public string EmailAddress { get; set; }
        public string BkashNumber { get; set; }
        public string AlterMobile { get; set; }
        public int[] StatusId { get; set; } = new int[] { 0, 0, 0 };
        public decimal MaxCodCharge { get; set; }
        public bool IsAutoProcess { get; set; }
        public decimal Credit { get; set; }
        public string FBURL { get; set; } = "";
        public string WebURL { get; set; } = "";
        public bool IsOfferActive { get; set; }
        public decimal OfferCodDiscount { get; set; }
        public int OfferType { get; set; }
        public int DistrictId { get; set; } = 0;
        public int ThanaId { get; set; } = 0;
        public int AreaId { get; set; } = 0;
        public string DistrictName { get; set; } = "";
        public string ThanaName { get; set; } = "";
        public string AreaName { get; set; } = "";

        public bool IsSms { get; set; }
        public bool IsEmail { get; set; }

        public string SourceType { get; set; }
        public int RetentionUserId { get; set; }
        public int AcquisitionUserId { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsDocument { get; set; }
        public string Remarks { get; set; }
        public bool IsCustomerSms { get; set; }
        public bool IsCustomerEmail { get; set; }
        public string Refreshtoken { get; set; }
        public string FirebaseToken { get; set; } = "";

        public decimal OfferBkashDiscountDhaka { get; set; }
        public decimal OfferBkashDiscountOutSideDhaka { get; set; }
        public decimal AdvancePayment { get; set; }
        public string KnowingSource { get; set; }
        public int? CustomerSMSLimit { get; set; } = 0;
        public int? CustomerVoiceSmsLimit { get; set; }
        public int? ViberSMSLimit { get; set; }
        public bool? IsLoanActive { get; set; } = false;
        public string LoanCompany { get; set; } = "";
        public string Gender { get; set; } = "";
        public int CategoryId { get; set; } = 0;
        public int SubCategoryId { get; set; } = 0;

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
        public string PreferredPaymentCycle { get; set; }
        public string RegistrationFrom { get; set; }
        public string BlockReason { get; set; }
        public int WeightRangeId { get; set; }
        public int DeliveryRangeIdInside { get; set; }
        public int DeliveryRangeIdIOutside { get; set; }
        public DateTime? PreferredPaymentCycleDate { get; set; }
        public bool? IsQuickOrderActive { get; set; }
        public bool? IsBreakAble { get; set; }
        public bool? IsHeavyWeight { get; set; }
        public bool IsTeleSales { get; set; }
        public DateTime? TeleSalesDate { get; set; }
        public int? TeleSales { get; set; } = 0;
        public byte? PaymentServiceType { get; set; }
        public decimal? PaymentServiceCharge { get; set; }
        public decimal? CollectionAmountLimt { get; set; }
        public decimal? CollectionAmountMInLimt { get; set; }
        public bool? MerchantAssignActive { get; set; }
        public decimal? CodChargeDhaka { get; set; } = 0;
        public decimal? CodChargeOutsideDhaka { get; set; } = 0;
        public decimal? CodChargePercentageDhaka { get; set; } = 0;
        public decimal? CodChargePercentageOutsideDhaka { get; set; } = 0;
        public byte? CodChargeTypeFlag { get; set; } = 1;
        public byte? CodChargeTypeOutsideFlag { get; set; } = 2;
        public bool IsInstaCod { get; set; }
        public bool IsOpenBox { get; set; }
        public List<PickupLocations> PickupLocationList { get; set; }
        //public OrderRequestViewModel OrderRequestViewModel { get; set; }
        public List<OrderRequestViewModel> OrderRequestList { get; set; }
        public virtual CourierOrdersViewModel CourierOrders { get; set; }
        public virtual AdminUsersViewModel AdminUsers { get; set; }
        public virtual DistrictsViewModel DistrictsViewModel { get; set; } = new DistrictsViewModel();
        public virtual RetentionMerchantOrderViewModel RetentionMerchantOrder { get; set; }
        public virtual VouchersViewModel VouchersViewModel { get; set; }
        public virtual DeliveryRange DeliveryRange { get; set; }
        public List<ActionModel> ActionModel { set; get; }
        public List<TeleSaleCourierUsers> TeleSaleCourierUsers { get; set; } = new List<TeleSaleCourierUsers>();
    }
    public class CourierUsersInfoViewModel
    {
        public int TotalUsers { set; get; }
        public List<AdCourier.Domain.Entities.DataModel.CourierUsers> CourierUsers { set; get; }
    }
    public class CourierUserPickupLocations : PickupLocations
    {
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string Address { set; get; } = "";
        public string Mobile { get; set; }
    }
    public class CourierUserPickupLocationModel
    {
        public int TotalCount { get; set; }
        public List<CourierUserPickupLocations> PickUpLocations { get; set; }
    }
    public class TelesalesActiveCourierUserModel
    {
        public int TotalOrder { get; set; }
        public string FullName { get; set; }
        public int RetentionUserId { get; set; }
        public int CourierUserId { get; set; }
        public string CompanyName { get; set; }
        public string Mobile { get; set; }
        public string AlterMobile { get; set; }
        public string BkashNumber { get; set; }
    }
}
