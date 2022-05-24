using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class AssignCourierUserCategoryViewModel
    {
        public int AssignCourierUserCategoryId { get; set; }
        public int CourierUserId { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryViewModel CategoryViewModel { get; set; } = null;
    }
}
