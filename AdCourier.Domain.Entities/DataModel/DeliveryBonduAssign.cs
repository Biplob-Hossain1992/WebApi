using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("DeliveryBonduAssign", Schema = "DT")]
    public class DeliveryBonduAssign
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public int CourierUserId { get; set; } = 0;
        public int DeliveryManUserId { get; set; }
        public string AssignType { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public int OrderId { get; set; } = 0;
    }
}
