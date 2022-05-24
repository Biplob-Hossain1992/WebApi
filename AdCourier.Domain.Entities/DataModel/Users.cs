using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Users", Schema = "AD")]
    public class Users
    {
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public byte AdminType { get; set; }
        public string Passwrd { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public byte IsActive { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string PersonalEmail { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public byte AllowOutsideAccess { get; set; }
        public bool IsRetention { get; set; } = false;
        public bool IsAcquisition { get; set; } = false;
        public decimal SalaryAmount { get; set; }

    }
}
