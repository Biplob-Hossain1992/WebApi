using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.IntegrationBody;
using AdCourier.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AdCourier.Infrastructure.Data
{
    public class IntegrationRepository : IIntegrationRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly SqlServerContext _sqlServerContext;
        public IntegrationRepository(SqlServerContext sqlServerContext, IOrderHistoryRepository orderHistoryRepository
            , IHttpContextAccessor httpContextAccessor)
        {
            _sqlServerContext = sqlServerContext;
            _orderHistoryRepository = orderHistoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> order(IntegrationOrderBodyModel request)
        {
            var orderHistory = new CourierOrderStatusHistory();

            var newOrder = AssignMethod(request);

            var resOrder = await _sqlServerContext.CourierOrders.AddAsync(newOrder);
            await _sqlServerContext.SaveChangesAsync();

            if (resOrder != null)
            {
                orderHistory.CourierOrderId = newOrder.CourierOrdersId;
                orderHistory.IsConfirmedBy = request.OrderFrom;
                orderHistory.PostedBy = newOrder.MerchantId;
                orderHistory.MerchantId = newOrder.MerchantId;
                orderHistory.PodNumber = newOrder.PodNumber;
                orderHistory.Comment = "New Order";

                var responseOrderHistory = await _orderHistoryRepository.AddCourierOrderHistory(orderHistory);
            }

            return newOrder.CourierOrdersId;
        }

        private CourierOrders AssignMethod(IntegrationOrderBodyModel request)
        {
            var service = new DeliveryRange();

            string merchantId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            var details = _sqlServerContext.DeliveryChargeDetails.OrderByDescending(d => d.CourierDeliveryCharge).FirstOrDefault(x => x.DistrictId.Equals(request.DistrictId) 
            && x.ThanaId.Equals(request.ThanaId) && x.AreaId.Equals(request.AreaId)
            && x.WeightRangeId.Equals(request.WeightRangeId)
            && x.ServiceType.Equals(request.ServiceType)
            && x.IsActive.Equals(true));

            if (details != null)
            {
                service = _sqlServerContext.DeliveryRange.FirstOrDefault(s => s.Id.Equals(details.DeliveryRangeId));
            }

            var order = new CourierOrders();

            order.CustomerName = request.CustomerName;
            order.Mobile = request.Mobile;
            order.OtherMobile = request.OtherMobile;
            order.Address = request.Address;
            order.DistrictId = request.DistrictId;
            order.ThanaId = request.ThanaId;
            order.AreaId = request.AreaId;
            order.CollectAddressDistrictId = request.CollectAddressDistrictId;
            order.CollectAddressThanaId = request.CollectAddressThanaId;
            order.CollectionName = request.CollectionName;
            order.CollectionAmount = request.CollectionAmount;
            order.ActualPackagePrice = request.ActualPackagePrice;
            order.Weight = request.Weight;
            order.WeightRangeId = request.WeightRangeId;
            order.PodNumber = request.PodNumber;
            order.OrderDate = DateTime.Now;
            order.CollectAddress = request.CollectAddress;
            order.CollectionTimeSlotId = request.CollectionTimeSlotId;
            order.CollectionTime = request.CollectionTime;
            order.OrderFrom = request.OrderFrom;
            order.OfficeDrop = request.OfficeDrop;
            order.Note = request.Note;
            order.MerchantId = Convert.ToInt32(merchantId);
            order.OrderType = request.OrderType;
            order.Comment = "New Order";
            order.Status = 0;
            order.IsActive = true;
            order.PaymentType = service.Name;
            order.ServiceType = details.ServiceType;
            order.DeliveryRangeId = details.DeliveryRangeId;
            order.DeliveryCharge = details.CourierDeliveryCharge;

            return order;
        }

        public async Task<bool> UpdateOrderHistory(string courierOrderId, UpdateStatusBodyModel request)
        {
            int startIndex = 3;
            int endIndex = courierOrderId.Length - 3;
            int orderId = Convert.ToInt32(courierOrderId.Substring(startIndex, endIndex));

            string merchantId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == orderId);

            if (entity != null)
            {
                entity.Comment = request.Comment;
                entity.Status = request.Status;
                entity.IsConfirmedBy = request.IsConfirmedBy;
                entity.UpdatedOn = DateTime.Now;
                entity.UpdatedBy = Convert.ToInt32(merchantId);

                // Update entity in DbSet
                _sqlServerContext.CourierOrders.Update(entity);

                // Save changes in database
                int count = await _sqlServerContext.SaveChangesAsync();
                if (count > 0)
                {
                    CourierOrderStatusHistory courierOrderStatusHistoryObj = new CourierOrderStatusHistory();
                    courierOrderStatusHistoryObj.CourierOrderId = entity.CourierOrdersId;
                    courierOrderStatusHistoryObj.IsConfirmedBy = request.IsConfirmedBy;
                    courierOrderStatusHistoryObj.OrderDate = entity.OrderDate;
                    courierOrderStatusHistoryObj.Status = request.Status;
                    courierOrderStatusHistoryObj.PostedBy = Convert.ToInt32(merchantId);
                    courierOrderStatusHistoryObj.MerchantId = Convert.ToInt32(merchantId);
                    courierOrderStatusHistoryObj.Comment = request.Comment;


                    var responseOrderHistory = await _orderHistoryRepository.AddCourierOrderHistory(courierOrderStatusHistoryObj);
                }
            }

            return true;
        }

    }
}
