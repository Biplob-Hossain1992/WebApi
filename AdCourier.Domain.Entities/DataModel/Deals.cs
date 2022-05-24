using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Deals", Schema = "Deal")]
    public class Deals
    {
        [Key]
        public int DealId { set; get; }
        public string DealTitle { set; get; }
        public Decimal CouponPrice { get; set; }
        public string FolderName { set; get; }
        public string Sizes { set; get; }
        public int DealQtn { set; get; }
        public string Colors { set; get; }
        public bool IsSoldOut { set; get; }
    }
}
