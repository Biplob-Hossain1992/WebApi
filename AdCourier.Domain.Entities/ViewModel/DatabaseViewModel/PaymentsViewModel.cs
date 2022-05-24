using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class PaymentsViewModel
    {
        public int PaymentId { get; set; }
        public string CardType { get; set; }
        public string PaymentType { get; set; }
        public string OnlineTransactionId { get; set; }
        public string PaymentStatus { get; set; }
    }
}
