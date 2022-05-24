using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("AssignCourierUserCategory", Schema = "DT")]
    public class AssignCourierUserCategory
    {
        [Key]
        [Column("AssignCourierUserCategoryId")]
        public int AssignCourierUserCategoryId { get; set; }
        public int CourierUserId { get; set; }
        public int CategoryId { get; set; }
    }
}
