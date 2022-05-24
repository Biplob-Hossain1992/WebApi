using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("WeightRange", Schema = "DT")]
    public class WeightRange
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Weight { get; set; }
        [MaxLength(20)]
        [Required]
        public string Type { get; set; } = "deliverytiger";
        public int WeightNumber { get; set; } = 0;
        public decimal ExpressTypeCourierDeliveryCharge { get; set; } = 0;
        public decimal RegularTypeCourierDeliveryCharge { get; set; } = 0;
        public decimal ExtraCollectionCharge { get; set; } = 0;
    }
}
