using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("OwnPhoneBook", Schema = "DT")]
    public class OwnPhoneBook
    {
        [Key]
        [Column("OwnPhoneBookId")]
        public int OwnPhoneBookId { get; set; }
        public int CourierUserId { get; set; }
        public string Mobile { get; set; }
        public string CustomerName { get; set; }
        public DateTime CurrentDate { get; set; } = DateTime.Now;
        public int PhoneBookGroupId { get; set; }
    }
}
