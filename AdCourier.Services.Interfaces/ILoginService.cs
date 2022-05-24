using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface ILoginService
    {
        Task<CourierUsersViewModel> UserLogin(CourierUsers courierUsers);
        Task<Users> AddAdminUsers(Users user);
        Task<CourierUsers> CheckReferrerMobile(string referrerMobile);
        Task<CourierUsers> GetCourierUsers(CourierUsers courierUsers);
        Task<CourierUsers> UserRegister(CourierUsers courierUsers);
        Task<CourierUsers> ResetPassword(CourierUsers courierUsers);
        Task<AdminUsersViewModel> AdminUserLogin(Users user);
        Task<LenderUserViewModel> LenderUserLogin(LenderUser lenderUser);
        Task<CourierUsers> UpdateBlockUser(int id, CourierUsers courierUsers);
        Task<IEnumerable<Users>> GetAdUsers();
        Task<IEnumerable<Users>> GetAdUsersByFilter(Users user);
    }
}
