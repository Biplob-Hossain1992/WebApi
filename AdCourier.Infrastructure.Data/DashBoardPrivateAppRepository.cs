using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class DashBoardPrivateAppRepository : IDashBoardPrivateAppRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        public DashBoardPrivateAppRepository(IOptions<ConnectionStringList> connectionStrings, SqlServerContext sqlServerContext)
        {
            _connectionStrings = connectionStrings.Value;
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<dynamic> GetThirdPartyPaymentInfo(RequestBodyModel request)
        {
            var dateWisePaymentRate = "0";
            string[] dcList =
            {
                "BD_Khulna_SIRoad_D"
                ,"BD_Rajshahi_Kazla_D"
                ,"BD_Chattogram_Agrabad_D"
                ,"BD_Gazipur_Choidana_D"
                ,"BD_Dhaka_Mohammadpur_D"
                ,"BD_Dhaka_Banani_D"
                ,"BD_Barisal_Sadar_D"
                ,"BD_Dhaka_Motijheel_D"
                ,"BD_Narayanganj_Jamtola_D"
                ,"BD_Kushtia_Courtpara_D"
                ,"BD_Rangpur_Jcmpny_D"
                ,"BD_Cumilla_Jhautola_D"
                ,"BD_Tangail_Simultoli_D"
                ,"BD_CoxsBazar_HspRoad_D"
                ,"BD_Dhaka_Badda_D"
                ,"BD_Dhaka_Mirpur_D"
                ,"BD_Jessore_Bejpara_D"
                ,"BD_Bogra_Jalessoritola_D"
                ,"BD_Mymensingh_Bolspr_D"
                ,"BD_Dhaka_Jatrabari_D"
                ,"BD_Dhaka_Uttara_D"
                ,"BD_Pabna_Shalgoriya_D"
                ,"BD_Sherpur_Goshai_D"
                ,"BD_Gopalganj_HLRoad_D"
                ,"BD_Munshiganj_Masjidmarket_D"
                ,"BD_Moulvibazar_Borkapon_D"
                ,"BD_Habiganj_Chirakandi_D"
                ,"BD_Sunamganj_Moynar_D"
                ,"BD_Natore_Mokbul_D"
                ,"BD_Sirajganj_Serebangla_D"
                ,"BD_Gaibandha_Stnroad_D"
                ,"BD_Dinajpur_Kalitalak_D"
                ,"BD_Khagrachari_Kbazar_D"
                ,"BD_Noakhali_Maizdicourt_D"
                ,"BD_Tongi_Mohamuni_D"
                ,"BD_Chapai_Nawabganj_D"
                ,"BD_Naogaon_Gitanjali_D"
                ,"BD_Patiya_BusStand_D"

            };


            var deliveredOrders = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                                         join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on _courierOrders.CourierOrdersId equals _history.CourierOrderId

                                         where _history.Status == 15
                                         && _courierOrders.CollectionAmount > 0
                                         && _courierOrders.IsOfferBkashActive == false
                                         && _courierOrders.CourierId == request.CourierId
                                         && _history.PostedOn >= request.FromDate && _history.PostedOn < request.ToDate.AddDays(1)
                                         select _courierOrders.PodNumber).Distinct().ToArrayAsync();


            var registeredDeliveredOrders = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                                                   join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on
                                                       _courierOrders.CourierOrdersId equals _history.CourierOrderId

                                                   where _history.Status == 15
                                                   && _courierOrders.CollectionAmount > 0
                                                   && dcList.Contains(_courierOrders.DistinctCenter)
                                                   && _courierOrders.IsOfferBkashActive == false
                                                   && _courierOrders.CourierId == request.CourierId
                                                   && _history.PostedOn >= request.FromDate && _history.PostedOn < request.ToDate.AddDays(1)
                                                   select _courierOrders.PodNumber).Distinct().ToArrayAsync();


            var paidOrders = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                              join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on _courierOrders.CourierOrdersId equals _history.CourierOrderId

                              where _history.Status == 24
                              && deliveredOrders.Contains(_courierOrders.PodNumber)
                              select _courierOrders.PodNumber).Distinct().ToListAsync();
            
            if(registeredDeliveredOrders.Length != 0 && request.CourierId == 32)
            {
                dateWisePaymentRate = (Convert.ToDecimal(paidOrders.Count() * 100) / Convert.ToDecimal(registeredDeliveredOrders.Length)).ToString("F");

            }
            else if(deliveredOrders.Length !=0)
            {
                dateWisePaymentRate = (Convert.ToDecimal(paidOrders.Count() * 100) / Convert.ToDecimal(deliveredOrders.Length)).ToString("F");
            }

            var districtWisePayment =  (await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                                             join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on _courierOrders.CourierOrdersId equals _history.CourierOrderId
                                             join _district in _sqlServerContext.Districts.AsNoTracking() on _courierOrders.DistrictId equals _district.DistrictId
                                             join _h in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on new { a = _courierOrders.CourierOrdersId, b = 15 } equals new { a = _h.CourierOrderId, b = _h.Status } into grp
                                             from _deliveredHistory in grp.DefaultIfEmpty()

                                             where _history.Status == 24
                                             && _history.IsConfirmedBy == "courier"
                                             && _courierOrders.MerchantId != 1
                                             && _courierOrders.CourierId == request.CourierId
                                             && _courierOrders.CollectionAmount > 0
                                             && _courierOrders.IsOfferBkashActive == false

                                             && _history.PostedOn >= request.FromDate
                                             && _history.PostedOn < request.ToDate.AddDays(1)

                                             select new
                                             {
                                                 District = _district.District,
                                                 DistrictId = _courierOrders.DistrictId,
                                                 CollectionAmount = _courierOrders.CollectionAmount,
                                                 PODNumber = _courierOrders.PodNumber,
                                                 DeliveredDate = _deliveredHistory.PostedOn.Date
                                             }).Distinct().ToListAsync()).GroupBy(x => new { x.District, x.DistrictId, x.CollectionAmount, x.PODNumber })
                                             .Select(y => new
                                             {
                                                 District = y.Key.District,
                                                 DistrictId = y.Key.DistrictId,
                                                 CollectionAmount = y.Key.CollectionAmount,
                                                 PODNumber = y.Key.PODNumber,
                                                 DeliveredDate = y.Max(x => x.DeliveredDate)
                                             });
                                             
            var paymentReport = districtWisePayment.GroupBy(x => x.DistrictId).Select(y => new
            {
                DistrictId = y.Key,
                District = y.FirstOrDefault().District,
                CollectionAmount = y.Sum(j=> j.CollectionAmount),
                PODNumber = y.Count()
            }).OrderByDescending(z=> z.CollectionAmount).ToList();


            var report = new
            {
                PaymentRate = dateWisePaymentRate,
                TotalPaymentAmount = paymentReport.Sum(x => x.CollectionAmount),
                TotalPODNumber = paymentReport.Sum(x => x.PODNumber),
                DeliveryTodayPODCount = deliveredOrders.Length,
                RegisteredDeliveredPODCount = registeredDeliveredOrders.Length,
                PaidTodayPODCount = paidOrders.Count(),
                TodayDelivered = districtWisePayment.Where(x => x.DeliveredDate >= request.FromDate.Date && x.DeliveredDate < request.ToDate.AddDays(1).Date).Count(),
                YesterdayDelivered = districtWisePayment.Where(x => x.DeliveredDate == request.FromDate.AddDays(-1).Date).Count(),
                DayBeforeYesterdayDelivered = districtWisePayment.Where(x => x.DeliveredDate == request.FromDate.AddDays(-2).Date).Count(),
                DayBeforeBeforeYesterdayDelivered = districtWisePayment.Where(x => x.DeliveredDate == request.FromDate.AddDays(-3).Date).Count(),
                PreviousDelivered = districtWisePayment.Where(x => x.DeliveredDate < request.FromDate.AddDays(-3).Date).Count(),
                PaymentReport = paymentReport
            };

            return report;
        }

        public async Task<dynamic> GetStatusWiseOrderInfo()
        {
            int[] packageStatus = { 7, 8 };
            int[] CourierHandOverStatus = { 9, 10 };
            int[] statusGroup = { 7, 8, 9, 10, 41, 44 };

            var statusWiseOrderGroup = await (from _orderStatus in _sqlServerContext.CourierOrderStatus
                                              join _orders in _sqlServerContext.CourierOrders on _orderStatus.StatusId equals _orders.Status
                                              where statusGroup.Contains(_orderStatus.StatusId)
                                              select new
                                              {
                                                  status = _orderStatus.StatusId,
                                                  statusNameBng = _orderStatus.StatusNameBng,
                                                  statusNameEng = _orderStatus.StatusNameEng,
                                                  orderId = _orders.Id
                                              }).GroupBy(g => g.status)
                              .Select(s => new OrderStatusCountView
                              {
                                  StatusId = s.Key,
                                  StatusName = s.FirstOrDefault().statusNameEng,
                                  StatusNameBng = s.FirstOrDefault().statusNameBng,
                                  OrderCount = s.Count()
                              }).ToListAsync();

            var statusWiseOrderInfo = new
            {
                statusNamePackage = "Package",
                totalPackagedOrder = statusWiseOrderGroup.Where(p => packageStatus.Contains(p.StatusId)).Sum(s => s.OrderCount),
                statusNameHandOverCourier = "Hand Over To Courier",
                totalHandOveredOrder = statusWiseOrderGroup.Where(p => CourierHandOverStatus.Contains(p.StatusId)).Sum(s => s.OrderCount),
                statusNameAcceptRider = "Delivery Accepted by Rider-41",
                totalRiderAcceptedOrder = statusWiseOrderGroup.Where(p => p.StatusId == 41).Sum(s => s.OrderCount),
                statusNameOrderCollected = "Order has been collected from collection point-44",
                totalCollectedOrder = statusWiseOrderGroup.Where(p => p.StatusId == 44).Sum(s => s.OrderCount)
            };

            return statusWiseOrderInfo;
        }

        public async Task<dynamic> GetDeliveryTigerPaymentInfo(RequestBodyModel request)
        {
            var dateWisePaymentRate = "0";

            var courierIds = new int[] { 50, 51, 52, 53, 54, 55, 56, 57,58,59,60,61,62,63,64,65,66,67};


            var deliveredOrders = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                                        join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on _courierOrders.CourierOrdersId equals _history.CourierOrderId

                                        where _history.Status == 15
                                        && _courierOrders.CollectionAmount > 0
                                        && _courierOrders.IsOfferBkashActive == false
                                        && courierIds.Contains(_courierOrders.CourierId)
                                        && _history.PostedOn >= request.FromDate && _history.PostedOn < request.ToDate.AddDays(1)
                                        select _courierOrders.PodNumber).Distinct().ToArrayAsync();


            var paidOrders = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                                   join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on _courierOrders.CourierOrdersId equals _history.CourierOrderId

                                   where _history.Status == 24
                                   && deliveredOrders.Contains(_courierOrders.PodNumber)
                                   select _courierOrders.PodNumber).Distinct().ToListAsync();

            if(deliveredOrders.Length != 0)
            {
                 dateWisePaymentRate = (Convert.ToDecimal(paidOrders.Count() * 100) / Convert.ToDecimal(deliveredOrders.Length)).ToString("F");
            }

            var districtWisePayment = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                                             join _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on _courierOrders.CourierOrdersId equals _history.CourierOrderId
                                             join _couriers in _sqlServerContext.Couriers.AsNoTracking() on _courierOrders.CourierId equals _couriers.CourierId

                                             where _history.Status == 24
                                             && _history.IsConfirmedBy == "courier"
                                             && _courierOrders.MerchantId != 1
                                             && courierIds.Contains(_courierOrders.CourierId)
                                             && _courierOrders.CollectionAmount > 0
                                             && _courierOrders.IsOfferBkashActive == false

                                             && _history.PostedOn >= request.FromDate
                                             && _history.PostedOn < request.ToDate.AddDays(1)

                                             select new
                                             {
                                                 CourierId = _couriers.CourierId,
                                                 CourierName = _couriers.CourierName,
                                                 CollectionAmount = _courierOrders.CollectionAmount,
                                                 PODNumber = _courierOrders.PodNumber
                                             }).ToListAsync();

            var paymentReport = districtWisePayment.GroupBy(x => x.CourierId).Select(y => new
            {
                CourierId = y.Key,
                CourierName = y.FirstOrDefault().CourierName,
                CollectionAmount = y.Sum(j => j.CollectionAmount),
                PODNumber = y.Count()
            }).OrderByDescending(z => z.CollectionAmount).ToList();


            var report = new
            {
                PaymentRate = dateWisePaymentRate,
                TotalPaymentAmount = paymentReport.Sum(x => x.CollectionAmount),
                TotalPODNumber = paymentReport.Sum(x => x.PODNumber),
                DeliveryTodayPODCount = deliveredOrders.Length,
                PaidTodayPODCount = paidOrders.Count(),
                PaymentReport = paymentReport
            };

            return report;

        }

        public async Task<IEnumerable<CourierOrders>> GetDtOrders(OrderBodyModel orderBodyModel, string dateFlag)
        {
            if(dateFlag == "today")
            {
                orderBodyModel.FromDate = DateTime.Now;
                orderBodyModel.ToDate = DateTime.Now;
            }

            IQueryable<CourierOrders> data = from _courierOrders in _sqlServerContext.CourierOrders
                                             where _courierOrders.OrderDate.Date >= orderBodyModel.FromDate
                                             && _courierOrders.OrderDate.Date <= orderBodyModel.ToDate
                                             && _courierOrders.MerchantId != 1
                                             select new CourierOrders
                                             {
                                                 Id = _courierOrders.Id,
                                                 CourierOrdersId = _courierOrders.CourierOrdersId,
                                                 DistrictId = _courierOrders.DistrictId,
                                                 MerchantId = _courierOrders.MerchantId,
                                                 OrderDate = _courierOrders.OrderDate,
                                                 Status = _courierOrders.Status,
                                                 OrderType = _courierOrders.OrderType,
                                                 CollectionAmount = _courierOrders.CollectionAmount,
                                                 ActualPackagePrice = _courierOrders.ActualPackagePrice,
                                                 DeliveryCharge = _courierOrders.DeliveryCharge,
                                                 CodCharge = _courierOrders.CodCharge,
                                                 CollectionCharge = _courierOrders.CollectionCharge,
                                                 DeliveryRangeId = _courierOrders.DeliveryRangeId,
                                                 OfficeDrop = _courierOrders.OfficeDrop
                                             };

            return await data.ToListAsync();
        }
    }
}
