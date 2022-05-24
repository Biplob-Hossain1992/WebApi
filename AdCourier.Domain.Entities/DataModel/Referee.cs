using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Referee", Schema = "DT")]
    public class Referee
    {
        [Key]
        [Column("RefereeId")]
        public int RefereeId { get; set; }
        public string OrderType { get; set; }
        public int RefereeOrder { get; set; }
        public bool IsActive { get; set; }
        public int RefereeUseDays { get; set; }
    }
}
