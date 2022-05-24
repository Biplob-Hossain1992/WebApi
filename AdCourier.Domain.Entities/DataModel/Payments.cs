using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Payments", Schema = "Deal")]
    public class Payments
    {
        [Key]
        [Column("PaymentId")]
        public int PaymentId { get; set; }
        public string CardType { get; set; }
        public char? PaymentType { get; set; }
        public string OnlineTransactionId { get; set; }
        public char? PaymentStatus { get; set; }

    }
}
