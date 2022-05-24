using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class LoanStatusViewModel
    {
        public int LoanApplicationId { get; set; } = 0;
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
        public string Comment { get; set; } = "";
    }

    public class LoanApprovedStatusViewModel
    {
        public int LoanApplicationId { get; set; } = 0;
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
        public string Comment { get; set; } = "";
        public DateTime CommentDate { get; set; }
        public string LenderUserName { get; set; } = "";
        public DateTime? DisbursementDate { get; set; }
        public decimal EmiAmount { get; set; } = 0;
    }
}
