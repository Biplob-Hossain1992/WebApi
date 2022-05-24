using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CustomerSMS", Schema = "Log")]
    public class CustomerSMS
    {
        [Key]
        [Column("CustomerSMSId")]
        public int CustomerSMSId { get; set; }
        public int CourierUserId { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime MessageDate { get; set; } = DateTime.Now;
        public int SmsPurchaseId { get; set; }
        public string SmsType { get; set; } = "";
    }
}
