using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CourierLocation", Schema = "DT")]
    public class CourierLocation
    {
        [Key]
        [Column("CourierLocationId")]
        public int CourierLocationId { get; set; }
        public string LocationName { get; set; }
        public string CourierName { get; set; }
        public bool IsActive { get; set; }
    }
}
