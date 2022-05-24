using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryNameEng { get; set; }
        public string CategoryNameBng { get; set; }
        public bool IsActive { get; set; }
        public List<AssignCourierUserCategoryViewModel> AssignCourierUserCategoryViewModel { set; get; }
    }
}
