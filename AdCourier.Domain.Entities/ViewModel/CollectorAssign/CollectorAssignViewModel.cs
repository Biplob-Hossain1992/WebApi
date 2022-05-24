namespace AdCourier.Domain.Entities.ViewModel.CollectorAssign
{
    public class CollectorAssignViewModel
    {
        public int CollectorAssignId { get; set; }
        public int CourierUserId { get; set; }
        public int CollectorId { get; set; }
        public string CollectorName { get; set; }
        public string CompanyName { get; set; }
        public string CourierUserName { get; set; }
        public string AssignType { get; set; }
        public string CourierUserMobile { get; set; }
    }

    public class DeliveryZoneInfo
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        public string DistrictBng { get; set; }
        public int ThanaId { get; set; }
        public string Thana { get; set; }
        public string ThanaBng { get; set; }
        public int AreaId { get; set; }
        public string Area { get; set; }
        public string AreaBng { get; set; }
        public int ParentId { get; set; }
    }
    
}
