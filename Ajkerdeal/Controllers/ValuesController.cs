using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ajkerdeal.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/values")]
    //[Authorize]
    public class ValuesController : ControllerBase
    {

        private readonly IGenericRepository<CourierOrders> _genericRepository;
        private SmtpClient _smtpClient;

        public ValuesController(IGenericRepository<CourierOrders> genericRepository, SmtpClient smtpClient)
        {
            _genericRepository = genericRepository;
            _smtpClient = smtpClient;
        }

        [HttpPost]
        [Route("mail")]
        public async Task<IActionResult> Post()
        {
            try
            {
                await _smtpClient.SendMailAsync(new MailMessage(
                        from: "info@deliverytiger.com.bd",
                        to: "porag214@gmail.com",
                        subject: "Test message subject",
                        body: "Test message body"
                        ));
                //Log.Information("mail send");
                return Ok("OK");
            }
            catch (Exception ex)
            {
                throw ex;
                //Log.Error(ex, "exception thrown");
                //Log.Error("Error {Message}" + ex.InnerException.Message);
            }

        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {

            CourierUsers c = new CourierUsers();

            c.UserName = "abin";

            var type = typeof(CourierUsers).GetProperties();

            //var propertyInfos = type.GetProperties();
            Type t1 = typeof(String);
            foreach (var propertyInfo in type)
            {
                var name = propertyInfo.Name;
                var value = propertyInfo.GetValue(c);
                var pram = propertyInfo.PropertyType.Name;
                var pram2 = propertyInfo.PropertyType;
                if (t1 == pram2)
                {
                    var d = "asdasd";
                }
            }

            int[] Numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -1 };

            if (!(Numbers.Contains(-1)))
            {
                var sdas = "asdasdad";
            }


            var data = _genericRepository.FindBy(o => o.MerchantId == 1);

            return Ok(data);
        }

        // GET api/values
        //[HttpGet]
        //[Route("Get")]
        [HttpGet]
        [Route("GetAllValues/data")]
        [MapToApiVersion("1.0")]
        public IEnumerable<string> Get3()
        {
            return new string[] { "value1.0", "value1.0" };
        }

        [HttpGet]
        [Route("GetAllValues/data")]
        [MapToApiVersion("1.1")]
        public IEnumerable<string> Get2()
        {
            return new string[] { "value1.1", "value1.1" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    //[ApiVersion("2.0")]
    //[Route("api/values")]
    //public class ValuesV2Controller : ControllerBase
    //{
    //    [HttpGet]
    //    [MapToApiVersion("2.0")]
    //    //[Route("Get")]
    //    public IEnumerable<string> Get()
    //    {
    //        return new string[] { "value2", "value2" };
    //    }
    //}
}
