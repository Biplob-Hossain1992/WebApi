using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class RiderPaymentReportViewModel
    {
        public int RiderId { get; set; }
        public string RiderName { get; set; }
        public int MerchantCount { get; set; }
        public decimal Commision { get; set; }
        public int TotalAmount { get; set; }
    }
}
