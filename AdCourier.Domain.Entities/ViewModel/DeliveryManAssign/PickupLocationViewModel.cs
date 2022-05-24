using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DeliveryManAssign
{
    public class PickupLocationViewModel
    {
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameEng { get; set; }
        public int ThanaId { get; set; }
        public string ThanaName { get; set; }
        public string ThanaNameEng { get; set; }
        public string PickupAddress { get; set; }
        public int CourierUserId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int AcceptedOrderCount { get; set; }
    }
}
