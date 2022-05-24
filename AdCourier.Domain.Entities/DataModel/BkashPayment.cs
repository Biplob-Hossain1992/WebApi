using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("BkashPayment",Schema ="Log")]
    public class BkashPayment
    {
        [Key]
        [Column("BkashPaymentId")]
        public int BkashPaymentId { get; set; }
        public int OrderId { get; set; }
        public string PODNumber { get; set; } = "";
        public string TransactionId { get; set; } = "";
        public int CourierId { get; set; } = 0;
        public string PaymentType { get; set; } = "";
        public int PostedBy { get; set; } = 0;
        public int Status { get; set; } = 0;
        public string InvoiceNumber { get; set; } = "";
        public string AdminType { get; set; } = "";
        public decimal CollectionAmount { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
    }
}
