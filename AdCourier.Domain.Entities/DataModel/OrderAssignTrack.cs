using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("OrderAssignTrack", Schema = "DT")]
    public class OrderAssignTrack
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int SelectCourierId { get; set; }
        public int AssignCourierId { get; set; }
    }
}
