using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Customers", Schema = "Deal")]
    public class Customers
    {
        [Key]
        [Column("CustomerId")]
        public int CustomerId { get; set; }
        public string CName { get; set; }
        public string CMobile { get; set; }
        public string CAddress { get; set; }
        public string CEmail { get; set; }
        public string DeviceId { get; set; }
    }
}
