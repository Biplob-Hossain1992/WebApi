using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class OtherService
    {

        public async Task<bool> UpdateComplainStatus(string orderId)
        {

            var orderCode = orderId.Split('-');

            string url = "https://adm.ajkerdeal.com/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var methodurl = url + "api/Complain/IsComplainExist/" + orderCode[1] + "/dt/" + "পার্সেল এখনো কালেকশন হয় নাই";
            var responseMessage = await client.GetAsync(methodurl);

            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> DownloadPoh(string OrderCode)
        {
            var requestModel = new 
            {
                OrderCode = OrderCode
            };
            var json = JsonConvert.SerializeObject(requestModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            string url = "https://adm.ajkerdeal.com/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var methodurl = url + "api/account/reports/DownloadPoh";
            var responseMessage = await client.PostAsync(methodurl, data);

            var result = await responseMessage.Content.ReadAsStringAsync();

            var finalResponse = JsonConvert.DeserializeObject<DownloadPohModel>(result);


            if (responseMessage.IsSuccessStatusCode && finalResponse.Success.Equals(1))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> BlockCodCodesForPoh(int MerchantId)
        {
            var requestModel = new
            {
                MerchantId = MerchantId
            };
            var json = JsonConvert.SerializeObject(requestModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            string url = "https://adm.ajkerdeal.com/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var methodurl = url + "api/transaction/BlockCodCodesForPoh";
            var responseMessage = await client.PostAsync(methodurl, data);

            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> AutoDownload(string orderId)
        {
            var requestModel = new
            {
                CourierOrderId = orderId,
                UserID = 3026,
                IsConfirmedBy = "admin"
            };

            var json = JsonConvert.SerializeObject(requestModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            string url = "https://adm.ajkerdeal.com/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var methodUrl = url + "api/transaction/SyncronizeAccountingAndMIS";
            var responseMessage = await client.PostAsync(methodUrl, data);

            if (responseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class DownloadPohModel
    {
        public int Success { get; set; }
    }
}
