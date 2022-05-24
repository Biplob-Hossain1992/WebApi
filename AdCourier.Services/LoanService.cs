using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.Other;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly SqlServerContext _sqlServerContext;
        private readonly IOrderRepository _orderRepository;

        public LoanService(ILoanRepository loanRepository, SqlServerContext sqlServerContext,
            IOrderRepository orderRepository)
        {
            _loanRepository = loanRepository;
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _orderRepository = orderRepository;
        }
        public async Task<LoanSurvey> AddLoanSurvey(LoanSurvey loanSurvey)
        {
            return await _loanRepository.AddLoanSurvey(loanSurvey);
        }

        public async Task<IEnumerable<dynamic>> GetLoanSurveyByCourierUser(int CourierUserId)
        {
            return await _loanRepository.GetLoanSurveyByCourierUser(CourierUserId);
        }

        public async Task<dynamic> GetCibFormCourierUserInfo(int CourierUserId)
        {
            return await _loanRepository.GetCibFormCourierUserInfo(CourierUserId);
        }

        public async Task<int> UpdateLoanSurvey(int loanSurveyId, LoanSurvey loanSurvey)
        {
            return await _loanRepository.UpdateLoanSurvey(loanSurveyId, loanSurvey);
        }

        public async Task<int> UpdateCourierWithLoanSurvey(int loanSurveyId, List<CouriersWithLoanSurvey> couriersWithLoanSurveys)
        {
            return await _loanRepository.UpdateCourierWithLoanSurvey(loanSurveyId, couriersWithLoanSurveys);
        }

        public async Task<IEnumerable<DanaToDtMatchColumnViewModel>> GetDanaColumnValue(int CourierUserId)
        {
            return await _loanRepository.GetDanaColumnValue(CourierUserId);
        }

        public async Task<int> UpdateLoanApplicantsComment(int LoanSurveyId, string Comments)
        {
            return await _loanRepository.UpdateLoanApplicantsComment(LoanSurveyId, Comments);
        }

        public async Task<CibFormData> AddCibFormData(CibFormData formData)
        {
            return await _loanRepository.AddCibFormData(formData);
        }

        public async Task<List<CibFormOtherLoan>> AddCibFormOtherLoan(int cibFormDataId, List<CibFormOtherLoan> otherLoans)
        {
            return await _loanRepository.AddCibFormOtherLoan(cibFormDataId, otherLoans);
        }

        public async Task<List<LoanApplicantFamilyMember>> AddLoanApplicantFamilyMember(int LoanSurveyId, List<LoanApplicantFamilyMember> LoanApplicantFamilyMember)
        {
            return await _loanRepository.AddLoanApplicantFamilyMember(LoanSurveyId, LoanApplicantFamilyMember);
        }

        public async Task<List<LoanApplicantFamilyMember>> UpdateLoanApplicantFamilyMember(int LoanSurveyId, List<LoanApplicantFamilyMember> LoanApplicantFamilyMember)
        {
            return await _loanRepository.UpdateLoanApplicantFamilyMember(LoanSurveyId, LoanApplicantFamilyMember);
        }

        public async Task<BorrowerBusinessData> AddBorrowerBusinessData(int courierUserId, BorrowerBusinessDataViewModel borrowerBusinessData)
        {
            var courierUser = new CourierUsers
            {
                AccountName = borrowerBusinessData.AccountName,
                AccountNumber = borrowerBusinessData.AccountNumber,
                RoutingNumber = borrowerBusinessData.RoutingNumber,
                BkashNumber = borrowerBusinessData.BkashNumber
            };

            await _orderRepository.UpdateCourierUserBankInformation(courierUserId, courierUser);

            var businessData = new BorrowerBusinessData
            {
                CourierUserId = borrowerBusinessData.CourierUserId,
                NoOfMaxProductSellingMonth = borrowerBusinessData.NoOfMaxProductSellingMonth,
                AvgAmountOfMaxSellingMonth = borrowerBusinessData.AvgAmountOfMaxSellingMonth,
                NoOfRegularProductSellingMonth = borrowerBusinessData.NoOfRegularProductSellingMonth,
                AvgAmountOfRegularSellingMonth = borrowerBusinessData.AvgAmountOfRegularSellingMonth,
                NoOfLowestProductSellingMonth = borrowerBusinessData.NoOfLowestProductSellingMonth,
                AvgAmountOfLowestSellingMonth = borrowerBusinessData.AvgAmountOfLowestSellingMonth,
                NoOfMaxProductPurchasedMonth = borrowerBusinessData.NoOfMaxProductPurchasedMonth,
                AvgAmountOfMaxPurchasedMonth = borrowerBusinessData.AvgAmountOfMaxPurchasedMonth,
                NoOfRegularProductPurchasedMonth = borrowerBusinessData.NoOfRegularProductPurchasedMonth,
                AvgAmountOfRegularPurchasedMonth = borrowerBusinessData.AvgAmountOfRegularPurchasedMonth,
                NoOfLowestProductPurchasedMonth = borrowerBusinessData.NoOfLowestProductPurchasedMonth,
                AvgAmountOfLowestPurchasedMonth = borrowerBusinessData.AvgAmountOfLowestPurchasedMonth,
                BusinessRent = borrowerBusinessData.BusinessRent,
                SalaryOfEmployee = borrowerBusinessData.SalaryOfEmployee,
                TransportationCost = borrowerBusinessData.TransportationCost,
                UtilityCost = borrowerBusinessData.UtilityCost,
                RepairCost = borrowerBusinessData.RepairCost,
                PackagingCost = borrowerBusinessData.PackagingCost,
                BusinessTax = borrowerBusinessData.BusinessTax,
                OthersExpense = borrowerBusinessData.OthersExpense,
                DebotrsOwingAmount = borrowerBusinessData.DebotrsOwingAmount,
                DebotrsOwingAmountPercentageOnSale = borrowerBusinessData.DebotrsOwingAmountPercentageOnSale,
                CreditorsOwingAmount = borrowerBusinessData.CreditorsOwingAmount,
                CreditorsOwingAmountPercentageOnPurchase = borrowerBusinessData.CreditorsOwingAmountPercentageOnPurchase,
                BankShortCode = borrowerBusinessData.BankShortCode,
                AccountType = borrowerBusinessData.AccountType,
                CashInHand = borrowerBusinessData.CashInHand,
                CashAtBank = borrowerBusinessData.CashAtBank,
                ChecqueBookPhotoUrl = borrowerBusinessData.ChecqueBookPhotoUrl
            };

            return await _loanRepository.AddBorrowerBusinessData(businessData);
        }

        public async Task<BorrowerBusinessDataViewModel> UpdateBorrowerBusinessData(int courierUserId, BorrowerBusinessDataViewModel borrowerBusinessData)
        {
            var courierUser = new CourierUsers
            {
                AccountName = borrowerBusinessData.AccountName,
                AccountNumber = borrowerBusinessData.AccountNumber,
                RoutingNumber = borrowerBusinessData.RoutingNumber,
                BkashNumber = borrowerBusinessData.BkashNumber
            };

            await _orderRepository.UpdateCourierUserBankInformation(courierUserId, courierUser);

            return await _loanRepository.UpdateBorrowerBusinessData(courierUserId, borrowerBusinessData);
        }

        public async Task<BorrowerBusinessDataViewModel> GetBorrowerBusinessData(int courierUserId)
        {
            return await _loanRepository.GetBorrowerBusinessData(courierUserId);
        }

        public async Task<LoanStatusViewModel> UpdateLoanStatus(LoanStatusViewModel loanStatus, ClaimsIdentity identity)
        {
            int lenderUserId = 0;
            if (identity != null)
            {
                IEnumerable<Claim> claim = identity.Claims;
                lenderUserId = Convert.ToInt32(claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value);
                //lenderUserId = Convert.ToInt32(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }

            //if(loanStatus.StatusCode == "00005")
            //{
            //    var LoanApprovalPaymentViewModel = new LoanApprovalPaymentViewModel
            //    {
            //        AcquiredAmount = 25,
            //        LoanApplicationId = loanStatus.LoanApplicationId
            //    };

            //    await _loanRepository.UpdateLoanApprovalPaymentDT(LoanApprovalPaymentViewModel);
            //}

            return await _loanRepository.UpdateLoanStatus(loanStatus, lenderUserId);
        }

        public async Task<LoanStatusViewModel> GetLoanStatus(int LoanSurveyId, int CourierUserId)
        {
            return await _loanRepository.GetLoanStatus(LoanSurveyId, CourierUserId);
        }

        public async Task<LoanDisbursementViewModel> UpdateLoanDisbursementDT(LoanDisbursementViewModel loanDisbursement, ClaimsIdentity identity)
        {
            //get the logged in lender user id from api auth token
            int lenderUserId = 0;
            if (identity != null)
            {
                IEnumerable<Claim> claim = identity.Claims;
                lenderUserId = Convert.ToInt32(claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value);
            }

            var entity = await _sqlServerContext.LoanDisbursement.FirstOrDefaultAsync(disburse => disburse.TransactionID.Equals(loanDisbursement.TransactionID));
            LoanDisbursement disbursement = new LoanDisbursement();

            if (entity == null)
            {
                DateTime dateTime = DateTime.Now;
                int month = dateTime.Month;
                int year = dateTime.Year;
                int emiday = loanDisbursement.EmiDay != "" ? Convert.ToInt32(loanDisbursement.EmiDay) : 5;
                int monthlyMaxEmiDay = loanDisbursement.MonthlyMaxEmiDate != "" ? Convert.ToInt32(loanDisbursement.MonthlyMaxEmiDate) : 15;

                DateTime emiDate = new DateTime(year, month, emiday);
                DateTime monthlyMaxEmiDate = new DateTime(year, month, monthlyMaxEmiDay);

                disbursement.DisbursementId = 0;
                disbursement.TransactionID = loanDisbursement.TransactionID;
                disbursement.DisbursementAmount = loanDisbursement.DisbursementAmount;
                disbursement.DisbursementDate = loanDisbursement.DisbursementDate;
                disbursement.EmiAmount = loanDisbursement.EmiAmount;
                disbursement.EmiDate = emiDate;
                disbursement.CumulativeRecovery = loanDisbursement.CumulativeRecovery;
                disbursement.LastMonthRecoveryAmount = loanDisbursement.LastMonthRecoveryAmount;
                disbursement.MonthlyMaxEmiDate = monthlyMaxEmiDate;
                disbursement.LoanSurveyId = loanDisbursement.LoanApplicationId;
                disbursement.StatusCode = loanDisbursement.StatusCode;
                disbursement.Status = loanDisbursement.Status;
                disbursement.LenderUserId = lenderUserId;
                disbursement.RequiredTenorMonth = loanDisbursement.LoanDuration;

                await _loanRepository.UpdateLoanDisbursementDT(disbursement);

                return loanDisbursement;
            }
            else
            {
                DateTime dateTime = DateTime.Now;
                int month = dateTime.Month;
                int year = dateTime.Year;
                int emiday = loanDisbursement.EmiDay != "" ? Convert.ToInt32(loanDisbursement.EmiDay) : 5;

                DateTime emiDate = new DateTime(year, month, emiday);

                entity.CumulativeRecovery = loanDisbursement.CumulativeRecovery;
                entity.EmiDate = emiDate;

                await _loanRepository.UpdateLoanDisbursementDT(entity);

                return loanDisbursement;
            }
        }

        public async Task<LoanDisbursementHistoryViewModel> UpdateLoanDisbursementHistoryDT(LoanDisbursementHistoryViewModel loanDisbursementHistory, ClaimsIdentity identity)
        {
            //get the logged in lender user id from api auth token
            int lenderUserId = 0;
            if (identity != null)
            {
                IEnumerable<Claim> claim = identity.Claims;
                lenderUserId = Convert.ToInt32(claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value);
            }

            return await _loanRepository.UpdateLoanDisbursementHistoryDT(loanDisbursementHistory, lenderUserId);
        }

        public async Task<RepaymentStatusViewModel> UpdateLoanRepaymentStatusDT(RepaymentStatusViewModel repaymentStatus, ClaimsIdentity identity)
        {
            //get the logged in lender user id from api auth token
            int lenderUserId = 0;
            if (identity != null)
            {
                IEnumerable<Claim> claim = identity.Claims;
                lenderUserId = Convert.ToInt32(claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value);
            }

            return await _loanRepository.UpdateLoanRepaymentStatusDT(repaymentStatus, lenderUserId);
        }

        public async Task<dynamic> GetLoanApplicationInformationShakti(int courierUserId)
        {
            return await _loanRepository.GetLoanApplicationInformationShakti(courierUserId);
        }

        public async Task<LoanApprovalPayment> AddLoanApprovalPayment(LoanApprovalPayment loanApprovalPayment)
        {
            return await _loanRepository.AddLoanApprovalPayment(loanApprovalPayment);
        }

        public async Task<dynamic> UpdateLoanApprovalPaymentDT(LoanApprovalPaymentViewModel loanApprovalPayment)
        {
            return await _loanRepository.UpdateLoanApprovalPaymentDT(loanApprovalPayment);
        }

        public async Task<IEnumerable<LoanApprovedStatusViewModel>> GetMerchantLoanApprovedStatus(int courierUserId)
        {
            return await _loanRepository.GetMerchantLoanApprovedStatus(courierUserId);
        }

        public async Task<IEnumerable<LoanSurveyViewModel>> GetApprovedLoanApplicantsData()
        {
            return await _loanRepository.GetApprovedLoanApplicantsData();
        }

        public async Task<LoanDisbursement> GetLoanDisbursementsInfo(int LoanSurveyId)
        {
            return await _loanRepository.GetLoanDisbursementsInfo(LoanSurveyId);
        }

        public async Task<IEnumerable<LoanDisbursementHistory>> GetLoanDisbursementsHistoryInfo(string TransactionId)
        {
            return await _loanRepository.GetLoanDisbursementsHistoryInfo(TransactionId);
        }

        public async Task<dynamic> GetDanaCustomInformation(int CourierUserId)
        {
            return await _loanRepository.GetDanaCustomInformation(CourierUserId);
        }

        public async Task<IEnumerable<LoanDisbursementInfoViewModel>> GetMerchantLoanDisbursementInfo(string StatusCode)
        {
            return await _loanRepository.GetMerchantLoanDisbursementInfo(StatusCode);
        }
    }
}
