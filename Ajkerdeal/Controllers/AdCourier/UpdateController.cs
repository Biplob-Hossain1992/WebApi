using AdCourier.Domain.Entities.BodyModel;
using AdCourier.Domain.Entities.BodyModel.CollectorAssign;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Update")]
    //[Authorize]
    public class UpdateController : ControllerBase
    {
        private readonly IOrderTrackingRepository _orderTrackingRepository;
        private readonly IPackagingChargeRangeService _packagingChargeRangeService;
        private readonly IWeightRangeService _weightRangeService;
        private readonly IDeliveryRangeService _deliveryRangeService;
        private readonly IDeliveryChargeDetailsService _deliveryChargeDetailsService;
        private readonly IOrderService _orderService;
        private readonly IBreakableService _breakableService;
        private readonly IOrderTrackingService _orderTrackingService;
        private readonly IDistrictInfoService _districtInfoService;
        private readonly ILoanService _loanService;
        public UpdateController(IPackagingChargeRangeService packagingChargeRangeService,
            IWeightRangeService weightRangeService,
            IDeliveryRangeService deliveryRangeService,
            IDeliveryChargeDetailsService deliveryChargeDetailsService,
            IOrderService orderService,
            IBreakableService breakableService,
            IOrderTrackingService orderTrackingService,
            IDistrictInfoService districtInfoService,
            ILoanService loanService,
            IOrderTrackingRepository orderTrackingRepository)
        {
            _packagingChargeRangeService = packagingChargeRangeService;
            _weightRangeService = weightRangeService;
            _deliveryRangeService = deliveryRangeService;
            _deliveryChargeDetailsService = deliveryChargeDetailsService;
            _orderService = orderService;
            _breakableService = breakableService;
            _orderTrackingService = orderTrackingService;
            _districtInfoService = districtInfoService;
            _loanService = loanService;
            _orderTrackingRepository = orderTrackingRepository;

        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateTemporaryCollectors/{id}")]
        public async Task<IActionResult> UpdateTemporaryCollectors(int id, [FromBody] Collectors collector)
        {
            var response = new SingleResponseModel<Collectors>();

            try
            {
                var data = await _weightRangeService.UpdateTemporaryCollectors(id, collector);
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
        [Route("UpdateNowOfflineRiders/{id}")]
        public async Task<IActionResult> UpdateNowOfflineRiders(int id, [FromBody] DeliveryUsers rider)
        {
            var response = new SingleResponseModel<DeliveryUsers>();

            try
            {
                var data = await _weightRangeService.UpdateNowOfflineRiders(id, rider);
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
        [Route("UpdateIsPermanentRider/{id}")]
        public async Task<IActionResult> UpdateIsPermanentRider(int id, [FromBody] DeliveryUsers rider)
        {
            var response = new SingleResponseModel<DeliveryUsers>();

            try
            {
                var data = await _weightRangeService.UpdateIsPermanentRider(id, rider);
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
        [Route("UpdateRiderTypeOfDeliveryBondhu/{id}")]
        public async Task<IActionResult> UpdateRiderTypeOfDeliveryBondhu(int id, [FromBody] DeliveryUsers rider)
        {
            var response = new SingleResponseModel<DeliveryUsers>();

            try
            {
                var data = await _weightRangeService.UpdateRiderTypeOfDeliveryBondhu(id, rider);
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
        [Route("UpdateDeliveryBonduAssignMultiple")]
        public async Task<IActionResult> UpdateDeliveryBonduAssignMultiple([FromBody] List<DeliveryBonduAssign> deliveryBonduAssign)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _weightRangeService.UpdateDeliveryBonduAssignMultiple(deliveryBonduAssign);
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
        [Route("UpdateImageLink")]
        public async Task<IActionResult> UpdateImageLink([FromBody] DeliveryRange deliveryRange)
        {
            var response = new SingleResponseModel<DeliveryRange>();
            try
            {
                var data = await _weightRangeService.UpdateImageLink(deliveryRange);
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
        [Route("UpdateLocationAssign/{id}")]
        public async Task<IActionResult> UpdateLocationAssign(int id, [FromBody] LocationAssign locationAssign)
        {
            var response = new SingleResponseModel<LocationAssign>();

            try
            {
                var data = await _weightRangeService.UpdateLocationAssign(id, locationAssign);
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
        [Route("UpdateDbActionBn/{id}")]
        public async Task<IActionResult> UpdateDbActionBn(int id, [FromBody] DbActionBn dbActionBn)
        {
            var response = new SingleResponseModel<DbActionBn>();

            try
            {
                var data = await _weightRangeService.UpdateDbActionBn(id, dbActionBn);
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
        [Route("UpdatePickupLocations/{id}")]
        public async Task<IActionResult> UpdatePickupLocations(int id, [FromBody] PickupLocations pickupLocations)
        {
            var response = new SingleResponseModel<PickupLocations>();

            try
            {
                var data = await _weightRangeService.UpdatePickupLocations(id, pickupLocations);
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
        [Route("UpdatePaymentCycle")]
        public async Task<IActionResult> UpdatePaymentCycle([FromBody] CourierUsers userProfile)
        {
            var response = new SingleResponseModel<CourierUsers>();
            try
            {
                var data = await _weightRangeService.UpdatePaymentCycle(userProfile);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contant to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateWeightRange/{id}")]
        public async Task<IActionResult> UpdateWeightRange(int id, [FromBody] WeightRange weightRange)
        {
            var response = new SingleResponseModel<WeightRange>();

            try
            {
                var data = await _weightRangeService.UpdateWeightRange(id, weightRange);
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
        [Route("UpdateDeliveryRange/{id}")]
        public async Task<IActionResult> UpdateDeliveryRange(int id, [FromBody] DeliveryRange deliveryRange)
        {
            var response = new SingleResponseModel<DeliveryRange>();

            try
            {
                var data = await _deliveryRangeService.UpdateDeliveryRange(id, deliveryRange);
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
        [Route("UpdateDeliveryChargeDetails/{id}")]
        public async Task<IActionResult> UpdateDeliveryChargeDetails(int id, [FromBody] DeliveryChargeDetails deliveryChargeDetails)
        {
            var response = new SingleResponseModel<DeliveryChargeDetails>();

            try
            {
                var data = await _deliveryChargeDetailsService.UpdateDeliveryChargeDetails(id, deliveryChargeDetails);
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
        [Route("UpdateDeliveryChargeMerchantDetails/{id}")]
        public async Task<IActionResult> UpdateDeliveryChargeMerchantDetails(int id, [FromBody] DeliveryChargeMerchantDetails deliveryChargeMerchantDetails)
        {
            var response = new SingleResponseModel<DeliveryChargeMerchantDetails>();

            try
            {
                var data = await _deliveryChargeDetailsService.UpdateDeliveryChargeMerchantDetails(id, deliveryChargeMerchantDetails);
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
        [Route("UpdateOrderHistory/{OrderId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateOrderHistory(string OrderId, [FromBody] CourierOrderStatusHistoryViewModel courierOrderStatusHistory)
        {

            var response = new SingleResponseModel<CourierOrderStatusHistory>();

            try
            {
                var data = await _orderService.UpdateOrderHistory(OrderId, courierOrderStatusHistory);
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
        [AllowAnonymous]
        [Route("UpdateBulkStatus")]
        public async Task<IActionResult> UpdateBulkStatus([FromBody] List<CourierOrders> request)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _weightRangeService.UpdateBulkStatus(request);
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
        [Route("UpdateBreakableCharge/{id}")]
        public async Task<IActionResult> UpdateBreakableCharge(int id, [FromBody] ExtraCharge breakable)
        {
            var response = new SingleResponseModel<ExtraCharge>();

            try
            {
                var data = await _breakableService.UpdateBreakableCharge(id, breakable);
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
        [Route("UpdateStatusGroup/{id}")]
        public async Task<IActionResult> UpdateStatusGroup(int id, [FromBody] StatusGroup statusGroup)
        {
            var response = new SingleResponseModel<StatusGroup>();

            try
            {
                var data = await _orderTrackingService.UpdateStatusGroup(id, statusGroup);
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
        [Route("UpdateCourierOrderStatus/{id}")]
        public async Task<IActionResult> UpdateCourierOrderStatus(int id, [FromBody] CourierOrderStatus courierOrderStatus)
        {
            var response = new SingleResponseModel<CourierOrderStatus>();

            try
            {
                var data = await _orderTrackingService.UpdateCourierOrderStatus(id, courierOrderStatus);
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
        [Route("UpdateCollectorAssign/{id}")]
        public async Task<IActionResult> UpdateCollectorAssign(int id, [FromBody] CollectorAssign collectorAssign)
        {
            var response = new SingleResponseModel<CollectorAssign>();

            try
            {
                var data = await _orderTrackingService.UpdateCollectorAssign(id, collectorAssign);
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
        [Route("UpdateMultipleCollectorAssignForLocation")]
        public async Task<IActionResult> UpdateMultipleCollectorAssignForLocation([FromBody] MultipleCollectorAssign multipleCollectorAssign)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.UpdateMultipleCollectorAssignForLocation(multipleCollectorAssign);
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
        [Route("UpdateCollectorAssignForLocation/{id}")]
        public async Task<IActionResult> UpdateCollectorAssignForLocation(int id, [FromBody] CollectorAssign collectorAssign)
        {
            var response = new SingleResponseModel<CollectorAssign>();

            try
            {
                var data = await _orderTrackingService.UpdateCollectorAssignForLocation(id, collectorAssign);
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
        [AllowAnonymous]
        [Route("UpdateCourierOrderCollector")]
        public async Task<IActionResult> UpdateCourierOrderCollector([FromBody] List<CourierOrderStatusHistoryViewModel> updateCourierOrderCollectorBodyModel)
        {
            var response = new ListResponseModel<CourierOrderStatusHistory>();
            try
            {
                var data = await _orderService.UpdateCourierOrderCollector(updateCourierOrderCollectorBodyModel);
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
        [Route("UpdateMerchantInformation/{id}")]
        public async Task<IActionResult> UpdateMerchantInformation(int id, [FromBody] CourierUsers courierUserInfo)
        {
            var response = new SingleResponseModel<CourierUsers>();

            try
            {
                var data = await _orderTrackingService.UpdateMerchantInformation(id, courierUserInfo);
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
        [Route("CustomerVoiceSmsLimit/{courierUserId}/{customerVoiceSmsLimit}")]
        public async Task<IActionResult> CustomerVoiceSmsLimit(int courierUserId, int customerVoiceSmsLimit)
        {
            var response = new SingleResponseModel<CourierUsers>();

            try
            {
                var data = await _orderTrackingService.CustomerVoiceSmsLimit(courierUserId, customerVoiceSmsLimit);
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
        [Route("UpdateCustomerSMSLimit/{courierUserId}/{customerSMSLimit}")]
        public async Task<IActionResult> UpdateCustomerSMSLimit(int courierUserId, int customerSMSLimit)
        {
            var response = new SingleResponseModel<CourierUsers>();

            try
            {
                var data = await _orderTrackingService.UpdateCustomerSMSLimit(courierUserId, customerSMSLimit);
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
        [Route("UpdateViberSMSLimit/{courierUserId}/{viberSMSLimit}")]
        public async Task<IActionResult> UpdateViberSMSLimit(int courierUserId, int viberSMSLimit)
        {
            var response = new SingleResponseModel<CourierUsers>();

            try
            {
                var data = await _orderTrackingRepository.UpdateViberSMSLimit(courierUserId, viberSMSLimit);
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
        [Route("UpdatePackagingChargeRange/{id}")]
        public async Task<IActionResult> UpdatePackagingChargeRange(int id, [FromBody] PackagingChargeRange packagingChargeRange)
        {
            var response = new SingleResponseModel<PackagingChargeRange>();

            try
            {
                var data = await _packagingChargeRangeService.UpdatePackagingChargeRange(id, packagingChargeRange);
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
        [Route("UpdateCourierOrders/{id}")]
        public async Task<IActionResult> UpdateCourierOrders(string id, [FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateCourierOrders(id, courierOrders);
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
        [Route("UpdateDeliveryChargeFromOperation/{id}")]
        public async Task<IActionResult> UpdateDeliveryChargeFromOperation(string id, [FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateDeliveryChargeFromOperation(id, courierOrders);
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
        [Route("UpdateCourierOrdersApp/{id}")]
        public async Task<IActionResult> UpdateCourierOrdersApp(string id, [FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateCourierOrdersApp(id, courierOrders);
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
        [Route("UpdateCourierOrdersAppV2/{id}")]
        public async Task<IActionResult> UpdateCourierOrdersAppV2(string id, [FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateCourierOrdersAppV2(id, courierOrders);
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
        [Route("UpdateOrdersBulk")]
        public async Task<IActionResult> UpdateOrdersBulk([FromBody] List<CourierOrders> courierOrders)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderService.UpdateOrdersBulk(courierOrders);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }
            return response.ToHttpResponse();
        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateOrderInformation")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateOrderInformation([FromBody] List<CourierOrders> courierOrders)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderService.UpdateOrderInformation(courierOrders);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }
            return response.ToHttpResponse();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("FixSpecialCharacter/{courierOrdersId}")]
        public async Task<IActionResult> FixSpecialCharacter(string courierOrdersId)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderService.FixSpecialCharacter(courierOrdersId);
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
        [Route("UpdateOrdersBondhuApp/{id}")]
        public async Task<IActionResult> UpdateOrdersBondhuApp(int id, [FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateOrdersBondhuApp(id, courierOrders);
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
        [Route("UpdateDeliveryUsers/{userId}")]
        public async Task<IActionResult> UpdateDeliveryUsers(int userid, [FromBody] DeliveryUsers request)
        {
            var response = new SingleResponseModel<DeliveryUsers>();
            try
            {
                var data = await _orderTrackingService.UpdateDeliveryUsers(request, userid);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error. Please contact with technical support";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdatePickupLocationsForLatLong")]
        public async Task<IActionResult> UpdatePickupLocationsForLatLong([FromBody] PickupLocations pickupLocations)
        {
            var response = new SingleResponseModel<PickupLocations>();

            try
            {
                var data = await _weightRangeService.UpdatePickupLocationsForLatLong(pickupLocations);
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
        [Route("UpdatePriceWithWeight")]
        public async Task<IActionResult> UpdatePriceWithWeight([FromBody] DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _weightRangeService.UpdatePriceWithWeight(deliveryChargeDetailsBodyModel);
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
        [Route("UpdateDtCategory/{categoryId}")]
        public async Task<IActionResult> UpdateDtCategory(int categoryId, [FromBody] Category category)
        {
            var response = new SingleResponseModel<Category>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateDtCategory(categoryId, category);
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
        [Route("UpdateSubCategory/{subCategoryId}")]
        public async Task<IActionResult> UpdateSubCategory(int subCategoryId, [FromBody] SubCategory subCategory)
        {
            var response = new SingleResponseModel<SubCategory>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateSubCategory(subCategoryId, subCategory);
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
        [Route("UpdateServicePriceBulk")]
        public async Task<IActionResult> UpdateServicePriceBulk([FromBody] DeliveryRange deliveryRange)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateServicePriceBulk(deliveryRange);
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
        [Route("UpdateWeightRangePriceBulk")]
        public async Task<IActionResult> UpdateWeightRangePriceBulk([FromBody] WeightRange weightRange)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateWeightRangePriceBulk(weightRange);
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateBkashPayment")]
        public async Task<IActionResult> UpdateBkashPayment([FromBody] List<CourierOrders> orders)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateBkashPayment(orders);
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
        [Route("UpdateCourierPodnumbers")]
        public async Task<IActionResult> UpdateCourierPodnumbers([FromBody] List<CourierOrders> orders)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _packagingChargeRangeService.UpdateCourierPodnumbers(orders);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException.Message.ToString();
            }
            return response.ToHttpCreatedResponse();
        }

        /// <summary>
        /// UpdateAssignmentFalse
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns>int</returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateAssignmentFalse")]
        public async Task<IActionResult> UpdateAssignmentFalse([FromBody] RequestBodyModel requestBody)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.UpdateAssignmentFalse(requestBody);
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
        [AllowAnonymous]
        [Route("UpdateDeliveryChargeDetails")]
        public async Task<IActionResult> UpdateDeliveryChargeDetails([FromBody] AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.UpdateDeliveryChargeDetails(assignCouirerAndServiceBodyModel);
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
        [AllowAnonymous]
        [Route("UpdateDeliveryChargeMerchantDetails")]
        public async Task<IActionResult> UpdateDeliveryChargeMerchantDetails([FromBody] AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.UpdateDeliveryChargeMerchantDetails(assignCouirerAndServiceBodyModel);
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
        [AllowAnonymous]
        [Route("UpdateServiceTypeCourier")]
        public async Task<IActionResult> UpdateServiceTypeCourier([FromBody] AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.UpdateServiceTypeCourier(assignCouirerAndServiceBodyModel);
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
        [AllowAnonymous]
        [Route("UpdateMerchantServiceTypeCourier")]
        public async Task<IActionResult> UpdateMerchantServiceTypeCourier([FromBody] AssignCouirerAndServiceBodyModel assignCouirerAndServiceBodyModel)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.UpdateMerchantServiceTypeCourier(assignCouirerAndServiceBodyModel);
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
        [Route("UpdateInvoiceNumber")]
        public async Task<IActionResult> UpdateInvoiceNumber([FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _orderService.UpdateInvoiceNumber(courierOrders);
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
        [Route("UpdateRangeInvoiceNumber")]
        public async Task<IActionResult> UpdateRangeInvoiceNumber([FromBody] List<CourierOrders> request)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _orderService.UpdateRangeInvoiceNumber(request);
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
        [Route("UpdateDistrict")]
        public async Task<IActionResult> UpdateDistrict([FromBody] Districts districts)
        {
            var response = new SingleResponseModel<Districts>();

            try
            {
                var data = await _districtInfoService.UpdateDistrict(districts);
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
        [Route("UpdateTimeSlot")]
        public async Task<IActionResult> UpdateTimeSlot([FromBody] CollectionTimeSlot timeSlot)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _districtInfoService.UpdateTimeSlot(timeSlot);
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
        [Route("QuickUpdateStatus")]
        public async Task<IActionResult> QuickUpdateStatus([FromBody] RequestBodyModel requestBody)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _orderService.QuickUpdateStatus(requestBody);
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
        [Route("UpdateTelesalesStatus/{id}")]
        public async Task<IActionResult> UpdateTelesalesStatus(int id, [FromBody] CourierUsersViewModel courierUsers)
        {
            var response = new SingleResponseModel<CourierUsers>();
            try
            {
                var data = await _weightRangeService.UpdateTelesalesStatus(id, courierUsers);
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
        [Route("UpdateMultipleOrdersWithRider")]
        public async Task<IActionResult> UpdateMultipleOrdersWithRider([FromBody] List<CourierOrders> courierOrders)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _orderService.UpdateMultipleOrdersWithRider(courierOrders);
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
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateUserProfile/{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] Users users)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _weightRangeService.UpdateUserProfile(userId, users);
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
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateVoucher")]
        public async Task<IActionResult> UpdateVoucher([FromBody] Vouchers vouchers)
        {
            var response = new SingleResponseModel<Vouchers>();

            try
            {
                var data = await _orderTrackingService.UpdateVoucher(vouchers);
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
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateAdUserSalaryAmount")]
        public async Task<IActionResult> UpdateAdUserSalaryAmount([FromBody] Users user)
        {
            var response = new SingleResponseModel<Users>();

            try
            {
                var data = await _weightRangeService.UpdateAdUserSalaryAmount(user);
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
        /// Update price and ordertype
        /// </summary>
        /// <returns>A response with int</returns>
        /// <response code="200">Returns int</response>
        /// <response code="500">If there was an internal server error</response>

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdatePriceWithOrderType")]
        public async Task<IActionResult> UpdatePriceWithOrderType([FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderService.UpdatePriceWithOrderType(courierOrders);
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
        /// Update ServiceType
        /// </summary>
        /// <returns>A response with int</returns>
        /// <response code="200">Returns success</response>
        /// <response code="500">If there was an internal server error</response>

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateServiceType")]
        public async Task<IActionResult> UpdateServiceType([FromBody] CourierOrders courierOrders)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderService.UpdateServiceType(courierOrders);
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
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateLoanSurvey/{loanSurveyId}")]
        public async Task<IActionResult> UpdateLoanSurvey(int loanSurveyId, [FromBody] LoanSurvey loanSurvey)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _loanService.UpdateLoanSurvey(loanSurveyId, loanSurvey);
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
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateCourierWithLoanSurvey/{loanSurveyId}")]
        public async Task<IActionResult> UpdateCourierWithLoanSurvey(int loanSurveyId, [FromBody] List<CouriersWithLoanSurvey> couriersWithLoanSurveys)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _loanService.UpdateCourierWithLoanSurvey(loanSurveyId, couriersWithLoanSurveys);
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
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateMerchantReview/{CourierUserId}")]
        public async Task<IActionResult> UpdateMerchantReview(int CourierUserId, [FromBody] CourierUsers courierUsers)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderService.UpdateMerchantReview(CourierUserId, courierUsers);
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
        [Route("UpdateCourier")]
        public async Task<IActionResult> UpdateCourier([FromBody] Couriers couriers)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _districtInfoService.UpdateCourier(couriers);
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
        /// Update PoH Orders
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdatePoHOrders/{type}")]
        public async Task<IActionResult> UpdatePoHOrders([FromBody] CourierOrders request, string type)
        {
            var response = new SingleResponseModel<int>();
            try
            {
                var data = await _weightRangeService.UpdatePoHOrders(request, type);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal Error, please contact with Technical Support";
            }
            return response.ToHttpResponse();
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateUserLocationAssign/{userLocationAssignId}")]
        public async Task<IActionResult> UpdateUserLocationAssign(int userLocationAssignId, [FromBody] UserLocationAssign userLocationAssign)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _weightRangeService.UpdateUserLocationAssign(userLocationAssignId, userLocationAssign);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal Error, please contact with Technical Support";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Update Reference wise Payments
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UpdateReferencewisePayment/{paymentFrom}")]
        public async Task<IActionResult> UpdateReferencewisePayment([FromBody] PaymentReference request, string paymentFrom)
        {
            var response = new SingleResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.UpdateReferencewisePayment(request, paymentFrom);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact with Technical Support";
            }

            return response.ToHttpResponse();
        }

    }
}