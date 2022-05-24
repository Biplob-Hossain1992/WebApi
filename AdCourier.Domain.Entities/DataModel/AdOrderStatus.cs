using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("OrderStatus", Schema = "Deal")]
    public class AdOrderStatus
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string OrderStatus { get; set; }
        public bool IsActive { get; set; }
        public int? Model { get; set; }
    }
}
