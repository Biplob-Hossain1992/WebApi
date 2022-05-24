using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class DealsViewModel
    {
        public int DealId { set; get; }
        public string DealTitle { set; get; }
        public decimal CouponPrice { get; set; }
        public string FolderName { set; get; }
        public string Sizes { set; get; }
        public int DealQtn { set; get; }
        public string Colors { set; get; }
        public bool IsSoldOut { set; get; }
    }
}
