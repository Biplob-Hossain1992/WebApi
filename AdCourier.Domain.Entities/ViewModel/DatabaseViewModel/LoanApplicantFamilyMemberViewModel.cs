using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class LoanApplicantFamilyMemberViewModel
    {
        public int FamilyMemberId { get; set; } = 0;
        public int LoanSurveyId { get; set; } = 0;
        public int CourierUserId { get; set; } = 0;
        public string FamilyMemberName { get; set; } = "";
        public string Relation { get; set; } = "";
        public int Age { get; set; } = 0;
        public string Education { get; set; } = "";
        public bool IsMarried { get; set; } = false;
        public string Occupation { get; set; } = "";
    }
}
