using System;
using System.Collections.Generic;
using System.Text;

namespace Crm.Domain.Entities.DapperDataModel
{
    public class OrderCrmDataModel
    {
        public int CouponId { get; set; }
        public int MerchantId { get; set; }
        public int CustomerId { get; set; }
        public int DealId { get; set; }
        public int CouponQtn { get; set; }
        public int CouponPrice { get; set; }
        public int Commission { get; set; }
        public int DeliveryCharge { get; set; }
        public DateTime PostedOn { get; set; }
        public String OrderFrom { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerAlternateMobile { get; set; }
        public string BkashMobileNumber { get; set; }
        public string PODnumber { get; set; }
        public string CustomerBillingAddress { get; set; }
        public int DeliveryDist { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public string Sizes { get; set; }
        public string OrderType { get; set; }
        public string Comments { get; set; }
        public string AppVersion { get; set; }
        //payment
        public string CardType { get; set; }
        public string PaymentType { get; set; }
        public string OnlineTransactionId { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class OrderCrmCountDataModel
    {
        public int TotalOrder { get; set; }
    }
}
