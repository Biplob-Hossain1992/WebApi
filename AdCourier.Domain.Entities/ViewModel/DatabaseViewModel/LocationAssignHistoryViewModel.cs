using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class LocationAssignHistoryViewModel
    {
        public int LocationAssignId { get; set; }
        public int DeliveryUserId { get; set; }
        public int CollectorId { get; set; }
        public DateTime? CurrentDate { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public string DtDefaultAssign { get; set; }
        public string AdDefaultAssign { get; set; }
        public int ZoneId { get; set; }
        public string FullName { get; set; } = "";
        public DateTime? UpdatedOn { get; set; }
    }
}
