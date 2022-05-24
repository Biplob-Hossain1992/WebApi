using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class UserProfileViewModel
    {
        public int ProfileID { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Mobile { get; set; }
        public string MobileExtra1 { get; set; }
        public string MobileExtra2 { get; set; }
        public string LoginEmail { get; set; }
        public Byte? BusinessModelType { get; set; }
        public string ContactAddress { get; set; }
        public int? DeliveryCharge { get; set; }
    }
}
