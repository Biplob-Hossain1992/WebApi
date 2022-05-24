using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Retention.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.Retention
{
    [Produces("application/json")]
    [Route("api/Retention")]
    public class RetentionController : ControllerBase
    {
        private readonly IRetentionService _retentionService;

        public RetentionController(IRetentionService retentionService)
        {
            _retentionService = retentionService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetRetentionMerchantList")]
        public async Task<IActionResult> GetRetentionMerchantList([FromBody] SearchBodyModel searchBodyModel)
        {
            var response = new ListResponseModel<CourierUsersViewModel>();
            try
            {
                var data = await _retentionService.GetRetentionMerchantList(searchBodyModel);
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
        [Route("GetRetentionMerchantListV1")]
        public async Task<IActionResult> GetRetentionMerchantListV1([FromBody] SearchBodyModel searchBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _retentionService.GetRetentionMerchantListV1(searchBodyModel);
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
        [Route("GetOrderWiseRetentionMerchantList")]
        public async Task<IActionResult> GetOrderWiseRetentionMerchantList([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<CourierUsersViewModel>();
            try
            {
                var data = await _retentionService.GetOrderWiseRetentionMerchantList(requestBodyModel);
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
        [Route("GetOrderedRetentionMerchantList")]
        public async Task<IActionResult> GetOrderedRetentionMerchantList([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _retentionService.GetOrderedRetentionMerchantList(requestBodyModel);
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
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [Route("AddVisitedMerchant")]
        public async Task<IActionResult> AddVisitedMerchant([FromBody] MerchantVisited merchantVisited)
        {
            var response = new SingleResponseModel<MerchantVisited>();

            try
            {
                var data = await _retentionService.AddVisitedMerchant(merchantVisited);
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
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [Route("AddCalledMerchant")]
        public async Task<IActionResult> AddCalledMerchant([FromBody] MerchantCalled merchantCalled)
        {
            var response = new SingleResponseModel<MerchantCalled>();

            try
            {
                var data = await _retentionService.AddCalledMerchant(merchantCalled);
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("NewRetentionMerchantFollowUpReport")]
        public async Task<IActionResult> NewRetentionMerchantFollowUpReport([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _retentionService.NewRetentionMerchantFollowUpReport(requestBodyModel);
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
        [Route("NewRetentionMerchantFollowUpReportDetails")]
        public async Task<IActionResult> NewRetentionMerchantFollowUpReportDetails([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _retentionService.NewRetentionMerchantFollowUpReportDetails(requestBodyModel);
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
        [Route("SrWiseRetentionMerchantFollowUp")]
        public async Task<IActionResult> SrWiseRetentionMerchantFollowUp([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _retentionService.SrWiseRetentionMerchantFollowUp(orderBodyModel);
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
        [Route("SrWiseRetentionMerchantFollowUpDetails")]
        public async Task<IActionResult> SrWiseRetentionMerchantFollowUpDetails([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _retentionService.SrWiseRetentionMerchantFollowUpDetails(requestBodyModel);
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
        [Route("MonthWiseUniqueOrderdMerchantList")]
        public async Task<IActionResult> MonthWiseUniqueOrderdMerchantList([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _retentionService.MonthWiseUniqueOrderdMerchantList(requestBodyModel);
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
        [Route("FullMonthUniqueOrderedMerchantList")]
        public async Task<IActionResult> FullMonthUniqueOrderedMerchantList([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _retentionService.FullMonthUniqueOrderedMerchantList(requestBodyModel);
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
        [Route("SrWiseRegularOrderedMerchantList")]
        public async Task<IActionResult> SrWiseRegularOrderedMerchantList([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _retentionService.SrWiseRegularOrderedMerchantList(requestBodyModel);
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
        [Route("GetTelesalesActiveMerchantList")]
        public async Task<IActionResult> GetTelesalesActiveMerchantList([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<CourierUsersViewModel>();

            try
            {
                var data = await _retentionService.GetTelesalesActiveMerchantList(requestBodyModel);
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
        [Route("GetSrWiseCourierUsersInfo")]
        public async Task<IActionResult> GetSrWiseCourierUsersInfo([FromBody] SearchBodyModel searchBodyModel)
        {
            var response = new SingleResponseModel<CourierUsersInfoViewModel>();

            try
            {
                var data = await _retentionService.GetSrWiseCourierUsersInfo(searchBodyModel);
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
