using AdCourier.Domain.Entities.TestModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<dynamic> GetJsonData()
        {
            var data =  await _testRepository.GetJsonData();
            return JObject.Parse(data.value);
        }
    }
}
