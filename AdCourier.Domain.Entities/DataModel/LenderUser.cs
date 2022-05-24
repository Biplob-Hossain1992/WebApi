using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("LenderUser", Schema = "Loan")]
    public class LenderUser
    {
        [Key]
        [Column("LenderUserId")]
        public int LenderUserId { get; set; }
        public string UserName { get; set; }
        public string LenderName { get; set; } = "";
        public string Password { get; set; }
        public string Mobile { get; set; } = "";
        public string RoleName { get; set; } = "";
    }
}
