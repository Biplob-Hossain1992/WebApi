
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CourierOrderStatusHistory", Schema = "DT")]
    public class CourierOrderStatusHistory
    {
        [Key]
        [Column("Id")]
        public int Id { set; get; } = 0;
        public string CourierOrderId { set; get; }
        public string IsConfirmedBy { set; get; } = "";
        public DateTime OrderDate { set; get; } = DateTime.Now;
        public int Status { set; get; } = 0;
        public DateTime PostedOn { set; get; } = DateTime.Now;
        public int PostedBy { set; get; } = 0;
        public int MerchantId { set; get; }
        public string Comment { set; get; } = "";
        public string PodNumber { set; get; } = "";
        public int CourierId { set; get; } = 0;
        public string HubName { get; set; } = "";
        public string CourierDeliveryManName { get; set; } = "";
        public string CourierDeliveryManMobile { get; set; } = "";
    }
}
