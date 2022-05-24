using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("LoanApprovalPayment", Schema = "Loan")]
    public class LoanApprovalPayment
    {
        [Key]
        public int ApprovalPaymentId { get; set; }
        public int LoanSurveyId { get; set; }
        public int CourierUserId { get; set; }
        public decimal DTAcquiredAmount { get; set; } = 0;
        public decimal ThirdPartyAcquiredAmount { get; set; } = 0;
        public string TransactionId { get; set; } = "";
    }
}
