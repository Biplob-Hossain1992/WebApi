using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DapperDataModel;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ProcedureDataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.RegisteredUsers;
using AdCourier.Domain.Entities.ViewModel.Report;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class OrderReportService : IOrderReportService
    {
        private readonly IOrderReportRepository _orderReportRepository;
        public OrderReportService(IOrderReportRepository orderReportRepository)
        {
            _orderReportRepository = orderReportRepository;
        }

        public async Task<IEnumerable<MerchantOrder>> GetMerchantOrders(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetMerchantOrders(orderBodyModel);
        }

        public async Task<IEnumerable<dynamic>> GetReceivedOrders(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetReceivedOrders(orderBodyModel);
        }
        public async Task<CourierConsignmentViewModel> CourierConsignment(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.CourierConsignment(orderBodyModel);
        }

        public async Task<IEnumerable<CourierReportViewModel>> CourierConsignmentDetails(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.CourierConsignmentDetails(requestBodyModel);
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> DeliveredOrderDetails(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.DeliveredOrderDetails(orderBodyModel);
        }

        public async Task<IEnumerable<RetentionUserPerformanceDapperModel>> RetentionUsersPerformance(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.RetentionUsersPerformance(orderBodyModel);
        }

        public async Task<ServiceTypeNew> GetDistrictSpeedByService(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetDistrictSpeedByService(orderBodyModel);
        }

        public async Task<IEnumerable<RetentionUserPerformanceDapperModel>> SRAssignedInactiveMerchantList(int retentionUserId, int inactiveDuration)
        {
            return await _orderReportRepository.SRAssignedInactiveMerchantList(retentionUserId, inactiveDuration);
        }

        public async Task<IEnumerable<RetentionComplainDetailsDapperModel>> RetentionUserWiseComplainDetails(int courierUserId)
        {
            return await _orderReportRepository.RetentionUserWiseComplainDetails(courierUserId);
        }

        public async Task<IEnumerable<dynamic>> OrderAssign(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.OrderAssign(orderBodyModel);
        }
        public async Task<IEnumerable<dynamic>> RiderOrderHistory(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.RiderOrderHistory(orderBodyModel);
        }
        public async Task<IEnumerable<dynamic>> GetOrders(OrderBodyModel orderBodyModel)
        {
            if (orderBodyModel.DateFormat.Trim().Equals("today"))
            {
                orderBodyModel.FromDate = DateTime.Today;
                orderBodyModel.ToDate = DateTime.Today;
            }
            else if (orderBodyModel.DateFormat.Trim().Equals("yesterday"))
            {
                orderBodyModel.FromDate = DateTime.Today.AddDays(-1);
                orderBodyModel.ToDate = DateTime.Today.AddDays(-1);
            }

            var data = await _orderReportRepository.GetOrders(orderBodyModel);

            var notStatus = new int[] { 0, 2, 29 };
            var needToPickStatus = new int[] { 1, 5, 6 };
            var ReceivedByDTHeadOfficeStatus = new int[] { 7, 8 };
            var pickedByDTCollectorStatus = new int[] { 3 };
            var shipmentOrderStatus = new int[] { 10, 11, 12, 13, 14, 23 };
            var postRejectedStatus = new int[] { 16, 17, 26 };
            var deliveredOrderStatus = new int[] { 15, 24, 25, 28 };

            var cod = new string[] { "Delivery Taka Collection" };
            var paid = new string[] { "Only Delivery" };

            var nextDayService = new int[] { 7, 14, 17 };
            var sadorService = new int[] { 18, 19 };
            var nextDaySadorService = new int[] { 7, 14, 17, 18, 19 };
            //inside dhaka (next day) - 14 outside dhaka - 7, 17

            var queryDateRange =
   new
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
               Order = data.Where(x => !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
               InsideOrder = data.Where(x => x.DistrictId.Equals(14) && !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
               OutsideOrder = data.Where(x => !x.DistrictId.Equals(14) && !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
               Amount = new
               {
                   CollectionAmount = new
                   {
                       Avg = data.Where(x => !notStatus.Contains(x.Status)).Select(p => p.ActualPackagePrice).Average(),
                       Sum = data.Where(x => !notStatus.Contains(x.Status)).Select(p => p.CollectionAmount).Sum()
                   }
               },
               ServiceType = new
               {
                   InsideMerchantNextDay = data.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                   && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                   InsideOtherMerchant = data.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                   && !nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                   OutsideMerchantNextDay = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                   && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                   OutsideMerchantSador = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                   && sadorService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                   OutsideOtherMerchant = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                   && !nextDaySadorService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count()
               }
           },
           Revenue = new
           {
               AverageRevenue = data.Where(x => !notStatus.Contains(x.Status)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average(),
               InsideAverageRevenue = data.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average(),
               OutsideAverageRevenue = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average()
           },
           COD = new
           {
               TotalCod = data.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
               InsideCod = data.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
               InsideCodAvg = data.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && x.DistrictId.Equals(14)).Select(p => p.CodCharge).Average(),
               OutsideCod = data.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && !x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
               OutsideCodAvg = data.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && !x.DistrictId.Equals(14)).Select(p => p.CodCharge).Average(),
               TotalCodCharge = data.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.CodCharge).Sum()
           },
           Collection = new
           {
               TotalCollectionAmount = data.Where(x => !notStatus.Contains(x.Status)).Select(p => p.CollectionAmount).Sum()
           },
           DeliveryCharge = new
           {
               TotalDeliveryCharge = data.Where(x => !notStatus.Contains(x.Status)).Select(p => p.DeliveryCharge).Sum(),
               InsideDeliveryCharge = data.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Sum(),
               InsideDeliveryChargeAvg = data.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Average(),
               OutsideDeliveryCharge = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Sum(),
               OutsideDeliveryChargeAvg = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Average()
           },
           CollectionCharge = new
           {
               AvgCollectionChargeNotOfficeDrop = data.Where(x => !notStatus.Contains(x.Status) && x.OfficeDrop == false).Select(p => p.CollectionCharge).Average()
           },
           NeedToPick = new
           {
               Order = data.Where(x => needToPickStatus.Contains(x.Status)).Select(p => p.Id).Count()
           },
           ReceivedByDTHeadOffice = new
           {
               Order = data.Where(x => ReceivedByDTHeadOfficeStatus.Contains(x.Status)).Select(p => p.Id).Count()
           },
           PickedByDTCollector = new
           {
               Order = data.Where(x => pickedByDTCollectorStatus.Contains(x.Status)).Select(p => p.Id).Count()
           },

           Shipment = new
           {
               Order = data.Where(x => shipmentOrderStatus.Contains(x.Status)).Select(p => p.Id).Count()
           },

           PostRejected = new
           {
               Order = data.Where(x => postRejectedStatus.Contains(x.Status)).Select(p => p.Id).Count()
           },
           Delivered = new
           {
               Order = data.Where(x => deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
               InsideOrder = new
               {
                   Order = data.Where(x => x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                   CodOrder = data.Where(x => x.DistrictId.Equals(14)
                   && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                   PaidOrder = data.Where(x => x.DistrictId.Equals(14)
                   && deliveredOrderStatus.Contains(x.Status) && paid.Contains(x.OrderType)).Select(p => p.Id).Count()
               },

               OutsideOrder = new
               {
                   Order = data.Where(x => !x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                   CodOrder = data.Where(x => !x.DistrictId.Equals(14)
                   && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                   PaidOrder = data.Where(x => !x.DistrictId.Equals(14)
                   && deliveredOrderStatus.Contains(x.Status)
                   && paid.Contains(x.OrderType)).Select(p => p.Id).Count()
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
               && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
               InsideOtherMerchant = data.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
               && !nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
               OutsideMerchantNextDay = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
               && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
               OutsideMerchantSador = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
               && sadorService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
               OutsideOtherMerchant = data.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
               && !nextDaySadorService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count()
           },
           NeedToPick = new
           {
               Merchant = data.Where(x => needToPickStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
           },
           ReceivedByDTHeadOffice = new
           {
               Merchant = data.Where(x => ReceivedByDTHeadOfficeStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
           },
           PickedByDTCollector = new
           {
               Merchant = data.Where(x => pickedByDTCollectorStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
           },

           Shipment = new
           {
               Merchant = data.Where(x => shipmentOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
           },

           PostRejected = new
           {
               Merchant = data.Where(x => postRejectedStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
           },
           Delivered = new
           {
               Merchant = data.Where(x => deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
               InsideMerchant = new
               {
                   Merchant = data.Where(x => x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                   CodMerchant = data.Where(x => x.DistrictId.Equals(14)
                   && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count(),
                   PaidMerchant = data.Where(x => x.DistrictId.Equals(14)
                   && deliveredOrderStatus.Contains(x.Status) && paid.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count()
               },

               OutsideMerchant = new
               {
                   Merchant = data.Where(x => !x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                   CodMerchant = data.Where(x => !x.DistrictId.Equals(14)
                   && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count(),
                   PaidMerchant = data.Where(x => !x.DistrictId.Equals(14)
                   && deliveredOrderStatus.Contains(x.Status)
                   && paid.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count()
               }
           }

       }

   };

            if (orderBodyModel.DateFormat.Trim().Equals("date"))
            {
                var queryDate =
              (from c in data
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
                           Order = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                           InsideOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14) && !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                           OutsideOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14) && !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                           Amount = new
                           {
                               CollectionAmount = new
                               {
                                   Avg = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.ActualPackagePrice).Average(),
                                   Sum = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.CollectionAmount).Sum()
                               }
                           },
                           ServiceType = new
                           {
                               InsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                               && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                               InsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                               && !nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                               OutsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                               && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                               OutsideMerchantSador = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                               && sadorService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                               OutsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                               && !nextDaySadorService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count()
                           }
                       },
                       Revenue = new
                       {
                           AverageRevenue = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average(),
                           InsideAverageRevenue = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average(),
                           OutsideAverageRevenue = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average()
                       },
                       COD = new
                       {
                           TotalCod = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                           InsideCod = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
                           InsideCodAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && x.DistrictId.Equals(14)).Select(p => p.CodCharge).Average(),
                           OutsideCod = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && !x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
                           OutsideCodAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && !x.DistrictId.Equals(14)).Select(p => p.CodCharge).Average(),
                           TotalCodCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.CodCharge).Sum()
                       },
                       Collection = new
                       {
                           TotalCollectionAmount = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.CollectionAmount).Sum()
                       },
                       DeliveryCharge = new
                       {
                           TotalDeliveryCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.DeliveryCharge).Sum(),
                           InsideDeliveryCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Sum(),
                           InsideDeliveryChargeAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Average(),
                           OutsideDeliveryCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Sum(),
                           OutsideDeliveryChargeAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Average()
                       },
                       CollectionCharge = new
                       {
                           AvgCollectionChargeNotOfficeDrop = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.OfficeDrop == false).Select(p => p.CollectionCharge).Average()
                       },
                       NeedToPick = new
                       {
                           Order = grderDateGroup.Where(x => needToPickStatus.Contains(x.Status)).Select(p => p.Id).Count()
                       },
                       ReceivedByDTHeadOffice = new
                       {
                           Order = grderDateGroup.Where(x => ReceivedByDTHeadOfficeStatus.Contains(x.Status)).Select(p => p.Id).Count()
                       },
                       PickedByDTCollector = new
                       {
                           Order = grderDateGroup.Where(x => pickedByDTCollectorStatus.Contains(x.Status)).Select(p => p.Id).Count()
                       },

                       Shipment = new
                       {
                           Order = grderDateGroup.Where(x => shipmentOrderStatus.Contains(x.Status)).Select(p => p.Id).Count()
                       },

                       PostRejected = new
                       {
                           Order = grderDateGroup.Where(x => postRejectedStatus.Contains(x.Status)).Select(p => p.Id).Count()
                       },
                       Delivered = new
                       {
                           Order = grderDateGroup.Where(x => deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                           InsideOrder = new
                           {
                               Order = grderDateGroup.Where(x => x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                               CodOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                               && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                               PaidOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                               && deliveredOrderStatus.Contains(x.Status) && paid.Contains(x.OrderType)).Select(p => p.Id).Count()
                           },

                           OutsideOrder = new
                           {
                               Order = grderDateGroup.Where(x => !x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                               CodOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                               && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                               PaidOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                               && deliveredOrderStatus.Contains(x.Status)
                               && paid.Contains(x.OrderType)).Select(p => p.Id).Count()
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
                           && nextDayService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count(),
                           InsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                           && !nextDayService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count(),
                           OutsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                           && nextDayService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count(),
                           OutsideMerchantSador = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                           && sadorService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count(),
                           OutsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                           && !nextDaySadorService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count()
                       },
                       NeedToPick = new
                       {
                           Merchant = grderDateGroup.Where(x => needToPickStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                       },
                       ReceivedByDTHeadOffice = new
                       {
                           Merchant = grderDateGroup.Where(x => ReceivedByDTHeadOfficeStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                       },
                       PickedByDTCollector = new
                       {
                           Merchant = grderDateGroup.Where(x => pickedByDTCollectorStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                       },

                       Shipment = new
                       {
                           Merchant = grderDateGroup.Where(x => shipmentOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                       },

                       PostRejected = new
                       {
                           Merchant = grderDateGroup.Where(x => postRejectedStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                       },
                       Delivered = new
                       {
                           Merchant = grderDateGroup.Where(x => deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                           InsideMerchant = new
                           {
                               Merchant = grderDateGroup.Where(x => x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                               CodMerchant = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                               && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count(),
                               PaidMerchant = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                               && deliveredOrderStatus.Contains(x.Status) && paid.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count()
                           },

                           OutsideMerchant = new
                           {
                               Merchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                               CodMerchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                               && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count(),
                               PaidMerchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                               && deliveredOrderStatus.Contains(x.Status)
                               && paid.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count()
                           }
                       }

                   }
               }).ToList();

                queryDate.Add(queryDateRange);

                return queryDate;
            }


            else if (orderBodyModel.DateFormat.Trim().Equals("month"))
            {

                var queryMonth =
                        (from c in data
                         group c by new
                         {
                             //Date = c.OrderDate.Date,
                             Month = c.OrderDate.Month,
                             Year = c.OrderDate.Year
                         } into grderDateGroup
                         select new
                         {
                             Name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(grderDateGroup.Key.Month) + "-" + grderDateGroup.Key.Year,
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
                                     Order = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                                     InsideOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14) && !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                                     OutsideOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14) && !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                                     Amount = new
                                     {
                                         CollectionAmount = new
                                         {
                                             Avg = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.ActualPackagePrice).Average(),
                                             Sum = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.CollectionAmount).Sum()
                                         }
                                     },
                                     ServiceType = new
                                     {
                                         InsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                                         && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                                         InsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                                         && !nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                                         OutsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                                         && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                                         OutsideMerchantSador = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                                         && sadorService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                                         OutsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                                         && !nextDaySadorService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count()
                                     }
                                 },
                                 Revenue = new
                                 {
                                     AverageRevenue = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average(),
                                     InsideAverageRevenue = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average(),
                                     OutsideAverageRevenue = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average()
                                 },
                                 COD = new
                                 {
                                     TotalCod = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                                     InsideCod = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
                                     InsideCodAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && x.DistrictId.Equals(14)).Select(p => p.CodCharge).Average(),
                                     OutsideCod = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && !x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
                                     OutsideCodAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && !x.DistrictId.Equals(14)).Select(p => p.CodCharge).Average(),
                                     TotalCodCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.CodCharge).Sum()
                                 },
                                 Collection = new
                                 {
                                     TotalCollectionAmount = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.CollectionAmount).Sum()
                                 },
                                 DeliveryCharge = new
                                 {
                                     TotalDeliveryCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.DeliveryCharge).Sum(),
                                     InsideDeliveryCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Sum(),
                                     InsideDeliveryChargeAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Average(),
                                     OutsideDeliveryCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Sum(),
                                     OutsideDeliveryChargeAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Average()
                                 },
                                 CollectionCharge = new
                                 {
                                     AvgCollectionChargeNotOfficeDrop = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.OfficeDrop == false).Select(p => p.CollectionCharge).Average()
                                 },
                                 NeedToPick = new
                                 {
                                     Order = grderDateGroup.Where(x => needToPickStatus.Contains(x.Status)).Select(p => p.Id).Count()
                                 },
                                 ReceivedByDTHeadOffice = new
                                 {
                                     Order = grderDateGroup.Where(x => ReceivedByDTHeadOfficeStatus.Contains(x.Status)).Select(p => p.Id).Count()
                                 },
                                 PickedByDTCollector = new
                                 {
                                     Order = grderDateGroup.Where(x => pickedByDTCollectorStatus.Contains(x.Status)).Select(p => p.Id).Count()
                                 },

                                 Shipment = new
                                 {
                                     Order = grderDateGroup.Where(x => shipmentOrderStatus.Contains(x.Status)).Select(p => p.Id).Count()
                                 },

                                 PostRejected = new
                                 {
                                     Order = grderDateGroup.Where(x => postRejectedStatus.Contains(x.Status)).Select(p => p.Id).Count()
                                 },
                                 Delivered = new
                                 {
                                     Order = grderDateGroup.Where(x => deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                                     InsideOrder = new
                                     {
                                         Order = grderDateGroup.Where(x => x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                                         CodOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                                         && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                                         PaidOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                                         && deliveredOrderStatus.Contains(x.Status) && paid.Contains(x.OrderType)).Select(p => p.Id).Count()
                                     },

                                     OutsideOrder = new
                                     {
                                         Order = grderDateGroup.Where(x => !x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                                         CodOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                                         && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                                         PaidOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                                         && deliveredOrderStatus.Contains(x.Status)
                                         && paid.Contains(x.OrderType)).Select(p => p.Id).Count()
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
                                     && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                                     InsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                                     && !nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                                     OutsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                                     && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                                     OutsideMerchantSador = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                                     && sadorService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count(),
                                     OutsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                                     && !nextDaySadorService.Contains(x.DeliveryRangeId)).Select(x => x.MerchantId).Distinct().Count()
                                 },
                                 NeedToPick = new
                                 {
                                     Merchant = grderDateGroup.Where(x => needToPickStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                                 },
                                 ReceivedByDTHeadOffice = new
                                 {
                                     Merchant = grderDateGroup.Where(x => ReceivedByDTHeadOfficeStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                                 },
                                 PickedByDTCollector = new
                                 {
                                     Merchant = grderDateGroup.Where(x => pickedByDTCollectorStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                                 },

                                 Shipment = new
                                 {
                                     Merchant = grderDateGroup.Where(x => shipmentOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                                 },

                                 PostRejected = new
                                 {
                                     Merchant = grderDateGroup.Where(x => postRejectedStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                                 },
                                 Delivered = new
                                 {
                                     Merchant = grderDateGroup.Where(x => deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                                     InsideMerchant = new
                                     {
                                         Merchant = grderDateGroup.Where(x => x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                                         CodMerchant = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                                         && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count(),
                                         PaidMerchant = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                                         && deliveredOrderStatus.Contains(x.Status) && paid.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count()
                                     },

                                     OutsideMerchant = new
                                     {
                                         Merchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                                         CodMerchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                                         && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count(),
                                         PaidMerchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                                         && deliveredOrderStatus.Contains(x.Status)
                                         && paid.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count()
                                     }
                                 }

                             }
                         }).ToList();

                queryMonth.Add(queryDateRange);
                return queryMonth;

            }

            else if (orderBodyModel.DateFormat.Trim().Equals("today") || orderBodyModel.DateFormat.Trim().Equals("yesterday"))
            {
                var queryDatee =
                (from c in data
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
                             Order = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                             InsideOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14) && !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                             OutsideOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14) && !notStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                             Amount = new
                             {
                                 CollectionAmount = new
                                 {
                                     Avg = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.ActualPackagePrice).Average(),
                                     Sum = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.CollectionAmount).Sum()
                                 }
                             },
                             ServiceType = new
                             {
                                 InsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                                 && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                                 InsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                                 && !nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                                 OutsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                                 && nextDayService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                                 OutsideMerchantSador = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                                 && sadorService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count(),
                                 OutsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                                 && !nextDaySadorService.Contains(x.DeliveryRangeId)).Select(x => x.Id).Distinct().Count()
                             }
                         },
                         Revenue = new
                         {
                             AverageRevenue = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average(),
                             InsideAverageRevenue = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average(),
                             OutsideAverageRevenue = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge + p.CodCharge + p.CollectionCharge).Average()
                         },
                         COD = new
                         {
                             TotalCod = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                             InsideCod = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
                             InsideCodAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && x.DistrictId.Equals(14)).Select(p => p.CodCharge).Average(),
                             OutsideCod = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && !x.DistrictId.Equals(14)).Select(p => p.Id).Count(),
                             OutsideCodAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType) && !x.DistrictId.Equals(14)).Select(p => p.CodCharge).Average(),
                             TotalCodCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.CodCharge).Sum()
                         },
                         Collection = new
                         {
                             TotalCollectionAmount = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.CollectionAmount).Sum()
                         },
                         DeliveryCharge = new
                         {
                             TotalDeliveryCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status)).Select(p => p.DeliveryCharge).Sum(),
                             InsideDeliveryCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Sum(),
                             InsideDeliveryChargeAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Average(),
                             OutsideDeliveryCharge = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Sum(),
                             OutsideDeliveryChargeAvg = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)).Select(p => p.DeliveryCharge).Average()
                         },
                         CollectionCharge = new
                         {
                             AvgCollectionChargeNotOfficeDrop = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.OfficeDrop == false).Select(p => p.CollectionCharge).Average()
                         },
                         NeedToPick = new
                         {
                             Order = grderDateGroup.Where(x => needToPickStatus.Contains(x.Status)).Select(p => p.Id).Count()
                         },
                         ReceivedByDTHeadOffice = new
                         {
                             Order = grderDateGroup.Where(x => ReceivedByDTHeadOfficeStatus.Contains(x.Status)).Select(p => p.Id).Count()
                         },
                         PickedByDTCollector = new
                         {
                             Order = grderDateGroup.Where(x => pickedByDTCollectorStatus.Contains(x.Status)).Select(p => p.Id).Count()
                         },

                         Shipment = new
                         {
                             Order = grderDateGroup.Where(x => shipmentOrderStatus.Contains(x.Status)).Select(p => p.Id).Count()
                         },

                         PostRejected = new
                         {
                             Order = grderDateGroup.Where(x => postRejectedStatus.Contains(x.Status)).Select(p => p.Id).Count()
                         },
                         Delivered = new
                         {
                             Order = grderDateGroup.Where(x => deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                             InsideOrder = new
                             {
                                 Order = grderDateGroup.Where(x => x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                                 CodOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                                 && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                                 PaidOrder = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                                 && deliveredOrderStatus.Contains(x.Status) && paid.Contains(x.OrderType)).Select(p => p.Id).Count()
                             },

                             OutsideOrder = new
                             {
                                 Order = grderDateGroup.Where(x => !x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.Id).Count(),
                                 CodOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                                 && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.Id).Count(),
                                 PaidOrder = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                                 && deliveredOrderStatus.Contains(x.Status)
                                 && paid.Contains(x.OrderType)).Select(p => p.Id).Count()
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
                             && nextDayService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count(),
                             InsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && x.DistrictId.Equals(14)
                             && !nextDayService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count(),
                             OutsideMerchantNextDay = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                             && nextDayService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count(),
                             OutsideMerchantSador = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                             && sadorService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count(),
                             OutsideOtherMerchant = grderDateGroup.Where(x => !notStatus.Contains(x.Status) && !x.DistrictId.Equals(14)
                             && !nextDaySadorService.Contains(x.DeliveryRangeId)).Select(p => p.MerchantId).Distinct().Count()
                         },
                         NeedToPick = new
                         {
                             Merchant = grderDateGroup.Where(x => needToPickStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                         },
                         ReceivedByDTHeadOffice = new
                         {
                             Merchant = grderDateGroup.Where(x => ReceivedByDTHeadOfficeStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                         },
                         PickedByDTCollector = new
                         {
                             Merchant = grderDateGroup.Where(x => pickedByDTCollectorStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                         },

                         Shipment = new
                         {
                             Merchant = grderDateGroup.Where(x => shipmentOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                         },

                         PostRejected = new
                         {
                             Merchant = grderDateGroup.Where(x => postRejectedStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count()
                         },
                         Delivered = new
                         {
                             Merchant = grderDateGroup.Where(x => deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                             InsideMerchant = new
                             {
                                 Merchant = grderDateGroup.Where(x => x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                                 CodMerchant = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                                 && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count(),
                                 PaidMerchant = grderDateGroup.Where(x => x.DistrictId.Equals(14)
                                 && deliveredOrderStatus.Contains(x.Status) && paid.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count()
                             },

                             OutsideMerchant = new
                             {
                                 Merchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14) && deliveredOrderStatus.Contains(x.Status)).Select(p => p.MerchantId).Distinct().Count(),
                                 CodMerchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                                 && deliveredOrderStatus.Contains(x.Status) && cod.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count(),
                                 PaidMerchant = grderDateGroup.Where(x => !x.DistrictId.Equals(14)
                                 && deliveredOrderStatus.Contains(x.Status)
                                 && paid.Contains(x.OrderType)).Select(p => p.MerchantId).Distinct().Count()
                             }
                         }

                     }
                 }).ToList();

                queryDatee.Add(queryDateRange);
                return queryDatee;
            }

            return null;
        }

        public async Task<IEnumerable<CollectorReceived>> CollectorReceivedReport(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.CollectorReceivedReport(orderBodyModel);
        }

        public async Task<IEnumerable<dynamic>> CollectorReceivedReportDetails(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.CollectorReceivedReportDetails(orderBodyModel);
        }

        public async Task<dynamic> OrderAgeingReport(string statusList)
        {
            var data = await _orderReportRepository.OrderAgeingReport(statusList);
            int totalOrderCount = data.Count();
            int[] dayWiseOrderCount = new int[7];
            int[] dayWiseAdvOrderCount = new int[7];
            int[] dayWiseCodOrderCount = new int[7];
            List<CourierOrderAgeingDataModel>[] dayWiseOrderDetails = new List<CourierOrderAgeingDataModel>[7];
            dayWiseOrderDetails[0] = new List<CourierOrderAgeingDataModel>();
            dayWiseOrderDetails[1] = new List<CourierOrderAgeingDataModel>();
            dayWiseOrderDetails[2] = new List<CourierOrderAgeingDataModel>();
            dayWiseOrderDetails[3] = new List<CourierOrderAgeingDataModel>();
            dayWiseOrderDetails[4] = new List<CourierOrderAgeingDataModel>();
            dayWiseOrderDetails[5] = new List<CourierOrderAgeingDataModel>();
            dayWiseOrderDetails[6] = new List<CourierOrderAgeingDataModel>();
            foreach (CourierOrderAgeingDataModel d in data)
            {
                if (d.TotalHour >= 0 && d.TotalHour <= 24)
                {
                    dayWiseOrderDetails[0].Add(d);
                    dayWiseOrderCount[0]++;
                    if (d.IsAdvOrder == 1)
                    {
                        dayWiseAdvOrderCount[0]++;
                    }
                    else
                    {
                        dayWiseCodOrderCount[0]++;
                    }
                }
                else if (d.TotalHour > 24 && d.TotalHour <= 48)
                {
                    dayWiseOrderDetails[1].Add(d);
                    dayWiseOrderCount[1]++;
                    if (d.IsAdvOrder == 1)
                    {
                        dayWiseAdvOrderCount[1]++;
                    }
                    else
                    {
                        dayWiseCodOrderCount[1]++;
                    }
                }
                else if (d.TotalHour > 48 && d.TotalHour <= 72)
                {
                    dayWiseOrderDetails[2].Add(d);
                    dayWiseOrderCount[2]++;
                    if (d.IsAdvOrder == 1)
                    {
                        dayWiseAdvOrderCount[2]++;
                    }
                    else
                    {
                        dayWiseCodOrderCount[2]++;
                    }
                }
                else if (d.TotalHour > 72 && d.TotalHour <= 96)
                {
                    dayWiseOrderDetails[3].Add(d);
                    dayWiseOrderCount[3]++;
                    if (d.IsAdvOrder == 1)
                    {
                        dayWiseAdvOrderCount[3]++;
                    }
                    else
                    {
                        dayWiseCodOrderCount[3]++;
                    }
                }
                else if (d.TotalHour > 96 && d.TotalHour <= 120)
                {
                    dayWiseOrderDetails[4].Add(d);
                    dayWiseOrderCount[4]++;
                    if (d.IsAdvOrder == 1)
                    {
                        dayWiseAdvOrderCount[4]++;
                    }
                    else
                    {
                        dayWiseCodOrderCount[4]++;
                    }
                }
                else if (d.TotalHour > 120 && d.TotalHour <= 144)
                {
                    dayWiseOrderDetails[5].Add(d);
                    dayWiseOrderCount[5]++;
                    if (d.IsAdvOrder == 1)
                    {
                        dayWiseAdvOrderCount[5]++;
                    }
                    else
                    {
                        dayWiseCodOrderCount[5]++;
                    }
                }
                else if (d.TotalHour > 144)
                {
                    dayWiseOrderDetails[6].Add(d);
                    dayWiseOrderCount[6]++;
                    if (d.IsAdvOrder == 1)
                    {
                        dayWiseAdvOrderCount[6]++;
                    }
                    else
                    {
                        dayWiseCodOrderCount[6]++;
                    }
                }
            }
            CourierOrderAgeingViewModel AgingDate = new CourierOrderAgeingViewModel
            {
                TotalOrderCount = totalOrderCount,
                DayWiseOrderCount = dayWiseOrderCount,
                DayWiseAdvOrderCount = dayWiseAdvOrderCount,
                DayWiseCodOrderCount = dayWiseCodOrderCount,
                DayWiseOrderDetails = dayWiseOrderDetails
            };

            return AgingDate;
        }

        public async Task<IEnumerable<AdCourier.Domain.Entities.ViewModel.CourierUsers.CourierUsersViewModel>> GetPreferredPaymentCycle(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetPreferredPaymentCycle(orderBodyModel);
        }

        public async Task<IEnumerable<LastMileInformationDapperModel>> GetLastMileInformation(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetLastMileInformation(orderBodyModel);
        }

        public async Task<dynamic> GetReferrerRefereeList(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetReferrerRefereeList(orderBodyModel);
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> GetOrderFromWiseOrderCount(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetOrderFromWiseOrderCount(orderBodyModel);
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetMerchantOrderFromDetails(LoadCourierOrderBodyModel loadCourierOrderBodyModel)
        {
            return await _orderReportRepository.GetMerchantOrderFromDetails(loadCourierOrderBodyModel);
        }

        public async Task<IEnumerable<AdCourier.Domain.Entities.DapperDataModel.CourierOrdersDapperModel>> PackageDateDeliveryDateReport(OrderBodyModel orderBodyModel)
        {
            if(orderBodyModel.StatusFlag.Equals(11))
                return await _orderReportRepository.PickDateDeliveryDateReport(orderBodyModel);
            else
                return await _orderReportRepository.PackageDateDeliveryDateReport(orderBodyModel);
        }
        public async Task<IEnumerable<AdCourier.Domain.Entities.ViewModel.DatabaseViewModel.CourierOrdersViewModel>> GetDeliveryRangeTypeWiseOrders(OrderBodyModel orderBodyModel)
        {
            //return await _orderReportRepository.GetDeliveryRangeTypeWiseOrders(orderBodyModel);

            var data = await _orderReportRepository.GetDeliveryRangeTypeWiseOrders(orderBodyModel);

            return data.Select(x => new AdCourier.Domain.Entities.ViewModel.DatabaseViewModel.CourierOrdersViewModel
            {
                CourierOrdersId = x.CourierOrdersId,
                Status = x.Status,
                StatusNameEng = x.StatusNameEng,
                Comment = x.Comment,
                ExpectedDeliveryDate = x.ExpectedDeliveryDate,
                Hours = x.Hours,
                PodNumber = x.PodNumber,
                Couriers = new CouriersViewModel
                {
                    CourierId = x.CourierId,
                    CourierName = x.CourierName
                },
                DistrictsViewModel = new DistrictsViewModel
                {
                    DistrictId = x.DistrictId,
                    District = x.District,
                    Thana = x.Thana
                },
                DeliveryRange = new DeliveryRange
                {
                    Name = x.DeliveryType,
                    Day = x.Day,
                    DayType = x.DayType
                }

            });
        }

        public async Task<IEnumerable<Domain.Entities.ViewModel.RegisteredUsers.RegisteredUsersViewModel>> GetAllRegisteredUsers(string joinDate)
        {
            return await _orderReportRepository.GetAllRegisteredUsers(joinDate);
        }
        public async Task<dynamic> GetRetentionAcquisitionUsers(int userId)
        {
            return await _orderReportRepository.GetRetentionAcquisitionUsers(userId);
        }
        public async Task<IEnumerable<MerchantDetailsResponseModel>> MerchantDetailsWithOrders(string joinDate, int flag)
        {
            return await _orderReportRepository.MerchantDetailsWithOrders(joinDate, flag);
        }
        public async Task<dynamic> AcquisitionManagerWiseOrderDetails(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.AcquisitionManagerWiseOrderDetails(requestBodyModel);
        }
        public async Task<IEnumerable<MerchantDetailsResponseModel>> MerchantDetails(string joinDate, int flag)
        {
            return await _orderReportRepository.MerchantDetails(joinDate, flag);
        }
        public async Task<IEnumerable<AdCourier.Domain.Entities.DapperDataModel.OrderResponseDapperModel>> GetTotalOrdersWithDateFlag(RequestBodyModel requestBody)
        {
            return await _orderReportRepository.GetTotalOrdersWithDateFlag(requestBody);
        }
        public async Task<IEnumerable<dynamic>> GetCalledMerchantList(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetCalledMerchantList(orderBodyModel);
        }
        public async Task<IEnumerable<dynamic>> GetVisitedMerchantList(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetVisitedMerchantList(orderBodyModel);
        }

        public async Task<IEnumerable<dynamic>> DateWiseOrderPlacedCalledMerchantList(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.DateWiseOrderPlacedCalledMerchantList(orderBodyModel);
        }

        public async Task<IEnumerable<dynamic>> DateWiseOrderPlacedVisitedMerchantList(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.DateWiseOrderPlacedVisitedMerchantList(orderBodyModel);
        }

        public async Task<IEnumerable<OrderStatusCountView>> StatusWiseTotalOrder(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.StatusWiseTotalOrder(orderBodyModel);
        }

        public async Task<IEnumerable<dynamic>> StatusWiseTotalOrderDetails(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.StatusWiseTotalOrderDetails(requestBodyModel);
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> CourierWiseReturnReport(OrderBodyModel orderBodyModel)
        {
            var data = await _orderReportRepository.CourierWiseReturnReport(orderBodyModel);

            return data.Select(x => new CourierOrdersViewModel
            {
                Id = x.Id,
                Status = x.Status,
                Comment = x.Comment,
                PodNumber = x.PodNumber,
                UpdatedOn = x.UpdatedOn,
                OrderDate = x.OrderDate,
                Couriers = new CouriersViewModel
                {
                    CourierId = x.CourierId,
                    CourierName = x.CourierName
                },
                CourierOrderStatus = new OrderStatusViewModel
                {
                    StatusNameEng = x.StatusNameEng
                }
            });
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> PendingShipmentReconciliation(string orderId, int flag)
        {
            var data = await _orderReportRepository.PendingShipmentReconciliation(orderId, flag);

            return data.Select(x => new CourierOrdersViewModel
            {
                Id = x.Id,
                Status = x.Status,
                TotalOrder = x.TotalOrder,
                CourierUsers = new CourierUsersViewModel
                {
                    CompanyName = x.CompanyName
                },
                CourierOrderStatus = new OrderStatusViewModel
                {
                    StatusNameEng = x.StatusNameEng
                }
            });
        }

        public async Task<IEnumerable<HoursCalculationViewModel>> HoursCalculationReport(RequestBodyModel bodyModel)
        {
            return await _orderReportRepository.HoursCalculationReport(bodyModel);
        }
        public async Task<IEnumerable<HoursCalculationViewModel>> HoursCalculationReportForDelivery(RequestBodyModel bodyModel)
        {
            return await _orderReportRepository.HoursCalculationReportForDelivery(bodyModel);
        }

        public async Task<dynamic> EatAnalysisOverCollectionReport(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.EatAnalysisOverCollectionReport(orderBodyModel);
        }


        public async Task<IEnumerable<dynamic>> GetTelesalesActiveMerchantList(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetTelesalesActiveMerchantList(orderBodyModel);
        }

        public async Task<IEnumerable<CourierBillReportViewModel>> GetCourierBillList(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.GetCourierBillList(requestBodyModel);
        }

        public async Task<IEnumerable<LoanSurveyViewModel>> GetLoanSurvey(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetLoanSurvey(orderBodyModel);
        }

        public async Task<IEnumerable<LoanSurveyViewModel>> GetLoanSurveyByLender(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetLoanSurveyByLender(orderBodyModel);
        }

        public async Task<IEnumerable<dynamic>> MonthWiseTotalCollectionAmount(int CourierUserId)
        {
            return await _orderReportRepository.MonthWiseTotalCollectionAmount(CourierUserId);
        }

        public async Task<IEnumerable<dynamic>> AcquisitionLeadManagement(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.AcquisitionLeadManagement(orderBodyModel);
        }

        public async Task<IEnumerable<CourierOrdersViewModel>> ReAttemptOrdersList(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.ReAttemptOrdersList(orderBodyModel);
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetVouchers(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.GetVouchers(requestBodyModel);
        }

        public async Task<IEnumerable<dynamic>> TelesalesDetails(int teleSalesStatus, DateTime date)
        {
            return await _orderReportRepository.TelesalesDetails(teleSalesStatus, date);
        }

        public async Task<IEnumerable<dynamic>> StatusWiseTelesalesDetails(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.StatusWiseTelesalesDetails(requestBodyModel);
        }

        public async Task<IEnumerable<dynamic>> StatusWiseHistoryCount(RequestBodyModel bodyModel)
        {
            return await _orderReportRepository.StatusWiseHistoryCount(bodyModel);
        }
        public async Task<IEnumerable<dynamic>> StatusWiseHistoryCountDetails(RequestBodyModel bodyModel)
        {
            return await _orderReportRepository.StatusWiseHistoryCountDetails(bodyModel);
        }

        public async Task<IEnumerable<dynamic>> GetReRoutedOrders(OrderBodyModel orderBodyModel)
        {
            return await _orderReportRepository.GetReRoutedOrders(orderBodyModel);
        }
        public async Task<IEnumerable<dynamic>> SlotBasedOrder(RequestBodyModel requestBody)
        {
            return await _orderReportRepository.SlotBasedOrder(requestBody);
        }

        public async Task<IEnumerable<dynamic>> TigerReport(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.TigerReport(requestBodyModel);
        }
        public async Task<IEnumerable<RiderPaymentReportViewModel>> RiderPaymentReport(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.RiderPaymentReport(requestBodyModel);
        }

        public async Task<IEnumerable<dynamic>> ReturnCourierPendingStatusCount(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.ReturnCourierPendingStatusCount(requestBodyModel);
        }
        public async Task<IEnumerable<dynamic>> ReturnCourierPendingStatusCountDetails(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.ReturnCourierPendingStatusCountDetails(requestBodyModel);
        }
        public async Task<IEnumerable<dynamic>> ReturnCourierPendingStatusDetails(RequestBodyModel requestBodyModel)
        {
            return await _orderReportRepository.ReturnCourierPendingStatusDetails(requestBodyModel);
        }
    }
}
