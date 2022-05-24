using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("DbActionBn", Schema = "DT")]
    public class DbActionBn
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public int ActionType { get; set; }
        public string ActionMessage { get; set; }
        public int UpdateStatus { get; set; }
        public string StatusMessage { get; set; }
        public string ColorCode { get; set; }
        public string Icon { set; get; } = "";
        public int IsPaymentType { set; get; } = 0;
        public string ProjectType { get; set; } = "";
        public string GroupType { get; set; }
        public string StatusNote { get; set; }
    }
}
