using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Banner
{
    public class BannerInfo
    {
        public bool ShowBanner { get; set; }
        public bool ShowPopupBanner { get; set; }
        public string BannerUrl { get; set; }
        public string PopupBannerUrl { get; set; }
        public int PopupFrequency { get; set; }
    }
}
