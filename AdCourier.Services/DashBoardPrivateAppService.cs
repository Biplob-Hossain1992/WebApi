using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AdCourier.Domain.Entities.DataModel;
using System.Linq;

namespace AdCourier.Services
{
    public class DashBoardPrivateAppService : IDashBoardPrivateAppService
    {

        private readonly IDashBoardPrivateAppRepository _dashBoardPrivateAppRepository;
        
        public DashBoardPrivateAppService(IDashBoardPrivateAppRepository dashBoardPrivateAppRepository)
        {
            _dashBoardPrivateAppRepository = dashBoardPrivateAppRepository;
        }

        public async Task<dynamic> GetThirdPartyPaymentInfo(RequestBodyModel request)
        {
            return await _dashBoardPrivateAppRepository.GetThirdPartyPaymentInfo(request);
        }

        public async Task<dynamic> GetStatusWiseOrderInfo()
        {
            return await _dashBoardPrivateAppRepository.GetStatusWiseOrderInfo();
        }

        public async Task<dynamic> GetDeliveryTigerPaymentInfo(RequestBodyModel request)
        {
            return await _dashBoardPrivateAppRepository.GetDeliveryTigerPaymentInfo(request);
        }

        public async Task<dynamic> GetDtOrders(OrderBodyModel orderBodyModel)
        {
            //get orders data inside date range
            var data = await _dashBoardPrivateAppRepository.GetDtOrders(orderBodyModel, "date");

            int[] notStatus = new int[] { 0, 2, 29 };
            int[] nextDayService = new int[] { 7, 14, 17 };
            int[] nextDay = new int[] { 7, 17 };
            int[] sadorService = new int[] { 18, 19 };
            int[] sador = new int[] { 18 };
            int[] nextDaySador = new int[] { 7, 17, 18 };
            int[] deliveredOrderStatus = new int[] { 15, 24, 25, 28 };

            var cod = new string[] { "Delivery Taka Collection" };
            var paid = new string[] { "Only Delivery" };

            //get total orders data
            var queryTotal = new
            {
                Name = "Total",
                Orders = new
                {
                    Placed = new
                    {
                        Order = data.Select(p => p.Id).Count(),
                        InsideOrder = data.Where(x => x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
                        OutsideOrder = data.Where(x => !x.DistrictId.Equals(14)).Select(p => p.Id).Count()
                    },
                    Confirmed = new
                    {
                        Order =
                        //inside
                        data.Where(o => o.DistrictId.Equals(14) && !notStatus.Contains(o.Status)
                        && !sadorService.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count() +
                        //outside
                        data.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && nextDay.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count() +
                        data.Where(o => !notStatus.Contains(o.Status) && sador.Contains(o.DeliveryRangeId))
                        .Select(s => s.Id).Count() +
                        data.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && !nextDaySador.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),

                        InsideOrder = data.Where(o => o.DistrictId.Equals(14) && !notStatus.Contains(o.Status)
                        && !sadorService.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                        
                        OutsideOrderWithSador = 
                        data.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && nextDay.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count() +
                        data.Where(o => !notStatus.Contains(o.Status) && sador.Contains(o.DeliveryRangeId))
                        .Select(s => s.Id).Count() +
                        data.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && !nextDaySador.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                        
                        ServiceType = new
                        {
                            InsideOrderNextDay = data.Where(o => !notStatus.Contains(o.Status) && o.DistrictId.Equals(14)
                            && o.DeliveryRangeId.Equals(14)).Select(s => s.Id).Count(),
                            InsideExceptNextDayOrder = data.Where(o => !notStatus.Contains(o.Status) && o.DistrictId.Equals(14)
                            && !nextDayService.Contains(o.DeliveryRangeId) && !sadorService.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                            OutsideOrderNextDay = data.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                            && nextDay.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                            OutsideOrderSador = data.Where(o => !notStatus.Contains(o.Status) && sador.Contains(o.DeliveryRangeId))
                            .Select(s => s.Id).Count(),
                            OutsideExceptNextDaySador = data.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                            && !nextDaySador.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count()
                        }
                    }
                },
                Merchants = new
                {
                    Placed = new
                    {
                        Merchant = data.Select(p => p.MerchantId).Distinct().Count(),
                        InsideMerchant = data.Where(x => x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count(),
                        OutsideMerchant = data.Where(x => !x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count()
                    },
                    Confirmed = new
                    {
                        Merchant = data.Where(x => !notStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                        InsideMerchant = data.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count(),
                        OutsideMerchant = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count(),
                        InsideMerchantNextDay = data.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                        && x.DeliveryRangeId.Equals(14)).Select(x => x.MerchantId).Distinct().Count(),
                        InsideMerchantExceptNextDay = data.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                        && !nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                        OutsideMerchantNextDay = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                        && nextDay.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                        OutsideMerchantSador = data.Where(x => !notStatus.Contains(x.Status) && sador.Contains(x.DeliveryRangeId))
                        .Select(x => x.MerchantId).Distinct().Count(),
                        OutsideMerchantExceptNextDaySador = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                        && !nextDaySador.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count()
                    }
                }
            };

            //get date range orders data
            var queryDate = (from c in data
                             group c by new
                             {
                                 Date = c.OrderDate.Date,
                                 Month = c.OrderDate.Month,
                                 Year = c.OrderDate.Year
                             } into grderDateGroup
                               select new
            {
                Name = grderDateGroup.Key.Date.ToShortDateString(),
                Orders = new
                {
                    Placed = new
                    {
                        Order = grderDateGroup.Select(p => p.Id).Count(),
                        InsideOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
                        OutsideOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14)).Select(p => p.Id).Count()
                    },
                    Confirmed = new
                    {
                        Order = 
                        //Inside
                        grderDateGroup.Where(o => o.DistrictId.Equals(14) && !notStatus.Contains(o.Status)
                        && !sadorService.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count() +
                        //Outside
                        grderDateGroup.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && nextDay.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count() +
                        grderDateGroup.Where(o => !notStatus.Contains(o.Status) && sador.Contains(o.DeliveryRangeId))
                        .Select(s => s.Id).Count() +
                        grderDateGroup.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && !nextDaySador.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),

                        InsideOrder = grderDateGroup.Where(o => o.DistrictId.Equals(14) && !notStatus.Contains(o.Status)
                        && !sadorService.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                        
                        OutsideOrderWithSador =
                        grderDateGroup.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && nextDay.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count() +
                        grderDateGroup.Where(o => !notStatus.Contains(o.Status) && sador.Contains(o.DeliveryRangeId))
                        .Select(s => s.Id).Count() +
                        grderDateGroup.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && !nextDaySador.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                        
                        ServiceType = new
                        {
                            InsideOrderNextDay = grderDateGroup.Where(o => !notStatus.Contains(o.Status) && o.DistrictId.Equals(14)
                            && o.DeliveryRangeId.Equals(14)).Select(s => s.Id).Count(),
                            InsideExceptNextDayOrder = grderDateGroup.Where(o => !notStatus.Contains(o.Status) && o.DistrictId.Equals(14)
                            && !nextDayService.Contains(o.DeliveryRangeId) && !sadorService.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                            OutsideOrderNextDay = grderDateGroup.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                            && nextDay.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                            OutsideOrderSador = grderDateGroup.Where(o => !notStatus.Contains(o.Status) && sador.Contains(o.DeliveryRangeId))
                            .Select(s => s.Id).Count(),
                            OutsideExceptNextDaySador = grderDateGroup.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                            && !nextDaySador.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count()
                        }
                    }
                },
                Merchants = new
                {
                    Placed = new
                    {
                        Merchant = grderDateGroup.Select(p => p.MerchantId).Distinct().Count(),
                        InsideMerchant = grderDateGroup.Where(x => x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count(),
                        OutsideMerchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count()
                    },
                    Confirmed = new
                    {
                        Merchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                        InsideMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count(),
                        OutsideMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count(),
                        InsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                        && x.DeliveryRangeId.Equals(14)).Select(x => x.MerchantId).Distinct().Count(),
                        InsideMerchantExceptNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                        && !nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                        OutsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                        && nextDay.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                        OutsideMerchantSador = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && sador.Contains(x.DeliveryRangeId))
                        .Select(x => x.MerchantId).Distinct().Count(),
                        OutsideMerchantExceptNextDaySador = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                        && !nextDaySador.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count()
                    }
                }
            }).ToList();
            queryDate.Add(queryTotal);

            //get today order data
            var dataToday = await _dashBoardPrivateAppRepository.GetDtOrders(orderBodyModel, "today");
            var queryToday = new
            {
                Name = "Today",
                Orders = new
                {
                    Placed = new
                    {
                        Order = dataToday.Select(p => p.Id).Count(),
                        InsideOrder = dataToday.Where(x => x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
                        OutsideOrder = dataToday.Where(x => !x.DistrictId.Equals(14)).Select(p => p.Id).Count()
                    },
                    Confirmed = new
                    {
                        Order = 
                        //Inside
                        dataToday.Where(o => o.DistrictId.Equals(14) && !notStatus.Contains(o.Status)
                        && !sadorService.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count() +
                        //Outside
                        dataToday.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && nextDay.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count() +
                        dataToday.Where(o => !notStatus.Contains(o.Status) && sador.Contains(o.DeliveryRangeId))
                        .Select(s => s.Id).Count() +
                        dataToday.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && !nextDaySador.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                        InsideOrder = dataToday.Where(o => o.DistrictId.Equals(14) && !notStatus.Contains(o.Status)
                        && !sadorService.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                        
                        OutsideOrderWithSador =
                        dataToday.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && nextDay.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count() +
                        dataToday.Where(o => !notStatus.Contains(o.Status) && sador.Contains(o.DeliveryRangeId))
                        .Select(s => s.Id).Count() +
                        dataToday.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                        && !nextDaySador.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                        
                        ServiceType = new
                        {
                            InsideOrderNextDay = dataToday.Where(o => !notStatus.Contains(o.Status) && o.DistrictId.Equals(14)
                            && o.DeliveryRangeId.Equals(14)).Select(s => s.Id).Count(),
                            InsideExceptNextDayOrder = dataToday.Where(o => !notStatus.Contains(o.Status) && o.DistrictId.Equals(14)
                            && !nextDayService.Contains(o.DeliveryRangeId) && !sadorService.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                            OutsideOrderNextDay = dataToday.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                            && nextDay.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count(),
                            OutsideOrderSador = dataToday.Where(o => !notStatus.Contains(o.Status) && sador.Contains(o.DeliveryRangeId))
                            .Select(s => s.Id).Count(),
                            OutsideExceptNextDaySador = dataToday.Where(o => !notStatus.Contains(o.Status) && !o.DistrictId.Equals(14)
                            && !nextDaySador.Contains(o.DeliveryRangeId)).Select(s => s.Id).Count()
                        }
                    }
                },
                Merchants = new
                {
                    Placed = new
                    {
                        Merchant = dataToday.Select(p => p.MerchantId).Distinct().Count(),
                        InsideMerchant = dataToday.Where(x => x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count(),
                        OutsideMerchant = dataToday.Where(x => !x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count()
                    },
                    Confirmed = new
                    {
                        Merchant = dataToday.Where(x => !notStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                        InsideMerchant = dataToday.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count(),
                        OutsideMerchant = dataToday.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.MerchantId).Distinct().Count(),
                        InsideMerchantNextDay = dataToday.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                        && x.DeliveryRangeId.Equals(14)).Select(x => x.MerchantId).Distinct().Count(),
                        InsideMerchantExceptNextDay = dataToday.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                        && !nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                        OutsideMerchantNextDay = dataToday.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                        && nextDay.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                        OutsideMerchantSador = dataToday.Where(x => !notStatus.Contains(x.Status) && sador.Contains(x.DeliveryRangeId))
                        .Select(x => x.MerchantId).Distinct().Count(),
                        OutsideMerchantExceptNextDaySador = dataToday.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                        && !nextDaySador.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count()
                    }
                }
            };

            queryDate.Add(queryToday);
            return queryDate;
        }
    }
}
