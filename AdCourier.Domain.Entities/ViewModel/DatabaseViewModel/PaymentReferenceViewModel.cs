using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class PaymentReferenceViewModel
    {
        public int PaymentReferenceId { get; set; }
        public int CourierId { get; set; } = 0;
        public decimal TotalCollectionAmount { get; set; } = 0;
        public string TransactionId { get; set; } = "";
        public int PostedBy { get; set; }   
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string Type { get; set; } = "";
        public string IsConfirmedBy { get; set; } = "";
        public PaymentReferenceDetails OrderDetails { get; set; }
    }
}
