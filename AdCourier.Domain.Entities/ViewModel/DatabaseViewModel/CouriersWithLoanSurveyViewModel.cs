using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class CouriersWithLoanSurveyViewModel
    {
        public int CouriersWithLoanSurveyId { get; set; }
        public int LoanSurveyId { get; set; }
        public int CourierId { get; set; }
        public string CourierName { get; set; }
    }
}
