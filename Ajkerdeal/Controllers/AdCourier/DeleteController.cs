using AdCourier.Domain.Entities.BodyModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Delete")]
    [Authorize]
    public class DeleteController : ControllerBase
    {
        private readonly IOrderTrackingService _orderTrackingService;

        public DeleteController(IOrderTrackingService orderTrackingService)
        {
            _orderTrackingService = orderTrackingService;
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("DeleteCollectorAssign/{id}")]
        public async Task<IActionResult> DeleteCollectorAssign(int id)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.DeleteCollectorAssign(id);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("DeletePickupLocations/{id}")]
        public async Task<IActionResult> DeletePickupLocations(int id)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.DeletePickupLocations(id);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("DeleteLocationAssign/{id}")]
        public async Task<IActionResult> DeleteLocationAssign(int id)
        {

            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.DeleteLocationAssign(id);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("DeleteUserLocationAssign/{userLocationAssignId}")]
        public async Task<IActionResult> DeleteUserLocationAssign(int userLocationAssignId)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.DeleteUserLocationAssign(userLocationAssignId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("DeleteDeliveryChargeDetails")]
        public async Task<IActionResult> DeleteDeliveryChargeDetails([FromBody] AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.DeleteDeliveryChargeDetails(assignCouirerAndServiceBodyModel);
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
