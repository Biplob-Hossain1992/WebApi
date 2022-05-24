using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("DeliveryZone", Schema = "Deal")]
    public class DeliveryZone
    {
        [Key]
        [Column("ZoneId")]
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string ZoneType { get; set; }
        public int IsActive { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int HubId { get; set; }
    }
}