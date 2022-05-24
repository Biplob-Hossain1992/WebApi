using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("LoanDisbursement", Schema = "Loan")]
    public class LoanDisbursement
    {
        [Key]
        public int DisbursementId { get; set; }
        public string TransactionID { get; set; } = "";
        public decimal DisbursementAmount { get; set; } = 0;
        public DateTime? DisbursementDate { get; set; }
        public decimal EmiAmount { get; set; } = 0;
        public DateTime? EmiDate { get; set; }
        public decimal CumulativeRecovery { get; set; } = 0;
        public decimal LastMonthRecoveryAmount { get; set; } = 0;
        public DateTime? MonthlyMaxEmiDate { get; set; }
        public int LoanSurveyId { get; set; } = 0;
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
        public int LenderUserId { get; set; } = 0;
        public int RequiredTenorMonth { get; set; } = 0;
    }
}
