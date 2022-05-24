using System.ComponentModel.DataAnnotations;

namespace AdCourier.Domain.Entities.ProcedureDataModel
{
    public class MerchantOrder
    {
        [Key]
        public int CourierUserId { get; set; }
        public string CompanyName { get; set; }
        public int Received { get; set; }
        public int Shipment { get; set; }
        public int Delivered { get; set; }
        public int PaymentGiven { get; set; }
        public decimal TakaCollection { get; set; }
    }
}
