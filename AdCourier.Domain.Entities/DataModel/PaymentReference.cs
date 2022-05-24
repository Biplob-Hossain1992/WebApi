using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("PaymentReference", Schema = "DT")]
    public class PaymentReference
    {
        [Key]
        [Column("PaymentReferenceId")]
        public int PaymentReferenceId { get; set; }
        public int CourierId { get; set; } = 0;
        public decimal TotalCollectionAmount { get; set; } = 0;
        public string TransactionId { get; set; } = "";
        public int PostedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string Type { get; set; } = "";
        public string IsConfirmedBy { get; set; } = "";
    }
}
