using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.OrderTracking
{
    public class CourierUsersLocationWiseSearchBodyModel : SearchBodyModel 
    {
        public int DistrictId { get; set; } = 0;
        public int ThanaId { get; set; } = 0;
        public int AreaId { get; set; } = 0;
    }
}
