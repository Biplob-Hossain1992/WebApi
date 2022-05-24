using AdCourier.Context;
using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.BodyModel.Permission;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public PermissionRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<bool> AddPermission(MailSmsChargeBodyModel mailSmsChargeBodyModel)
        {

            var entityDelete = await _sqlServerContext.Permission.Where(item => item.MerchantId == mailSmsChargeBodyModel.MerchantId).ToListAsync();

            if (entityDelete.Count > 0)
            {
                _sqlServerContext.Permission.RemoveRange(entityDelete);
            }

            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == mailSmsChargeBodyModel.MerchantId);
            if (entity != null)
            {
                entity.IsCustomerSms = mailSmsChargeBodyModel.permissionModel.CustomerSms;
                entity.IsCustomerEmail = mailSmsChargeBodyModel.permissionModel.CustomerEmail;
                entity.IsSms = mailSmsChargeBodyModel.permissionModel.Sms;
                entity.IsEmail = mailSmsChargeBodyModel.permissionModel.Email;
                entity.SmsCharge = mailSmsChargeBodyModel.SmsCharge;
                entity.MailCharge = mailSmsChargeBodyModel.MailCharge;
                entity.ReturnCharge = mailSmsChargeBodyModel.ReturnCharge;
                entity.CollectionCharge = mailSmsChargeBodyModel.CollectionCharge;
                // Update entity in DbSet
                _sqlServerContext.CourierUsers.Update(entity);
            }

            if (!(mailSmsChargeBodyModel.permissionModel.StatusId.Contains(-1)))
            {
                List<Permission> permissionList = new List<Permission>();

                foreach (var statusId in mailSmsChargeBodyModel.permissionModel.StatusId)
                {
                    var permission = new Permission();
                    permission.MerchantId = mailSmsChargeBodyModel.permissionModel.MerchantId;
                    permission.Email = mailSmsChargeBodyModel.permissionModel.Email;
                    permission.Sms = mailSmsChargeBodyModel.permissionModel.Sms;
                    permission.StatusId = statusId;
                    permission.PermissionType = mailSmsChargeBodyModel.permissionModel.PermissionType;
                    permissionList.Add(permission);
                }
                await _sqlServerContext.Permission.AddRangeAsync(permissionList);
            }

            

            await _sqlServerContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CourierUsers>> GetAllCourierUsersList()
        {
            var courierUserIds = new int[] { 1, 30038 };

            IQueryable<CourierUsers> data =  _sqlServerContext.CourierUsers.Where(s => courierUserIds.Contains(s.CourierUserId)).Select(x => new CourierUsers {
                CourierUserId =  x.CourierUserId,
                CompanyName = x.CompanyName,
                Address = x.Address
            });
            return await data.ToListAsync();
        }
        public async Task<IEnumerable<CourierUsersViewModel>> GetAllCourierUsers()
        {

                var permission = await _sqlServerContext.Permission.ToListAsync();

            //var result = await (from courierUsers in _sqlServerContext.CourierUsers
            IQueryable<CourierUsersViewModel> result = from courierUsers in _sqlServerContext.CourierUsers

                                let smss = _sqlServerContext.Permission.Where(s => s.MerchantId.Equals(courierUsers.CourierUserId)).Select(w => w.Sms).Count()
                                    let email = _sqlServerContext.Permission.Where(s => s.MerchantId.Equals(courierUsers.CourierUserId)).Select(w => w.Email).Count()
                                    let statusIds = permission.Where(s => s.MerchantId.Equals(courierUsers.CourierUserId)).Select(w => w.StatusId).ToArray()

                                    select new CourierUsersViewModel
                                    {
                                        Address = courierUsers.Address,
                                        CourierUserId = courierUsers.CourierUserId,
                                        UserName = courierUsers.UserName,
                                        CompanyName = courierUsers.CompanyName,
                                        Mobile = courierUsers.Mobile,
                                        CollectionCharge = courierUsers.CollectionCharge,
                                        ReturnCharge = courierUsers.ReturnCharge,
                                        SmsCharge = courierUsers.SmsCharge,
                                        MailCharge = courierUsers.MailCharge,
                                        Sms = smss > 0 ? true : false,
                                        Email = email > 0 ? true : false,
                                        StatusId = statusIds
                                    };
                return await result.ToListAsync();
        }

        public async Task<SmsMailBodyModel> UpdateSmsEmail(int id, SmsMailBodyModel smsMailBodyModel)
        {
            var entity = await _sqlServerContext.CourierOrderStatus.FirstOrDefaultAsync(item => item.StatusId == id);
            if (entity != null)
            {
                if (entity.Message != smsMailBodyModel.Message)
                {
                    entity.Message = smsMailBodyModel.Message;
                }
                if (entity.Email != smsMailBodyModel.Email)
                {
                    entity.Email = smsMailBodyModel.Email;
                }

                if (entity.CustomerEmail != smsMailBodyModel.CustomerEmail)
                {
                    entity.CustomerEmail = smsMailBodyModel.CustomerEmail;
                }
                if (entity.CustomerMessage != smsMailBodyModel.CustomerMessage)
                {
                    entity.CustomerMessage = smsMailBodyModel.CustomerMessage;
                }

                if (entity.RetentionMessage != smsMailBodyModel.RetentionMessage)
                {
                    entity.RetentionMessage = smsMailBodyModel.RetentionMessage;
                }

                // Update entity in DbSet
                _sqlServerContext.CourierOrderStatus.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return smsMailBodyModel;
        }

        public async Task<NotificationBodyModel> UpdateDeliveryBonduNotification(int id, NotificationBodyModel notificationBodyModel)
        {
            var entity = await _sqlServerContext.CourierOrderStatus.FirstOrDefaultAsync(item => item.StatusId == id);
            if (entity != null)
            {
                if (entity.Title != notificationBodyModel.Title)
                {
                    entity.Title = notificationBodyModel.Title;
                }
                if (entity.NotificationType != notificationBodyModel.NotificationType)
                {
                    entity.NotificationType = notificationBodyModel.NotificationType;
                }

                if (entity.BigText != notificationBodyModel.BigText)
                {
                    entity.BigText = notificationBodyModel.BigText;
                }
                if (entity.ServiceType != notificationBodyModel.ServiceType)
                {
                    entity.ServiceType = notificationBodyModel.ServiceType;
                }

                if (entity.Description != notificationBodyModel.Description)
                {
                    entity.Description = notificationBodyModel.Description;
                }
                if (entity.ImageLink != notificationBodyModel.ImageLink)
                {
                    entity.ImageLink = notificationBodyModel.ImageLink;
                }
                if (entity.IsActiveNotification != notificationBodyModel.IsActiveNotification)
                {
                    entity.IsActiveNotification = notificationBodyModel.IsActiveNotification;
                }
                _sqlServerContext.CourierOrderStatus.Update(entity);
                await _sqlServerContext.SaveChangesAsync();
            }

            return notificationBodyModel;
        }
    }
}
