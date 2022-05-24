using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Route("api/[controller]")]
    //[Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateRegisterTermsConditions/{id}")]
        public async Task<IActionResult> UpdateRegisterTermsConditions(int id, [FromBody] Settings settings)
        {
            var response = new SingleResponseModel<Settings>();

            try
            {
                var data = await _settingsService.UpdateRegisterTermsConditions(id, settings);
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
        [Route("UpdateSettings/{id}")]
        public async Task<IActionResult> UpdateSettings(int id, [FromBody] Settings settings)
        {
            var response = new SingleResponseModel<Settings>();

            try
            {
                var data = await _settingsService.UpdateSettings(id, settings);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpCreatedResponse();
        }

        /// <summary>
        /// UpdateVoucherTermsConditions
        /// </summary>
        /// <returns>A response with UpdateVoucherTermsConditions</returns>
        /// <response code="200">Returns the Updated Voucher Terms Conditions</response>
        /// <response code="500">If there was an internal server error</response>

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateVoucherTermsConditions/{id}")]
        public async Task<IActionResult> UpdateVoucherTermsConditions(int id, [FromBody] Settings settings)
        {
            var response = new SingleResponseModel<Settings>();

            try
            {
                var data = await _settingsService.UpdateVoucherTermsConditions(id, settings);
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
        [ProducesResponseType(500)]
        [Route("GetSettings")]
        public async Task<IActionResult> GetSettings()
        {
            var response = new SingleResponseModel<Settings>();

            try
            {
                var data = await _settingsService.GetSettings();
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
        [Route("GetOfferCharge/{merchantId}")]
        public async Task<IActionResult> GetDeliveryChargeByOffer(int merchantId)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _settingsService.GetOfferCharge(merchantId);
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
