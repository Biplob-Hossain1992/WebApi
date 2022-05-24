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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRedisCacheClient _redis;
        public CustomerService(ICustomerRepository customerRepository, IRedisCacheClient redis)
        {
            _customerRepository = customerRepository;
            _redis = redis;
        }

        public async Task<Customers> GetCustomerInformation(int customerId)
        {
            string key = "GetCustomerInformation" + customerId.ToString();

            if (await _redis.Db2.Database.KeyExistsAsync(key) == true)
            {
                return await _redis.Db2.GetAsync<Customers>(key);
            }
            else
            {
                var data = await _customerRepository.GetCustomerInformation(customerId);
                if (data != null)
                {
                    bool added = await _redis.Db2.AddAsync(key, data, DateTimeOffset.Now.AddHours(4));
                }
                return data;
            }
        }
    }
}
