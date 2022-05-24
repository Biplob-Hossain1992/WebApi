using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IJwtSecurityService _jwtSecurityService;
        public AccountController(ILoginService loginService, IJwtSecurityService jwtSecurityService)
        {
            _loginService = loginService;
            _jwtSecurityService = jwtSecurityService;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("AdminUserLogin")]
        public async Task<IActionResult> AdminUserLogin([FromBody] Users user)
        {
            var response = new SingleResponseModel<AdminUsersViewModel>();

            try
            {
                var data = await _loginService.AdminUserLogin(user);
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
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("LenderUserLogin")]
        public async Task<IActionResult> LenderUserLogin([FromBody] LenderUser lenderUser)
        {
            var response = new SingleResponseModel<LenderUserViewModel>();

            try
            {
                var data = await _loginService.LenderUserLogin(lenderUser);
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
        [Route("AddAdminUsers")]
        public async Task<IActionResult> AddAdminUsers([FromBody] Users users)
        {
            var response = new SingleResponseModel<Users>();

            try
            {
                var data = await _loginService.AddAdminUsers(users);
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
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] CourierUsers courierUsers)
        {
            var response = new SingleResponseModel<CourierUsersViewModel>();

            try
            {
                var data = await _loginService.UserLogin(courierUsers);
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
        [Route("UserRegister")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UserRegister([FromBody] CourierUsers courierUsers)
        {
            var response = new SingleResponseModel<CourierUsers>();

            try
            {

                var data = await _loginService.UserRegister(courierUsers);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("CheckReferrerMobile/{referrerMobile}")]
        public async Task<IActionResult> CheckReferrerMobile(string referrerMobile)
        {
            var response = new SingleResponseModel<CourierUsers>();
            try
            {

                var data = await _loginService.CheckReferrerMobile(referrerMobile);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetCourierUsers")]
        public async Task<IActionResult> GetCourierUsers([FromBody] CourierUsers courierUsers)
        {
            var response = new SingleResponseModel<CourierUsers>();
            try
            {

                var data = await _loginService.GetCourierUsers(courierUsers);
                response.Model = data;
            }
            catch (Exception exp)
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
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] CourierUsers courierUsers)
        {
            var response = new SingleResponseModel<CourierUsers>();

            try
            {
                var data = await _loginService.ResetPassword(courierUsers);
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
        [Route("{refreshToken}/RefreshToken")]
        public IActionResult GetRefreshToken([FromRoute] string refreshToken)
        {
            var response = new SingleResponseModel<CourierUsersViewModel>();

            try
            {
                var data = _jwtSecurityService.GetRefreshToken(refreshToken);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpResponse();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateBlockUser/{id}")]
        public async Task<IActionResult> UpdateBlockUser(int id, [FromBody] CourierUsers courierUsers)
        {
            var response = new SingleResponseModel<CourierUsers>();

            try
            {
                var data = await _loginService.UpdateBlockUser(id, courierUsers);
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