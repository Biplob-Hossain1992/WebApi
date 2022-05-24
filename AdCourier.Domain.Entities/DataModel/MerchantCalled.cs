using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("MerchantCalled", Schema = "Retention")]
    public class MerchantCalled
    {
        [Key]
        [Column("MerchantCalledId")]
        public int MerchantCalledId { get; set; }
        public int CourierUserId { get; set; }
        public int AdminUserId { get; set; }
        public string Mobile { get; set; }
        public decimal CallDuration { get; set; }
        public string CalledSummary { get; set; }
        public DateTime CalledDate { get; set; } = DateTime.Now;
    }
}
