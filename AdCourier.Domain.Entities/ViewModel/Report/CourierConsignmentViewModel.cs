using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Report
{
    public class CourierConsignmentViewModel
    {
        //public List<CourierGroup> courierGroupList { get; set; }
        public List<CourierGroup> courierDhakaGroupList { get; set; }
        public List<CourierGroup> courierOutSideDhakaGroupList { get; set; }
        public CourierGroup courierGroup { get; set; }
    }

    public class CourierGroup
    {
        public int CourierId { get; set; }
        public string CourierName { get; set; }
        public int PodNumber { get; set; }
        public int MerchantCount { get; set; }

        public int PodNumberOutSide { get; set; }
        public int MerchantCountOutSide { get; set; }
    }

    public class CourierReportViewModel
    {
        public string CourierOrderId { get; set; }
        public int CourierId { get; set; }
        public string CourierName { get; set; }
        public string PodNumber { get; set; }
        public int MerchantId { get; set; }
        public string CompanyName { get; set; }
        public int DistrictId { get; set; }
    }
    public class CourierBillReportViewModel
    {
        public int Total { get; set; }
        public int CourierId { get; set; }
        public string CourierName { get; set; }
        public string District { get; set; }
        public string Payment { get; set; }
    }
}
