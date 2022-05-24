using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("PriceAndOrderTypeHistory", Schema = "Log")]
    public class PriceAndOrderTypeHistory
    {
        [Key]
        [Column("PriceAndOrderTypeHistoryId")]
        public int PriceAndOrderTypeHistoryId { get; set; }
        public int Id { get; set; }
        public string OrderType { get; set; }
        public decimal CollectionAmount { get; set; }
        public decimal CodCharge { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public int PostedBy { get; set; }
    }
}
