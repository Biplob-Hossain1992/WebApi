using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.DeliveryBondu
{
    public class DeliveryBondhuRegistration
    {
        public string Name { set; get; } = "";
        public string Mobile { set; get; } = "";
        public string AlternativeMobile { set; get; } = "";
        public string BkashMobileNumber { set; get; } = "";
        public string Password { set; get; } = "";
        public string Address { set; get; } = "";
        public int DistrictId { set; get; } = 0;
        public int ThanaId { set; get; } = 0;
        public int PostCode { set; get; } = 0;
        public string DistrictName { set; get; } = "";
        public string ThanaName { set; get; } = "";
        public int IsActive { set; get; } = 1;
        
        public int DeliveryCharge { set; get; } = 25;
        public int UserId { set; get; } = 0;
        public int Commission { set; get; } = 0;
        public int UserType { set; get; } = 0;
        public int AreaId { set; get; } = 0;
        public string AreaName { set; get; } = "";
        public string RegistrationFrom { set; get; } = "";
        public string Version { set; get; } = "";


    }
}
