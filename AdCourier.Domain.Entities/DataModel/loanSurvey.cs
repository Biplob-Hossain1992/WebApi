using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AdCourier.Domain.Entities.DataModel
{
    [Table("LoanSurvey", Schema = "Loan")]
    public class LoanSurvey
    {
        [Key]
        [Column("LoanSurveyId")]
        public int LoanSurveyId { get; set; }
        public int CourierUserId { get; set; }
        public string Gender { get; set; }
        public string TradeLicenseImageUrl { get; set; }
        public decimal InterestedAmount { get; set; }
        public decimal TransactionAmount { get; set; }
        public bool IsBankAccount { get; set; } 
        public bool IsLocalShop { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public decimal MonthlyTotalCodAmount { get; set; }
        public string GuarantorMobile { get; set; }
        public string GuarantorName { get; set; }
        public decimal MonthlyTotalAverageSale { get; set; }
        public string MerchantName { get; set; }
        public decimal LoanAmount { get; set; }
        public string BankName { get; set; } = "";
        public string Age { get; set; } = "";
        public string BasketValue { get; set; } = "";
        public string CardHolder { get; set; } = "";
        public string CardLimit { get; set; } = "";
        public string LoanEmi { get; set; } = "";
        public bool? HasCreditCard { get; set; } = false;
        public bool? HasTin { get; set; } = false;
        public string EduLevel { get; set; } = "";
        public string RepayType { get; set; } = "";
        public string MonthlyOrder { get; set; } = "";
        public string MonthlyExp { get; set; } = "";
        public string Recommend { get; set; } = "";
        public string RelationMarchent { get; set; } = "";
        public string ShopOwnership { get; set; } = "";
        public string TinNumber { get; set; } = "";
        public string HomeOwnership { get; set; } = "";
        public string Married { get; set; } = "";
        public string FamMem { get; set; } = "";
        public bool HasTradeLicense { get; set; }
        public string TradeLicenseNo { get; set; }
        public DateTime? TradeLicenseExpireDate { get; set; }
        public string CompanyBankAccNo { get; set; }
        public string CompanyBankAccName { get; set; }
        public decimal AnnualTotalIncome { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string NidNo { get; set; }
        public decimal OthersIncome { get; set; }
        public int ReqTenorMonth { get; set; }
        public string ResidenceLocation { get; set; }
        public bool HasPreviousLoan { get; set; }
        public string LenderType { get; set; } = "";
        public string BankStatementUrl { get; set; } = "";
        public string Comments { get; set; } = "";
        public DateTime? BusinessStartDate { get; set; }
        public string NidImageUrl { get; set; } = "";
        public string TinImageUrl { get; set; } = "";
        public string CibUploadedFormUrl { get; set; } = "";
        public string PresentAddHouseNo { get; set; } = "";
        public string PresentAddRoadNo { get; set; } = "";
        public string PresentAddRoadName { get; set; } = "";
        public string PresentAddArea { get; set; } = "";
        public string PresentAddPostOffice { get; set; } = "";
        public int PresentAddDistrictId { get; set; } = 0;
        public int PresentAddThanaId { get; set; } = 0;
        public string HouseOwner { get; set; } = "";
        public bool IsOwner { get; set; } = false;
        public int DurationOfLiving { get; set; } = 0;
        public string PermanentAddHouseNo { get; set; } = "";
        public string PermanentAddRoadNo { get; set; } = "";
        public string PermanentAddRoadName { get; set; } = "";
        public string PermanentAddArea { get; set; } = "";
        public string PermanentAddPostOffice { get; set; } = "";
        public int PermanentAddDistrictId { get; set; } = 0;
        public int PermanentAddThanaId { get; set; } = 0;
        public string FatherName { get; set; } = "";
        public string MotherName { get; set; } = "";
        public string SpouseName { get; set; } = "";
        public bool IsLoanDue { get; set; } = false;
        public string NidBackImageUrl { get; set; } = "";
        public int Experience { get; set; } = 0;
        public string Occupation { get; set; } = "";
        public string PermanentAddHouseOwnerName { get; set; } = "";
        public string CreditCardNumber { get; set; } = "";
        public string ApplicantPhotoUrl { get; set; } = "";
        public string LoanRepayType { get; set; } = "";
        public string OtherIncomeSource { get; set; } = "";
    }
}
