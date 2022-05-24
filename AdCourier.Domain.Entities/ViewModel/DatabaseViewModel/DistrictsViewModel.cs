using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class DistrictsViewModel
    {
        public int DistrictId { get; set; }
        public string District { get; set; }
        public int ThanaId { get; set; }
        public string Thana { get; set; }
        public string Area { get; set; }
        public string EdeshMobileNo { get; set; }
        public string TigerMobileNo { get; set; }
    }
}
