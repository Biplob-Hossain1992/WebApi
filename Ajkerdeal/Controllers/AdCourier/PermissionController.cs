using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.BodyModel.Permission;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    //[Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] MailSmsChargeBodyModel permission)
        {
            var response = new SingleResponseModel<bool>();

            try
            {
                var data = await _permissionService.AddPermission(permission);
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
        [Route("GetAllCourierUsers")]
        public async Task<IActionResult> GetAllCourierUsers()
        {
            var response = new ListResponseModel<CourierUsersViewModel>();

            try
            {
                var data = await _permissionService.GetAllCourierUsers();
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
        [Route("GetAllCourierUsersList")]
        public async Task<IActionResult> GetAllCourierUsersList()
        {
            var response = new ListResponseModel<CourierUsers>();

            try
            {
                var data = await _permissionService.GetAllCourierUsersList();
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
        [Route("UpdateSmsEmail/{id}")]
        public async Task<IActionResult> UpdateSmsEmail(int id, [FromBody] SmsMailBodyModel smsMailBodyModel)
        {
            var response = new SingleResponseModel<SmsMailBodyModel>();

            try
            {
                var data = await _permissionService.UpdateSmsEmail(id, smsMailBodyModel);
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
        [Route("UpdateDeliveryBonduNotification/{id}")]
        public async Task<IActionResult> UpdateDeliveryBonduNotification(int id, [FromBody] NotificationBodyModel notificationBodyModel)
        {
            var response = new SingleResponseModel<NotificationBodyModel>();

            try
            {
                var data = await _permissionService.UpdateDeliveryBonduNotification(id, notificationBodyModel);
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
