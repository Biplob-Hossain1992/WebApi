using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.CourierOrder
{
    public class ServiceTypeNew
    {
        public IEnumerable<DistrictSpeedModel> DistrictSpeedModels { get; set; }
        public IEnumerable<DistrictSpeedDetailsModel> DistrictSpeedDetailsModels { get; set; }
    }

    public class ServiceType
    {
        public int ServiceId { get; set; }
        public string ServiceTypeName { get; set; }
        public string ServiceInfo { get; set; }
        public int[] DeliveryRangeId { get; set; }
        public string[] ServiceTypeShow { get; set; }
    }

    public class DistrictSpeedModel
    {
        public int PackagedPodNumber { get; set; }
        public int DeliveredPodNumber { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        public decimal SpeedPercentage { get; set; }
        public decimal? Days { get; set; }
        public int TotalOrder { get; set; }
        public int TotalDeliveredOrder { get; set; }
        public int TotalDeliveredInTime { get; set; }
        public int TotalDeliveredOutTime { get; set; }
        public int TotalUndeliveredOrder { get; set; }
    }

    public class DistrictSpeedDetailsModel
    {
        public string PackagedCourierOrderId { get; set; }
        public string DeliveredCourierOrderId { get; set; }
        public DateTime PackagedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public decimal? Days { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public string District { get; set; }
        public string Thana { get; set; }
        public string Area { get; set; }
        public string PackagedPodNumber { get; set; }
        public string DeliveredPodNumber { get; set; }
        public string LastHubPodNumber { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string ServiceType { get; set; }
        public string CourierName { get; set; }
        public int DeliveredInTime { get; set; }
        public int DeliveredOutTime { get; set; }
    }
}
