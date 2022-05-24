using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("DeliveryChargeDetails_test", Schema = "DT")]
    public class DeliveryChargeDetails_test
    {
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public int WeightRangeId { get; set; }
        public int DeliveryRangeId { get; set; }
        public decimal CourierDeliveryCharge { get; set; }
        public bool IsOpenBox { get; set; }
        //public decimal CityDeliveryCharge { get; set; } = 0;
        public int CourierId { get; set; }
    }
}
