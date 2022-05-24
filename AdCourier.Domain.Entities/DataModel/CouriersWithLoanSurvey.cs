using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CouriersWithLoanSurvey", Schema = "Loan")]
    public class CouriersWithLoanSurvey
    {
        [Key]
        [Column("CouriersWithLoanSurveyId")]
        public int CouriersWithLoanSurveyId { get; set; }
        public int LoanSurveyId { get; set; }
        public int CourierId { get; set; }
        public string CourierName { get; set; }
    }
}
