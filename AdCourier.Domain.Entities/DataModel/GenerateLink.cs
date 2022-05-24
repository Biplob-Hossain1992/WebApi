using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("GenerateLink", Schema = "DT")]
    public class GenerateLink
    {
        [Key]
        [Column("GenerateLinkId")]
        public int GenerateLinkId { get; set; }
        public string OrderType { get; set; }
        public decimal CollectionAmount { get; set; } = 0;
        public string PaymentOption { get; set; }
        public decimal CodCharge { get; set; }
        public string CustomerMobile { get; set; }
        public int ClassifiedId { get; set; }
        public string OfferCode { get; set; } = UniqueCodeGenerator.GetUniqueCode(isCharaterLowerCaseInCouponCode: true, minNumberForRandomNumberGenerator: 10, maxNumberForRandomNumberGenerator: 99);
    }
}
