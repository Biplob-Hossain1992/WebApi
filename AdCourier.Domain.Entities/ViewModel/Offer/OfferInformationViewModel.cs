using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Offer
{
    public class OfferInformationViewModel
    {
        public string CourierOrdersId { set; get; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string MerchantMobile { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { set; get; }
        public decimal CollectionAmount { get; set; }
        public string CollectionName { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal OfferDiscount { get; set; }
        public int OfferType { get; set; }
        public string OfferCode { get; set; }
        public string CompanyName { get; set; }
    }
}
