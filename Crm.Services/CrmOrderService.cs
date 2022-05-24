using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using Crm.Domain.Entities.DapperBodyModel;
using Crm.Domain.Entities.DapperViewModel;
using Crm.Domain.Entities.DapperViewModel.DatabaseViewModel;
using Crm.Domain.Interfaces;
using Crm.Services.Interfaces;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crm.Services
{
    public class CrmOrderService : ICrmOrderService
    {
        private readonly ICrmOrderRepository _crmOrderRepositor;
        private readonly IRedisCacheClient _redis;
        public CrmOrderService(ICrmOrderRepository crmOrderRepositor, IRedisCacheClient redis)
        {
            _crmOrderRepositor = crmOrderRepositor;
            _redis = redis;
        }

        public async Task<Deals> GetProductInformation(int dealId)
        {
            string key = "GetProductInformation" + dealId.ToString();

            if (await _redis.Db2.Database.KeyExistsAsync(key) == true)
            {
                return await _redis.Db2.GetAsync<Deals>(key);
            }
            else
            {
                var data = await _crmOrderRepositor.GetProductInformation(dealId);
                if (data != null)
                {
                    bool added = await _redis.Db2.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                }

                return data;
            }
        }

        public async Task<CombineCrmOrderViewModel> GetOrders(SearchOrderBodyModel searchOrderBodyModel)
        {

            var orderModel = new CombineCrmOrderViewModel();

            var data =  await _crmOrderRepositor.GetOrders(searchOrderBodyModel);

            orderModel.CouponsCountViewModel = data.OrderCrmCountDataModel;

            orderModel.CouponsViewModel = (from o in data.OrderCrmDataModel
                    select new CouponsViewModel
                    {
                        CouponId = o.CouponId,
                        MerchantId = o.MerchantId,
                        CustomerId = o.CustomerId,
                        CouponQtn = o.CouponQtn,
                        CouponPrice = o.CouponPrice,
                        Commission = o.Commission,
                        DeliveryCharge = o.DeliveryCharge,
                        PostedOn = o.PostedOn,
                        OrderFrom = o.OrderFrom,
                        CustomerMobile = o.CustomerMobile,
                        CustomerAlternateMobile = o.CustomerAlternateMobile,
                        BkashMobileNumber = o.BkashMobileNumber,
                        PodNumber = o.PODnumber,
                        CustomerBillingAddress = o.CustomerBillingAddress,
                        DistrictId = o.DeliveryDist,
                        ThanaId = o.ThanaId,
                        AreaId = o.AreaId,
                        Sizes = o.Sizes,
                        OrderType = o.OrderType,
                        DealId = o.DealId,
                        Comments = o.Comments,
                        AppVersion = o.AppVersion,
                        PaymentsViewModel = new PaymentsViewModel
                        {
                            PaymentType = o.PaymentType,
                            CardType = o.CardType,
                            OnlineTransactionId = o.OnlineTransactionId,
                            PaymentStatus = o.PaymentStatus
                        },
                    }).ToList();

            return orderModel;
        }

        public async Task<IEnumerable<Domain.Entities.DapperViewModel.DatabaseViewModel.OrderStatusHistoryViewModel>> GetOrderHistoryInformation(string orderId)
        {
            var data = await _crmOrderRepositor.GetOrderHistoryInformation(orderId);

            return (from o in data
                    select new Domain.Entities.DapperViewModel.DatabaseViewModel.OrderStatusHistoryViewModel
                    {
                        CouponId = o.CouponId,
                        ConfirmationDate = o.ConfirmationDate,
                        Comments = o.Comments,
                        UsersViewModel = new UsersViewModel
                        {
                            UserName = o.UserName
                        },
                        AdOrderStatusViewModel = new AdOrderStatusViewModel
                        {
                            StatusId = o.StatusId,
                            OrderStatus = o.OrderStatus
                        }
                    }).ToList();
        }
    }
}
