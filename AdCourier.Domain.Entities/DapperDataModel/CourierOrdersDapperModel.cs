using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.DapperDataModel
{
    public class CourierOrdersDapperModel
    {
        public int DistrictId { get; set; }
        public string District { get; set; }
        public int CourierId { get; set; }
        public string CourierName { get; set; }
        public string CourierOrdersId { get; set; }
        public string DeliveredCourierOrdersId { get; set; }
        public int Status { get; set; }
        public string StatusNameEng { get; set; }
        public string Comment { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public int Hours { get; set; }
        public DateTime DeliveredDate { get; set; }
        public DateTime PackageDate { get; set; }
        public DateTime PickDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Thana { get; set; }
        public string DeliveryType { get; set; }
        public string Day { get; set; }
        public string DayType { get; set; }
        public string PodNumber { get; set; }
        public int CustomerRejectStatus { get; set; }
        public int PendingStatus { get; set; }
    }
    public class OrderResponseDapperModel
    {
        public string CompanyName { get; set; }
        public string Id { get; set; }
        public string CourierName { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string OtherMobile { get; set; }
        public string OrderDate { get; set; }
        public string StatusNameEng { get; set; }
        public string PodNumber { get; set; }
        public string UpdatedOn { get; set; }
        public string District { get; set; }
        public string Thana { get; set; }
        public string Area { get; set; }
        //public virtual DistrictsViewModel DistrictsViewModel { get; set; } = new DistrictsViewModel();
    }
}
