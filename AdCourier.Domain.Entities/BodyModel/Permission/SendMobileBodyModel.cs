using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.Permission
{
    public class SendMobileBodyModel
    {
        public string[] numbers { set; get; }
        public string text { set; get; }
        public int type { set; get; }
        public int datacoding { set; get; }
    }

    //public class SendMobileBodyModel
    //{
    //    List<SendMobileModel> sms = new List<SendMobileModel>();

    //    public bool AddSms(string[] numbers, string text, int type, int datacoding)
    //    {
    //        sms.Add(new SendMobileModel { numbers = numbers, text = text, type = type, datacoding = 0 });
    //        return true;
    //    }
    //}
}
