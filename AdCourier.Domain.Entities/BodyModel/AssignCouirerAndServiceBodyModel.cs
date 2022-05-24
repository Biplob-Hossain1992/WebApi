using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel
{
    public class AssignCouirerAndServiceBodyModel
    {
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public int DeliveryRangeId { get; set; }
        public string ServiceType { get; set; }
        public bool IsActive { get; set; }
        public int CourierId { get; set; }
        public int UserId { get; set; }
        public int CourierUserId { get; set; }
    }
}
