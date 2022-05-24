using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IJwtSecurityService
    {
        CourierUsersViewModel GetToken(CourierUsersViewModel courierUsers);

        AdminUsersViewModel GetTokenAdmin(AdminUsersViewModel courierUsers);

        LenderUserViewModel GetTokenLoanLender(LenderUserViewModel lenderUser);

        CourierUsersViewModel GetRefreshToken(string refreshToken);
    }
}
