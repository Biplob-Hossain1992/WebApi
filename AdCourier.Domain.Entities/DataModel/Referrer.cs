using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Referrer", Schema = "DT")]
    public class Referrer
    {
        [Key]
        [Column("ReferrerId")]
        public int ReferrerId { get; set; }
        public string OrderType { get; set; }
        public int ReferrerOrder { get; set; }
        public bool IsActive { get; set; }
        public int ReferrerUseDays { get; set; }
    }
}
