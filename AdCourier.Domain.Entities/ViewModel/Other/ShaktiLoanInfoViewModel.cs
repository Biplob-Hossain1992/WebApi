using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Other
{
    public class ShaktiLoanInfoViewModel
    {
        public string Partner_Application_Id { get; set; } = "";
        public string Nid_No { get; set; } = "";
        public string Applicant_Name { get; set; } = "";
        public string Org_Name { get; set; } = "";
        public string Mobile_NO { get; set; } = "";
        public DateTime? Date_of_Birth { get; set; }
        public string Gender { get; set; } = "";
        public decimal Loan_Amount { get; set; } = 0;
        public string Present_Address { get; set; } = "";
        public string Business_Address { get; set; } = "";
        public int Experience { get; set; } = 0;
        public string Education { get; set; } = "";
        public string Spouse_Name { get; set; } = "";
        public string Fathers_Name { get; set; } = "";
        public string Mothers_Name { get; set; } = "";
        public string Occupation { get; set; } = "";
        public string Present_House_No { get; set; } = "";
        public string Present_Road_No { get; set; } = "";
        public string Present_Road_Name { get; set; } = "";
        public string House_Owner { get; set; } = "";
        public string Present_Moholla { get; set; } = "";
        public string Present_Post_Office { get; set; } = "";
        public string Present_Thana { get; set; } = "";
        public string Present_District { get; set; } = "";
        public int Is_House_Owner { get; set; } = 0;
        public int Present_Duration_LivesHere { get; set; } = 0;
        public string Permanent_House_No { get; set; } = "";
        public string Permanent_Road_No { get; set; } = "";
        public string Permanent_Road_Name { get; set; } = "";
        public string Permanent_House_Owner { get; set; } = "";
        public string Permanent_Moholla { get; set; } = "";
        public string Permanent_Post_Office { get; set; } = "";
        public string Permanent_Thana { get; set; } = "";
        public string Permanent_District { get; set; } = "";
        public decimal Request_Amount { get; set; } = 0;
        public int Loan_Duration { get; set; } = 0;
        public string Repayment_Method { get; set; } = "";
        public decimal Avg_Good_Sales_Per_Month { get; set; } = 0;
        public int No_Of_Good_Months { get; set; } = 0;
        public decimal Avg_Bad_Sales_Per_Month { get; set; } = 0;
        public int No_Of_Bad_Months { get; set; } = 0;
        public decimal Avg_Regular_Sales_Per_Month { get; set; } = 0;
        public int No_Of_Regular_Months { get; set; } = 0;
        public decimal Avg_Good_Cost_Per_Month { get; set; } = 0;
        public int No_Of_Good_Months_Cost { get; set; } = 0;
        public decimal Avg_Bad_Cost_Per_Month { get; set; } = 0;
        public int No_Of_Bad_Months_Cost { get; set; } = 0;
        public decimal Avg_Regular_Cost_Per_Month { get; set; } = 0;
        public int No_Of_Regular_Months_Cost { get; set; } = 0;
        public decimal Avg_Transaction_Amt_Per_Month { get; set; } = 0;
        public decimal Rent_Of_Business { get; set; } = 0;
        public decimal Salary_Of_Employees { get; set; } = 0;
        public decimal Transportation_Cost { get; set; } = 0;
        public decimal Utility_Cost { get; set; } = 0;
        public decimal Repair_Cost { get; set; } = 0;
        public decimal Packaging_Cost { get; set; } = 0;
        public decimal Tax_Charges { get; set; } = 0;
        public decimal Other_Expense { get; set; } = 0;
        public decimal Owed_By_Debtors { get; set; } = 0;
        public decimal Owed_By_Creditors { get; set; } = 0;
        public int Other_Loan_Monthly_Installment { get; set; } = 0;
        public decimal Other_Loan_Amount { get; set; } = 0;
        public string Other_Loan_Name { get; set; } = "";
        public string Account_No { get; set; } = "";
        public string Account_Name { get; set; } = "";
        public string Bank_Code { get; set; } = "";
        public string Routing_Number { get; set; } = "";
        public string Mobile_Account_No { get; set; } = "";
        public decimal Cash_In_Hand { get; set; } = 0;
        public decimal Cash_At_Bank { get; set; } = 0;
        public string Credit_Card_No { get; set; } = "";
        public int Credit_Card_Limit { get; set; } = 0;
        public string Nid_Front_Doc { get; set; } = "";
        public string Nid_Back_Doc { get; set; } = "";
        public string Applicant_Photo_Doc { get; set; } = "";
        public string Check_Book_Leaf_Doc { get; set; } = "";
        public string Source { get; set; } = "";
        public decimal MonthlyIncome { get; set; } = 0;
        public List<PartnerFamilyMember> PartnerFamilyMemberList { get; set; }
        public List<PartnerOtherIncome> Partner_Other_Income_Model { get; set; }
        public List<MonthlyOrderTransaction> Merchant_Monthly_Transaction { get; set; }
    }

    public class PartnerFamilyMember
    {
        public string FMNAME { get; set; } = "";
        public string RELATION { get; set; } = "";
        public int Age { get; set; } = 0;
        public string EDUCATION { get; set; } = "";
        public int ISMARRIED { get; set; } = 0;
        public string OCCUPATION { get; set; } = "";
        public string Partner_Application_id { get; set; } = "";
    }

    public class PartnerOtherIncome
    {
        public string Source { get; set; } = "";
        public decimal Monthly_Income { get; set; } = 0;
        public string Partner_Application_Id { get; set; } = "";
    }

    public class MonthlyOrderTransaction
    {
        public string YYYYMM { get; set; } = "";
        public decimal TRANSACTIONAMOUNT { get; set; } = 0;
    }
}
