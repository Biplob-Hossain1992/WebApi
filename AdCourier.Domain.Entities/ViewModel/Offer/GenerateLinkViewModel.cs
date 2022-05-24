using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Offer
{
    public class GenerateLinkViewModel
    {
        public int GenerateLinkId { get; set; }
        public string OrderType { get; set; }
        public decimal CollectionAmount { get; set; }
        public string PaymentOption { get; set; }
        public decimal CodCharge { get; set; }
        public string CustomerMobile { get; set; }
        public int ClassifiedId { get; set; }
        public string ProductTitle { get; set; }
        public string OfferCode { get; set; }
    }
}
