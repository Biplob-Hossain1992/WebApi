using System;

namespace AdCourier.Domain.Entities.BodyModel.Report
{
    public class OrderBodyModel
    {
        public DateTime FromDate { set; get; } = new DateTime();
        public DateTime ToDate { set; get; } = new DateTime();
        public string DateFormat { get; set; } = "date";
        public string InputDateType { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CollectorId { get; set; }
        public string PreferredPaymentCycle { get; set; } = "";
        //public int Id { get; set; }
        public int DeliveryRangeId { get; set; } = 0;
        public int Hours { set; get; }
        public int DistrictId { set; get; } = 0;
        public int CourierId { set; get; } = 0;
        public int OutsideDhaka { set; get; } = 0;
        public int RetentionUserId { set; get; } = 0;
        public int MerchantId { get; set; } = 0;
        public int LastDymanicMonth { get; set; } = 3;
        public int DateFlag { set; get; } = 0;
        public int lenderUserId { get; set; } = 0;
        public int StatusFlag { get; set; } = 0;
        public string DeliveryRangeIds { get; set; } = "";
        public int TigerStatusId { get; set; } = 0;
    }
}
