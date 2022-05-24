using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AyaatLibrary.ResponseModel;
using Crm.Domain.Entities.DapperBodyModel;
using Crm.Domain.Entities.DapperViewModel;
using Crm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using OrderStatusHistoryViewModel = Crm.Domain.Entities.DapperViewModel.DatabaseViewModel.OrderStatusHistoryViewModel;

namespace Ajkerdeal.Controllers.Crm
{
    [Produces("application/json")]
    [Route("api/CrmFetch")]
    public class CrmFetchController : ControllerBase
    {
        
        private readonly ICrmOrderService _crmOrderService;
        private readonly IMerchantService _merchantService;
        private readonly ICustomerService _customerService;

        public CrmFetchController(ICrmOrderService crmOrderService, 
            IMerchantService merchantService, ICustomerService customerService)
        {
            _crmOrderService = crmOrderService;
            _merchantService = merchantService;
            _customerService = customerService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [Route("GetMerchantInformation/{merchantId}")]
        public async Task<IActionResult> GetMerchantInformation(int merchantId)
        {
            var response = new SingleResponseModel<UserProfile>();

            try
            {
                var data = await _merchantService.GetMerchantInformation(merchantId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [Route("GetCustomerInformation/{customerId}")]
        public async Task<IActionResult> GetCustomerInformation(int customerId)
        {
            var response = new SingleResponseModel<Customers>();

            try
            {
                var data = await _customerService.GetCustomerInformation(customerId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [Route("GetProductInformation/{dealId}")]
        public async Task<IActionResult> GetProductInformation(int dealId)
        {
            var response = new SingleResponseModel<Deals>();

            try
            {
                var data = await _crmOrderService.GetProductInformation(dealId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetOrders")]
        public async Task<IActionResult> GetOrders([FromBody] SearchOrderBodyModel searchOrderBodyModel)
        {
            var response = new SingleResponseModel<CombineCrmOrderViewModel>();

            try
            {
                var data = await _crmOrderService.GetOrders(searchOrderBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetOrderHistoryInformation/{orderId}")]
        public async Task<IActionResult> GetOrderHistoryInformation(string orderId)
        {
            var response = new ListResponseModel<OrderStatusHistoryViewModel>();

            try
            {
                var data = await _crmOrderService.GetOrderHistoryInformation(orderId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpResponse();
        }
    }
}
