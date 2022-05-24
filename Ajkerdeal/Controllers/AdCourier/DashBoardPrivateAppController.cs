using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AyaatLibrary.ResponseModel;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/DashBoardPrivateApp")]
    public class DashBoardPrivateAppController : Controller
    {
        private readonly IDashBoardPrivateAppService _dashBoardPrivateAppService;

        public DashBoardPrivateAppController(IDashBoardPrivateAppService dashBoardPrivateAppService)
        {
            _dashBoardPrivateAppService = dashBoardPrivateAppService;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetThirdPartyPaymentInfo")]
        public async Task<IActionResult> GetThirdPartyPaymentInfo([FromBody] RequestBodyModel request)
        {
            var response = new SingleResponseModel<dynamic>();
            try
            {
                var data = await _dashBoardPrivateAppService.GetThirdPartyPaymentInfo(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with DeliveryTiger Tech Team";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetDeliveryTigerPaymentInfo")]
        public async Task<IActionResult> GetDeliveryTigerPaymentInfo([FromBody] RequestBodyModel request)
        {
            var response = new SingleResponseModel<dynamic>();
            try
            {
                var data = await _dashBoardPrivateAppService.GetDeliveryTigerPaymentInfo(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with Delivery Tiger Tech Team";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetStatusWiseOrderInfo")]
        public async Task<IActionResult> GetStatusWiseOrderInfo()
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _dashBoardPrivateAppService.GetStatusWiseOrderInfo();
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with DeliveryTiger Tech Team";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetDtOrders")]
        public async Task<IActionResult> GetDtOrders([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _dashBoardPrivateAppService.GetDtOrders(orderBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with DeliveryTiger Tech Team.";
            }
            return response.ToHttpResponse();
        }
    }
}
