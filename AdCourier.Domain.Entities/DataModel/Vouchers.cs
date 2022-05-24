using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Vouchers", Schema = "DT")]
    public class Vouchers
    {
        [Key]
        [Column("VoucherId")]
        public int VoucherId { get; set; }
        public string MerchantMobile { get; set; }
        public string VoucherCode { get; set; }
        public int ApplicableQuantity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal VoucherDiscount { get; set; }
        public int CourierUserId { get; set; }
        public bool IsActive { get; set; }
        public int DeliveryRangeId { get; set; }
        public int InsertBy { get; set; }
        public DateTime InsertedOn { get; set; } = DateTime.Now;
    }
}
