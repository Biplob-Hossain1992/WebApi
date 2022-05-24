using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails
{
    public class GetDeliveryChargeDetailsViewModel
    {

        public int Id { set; get; }
        public string CompositeId { set; get; }
        public int DistrictId { set; get; }
        public int ThanaId { set; get; }
        public int AreaId { set; get; }
        public decimal CourierDeliveryCharge { set; get; }
        public string Weight { set; get; }
        public string Type { set; get; }
        public string DistrictEng { set; get; }
        public string DistrictBng { set; get; }
        public string ThanaNameEng { set; get; }
        public string ThanaNameBng { set; get; }
        public string AreaNameEng { set; get; }
        public string AreaNameBng { set; get; }
        public string Name { set; get; }
        public int WeightRangeId { set; get; }
        public int DeliveryRangeId { set; get; }
        public string Day { set; get; }
        public int Ranking { get; set; }
        public string DayType { get; set; }
        public bool IsOpenBox { get; set; }
        public string OnImageLink { get; set; }
        public string OffImageLink { get; set; }
        public int ShowHide { get; set; }
        public string DeliveryAlertMessage { get; set; }
        public string LoginHours { get; set; }
        public string DateAdvance { get; set; }
        //public decimal CityDeliveryCharge { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal ExtraDeliveryCharge { get; set; }
        public decimal ExtraCollectionCharge { get; set; } 
    }
    public class WeightRangeWiseData
    {
        public int DeliveryRangeId { get; set; }
        public string DeliveryType { set; get; }
        public int WeightRangeId { set; get; }
        public decimal ChargeAmount { set; get; }
        public string Days { set; get; }
        public int Ranking { get; set; }
        public string DayType { get; set; }
        public String OnImageLink { get; set; }
        public String OffImageLink { get; set; }
        public int ShowHide { get; set; } 
        public string DeliveryAlertMessage { get; set; }
        public string LoginHours { get; set; }
        public string DateAdvance { get; set; }
        //public decimal CityDeliveryCharge { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal ExtraDeliveryCharge { get; set; }
        public string Type { get; set; }
        public decimal ExtraCollectionCharge { get; set; }
    }
    public class AreaWiseChargeDetailsViewModel
    {
        public int WeightRangeId { set; get; }
        public string Weight { set; get; }
        public bool IsOpenBox { get; set; }
        public List<WeightRangeWiseData> WeightRangeWiseData { set; get; }
        public List<DeliveryTypeModel> DeliveryTypeModel { set; get; }
    }
    public class DeliveryTypeModel
    {
        public string DeliveryType { set; get; }
        public List<DeliveryDayChargeModel> DeliveryDayChargeModel { set; get; }
    }
    public class DeliveryDayChargeModel
    {
        public int WeightRangeId { set; get; }
        public string DeliveryType { set; get; }
        public decimal ChargeAmount { set; get; }
        public string Days { set; get; }
        public string DayType { get; set; }
    }
}
