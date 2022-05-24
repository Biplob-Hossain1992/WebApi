using AdCourier.Domain.Entities.DataModel;
using AdCourier.Services.Interfaces;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class FirebaseCloudService : IFirebaseCloudService
    {
        private static readonly Uri FcmUri = new Uri(uriString: @"https://fcm.googleapis.com", uriKind: UriKind.Absolute);

        private const string FcmApiKey = "AIzaSyCOwstSW34mzTq9HH76YoM_78Rw2FrLNTM";

        public async Task<bool> SendNotificationDeliveryBondhu(string firebaseToken, CourierOrderStatus courierOrderStatus)
        {

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = FcmUri;
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + FcmApiKey);

                var response = await httpClient.PostAsJsonAsync(@"/fcm/send",
                    new
                    {
                        to = firebaseToken,
                        data = new
                        {
                            notificationType = courierOrderStatus.NotificationType,
                            title = courierOrderStatus.Title,
                            description = courierOrderStatus.Description,
                            imageLink = courierOrderStatus.ImageLink,
                            bigText = courierOrderStatus.BigText,
                            serviceType = courierOrderStatus.ServiceType,
                            body = courierOrderStatus.Description
                        }

                    });
                if (response.IsSuccessStatusCode)
                {
                    return response.IsSuccessStatusCode;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
