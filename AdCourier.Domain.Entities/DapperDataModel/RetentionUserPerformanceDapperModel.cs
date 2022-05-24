using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.DapperDataModel
{
    public class RetentionUserPerformanceDapperModel
    {
        public int CourierUserId { get; set; }
        public string CompanyName { get; set; }
        public int TotalOrder { get; set; }
        public int TotalMerchant { get; set; }
        public DateTime LastOrderDate { get; set; }
        public int TotalUnsolvedComplain { get; set; }
        public string Mobile { get; set; } = "";
        public string BkashNumber { get; set; } = "";
        public string AlterMobile { get; set; } = "";
        public int TeleSales { get; set; }
    }
}
