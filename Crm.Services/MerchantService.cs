using AdCourier.Domain.Entities.DataModel;
using Crm.Domain.Interfaces;
using Crm.Services.Interfaces;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly IRedisCacheClient _redis;
        public MerchantService(IMerchantRepository merchantRepository, IRedisCacheClient redis)
        {
            _merchantRepository = merchantRepository;
            _redis = redis;
        }

        public async Task<UserProfile> GetMerchantInformation(int merchantId)
        {

            string key = "GetMerchantInformation" + merchantId.ToString();

            if (await _redis.Db2.Database.KeyExistsAsync(key) == true)
            {
                return await _redis.Db2.GetAsync<UserProfile>(key);
            }
            else
            {
                var data = await _merchantRepository.GetMerchantInformation(merchantId);
                if (data != null)
                {
                    bool added = await _redis.Db2.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                }
                return data;
            }
        }
    }
}
