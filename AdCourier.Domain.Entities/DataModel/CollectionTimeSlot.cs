using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CollectionTimeSlot", Schema = "DT")]
    public class CollectionTimeSlot
    {
        [Key]
        [Column("CollectionTimeSlotId")]
        public int CollectionTimeSlotId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int Ordering { get; set; }
        public bool IsActive { get; set; }
        public int OrderLimit { get; set; } = 0;
        public TimeSpan? CutOffTime { get; set; }
        public string SlotName { get; set; } = "";
    }
}
