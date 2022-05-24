using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.IntegrationBody;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Authorize]
    [Route("api/getway")]
    [ApiController]
    public class IntegrationController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;
        private readonly IIntegrationRepository _integrationRepository;
        public IntegrationController(IIntegrationService integrationService, IIntegrationRepository integrationRepository)
        {
            _integrationService = integrationService;
            _integrationRepository = integrationRepository;
        }


        [HttpPost]
        [Route("Order")]
        public async Task<IActionResult> Order([FromBody] IntegrationOrderBodyModel request)
        {
            var response = new SingleResponseModel<string>();

            try
            {
                var data = await _integrationService.order(request);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }
            return response.ToHttpCreatedResponse();
        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("Order/{OrderId}")]
        public async Task<IActionResult> UpdateOrderHistory(string OrderId, [FromBody] UpdateStatusBodyModel request)
        {

            var response = new SingleResponseModel<bool>();

            try
            {
                var data = await _integrationRepository.UpdateOrderHistory(OrderId, request);
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
