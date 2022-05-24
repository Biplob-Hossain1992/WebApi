using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.DapperDataModel
{
    public class PickOrderDapperModel
    {
        public string CollectionName { get; set; }
        public int Status { get; set; }
        public string StatusNameBng { get; set; }
        public string OrderTrackStatusGroup { get; set; }
        public int StatusGroupId { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime ExpectedFirstDeliveryDate { get; set; }
        public string DistrictBng { get; set; }
        public string District { get; set; }
        public string PickDistrictBng { get; set; }
        public string HubName { get; set; }
        public string TrackingColor { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public bool TrackingFlag { get; set; }
        public int DistrictId { get; set; }
        public int PickDistrictId { get; set; }
        public decimal TrackingOrderBy { get; set; }
        public string CourierDeliveryManMobile { get; set; }
        public string CourierDeliveryManName { get; set; }
        public string CourierComment { get; set; }
        public string EDeshMobileNo { get; set; }
    }
}
