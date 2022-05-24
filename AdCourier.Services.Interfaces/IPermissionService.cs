using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.BodyModel.Permission;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> AddPermission(MailSmsChargeBodyModel permission);
        Task<IEnumerable<CourierUsersViewModel>> GetAllCourierUsers();
        Task<IEnumerable<CourierUsers>> GetAllCourierUsersList();
        Task<SmsMailBodyModel> UpdateSmsEmail(int id, SmsMailBodyModel smsMailBodyModel);
        Task<NotificationBodyModel> UpdateDeliveryBonduNotification(int id, NotificationBodyModel notificationBodyModel);
        
    }
}
