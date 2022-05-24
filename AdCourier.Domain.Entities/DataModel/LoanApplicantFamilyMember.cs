using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("LoanApplicantFamilyMember", Schema = "Loan")]
    public class LoanApplicantFamilyMember
    {
        [Key]
        [Column("FamilyMemberId")]
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
