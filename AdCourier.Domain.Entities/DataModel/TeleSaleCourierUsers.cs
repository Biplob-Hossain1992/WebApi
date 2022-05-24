using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("TeleSaleCourierUsers", Schema = "DT")]
    public class TeleSaleCourierUsers
    {
        [Key]
        [Column("TeleSaleCourierUsersId")]
        public int TeleSaleCourierUsersId { get; set; }
        public int CourierUserId { get; set; } = 0;
        public int CourierId { get; set; } = 0;
        public string CourierName { get; set; } = "";
        public int TeleSales { get; set; } = 0;
    }
}
