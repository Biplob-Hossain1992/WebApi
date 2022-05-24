using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("JsonData", Schema = "Dana")]
    public class JsonData
    {
        [Key]
        public int id { get; set; }
        [Column(TypeName = "nvarchar")]
        public string key { get; set; }
        [Column(TypeName = "nvarchar")]
        public string value { get; set; }
        public int type { get; set; }
        public string endpoint { get; set; }
    }
}
