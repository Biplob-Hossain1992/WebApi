using System;
using System.Collections.Generic;
using System.Text;

namespace Retention.Domain.Entities.ViewModel
{
    public class RetentionMerchantOrderViewModel
    {
        public DateTime LastOrderDate { get; set; }
        public int TotalOrder { get; set; }
    }
}
