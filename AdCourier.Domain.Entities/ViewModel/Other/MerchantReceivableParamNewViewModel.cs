using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Other
{
    public class MerchantReceivableParamNewViewModel
    {
        public int MerchantId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TransactionNo { get; set; }
    }

    public class MerchantRecivableOrder
    {
        public string OrderCode { get; set; }
        public string MonthName { get; set; }
        public double TotalAmount { get; set; }
        public string Name { get; set; }
        public int IsCashCollected { get; set; } = 0;
    }

    public class MerchantReceivableViewModel
    {
        public List<MerchantRecivableOrder> OrderList { get; set; }
        public int YearOrder { get; set; } = 0;
        public int MonthOrder { get; set; } = 0;
        public string MonthName { get; set; } = "";
        public double TotalAmount { get; set; }
    }
}
