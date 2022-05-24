namespace AdCourier.Domain.Entities.ViewModel.DeliverManAssign
{
    public class DeliveryManAssignViewModel
    {
        public int Id { get; set; }
        public int CourierUserId { get; set; }
        public int DeliveryManUserId { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string CourierUserName { get; set; }
        public string AssignType { get; set; }
        public string CourierUserMobile { get; set; }
    }
}
