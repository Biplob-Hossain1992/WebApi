namespace Crm.Domain.Entities.DapperViewModel.DatabaseViewModel
{
    public class AdOrderStatusViewModel
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string OrderStatus { get; set; }
        public bool IsActive { get; set; }
        public int? Model { get; set; }
    }
}
