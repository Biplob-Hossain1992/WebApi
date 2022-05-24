using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.DapperDataModel
{
    public class LastMileInformationDapperModel
    {
        public int Id { get; set; }
        public string District { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceCourier { get; set; }
    }
}
