using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("ExtraCharge", Schema = "DT")]
    public class ExtraCharge
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Required]
        public decimal BreakableCharge { get; set; }
        public decimal CodChargePercentage { get; set; }
        public decimal CodChargeDhakaPercentage { get; set; }
        public int CodChargeMin { get; set; }
        public decimal BigProductCharge { get; set; }
        public decimal PerSmsCharge { get; set; }
        public decimal CodChargeMinOutSideDhaka { get; set; }
        public decimal AboveChargeApply { get; set; }
        public decimal CodChargeAbovePercentage { get; set; }
        public byte? CodChargeTypeFlag { get; set; } = 1;
        public decimal? CodChargeDhaka { get; set; } = 0;
        public decimal? CodChargeOutsideDhaka { get; set; } = 0;
        public decimal OpenBoxCharge { get; set; } = 0;
        //public decimal FirstCollectionCharge { get; set; }
        //public decimal CollectionCharge { get; set; }
    }
}
