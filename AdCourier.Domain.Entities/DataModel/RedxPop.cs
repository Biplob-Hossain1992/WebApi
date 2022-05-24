using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("RedxPop", Schema = "DT")]
    public class RedxPop
    {
        [Key]
        [Column("PopId")]
        public int PopId { get; set; }
        public string tracking_id { get; set; }
        public string id { get; set; }
        public string created_at { get; set; }
        public string receive_date { get; set; }
        public string area { get; set; }
        public string zone_name { get; set; }
        public decimal? cash { get; set; }
        public string current_status { get; set; }
        public int? entry_count { get; set; }
        public decimal? shopup_charge { get; set; }
        public decimal? shopup_return_charge { get; set; }
        public decimal? baki_charge { get; set; }
        public decimal? baki_payable { get; set; }
    }
}
