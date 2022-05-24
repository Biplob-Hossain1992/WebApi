using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("ChangeDeliveryChargeDetailsLog", Schema = "Log")]
    public class ChangeDeliveryChargeDetailsLog
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public int OldDeliveryRangeId { get; set; }
        public int NewDeliveryRangeId { get; set; }
        public decimal CourierDeliveryCharge { get; set; }
        public int OldCourierId { get; set; }
        public int NewCourierId { get; set; }
        public string ServiceType { get; set; }
        public bool OldIsActive { get; set; }
        public bool NewIsActive { get; set; }
        public DateTime ChangedDate { get; set; }
        public int UserId { get; set; }
    }
}
