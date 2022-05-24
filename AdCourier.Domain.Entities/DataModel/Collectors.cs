using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Collectors", Schema = "AD")]
    public class Collectors
    {
        [Key]
        [Column("CollectorId")]
        public int CollectorId { get; set; }
        public string CollectorName { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public bool IsTemporary { get; set; }
    }
}
