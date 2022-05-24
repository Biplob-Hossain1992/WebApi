using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AdCourier.Domain.Entities.DataModel
{
    [Table("AcquisitionLeadManagement", Schema = "DT")]
    public class AcquisitionLeadManagement
    {
        [Key]
        [Column("AcquisitionLeadManagementId")]

        public int AcquisitionLeadManagementId { get; set; }
        public string Mobile { get; set; }
        public string CompanyName { get; set; }
        public int AcquisitionUserId { get; set; }
        public DateTime AcquiredDate { get; set; } = DateTime.Now;
    }
}
