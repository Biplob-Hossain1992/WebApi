using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class UserLocationAssignViewModel
    {
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameBng { get; set; }
        public int ThanaId { get; set; }
        public string ThanaName { get; set; }
        public string ThanaNameBng { get; set; }
        public int AreaId { get; set; } = 0;
        public string AreaName { get; set; } = "";
        public string AreaNameBng { get; set; } = "";
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserType { get; set; }
    }
}
