using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("LoanDisbursementHistory", Schema = "Loan")]
    public class LoanDisbursementHistory
    {
        [Key]
        public int Id { get; set; }
        public string RefTransactionID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; } = 0;
        public string PaymentMethod { get; set; } = "";
        public string PaymentTransactionID { get; set; } = "";
        public int LenderUserId { get; set; } = 0;
    }
}
