using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Couriers", Schema = "Deal")]
    public class Couriers
    {
        [Key]
        public int CourierId { set; get; }
        public string CourierName { set; get; }
        public string ContactNo { set; get; }
        public string ContactAddress { set; get; }
        public bool IsActive { get; set; } = true;
        public int PostedBy { get;set;} = 0;
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public int UpdatedBy { get; set; } = 0;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsDeleted { get; set; } = false;
        public bool IsPresent { get; set; } = true;
        public bool IsOwnTiger { get; set; } = true;
    }
}
