using System;

namespace AdCourier.Domain.Entities.BodyModel.Dashboard
{
    public class OrderCountByStatusGroupBodyModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int CourierUserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
