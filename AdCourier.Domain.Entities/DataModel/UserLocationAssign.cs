using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("UserLocationAssign", Schema = "DT")]
    public class UserLocationAssign
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; } = 0;
        public int UserId { get; set; }
        public string UserType { get; set; }
    }
}
