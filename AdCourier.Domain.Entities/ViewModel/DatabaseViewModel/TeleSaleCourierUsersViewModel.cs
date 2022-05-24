using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class TeleSaleCourierUsersViewModel
    {
        public int TeleSaleCourierUsersId { get; set; }
        public int CourierUserId { get; set; } = 0;
        public int CourierId { get; set; } = 0;
        public string CourierName { get; set; } = "";
        public int TeleSales { get; set; } = 0;
    }
}
