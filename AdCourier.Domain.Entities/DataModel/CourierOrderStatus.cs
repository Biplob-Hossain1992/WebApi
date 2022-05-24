using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("CourierOrderStatus", Schema = "DT")]
    public class CourierOrderStatus
    {
        [Key]
        [Column("StatusId")]
        public int StatusId { set; get; }
        [Required]
        [StringLength(255, ErrorMessage = "Do not enter more than 255 characters")]
        public string StatusNameEng { set; get; } = "";
        [Required]
        [StringLength(255, ErrorMessage = "Do not enter more than 255 characters")]
        public string StatusNameBng { set; get; } = "";
        [Required]
        [StringLength(150, ErrorMessage = "Do not enter more than 150 characters")]
        public string StatusGroup { set; get; } = "";
        [Required]
        [StringLength(150, ErrorMessage = "Do not enter more than 150 characters")]
        public string FulfillmentStatusGroup { set; get; } = "";
        [Required]
        [StringLength(150, ErrorMessage = "Do not enter more than 150 characters")]
        public string OrderTrackStatusGroup { get; set; } = "";

        //[Required]
        [StringLength(150, ErrorMessage = "Do not enter more than 150 characters")]
        public string OrderTrackStatusPublicGroup { get; set; } = "";

        [StringLength(150, ErrorMessage = "Do not enter more than 150 characters")]
        public string DashboardStatusGroup { get; set; } = "";

        [Required]
        [StringLength(150, ErrorMessage = "Do not enter more than 30 characters")]
        public string StatusType { get; set; } = "";
        public bool IsActive { set; get; } = true;
        public DateTime PostedOn { set; get; } = DateTime.Now;
        public string Message { get; set; } = "";
        public string Email { get; set; } = "";
        public string CustomerMessage { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string RetentionMessage { get; set; } = "";
        public int NotificationType { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageLink { get; set; } = "";
        public string BigText { get; set; } = "";
        public string ServiceType { get; set; } = "";
        public bool IsActiveNotification { get; set; } = false;
    }
}
