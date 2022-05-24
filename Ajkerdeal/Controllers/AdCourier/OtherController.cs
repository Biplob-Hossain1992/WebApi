using AdCourier.Domain.Entities.BodyModel.Complain;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.Other;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static AdCourier.Domain.Entities.ViewModel.Other.DistrictResult;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Route("api/[controller]")]
    //[Authorize]
    public class OtherController : Controller
    {

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllDistrictFromApi/{id}")]
        public async Task<IActionResult> GetAllDistrictFromApi(int id)
        {
            try
            {

                string url = "https://api.ajkerdeal.com";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(
                           new MediaTypeWithQualityHeaderValue("application/json"));
                string methodUrl = url + "/District/v3/LoadAllDistrictFromJson/" + id;
                var responseMessage = await client.GetAsync(methodUrl);
                var res = await responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Result>(res);

                // temporary solutions for covid 19 virus
                //if (id == 0)
                //{
                //    result.Data.DistrictInfo.RemoveRange(1, 68);
                //}
                
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllMerchantFromApi/{id}")]
        public async Task<IActionResult> GetAllMerchantFromApi(int id)
        {
            try
            {
                var response = new SingleResponseModel<AdvanceBalanceResult>();
                string url = "https://adm.ajkerdeal.com/";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                string methodurl = url + "api/account/reports/GetMerchantCurrentAdvanceBalance/" + id;
                var responseMessage = await client.GetAsync(methodurl);
                var stringresponse = await responseMessage.Content.ReadAsStringAsync();
                response.Model = JsonConvert.DeserializeObject<AdvanceBalanceResult>(stringresponse);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllDeliveredOrdersAccountingFromApi/{id}")]
        public async Task<IActionResult> GetAllDeliveredOrdersAccountingFromApi(int id)
        {
            var response = new SingleResponseModel<DtMerchantPayableDetailsViewModel>();
            string url = "https://adm.ajkerdeal.com";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var methodurl = url + "/api/account/reports/GetDtAllDeliveredOrdersAccounting/" + id;
            var responseMessage = await client.GetAsync(methodurl);
            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            response.Model = JsonConvert.DeserializeObject<DtMerchantPayableDetailsViewModel>(stringResponse);
            return Ok(response);
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetDTMerchantInstantPaymentStatus/{id}")]
        public async Task<IActionResult> GetDTMerchantInstantPaymentStatus(int id)
        {
            var response = new SingleResponseModel<DTMerchantInstantPaymentStatus>();
            string url = "https://adm.ajkerdeal.com";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var methodurl = url + "/api/account/reports/GetDTMerchantInstantPaymentStatus/" + id;
            var responseMessage = await client.GetAsync(methodurl);
            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            response.Model = JsonConvert.DeserializeObject<DTMerchantInstantPaymentStatus>(stringResponse);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("InsertDTComplain")]
        public async Task<IActionResult> InsertDTComplain([FromBody]DTComplainViewModel dTComplainViewModel)
        {
            var response = new SingleResponseModel<int>();
            var json = JsonConvert.SerializeObject(dTComplainViewModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            string url = "https://adm.ajkerdeal.com"; //"http://localhost:15332";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var methodurl = url + "/api/ComplainInsert/InsertDTComplain";
            var responseMessage = await client.PostAsync(methodurl, data);
            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            response.Model = Convert.ToInt32(stringResponse);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("MerchantPoHEligibilityCheck/{id}")]
        public async Task<IActionResult> MerchantPoHEligibilityCheck(int id)
        {
            var response = new SingleResponseModel<dynamic>();
            string url = "https://adm.ajkerdeal.com";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("applicationi/json"));
            var methodUrl = url + "/api/account/reports/MerchantPoHEligibleEligibilityCheck/" + id;
            var responseMessage = await client.GetAsync(methodUrl);
            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            response.Model = JsonConvert.DeserializeObject<dynamic>(stringResponse);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetMerchantMonthlyReceivableList_V2")]
        public async Task<IActionResult> GetMerchantMonthlyReceivableList_V2([FromBody] MerchantReceivableParamNewViewModel merchantReceivableParam)
        {
            var response = new SingleResponseModel<MerchantReceivableViewModel>();
            var json = JsonConvert.SerializeObject(merchantReceivableParam);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            string url = "https://adm.ajkerdeal.com";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var methodUrl = url + "/api/account/reports/GetMerchantMonthlyReceivableList_V2";
            var responseMessage = await client.PostAsync(methodUrl, data);
            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            response.Model = JsonConvert.DeserializeObject<MerchantReceivableViewModel>(stringResponse);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetDTMerchantPaidChequeList/{MerchantId}")]
        public async Task<IActionResult> GetDTMerchantPaidChequeList(int MerchantId)
        {
            var response = new ListResponseModel<dynamic>();
            string url = "https://adm.ajkerdeal.com";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var methodUrl = url + "/api/account/reports/GetDTMerchantPaidChequeList/" + MerchantId;
            var responseMessage = await client.GetAsync(methodUrl);
            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            response.Model = JsonConvert.DeserializeObject<List<dynamic>>(stringResponse);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("GetDTMerchantPaidChequeDetails/{MerchantId}/{TransactionNo}")]
        public async Task<IActionResult> GetDTMerchantPaidChequeDetails(int MerchantId, string TransactionNo)
        {
            var response = new ListResponseModel<dynamic>();
            string url = "https://adm.ajkerdeal.com";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var methodUrl = url + "/api/account/reports/getDTMerchantPaidChequeDetails/" + MerchantId + "/" + TransactionNo;
            var responseMessage = await client.GetAsync(methodUrl);
            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            response.Model = JsonConvert.DeserializeObject<List<dynamic>>(stringResponse);
            return Ok(response);
        }

        /// <summary>
        /// CloseDTComplainBulk
        /// </summary>
        /// <param name="requestModels"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("CloseDTComplainBulk")]
        public async Task<IActionResult> CloseDTComplainBulk([FromBody] List<ComplainCloseRequestModel> requestModels)
        {
            var response = new SingleResponseModel<bool>();
            var json = JsonConvert.SerializeObject(requestModels);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            string url = "https://adm.ajkerdeal.com"; //"http://localhost:15332";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var methodurl = url + "/api/Complain/CloseDTComplainBulk";
            var responseMessage = await client.PostAsync(methodurl, data);
            var stringResponse = await responseMessage.Content.ReadAsStringAsync();
            var jsonDeserialize = JsonConvert.DeserializeObject<dynamic>(stringResponse);
            response.Model = jsonDeserialize.IsClosed;
            return Ok(response);
        }
    }
}
