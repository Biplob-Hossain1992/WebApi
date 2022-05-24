using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Banner
{
    public class BannerInfoViewModel
    {
        public string ReferBanner { get; set; }
        public string LoanSurveyBanner { get; set; }
        public PopUp PopUp { get; set; }
        public BannerList BannerList { get; set; }
        public int DashboardDataDuration { get; set; }
        public bool ShowOrderPopup { get; set; }
        public int InstantPaymentOTPLimit { get; set; }
        public int InstantPaymentHourLimit { get; set; }
        public string InstantPaymentHourLimitRange { get; set; }
        public int CurrentAppVersionCode { get; set; }
        public decimal ReAttemptCharge { get; set; } = 3;
        public bool IsReferActive { get; set; }
        public bool IsLoanSurveyActive { get; set; }

    }
    public class BannerList {
        public bool ShowBanner { get; set; }
        public List<Banner> Banners { get; set; }
    }
    public class PopUp {
        public string PopUpUrl { get; set; }
        public bool ShowPopUp { get; set; }
        public int PopUpFrequency { get; set; }
        public string Route { get; set; }

    }
    public class Banner {
        public string BannerUrl { get; set; }
        public bool IsActive { get; set; }
        public string WebUrl { get; set; }
        public bool IsWebLinkActive { get; set; }
    }
}
