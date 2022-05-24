using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.Other;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface ILoanRepository
    {
        Task<LoanSurvey> AddLoanSurvey(LoanSurvey loanSurvey);
        Task<IEnumerable<dynamic>> GetLoanSurveyByCourierUser(int CourierUserId);
        Task<dynamic> GetCibFormCourierUserInfo(int CourierUserId);
        Task<int> UpdateLoanSurvey(int loanSurveyId, LoanSurvey loanSurvey);
        Task<int> UpdateCourierWithLoanSurvey(int loanSurveyId, List<CouriersWithLoanSurvey> couriersWithLoanSurveys);
        Task<IEnumerable<DanaToDtMatchColumnViewModel>> GetDanaColumnValue(int CourierUserId);
        Task<int> UpdateLoanApplicantsComment(int LoanSurveyId, string Comments);
        Task<CibFormData> AddCibFormData(CibFormData formData);
        Task<List<CibFormOtherLoan>> AddCibFormOtherLoan(int cibFormDataId, List<CibFormOtherLoan> otherLoans);
        Task<List<LoanApplicantFamilyMember>> AddLoanApplicantFamilyMember(int LoanSurveyId, List<LoanApplicantFamilyMember> LoanApplicantFamilyMember);
        Task<List<LoanApplicantFamilyMember>> UpdateLoanApplicantFamilyMember(int LoanSurveyId, List<LoanApplicantFamilyMember> LoanApplicantFamilyMember);
        Task<BorrowerBusinessData> AddBorrowerBusinessData(BorrowerBusinessData borrowerBusinessData);
        Task<BorrowerBusinessDataViewModel> UpdateBorrowerBusinessData(int courierUserId, BorrowerBusinessDataViewModel borrowerBusinessData);
        Task<BorrowerBusinessDataViewModel> GetBorrowerBusinessData(int courierUserId);
        Task<LoanStatusViewModel> UpdateLoanStatus(LoanStatusViewModel loanStatus, int lenderUserId);
        Task<LoanStatusViewModel> GetLoanStatus(int LoanSurveyId, int CourierUserId);
        Task<LoanDisbursement> UpdateLoanDisbursementDT(LoanDisbursement loanDisbursement);
        Task<LoanDisbursementHistoryViewModel> UpdateLoanDisbursementHistoryDT(LoanDisbursementHistoryViewModel loanDisbursementHistory, int lenderUserId);
        Task<RepaymentStatusViewModel> UpdateLoanRepaymentStatusDT(RepaymentStatusViewModel repaymentStatus, int lenderUserId);
        Task<dynamic> GetLoanApplicationInformationShakti(int courierUserId);
        Task<LoanApprovalPayment> AddLoanApprovalPayment(LoanApprovalPayment loanApprovalPayment);
        Task<dynamic> UpdateLoanApprovalPaymentDT(LoanApprovalPaymentViewModel loanApprovalPayment);
        Task<IEnumerable<LoanApprovedStatusViewModel>> GetMerchantLoanApprovedStatus(int courierUserId);
        Task<IEnumerable<LoanSurveyViewModel>> GetApprovedLoanApplicantsData();
        Task<LoanDisbursement> GetLoanDisbursementsInfo(int LoanSurveyId);
        Task<IEnumerable<LoanDisbursementHistory>> GetLoanDisbursementsHistoryInfo(string TransactionId);
        Task<dynamic> GetDanaCustomInformation(int CourierUserId);
        Task<IEnumerable<LoanDisbursementInfoViewModel>> GetMerchantLoanDisbursementInfo(string StatusCode);
    }
}
