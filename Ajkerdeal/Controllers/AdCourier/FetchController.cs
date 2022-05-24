using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails;
using AdCourier.Domain.Entities.BodyModel.OrderTracking;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CollectorAssign;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.DeliverManAssign;
using AdCourier.Domain.Entities.ViewModel.DeliveryManAssign;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using AdCourier.Domain.Entities.ViewModel.Offer;
using AdCourier.Domain.Entities.ViewModel.OrderTracking;
using AdCourier.Domain.Entities.ViewModel.Poh;
using AdCourier.Domain.Entities.ViewModel.ReturnProducts;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Fetch")]
    //[Authorize]
    public class FetchController : ControllerBase
    {
        
        private readonly IPackagingChargeRangeService _packagingChargeRangeService;
        private readonly IWeightRangeService _weightRangeService;
        private readonly IDeliveryRangeService _deliveryRangeService;
        private readonly IDeliveryChargeDetailsService _deliveryChargeDetailsService;
        private readonly IOrderService _orderService;
        private readonly IBreakableService _breakableService;
        private readonly IOrderTrackingService _orderTrackingService;
        private readonly ILoginService _loginService;
        private readonly IReturnService _returnService;
        private readonly ILoginRepository _loginRepository;
        private readonly IDistrictInfoService _districtInfoService;
        private readonly IWeightRangeRepository _weightRangeRepository;

        public FetchController(IPackagingChargeRangeService packagingChargeRangeService, 
            IWeightRangeService weightRangeService,
            IDeliveryRangeService deliveryRangeService,
            IDeliveryChargeDetailsService deliveryChargeDetailsService,
            IOrderService orderService,
            IBreakableService breakableService,
            IOrderTrackingService orderTrackingService,
            ILoginService loginService,
            IReturnService returnService,
            ILoginRepository loginRepository,
            IDistrictInfoService districtInfoService,
            IWeightRangeRepository weightRangeRepository)
        {
            _packagingChargeRangeService = packagingChargeRangeService;
            _weightRangeService = weightRangeService;
            _deliveryRangeService = deliveryRangeService;
            _deliveryChargeDetailsService = deliveryChargeDetailsService;
            _orderService = orderService;
            _breakableService = breakableService;
            _orderTrackingService = orderTrackingService;
            _loginService = loginService;
            _returnService = returnService;
            _loginRepository = loginRepository;
            _districtInfoService = districtInfoService;
            _weightRangeRepository = weightRangeRepository;
        }

        /// <summary>
        /// Retrieves Weight Range
        /// </summary>
        /// <returns>A response with weight range list</returns>
        /// <response code="200">Returns the weight range list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetWeightRange")]
        public async Task<IActionResult> GetWeightRange()
        {
            var response = new ListResponseModel<WeightRange>();

            try
            {
                var data = await _weightRangeService.GetWeightRange();
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
        [Route("GetMerchantCollectionCharge/{id}")]
        public async Task<IActionResult> GetMerchantCollectionCharge(int id)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = await _weightRangeService.GetMerchantCollectionCharge(id);
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
        [Route("GetMerchantCredit/{id}")]
        public IActionResult GetMerchantCredit(int id)
        {
            var response = new SingleResponseModel<bool>();

            try
            {
                var data = _weightRangeService.GetMerchantCredit(id);
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
        [Route("GetMerchantServiceCharge/{id}")]
        public IActionResult GetMerchantServiceCharge(int id)
        {
            var response = new SingleResponseModel<int>();

            try
            {
                var data = id;
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
        [Route("GetDbActionBn")]
        public async Task<IActionResult> GetDbActionBn()
        {
            var response = new ListResponseModel<DbActionBn>();

            try
            {
                var data = await _weightRangeService.GetDbActionBn();
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
        [Route("GetDTServices")]
        public async Task<IActionResult> GetDTServices()
        {
            var response = new ListResponseModel<ServiceType>();

            try
            {
                var data = await _weightRangeService.GetDTServices();
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with technical suppor";
            }

            return response.ToHttpResponse();
        }




        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("GetOrderInformation/{orderId}")]
        public async Task<IActionResult> GetOrderInformation(int orderId)
        {
            var response = new SingleResponseModel<CourierOrders>();

            try
            {
                var data = await _orderService.GetOrderInformation(orderId);
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
        [AllowAnonymous]
        [Route("GetOrderHistoryInformation/{orderId}")]
        public async Task<IActionResult> GetOrderHistoryInformation(string orderId)
        {
            var response = new ListResponseModel<OrderStatusHistoryViewModel>();

            try
            {
                var data = await _orderService.GetOrderHistoryInformation(orderId);
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
        [Route("GetCollectionTimeSlot")]
        public async Task<IActionResult> GetCollectionTimeSlot()
        {
            var response = new ListResponseModel<CollectionTimeSlotViewModel>();

            try
            {
                var data = await _deliveryRangeService.GetCollectionTimeSlot();
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
        [Route("GetCollectionTimeSlotByTime")]
        public async Task<IActionResult> GetCollectionTimeSlotByTime([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<CollectionTimeSlotViewModel>();
            try
            {
                var data = await _deliveryRangeService.GetCollectionTimeSlotByTime(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetDeliveryRange")]
        public async Task<IActionResult> GetDeliveryRange()
        {
            var response = new ListResponseModel<DeliveryRange>();

            try
            {
                var data = await _deliveryRangeService.GetDeliveryRange();
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
        [Route("GetDeliveryChargeDetails")]
        public async Task<IActionResult> GetDeliveryChargeDetails()
        {
            var response = new ListResponseModel<GetDeliveryChargeDetailsViewModel>();
            try
            {

                var data = await _deliveryChargeDetailsService.GetDeliveryChargeDetails();

                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("GetSACodChargeList")]
        public async Task<IActionResult> GetSACodChargeList()
        {
            var response = new ListResponseModel<SACodChargesViewModel>();
            try
            {
                var data = await _deliveryChargeDetailsService.GetSACodChargeList();
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
        /// load all orders.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "status": -1,
        ///        "statusList" : [-1],
        ///        "statusGroup" : ["-1"] ,          
        ///        "fromDate" : "1/1/2001" ,          
        ///        "toDate": "1/1/2001",
        ///        "courierUserId" : 1,
        ///        "podNumber" : "",
        ///        "orderIds" : "",
        ///        "collectionName" : "",
        ///        "index" : 0,
        ///        "count" : 20
        ///     }
        ///
        /// </remarks>
        /// <param name="loadCourierOrderBodyModel"></param>

        [HttpPost]
        [ProducesResponseType(500)]
        [Route("LoadCourierOrder")]
        [ProducesResponseType(typeof(CourierOrderDetailsViewModel), 200)]
        public async Task<IActionResult> LoadCourierOrder([FromBody] LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var response = new SingleResponseModel<CourierOrderDetailsViewModel>();
            try
            {
                var data = await _orderService.LoadCourierOrder(loadCourierOrderBodyModel);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders([FromBody] LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var response = new SingleResponseModel<CourierOrderDetailsViewModel>();
            try
            {
                var data = await _orderService.GetAllOrders(loadCourierOrderBodyModel);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("GetCodCollections")]
        public async Task<IActionResult> GetCodCollections([FromBody] LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var response = new SingleResponseModel<CourierOrderDetailsViewModel>();
            try
            {
                var data = await _orderService.GetCodCollections(loadCourierOrderBodyModel);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(500)]
        [Route("LoadOrders")]
        [ProducesResponseType(typeof(CourierOrderDetailsViewModel), 200)]
        public async Task<IActionResult> LoadOrders([FromBody] LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var response = new SingleResponseModel<CourierOrderDetailsViewModel>();
            try
            {
                var data = await _orderService.LoadOrders(loadCourierOrderBodyModel);
                response.Model = data;
                response.Message = "LoadOrders";
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetBreakableCharge")]
        public async Task<IActionResult> GetBreakableCharge()
        {
            var response = new SingleResponseModel<ExtraCharge>();
            try
            {

                var data = await _breakableService.GetBreakableCharge();

                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetReferrer")]
        public async Task<IActionResult> GetReferrer()
        {
            var response = new SingleResponseModel<Referrer>();
            try
            {

                var data = await _loginRepository.GetReferrer();

                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetReferee")]
        public async Task<IActionResult> GetReferee()
        {
            var response = new SingleResponseModel<Referee>();
            try
            {

                var data = await _loginRepository.GetReferee();

                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("LoadCourierOrderAmountDetails")]
        public async Task<IActionResult> LoadCourierOrderAmountDetails([FromBody]LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var response = new SingleResponseModel<CourierAmountDetailsResponse>();
            try
            {
                var data = await _orderService.LoadCourierOrderAmountDetails(loadCourierOrderBodyModel);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("LoadCourierOrderAmountDetailsV2")]
        public async Task<IActionResult> LoadCourierOrderAmountDetailsV2([FromBody] LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var response = new SingleResponseModel<CourierAmountDetailsResponse>();
            try
            {
                var data = await _orderService.LoadCourierOrderAmountDetailsV2(loadCourierOrderBodyModel);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetOrderTracking/{flag}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderTracking([FromBody] OrderTrackingBodyModel orderTrackingBodyModel, string flag = "private")
        {
            var response = new ListResponseModel<OrderTrackingStatusViewModel>();
            try
            {
                var data = await _orderTrackingService.GetOrderTracking(orderTrackingBodyModel, flag);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetOrderTrackingNew/{flag}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderTrackingNew([FromBody] OrderTrackingBodyModel orderTrackingBodyModel, string flag = "private")
        {
            var response = new SingleResponseModel<OrderTracking>();
            try
            {
                var data = await _orderTrackingService.GetOrderTrackingNew(orderTrackingBodyModel, flag);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("DeliveryChargeDetailsAreaWise")]
        public async Task<IActionResult> DeliveryChargeDetailsAreaWise([FromBody] DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            var response = new ListResponseModel<AreaWiseChargeDetailsViewModel>();
            try
            {
                var result = await _deliveryChargeDetailsService.DeliveryChargeDetailsAreaWise(deliveryChargeDetailsBodyModel);
                response.Model = result;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("DeliveryChargeMerchantDetailsAreaWise")]
        public async Task<IActionResult> DeliveryChargeMerchantDetailsAreaWise([FromBody] DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            var response = new ListResponseModel<AreaWiseChargeDetailsViewModel>();
            try
            {
                var result = await _deliveryChargeDetailsService.DeliveryChargeMerchantDetailsAreaWise(deliveryChargeDetailsBodyModel);
                response.Model = result;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("DeliveryChargeDetailsAreaWise_test")]
        public async Task<IActionResult> DeliveryChargeDetailsAreaWise_test([FromBody] DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            var response = new ListResponseModel<AreaWiseChargeDetailsViewModel>();
            try
            {
                var result = await _deliveryChargeDetailsService.DeliveryChargeDetailsAreaWise_test(deliveryChargeDetailsBodyModel);
                response.Model = result;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("LoadCourierOrderStatus")]
        public async Task<IActionResult> LoadCourierStatus()
        {
            var response = new ListResponseModel<CourierOrderStatus>();

            try
            {
                response.Model = await _orderTrackingService.LoadCourierStatus();
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();

        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetCourierOrderStatus")]
        public async Task<IActionResult> GetCourierOrderStatus()
        {
            var response = new ListResponseModel<CourierOrderStatusViewModel>();
            try
            {
                var data = await _orderTrackingService.GetCourierOrderStatus();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("DeliveryChargeDetailsSearchWise")]
        public async Task<IActionResult> DeliveryChargeDetailsSearchWise([FromBody]DeliveryChargeDetailsSearchModel deliveryChargeDetailsSearch)
        {
            var response = new ListResponseModel<GetDeliveryChargeDetailsViewModel>();
            try
            {
                var result = await _deliveryChargeDetailsService.DeliveryChargeDetailsSearchWise(deliveryChargeDetailsSearch);
                response.Model = result;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();

        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetStatusGroup")]
        public async Task<IActionResult> GetStatusGroup()
        {
            var response = new ListResponseModel<StatusGroup>();
            try
            {
                var data = await _orderTrackingService.GetStatusGroup();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("OrderUpdateHistory/{CourierOrderId=0}")]
        public async Task<IActionResult> OrderUpdateHistory(string CourierOrderId)
        {
            var response = new ListResponseModel<CourierOrderTrackHistoryViewModel>();
            try
            {
                var data = await _orderTrackingService.OrderUpdateHistory(CourierOrderId);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllCollectors")]
        public async Task<IActionResult> GetAllCollectors()
        {
            var response = new ListResponseModel<Collectors>();
            try
            {
                var data = await _orderTrackingService.GetAllCollectors();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllDeliveryMan")]
        public async Task<IActionResult> GetAllDeliveryMan()
        {
            var response = new ListResponseModel<DeliveryUsers>();
            try
            {
                var data = await _orderTrackingService.GetAllDeliveryMan();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetLocationAssignDeliveryMan")]
        public async Task<IActionResult> GetLocationAssignDeliveryMan()
        {
            var response = new ListResponseModel<DeliveryUsers>();
            try
            {
                var data = await _orderTrackingService.GetLocationAssignDeliveryMan();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("CollectorWiseData")]
        public async Task<IActionResult> CollectorWiseData([FromBody]CollectorOrderBodyModel collectorOrderBodyModel) ///{CollectorId=0}
        {
            var response = new ListResponseModel<CourierOrderViewModel>();
            try
            {
                var data = await _orderService.CollectorWiseData(collectorOrderBodyModel);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "";
            }
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("ListOfCourierUser/{CollectorId=0}/{GroupType=0}")]
        public async Task<IActionResult> ListOfCourierUser(int CollectorId,int GroupType=0)
        {
            var response = new ListResponseModel<CourierUsers>();

            try
            {
                var data = await _orderService.ListOfCourierUser(CollectorId, GroupType);
                response.Model = data;
            }
            catch(Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "";

            }
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllCollectorsLocationAssign")]
        public async Task<IActionResult> GetAllCollectorsLocationAssign()
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _orderTrackingService.GetAllCollectorsLocationAssign();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllLocationAssign")]
        public async Task<IActionResult> GetAllLocationAssign()
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _orderTrackingService.GetAllLocationAssign();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllCollectorsAssign")]
        public async Task<IActionResult> GetAllCollectorsAssign()
        {
            var response = new ListResponseModel<CollectorAssignViewModel>();
            try
            {
                var data = await _orderTrackingService.GetAllCollectorsAssign();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetDeliveryZoneInfo")]
        public async Task<IActionResult> GetDeliveryZoneInfo()
        {
            var response = new ListResponseModel<DeliveryZoneInfo>();
            try
            {
                var data = await _orderTrackingService.GetDeliveryZoneInfo();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetDeliveryZone")]
        public async Task<IActionResult> GetDeliveryZone()
        {
            var response = new ListResponseModel<DeliveryZone>();
            try
            {
                var data = await _orderTrackingService.GetDeliveryZone();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllDeliveryMansAssign")]
        public async Task<IActionResult> GetAllDeliveryMansAssign()
        {
            var response = new ListResponseModel<DeliveryManAssignViewModel>();
            try
            {
                var data = await _orderTrackingService.GetAllDeliveryMansAssign();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetCourierUsersInfo")]
        public async Task<IActionResult> GetCourierUsersInfo([FromBody] SearchBodyModel searchBody)
        {
            var response = new SingleResponseModel<CourierUsersInfoViewModel>();

            try
            {
                var data = await _orderService.GetCourierUsersInfo(searchBody);
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
        [Route("GetCourierUserInfo/{courierUserId}")]
        public async Task<IActionResult> GetCourierUserInfo(int courierUserId)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _orderService.GetCourierUserInfo(courierUserId);
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
        [Route("GetPackagingChargeRange/{onlyActive=true}")]
        public async Task<IActionResult> GetPackagingChargeRange(bool onlyActive=true)
        {
            var response = new ListResponseModel<PackagingChargeRange>();

            try
            {
                var data = await _packagingChargeRangeService.GetPackagingChargeRange(onlyActive);
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
        [Route("GetAdUsers")]
        public async Task<IActionResult> GetAdUsers()
        {
            var response = new ListResponseModel<Users>();

            try
            {
                var data = await _loginService.GetAdUsers();
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
        [Route("GetAdUsersByFilter")]
        public async Task<IActionResult> GetAdUsersByFilter([FromBody] Users user)
        {
            var response = new ListResponseModel<Users>();

            try
            {
                var data = await _loginService.GetAdUsersByFilter(user);
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
        [Route("GetAllReturnProducts")]
        public async Task<IActionResult> GetAllReturnProducts()
        {
            var response = new SingleResponseModel<ReturnProductsViewModel>();

            try
            {
                var data = await _returnService.GetAllReturnProducts();
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
        [Route("GetAllReturnProductsReport/{statusIds}")]
        public async Task<IActionResult> GetAllReturnProductsReport(string statusIds)
        {
            var response = new SingleResponseModel<ReturnProductsViewModel>();

            try
            {
                var data = await _returnService.GetAllReturnProductsReport(statusIds);
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
        [Route("LoadCourierReport")]
        public async Task<IActionResult> LoadCourierReport([FromBody]LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            var response = new SingleResponseModel<CourierOrderDetailsViewModel>();
            try
            {
                var data = await _orderService.LoadCourierReport(loadCourierOrderBodyModel);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("LoadAllDistricts")]
        [AllowAnonymous]
        //[ResponseCache(Duration = 86400)]
        public async Task<IActionResult> LoadAllDistricts()
        {
            var response = new ListResponseModel<Districts>();
            try
            {
                var data = await _orderService.LoadAllDistricts();
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("LoadAllDistrictsById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> LoadAllDistrictsById(int id)
        {
            var response = new ListResponseModel<Districts>();
            try
            {
                var data = await _orderService.LoadAllDistrictsById(id);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllCourierUsersList/{companyName=0}")]
        public async Task<IActionResult> GetAllCourierUsersList(string companyName)
        {
            var response = new ListResponseModel<CourierUsers>();

            try
            {
                var data = await _orderService.GetAllCourierUsersList(companyName);
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
        [Route("GetTeleSaleCourierUsersList/{courierUserId}/{teleSales}")]
        public async Task<IActionResult> GetTeleSaleCourierUsersList(int courierUserId, int teleSales)
        {
            var response = new ListResponseModel<TeleSaleCourierUsers>();

            try
            {
                var data = await _orderService.GetTeleSaleCourierUsersList(courierUserId, teleSales);
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
        [Route("GetAllHubs")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHubs()
        {
            var response = new ListResponseModel<Hub>();

            try
            {
                var data = await _weightRangeService.GetAllHubs();
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
        [Route("GetPickupLocations/{courierUserId}")]
        public async Task<IActionResult> GetPickupLocations(int courierUserId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _weightRangeService.GetPickupLocations(courierUserId);
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
        [Route("GetPickupLocationsWithAcceptedOrderCount/{courierUserId}")]
        public async Task<IActionResult> GetPickupLocationsWithAcceptedOrderCount(int courierUserId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _weightRangeService.GetPickupLocationsWithAcceptedOrderCount(courierUserId);
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
        [Route("GetPriceList/{districtId}/{deliveryRangeId}")]
        public async Task<IActionResult> GetPriceList(int districtId, int deliveryRangeId)
        {
            var response = new ListResponseModel<PriceListViewModel>();

            try
            {
                var data = await _weightRangeService.GetPriceList(districtId, deliveryRangeId);
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
        [Route("GetCourierUserListWithPickupLocations")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourierUserListWithPickupLocations([FromBody] CourierUsersLocationWiseSearchBodyModel merchant)
        {
            var response = new SingleResponseModel<CourierUserPickupLocationModel>();

            try
            {
                var data = await _orderService.GetCourierUserListWithPickupLocations(merchant);
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
        [Route("GetOfferInformation/{orderid}/{offercode}")]
        public async Task<IActionResult> GetOfferInformation(int orderid, string offercode)
        {
            var response = new SingleResponseModel<OfferInformationViewModel>();

            try
            {
                var data = await _weightRangeService.GetOfferInformation(orderid, offercode);
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
        [AllowAnonymous]
        [Route("GetCourierUsersInformation/{courierUserId}")]
        public async Task<IActionResult> GetCourierUsersInformation(int courierUserId)
        {
            var response = new SingleResponseModel<CourierUsersViewModel>();

            try
            {
                response.Model = await _orderTrackingService.GetCourierUsersInformation(courierUserId);
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();

        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetCustomerOrders/{mobileNo}")]
        public async Task<IActionResult> GetCustomerOrders(string mobileNo)
        {
            var response = new SingleResponseModel<CourierOrderDetailsViewModel>();
            try
            {
                var data = await _orderService.GetCustomerOrders(mobileNo);
                response.Model = data;
            }
            catch (Exception exp)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }

            return response.ToHttpResponse();
        }
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAcceptedRiders")]
        public async Task<IActionResult> GetAcceptedRiders([FromBody] PickupLocations pickupLocations)
        {
            var response = new ListResponseModel<DeliveryUserAcceptedViewModel>();

            try
            {
                var data = await _orderService.GetAcceptedRiders(pickupLocations);
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
        [Route("GetRidersOfficeInfo")]
        public async Task<IActionResult> GetRidersOfficeInfo([FromBody] RequestBodyModel requestBodyModel)
        {
            var response = new SingleResponseModel<DeliveryUsersViewModel>();

            try
            {
                var data = await _orderService.GetRidersOfficeInfo(requestBodyModel);
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
        [Route("GetHubsByPickupLocation")]
        public async Task<IActionResult> GetHubsByPickupLocation([FromBody] PickupLocations pickupLocation)
        {
            var response = new SingleResponseModel<Hub>();

            try
            {
                var data = await _weightRangeService.GetHubsByPickupLocation(pickupLocation);
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
        [Route("GetBalanceLoadLimit/{merchantId}")]
        public async Task<IActionResult> GetBalanceLoadLimit(int merchantId)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = new
                {
                    MinAmount = 10,
                    MaxAmount = 100000
                };

                response.Model = await Task.FromResult(data);
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
        [Route("GetDTOrderGenericLimit")]
        public async Task<IActionResult> GetDTOrderGenericLimit()
        {
            var response = new SingleResponseModel<DTOrderGenericLimits>();

            try
            {
                var dtOrderGenericData = new DTOrderGenericLimits()
                {
                    CollectionAmount = 10000.00,
                    ActualPackagePrice = 10000.00
                };

                response.Model = await Task.FromResult(dtOrderGenericData);
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
        [Route("GetReturnOrders/{merchantId}/{index}/{count}")]
        public async Task<IActionResult> GetReturnOrders(int merchantId, int index, int count)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _weightRangeService.GetReturnOrders(merchantId, index, count);
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
        [Route("GetDuplicatesCourierUsersInfo")]
        public async Task<IActionResult> GetDuplicatesCourierUsersInfo()
        {
            var response = new ListResponseModel<CourierUsersViewModel>();

            try
            {
                var data = await _orderService.GetDuplicatesCourierUsersInfo();
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
        [Route("GetSurveyQuestion")]
        public async Task<IActionResult> GetSurveyQuestion()
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _weightRangeService.GetSurveyQuestion();
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
        [Route("DanaMatchColumn/{CourierUserId}")]
        public async Task<IActionResult> DanaMatchColumn(string CourierUserId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _weightRangeService.DanaMatchColumn(CourierUserId);
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
        [Route("GetAssignCourierUserCategory")]
        public async Task<IActionResult> GetAssignCourierUserCategory([FromBody] AssignCourierUserCategory assignCourierUserCategory)
        {
            var response = new ListResponseModel<Category>();

            try
            {
                var data = await _weightRangeService.GetAssignCourierUserCategory(assignCourierUserCategory);
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
        [Route("GetDtCategories/{isActive}")]
        public async Task<IActionResult> GetDtCategories(bool isActive)
        {
            var response = new ListResponseModel<Category>();

            try
            {
                var data = await _weightRangeService.GetDtCategories(isActive);
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
        [Route("GetCategoriesForAdmin/{isActive}")]
        public async Task<IActionResult> GetCategoriesForAdmin(bool isActive)
        {
            var response = new ListResponseModel<Category>();

            try
            {
                var data = await _weightRangeService.GetCategoriesForAdmin(isActive);
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
        [Route("GetSubCategories/{isActive}")]
        public async Task<IActionResult> GetSubCategories(bool isActive)
        {
            var response = new ListResponseModel<SubCategory>();

            try
            {
                var data = await _weightRangeService.GetSubCategories(isActive);
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
        [Route("GetSubCategoryById/{isActive}/{categoryId}")]
        public async Task<IActionResult> GetSubCategoryById(bool isActive, int categoryId)
        {
            var response = new ListResponseModel<SubCategory>();

            try
            {
                var data = await _weightRangeService.GetSubCategoryById(isActive, categoryId);
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
        [Route("GetAssignCouirerAndService/{districtId}/{thanaId}")]
        public async Task<IActionResult> GetAssignCouirerAndService(int districtId, int thanaId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderService.GetAssignCouirerAndService(districtId, thanaId);
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
        [Route("GetMerchantAssignCouirerAndService/{districtId}/{thanaId}/{courierUserId}")]
        public async Task<IActionResult> GetMerchantAssignCouirerAndService(int districtId, int thanaId, int courierUserId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderService.GetMerchantAssignCouirerAndService(districtId, thanaId, courierUserId);
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
        [Route("GetDeliveredReturnedCount")]
        public async Task<IActionResult> GetDeliveredReturnedCount([FromBody] LoadCourierOrderBodyModel bodyModel)
        {
            var response = new ListResponseModel<DeliveredReturnedCountModel>();

            try
            {
                var data = await _weightRangeService.GetDeliveredReturnedCount(bodyModel);
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
        [Route("GetDeliveredReturnedCountWiseDetails")]
        public async Task<IActionResult> GetDeliveredReturnedCountWiseDetails([FromBody] RequestBodyModel bodyModel)
        {
            var response = new ListResponseModel<DeliveredReturnedDetailsViewModel>();

            try
            {
                var data = await _weightRangeService.GetDeliveredReturnedCountWiseDetails(bodyModel);
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
        [Route("BondhuAppMismatchData")]
        public async Task<IActionResult> BondhuAppMismatchData([FromBody] RequestBodyModel bodyModel)
        {
            var response = new ListResponseModel<BondhuAppMismatchDataViewModel>();

            try
            {
                var data = await _weightRangeService.BondhuAppMismatchData(bodyModel);
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
        [Route("GetServiceDistricts/{deliveryRangeId}")]
        public async Task<IActionResult> GetServiceDistricts(int deliveryRangeId)
        {
            var response = new ListResponseModel<Districts>();

            try
            {
                var data = await _weightRangeService.GetServiceDistricts(deliveryRangeId);
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
        [Route("GetServiceDistricts")]
        [ResponseCache(Duration = 86400)]
        public async Task<IActionResult> GetServiceDistricts([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<Districts>();

            try
            {
                var data = await _weightRangeService.GetServiceDistricts(request.DeliveryRangeIds);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical suppor.";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetDetailedSAReport")]
        public async Task<IActionResult> GetDetailedSAReport ([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.GetDetailedSAReport(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }

            return response.ToHttpResponse();
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetAcceptedCourierOrders/{courierUserId}")]
        public async Task<IActionResult> GetAcceptedCourierOrders(int courierUserId)
        {
            var response = new SingleResponseModel<CourierOrdersViewModel>();
            try
            {
                var data = await _weightRangeService.GetAcceptedCourierOrders(courierUserId);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact with technical support";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetSABookingReport")]
        public async Task<IActionResult> GetSABookingReport([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.GetSABookingReport(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical suppor";
            }

            return response.ToHttpResponse();
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetRiderWiseCollectionReport")]
        public async Task<IActionResult> GetRiderWiseCollectionReport([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.GetRiderWiseCollectionReport(request);
                response.Model = data;
            }
            catch(Exception ex)
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
        [Route("GetCollectionSlotWiseOrders")]
        public async Task<IActionResult> GetCollectionSlotWiseOrders([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<CourierOrdersViewModel>();
            try
            {
                var data = await _weightRangeService.GetCollectionSlotWiseOrders(request);
                response.Model = data;
            }
            catch(Exception ex)
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
        [Route("GetPackagedWiseOrders")]
        public async Task<IActionResult> GetPackagedWiseOrders([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.GetPackagedWiseOrders(request);
                response.Model = data;
            }
            catch(Exception ex)
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
        [Route("GetCutomerListForApp")]
        public async Task<IActionResult> GetCutomerListForApp([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _districtInfoService.GetCutomerListForApp(request);
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
        [Route("GetCutomerWiseOrdersDetailsForApp")]
        public async Task<IActionResult> GetCutomerWiseOrdersDetailsForApp([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _districtInfoService.GetCutomerWiseOrdersDetailsForApp(request);
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
        [Route("LoadAllDistrictsByIds")]
        public async Task<IActionResult> LoadAllDistrictsByIds([FromBody] List<Districts> request)
        {
            var response = new ListResponseModel<Districts>();
            try
            {
                var data = await _orderService.LoadAllDistrictsByIds(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("GetQuickOfficeReceivedDetails/{courierOrdersId}/{userId}/{hubName}")]
        public async Task<IActionResult> GetQuickOfficeReceivedDetails(string courierOrdersId, int userId, string hubName)
        {
            var response = new ListResponseModel<OrderStatusHistoryViewModel>();

            try
            {
                var data = await _orderService.GetQuickOfficeReceivedDetails(courierOrdersId,userId,hubName);
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
        [AllowAnonymous]
        [Route("GetQuickUpdateStatusDetails")]
        public async Task<IActionResult> GetQuickUpdateStatusDetails([FromBody] RequestBodyModel bodyModel)
        {
            var response = new ListResponseModel<CourierOrderViewModel>();

            try
            {
                var data = await _orderService.GetQuickUpdateStatusDetails(bodyModel);
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
        [Route("GetMyPhoneBookGroup/{courierUserId}")]
        public async Task<IActionResult> GetMyPhoneBookGroup(int courierUserId)
        {
            var response = new ListResponseModel<PhoneBookGroup>();

            try
            {
                var data = await _weightRangeService.GetMyPhoneBookGroup(courierUserId);
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
        [Route("GetDistrictwiseCourierUserInfo/{isInsideDhaka}/{companyName}")]
        public async Task<IActionResult> GetDistrictwiseCourierUserInfo(bool isInsideDhaka, string companyName)
        {
            var response = new ListResponseModel<CourierUsersViewModel>();
            try
            {
                var data = await _weightRangeService.GetDistrictwiseCourierUserInfo(isInsideDhaka, companyName);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("GetLocationWiseRiders")]
        public async Task<IActionResult> GetLocationWiseRiders([FromBody] List<LocationAssign> locationAssigns)
        {
            var response = new ListResponseModel<DeliveryUsersViewModel>();

            try
            {
                var data = await _orderService.GetLocationWiseRiders(locationAssigns);
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
        [Route("GetUser/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var response = new SingleResponseModel<AdminUsersViewModel>();

            try
            {
                var data = await _weightRangeService.GetUser(userId);
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
        [AllowAnonymous]
        [Route("GetFirstTimeOrderedMerchantList")]
        public async Task<IActionResult> GetFirstTimeOrderedMerchantList([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<CourierOrdersViewModel>();

            try
            {
                var data = await _orderService.GetFirstTimeOrderedMerchantList(request);
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
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("GetMerchantAssignedVoucher")]
        public async Task<IActionResult> GetMerchantAssignedVoucher([FromBody] List<VouchersViewModel> vouchers)
        {
            var response = new ListResponseModel<VouchersViewModel>();

            try
            {
                var data = await _orderService.GetMerchantAssignedVoucher(vouchers);
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
        [Route("GetDTDeliveryChargeInfo")]
        public async Task<IActionResult> GetDTDeliveryChargeInfo([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<DeliveryRange>();
            try
            {
                var data = await _weightRangeService.GetDTDeliveryChargeInfo(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetPurchasedSMSInfo/{courierUserId}")]
        public async Task<IActionResult> GetPurchasedSMSInfo(int courierUserId)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.GetPurchasedSMSInfo(courierUserId);
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
        [AllowAnonymous]
        [Route("GetOrderDetails/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _orderService.GetOrderDetails(orderId);
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
        /// GetDatewiseVoucherInfo
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// /// <response code="400">If There was a Bad Request</response>
        /// <response code="500">If there was an internal server error</response>
        /// 
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetDatewiseVoucherInfo")]
        public async Task<IActionResult> GetDatewiseVoucherInfo([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.GetDatewiseVoucherInfo(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact AjkerDeal IT Support";
            }

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("GetSpecialService")]
        public async Task<IActionResult> GetSpecialService([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<WeightRangeWiseData>();

            try
            {
                var data = await _orderService.GetSpecialService(request);
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
        /// GetAllCouriers
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// /// <response code="400">If There was a Bad Request</response>
        /// <response code="500">If there was an internal server error</response>
        /// 

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [Route("GetAllCouriers")]
        public async Task<IActionResult> GetAllCouriers()
        {
            var response = new ListResponseModel<CouriersViewModel>();

            try
            {
                var data = await _orderService.GetAllCouriers();
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
        [Route("GetLenderUsers")]
        public async Task<IActionResult> GetLenderUsers()
        {
            var response = new ListResponseModel<LenderUser>();

            try
            {
                var data = await _weightRangeService.GetLenderUsers();
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
        [Route("GetLenderWiseAssignedCourierUsers/{lenderUserId}")]
        public async Task<IActionResult> GetLenderWiseAssignedCourierUsers(int lenderUserId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _weightRangeService.GetLenderWiseAssignedCourierUsers(lenderUserId);
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
        /// SameDayCollectedPendingOrdersCount
        /// </summary>
        /// <returns>A response with  list</returns>
        /// <response code="200">Returns true</response>
        /// /// <response code="400">If There was a Bad Request</response>
        /// <response code="500">If there was an internal server error</response>
        /// 

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("SameDayCollectedPendingOrdersCount")]
        public async Task<IActionResult> SameDayCollectedPendingOrdersCount([FromBody] RequestBodyModel requestBody)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _orderService.SameDayCollectedPendingOrdersCount(requestBody);
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
        [Route("LoadPOHOrders")]
        public async Task<IActionResult> LoadPOHOrders([FromBody] LoadCourierOrderBodyModel request)
        {
            var response = new SingleResponseModel<CourierOrderDetailsViewModel>();
            try
            {
                var data = await _orderService.LoadPOHOrders(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal server error, please contact with technical support";
            }

            return response.ToHttpResponse();
        }

        /// <summary>
        /// Payment Reference Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetPaymentReferenceReport")]
        public async Task<IActionResult> GetPaymentReferenceReport([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.GetPaymentReferenceReport(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with technical support";
            }

            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetUserLocationAssign")]
        public async Task<IActionResult> GetUserLocationAssign()
        {
            var response = new ListResponseModel<UserLocationAssignViewModel>();

            try
            {
                var data = await _weightRangeService.GetUserLocationAssign();
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with technical support";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetPohApplicable/{mobile}/{courierUserId}")]
        public async Task<IActionResult> GetPohApplicable(string mobile, int courierUserId)
        {
            var response = new SingleResponseModel<PohViewModel>();

            try
            {
                var data = await _weightRangeRepository.GetPohApplicable(mobile, courierUserId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with technical support";
            }
            return response.ToHttpResponse();
        }

        /// <summary>
        /// GetAllCustomComment
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns> list of data</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetAllCustomComment/{orderId}")]
        public async Task<IActionResult> GetAllCustomComment(int orderId)
        {
            var response = new ListResponseModel<dynamic>();

            try
            {
                var data = await _districtInfoService.GetAllCustomComment(orderId);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with technical support";
            }
            return response.ToHttpResponse();
        }

        /// <summary>
        /// GetPohOrderStatuswise api for admin panel
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetPohOrderStatuswise")]
        public async Task<IActionResult> GetPohOrderStatuswise([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.GetPohOrderStatuswise(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact with technical support";
            }

            return response.ToHttpResponse();
        }


        /// <summary>
        /// GetPohOrderwiseReport api for admint panel
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetPohOrderwiseReport")]
        public async Task<IActionResult> GetPohOrderwiseReport([FromBody] RequestBodyModel request)
        {
            var response = new ListResponseModel<dynamic>();
            try
            {
                var data = await _weightRangeService.GetPohOrderwiseReport(request);
                response.Model = data;
            }
            catch(Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, Please contact with technical support";
            }

            return response.ToHttpResponse();
        }
    }
}