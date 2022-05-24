using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.Complain
{
    public class ComplainCloseRequestModel
    {
        public string BookingCode { get; set; }
        public string ComplainType { get; set; }
        public string Source { get; set; }
    }
}
