using AdCourier.Domain.Entities.BodyModel.Permission;
using AdCourier.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class SmsEmailService : ISmsEmailService
    {
        public async Task<dynamic> SendSMSInfobip(InfobipSMSBodyModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://6jnlrz.api.infobip.com/");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "App 9ee985c0a29b8325a5e9550498c17cdf-d0eae13e-6316-48fb-8179-90149807af80");

                var httpResponse = await client.PostAsync("sms/2/text/advanced", content);
                var result = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<dynamic>(result);

                return response;
            }

        }

        public async Task<bool> SmsSend(dynamic listOfData)
        {

            var jsonString = JsonConvert.SerializeObject(listOfData);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            string url = "https://bridge.ajkerdeal.com";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("API_KEY", "Ajkerdeal_~La?Rj73FcLm");
            string methodUrl = url + "/SmsComunication/SendSms";
            HttpResponseMessage response = await client.PostAsync(methodUrl, content);

            client.Dispose();
            if (response.IsSuccessStatusCode)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
    }
}
