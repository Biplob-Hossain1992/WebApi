using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.CourierOrder
{
    public class CourierOrderAgeingViewModel
    {
        public int TotalOrderCount { get; set; }
        public int[] DayWiseOrderCount { get; set; }
        public int[] DayWiseAdvOrderCount { get; set; }
        public int[] DayWiseCodOrderCount { get; set; }
        public List<CourierOrderAgeingDataModel>[] DayWiseOrderDetails { get; set; }
    }
    public class CourierOrderAgeingDataModel
    {
        public string CourierOrdersId { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public string CollectionName { get; set; }
        public string CompanyName { get; set; }
        public int TotalHour { get; set; }
        public int IsAdvOrder { get; set; }
    }
}
