using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.DeliveryBondu
{
    public class NotificationBodyModel
    {
        public int StatusId { get; set; }
        public int NotificationType { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageLink { get; set; } = "";
        public string BigText { get; set; } = "";
        public string ServiceType { get; set; } = "";
        public bool IsActiveNotification { get; set; } = false;
    }
}
