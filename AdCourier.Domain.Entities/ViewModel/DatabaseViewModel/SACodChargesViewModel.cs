using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class SACodChargesViewModel
    {
        public decimal MinAmount { get; set; } = 0;
        public decimal MaxAmount { get; set; } = 0;
        public decimal CodCharge { get; set; } = 0;
        public decimal IntervalAmount { get; set; } = 0;
    }
}
