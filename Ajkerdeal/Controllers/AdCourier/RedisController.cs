using AdCourier.Domain.Entities.ViewModel;
using AyaatLibrary.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers.AdCourier
{
    [Produces("application/json")]
    [Route("api/Redis")]
    //[Authorize]
    public class RedisController : ControllerBase
    {
        private readonly IRedisCacheClient _redis;
        public RedisController(IRedisCacheClient redis)
        {
            _redis = redis;
        }

        [HttpDelete]
        [Route("SingleRemoveAsync/{key}")]
        public async Task<IActionResult> RemoveAsync(string key)
        {

            var response = new SingleResponseModel<bool>();

            try
            {
                var data = await _redis.Db1.RemoveAsync(key);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }
            return response.ToHttpResponse();
        }

        [HttpDelete]
        [Route("MultipleRemoveAsync/{keys}")]
        public async Task<IActionResult> RemoveAllAsync(string keys)
        {

            var response = new SingleResponseModel<bool>();

            try
            {
                var keyArray = keys.Split(',').ToList();
                await _redis.Db1.RemoveAllAsync(keyArray);
                response.Model = true;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }
            return response.ToHttpResponse();
        }

        [HttpDelete]
        [Route("SingleFlushDbAsync")]
        public async Task<IActionResult> FlushDbAsync()
        {

            var response = new SingleResponseModel<bool>();

            try
            {
                await _redis.Db1.FlushDbAsync();
                response.Model = true;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [Route("KeyExistsAsync/{key}")]
        public async Task<IActionResult> KeyExistsAsync(string key)
        {

            var response = new SingleResponseModel<bool>();

            try
            {

                bool data = await _redis.Db1.Database.KeyExistsAsync(key);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
            }
            return response.ToHttpResponse();
        }
    }
}
