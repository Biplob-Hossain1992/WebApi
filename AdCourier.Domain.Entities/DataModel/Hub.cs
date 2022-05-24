using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Hub", Schema = "DT")]
    public class Hub
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public string HubAddress { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string HubMobile { get; set; }
        public int RedxPickUpStoreId { get; set; }
    }
}
