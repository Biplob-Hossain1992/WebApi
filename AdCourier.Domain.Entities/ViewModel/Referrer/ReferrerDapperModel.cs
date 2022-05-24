using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Referrer
{
    public class ReferrerDapperModel
    {
        public DateTime JoinDate { get; set; }
        public int ReferrerCount { get; set; }
        public int ReferrerOrder { get; set; }
        public int ReferrerOrderUse { get; set; }
    }
}
