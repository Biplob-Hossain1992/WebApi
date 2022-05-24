using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class LoanDisbursementViewModel
    {
        // ******** Don't touch this model or don't add any property to this model ********
        // ******** This model is concerned to third party loan api ********
        public string TransactionID { get; set; } = "";
        public decimal DisbursementAmount { get; set; } = 0;
        public DateTime? DisbursementDate { get; set; }
        public decimal EmiAmount { get; set; } = 0;
        public string EmiDay { get; set; } = "10";
        public decimal CumulativeRecovery { get; set; } = 0;
        public decimal LastMonthRecoveryAmount { get; set; } = 0;
        public string MonthlyMaxEmiDate { get; set; } = "15";
        public int LoanApplicationId { get; set; } = 0;
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
        public int LoanDuration { get; set; } = 0;
    }
}
