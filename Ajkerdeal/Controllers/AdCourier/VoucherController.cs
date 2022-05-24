using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
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
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("CheckVoucher")]
        public async Task<IActionResult> CheckVoucher(VouchersViewModel vouchersViewModel)
        {
            var response = new SingleResponseModel<VouchersViewModel>();
            try
            {

                var data = await _voucherService.CheckVoucher(vouchersViewModel);
                response.Model = data;
                response.Message = data.Message;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }
    }
}
