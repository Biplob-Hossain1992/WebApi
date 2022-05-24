using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DapperDataModel;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ProcedureDataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.RegisteredUsers;
//using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.Report;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Report")]
    public class ReportController : Controller
    {
        private readonly IOrderReportService _orderReportService;
        private readonly IBondhuService _bondhuService;

        public ReportController(IOrderReportService orderReportService, IBondhuService bondhuService)
        {
            this._orderReportService = orderReportService;
            _bondhuService = bondhuService;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetOrders")]
        public async Task<IActionResult> GetOrders([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.GetOrders(orderBodyModel);
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
        [Route("GetMerchantOrders")]
        public async Task<IActionResult> GetMerchantOrders([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<MerchantOrder>();

            try
            {
                var data = await _orderReportService.GetMerchantOrders(orderBodyModel);
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
        [Route("GetReceivedOrders")]
        public async Task<IActionResult> GetReceivedOrders([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.GetReceivedOrders(orderBodyModel);
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
        [Route("CourierConsignment")]
        public async Task<IActionResult> CourierConsignment([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new SingleResponseModel<CourierConsignmentViewModel>();

            try
            {
                var data = await _orderReportService.CourierConsignment(orderBodyModel);
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
        [Route("CourierConsignmentDetails")]
        public async Task<IActionResult> CourierConsignmentDetails([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<CourierReportViewModel>();

            try
            {
                var data = await _orderReportService.CourierConsignmentDetails(requestBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Message = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("DeliveredOrderDetails")]
        public async Task<IActionResult> DeliveredOrderDetails([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<CourierOrdersViewModel>();

            try
            {
                var data = await _orderReportService.DeliveredOrderDetails(orderBodyModel);
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
        [Route("RetentionUsersPerformance")]
        public async Task<IActionResult> RetentionUsersPerformance([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<RetentionUserPerformanceDapperModel>();

            try
            {
                var data = await _orderReportService.RetentionUsersPerformance(orderBodyModel);
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
        [Route("GetDistrictSpeedByService")]
        public async Task<IActionResult> GetDistrictSpeedByService([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new SingleResponseModel<ServiceTypeNew>();

            try
            {
                var data = await _orderReportService.GetDistrictSpeedByService(orderBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Message = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("SRAssignedInactiveMerchantList/{retentionUserId}/{inactiveDuration}")]
        public async Task<IActionResult> SRAssignedInactiveMerchantList(int retentionUserId, int inactiveDuration)
        {
            var response = new ListResponseModel<RetentionUserPerformanceDapperModel>();

            try
            {
                var data = await _orderReportService.SRAssignedInactiveMerchantList(retentionUserId, inactiveDuration);
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
        [Route("RetentionUserWiseComplainDetails/{courierUserId}")]
        public async Task<IActionResult> RetentionUserWiseComplainDetails(int courierUserId)
        {
            var response = new ListResponseModel<RetentionComplainDetailsDapperModel>();

            try
            {
                var data = await _orderReportService.RetentionUserWiseComplainDetails(courierUserId);
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
        [Route("CollectorReceivedReport")]
        public async Task<IActionResult> CollectorReceivedReport([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<CollectorReceived>();

            try
            {
                var data = await _orderReportService.CollectorReceivedReport(orderBodyModel);
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
        [Route("CollectorReceivedReportDetails")]
        public async Task<IActionResult> CollectorReceivedReportDetails([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.CollectorReceivedReportDetails(orderBodyModel);
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
        [Route("OrderAssign")]
        public async Task<IActionResult> OrderAssign([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.OrderAssign(orderBodyModel);
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
        [Route("RiderOrderHistory")]
        public async Task<IActionResult> RiderOrderHistory([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.RiderOrderHistory(orderBodyModel);
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
        [Route("OrderAgeingReport/{statusList}")]
        public async Task<IActionResult> OrderAgeingReport(string statusList)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.OrderAgeingReport(statusList);
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
        [Route("GetPreferredPaymentCycle")]
        public async Task<IActionResult> GetPreferredPaymentCycle([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<CourierUsersViewModel>();

            try
            {
                var data = await _orderReportService.GetPreferredPaymentCycle(orderBodyModel);
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
        [Route("GetLastMileInformation")]
        public async Task<IActionResult> GetLastMileInformation([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<LastMileInformationDapperModel>();

            try
            {
                var data = await _orderReportService.GetLastMileInformation(orderBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Message = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetReferrerRefereeList")]
        public async Task<IActionResult> GetReferrerRefereeList([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.GetReferrerRefereeList(orderBodyModel);
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
        [Route("GetDeliveryRangeTypeWiseOrders")]
        public async Task<IActionResult> GetDeliveryRangeTypeWiseOrders([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<CourierOrdersViewModel>();

            try
            {
                var data = await _orderReportService.GetDeliveryRangeTypeWiseOrders(orderBodyModel);
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
        [Route("PackageDateDeliveryDateReport")]
        public async Task<IActionResult> PackageDateDeliveryDateReport([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<CourierOrdersDapperModel>();

            try
            {
                var data = await _orderReportService.PackageDateDeliveryDateReport(orderBodyModel);
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
        [Route("GetOrderFromWiseOrderCount")]
        public async Task<IActionResult> GetOrderFromWiseOrderCount([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<CourierOrdersViewModel>();

            try
            {
                var data = await _orderReportService.GetOrderFromWiseOrderCount(orderBodyModel);
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
        [Route("GetMerchantOrderFromDetails")]
        public async Task<IActionResult> GetMerchantOrderFromDetails([FromBody] LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var response = new ListResponseModel<CourierUsersViewModel>();

            try
            {
                var data = await _orderReportService.GetMerchantOrderFromDetails(loadCourierOrderBodyModel);
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
        [Route("GetAllRegisteredUsers/{joinDate}")]
        public async Task<IActionResult> GetAllRegisteredUsers(string joinDate)
        {
            var response = new ListResponseModel<RegisteredUsersViewModel>();

            try
            {
                var data = await _orderReportService.GetAllRegisteredUsers(joinDate);
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
        [Route("GetRetentionAcquisitionUsers/{userId}")]
        public async Task<IActionResult> GetRetentionAcquisitionUsers(int userId)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.GetRetentionAcquisitionUsers(userId);
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
        [Route("MerchantDetailsWithOrders/{joinDate}/{flag}")]
        public async Task<IActionResult> MerchantDetailsWithOrders(string joinDate, int flag)
        {
            var response = new ListResponseModel<MerchantDetailsResponseModel>();

            try
            {
                var data = await _orderReportService.MerchantDetailsWithOrders(joinDate, flag);
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
        [Route("AcquisitionManagerWiseOrderDetails")]
        public async Task<IActionResult> AcquisitionManagerWiseOrderDetails([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.AcquisitionManagerWiseOrderDetails(requestBodyModel);
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
        [Route("MerchantDetails/{joinDate}/{flag}")]
        public async Task<IActionResult> MerchantDetails(string joinDate, int flag)
        {
            var response = new ListResponseModel<MerchantDetailsResponseModel>();

            try
            {
                var data = await _orderReportService.MerchantDetails(joinDate, flag);
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
        [Route("GetTotalOrdersWithDateFlag")]
        public async Task<IActionResult> GetTotalOrdersWithDateFlag([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<OrderResponseDapperModel>();

            try
            {
                var data = await _orderReportService.GetTotalOrdersWithDateFlag(requestBody);
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
        [Route("GetCalledMerchantList")]
        public async Task<IActionResult> GetCalledMerchantList([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.GetCalledMerchantList(orderBodyModel);
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
        [Route("GetVisitedMerchantList")]
        public async Task<IActionResult> GetVisitedMerchantList([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.GetVisitedMerchantList(orderBodyModel);
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
        [Route("DateWiseOrderPlacedCalledMerchantList")]
        public async Task<IActionResult> DateWiseOrderPlacedCalledMerchantList([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.DateWiseOrderPlacedCalledMerchantList(orderBodyModel);
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
        [Route("DateWiseOrderPlacedVisitedMerchantList")]
        public async Task<IActionResult> DateWiseOrderPlacedVisitedMerchantList([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.DateWiseOrderPlacedVisitedMerchantList(orderBodyModel);
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
        [Route("StatusWiseTotalOrder")]
        public async Task<IActionResult> StatusWiseTotalOrder([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<OrderStatusCountView>();

            try
            {
                var data = await _orderReportService.StatusWiseTotalOrder(orderBodyModel);
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
        [Route("StatusWiseTotalOrderDetails")]
        public async Task<IActionResult> StatusWiseTotalOrderDetails([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.StatusWiseTotalOrderDetails(requestBodyModel);
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
        [Route("GetDeliveryBondhuShowOrderAutomatic")]
        public async Task<IActionResult> GetDeliveryBondhuShowOrderAutomatic()
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _bondhuService.GetDeliveryBondhuShowOrderAutomatic();
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
        [Route("GetUpdateTimeSlotAutomatic_TempOff")]
        public async Task<IActionResult> GetUpdateTimeSlotAutomatic()
        {
            var response = new ListResponseModel<CourierOrders>();

            try
            {
                var data = await _bondhuService.GetUpdateTimeSlotAutomatic();
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
        [Route("CourierWiseReturnReport")]
        public async Task<IActionResult> CourierWiseReturnReport([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<CourierOrdersViewModel>();

            try
            {
                var data = await _orderReportService.CourierWiseReturnReport(orderBodyModel);
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
        [Route("PendingShipmentReconciliation/{orderId}/{flag}")]
        public async Task<IActionResult> PendingShipmentReconciliation(string orderId, int flag)
        {
            var response = new ListResponseModel<CourierOrdersViewModel>();

            try
            {
                var data = await _orderReportService.PendingShipmentReconciliation(orderId, flag);
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
        [Route("HoursCalculationReport")]
        public async Task<IActionResult> HoursCalculationReport([FromBody] RequestBodyModel bodyModel)
        {
            var response = new ListResponseModel<HoursCalculationViewModel>();

            try
            {
                var data = await _orderReportService.HoursCalculationReport(bodyModel);
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
        [Route("HoursCalculationReportForDelivery")]
        public async Task<IActionResult> HoursCalculationReportForDelivery([FromBody] RequestBodyModel bodyModel)
        {
            var response = new ListResponseModel<HoursCalculationViewModel>();

            try
            {
                var data = await _orderReportService.HoursCalculationReportForDelivery(bodyModel);
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
        [Route("EatAnalysisOverCollectionReport")]
        public async Task<IActionResult> EatAnalysisOverCollectionReport([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.EatAnalysisOverCollectionReport(orderBodyModel);
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
        [Route("GetTelesalesActiveMerchantList")]
        public async Task<IActionResult> GetTelesalesActiveMerchantList([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.GetTelesalesActiveMerchantList(orderBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Message = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetCourierBillList")]
        public async Task<IActionResult> GetCourierBillList([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<CourierBillReportViewModel>();

            try
            {
                var data = await _orderReportService.GetCourierBillList(requestBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Message = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetLoanSurvey")]
        public async Task<IActionResult> GetLoanSurvey([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<LoanSurveyViewModel>();

            try
            {
                var data = await _orderReportService.GetLoanSurvey(orderBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Message = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetLoanSurveyByLender")]
        public async Task<IActionResult> GetLoanSurveyByLender([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<LoanSurveyViewModel>();

            try
            {
                var data = await _orderReportService.GetLoanSurveyByLender(orderBodyModel);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Message = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("MonthWiseTotalCollectionAmount/{CourierUserId}")]
        public async Task<IActionResult> MonthWiseTotalCollectionAmount(int CourierUserId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.MonthWiseTotalCollectionAmount(CourierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.Message = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("AcquisitionLeadManagement")]
        public async Task<IActionResult> AcquisitionLeadManagement([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.AcquisitionLeadManagement(orderBodyModel);
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
        [Route("ReAttemptOrdersList")]
        public async Task<IActionResult> ReAttemptOrdersList([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<CourierOrdersViewModel>();

            try
            {
                var data = await _orderReportService.ReAttemptOrdersList(orderBodyModel);
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
        [Route("GetVouchers")]
        public async Task<IActionResult> GetVouchers([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<CourierUsersViewModel>();

            try
            {
                var data = await _orderReportService.GetVouchers(requestBodyModel);
                response.Model = data;
            }
            catch (Exception)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Telesales details for individual status
        /// </summary>
        //need to remove
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("TelesalesDetails/{teleSalesStatus}/{date}")]
        public async Task<IActionResult> TelesalesDetails(int teleSalesStatus, DateTime date)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _orderReportService.TelesalesDetails(teleSalesStatus, date);
                response.Model = data;
            }
            catch (Exception)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Telesales details for individual status
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("StatusWiseTelesalesDetails")]
        public async Task<IActionResult> StatusWiseTelesalesDetails([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _orderReportService.StatusWiseTelesalesDetails(requestBodyModel);
                response.Model = data;
            }
            catch (Exception)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Status Wise History Count
        /// </summary>
        /// <returns>A response with StatusWiseHistoryCount list</returns>
        /// <response code="200">Returns the StatusWiseHistoryCount list</response>
        /// <response code="500">If there was an internal server error</response>
        /// 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("StatusWiseHistoryCount")]
        public async Task<IActionResult> StatusWiseHistoryCount([FromBody] RequestBodyModel bodyModel)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _orderReportService.StatusWiseHistoryCount(bodyModel);
                response.Model = data;
            }
            catch (Exception)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Status Wise History Count
        /// </summary>
        /// <returns>A response with StatusWiseHistoryCount list</returns>
        /// <response code="200">Returns the StatusWiseHistoryCount list</response>
        /// <response code="500">If there was an internal server error</response>
        /// 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("StatusWiseHistoryCountDetails")]
        public async Task<IActionResult> StatusWiseHistoryCountDetails([FromBody] RequestBodyModel bodyModel)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _orderReportService.StatusWiseHistoryCountDetails(bodyModel);
                response.Model = data;
            }
            catch (Exception)
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
        [Route("GetReRoutedOrders")]
        public async Task<IActionResult> GetReRoutedOrders([FromBody] OrderBodyModel orderBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.GetReRoutedOrders(orderBodyModel);
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
        [Route("SlotBasedOrder")]
        public async Task<IActionResult> SlotBasedOrder([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _orderReportService.SlotBasedOrder(requestBody);
                response.Model = data;
            }
            catch (Exception)
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
        [Route("TigerReport")]
        public async Task<IActionResult> TigerReport([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.TigerReport(requestBodyModel);
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
        [Route("RiderPaymentReport")]
        public async Task<IActionResult> RiderPaymentReport([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<RiderPaymentReportViewModel>();

            try
            {
                var data = await _orderReportService.RiderPaymentReport(requestBodyModel);
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
        [Route("ReturnCourierPendingStatusCount")]
        public async Task<IActionResult> ReturnCourierPendingStatusCount([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.ReturnCourierPendingStatusCount(requestBodyModel);
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
        [Route("ReturnCourierPendingStatusDetails")]
        public async Task<IActionResult> ReturnCourierPendingStatusDetails([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.ReturnCourierPendingStatusDetails(requestBodyModel);
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
        [Route("ReturnCourierPendingStatusCountDetails")]
        public async Task<IActionResult> ReturnCourierPendingStatusCountDetails([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderReportService.ReturnCourierPendingStatusCountDetails(requestBodyModel);
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
