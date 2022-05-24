using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.CourierOrder
{
    public class ServiceAmountDetails
    {
        public string CustomerName { set; get; }
        public string CourierOrdersId { set; get; }
        public decimal CollectionAmount { set; get; }
        public decimal DeliveryCharge { set; get; }
        public decimal BreakableCharge { set; get; }
        public decimal CodCharge { set; get; }
        public decimal CollectionCharge { set; get; }
        public decimal ReturnCharge { set; get; }
        public string StatusGroup { get; set; }
        public string StatusType { get; set; }

    }

    public class ServiceAmount
    {
        public IEnumerable<ServiceAmountDetails> ServiceAmountDetails { get; set; }
        public ServiceAmountTotal ServiceAmountTotal { get; set; }

    }

    public class ServiceAmountTotal
    {
        public int TotalCount { get; set; }
    }

    public class MerchantAdvanceBalanceInfo
    {
        public int ServiceCharge { get; set; } = 0;
        public int AdjustBalance { get; set; } = 0;
        public int Credit { get; set; } = 0;
        public int StaticVal { get; set; } = 0;
        public int CalculatedCollectionAmount { get; set; } = 0;
    }
}
