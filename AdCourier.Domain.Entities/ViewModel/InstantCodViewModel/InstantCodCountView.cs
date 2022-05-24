using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.InstantCodViewModel
{
    public class InstantCodCountView
    {
        public int UnCollectedOrdersCount { get; set; }
        public int SaReceiptCollectionCount { get; set; }
        public decimal SaReceiptCollectionAmount { get; set; }
    }
}
