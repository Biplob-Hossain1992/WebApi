using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.OrderRequest;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/QuickOrder")]
    public class QuickOrderController: ControllerBase
    {
        private readonly IQuickOrderService _quickOrderService;

        public QuickOrderController(IQuickOrderService quickOrderService)
        {
            _quickOrderService = quickOrderService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddQuickOrders")]
        public async Task<IActionResult> AddQuickOrders([FromBody] SearchBodyModel searchBodyModel)
        {
            var response = new ListResponseModel<CourierOrders>();

            try
            {
                var data = await _quickOrderService.AddQuickOrders(searchBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetGenerateQuickOrders")]
        public async Task<IActionResult> GetGenerateQuickOrders([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<CourierOrders>();

            try
            {
                var data = await _quickOrderService.GetGenerateQuickOrders(requestBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("CheckIsQuickOrder/{orderId}")]
        public async Task<IActionResult> CheckIsQuickOrder(string orderId)
        {
            var response = new SingleResponseModel<bool>();

            try
            {
                var data = await _quickOrderService.CheckIsQuickOrder(orderId);
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
        [Route("IsAcceptedQuickOrder/{orderRequestId}")]
        public async Task<IActionResult> IsAcceptedQuickOrder(int orderRequestId)
        {
            var response = new SingleResponseModel<bool>();

            try
            {
                var data = await _quickOrderService.IsAcceptedQuickOrder(orderRequestId);
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
        [Route("GetMerchantByCompanyName/{companyName}")]
        public async Task<IActionResult> GetMerchantByCompanyName(string companyName)
        {
            var response = new ListResponseModel<CourierUsers>();

            try
            {
                var data = await _quickOrderService.GetMerchantByCompanyName(companyName);
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
        [Route("LoadQuickOrder")]
        public async Task<IActionResult> LoadQuickOrder([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _quickOrderService.LoadQuickOrder(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }


            return response.ToHttpResponse();

        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateOrderInfoForApp")]
        public async Task<IActionResult> UpdateOrderInfoForApp([FromBody] CourierOrders orders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _quickOrderService.UpdateOrderInfoForApp(orders);
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
        [Route("QuickOrderProcess")]
        public async Task<IActionResult> QuickOrderProcess ([FromBody] CourierOrders request)
        {
            var response = new SingleResponseModel<CourierOrders>();
            try
            {
                var data = await _quickOrderService.QuickOrderProcess(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact to technical support";
            }

            return response.ToHttpResponse();
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetMerchantWiseRequestOrders")]
        public async Task<IActionResult> GetMerchantWiseRequestOrders([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<OrderRequestViewModel>();
            try
            {
                var data = await _quickOrderService.GetMerchantWiseRequestOrders(requestBody);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }


            return response.ToHttpResponse();

        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateMultipleTimeSlot")]
        public async Task<IActionResult> UpdateMultipleTimeSlot([FromBody] List<OrderRequest> orderRequests)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _quickOrderService.UpdateMultipleTimeSlot(orderRequests);
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
        [Route("UpdateRider")]
        public async Task<IActionResult> UpdateRider([FromBody] OrderRequest orderRequests)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _quickOrderService.UpdateRider(orderRequests);
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
        [Route("GetQuickOrderGenerateForHub")]
        public async Task<IActionResult> GetQuickOrderGenerateForHub([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<CourierOrdersViewModel>();
            try
            {
                var data = await _quickOrderService.GetQuickOrderGenerateForHub(request);
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
        [Route("DeleteOrderRequest/{orderRequestId}")]
        public async Task<IActionResult> DeleteOrderRequest(int orderRequestId)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _quickOrderService.DeleteOrderRequest(orderRequestId);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }

            return response.ToHttpResponse();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateCollectionTimeSlotIdManually/{flag}")]
        public async Task<IActionResult> UpdateCollectionTimeSlotIdManually(int flag)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _quickOrderService.UpdateCollectionTimeSlotIdManually(flag) ;
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
