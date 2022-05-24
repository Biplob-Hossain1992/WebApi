using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
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
    [Route("api/Entry")]
    [Authorize]
    public class EntryController : ControllerBase
    {
        private readonly IPackagingChargeRangeService _packagingChargeRangeService;
        private readonly IWeightRangeService _weightRangeService;
        private readonly IDeliveryRangeService _deliveryRangeService;
        private readonly IDeliveryChargeDetailsService _deliveryChargeDetailsService;
        private readonly IBreakableService _breakableService;
        private readonly IOrderTrackingService _orderTrackingService;
        private readonly IOrderRequestService _orderRequestService;
        private readonly IDistrictInfoService _districtInfoService;

        public EntryController(IPackagingChargeRangeService packagingChargeRangeService,
            IWeightRangeService weightRangeService,
            IDeliveryRangeService deliveryRangeService,
            IDeliveryChargeDetailsService deliveryChargeDetailsService,
            IBreakableService breakableService, IOrderTrackingService orderTrackingService, IOrderRequestService orderRequestService,
            IDistrictInfoService districtInfoService)
        {
            _packagingChargeRangeService = packagingChargeRangeService;
            _weightRangeService = weightRangeService;
            _deliveryRangeService = deliveryRangeService;
            _deliveryChargeDetailsService = deliveryChargeDetailsService;
            _breakableService = breakableService;
            _orderTrackingService = orderTrackingService;
            _orderRequestService = orderRequestService;
            _districtInfoService = districtInfoService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddDbActionBn")]
        public async Task<IActionResult> AddDbActionBn([FromBody] DbActionBn dbActionBn)
        {
            var response = new SingleResponseModel<DbActionBn>();

            try
            {
                var data = await _weightRangeService.AddDbActionBn(dbActionBn);
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
        [Route("AddOrderAssignTrack")]
        public async Task<IActionResult> AddOrderAssignTrack([FromBody] OrderAssignTrack orderAssignTrack)
        {
            var response = new SingleResponseModel<OrderAssignTrack>();

            try
            {
                var data = await _weightRangeService.AddOrderAssignTrack(orderAssignTrack);
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
        [Route("AddCollectionTimeSlot")]
        public async Task<IActionResult> AddCollectionTimeSlot([FromBody] CollectionTimeSlot collectionTimeSlot)
        {
            var response = new SingleResponseModel<CollectionTimeSlot>();

            try
            {
                var data = await _weightRangeService.AddCollectionTimeSlot(collectionTimeSlot);
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
        [Route("AddPickupLocations")]
        public async Task<IActionResult> AddPickupLocations([FromBody] PickupLocations pickupLocations)
        {
            var response = new SingleResponseModel<PickupLocations>();

            try
            {
                var data = await _weightRangeService.AddPickupLocations(pickupLocations);
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
        [Route("AddWeightRange")]
        public async Task<IActionResult> AddWeightRange([FromBody] WeightRange weightRange)
        {
            var response = new SingleResponseModel<WeightRange>();

            try
            {
                var data = await _weightRangeService.AddWeightRange(weightRange);
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
        [Route("AddDeliveryRange")]
        public async Task<IActionResult> AddDeliveryRange([FromBody] DeliveryRange deliveryRange)
        {
            var response = new SingleResponseModel<DeliveryRange>();

            try
            {
                var data = await _deliveryRangeService.AddDeliveryRange(deliveryRange);
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
        [Route("AddDeliveryChargeDetails")]
        public async Task<IActionResult> AddDeliveryChargeDetails([FromBody] DeliveryChargeDetails deliveryChargeDetails)
        {
            var response = new SingleResponseModel<DeliveryChargeDetails>();

            try
            {
                var data = await _deliveryChargeDetailsService.AddDeliveryChargeDetails(deliveryChargeDetails);
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddBreakableCharge")]
        public async Task<IActionResult> AddBreakableCharge([FromBody] ExtraCharge breakable)
        {
            var response = new SingleResponseModel<ExtraCharge>();

            try
            {
                var data = await _breakableService.AddBreakableCharge(breakable);
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
        [Route("AddCourierOrderStatus")]
        public async Task<IActionResult> AddCourierOrderStatus([FromBody] CourierOrderStatus courierOrderStatus)
        {
            var response = new SingleResponseModel<CourierOrderStatus>();

            try
            {
                var data = await _orderTrackingService.AddCourierOrderStatus(courierOrderStatus);
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
        [Route("AddStatusGroup")]
        public async Task<IActionResult> AddStatusGroup([FromBody] StatusGroup statusGroup)
        {
            var response = new SingleResponseModel<StatusGroup>();

            try
            {
                var data = await _orderTrackingService.AddStatusGroup(statusGroup);
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
        [AllowAnonymous]
        [Route("AddMultipleLocationAssign")]
        public async Task<IActionResult> AddMultipleLocationAssign([FromBody] List<LocationAssign> locationAssign)
        {
            var response = new ListResponseModel<LocationAssign>();

            try
            {
                var data = await _orderTrackingService.AddMultipleLocationAssign(locationAssign);
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
        [Route("AddMultipleCollectorAssign")]
        public async Task<IActionResult> AddMultipleCollectorAssign([FromBody] List<CollectorAssign> collectorAssign)
        {
            var response = new ListResponseModel<CollectorAssign>();

            try
            {
                var data = await _orderTrackingService.AddMultipleCollectorAssign(collectorAssign);
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
        [Route("AddCollectorAssign")]
        public async Task<IActionResult> AddCollectorAssign([FromBody] CollectorAssign collectorAssign)
        {
            var response = new SingleResponseModel<CollectorAssign>();

            try
            {
                var data = await _orderTrackingService.AddCollectorAssign(collectorAssign);
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
        [Route("AddDeliveryBonduAssign")]
        public async Task<IActionResult> AddDeliveryBonduAssign([FromBody] DeliveryBonduAssign deliveryBonduAssign)
        {
            var response = new SingleResponseModel<DeliveryBonduAssign>();

            try
            {
                var data = await _orderTrackingService.AddDeliveryManAssign(deliveryBonduAssign);
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
        [Route("AddDeliveryBonduAssignMultiple")]
        public async Task<IActionResult> AddDeliveryBonduAssignMultiple([FromBody] List<DeliveryBonduAssign> deliveryBonduAssign)
        {
            var response = new ListResponseModel<DeliveryBonduAssign>();

            try
            {
                var data = await _orderTrackingService.AddDeliveryBonduAssignMultiple(deliveryBonduAssign);
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
        [Route("AddPackagingChargeRange")]
        public async Task<IActionResult> AddPackagingChargeRange([FromBody] PackagingChargeRange packagingChargeRange)
        {
            var response = new SingleResponseModel<PackagingChargeRange>();

            try
            {
                var data = await _packagingChargeRangeService.AddPackagingChargeRange(packagingChargeRange);
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
        [Route("AddSurveyQuestionAnswerLog")]
        public async Task<IActionResult> AddSurveyQuestionAnswerLog([FromBody] List<SurveyQuestionAnswerLog> surveyQuestionAnswerLog)
        {
            var response = new ListResponseModel<SurveyQuestionAnswerLog>();

            try
            {
                var data = await _packagingChargeRangeService.AddSurveyQuestionAnswerLog(surveyQuestionAnswerLog);
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
        [Route("AddDtCategories")]
        public async Task<IActionResult> AddDtCategories([FromBody] List<Category> categories)
        {
            var response = new ListResponseModel<Category>();

            try
            {
                var data = await _packagingChargeRangeService.AddDtCategories(categories);
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
        [Route("AddSubCategories")]
        public async Task<IActionResult> AddSubCategories([FromBody] List<SubCategory> subCategories)
        {
            var response = new ListResponseModel<SubCategory>();

            try
            {
                var data = await _packagingChargeRangeService.AddSubCategories(subCategories);
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
        [AllowAnonymous]
        [Route("AddAssignCouirerAndService")]
        public async Task<IActionResult> AddAssignCouirerAndService([FromBody] List<DeliveryChargeDetails> deliveryChargeDetails)
        {
            var response = new ListResponseModel<DeliveryChargeDetails>();

            try
            {
                var data = await _packagingChargeRangeService.AddAssignCouirerAndService(deliveryChargeDetails);
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
        [AllowAnonymous]
        [Route("AddMerchantWiseAssignCouirerAndService")]
        public async Task<IActionResult> AddMerchantWiseAssignCouirerAndService([FromBody] List<DeliveryChargeMerchantDetails> deliveryChargeMerchantDetails)
        {
            var response = new ListResponseModel<DeliveryChargeMerchantDetails>();

            try
            {
                var data = await _packagingChargeRangeService.AddMerchantWiseAssignCouirerAndService(deliveryChargeMerchantDetails);
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
        [AllowAnonymous]
        [Route("AddOrderRequest")]
        public async Task<IActionResult> AddOrderRequest([FromBody] OrderRequest request)
        {
            var response = new SingleResponseModel<OrderRequest>();
            try
            {
                var data = await _orderRequestService.AddOrderRequest(request);
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
        [AllowAnonymous]
        [Route("AddDistrict")]
        public async Task<IActionResult> AddDistrict([FromBody] Districts districts)
        {
            var response = new SingleResponseModel<Districts>();
            try
            {
                var data = await _districtInfoService.AddDistrict(districts);
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
        [AllowAnonymous]
        [Route("BkashPaymentLog")]
        public async Task<IActionResult> BkashPaymentLog([FromBody] List<BkashPayment> request)
        {
            var response = new ListResponseModel<BkashPayment>();
            try
            {
                var data = await _packagingChargeRangeService.BkashPaymentLog(request);
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
        [AllowAnonymous]
        [Route("AddHub")]
        public async Task<IActionResult> AddHub([FromBody] Hub hub)
        {
            var response = new SingleResponseModel<Hub>();
            try
            {
                var data = await _districtInfoService.AddHub(hub);
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("UpdateHub")]
        public async Task<IActionResult> UpdateHub([FromBody] Hub hub)
        {
            var response = new SingleResponseModel<Hub>();
            try
            {
                var data = await _districtInfoService.UpdateHub(hub);
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
        [AllowAnonymous]
        [Route("AddCustomerSMSLog")]
        public async Task<IActionResult> AddCustomerSMSLog([FromBody] List<CustomerSMS> customerSMs)
        {
            var response = new ListResponseModel<CustomerSMS>();
            try
            {
                var data = await _packagingChargeRangeService.AddCustomerSMSLog(customerSMs);
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
        [AllowAnonymous]
        [Route("AddCouriersWithLoanSurvey")]
        public async Task<IActionResult> AddCouriersWithLoanSurvey([FromBody] List<CouriersWithLoanSurvey> couriersWithLoanSurvey)
        {
            var response = new ListResponseModel<CouriersWithLoanSurvey>();
            try
            {
                var data = await _packagingChargeRangeService.AddCouriersWithLoanSurvey(couriersWithLoanSurvey);
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
        [Route("AddVoucher")]
        public async Task<IActionResult> AddVoucher([FromBody] List<Vouchers> vouchers)
        {
            var response = new ListResponseModel<Vouchers>();
            try
            {
                var data = await _weightRangeService.AddVoucher(vouchers);
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
        [Route("AddOwnPhoneBook")]
        public async Task<IActionResult> AddOwnPhoneBook([FromBody] List<OwnPhoneBook> ownPhoneBook)
        {
            var response = new ListResponseModel<OwnPhoneBook>();

            try
            {
                var data = await _deliveryRangeService.AddOwnPhoneBook(ownPhoneBook);
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
        [Route("AddPhoneBookGroup")]
        public async Task<IActionResult> AddPhoneBookGroup([FromBody] List<PhoneBookGroup> phoneBookGroup)
        {
            var response = new ListResponseModel<PhoneBookGroup>();

            try
            {
                var data = await _deliveryRangeService.AddPhoneBookGroup(phoneBookGroup);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpCreatedResponse();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddNumnerInGroup")]
        public async Task<IActionResult> AddNumnerInGroup([FromBody] List<OwnPhoneBook> ownPhoneBook)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _deliveryRangeService.AddNumnerInGroup(ownPhoneBook);
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
        [Route("AddAcquisitionLead")]
        public async Task<IActionResult> AddAcquisitionLead([FromBody] AcquisitionLeadManagement acquisitionLead)
        {
            var response = new SingleResponseModel<AcquisitionLeadManagement>();
            try
            {
                var data = await _weightRangeService.AddAcquisitionLead(acquisitionLead);
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
        [AllowAnonymous]
        [Route("AddMerchantInfoUpdateLog")]
        public async Task<IActionResult> AddMerchantInfoUpdateLog([FromBody] MerchantInfoUpdate merchantInfoUpdate)
        {
            var response = new SingleResponseModel<MerchantInfoUpdate>();
            try
            {
                var data = await _districtInfoService.AddMerchantInfoUpdateLog(merchantInfoUpdate);
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
        [Route("AddSmsPurchase")]
        public async Task<IActionResult> AddSmsPurchase([FromBody] SMSPurchase request)
        {
            var response = new SingleResponseModel<SMSPurchase>();
            try
            {
                var data = await _weightRangeService.AddSmsPurchase(request);
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddLenderUser")]
        public async Task<IActionResult> AddLenderUser([FromBody] LenderUser lenderUser)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _weightRangeService.AddLenderUser(lenderUser);
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddUserLocationAssign")]
        public async Task<IActionResult> AddUserLocationAssign([FromBody] UserLocationAssign userLocationAssign)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _weightRangeService.AddUserLocationAssign(userLocationAssign);
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
        /// AddCourier
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// /// <response code="400">If There was a Bad Request</response>
        /// <response code="500">If there was an internal server error</response>
        ///
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("AddCourier")]
        public async Task<IActionResult> AddCourier([FromBody] Couriers couriers)
        {
            var response = new SingleResponseModel<Couriers>();
            try
            {
                var data = await _orderRequestService.AddCourier(couriers);
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
        [Route("AssignLenderUserToCourierUser")]
        public async Task<IActionResult> AssignLenderUserToCourierUser([FromBody] List<LenderCourierUserAssignment> lenderCourierUserAssignments)
        {
            var response = new ListResponseModel<LenderCourierUserAssignment>();

            try
            {
                var data = await _weightRangeService.AssignLenderUserToCourierUser(lenderCourierUserAssignments);
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("UnAssignLenderUserToCourierUser")]
        public async Task<IActionResult> UnAssignLenderUserToCourierUser([FromBody] List<LenderCourierUserAssignment> lenderCourierUserAssignments)
        {
            var response = new ListResponseModel<LenderCourierUserAssignment>();

            try
            {
                var data = await _weightRangeService.UnAssignLenderUserToCourierUser(lenderCourierUserAssignments);
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
        /// AddCustomComment
        /// </summary>
        /// <returns>A response with  inserted model</returns>
        /// <response code="200">Returns true</response>
        /// /// <response code="400">If There was a Bad Request</response>
        /// <response code="500">If there was an internal server error</response>
        ///
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("AddCustomComment")]
        public async Task<IActionResult> AddCustomComment([FromBody] CustomComment customComment)
        {
            var response = new SingleResponseModel<CustomComment>();
            try
            {
                var data = await _districtInfoService.AddCustomComment(customComment);
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
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("BulkInsertRedxPopData")]
        public async Task<IActionResult> BulkInsertRedxPopData([FromBody] List<RedxPop> popData)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.BulkInsertRedxPopData(popData);
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
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("AddMailContent")]
        public async Task<IActionResult> AddMailContent([FromBody] PaymentMail request)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _orderTrackingService.AddMailContent(request);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpCreatedResponse();
        }
    }
}