using System;

namespace AdCourier.Domain.Entities.BodyModel.CourierOrder
{
    public class LoadCourierOrderBodyModel
    {
        public int Status { set; get; } = -1;
        public int[] StatusList { set; get; } = new int[] { -1 };
        public string[] StatusGroup { get; set; } = new string[] { "-1" };
        public DateTime FromDate { set; get; } = new DateTime();
        public DateTime ToDate { set; get; } = new DateTime();
        public int CourierUserId { set; get; } = -1;
        public string PodNumber { set; get; } = "";
        public string OrderIds { set; get; } = "";
        public string CollectionName { set; get; } = "";
        public int Index { set; get; } = 0;
        public int Count { set; get; } = 100;
        public string Mobile { get; set; } = "";
        public int[] DistrictIds { get; set; } = new int[] { -1 };
        public string DistrictGroupName { get; set; } = "";
        public int CourierId { get; set; } = 0;
        public int DistrictId { get; set; } = 0;
        public int ThanaId { get; set; } = 0;
        public int AreaId { get; set; } = 0;
        public string PaymentType { get; set; } = "-1";
        public string Priority { get; set; } = "";
        public string OrderType { get; set; } = "";
        public string OrderFrom { get; set; } = "-1";
        public int LowPrice { get; set; } = 0;
        public int HighPrice { get; set; } = 0;
        public int MinWeight { get; set; } = 0;
        public int MaxWeight { get; set; } = 0;
    }
    public class CollectorOrderBodyModel
    {
        public int CollectorId { set; get; }
        public string CollectionType { set; get; }
        public int CourierUserId { set; get; }
        public int Index { set; get; }
        public int Count { set; get; }
    }
}
