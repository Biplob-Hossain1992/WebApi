using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.Offer;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Offer")]
    //[Authorize]
    public class OfferController : ControllerBase
    {
        private readonly IGenerateLinkService _generateLinkService;
        public OfferController(IGenerateLinkService generateLinkService)
        {
            _generateLinkService = generateLinkService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddGenerateLink")]
        public async Task<IActionResult> AddGenerateLink([FromBody] GenerateLink generateLink)
        {
            var response = new SingleResponseModel<GenerateLink>();

            try
            {
                var data = await _generateLinkService.AddGenerateLink(generateLink);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetGenerateLinks")]
        public async Task<IActionResult> GetGenerateLinks()
        {
            var response = new ListResponseModel<GenerateLink>();

            try
            {
                var data = await _generateLinkService.GetGenerateLinks();
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
        [ProducesResponseType(500)]
        [Route("GetOffer/{offerId}")]
        public async Task<IActionResult> GetOffer(string offerId)
        {
            var response = new SingleResponseModel<GenerateLinkViewModel>();

            try
            {
                var data = await _generateLinkService.GetOffer(offerId);
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateOffer/{id}")]
        public async Task<IActionResult> UpdateOffer(int id, [FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _generateLinkService.UpdateOffer(id, courierOrders);
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
        [ProducesResponseType(500)]
        [Route("GetOfferByMerchant/{merchantId}")]
        public async Task<IActionResult> GetOfferByMerchant(int merchantId)
        {
            var response = new SingleResponseModel<bool>();

            try
            {
                var data = await _generateLinkService.GetOfferByMerchant(merchantId);
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
