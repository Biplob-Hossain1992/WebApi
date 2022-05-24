using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.BodyModel.Permission;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<bool> AddPermission(MailSmsChargeBodyModel permission)
        {
            return await _permissionRepository.AddPermission(permission);
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetAllCourierUsers()
        {
            return await _permissionRepository.GetAllCourierUsers();
        }
        public async Task<IEnumerable<CourierUsers>> GetAllCourierUsersList()
        {
            return await _permissionRepository.GetAllCourierUsersList();
        }

        public async Task<SmsMailBodyModel> UpdateSmsEmail(int id, SmsMailBodyModel smsMailBodyModel)
        {
            return await _permissionRepository.UpdateSmsEmail(id, smsMailBodyModel);
        }

        public async Task<NotificationBodyModel> UpdateDeliveryBonduNotification(int id, NotificationBodyModel notificationBodyModel)
        {
            return await _permissionRepository.UpdateDeliveryBonduNotification(id, notificationBodyModel);
        }
    }
}
