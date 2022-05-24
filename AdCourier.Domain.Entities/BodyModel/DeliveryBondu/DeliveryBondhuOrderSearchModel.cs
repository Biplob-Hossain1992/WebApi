using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.DeliveryBondu
{
    public class DeliveryBondhuOrderSearchModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int DeliveryManId { get; set; } = 0;
        public string Status { get; set; } = "-1";
        public string DateType { get; set; }
        public int CollectionTimeSlotId { get; set; } = 0;
    }
}
