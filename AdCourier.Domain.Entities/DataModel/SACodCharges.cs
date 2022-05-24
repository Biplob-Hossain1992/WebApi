using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("SACodCharges", Schema = "DT")]
    public class SACodCharges
    {
        [Key]
        public int Id { get; set; }
        public decimal MinAmount { get; set; } = 0;
        public decimal MaxAmount { get; set; } = 0;
        public decimal CodCharge { get; set; } = 0;
        public decimal IntervalAmount { get; set; } = 0;
    }
}
