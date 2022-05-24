using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CourierAssign", Schema = "Deal")]
    public class CourierAssign
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public int CourierId { get; set; }
        public int PremiumCourierId { get; set; }
        public bool IsActive { get; set; }
    }
}
