using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class DanaService : IDanaService
    {
        private readonly IDanaRepository _danaRepository;
        public DanaService(IDanaRepository danaRepository)
        {
            _danaRepository = danaRepository;
        }
        public async Task<dynamic> AddDanaJsonData(JsonBodyModel jsonBodyModel)
        {
            string res = await ClientMethod(jsonBodyModel);

            jsonBodyModel.Json = res;

            var data = await _danaRepository.AddDanaJsonData(jsonBodyModel);
            return JObject.Parse(data.value);

        }

        private static async Task<string> ClientMethod(JsonBodyModel jsonBodyModel)
        {
            string url = "https://dana.money/cp/api/v3/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + jsonBodyModel.Token);
            string methodUrl = url + jsonBodyModel.Endpoint;
            var responseMessage = await client.GetAsync(methodUrl);
            var res = await responseMessage.Content.ReadAsStringAsync();
            return res;
        }

        public async Task<PohScore> AddPohScore(PohScore pohScoreModel)
        {
            return await _danaRepository.AddPohScore(pohScoreModel);
        }
    }
}
