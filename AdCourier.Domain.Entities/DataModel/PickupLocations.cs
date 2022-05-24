using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("PickupLocations", Schema = "DT")]
    public class PickupLocations
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        //[Range(1, 10000)]
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public int CourierUserId { get; set; }
        public string PickupAddress { get; set; } = "";
        public string Longitude { get; set; } = "";
        public string Latitude { get; set; } = "";
        public bool IsActive { get; set; } = true;
        [Column(TypeName = "varchar(12)")]
        public string Mobile { get; set; } = "";

    }
}
