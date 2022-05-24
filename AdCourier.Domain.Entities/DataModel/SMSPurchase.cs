using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("SMSPurchase", Schema = "DT")]
    public class SMSPurchase
    {
        [Key]
        [Column("SmsPurchaseId")]
        public int SmsPurchaseId { get; set; }
        public int CourierUserId { get; set; }
        public int BuySmsCount { get; set; }
        public decimal TotalSmsCharge { get; set; }
        public string TransactionId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}
