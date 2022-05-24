using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.OrderRequest;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Bondhu")]
    public class BondhuController : ControllerBase
    {

        private readonly IBondhuService _bondhuService;
        private readonly IQuickOrderService _quickOrderService;

        public BondhuController(IBondhuService bondhuService, IQuickOrderService quickOrderService)
        {
            _bondhuService = bondhuService;
            _quickOrderService = quickOrderService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetBondhuAcceptStatus")]
        public async Task<IActionResult> GetBondhuAcceptStatus([FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<bool>();
            try
            {
                var data = await _bondhuService.GetBondhuAcceptStatus(courierOrders);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact to technical support";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// GetBondhuAcceptStatus_Test
        /// </summary>
        /// <param name="courierOrders"></param>
        /// <returns>list</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetBondhuAcceptStatus_Test")]
        public async Task<IActionResult> GetBondhuAcceptStatus_Test([FromBody] List<CourierOrders> courierOrders)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _bondhuService.GetBondhuAcceptStatus_Test(courierOrders);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact to technical support";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("LoadOrderForBondhuApp")]
        public async Task<IActionResult> LoadOrderForBondhuApp([FromBody] SelfDeliveryOrderRequestNewModel model)
        {
            var response = new SingleResponseModel<SelfDeliveryAllDataResponseModel>();

            try
            {
                var data = await _bondhuService.LoadOrderForBondhuApp(model);
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
        [ProducesResponseType(500)]
        [Route("LoadOrderForBondhuAppByTimeSlot")]
        public async Task<IActionResult> LoadOrderForBondhuAppByTimeSlot([FromBody] SelfDeliveryOrderRequestNewModel model)
        {
            var response = new SingleResponseModel<SelfDeliveryAllDataResponseModel>();

            try
            {
                var data = await _bondhuService.LoadOrderForBondhuAppByTimeSlot(model);
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
        [ProducesResponseType(500)]
        [Route("LoadOrderForBondhuAppByTimeSlotNew")]
        public async Task<IActionResult> LoadOrderForBondhuAppByTimeSlotNew([FromBody] SelfDeliveryOrderRequestNewModel model)
        {
            var response = new SingleResponseModel<SelfDeliveryAllDataResponseModel>();

            try
            {
                var data = await _bondhuService.LoadOrderForBondhuAppByTimeSlotNew(model);
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
        [Route("UpdateBondhuOrder")]
        public async Task<IActionResult> UpdateBondhuOrder([FromBody] List<CourierOrders> courierOrders)
        {
            var response = new SingleResponseModel<bool>();
            try
            {
                var data = await _bondhuService.UpdateBondhuOrder(courierOrders);
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
        [ProducesResponseType(500)]
        [Route("DeliveryManRegistration")]
        public async Task<IActionResult> DeliveryManRegistration([FromBody] DeliveryBondhuRegistration bondhuRegistration)
        {
            var response = new SingleResponseModel<SelfDeliveryModel>();

            try
            {
                var data = await _bondhuService.DeliveryManRegistration(bondhuRegistration);
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
        [ProducesResponseType(500)]
        [Route("UpdateDeliveryManInfo")]
        public async Task<IActionResult> UpdateDeliveryManInfo([FromBody] DeliveryManGeneralInfoUpdate infoUpdate)
        {
            var response = new SingleResponseModel<bool>();

            try
            {
                var data = await _bondhuService.UpdateDeliveryManInfo(infoUpdate);
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
        [ProducesResponseType(500)]
        [Route("Login")]
        public async Task<IActionResult> SelfDeliveryLogin([FromBody] SelfDeliveryLoginModel model)
        {
            var response = new SingleResponseModel<SelfDeliveryLoginResponseModel>();

            try
            {
                var data = await _bondhuService.SelfDeliveryLogin(model);
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
        [ProducesResponseType(500)]
        [Route("GetDeliveryBondhuOrderStatusHistoryCountDeliveryManWise")]
        public async Task<IActionResult> GetDeliveryBondhuOrderStatusHistoryCountDeliveryManWise([FromBody] DeliveryBondhuOrderSearchModel searchModel)
        {
            var response = new ListResponseModel<OrderStatusCountDeliveryManWise>();

            try
            {
                var data = await _bondhuService.GetDeliveryBondhuOrderStatusHistoryCountDeliveryManWise(searchModel);
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
        [ProducesResponseType(500)]
        [Route("GetDtOrderHistoryDetailsReportForDeliveryMan")]
        public async Task<IActionResult> GetDtOrderHistoryDetailsReportForDeliveryMan([FromBody] DeliveryBondhuOrderSearchModel searchModel)
        {
            var response = new ListResponseModel<DtOrderDetailsDataModel>();

            try
            {
                var data = await _bondhuService.GetDtOrderHistoryDetailsReportForDeliveryMan(searchModel);
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
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetBondhuInfo/{bondhuId}")]
        public async Task<IActionResult> GetBondhuInfo(int bondhuId)
        {
            var response = new SingleResponseModel<UserAccessResponseModel>();
            try
            {
                var data = await _bondhuService.GetBondhuInfo(bondhuId);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact to technical support";
            }

            return response.ToHttpResponse();
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UserAccess/{BondhuId}/{IsNowOffline}")]
        public async Task<IActionResult> UserAccess(int BondhuId, bool IsNowOffline)
        {
            var response = new SingleResponseModel<DeliveryUsers>();
            try
            {
                var data = await _bondhuService.UserAccess(BondhuId, IsNowOffline);
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
        [Route("GetQuickOrders")]
        public async Task<IActionResult> GetQuickOrders([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<CourierUsersViewModel>();
            try
            {
                var data = await _quickOrderService.GetQuickOrders(request);
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
        [Route("GetMerchantQuickOrders")]
        public async Task<IActionResult> GetMerchantQuickOrders([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<OrderRequestViewModel>();
            try
            {
                var data = await _quickOrderService.GetMerchantQuickOrders(request);
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
        [Route("UpdateOrderRequests")]
        public async Task<IActionResult> UpdateOrderRequests([FromBody] List<OrderRequest> orderRequests)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _quickOrderService.UpdateOrderRequests(orderRequests);
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
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetQuickOrderStatus")]
        public async Task<IActionResult> GetQuickOrderStatus()
        {
            var response = new ListResponseModel<ActionModel>();
            try
            {
                //string ColorCode1 = "#45BE59";
                var actionModel = new List<ActionModel>();

                actionModel.Add(new ActionModel
                {
                    ButtonName = "নতুন অর্ডার",
                    StatusUpdate = 0,
                    StatusMessage = "",
                    ColorCode = ""
                });

                actionModel.Add(new ActionModel
                {
                    ButtonName = "অ্যাকসেপ্ট",
                    StatusUpdate = 41,
                    StatusMessage = "",
                    ColorCode = ""
                });

                actionModel.Add(new ActionModel
                {
                    ButtonName = "কালেক্ট করা হয়েছে",
                    StatusUpdate = 44,
                    StatusMessage = "",
                    ColorCode = ""
                });


                var data = await Task.FromResult(actionModel);

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
        [Route("UpdateDocumentUrl")]
        public async Task<IActionResult> UpdateDocumentUrl([FromBody] List<CourierOrders> orders)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _bondhuService.UpdateDocumentUrl(orders);
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
        [Route("AddLatLag")]
        public async Task<IActionResult> AddLatLag([FromBody] LatLagModel model)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _bondhuService.AddLatLag(model);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// GetZoneWiseOrdersCount
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// <response code="500">If there was an internal server error</response>

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetZoneWiseOrdersCount")]
        public async Task<IActionResult> GetZoneWiseOrdersCount([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _bondhuService.GetZoneWiseOrdersCount(requestBody);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// GetZoneWiseOrderDetails
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// <response code="500">If there was an internal server error</response>
        /// 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetZoneWiseOrderDetails")]
        public async Task<IActionResult> GetZoneWiseOrderDetails([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _bondhuService.GetZoneWiseOrderDetails(requestBody);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// CollectedNotCollectedMerchantInfo
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// <response code="500">If there was an internal server error</response>
        /// 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("CollectedNotCollectedMerchantInfo")]
        public async Task<IActionResult> CollectedNotCollectedMerchantInfo([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _bondhuService.CollectedNotCollectedMerchantInfo(requestBody);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }
        /// <summary>
        /// UpdateSelfDeliveryUserPassword
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// <response code="500">If there was an internal server error</response>
        /// 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateSelfDeliveryUserPassword")]
        public async Task<IActionResult> UpdateSelfDeliveryUserPassword([FromBody] SelfDeliveryUserPasswordUpdateModel updateModel)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _bondhuService.UpdateSelfDeliveryUserPassword(updateModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// DeliveredAndPendingCustomerInfo
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// <response code="500">If there was an internal server error</response>
        /// 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("DeliveredAndPendingCustomerInfo")]
        public async Task<IActionResult> DeliveredAndPendingCustomerInfo([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _bondhuService.DeliveredAndPendingCustomerInfo(requestBody);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// MerchantWiseOrder
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// <response code="500">If there was an internal server error</response>
        /// 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("MerchantWiseOrder")]
        public async Task<IActionResult> MerchantWiseOrder([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _bondhuService.MerchantWiseOrder(requestBody);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// GetAllLocationAssignHistory
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// <response code="500">If there was an internal server error</response>
        /// 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetAllLocationAssignHistory")]
        public async Task<IActionResult> GetAllLocationAssignHistory([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _bondhuService.GetAllLocationAssignHistory(requestBody);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// GetCustomCommentsWithDateRange
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns> list </returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetCustomCommentsWithDateRange")]
        public async Task<IActionResult> GetCustomCommentsWithDateRange([FromBody] RequestBodyModel requestBody)
        {
            var response = new SingleResponseModel<dynamic>();
            try
            {
                var data = await _bondhuService.GetCustomCommentsWithDateRange(requestBody);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// GetMerchantWiseRiderCountWithDetails
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns> List </returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetMerchantWiseRiderCountWithDetails")]
        public async Task<IActionResult> GetMerchantWiseRiderCountWithDetails([FromBody] RequestBodyModel requestBody)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _bondhuService.GetMerchantWiseRiderCountWithDetails(requestBody);
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
