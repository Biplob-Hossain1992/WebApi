using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Districts", Schema = "Deal")]
    public class Districts
    {
        [Key]
        [Column("DistrictId")]
        public int DistrictId { set; get; }
        public string District { set; get; }
        public string DistrictBng { set; get; }
        public int AreaType { set; get; }
        public int ParentId { set; get; }
        public string PostalCode { set; get; }
        public bool IsCity { get; set; }
        public bool IsActive { set; get; }
        public bool IsActiveForCorona { get; set; }
        public bool? IsPickupLocation { get; set; }
        public int DistrictPriority { get; set; } = 1000;
        public string RedxHubName { get; set; } = "";
        public int UpdatedBy { get; set; } = 0;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public int RedxAreaId { get; set; } = 0;
        public string RedxAreaName { get; set; } = "";
        public string PaperflyAreaName { get; set; } = "";
        //public string PaperflyMobileNo { get; set; } = "";
        public bool? IsDtOwnSecondMileDelivery { get; set; } = false;
        public string EDeshMobileNo { get; set; } = "";
        public int HasExpressDelivery { get; set; } = 1;
        public string TigerMobileNo { get; set; } = "";
        public string OwnSecondMileDelivery { get; set; } = "";
        public int CollectionTimeSlotId { get; set; } = 0;
        public string NextDayAlertMessage { get; set; }
        public string EdeshThana { get; set; } = "";
    }
}
