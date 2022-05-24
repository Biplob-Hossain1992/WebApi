using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("SubCategory", Schema = "DT")]
    public class SubCategory
    {
        [Key]
        [Column("SubCategoryId")]
        public int SubCategoryId { get; set; }
        public string SubCategoryNameEng { get; set; } = "";
        public string SubCategoryNameBng { get; set; } = "";
        public bool? IsActive { get; set; }
        public int? CategoryId { get; set; } = 0;
    }
}
