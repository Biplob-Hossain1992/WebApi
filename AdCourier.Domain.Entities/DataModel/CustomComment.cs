using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CustomComment", Schema = "DT")]
    public class CustomComment
    {
        [Key]
        [Column("CustomCommentId")]
        public int CustomCommentId { get; set; }
        public string Comment { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public int PostedBy { get; set; }
        public int OrderId { get; set; }
        public string IsConfirmedBy { get; set; } = "admin";
        public int StatusId { get; set; }
    }
}
