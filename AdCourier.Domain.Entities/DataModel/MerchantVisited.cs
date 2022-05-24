using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("MerchantVisited", Schema = "Retention")]
    public class MerchantVisited
    {
        [Key]
        [Column("MerchantVisitedId")]
        public int MerchantVisitedId { get; set; }
        public int CourierUserId { get; set; }
        public int AdminUserId { get; set; }
        public string Latitude { get; set; }
        public string longitude { get; set; }
        public string VisitedSummary { get; set; }
        public DateTime VisitedDate { get; set; } = DateTime.Now;
    }
}
