using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("RepaymentStatus", Schema = "Loan")]
    public class RepaymentStatus
    {
        [Key]
        public int RepaymentStatusId { get; set; }
        public DateTime? RepaymentDate { get; set; }
        public decimal Amount { get; set; } = 0;
        public string PaymentMethod { get; set; } = "";
        public string RefTransactionID { get; set; } = "";
        public string PaymentTransactionID { get; set; } = "";
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
        public int LenderUserId { get; set; } = 0;
    }
}
