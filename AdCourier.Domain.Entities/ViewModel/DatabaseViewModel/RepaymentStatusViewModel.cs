using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class RepaymentStatusViewModel
    {
        public DateTime? RepaymentDate { get; set; }
        public decimal Amount { get; set; } = 0;
        public string PaymentMethod { get; set; } = "";
        public string RefTransactionID { get; set; } = "";
        public string PaymentTransactionID { get; set; } = "";
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
