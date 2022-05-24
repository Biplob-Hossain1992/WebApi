using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("PaymentReferenceDetails", Schema = "DT")]
    public class PaymentReferenceDetails
    {
        [Key]
        [Column("PaymentReferenceDetailsId")]
        public int PaymentReferenceDetailsId { get; set; }
        public int OrderId { get; set; }
        public int Status { get; set; }
        public string PodNumber { get; set; } = "";
        public int PaymentReferenceId { get; set; }
        public int CourierId { get; set; }
        public string PaymentFrom { get; set; } = "";
        public int PostedBy { get; set; } = 0;
        public string AdminType { get; set; } = "";
        public decimal CollectionAmount { get; set; } = 0;
        public string Type { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
    }
}
