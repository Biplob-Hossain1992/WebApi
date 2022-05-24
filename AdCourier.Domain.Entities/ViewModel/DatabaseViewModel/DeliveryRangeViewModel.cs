using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class DeliveryRangeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public bool IsActive { get; set; } = true;
        public int Ranking { get; set; }
        public string DayType { get; set; } = "";
        public string OnImageLink { get; set; } = "";
        public string OffImageLink { get; set; } = "";
        public int ShowHide { get; set; } = 0;
        public string DeliveryAlertMessage { get; set; } = "";
        public string LoginHours { get; set; } = "";
        public string DateAdvance { get; set; } = "";
        public decimal CourierDeliveryCharge { get; set; }
        public string Type { get; set; }
        public int PriorityService { get; set; } = 0;
        public int AddCddDate { get; set; } = 0;
    }
}
