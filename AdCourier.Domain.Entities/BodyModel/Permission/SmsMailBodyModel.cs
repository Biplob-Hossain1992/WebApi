namespace AdCourier.Domain.Entities.BodyModel.Permission
{
    public class SmsMailBodyModel
    {
        public int StatusId { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMessage { get; set; }
        public string RetentionMessage { get; set; }
    }
}
