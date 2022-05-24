using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Services.Interfaces;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;

namespace AdCourier.Services
{
    public class JwtSecurityService : IJwtSecurityService
    {
        private readonly AppSettings _appSettings;
        private readonly SqlServerContext _sqlServerContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtSecurityService(IOptions<AppSettings> appSettings, SqlServerContext sqlServerContext)
        {
            _appSettings = appSettings.Value;
            _sqlServerContext = sqlServerContext;
        }

        public AdminUsersViewModel GetTokenAdmin(AdminUsersViewModel user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, "DeliveryTigerAdmin")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Passwrd = null;

            return user;
        }

        public LenderUserViewModel GetTokenLoanLender(LenderUserViewModel lenderUser)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, lenderUser.LenderUserId.ToString()),
                    new Claim(ClaimTypes.Role, "LoanLenderApplication")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            lenderUser.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            lenderUser.Password = null;

            return lenderUser;
        }

        public CourierUsersViewModel GetToken(CourierUsersViewModel user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.CourierUserId.ToString()),
                     new Claim(ClaimTypes.Role, "DeliveryTiger")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.RefreshToken = Guid.NewGuid().ToString();
            // remove password before returning
            user.Password = null;

            // update Refreshtoken
            var entity = _sqlServerContext.CourierUsers.FirstOrDefault(item => item.CourierUserId == user.CourierUserId);
            entity.Refreshtoken = user.RefreshToken;
            entity.FirebaseToken = user.FirebaseToken;
            _sqlServerContext.CourierUsers.Update(entity);
            _sqlServerContext.SaveChanges();

            return user;
        }

        public CourierUsersViewModel GetRefreshToken(string refreshToken)
        {
            var _refreshToken = _sqlServerContext.CourierUsers.SingleOrDefault(m => m.Refreshtoken == refreshToken.Trim());

            if (_refreshToken != null)
            {
                var user = new CourierUsersViewModel();

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, _refreshToken.CourierUserId.ToString()),
                     new Claim(ClaimTypes.Role, "DeliveryTiger")
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);
                user.RefreshToken = Guid.NewGuid().ToString();
                // remove password before returning
                user.CourierUserId = _refreshToken.CourierUserId;
                user.Password = null;
                user.UserName = _refreshToken.UserName;
                user.CompanyName = _refreshToken.CompanyName;
                user.Mobile = _refreshToken.Mobile;
                user.IsActive = _refreshToken.IsActive;
                user.Address = _refreshToken.Address;
                user.CollectionCharge = _refreshToken.CollectionCharge;
                user.ReturnCharge = _refreshToken.ReturnCharge;
                user.SmsCharge = _refreshToken.SmsCharge;
                user.MailCharge = _refreshToken.MailCharge;
                user.Sms = _refreshToken.IsSms;
                user.Email = _refreshToken.IsEmail;
                user.EmailAddress = _refreshToken.EmailAddress;
                user.BkashNumber = _refreshToken.BkashNumber;
                user.AlterMobile = _refreshToken.AlterMobile;
                user.MaxCodCharge = _refreshToken.MaxCodCharge;
                user.IsAutoProcess = _refreshToken.IsAutoProcess;

                // update Refreshtoken
                var entity = _sqlServerContext.CourierUsers.FirstOrDefault(item => item.CourierUserId == _refreshToken.CourierUserId);
                entity.Refreshtoken = user.RefreshToken;
                _sqlServerContext.CourierUsers.Update(entity);
                _sqlServerContext.SaveChanges();

                return user;
            }

            return null;
        }
    }
}
