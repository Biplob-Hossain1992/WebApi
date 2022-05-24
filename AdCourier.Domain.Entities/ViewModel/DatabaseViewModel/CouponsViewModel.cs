using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class CouponsViewModel
    {
        public int CouponId { get; set; }
        public int MerchantId { get; set; }
        public int CustomerId { get; set; }
        public int DealId { get; set; }
        public int CouponQtn { get; set; }
        public int CouponPrice { get; set; }
        public int Commission { get; set; }
        public int DeliveryCharge { get; set; }
        public DateTime PostedOn { get; set; }
        public String OrderFrom { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerAlternateMobile { get; set; }
        public string PodNumber { get; set; }
        public string BkashMobileNumber { get; set; }
        public string CustomerBillingAddress { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public string Sizes { get; set; }
        public string OrderType { get; set; }
        public string Comments { get; set; }
        public string AppVersion { get; set; }
        public virtual PaymentsViewModel PaymentsViewModel { get; set; } = new PaymentsViewModel();
        public virtual UserProfileViewModel UserProfileViewModel { get; set; } = new UserProfileViewModel();
        public virtual CustomersViewModel CustomersViewModel { get; set; } = new CustomersViewModel();
        public virtual DealsViewModel DealsViewModel { get; set; } = new DealsViewModel();
    }
}
