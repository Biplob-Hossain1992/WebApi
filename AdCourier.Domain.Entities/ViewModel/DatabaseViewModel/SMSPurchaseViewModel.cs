using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class SMSPurchaseViewModel
    {
        public int CourierUserId { get; set; }
        public int BuySmsCount { get; set; }
        public decimal TotalSmsCharge { get; set; }
        public string TransactionId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }

    public class GetPurchasedSMSInfoViewModel
    {
        public int SmsPurchaseId { get; set; }
        public int CourierUserId { get; set; }
        public int SMSLeft { get; set; }
    }
}
