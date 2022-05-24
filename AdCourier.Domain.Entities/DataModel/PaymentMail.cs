using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("PaymentMail", Schema = "Log")]
    public class PaymentMail
    {
        [Key]
        [Column("LogId")]
        public int LogId { get; set; }
        public string MerchantName { get; set; } = "";
        public int? MerchantId { get; set; }
        public string RegistrationPhoneNo { get; set; } = "";
        public string AccountHolderName { get; set; } = "";
        public string AccountNo { get; set; } = "";
        public string RoutingNo { get; set; } = "";
        public string BankName { get; set; } = "";
        public string BranchName { get; set; } = "";
        public DateTime PostedOn { get; set; } = DateTime.Now;
    }
}
