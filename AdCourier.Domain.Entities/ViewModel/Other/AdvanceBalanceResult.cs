using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Other
{
    public class AdvanceBalanceResult
    {
        public int MerchantId { get; set; }
        public string MerchantName { get; set; }
        public int AdvanceAmount { get; set; }
        public int AmountAdjusted { get; set; }
        public int Balance { get; set; }
        public int AccReceivable { get; set; }
    }
}
