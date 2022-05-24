using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Order")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateBulkOrders")]
        public async Task<IActionResult> UpdateBulkOrders([FromBody] List<CourierOrderStatusHistoryViewModel> courierOrderStatusHistory)
        {
            var response = new ListResponseModel<CourierOrderStatusHistory>();

            try
            {
                var data = await _orderService.UpdateBulkOrders(courierOrderStatusHistory);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddOrdersBulk")]
        public async Task<IActionResult> AddOrdersBulk([FromBody] List<CourierOrders> courierOrders)
        {
            var response = new ListResponseModel<CourierOrders>();

            try
            {
                var data = await _orderService.AddOrdersBulk(courierOrders);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }
            return response.ToHttpCreatedResponse();
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddOrder")]
        public async Task<IActionResult> AddOrder([FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                Log.Information("Starting Order {@courierOrders} on {Created}", courierOrders, DateTime.Now);
                var data = await _orderService.AddOrder(courierOrders);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
                Log.Error(ex,"Error Order {@courierOrders} on {Created}", courierOrders, DateTime.Now);
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("GetDeliveryChargeDetailsPrice")]
        public async Task<IActionResult> GetDeliveryChargeDetailsPrice([FromBody] DeliveryChargeDetails deliveryChargeDetails)
        {
            var response = new SingleResponseModel<DeliveryChargeDetails>();

            try
            {
                var data = await _orderService.GetDeliveryChargeDetailsPrice(deliveryChargeDetails);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }
            return response.ToHttpResponse();
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetDeliveryChargeMerchantDetailsCourier")]
        public async Task<IActionResult> GetDeliveryChargeMerchantDetailsCourier([FromBody] DeliveryChargeMerchantDetails request)
        {
            var response = new SingleResponseModel<DeliveryChargeMerchantDetails>();
            try
            {
                var data = await _orderService.GetDeliveryChargeMerchantDetailsCourier(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }

            return response.ToHttpResponse();
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("GetChangeDeliveryChargeDetailsLog")]
        public async Task<IActionResult> GetChangeDeliveryChargeDetailsLog([FromBody]ChangeDeliveryChargeDetailsLog changeDeliveryChargeDetailsLog)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderService.GetChangeDeliveryChargeDetailsLog(changeDeliveryChargeDetailsLog);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }
            return response.ToHttpResponse();
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("GetDeliveryChargeDetailsPrice_test")]
        public async Task<IActionResult> GetDeliveryChargeDetailsPrice_test([FromBody] DeliveryChargeDetails_test deliveryChargeDetails)
        {
            var response = new SingleResponseModel<DeliveryChargeDetails_test>();

            try
            {
                var data = await _orderService.GetDeliveryChargeDetailsPrice_test(deliveryChargeDetails);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }
            return response.ToHttpResponse();
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("SendPushNotification/{courierUserId}")]
        public async Task<IActionResult> SendPushNotification(int courierUserId, [FromBody] OrderStatusViewModel request)
        {
            var response = new SingleResponseModel<bool>();
            try
            {
                var data = await _orderService.SendPushNotification(courierUserId, request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal server error, Please contact Delivery Tiger Tech Team";
            }

            return response.ToHttpResponse();
        }
    }
}