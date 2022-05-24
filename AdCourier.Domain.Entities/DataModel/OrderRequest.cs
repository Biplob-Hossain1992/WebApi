using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AdCourier.Domain.Entities.DataModel
{
    [Table("OrderRequest", Schema = "DT")]
    public class OrderRequest
    {
        [Key]
        [Column("OrderRequestId")]
        public int OrderRequestId { get; set; }
        public int CourierUserId { get; set; }
        public int RequestOrderAmount { get; set; } = 0;
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public int DistrictId { get; set; } = 0;
        public int ThanaId { get; set; } = 0;
        public DateTime CollectionDate { get; set; } = DateTime.Now;
        public int CollectionTimeSlotId { get; set; } = 0;
        public int DeliveryUserId { get; set; } = 0;
        public int Status { get; set; } = 0;
        public DateTime? DeliveryUserActionDate { get; set; }
    }
}
