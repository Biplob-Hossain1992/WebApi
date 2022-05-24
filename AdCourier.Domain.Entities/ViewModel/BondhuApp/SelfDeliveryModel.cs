using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.BondhuApp
{
    //transfer from ajkerdealApi
    public class SelfDeliveryModel
    {
        public string Message { set; get; }
        public int CustomerId { set; get; }
    }
    public class StatusModel
    {
        public string StatusName { set; get; }
        public string Status { set; get; }
        public string DtStatus { get; set; } = "";
        public int Flag { set; get; }
        public int CollectionFilter { set; get; } = 0;
        public string[] Type { get; set; }
        public string CustomType { get; set; } = "no";
        public bool isUnavailableShow { get; set; } = false;
        public bool allowLocationAdd { get; set; } = false;
        public bool allowPrint { get; set; } = false;
        public bool AllowImageUpload { get; set; } = false;
        public bool IsCollectionTimerShow { get; set; } = false;
        public bool IsWeightUpdateEnable { get; set; } = false;
    }

    public class FeaturesResponse
    {
        public string BannerUrl { set; get; }
        public string WebUrl { set; get; }
    }
    public class DeliveryManRegistration
    {
        public string Name { set; get; } = "";
        public string Mobile { set; get; } = "";
        public string AlternativeMobile { set; get; } = "";
        public string BkashMobileNumber { set; get; } = "";
        public int DeliveryCharge { set; get; } = 25;
        public string Password { set; get; } = "";
        public int IsActive { set; get; } = 1;
        public string Address { set; get; } = "";
        public int UserId { set; get; } = 0;
        public int Commission { set; get; } = 0;
        public int UserType { set; get; } = 0;
        public int DistrictId { set; get; } = 0;
        public string DistrictName { set; get; } = "";
        public int ThanaId { set; get; } = 0;
        public string ThanaName { set; get; } = "";
        public int AreaId { set; get; } = 0;
        public string AreaName { set; get; } = "";
        public int PostCode { set; get; } = 0;
    }
    public class DeliveryManGeneralInfo
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Mobile { set; get; }
        public string AlternativeMobile { set; get; }
        public string BkashMobileNumber { set; get; }
        public string Address { set; get; }
        public int DistrictId { set; get; } = 0;
        public string DistrictName { set; get; } = "";
        public int ThanaId { set; get; } = 0;
        public string ThanaName { set; get; } = "";
        public int PostCode { set; get; } = 0;
        public int AreaId { set; get; } = 0;
        public string AreaName { set; get; } = "";

    }
    public class DeliveryManGeneralInfoUpdate
    {
        public int BondhuId { set; get; }
        public string Name { set; get; }
        public string Mobile { set; get; }
        public string AlternativeMobile { set; get; }
        public string BkashMobileNumber { set; get; }
        public string Address { set; get; }
        public List<DeliveryManAreaInfoResponse> AreaInfo { set; get; }

    }
    public class DeliveryManAreaInfoUpdate
    {
        public int DistrictId { set; get; } = 0;
        public string DistrictName { set; get; } = "";
        public int ThanaId { set; get; } = 0;
        public string ThanaName { set; get; } = "";
        public int PostCode { set; get; } = 0;
        public int AreaId { set; get; } = 0;
        public string AreaName { set; get; } = "";

    }
    public class DeliveryManInfoResponse
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Mobile { set; get; }
        public string AlternativeMobile { set; get; }
        public string BkashMobileNumber { set; get; }
        public string Address { set; get; }

        public int DistrictId { set; get; } = 0;
        public string DistrictName { set; get; } = "";
        public int ThanaId { set; get; } = 0;
        public string ThanaName { set; get; } = "";
        public int PostCode { set; get; } = 0;
        public int AreaId { set; get; } = 0;
        public string AreaName { set; get; } = "";
        public bool IsProfileImage { set; get; } = false;
        public bool IsDrivingLicense { set; get; } = false;
        public bool IsNID { set; get; } = false;
    }
    public class DeliveryManGeneralInfoResponse
    {
        public int BondhuId { set; get; }
        public string Name { set; get; }
        public string Mobile { set; get; }
        public string AlternativeMobile { set; get; }
        public string BkashMobileNumber { set; get; }
        public string Address { set; get; }
        public bool IsProfileImage { set; get; } = false;
        public bool IsDrivingLicense { set; get; } = false;
        public bool IsNID { set; get; } = false;
        public SelfDeliveryImageInfo imageInfo { set; get; }
        public List<DeliveryManAreaInfoResponse> AreaInfo { set; get; }

    }
    public class SelfDeliveryImageInfo
    {
        public string Nid { set; get; }
        public string ProfileImage { set; get; }
        public string DrivingImage { set; get; }
    }
    public class DeliveryManAreaInfoResponse
    {
        public int DistrictId { set; get; } = 0;
        public string DistrictName { set; get; } = "";
        public int ThanaId { set; get; } = 0;
        public string ThanaName { set; get; } = "";
        public int PostCode { set; get; } = 0;
        public int AreaId { set; get; } = 0;
        public string AreaName { set; get; } = "";

    }
    public class LoadBondhuInfoModel
    {
        public int BondhuId { set; get; }
    }

    public class SelfDeliveryLoginModel
    {
        public string MobileNumber { set; get; }
        public string Password { set; get; }
        public string FirebaseToken { set; get; }
    }
    public class SelfDeliveryLoginResponseModel
    {
        public int DeliveryUserId { set; get; } = 0;
        public int IsActive { set; get; } = 0;
        public string MobileNumber { set; get; } = "";
        public string BkashMobileNumber { set; get; } = "";
        public string ProfileImage { set; get; } = "";
        public bool IsProfileImage { set; get; } = false;
        public string DeliveryUserName { set; get; } = "";
        public string Message { set; get; } = "";
        public string FirebaseToken { set; get; } = "";
    }
    public class LatLagModel
    {
        public int MerchantId { get; set; }
        public int DeliveryUserId { get; set; }
        public string Latitude { set; get; }
        public string Longitude { set; get; }
        public string Confirmation { get; set; }
    }
    public class MerchantLatLagModel
    {
        public int CourierUserId { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public string Latitude { set; get; }
        public string Longitude { set; get; }
    }
    public class UserLatLagModel
    {
        public int BondhuId { set; get; }
        public string Latitude { set; get; }
        public string Longitude { set; get; }
    }
    public class UserAccessModel
    {
        public int BondhuId { set; get; }
        public bool IsNowOffline { set; get; }
        public int IsUserModeChange { set; get; }
    }
    public class UserAccessResponseModel
    {
        public string RiderType { get; set; }
        public int? UserType { set; get; }
        public bool IsNowOffline { set; get; }
        public int LocationUpdateIntervalInMinute { set; get; } = 20;
        public int LocationDistanceInMeter { set; get; } = 20;
        public string ProfileImage { set; get; } = "";
        public bool? IsProfileImage { set; get; } = false;
        public bool? IsDrivingLicense { set; get; } = false;
        public bool? IsNID { set; get; } = false;
        public string Name { set; get; } = "";
        public string Mobile { set; get; } = "";

    }
    public class SelfDeliveryOrderRequestModel
    {
        public int DeliveryUserId { set; get; }
        public string ProductTitle { set; get; }
        public string StatusId { set; get; }
        public string DtStatusId { set; get; } = "40";
        public int Index { set; get; }
        public int Count { set; get; }
        public int Flag { set; get; }
    }

    public class SelfDeliveryOrderRequestNewModel
    {
        public int DeliveryUserId { set; get; }
        public string ProductTitle { set; get; }
        public string StatusId { set; get; }
        public string DtStatusId { set; get; }
        public int Index { set; get; }
        public int Count { set; get; }
        public int Flag { set; get; }
        public string Type { get; set; }
        public string CustomType { get; set; }
        public string RiderType { get; set; } = "";
        public string OrderId { get; set; }
        public int CollectionSlotId { get; set; }
        public string FromDate { get; set; } = "";
        public string ToDate { get; set; } = "";
    }
    public class PodSelfDeliveryOrderRequestModel
    {
        public int DeliveryUserId { set; get; }
        public string StatusId { set; get; }
        public string PodNumber { set; get; }
        public int Index { set; get; }
        public int Count { set; get; }
        public int Flag { set; get; }
    }
    public class SelfDeliveryOrderResponseModel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int CollectAddressDistrictId { get; set; }
        public int CollectAddressThanaId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string CustomerId { set; get; }
        public string CustomerName { set; get; }
        public string CustomerMobileNumber { set; get; }
        public string CustomerAddress { set; get; }
        public string CouponId { set; get; }
        public string ProductTitle { set; get; }
        public int CouponPrice { set; get; }
        public int ProductQtn { set; get; }
        public string Colors { get; set; }
        public string FolderName { set; get; }
        public int StatusId { set; get; }
        //public int? TotalCount { set; get; }
        public int IsDone { set; get; }
        public string Comments { set; get; }
        public int BondhuCharge { set; get; }
        public DateTime OrderDate { set; get; }
        public int MerchantId { set; get; }
        public string DealId { set; get; }
        public DateTime DeliveryDate { set; get; }
        public int CommentedBy { set; get; }
        public string PODNumber { set; get; }
        public string HubName { set; get; }
        public string HubAddress { get; set; }
        public string SourcePersonName { set; get; }
        public string SourceAddress { set; get; }
        public string SourceMobile { set; get; }
        public int SourceDealPrice { set; get; }
        public string PaymentType { set; get; }
        public int DeliveryCharge { set; get; }
        public int DeliveryManCommission { set; get; }
        public int CouponDeliveryManCommission { set; get; }
        public string DeliveryManAddress { set; get; }
        public string DeliveryManName { set; get; }
        public string PodPickUpAddress { set; get; }
        public int CollectionPointId { set; get; }
        public string District { set; get; }
        public string CollectDistrict { get; set; }
        public string OrderType { get; set; }
        public bool IsAdvancePayment { get; set; }
        public int CollectionAmount { get; set; }
        public string Sizes { get; set; }
        public string AlterMobile { get; set; }
        public string DeliveryRangeName { get; set; }
        public int RiderAcceptOrder { get; set; }
        public DateTime RiderAcceptDate { get; set; }
        public int WeightRangeId { get; set; }
        public int DeliveryRangeId { get; set; }
        public int PriorityService { get; set; }
        public string Weight { get; set; }
        public bool IsHeavyWeight { get; set; }
        public string ServiceType { get; set; }
        public string DocumentUrl { get; set; }
        public int CollectionTimeSlotId { get; set; }
        public int PaymentServiceType { get; set; } = 0;
    }

    public class SelfDeliveryAllDataResponseModel
    {
        public int? TotalCount { set; get; }
        public List<CustomerOrderResponseModel> customerOrderResponseModel { set; get; }

    }

    public class PodWiaeResponseModel
    {
        public int? TotalCount { set; get; }
        public List<PodResponseModel> PodWiseDataModel { set; get; }

    }

    public class PodResponseModel
    {
        public string PodNumber { set; get; }
        public string CollectionAddress { set; get; }
        public int TotalPodCommission { set; get; }
        public int TotalCustomer { set; get; }
        public List<ActionData> Actions { set; get; }
        public List<CustomerOrderResponseModel> CustomerDataModel { set; get; }
    }
    public class CustomerOrderResponseModel
    {
        public int MerchantId { get; set; }
        public int CollectAddressDistrictId { get; set; }
        public int CollectAddressThanaId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Id { set; get; }
        public string Name { set; get; }
        public string District { set; get; }
        public string MobileNumber { set; get; }
        public string AlterMobile { set; get; }
        public string Address { set; get; }
        public int DeliveryCommission { set; get; }
        public int BondhuCharge { set; get; }
        public int TotalOrder { set; get; }
        public int TotalPayment { set; get; }
        public string ServiceType { get; set; }
        public List<ActionData> Actions { set; get; }
        public CollectionSource SourceInfo { set; get; }
        public List<CustomerOrderData> CustomerOrderDataModel { set; get; }
        public CustomerMessage CustomerMessageData { set; get; }
    }

    //public class CollectionTimeSlot
    //{
    //    public TimeSpan StartTime { get; set; }
    //    public TimeSpan EndTime { get; set; }
    //}
    public class CustomerOrderData
    {
        public int PriorityService { get; set; }
        public string Colors { get; set; }
        public string Sizes { get; set; }
        public string CouponId { set; get; }
        public string ProductTitle { set; get; }
        public int ProductPrice { set; get; }
        public string DeliveryType { get; set; }
        public bool IsAdvancePayment { get; set; }
        public int ProductQtn { set; get; }
        public string ImageUrl { set; get; }
        public int StatusId { set; get; }
        public int IsDone { set; get; }
        public string Comments { set; get; }
        public int BondhuCharge { set; get; }
        public DateTime OrderDate { set; get; }
        public int MerchantId { set; get; }
        public int TotalPayment { set; get; } = 0;
        public string DealId { set; get; }
        public DateTime DeliveryDate { set; get; }
        public int CommentedBy { set; get; }
        public string PODNumber { set; get; }
        public string HubName { set; get; }
        public CollectionSource SourceInfo { set; get; }
        public List<ActionData> Actions { set; get; }
        public CollectionTimeSlot CollectionTimeSlot { set; get; }
        public CustomerMessage CustomerMessageData { set; get; }
        public int CollectionPointId { set; get; }
        public string CustomerId { set; get; }
        public int DeliveryCharge { set; get; }
        public int DeliveryRangeId { get; set; }
        public int WeightRangeId { get; set; }
        public bool IsHeavyWeight { get; set; }
        public string DocumentUrl { get; set; }
        public int IsPohOrder { get; set; } = 0;
    }

    public class CollectionSource
    {
        public string SourcePersonName { set; get; }
        public string SourceAddress { set; get; }
        public string SourceMobile { set; get; }
        public int SourceDealPrice { set; get; }
        public SourceMessage SourceMessageData { set; get; }
    }

    public class SourceMessage
    {
        public string Message { set; get; } = "";
        public string Instructions { set; get; } = "";
        public string Status { set; get; } = "";
        public int IsPay { set; get; } = -1;
    }
    public class CustomerMessage
    {
        public string Message { set; get; }
        public string Instructions { set; get; }
    }

    public class ActionModel
    {
        public string ButtonName { set; get; }
        public int StatusUpdate { set; get; }
        public string StatusMessage { set; get; }
        public string ColorCode { set; get; }
    }
    public class ActionData
    {
        public int ActionType { set; get; }
        public string ActionMessage { set; get; }
        public int UpdateStatus { set; get; }
        public string StatusMessage { set; get; }
        public string ColorCode { set; get; }
        public int CollectionPointAvailable { set; get; } = 0;
        public string Icon { set; get; } = "";
        public int IsPaymentType { set; get; } = 0;
        public int PopUpDialog { get; set; } = 0;
    }

    public class ProductCollectionResponse
    {
        public string ImageUrl { set; get; }
        public string ProductTitle { set; get; }
        public int ProductPrice { set; get; }
        public int ProductQtn { set; get; }
        public int IsPay { set; get; } = -1;
    }

    public class ProductCollectionRequest
    {
        public int CouponId { set; get; }
        public int DeliveryUserId { set; get; }
        public int CollectionPointId { set; get; }
    }

    public class SelfDeliveryFirebaseTokenUpdateModel
    {
        public string FirebaseToken { set; get; }
        public int Id { set; get; }
    }

    public class SelfDeliveryUserPasswordUpdateModel
    {
        public string Password { set; get; }
        public int CustomerId { set; get; }
        public string Mobile { get; set; }
    }

    public class SelfDeliveryUserSearhByMobileModel
    {
        public string MobileNumber { set; get; }

    }
    public class SelfDeliveryProfileImageModel
    {
        public int BondhuId { set; get; }
        public bool IsProfileImage { set; get; } = false;
        public bool IsDrivingLicense { set; get; } = false;
        public bool IsNID { set; get; } = false;

    }
    public static class BaseUrlModel
    {
        //public static string ImageBaseUrl => "https://d2uqwduki1f2hj.cloudfront.net/";
        //public static string ImageBaseExtendedUrl => "https://d2uqwduki1f2hj.cloudfront.net/images/deals/";
        public static string ImageBaseUrl => "https://static.ajkerdeal.com/";
        public static string ImageBaseExtendedUrl => "https://static.ajkerdeal.com/images/deals/";
        public static string DesktopBaseUrl => "https://ajkerdeal.com/";
        public static string MobileBaseUrl => "https://m.ajkerdeal.com/";
        public static string MerchantBaseUrl => "https://www.ajkerdeal.com/";
        public static string S3Url => "https://s3.us-east-2.amazonaws.com/ajkerdeal-images-ohio-1/";
        public static string ElasticSearchUrl => "https://es2.ajkerdeal.com/";
        public static string AjkerdealApiLink => "https://api.ajkerdeal.com/";
    }
}
