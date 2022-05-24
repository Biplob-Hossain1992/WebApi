using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("LocationAssignHistory", Schema = "Log")]
    public class LocationAssignHistory
    {
        [Key]
        [Column("LocationAssignHistoryId")]
        public int LocationAssignHistoryId { get; set; }
        public int DeliveryUserId { get; set; }
        public int CollectorId { get; set; }
        public DateTime CurrentDate { get; set; } = DateTime.Now;
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public string DtDefaultAssign { get; set; }
        public string AdDefaultAssign { get; set; }
        public int ZoneId { get; set; } = 0;
        public int InsertedBy { get; set; } = 0;
        public int UpdatedBy { get; set; } = 0;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
