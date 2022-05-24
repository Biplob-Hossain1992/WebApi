using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IFirebaseCloudService
    {
        Task<bool> SendNotificationDeliveryBondhu(string firebaseToken, CourierOrderStatus courierOrderStatus);
    }
}
