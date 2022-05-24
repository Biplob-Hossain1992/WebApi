using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class LoanDisbursementHistoryViewModel
    {
        public string RefTransactionID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; } = 0;
        public string PaymentMethod { get; set; } = "";
        public string PaymentTransactionID { get; set; }
    }
}
