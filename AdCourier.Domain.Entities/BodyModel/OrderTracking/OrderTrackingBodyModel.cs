using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.OrderTracking
{
    public class OrderTrackingBodyModel
    {
        [RegularExpression(@"^[dtDT0123456789-]+$", ErrorMessage = "Please enter correct format")]
        public string CourierOrderId { get; set; }
        public int CourierUserId { get; set; }
    }
}
