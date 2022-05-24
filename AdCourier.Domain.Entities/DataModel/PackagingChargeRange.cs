using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("PackagingChargeRange", Schema = "DT")]
    public class PackagingChargeRange
    {
        [Key]
        [Column("PackagingChargeId")]
        public int PackagingChargeId { get; set; }
        public string PackagingName { get; set; }
        public decimal PackagingCharge { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
