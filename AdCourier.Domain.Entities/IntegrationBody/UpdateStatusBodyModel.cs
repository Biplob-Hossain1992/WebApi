using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.IntegrationBody
{
    public class UpdateStatusBodyModel
    {
        public string IsConfirmedBy { set; get; }
        public string Comment { set; get; }
        public int Status { set; get; }        
    }
}
