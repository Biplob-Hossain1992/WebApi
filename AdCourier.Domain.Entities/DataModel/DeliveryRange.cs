using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("DeliveryRange", Schema = "DT")]
    public class DeliveryRange
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(20)]
        [Required]
        public string Day { get; set; }
        public bool IsActive { get; set; } = true;
        public int Ranking { get; set; }
        public string DayType { get; set; } = "";
        public string OnImageLink { get; set; } = "";
        public string OffImageLink { get; set; } = "";
        public int ShowHide { get; set; } = 0;
        public string DeliveryAlertMessage { get; set; } = "";
        public string LoginHours { get; set; } = "";
        public string DateAdvance { get; set; } = "";
        public decimal CourierDeliveryCharge { get; set; }
        public string Type { get; set; }
        public int PriorityService { get; set; } = 0;
        public int AddCddDate { get; set; } = 0;
    }
}
