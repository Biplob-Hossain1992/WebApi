using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class VouchersViewModel
    {
        public int VoucherId { get; set; }
        public string MerchantMobile { get; set; }
        public string VoucherCode { get; set; }
        public int ApplicableQuantity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal VoucherDiscount { get; set; }
        public int CourierUserId { get; set; }
        public bool IsActive { get; set; }
        public int DeliveryRangeId { get; set; }
        public string Message { get; set; }
        public string DeliveryRangeName { get; set; }
        public int InsertBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public string FullName { get; set; }
    }
}
