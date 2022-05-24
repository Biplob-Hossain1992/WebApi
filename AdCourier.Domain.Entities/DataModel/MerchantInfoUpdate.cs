using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("MerchantInfoUpdate", Schema = "Log")]
    public class MerchantInfoUpdate
    {
        [Key]
        [Column("MerchantInfoUpdateId")]
        public int MerchantInfoUpdateId { get; set; }
        public int CourierUserId { get; set; }
        public int UserId { get; set; }
        public DateTime InsertedOn { get; set; } = DateTime.Now;
    }
}
