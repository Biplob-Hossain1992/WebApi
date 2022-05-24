using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class InstantCodController : ControllerBase
    {
        private readonly IInstantCodRepository _instantCodRepository;
        private readonly IInstantCodService _instantCodService;
        public InstantCodController(IInstantCodService instantCodService, IInstantCodRepository instantCodRepository)
        {
            _instantCodService = instantCodService;
            _instantCodRepository = instantCodRepository;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddInstantCodOrder")]
        public async Task<IActionResult> AddInstantCodOrder([FromBody] List<CourierOrders> courierOrders)
        {
            var response = new ListResponseModel<CourierOrders>();

            try
            {
                var data = await _instantCodService.AddInstantCodOrder(courierOrders);
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
        [Route("UpdateInstantCodOrder")]
        public async Task<IActionResult> UpdateInstantCodOrder([FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _instantCodRepository.UpdateInstantCodOrder(courierOrders);
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
        [Route("GetInstantCodOrders")]
        public async Task<IActionResult> GetInstantCodOrders([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _instantCodRepository.GetInstantCodOrders(request);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }


            return response.ToHttpResponse();

        }
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetInstantCodCollectionDetails")]
        public async Task<IActionResult> GetInstantCodCollectionDetails([FromBody] RequestBodyModel request)
        {
            var response = new SingleResponseModel<dynamic>();
            try
            {
                var data = await _instantCodRepository.GetInstantCodCollectionDetails(request);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }
            return response.ToHttpResponse();
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetCourierLocations/{isActive}")]
        public async Task<IActionResult> GetCourierLocations(int isActive)
        {
            var response = new ListResponseModel<CourierLocation>();
            try
            {
                var data = await _instantCodRepository.GetCourierLocations(isActive);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddCourierLocation")]
        public async Task<IActionResult> AddCourierLocation([FromBody] CourierLocation courierLocation)
        {
            var response = new SingleResponseModel<CourierLocation>();

            try
            {
                var data = await _instantCodRepository.AddCourierLocation(courierLocation);
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
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("CheckInstaCod")]
        public async Task<IActionResult> CheckInstaCod([FromBody] RequestBodyModel request)
        {
            var response = new SingleResponseModel<bool>();

            try
            {
                var data = await _instantCodService.CheckInstaCod(request);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpCreatedResponse();
        }
    }
}
