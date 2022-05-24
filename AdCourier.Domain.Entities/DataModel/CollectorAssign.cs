using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CollectorAssign", Schema = "DT")]
    public class CollectorAssign
    {
        [Key]
        [Column("CollectorAssignId")]
        public int CollectorAssignId { get; set; }
        public int CourierUserId { get; set; }
        public int CollectorId { get; set; }
        public string AssignType { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public int DistrictId { get; set; } = 0;
        public int ThanaId { get; set; } = 0;
        public int AreaId { get; set; } = 0;
    }
}
