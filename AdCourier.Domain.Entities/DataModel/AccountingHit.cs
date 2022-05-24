using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("AccountingHit", Schema = "Log")]
    public class AccountingHit
    {
        [Key]
        [Column("LogId")]

        public int LogId { get; set; }
        public string OrderId { get; set; }

    }
}
