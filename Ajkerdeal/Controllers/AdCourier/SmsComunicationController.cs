using AdCourier.Domain.Entities.BodyModel.Permission;
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
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SmsComunicationController : ControllerBase
    {
        private readonly ISmsEmailService _smsEmailService;
        public SmsComunicationController(ISmsEmailService smsEmailService)
        {
            _smsEmailService = smsEmailService;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("SendSms")]
        public async Task<IActionResult> SendSms([FromBody] List<SendMobileBodyModel> request)
        {
            var response = new SingleResponseModel<bool>();
            try
            {
                var data = await _smsEmailService.SmsSend(request);
                response.Message = "Successfully send sms";
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact with AjkerDeal Technical Support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("SendSMSInfobip")]
        public async Task<IActionResult> SendSMSInfobip([FromBody] InfobipSMSBodyModel request)
        {
            var response = new SingleResponseModel<dynamic>();
            try
            {
                var data = await _smsEmailService.SendSMSInfobip(request);

                if (data.requestError != null)
                {
                    response.DidError = true;
                    response.ErrorMessage = "Error when calling ThirdParty";
                    response.Model = data;
                }
                else
                {
                    response.Model = data;
                }
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact AjkerDeal Technical Support";
            }

            return response.ToHttpResponse();
        }

    }
}
