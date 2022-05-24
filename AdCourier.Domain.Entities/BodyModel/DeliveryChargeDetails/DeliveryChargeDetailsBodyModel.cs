using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails
{
    public class DeliveryChargeDetailsBodyModel
    {
        public int DistrictId { set; get; }
        public int ThanaId { set; get; }
        public int AreaId { set; get; } = 0;
        public int WeightRangeId { get; set; }
        public string CourierOrderId { get; set; }
        public int DeliveryRangeId { get; set; }
        public String ServiceType { get; set; } = "alltoall";
        public Decimal ExtraCollectionCharge { get; set; } = 0;
    }
    public class DeliveryChargeDetailsSearchModel
    {
        public int DistrictId { set; get; }
        public int ThanaId { set; get; }
        public int WeightId { set; get; }
        public int DeliveryTypeId { set; get; }
    }
}
