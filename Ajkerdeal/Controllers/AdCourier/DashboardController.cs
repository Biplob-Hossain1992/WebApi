using AdCourier.Domain.Entities.BodyModel.Dashboard;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.Banner;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.HelpLine;
using AdCourier.Domain.Entities.ViewModel.Other;
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
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/Dashboard")]
    //[Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IBreakableService _breakableService;
        public DashboardController(IDashboardService dashboardService , IBreakableService breakableService)
        {
            _dashboardService = dashboardService;
            _breakableService = breakableService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [MapToApiVersion("1.0")]
        [Route("GetOrderCountByStatusGroup")]
        public async Task<IActionResult> GetOrderCountByStatusGroup([FromBody] OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            var response = new ListResponseModel<StatusGroupViewModel>();

            try
            {
                var data = await _dashboardService.GetOrderCountByStatusGroup(orderCountByStatusGroupBodyModel);
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
        [MapToApiVersion("1.0")]
        [Route("GetDashBoardOrder")]
        public async Task<IActionResult> GetDashBoardOrder([FromBody] OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            var response = new SingleResponseModel<DashBoardOrderViewModel>();

            try
            {
                var data = await _dashboardService.GetDashBoardOrder(orderCountByStatusGroupBodyModel);
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
        [MapToApiVersion("1.0")]
        [Route("GetOrderCountByStatusGroupV3")]
        public async Task<IActionResult> GetOrderCountByStatusGroupV3([FromBody] OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            var response = new ListResponseModel<StatusGroupViewModel>();

            try
            {
                var data = await _dashboardService.GetOrderCountByStatusGroupV3(orderCountByStatusGroupBodyModel);
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
        [MapToApiVersion("1.1")]
        [Route("GetOrderCountByStatusGroup")]
        public async Task<IActionResult> GetOrderCountByStatusGroupNew([FromBody] OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            var response = new SingleResponseModel<DashboardViewModel>();

            try
            {
                var data = await _dashboardService.GetOrderCountByStatusGroupNew(orderCountByStatusGroupBodyModel);
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
        [MapToApiVersion("1.0")]
        [Route("GetOrderCountByStatusGroupv2")]
        public async Task<IActionResult> GetOrderCountByStatusGroupNewv2([FromBody] OrderCountByStatusGroupBodyModel orderCountByStatusGroupBodyModel)
        {
            var response = new SingleResponseModel<DashboardViewModel>();

            try
            {
                var data = await _dashboardService.GetOrderCountByStatusGroupNew(orderCountByStatusGroupBodyModel);
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
        [MapToApiVersion("1.0")]
        [Route("GetCollection/{courierUserId}")]
        public async Task<IActionResult> GetCollection(int courierUserId)
        {
            var response = new SingleResponseModel<StatusGroupViewModel>();

            try
            {
                var data = await _dashboardService.GetCollection(courierUserId);
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
        [MapToApiVersion("1.0")]
        [Route("GetCollectionHistory/{courierUserId}")]
        public async Task<IActionResult> GetCollectionHistory(int courierUserId)
        {
            var response = new ListResponseModel<CourierOrderStatusHistory>();

            try
            {
                var data = await _dashboardService.GetCollectionHistory(courierUserId);
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
        [MapToApiVersion("1.0")]
        [Route("GetMerchantBalanceInfo/{courierUserId}/{totalAmount}")]
        public async Task<IActionResult> GetMerchantBalanceInfo(int courierUserId, int totalAmount)
        {
            var response = new SingleResponseModel<MerchantAdvanceBalanceInfo>();
            try
            {
                var data = await _dashboardService.GetMerchantBalanceInfo(courierUserId,totalAmount);
                if (data!=null)
                {
                    response.Model = data;
                }
                else
                {
                    response.Model = new MerchantAdvanceBalanceInfo();
                }
                
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
        [Route("UpdateSummary/{id}")]
        public async Task<IActionResult> UpdateSummary(int id, [FromBody] StatusGroupViewModel statusGroup)
        {
            var response = new SingleResponseModel<StatusGroup>();

            try
            {
                var data = await _dashboardService.UpdateSummary(id, statusGroup);
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
        [Route("GetCustomerInfoByMobile/{mobile}")]
        public async Task<IActionResult> GetCustomerInfoByMobile(string mobile)
        {
            var response = new SingleResponseModel<dynamic>();

            try
            {
                var data = await _dashboardService.GetCustomerInfoByMobile(mobile);
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
        [Route("GetBanner")]
        public IActionResult GetBanner()
        {
            var response = new SingleResponseModel<BannerInfo>();

            try
            {
                var data = new BannerInfo() {
                    ShowBanner = true,
                    ShowPopupBanner = false,
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/same_day_delivery.jpg",
                    PopupBannerUrl = "https://static.ajkerdeal.com/images/merchant/same_day_delivery.jpg",
                    PopupFrequency = 0
                };

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
        [Route("GetAllBanners")]
        public IActionResult GetAllBanners()
        {
            var response = new SingleResponseModel<BannerInfoViewModel>();
            try
            {
                var ListOfBanners = new List<Banner>();
                var banner1 = new Banner
                {
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/instant_payment.jpg",
                    IsActive = true,
                    WebUrl = "instant_payment",
                    IsWebLinkActive = false
                };
                ListOfBanners.Add(banner1);
                var banner2 = new Banner
                {
                    //BannerUrl = "https://static.ajkerdeal.com/images/merchant/dt_060920.jpg",
                    //BannerUrl = "https://static.ajkerdeal.com/images/merchant/sadar-express.jpg",
                    //BannerUrl = "https://static.ajkerdeal.com/images/merchant/Sodor_20.jpg",
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/48-hours.jpg",
                    IsActive = false,
                    WebUrl = "https://deliverytiger.com.bd/how-to-active-instant-payment",
                    IsWebLinkActive = false
                };
                ListOfBanners.Add(banner2);
                var banner3 = new Banner
                {
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/livesale.jpg",
                    IsActive = false,
                    WebUrl = "live",
                    IsWebLinkActive = false
                };
                ListOfBanners.Add(banner3);
                var banner4 = new Banner
                {
                    //BannerUrl = "http://static.ajkerdeal.com/images/merchant/dt_070421.jpg",
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/25-60-taka.jpg",
                    IsActive = false,
                    WebUrl = "",
                    IsWebLinkActive = false
                };
                ListOfBanners.Add(banner4);
                var banner5 = new Banner
                {
                    //BannerUrl = "http://static.ajkerdeal.com/images/merchant/sara_desh.jpg",
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/next-day.jpg",
                    IsActive = false,
                    WebUrl = "",
                    IsWebLinkActive = false
                };
                ListOfBanners.Add(banner5);
                var banner6 = new Banner
                {
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/loan_sliding_banner.jpg",
                    IsActive = false,
                    WebUrl = "survey",
                    IsWebLinkActive = false
                };
                ListOfBanners.Add(banner6);
                var banner7 = new Banner
                {
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/chumbok.jpg",
                    IsActive = false,
                    WebUrl = "chumbok",
                    IsWebLinkActive = false
                };
                ListOfBanners.Add(banner7);
                var banner8 = new Banner
                {
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/seven_day_payment.jpg",
                    IsActive = true,
                    WebUrl = "seven_day_payment",
                    IsWebLinkActive = false
                };
                ListOfBanners.Add(banner8);
                var banner9 = new Banner
                {
                    BannerUrl = "https://static.ajkerdeal.com/images/merchant/insta_Cod.jpg",
                    IsActive = true,
                    WebUrl = "instacod",
                    IsWebLinkActive = true
                };
                ListOfBanners.Add(banner9);


                var bannerInfoData = new BannerInfoViewModel()
                {
                    //ReferBanner = "https://static.ajkerdeal.com/images/dt/banner_referral.png",
                    ReferBanner = "",
                    LoanSurveyBanner = "https://static.ajkerdeal.com/images/merchant/loan_banner.jpg",
                    PopUp = new PopUp {
                        //PopUpUrl = "https://static.ajkerdeal.com/images/merchant/same_day_delivery.jpg",
                        //PopUpUrl = "https://static.ajkerdeal.com/images/merchant/notice_lockdown.png",
                        //PopUpUrl = "https://static.ajkerdeal.com/images/merchant/notification-three-day-off-payment.jpg",
                        PopUpUrl = "https://static.ajkerdeal.com/images/merchant/notification-technical-issue-payment.jpg",
                        ShowPopUp = false,
                        PopUpFrequency = 0,
                        Route = ""
                        
                    },
                    BannerList = new BannerList {
                        ShowBanner = true,
                        Banners = ListOfBanners
                    },
                    DashboardDataDuration = 11,
                    ShowOrderPopup = true,
                    InstantPaymentHourLimit = 24,
                    InstantPaymentOTPLimit = 70000,
                    InstantPaymentHourLimitRange = "24-48",
                    CurrentAppVersionCode = 30,
                    IsReferActive = false,
                    IsLoanSurveyActive = true

                };

                response.Model = bannerInfoData;

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
        [Route("GetDeliveryChargeCalInfo")]
        public async Task<IActionResult> GetDeliveryChargeCalInfo()
        {
            var response = new SingleResponseModel<DeliveryChargeCalculationViewModel>();
            try
            {
                var InSideDhakaDeliveryRanges = new List<DeliveryRangeModel>();
                var InDhakaRegular = new DeliveryRangeModel
                {
                    Name = "Regular Delivery",
                    DaliveryRangeId = 16,
                    OnImage = "https://static.ajkerdeal.com/images/dt/newlogin/dhaka-regular-48-hours---25-01.png",
                    OffImage = "https://static.ajkerdeal.com/images/dt/newlogin/dhaka-regular-48-hours---25-02.png"
                };
                InSideDhakaDeliveryRanges.Add(InDhakaRegular);
                //var InDhakaSameDay = new DeliveryRangeModel
                //{
                //    Name = "Same Day Delivery",
                //    DaliveryRangeId = 5,
                //    OnImage = "https://static.ajkerdeal.com/images/dt/newlogin/sameday-01.png",
                //    OffImage = "https://static.ajkerdeal.com/images/dt/newlogin/sameday-02.png"
                //};
                //InSideDhakaDeliveryRanges.Add(InDhakaSameDay);
                var InDhakaNextDay = new DeliveryRangeModel
                {
                    Name = "Next Day Delivery",
                    DaliveryRangeId = 14,
                    OnImage = "https://static.ajkerdeal.com/images/dt/newlogin/nextday-01.png",
                    OffImage = "https://static.ajkerdeal.com/images/dt/newlogin/nextday-02.png"
                };
                InSideDhakaDeliveryRanges.Add(InDhakaNextDay);

                var OutSideDhakaDeliveryRanges = new List<DeliveryRangeModel>();
                var OutDhakaRegular = new DeliveryRangeModel
                {
                    Name = "Regular Delivery",
                    DaliveryRangeId = 21,
                    OnImage = "https://static.ajkerdeal.com/images/dt/newlogin/sador-regular-3-days---50-01.png",
                    OffImage = "https://static.ajkerdeal.com/images/dt/newlogin/sador-regular-3-days---50-02.png"
                };
                OutSideDhakaDeliveryRanges.Add(OutDhakaRegular);
                var OutDhakaSameDay = new DeliveryRangeModel
                {
                    Name = "Express Delivery",
                    DaliveryRangeId = 18,
                    OnImage = "https://static.ajkerdeal.com/images/dt/newlogin/city-express-48-hours---75-01.png",
                    OffImage = "https://static.ajkerdeal.com/images/dt/newlogin/city-express-48-hours---75-02.png"
                };
                OutSideDhakaDeliveryRanges.Add(OutDhakaSameDay);
                //var OutDhakaNextDay = new DeliveryRangeModel
                //{
                //    Name = "Postal Delivery",
                //    DaliveryRangeId = 8,
                //    OnImage = "https://static.ajkerdeal.com/images/dt/newlogin/postal-01.png",
                //    OffImage = "https://static.ajkerdeal.com/images/dt/newlogin/postal-02.png"
                //};
                //OutSideDhakaDeliveryRanges.Add(OutDhakaNextDay);


                var deliveryChargeCalInfoData = new DeliveryChargeCalculationViewModel()
                {
                    CODCharge = _breakableService.GetBreakableCharge().Result.CodChargeDhakaPercentage.ToString("0.0"), //"1.0",
                    InSideDhaka = new InSideOutSideDhakaModel
                    {
                        Text = "ঢাকা সিটিতে",
                        DistrictId = 14,
                        DeliveryRange = InSideDhakaDeliveryRanges,
                        Charge = "৳ ৫",
                        ChargePercentage = "১.০%"
                    },
                    OutSideDhaka = new InSideOutSideDhakaModel
                    {
                        Text = "ঢাকা সিটির বাইরে",
                        DistrictId = 1,
                        DeliveryRange = OutSideDhakaDeliveryRanges,
                        Charge = "৳ ১৫",
                        ChargePercentage = "১.০%"
                    }
                };
                response.Model = await Task.FromResult(deliveryChargeCalInfoData);

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
        [Route("GetHelpLineNumbers")]
        public IActionResult GetHelpLineNumbers()
        {
            var response = new SingleResponseModel<HelpLineNumbers>();

            try
            {
                var data = new HelpLineNumbers()
                {
                    HelpLine1 = "01894811444",
                    HelpLine2 = "01894811222"
                };

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
        [Route("GetCouriers")]
        public async Task<IActionResult> GetCouriers()
        {
            var response = new ListResponseModel<CouriersViewModel>();

            try
            {
                var listOfCouriers = new List<CouriersViewModel>();
                
                var SA = new CouriersViewModel()
                {
                    CourierId = 4,
                    CourierName = "SA"
                };
                listOfCouriers.Add(SA);
                var Sundarban = new CouriersViewModel()
                {
                    CourierId = 5,
                    CourierName = "Sundarban"
                };
                listOfCouriers.Add(Sundarban);
                var Redx = new CouriersViewModel()
                {
                    CourierId = 34,
                    CourierName = "Redx"
                };
                listOfCouriers.Add(Redx);
                var Pathao = new CouriersViewModel()
                {
                    CourierId = 22,
                    CourierName = "Pathao"
                };
                listOfCouriers.Add(Pathao);
                var ECourier = new CouriersViewModel()
                {
                    CourierId = 3,
                    CourierName = "E Courier"
                };
                listOfCouriers.Add(ECourier);
                var Paperfly = new CouriersViewModel()
                {
                    CourierId = 30,
                    CourierName = "Paperfly"
                };
                listOfCouriers.Add(Paperfly);
                
                var Steadfast = new CouriersViewModel()
                {
                    CourierId = 49,
                    CourierName = "Steadfast"
                };
                listOfCouriers.Add(Steadfast);
                var Others = new CouriersViewModel()
                {
                    CourierId = 68,
                    CourierName = "Others"
                };
                listOfCouriers.Add(Others);

                response.Model = listOfCouriers;

            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Retrieves sms count
        /// </summary>
        /// <returns>A response with sms count list</returns>
        /// <response code="200">Returns the sms count list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetListOfSMSCount")]
        public async Task<IActionResult> GetListOfSMSCount()
        {
            var response = new SingleResponseModel<int[]>();

            try
            {
                int[] smsArray = new int[5] { 50, 100, 200, 300, 500 };

                response.Model = await Task.FromResult(smsArray);

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
