namespace Crm.Domain.Entities.DapperBodyModel
{
    public class SearchOrderBodyModel
    {
        public string FromDate { get; set; } = "-1";
        public string ToDate { get; set; } = "-1";
        public int CouponId { get; set; } = -1;
        public int StatusId { get; set; } = -1;
        public string CardType { get; set; } = "-1";
        public string Mobile { get; set; } = "-1";
        public string MobileType { get; set; } = "-1";
        public int Index { get; set; } = 0;
        public int Count { get; set; } = 50;
    }
}
