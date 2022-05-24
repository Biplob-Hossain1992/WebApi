namespace AdCourier.Domain.Entities.ViewModel.CourierUsers
{
    public class AdminUsersViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public byte AdminType { get; set; }
        public string Passwrd { get; set; }
        public byte IsActive { get; set; }
        public string Token { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
    }
}
