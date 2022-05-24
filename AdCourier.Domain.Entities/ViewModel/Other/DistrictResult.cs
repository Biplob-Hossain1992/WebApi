using System.Collections.Generic;

namespace AdCourier.Domain.Entities.ViewModel.Other
{
    public class DistrictResult
    {
        public class Result
        {
            public string MessageCode { get; set; }
            public string MessageText { get; set; }
            public bool DatabseTracking { get; set; }
            public Data Data { get; set; }
        }

        public class Data
        {
            public List<DistrictModel> DistrictInfo { set; get; }
        }
        public class DistrictModel
        {
            public int DistrictPriority { get; set; }
            public int ParentId { get; set; }
            public bool IsCity { get; set; }
            public int IsCourier { get; set; }
            public string PostalCode { get; set; } = "0";
            public int ThirdPartyLocationId { get; set; }
            public int HasArea { get; set; }
            public int DTHasExpressDelivery { get; set; }
            public int HasExpressDelivery { get; set; }
            public int DeliveryPaymentType { get; set; }

            public int DistrictId { set; get; }
            public string District { set; get; } = "";
            public int DeliveryCharge { set; get; }
            public string DistrictBng { set; get; } = "";
            public byte IsAdvPaymentActive { set; get; }
            public int AppDeliveryCharge { set; get; }
            public int AppMultipleOrderDeliveryCharge { set; get; }
            public int AppAdvPaymentDeliveryCharge { set; get; }

            public IEnumerable<ThanaModel> ThanaHome { get; set; }
            public IEnumerable<ThanaModel> ThanaShundarban { get; set; }
        }

        public class ThanaModel
        {
            public int ThanaId { set; get; }
            public string Thana { set; get; }
            public int DeliveryCharge { set; get; }
            public string ThanaBng { set; get; }
            public string PostalCode { get; set; } = "0";
            public int IsPostalAddress { get; set; }
            public byte IsAdvPaymentActive { set; get; }
            public int AppDeliveryCharge { set; get; }
            public int AppMultipleOrderDeliveryCharge { set; get; }
            public int AppAdvPaymentDeliveryCharge { set; get; }
            public int ParentId { get; set; }
            public bool IsCity { get; set; }
            public int IsCourier { get; set; }
            public int ThanaPriority { get; set; }
            public int HasArea { get; set; }
            public int ThirdPartyLocationId { get; set; }
            public int DTHasExpressDelivery { get; set; }
            public int HasExpressDelivery { get; set; }
            public string SpecialDeliverySpeed { get; set; }
            public int DeliveryPaymentType { get; set; } = 0;
        }

        public class AreaModel
        {
            public int AreaId { set; get; }
            public string PostalCode { get; set; } = "0";
            public int IsPostalAddress { get; set; }
            public string Area { set; get; }
            public int DeliveryCharge { set; get; }
            public string AreaBng { set; get; }
            public byte IsAdvPaymentActive { set; get; }
            public int AppDeliveryCharge { set; get; }
            public int AppMultipleOrderDeliveryCharge { set; get; }
            public int AppAdvPaymentDeliveryCharge { set; get; }
            public int ParentId { get; set; }
            public bool IsCity { get; set; }
            public int IsCourier { get; set; }
            public int AreaPriority { get; set; }
            public int DistrictId { get; set; }
            public int DTHasExpressDelivery { get; set; }
            public int HasExpressDelivery { get; set; }
            public int DeliveryPaymentType { get; set; }

        }
    }
}
