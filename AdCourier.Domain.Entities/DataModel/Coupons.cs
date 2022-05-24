using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Coupons", Schema = "Deal")]
    public class Coupons
    {
        [Key]
        [Column("CouponId")]
        public int CouponId { get; set; }
        public int CouponQtn { get; set; }
        public int CouponPrice { get; set; }
    }
}
