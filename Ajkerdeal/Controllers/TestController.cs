using AdCourier.Context;
using AdCourier.Domain.Entities;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdCourier.Domain.Entities.ViewModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Services.Interfaces;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AyaatLibrary.ResponseModel;
using System.ComponentModel.DataAnnotations;

namespace Ajkerdeal.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : ControllerBase
    {
        private readonly SqlServerContext _sqlServerContext;
        private SmtpClient _smtpClient;
        private readonly IRedisCacheClient _redis;
        private readonly ConnectionStringList _connectionStrings;
        private readonly IWeightRangeService _weightRangeService;
        private readonly ITestService _testService;


        public TestController(SmtpClient smtpClient, IRedisCacheClient redis, IOptions<ConnectionStringList> connectionStrings,
            SqlServerContext sqlServerContext, IWeightRangeService weightRangeService, ITestService testService)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _smtpClient = smtpClient;
            _redis = redis;
            _connectionStrings = connectionStrings.Value;
            _weightRangeService = weightRangeService;
            _testService = testService;
        }

        [HttpGet]
        [Route("GetJsonData")]
        public async Task<IActionResult> GetJsonData()
        {
            var data = await _testService.GetJsonData();
            return Ok(data);
        }

        [HttpPost]
        [Route("EntityStateTest")]
        public async Task<IActionResult> EntityStateTest()
        {
            try
            {
                var order = (from o in _sqlServerContext.CourierOrders
                             where o.Id == 1
                             select o).FirstOrDefault();

                order.CustomerName = "abin hasan";
                order.OtherMobile = "0191353738";

                var d = _sqlServerContext.Entry(order).State;
                var d2 = EntityState.Unchanged;

                var dd = _sqlServerContext.Entry(order).Properties;

                foreach (var property in _sqlServerContext.Entry(order).Properties)
                {
                    var original = property.OriginalValue;
                    var current = property.CurrentValue;

                    if (ReferenceEquals(original, current))
                    {
                        continue;
                    }

                    if (original == null)
                    {
                        property.IsModified = true;
                        continue;
                    }

                    var propertyIsModified = !original.Equals(current);
                    property.IsModified = propertyIsModified;
                }


                //var vv=  _sqlServerContext.Entry(order).Property(p => p).Metadata;

                _sqlServerContext.Entry(order).Property(p => p.OtherMobile).IsModified = true;

                if (_sqlServerContext.Entry(order).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(order);
                    await _sqlServerContext.SaveChangesAsync();
                }

                var added = true;
                if (added)
                    return Ok();
                else
                    return BadRequest("Cannot add user");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("RedisAdd")]
        public async Task<IActionResult> RedisAdd()
        {


            try
            {

                //string bio = "DT-2";
                //var d = bio.Length;

                //int endIndex = bio.Length - 3;
                //string authorName = bio.Substring(3, endIndex);

                //string str = "DT-111119";
                //int startIndex = 3;
                //int endIndex = str.Length - 3;
                //int title = Convert.ToInt32(str.Substring(startIndex, endIndex));

                var user = new User()
                {
                    Firstname = "Abin",
                    Lastname = "Hasan",
                    Twitter = "@abin",
                    Blog = "http://abinhasan.github.io/"
                };


                bool added = await _redis.Db1.AddAsync("abin", user, DateTimeOffset.Now.AddSeconds(20));

                if (added)
                    return Ok();
                else
                    return BadRequest("Cannot add user");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("FileAdd")]
        public async Task<IActionResult> FileAdd()
        {


            //try
            //{
            //    string text = "The text inside the file.";
            //    System.IO.File.WriteAllText("file_name.txt", text);
            //    return Ok();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}


            var response = new SingleResponseModel<bool>();

            try
            {

                var user = new User()
                {
                    Firstname = "Abin",
                    Lastname = "Hasan",
                    Twitter = "@abin",
                    Blog = "http://abinhasan.github.io/"
                };


                bool added = await _redis.Db1.AddAsync("abin", user, DateTimeOffset.Now.AddSeconds(20));

                response.Model = added;
            }
            catch (Exception ex)
            {

                //string text = ex.Message.ToString();
                //System.IO.File.WriteAllText("file_name.txt", text);
                //System.IO.File.WriteAllText("file_name1.txt", ex.InnerException.Message.ToString());

                response.DidError = true;
                response.ErrorMessage = ex.Message.ToString();
            }
            return response.ToHttpResponse();
        }

        [HttpPost]
        [Route("mail")]
        public async Task<IActionResult> Post()
        {
            try
            {
                PaymentMail request = new PaymentMail();
                request.AccountHolderName = "Md Abin Hasan";
                request.RegistrationPhoneNo = "01715269261";

                await _smtpClient.SendMailAsync(new MailMessage(
                        from: "info@deliverytiger.com.bd",
                        to: "porag214@gmail.com",
                        subject: "Test message subject",
                        body: "Test message body"
                        ));

                //await _smtpClient.SendMailAsync(new MailMessage(
                //     from: "info@deliverytiger.com.bd",
                //     to: "porag214@gmail.com",
                //     subject: "Bank payment information",
                //     body: $"চলে আসলো ব্যাংক ট্রান্সফার সুবিধা। এখন থেকে ডেলিভারির মাত্র ২৪ ঘণ্টায়(চার্জ প্রযোজ্য)। ব্যাংক ট্রান্সফার এক্টিভেট করতে হলে আপনার নিম্নাক্ত তথ্যসহ ডেলিভারি টাইগারের রেজিস্টার্ড মেইল থেকে দ্রুত ইমেইল করুন।{Environment.NewLine}একাউন্ট নামঃ {request.AccountHolderName} {Environment.NewLine}একাউন্ট নম্বরঃ {request.AccountNo} {Environment.NewLine}রাউটিং নম্বরঃ {request.RoutingNo} {Environment.NewLine}ব্যাংক নামঃ {request.BankName} {Environment.NewLine}ব্রাঞ্চ নামঃ {request.BranchName} {Environment.NewLine}ব্যাংকে এ প্রদেয় মোবাইল নম্বরঃ {request.RegistrationPhoneNo}"
                //     ));

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

        [HttpPost]
        [Route("CheckOrderFrom")]
        public IActionResult CheckOrderFrom([FromBody] Special courierOrders)
        {

            if (courierOrders.OrderFrom != "" && courierOrders.OrderFrom.Contains("android-"))
            {
                int startIndex = 8;
                int endIndex = courierOrders.OrderFrom.Length - 8;
                courierOrders.Version = courierOrders.OrderFrom.Substring(startIndex, endIndex);
                courierOrders.OrderFrom = "android";
            }

            return Ok(courierOrders);
        }


        [HttpGet]
        [Route("GetAllTest")]
        public IActionResult GetAll()
        {
            try
            {
                int data = 0;
                //Thread thread = new Thread(RunMillionIterations) ;
                //thread.Start();
                //Parallel.For(0, 10000000, x => RunMillionIterations());
                //using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
                //{
                //    connection.Open();

                //    data = connection.Execute(
                //       sql: @"[DT].[USP_UpdateTestProce]",
                //       param: null,
                //       commandType: CommandType.StoredProcedure);
                //}

                //int i = 0;
                //var extra = _sqlServerContext.ExtraCharge.FirstOrDefault();

                //var pohStatus = extra.AutoDownloadPohStatus.Split(',');

                //var a = Array.ConvertAll(pohStatus, s => int.TryParse(s, out var i) ? i : 0);

                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("SlotChangeSMSandNotification")]
        public async Task<IActionResult> SlotChangeSMSandNotification([FromBody] List<CourierOrders> orders)
        {
            var response = new SingleResponseModel<bool>();
            try
            {
                var data = await _weightRangeService.SlotChangeSMSandNotification(orders);
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an error, please contact with IT Support";
            }

            return response.ToHttpResponse();
        }

        private static void RunMillionIterations()
        {
            string x = "";
            for (int i = 0; i < 10000000; i++)
            {
                x = x + "s";
            }
        }

        [HttpPost]
        [Route("RemoveSpecialCharacters")]
        public async Task<IActionResult> RemoveSpecialCharacters([FromBody] Special special)
        {

            //special.myString = special.myString.Replace("'", " ").Replace(",", " ").Replace("\t", " ").Replace("\n", " ").Replace(":", " ");

            //special.myString = Regex.Replace(special.myString, @"\s+", " ", RegexOptions.Multiline);



            string[] chars = new string[] { "\n", "\t", ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", @"\", ";", "_", "(", ")", ":", "|", "[", "]" };
            for (int i = 0; i < chars.Length; i++)
            {
                if (special.myString.Contains(chars[i]))
                {
                    special.myString = special.myString.Replace(chars[i], "");
                }
            }

            special.myString = Regex.Replace(special.myString, @"\s+", " ", RegexOptions.Multiline);

            return Ok(special.myString.Trim());
            //return Ok(myString2);
        }




        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("GetAllVouchers")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllVouchers()
        {
            var response = new ListResponseModel<Vouchers>();
            try
            {
                var data = await _weightRangeService.GetAllVouchers();
                response.Model = data;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support.";
            }
            return response.ToHttpResponse();
        }

        [HttpGet]
        [Route("{xCoordinate}")]
        public IActionResult GetByCoordinates([FromQuery] string xCoordinate)
        {
            return Ok(xCoordinate);
        }

        [HttpPost]
        [Route("AddTestOrder")]
        public IActionResult AddTestOrder([FromBody] TestOrder request)
        {

            return Ok(request);
        }

        public class TestOrder
        {
            [Required]
            [StringLength(8, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
            public string MyString { get; set; }
            public string Version { get; set; }

        }

        public class Special
        {
            public string myString { get; set; }
            public string OrderFrom { get; set; }
            public string Version { get; set; }

        }
        public class User
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Twitter { get; set; }
            public string Blog { get; set; }
        }
    }
}
