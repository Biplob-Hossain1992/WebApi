using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.Permission
{

    public class InfobipSMSBodyModel
    {
        public List<SendSMSInfobipBodyModel> messages { get; set; }
    }

    public class SendSMSInfobipBodyModel
    {
        public string from { get; set; }
        public List<Destination> destinations { get; set; }
        public string text { get; set; }
    }

    public class Destination
    {
        public string to { get; set; }
    }
}
