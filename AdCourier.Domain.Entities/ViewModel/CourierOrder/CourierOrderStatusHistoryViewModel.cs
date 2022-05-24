using AdCourier.Domain.Entities.DataModel;
using System.Collections.Generic;

namespace AdCourier.Domain.Entities.ViewModel.CourierOrder
{
    public class CourierOrderStatusHistoryViewModel : CourierOrderStatusHistory
    {
        public string MerchantPaymentAmount { get; set; }
        public string BkashNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }

        public string CustomerEmail { get; set; } = "";
        public string CustomerMessageFormat { get; set; } = "";
        public string CustomerEmailFormat { get; set; } = "";
        public string RetentionMessageFormat { get; set; } = "";
        public string CourierName { get; set; }
        public string MerchantMobile { get; set; }
        public string MerchantName { get; set; }
        public string CompanyName { get; set; }
        public string MerchantEmail { get; set; } = "";
        public string MessageFormat { get; set; } = "";
        public string EmailFormat { get; set; } = "";
        public bool IsSms { get; set; }
        public bool IsEmail { get; set; }
        public bool IsCustomerSms { get; set; }
        public bool IsCustomerEmail { get; set; }
        public string DistrictName { get; set; }
        public int DistrictId { get; set; }
        public string ThanaName { get; set; }
        public string AreaName { get; set; }
        public decimal CourierCharge { get; set; } = 0;
        public string RiderMobile { get; set; }
        public string RetentionUserMobile { get; set; } = "";
        public string ReferrerMobile { get; set; }
        public string EDeshMobileNo { get; set; }
        public int OrderId { get; set; }
        public int Total { get; set; }
        public virtual List<Couriers> Couriers { get; set; }
    }
}
