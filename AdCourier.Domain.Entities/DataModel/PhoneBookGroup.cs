using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("PhoneBookGroup", Schema = "DT")]
    public class PhoneBookGroup
    {
        [Key]
        [Column("PhoneBookGroupId")]
        public int PhoneBookGroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int? CourierUserId { get; set; }
    }
}
