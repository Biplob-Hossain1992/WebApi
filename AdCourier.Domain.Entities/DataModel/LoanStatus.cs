using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("LoanStatus", Schema = "Loan")]
    public class LoanStatus
    {
        [Key]
        [Column("LoanStatusId")]
        public int LoanStatusId { get; set; }
        public int LoanSurveyId { get; set; } = 0;
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
        public string Comment { get; set; } = "";
        public DateTime CommentDate { get; set; } = DateTime.Now;
        public int LenderUserId { get; set; } = 0;
    }
}
