using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.Utility;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly AppSettings _appSettings;
        private readonly IJwtSecurityService _jwtSecurityService;
        public LoginService(ILoginRepository loginRepository, 
            IOptions<AppSettings> appSettings,
            IJwtSecurityService jwtSecurityService)
        {
            _loginRepository = loginRepository;
            _appSettings = appSettings.Value;
            _jwtSecurityService = jwtSecurityService;
        }

        public async Task<CourierUsers> CheckReferrerMobile(string referrerMobile)
        {
            return await _loginRepository.CheckReferrerMobile(referrerMobile);
        }
        public async Task<CourierUsers> GetCourierUsers(CourierUsers courierUsers)
        {
            return await _loginRepository.GetCourierUsers(courierUsers);
        }

        public async Task<AdminUsersViewModel> AdminUserLogin(Users user)
        {

            if (string.IsNullOrEmpty(user.Passwrd) || string.IsNullOrEmpty(user.UserName))
                return null;

            var data = await _loginRepository.AdminUserLogin(user);

            if (data == null)
                return null;


            var userToekn = _jwtSecurityService.GetTokenAdmin(data);

            return userToekn;
        }

        public async Task<LenderUserViewModel> LenderUserLogin(LenderUser lenderUser)
        {
            if (string.IsNullOrEmpty(lenderUser.Password) || string.IsNullOrEmpty(lenderUser.UserName))
                return null;

            var data = await _loginRepository.LenderUserLogin(lenderUser);

            if (data == null)
                return null;

            var userToken = _jwtSecurityService.GetTokenLoanLender(data);

            return userToken;
        }

        public async Task<CourierUsersViewModel> UserLogin(CourierUsers courierUsers)
        {

            if (string.IsNullOrEmpty(courierUsers.Mobile) || string.IsNullOrEmpty(courierUsers.Password))
                return null;

            var user =  await _loginRepository.UserLogin(courierUsers);
            user.FirebaseToken = courierUsers.FirebaseToken;

            if (user == null)
                return null;


            var userToekn = _jwtSecurityService.GetToken(user);

            return userToekn;
        }

        public async Task<Users> AddAdminUsers(Users user)
        {
            return await _loginRepository.AddAdminUsers(user);
        }

        public async Task<CourierUsers> UserRegister(CourierUsers courierUsers)
        {
            if (!string.IsNullOrEmpty(courierUsers.Referrer))
            {
                //var user = await _loginRepository.CheckReferrerMobile(courierUsers.Referrer);
                //var referrer = await _loginRepository.GetReferrer();
                var referee = await _loginRepository.GetReferee();

                courierUsers.RefereeOrder = referee.RefereeOrder;
                courierUsers.RefereeStartTime = DateTime.Now;
                courierUsers.RefereeEndTime = DateTime.Now.AddDays(referee.RefereeUseDays);
                courierUsers.OrderType = referee.OrderType;
            }

            //RemoveSpecialCharacters
            courierUsers.WebURL = SpecialCharacters.RemoveSpecialCharacters(courierUsers.WebURL);
            courierUsers.FBURL = SpecialCharacters.RemoveSpecialCharacters(courierUsers.FBURL);
            courierUsers.Password = SpecialCharacters.RemoveSpecialCharacters(courierUsers.Password);
            courierUsers.CompanyName = SpecialCharacters.RemoveSpecialCharacters(courierUsers.CompanyName);
            courierUsers.Address = SpecialCharacters.RemoveSpecialCharacters(courierUsers.Address);
            //RemoveSpecialCharacters


            //courierUsers.HashPassword = BCrypt.Net.BCrypt.HashPassword(courierUsers.Password);

            var data = await _loginRepository.UserRegister(courierUsers);
            if (data != null)
            {
                var referrerUpdate = await _loginRepository.UpdateReferrer(courierUsers.Referrer);
            }
            return data;
        }



        public async Task<CourierUsers> ResetPassword(CourierUsers courierUsers)
        {
            return await _loginRepository.ResetPassword(courierUsers);
        }

        public async Task<IEnumerable<Users>> GetAdUsers()
        {
            return await _loginRepository.GetAdUsers();
        }
        public async Task<IEnumerable<Users>> GetAdUsersByFilter(Users user)
        {
            return await _loginRepository.GetAdUsersByFilter(user);
        }
        public async Task<CourierUsers> UpdateBlockUser(int id, CourierUsers courierUsers)
        {
            return await _loginRepository.UpdateBlockUser(id, courierUsers);
        }
    }
}
