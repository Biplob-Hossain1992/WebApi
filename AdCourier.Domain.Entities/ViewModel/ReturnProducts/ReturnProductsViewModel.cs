using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.ReturnProducts
{
    public class ReturnProductsViewModel
    {
        public int TotalReturnMerchantCount { get; set; }
        public int TotalReturnProductCount { get; set; }
        public IEnumerable<ReturnMerchantDetails> ReturnMerchantDetails { get; set; }
    }

    public class ReturnMerchantDetails
    {
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string MerchantName { get; set; }
        public string MerchantMobile { get; set; }
        public int ReturnCollectorId { get; set; }
        public string ReturnCollectorName { get; set; }
        public int TotalReturnProductCount { get; set; }
        public IEnumerable<CourierOrderViewModel> ReturnProductDetails { get; set; }
    }
}
