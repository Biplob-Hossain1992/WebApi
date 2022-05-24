using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.CourierOrder
{
    public class DeliveredReturnedDetailsViewModel
    {
        public string CourierOrdersId { set; get; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string StatusNameEng { get; set; }
        public int MerchantId { get; set; }
        public int Status { set; get; }
        public string CollectionName { set; get; }
    }

    public class DeliveredReturnedCountModel
    {
        public int Delivered { set; get; }
        public int Return { get; set; }
        public int DeliveredPercentage { get; set; }
        public int ReturnPercentagee { get; set; }
    }

}
