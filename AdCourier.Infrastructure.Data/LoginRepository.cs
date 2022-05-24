using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using System.Collections.Generic;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;

namespace AdCourier.Infrastructure.Data
{
    public class LoginRepository : ILoginRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public LoginRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<CourierUsers> CheckReferrerMobile(string referrerMobile)
        {
            IQueryable<CourierUsers> response = from c in _sqlServerContext.CourierUsers
                                                where c.Mobile.Equals(referrerMobile.Trim())
                                                && c.IsActive == true
                                                select new CourierUsers
                                                {
                                                    CourierUserId = c.CourierUserId,
                                                    CompanyName = c.CompanyName,
                                                    Mobile = c.Mobile,
                                                    ReferrerStartTime = c.ReferrerStartTime,
                                                    RefereeEndTime = c.RefereeEndTime
                                                };
            return await response.FirstOrDefaultAsync();
        }
        public async Task<CourierUsers> GetCourierUsers(CourierUsers courierUsers)
        {
            IQueryable<CourierUsers> response = from c in _sqlServerContext.CourierUsers
                                  where c.Mobile.Equals(courierUsers.Mobile.Trim())
                                  && c.IsActive == true
                                  select c;
            return await response.FirstOrDefaultAsync();
        }

        public async Task<CourierUsers> ResetPassword(CourierUsers courierUsers)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.Mobile == courierUsers.Mobile.Trim());

            if (entity != null)
            {
                entity.Password = courierUsers.Password.Trim();
                // Update entity in DbSet
                _sqlServerContext.CourierUsers.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<CourierUsers> UpdateReferrer(string referrerMobile)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.Mobile == referrerMobile.Trim());

            if (entity != null)
            {
                var referrer = _sqlServerContext.Referrer.FirstOrDefault();

                entity.ReferrerOrder = referrer.ReferrerOrder + entity.ReferrerOrder;
                entity.ReferrerStartTime = ReferrerStartTimeCalculation(entity);
                entity.ReferrerEndTime = ReferrerEndTimeCalculation(entity, referrer);
                // Update entity in DbSet
                _sqlServerContext.CourierUsers.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }
            return entity;
        }

        private DateTime? ReferrerStartTimeCalculation(CourierUsers user)
        {
            if (!user.ReferrerStartTime.HasValue)
            {
                return DateTime.Now;
            }
            else
            {
                return user.ReferrerStartTime;
            }
        }

        private DateTime? ReferrerEndTimeCalculation(CourierUsers user, Referrer referrer)
        {
            if (!user.RefereeEndTime.HasValue)
            {
                return DateTime.Now.AddDays(referrer.ReferrerUseDays);
            }
            else
            {
                return user.RefereeEndTime;
            }
        }

        public async Task<AdminUsersViewModel> AdminUserLogin(Users user)
        {

            IQueryable<AdminUsersViewModel> response = from c in _sqlServerContext.Users
                                  where c.UserName.Equals(user.UserName)
                                  && c.Passwrd.Equals(user.Passwrd)
                                  && c.IsActive == 1
                                  select new AdminUsersViewModel
                                  {
                                      UserId = c.UserId,
                                      FullName = c.FullName,
                                      IsActive = c.IsActive,
                                      UserName = c.UserName,
                                      Passwrd = c.Passwrd,
                                      AdminType = c.AdminType,
                                      Mobile = c.Mobile
                                  };
            return await response.FirstOrDefaultAsync();

        }

        public async Task<LenderUserViewModel> LenderUserLogin(LenderUser lenderUser)
        {
            var loginResponse = await _sqlServerContext.LenderUser
                                .Where(l => l.UserName.Equals(lenderUser.UserName)
                                && l.Password.Equals(lenderUser.Password))
                                .Select(s => new LenderUserViewModel
                                {
                                    LenderUserId = s.LenderUserId,
                                    UserName = s.UserName,
                                    LenderName = s.LenderName,
                                    Mobile = s.Mobile,
                                    Password = s.Password,
                                    RoleName = s.RoleName
                                }).FirstOrDefaultAsync();

            return loginResponse;
        }

        public async Task<CourierUsersViewModel> UserLogin(CourierUsers courierUsers)
        {

            IQueryable<CourierUsersViewModel> response = from c in _sqlServerContext.CourierUsers
                                                         join _usersTemp in _sqlServerContext.Users.AsNoTracking() on c.RetentionUserId equals _usersTemp.UserId into _adminUsers
                                                         from _users in _adminUsers.DefaultIfEmpty()
                                                         where c.Mobile.Equals(courierUsers.Mobile)
                                                         && c.Password.Equals(courierUsers.Password)
                                                         && c.IsActive == true
                                                         //let pickUpLocations = _sqlServerContext.PickupLocations.Where(x => x.CourierUserId.Equals(c.CourierUserId)).AsQueryable()
                                                         //let districtName = pickUpLocations.Count() > 0 && pickUpLocations.FirstOrDefault().DistrictId != 0 ? _sqlServerContext.Districts.Where(x => x.DistrictId.Equals(pickUpLocations.FirstOrDefault().DistrictId)).FirstOrDefault().District : ""
                                                         //let thanaName = pickUpLocations.Count() > 0 && pickUpLocations.FirstOrDefault().ThanaId != 0 ? _sqlServerContext.Districts.Where(x => x.DistrictId.Equals(pickUpLocations.FirstOrDefault().ThanaId)).FirstOrDefault().District : ""
                                                         //let areaName = pickUpLocations.Count() > 0 && pickUpLocations.FirstOrDefault().AreaId != 0 ? _sqlServerContext.Districts.Where(x => x.DistrictId.Equals(pickUpLocations.FirstOrDefault().AreaId)).FirstOrDefault().District : ""

                                                         select new CourierUsersViewModel
                                                         {
                                                             CourierUserId = c.CourierUserId,
                                                             Address = c.Address,
                                                             IsActive = c.IsActive,
                                                             IsBlock = c.IsBlock,
                                                             UserName = c.UserName,
                                                             CompanyName = c.CompanyName,
                                                             Mobile = c.Mobile,
                                                             Password = c.Password,
                                                             Sms = c.IsSms,
                                                             Email = c.IsEmail,
                                                             CollectionCharge = c.CollectionCharge,
                                                             ReturnCharge = c.ReturnCharge,
                                                             SmsCharge = c.SmsCharge,
                                                             MailCharge = c.MailCharge,
                                                             EmailAddress = c.EmailAddress,
                                                             BkashNumber = c.BkashNumber,
                                                             AlterMobile = c.AlterMobile,
                                                             MaxCodCharge = c.MaxCodCharge,
                                                             IsAutoProcess = c.IsAutoProcess,
                                                             Credit = c.Credit,
                                                             FBURL = c.FBURL,
                                                             WebURL = c.WebURL,
                                                             IsInstaCod = c.IsInstaCod,
                                                             AdminUsers = new AdminUsersViewModel
                                                             {
                                                                 UserId = _users.UserId == null ? 0 : _users.UserId,
                                                                 FullName = _users.UserName == null ? "" : _users.FullName,
                                                                 Mobile = _users.Mobile == null ? "" : _users.Mobile
                                                             },
                                                             //DistrictId = pickUpLocations.Count() > 0 ? pickUpLocations.FirstOrDefault().DistrictId : 0,
                                                             //ThanaId = pickUpLocations.Count() > 0 ? pickUpLocations.FirstOrDefault().ThanaId : 0,
                                                             //AreaId = pickUpLocations.Count() > 0 ? pickUpLocations.FirstOrDefault().AreaId : 0,
                                                             //DistrictName = districtName,
                                                             //ThanaName = thanaName,
                                                             //AreaName = areaName,
                                                             //PickupLocationList = pickUpLocations.ToList()
                                                         };

            return await response.FirstOrDefaultAsync();

        }

        public async Task<Users> AddAdminUsers(Users users)
        {
            await _sqlServerContext.Users.AddAsync(users);
            await _sqlServerContext.SaveChangesAsync();
            return users;
        }

        public async Task<CourierUsers> UserRegister(CourierUsers courierUsers)
        {
            await _sqlServerContext.CourierUsers.AddAsync(courierUsers);
            await _sqlServerContext.SaveChangesAsync();
            return courierUsers;
        }

        public async Task<IEnumerable<Users>> GetAdUsers()
        {
            IQueryable<Users> data = from w in _sqlServerContext.Users.AsNoTracking()
                                     where w.IsActive == 1
                                     && w.AdminType == 4
                                     select w;
            return await data.ToListAsync();
        }

        public async Task<IEnumerable<Users>> GetAdUsersByFilter(Users user)
        {
            if(user.AdminType != 255)
            {
                var data = from u in _sqlServerContext.Users.AsNoTracking()
                       where u.IsActive == 1
                       && u.AdminType == user.AdminType
                       select u;

                return await data.ToListAsync();
            }
            else if (user.UserName != "")
            {
                var data = _sqlServerContext.Users.Where(u => u.UserName.Contains(user.UserName) && u.IsActive == 1);
                return await data.ToListAsync();
            }
            else if (user.AdminType.Equals(255)) //this condition applied for GetAll Users (used in adadminapp)
            {
                var data = _sqlServerContext.Users.Where(u => u.IsActive.Equals(1));
                return await data.ToListAsync();
            }
            return null;
        }

        public async Task<Referrer> GetReferrer()
        {
            IQueryable<Referrer> data = from w in _sqlServerContext.Referrer.AsNoTracking()
                                        where w.IsActive.Equals(true)
                                        select w;
            return await data.FirstOrDefaultAsync();
        }

        public async Task<Referee> GetReferee()
        {
            IQueryable<Referee> data = from w in _sqlServerContext.Referee.AsNoTracking()
                                        where w.IsActive.Equals(true)
                                        select w;
            return await data.FirstOrDefaultAsync();
        }

        public async Task<CourierUsers> UpdateBlockUser(int id, CourierUsers courierUsers)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == id);
            if (entity != null)
            {
                entity.IsBlock = courierUsers.IsBlock;
                entity.BlockReason = courierUsers.BlockReason;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return entity;
            }
            return null;
        }
    }
}
