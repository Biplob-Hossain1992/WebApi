using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Other
{
    public class LoanDisbursementInfoViewModel
    {
        public int CourierUserId { get; set; }
        public string StatusCode { get; set; } = "";
        public decimal LoanApprovalAmount { get; set; } = 0;
        public decimal EmiAmount { get; set; } = 0;
        public int TenorMonth { get; set; } = 0;
        public string CompanyName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string LenderUserName { get; set; } = "";
        public decimal CodAmount { get; set; } = 0;
    }
}
