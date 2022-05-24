using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class DeliveryUsersViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int IsActive { get; set; }
        public bool IsNowOffline { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsPermanentRider { get; set; }
        public string FirebaseToken { get; set; }
        public string RiderType { get; set; }

        //public virtual OfficeInfoViewModel OfficeInfoViewModel { get; set; } = null;
    }
}
