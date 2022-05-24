using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class CustomersViewModel
    {
        public int CustomerId { get; set; }
        public string CName { get; set; }
        public string CMobile { get; set; }
        public string CAddress { get; set; }
        public string CEmail { get; set; }
        public string DeviceId { get; set; }
    }
}
