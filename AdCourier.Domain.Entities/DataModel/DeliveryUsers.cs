using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("DeliveryUsers", Schema = "AD")]
    public class DeliveryUsers
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int IsActive { get; set; }
        public bool IsNowOffline { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; } = 0;
        public bool IsPermanentRider { get; set; }
        public string FirebaseToken { get; set; }
        public string RiderType { get; set; }
        public string HubName { get; set; }
        public bool? IsProfileImage { get; set; } = false;
        public int? UserType { get; set; } = 0;
        public bool? IsDrivingLicense { get; set; } = false;
        public bool? IsNID { get; set; } = false;
        public string AlternativeMobile { get; set; } = "";
        public string BkashMobileNumber { set; get; } = "";
        public string Address { set; get; } = "";
        public int? DistrictId { set; get; } = 0;
        public string DistrictName { set; get; } = "";
        public int? ThanaId { set; get; } = 0;
        public string ThanaName { set; get; } = "";
        public int? PostCode { set; get; } = 0;
        public int? AreaId { set; get; } = 0;
        public string AreaName { set; get; } = "";
    }
}
