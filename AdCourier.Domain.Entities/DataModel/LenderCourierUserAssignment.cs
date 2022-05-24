using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("LenderCourierUserAssignment", Schema = "Loan")]
    public class LenderCourierUserAssignment
    {
        [Key]
        [Column("AssignmentId")]
        public int AssignmentId { get; set; }
        public int LenderUserId { get; set; }
        public int CourierUserId { get; set; }
    }
}
