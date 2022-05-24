using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DeliveryManAssign
{
    public class DeliveryUserAcceptedViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string[] CourierOrderIds { get; set; }
    }
    public class DeliveryUserAcceptedSingleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CourierOrderId { get; set; }
    }
}
