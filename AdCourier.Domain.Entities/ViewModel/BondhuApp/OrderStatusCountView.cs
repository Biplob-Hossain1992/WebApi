using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.BondhuApp
{
    public class OrderStatusCountView
    {
        public int DeliveryManId { get; set; }
        public string DeliveryManName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusNameBng { get; set; }
        public int OrderCount { get; set; }
        public int MerchantCount { get; set; }
        public string Company { get; set; }
    }
    public class OrderStatusCountDeliveryManWise
    {
        public int DeliveryManId { get; set; }
        public string DeliveryManName { get; set; }
        public List<OrderStatusCountView> OrderCounts { get; set; }
    }
    //public class OrderStatusHistoryCount
    //{
    //    public List<OrderStatusCountDeliveryManWise> DtCounts { get; set; }
    //}

    public class CollectedNotCollectedMerchantWithCustomerInfo
    {
        public string CompanyName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int CollectAddressThanaId { get; set; }
        public int MerchantId { get; set; }
        public int Id { get; set; }
        public string CustomerName { get; set; }
    }
}
