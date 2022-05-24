using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Settings", Schema = "DT")]
    public class Settings
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        //[Required]
        public string TermsConditions { get; set; }
        public string RegisterTermsConditions { get; set; }
        public string VoucherTermsConditions { get; set; }
    }
}
