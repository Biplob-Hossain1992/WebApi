using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.Other;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Loan")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddLoanSurvey")]
        public async Task<IActionResult> AddLoanSurvey([FromBody] LoanSurvey loanSurvey)
        {
            var response = new SingleResponseModel<LoanSurvey>();

            try
            {
                var data = await _loanService.AddLoanSurvey(loanSurvey);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetLoanSurveyByCourierUser/{CourierUserId}")]
        public async Task<IActionResult> GetLoanSurveyByCourierUser(int CourierUserId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _loanService.GetLoanSurveyByCourierUser(CourierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetCibFormCourierUserInfo/{CourierUserId}")]
        public async Task<IActionResult> GetCibFormCourierUserInfo(int CourierUserId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _loanService.GetCibFormCourierUserInfo(CourierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetDanaColumnValue/{CourierUserId}")]
        public async Task<IActionResult> GetDanaColumnValue(int CourierUserId)
        {
            var response = new ListResponseModel<DanaToDtMatchColumnViewModel>();

            try
            {
                var data = await _loanService.GetDanaColumnValue(CourierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateLoanApplicantsComment/{LoanSurveyId}")]
        public async Task<IActionResult> UpdateLoanApplicantsComment(int LoanSurveyId, [FromBody] LoanSurveyViewModel loanSurveyViewModel)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _loanService.UpdateLoanApplicantsComment(LoanSurveyId, loanSurveyViewModel.Comments);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddCibFormData")]
        public async Task<IActionResult> AddCibFormData([FromBody] CibFormData formData)
        {
            var response = new SingleResponseModel<CibFormData>();

            try
            {
                var data = await _loanService.AddCibFormData(formData);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddCibFormOtherLoan/{cibFormDataId}")]
        public async Task<IActionResult> AddCibFormOtherLoan(int cibFormDataId, [FromBody] List<CibFormOtherLoan> otherLoans)
        {
            var response = new ListResponseModel<CibFormOtherLoan>();

            try
            {
                var data = await _loanService.AddCibFormOtherLoan(cibFormDataId, otherLoans);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddLoanApplicantFamilyMember/{LoanSurveyId}")]
        public async Task<IActionResult> AddLoanApplicantFamilyMember(int LoanSurveyId, [FromBody] List<LoanApplicantFamilyMember> LoanApplicantFamilyMember)
        {
            var response = new ListResponseModel<LoanApplicantFamilyMember>();

            try
            {
                var data = await _loanService.AddLoanApplicantFamilyMember(LoanSurveyId, LoanApplicantFamilyMember);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateLoanApplicantFamilyMember/{LoanSurveyId}")]
        public async Task<IActionResult> UpdateLoanApplicantFamilyMember(int LoanSurveyId, [FromBody] List<LoanApplicantFamilyMember> LoanApplicantFamilyMember)
        {
            var response = new ListResponseModel<LoanApplicantFamilyMember>();

            try
            {
                var data = await _loanService.UpdateLoanApplicantFamilyMember(LoanSurveyId, LoanApplicantFamilyMember);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("GetBorrowerBusinessData/{courierUserId}")]
        public async Task<IActionResult> GetBorrowerBusinessData(int courierUserId)
        {
            var response = new SingleResponseModel<BorrowerBusinessDataViewModel>();

            try
            {
                var data = await _loanService.GetBorrowerBusinessData(courierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddBorrowerBusinessData/{courierUserId}")]
        public async Task<IActionResult> AddBorrowerBusinessData(int courierUserId, [FromBody] BorrowerBusinessDataViewModel borrowerBusinessData)
        {
            var response = new SingleResponseModel<BorrowerBusinessData>();
            try
            {
                var data = await _loanService.AddBorrowerBusinessData(courierUserId, borrowerBusinessData);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateBorrowerBusinessData/{courierUserId}")]
        public async Task<IActionResult> UpdateBorrowerBusinessData(int courierUserId,[FromBody] BorrowerBusinessDataViewModel borrowerBusinessData)
        {
            var response = new SingleResponseModel<BorrowerBusinessDataViewModel>();

            try
            {
                var data = await _loanService.UpdateBorrowerBusinessData(courierUserId, borrowerBusinessData);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateLoanStatus")]
        public async Task<IActionResult> UpdateLoanStatus([FromBody] LoanStatusViewModel loanStatus)
        {
            var response = new SingleResponseModel<LoanStatusViewModel>();

            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var data = await _loanService.UpdateLoanStatus(loanStatus, identity);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("GetLoanStatus/{LoanSurveyId}/{CourierUserId}")]
        public async Task<IActionResult> GetLoanStatus(int LoanSurveyId, int CourierUserId)
        {
            var response = new SingleResponseModel<LoanStatusViewModel>();

            try
            {
                var data = await _loanService.GetLoanStatus(LoanSurveyId, CourierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateLoanDisbursementDT")]
        public async Task<IActionResult> UpdateLoanDisbursementDT([FromBody] LoanDisbursementViewModel loanDisbursement)
        {
            var response = new SingleResponseModel<LoanDisbursementViewModel>();

            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var data = await _loanService.UpdateLoanDisbursementDT(loanDisbursement, identity);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateLoanDisbursementHistoryDT")]
        public async Task<IActionResult> UpdateLoanDisbursementHistoryDT([FromBody] LoanDisbursementHistoryViewModel loanDisbursementHistory)
        {
            var response = new SingleResponseModel<LoanDisbursementHistoryViewModel>();

            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var data = await _loanService.UpdateLoanDisbursementHistoryDT(loanDisbursementHistory, identity);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateLoanRepaymentStatusDT")]
        public async Task<IActionResult> UpdateLoanRepaymentStatusDT([FromBody] RepaymentStatusViewModel repaymentStatus)
        {
            var response = new SingleResponseModel<RepaymentStatusViewModel>();

            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var data = await _loanService.UpdateLoanRepaymentStatusDT(repaymentStatus, identity);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("GetLoanApplicationInformationShakti/{courierUserId}")]
        public async Task<IActionResult> GetLoanApplicationInformationShakti(int courierUserId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _loanService.GetLoanApplicationInformationShakti(courierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [Route("AddLoanApprovalPayment")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddLoanApprovalPayment([FromBody] LoanApprovalPayment loanApprovalPayment)
        {
            var response = new SingleResponseModel<LoanApprovalPayment>();

            try
            {
                var data = await _loanService.AddLoanApprovalPayment(loanApprovalPayment);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpPost]
        [Route("UpdateLoanApprovalPaymentDT")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateLoanApprovalPaymentDT([FromBody] LoanApprovalPaymentViewModel loanApprovalPayment)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _loanService.UpdateLoanApprovalPaymentDT(loanApprovalPayment);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetMerchantLoanApprovedStatus/{courierUserId}")]
        public async Task<IActionResult> GetMerchantLoanApprovedStatus(int courierUserId)
        {
            var response = new ListResponseModel<LoanApprovedStatusViewModel>();

            try
            {
                var data = await _loanService.GetMerchantLoanApprovedStatus(courierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetApprovedLoanApplicantsData")]
        public async Task<IActionResult> GetApprovedLoanApplicantsData()
        {
            var response = new ListResponseModel<LoanSurveyViewModel>();

            try
            {
                var data = await _loanService.GetApprovedLoanApplicantsData();
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetLoanDisbursementsInfo/{LoanSurveyId}")]
        public async Task<IActionResult> GetLoanDisbursementsInfo(int LoanSurveyId)
        {
            var response = new SingleResponseModel<LoanDisbursement>();

            try
            {
                var data = await _loanService.GetLoanDisbursementsInfo(LoanSurveyId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetLoanDisbursementsHistoryInfo/{TransactionId}")]
        public async Task<IActionResult> GetLoanDisbursementsHistoryInfo(string TransactionId)
        {
            var response = new ListResponseModel<LoanDisbursementHistory>();

            try
            {
                var data = await _loanService.GetLoanDisbursementsHistoryInfo(TransactionId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetDanaCustomInformation/{CourierUserId}")]
        public async Task<IActionResult> GetDanaCustomInformation(int CourierUserId)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _loanService.GetDanaCustomInformation(CourierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetMerchantLoanDisbursementInfo/{StatusCode}")]
        public async Task<IActionResult> GetMerchantLoanDisbursementInfo(string StatusCode)
        {
            var response = new ListResponseModel<LoanDisbursementInfoViewModel>();

            try
            {
                var data = await _loanService.GetMerchantLoanDisbursementInfo(StatusCode);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }
    }
}
