using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class CollectionTimeSlotViewModel
    {
        public int CollectionTimeSlotId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int Ordering { get; set; }
        public bool IsActive { get; set; }
        public int OrderLimit { get; set; } 
        public TimeSpan? CutOffTime { get; set; }
        public string FormattingStartTime { get; set; }
        public string FormattingEndTime { get; set; }
        public string FormattingCutOffTime { get; set; }
        public string SlotName { get; set; }
    }
}
