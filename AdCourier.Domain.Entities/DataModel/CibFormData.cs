using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CibFormData", Schema = "Loan")]
    public class CibFormData
    {
        public int Id { get; set; }
        public int CourierUserId { get; set; }
        public int TypeOfFinancing { get; set; } = 0;
        public int NumberOfInstallments { get; set; } = 0;
        public string PeriodicityOfPayment { get; set; }
        public int AmountOfLoan { get; set; } = 0;
        public string SectorType { get; set; } = "";
        public string SectorCode { get; set; } = "";
        public string BranchName { get; set; } = "";
        public string CibSubjctCode { get; set; } = "";
        public string FiSubjectCode { get; set; } = "";
        public DateTime? Date { get; set; }
        public string RefNo { get; set; } = "";
        public string ManagerName { get; set; } = "";
        public string ApplicantName { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string FatherName { get; set; } = "";
        public string MotherName { get; set; } = "";
        public string SpouseName { get; set; } = "";
        public string PermanentAddress { get; set; } = "";
        public string PresentAddress { get; set; } = "";
        public string NidNo { get; set; } = "";
        public string OtherIdNumber { get; set; } = "";
        public DateTime? OtherIdIssueDate { get; set; }
        public string OtherIdIssueCountry { get; set; } = "";
        public DateTime? DateOfBirth { get; set; }
        public string DistrictOfBirth { get; set; } = "";
        public string CountryOfBirth { get; set; } = "";
        public string TinNumber { get; set; } = "";
        public string Gender { get; set; } = "";
        public string TelephoneNumber { get; set; } = "";
        public string CibFormLink { get; set; } = "";
    }
}
