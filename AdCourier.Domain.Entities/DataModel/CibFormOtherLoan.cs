using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CibFormOtherLoan", Schema = "Loan")]
    public class CibFormOtherLoan
    {
        public int Id { get; set; }
        public int CibFormDataId { get; set; }
        public int CourierUserId { get; set; }
        public string CompanyName { get; set; } = "";
        public string MainAddress { get; set; } = "";
        public string AdditionalAddress { get; set; } = "";
        public string LoanBankName { get; set; } = "";
        public string LoanBankBranch { get; set; } = "";
    }
}
