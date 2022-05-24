using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Order_status_History", Schema = "Deal")]
    public class OrderStatusHistory
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int? CouponId { get; set; }
        public int? OrderStatus { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public int? ConfirmedBy { get; set; }
        public int? MerchantId { get; set; }
        public int? DealId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime DeliveryConfirmationDate { get; set; }
        public int? IsConfirmedByMerchant { get; set; }
        public int? UpdateFrom { get; set; }
        public string Comments { get; set; }
        public string PODnumber { get; set; }
        public string HubName { get; set; }
    }
}
