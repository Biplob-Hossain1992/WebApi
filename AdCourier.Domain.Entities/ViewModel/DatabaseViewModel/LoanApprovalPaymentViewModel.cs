using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    //Do Not Change this api, as it is related to third party loan api
    public class LoanApprovalPaymentViewModel
    {
        public int LoanApplicationId { get; set; } = 0;
        public decimal AcquiredAmount { get; set; } = 0;
    }
}
