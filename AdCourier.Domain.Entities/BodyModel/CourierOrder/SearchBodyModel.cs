using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.CourierOrder
{
    public class SearchBodyModel
    {
        public int Index { set; get; } = 0;
        public int Count { set; get; } = 20;
        public string Search { set; get; } = "";
        public int QuickOrderLimit { get; set; } = 10;
        public string QuickOrderGenerateForHub { get; set; }
        public int QuickOrderGenerateBy { get; set; }
        public int RetentionUserId { get; set; }
    }

    public class RequestBodyModel
    {
        public DateTime FromDate { set; get; } = new DateTime();
        public DateTime ToDate { set; get; } = new DateTime();
        public int MerchantId { set; get; } = 0;
        public string Type { set; get; }
        public int DateFlag { set; get; }
        public int DistrictId { set; get; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public int CourierId { set; get; }
        public int DeliveryRangeId { set; get; }
        public int StatusId { set; get; } = 0;
        public int[] DeliveryRangeIds { get; set; }
        public string PaymentType { get; set; }
        public int DeliveryRiderId { get; set; }
        public DateTime RequestDate { get; set; }
        public int CollectionTimeSlotId { get; set; } = 0;
        public string StatusIds { set; get; } = "";
        public string HubName { get; set; } = "";
        public int Index { set; get; } = 0;
        public int Count { set; get; } = 20;
        public string Mobile { get; set; } = "";
        public int RetentionUserId { get; set; }
        public string RetentionUsers { get; set; } = "";
        public string SentToHubName { get; set; } = "";
        public int Flag { get; set; } = 0;
        public string CourierOrdersId { get; set; } = "";
        public int UserId { get; set; } = 0;
        public string Search { get; set; } = "";
        public string Comment { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public int ZoneId { get; set; } = 0;
        public string CollectionName { get; set; }
    }

    public class JsonBodyModel
    {
        public string Token { get; set; }
        public string Json { set; get; }
        public string Endpoint { set; get; }
        public int CourierUserId { get; set; }
    }
}
