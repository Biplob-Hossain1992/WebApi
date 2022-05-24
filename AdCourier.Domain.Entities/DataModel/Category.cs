using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Category", Schema = "DT")]
    public class Category
    {
        [Key]
        [Column("CategoryId")]
        public int CategoryId { get; set; }
        public string CategoryNameEng { get; set; } = "";
        public string CategoryNameBng { get; set; } = "";
        public bool? IsActive { get; set; }
    }
}
