using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
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
    public class DanaController : ControllerBase
    {
        private readonly IDanaService _danaService;

        public DanaController(IDanaService danaService)
        {
            _danaService = danaService;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddDanaJsonData")]
        public async Task<IActionResult> AddDanaJsonData([FromBody] JsonBodyModel jsonBodyModel)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _danaService.AddDanaJsonData(jsonBodyModel);
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
        [Route("AddPohScore")]
        public async Task<IActionResult> AddPohScore([FromBody] PohScore pohScoreModel)
        {
            var response = new SingleResponseModel<PohScore>();

            try
            {
                var data = await _danaService.AddPohScore(pohScoreModel);
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
