using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("StatusGroup", Schema = "DT")]
    public class StatusGroup
    {
        [Key]
        [Column("StatusGroupId")]
        public int StatusGroupId { get; set; }
        [MaxLength(150)]
        [Required]
        public string ReportStatusGroup { get; set; }
        [MaxLength(150)]
        [Required]
        public string FulfillmentStatusGroup { get; set; }
        [MaxLength(150)]
        [Required]
        public string OrderTrackStatusGroup { get; set; }
        [MaxLength(150)]
        //[Required]
        public string OrderTrackStatusPublicGroup { get; set; }
        [MaxLength(150)]
        public string DashboardStatusGroup { get; set; }
        public int DashboardSpanCount { get; set; }
        [MaxLength(50)]
        public string DashboardViewColorType { get; set; }
        public int DashboardViewOrderBy { get; set; }
        public string DashboardRouteUrl { get; set; }
        public string DashboardCountSumView { get; set; }
        public string DashboardStatusFilter { get; set; }
        public string DashboardImageUrl { get; set; }
        public string TrackingColor { get; set; }
        public decimal TrackingOrderBy { get; set; }
    }
}
