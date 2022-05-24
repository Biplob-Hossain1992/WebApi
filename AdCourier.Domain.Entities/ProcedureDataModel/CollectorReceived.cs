using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdCourier.Domain.Entities.ProcedureDataModel
{
    public class CollectorReceived
    {
        [Key]
        public int CollectorId { get; set; }
        public string CollectorName { get; set; }
        public int TotalCollected { get; set; }
        public int TotalReceived { get; set; }
        public int CollectedUniqMerchant { get; set; }
        public int ReceivedUniqMerchant { get; set; }

    }
}
