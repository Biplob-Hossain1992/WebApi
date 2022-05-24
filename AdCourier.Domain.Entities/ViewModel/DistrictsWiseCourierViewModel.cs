using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel
{

    public class AssignCouirerAndServiceViewModel
    {
        public int DistrictId { get; set; }
        public string DistrictEng { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public int DeliveryRangeId { get; set; }
        public int CourierId { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string DayType { get; set; }
        public string CourierName { get; set; }
        public decimal CourierDeliveryCharge { get; set; }
        public string ServiceType { get; set; }
        public bool IsActive { get; set; }
        public int CourierUserId { get; set; }
        public string CompanyName { get; set; }
        public bool IsSpecial { get; set; }
    }
}
