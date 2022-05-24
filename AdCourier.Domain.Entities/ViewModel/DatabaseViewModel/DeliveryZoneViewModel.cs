using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class DeliveryZoneViewModel
    {
        public int ZoneId { get; set; } = 0;
        public string ZoneName { get; set; } = "";
        public string ZoneType { get; set; }
        public int IsActive { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int HubId { get; set; }
        public int TotalOrder { get; set; }
        public int TotalMerchant { get; set; }
    }
}
