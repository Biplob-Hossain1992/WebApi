using System;
using System.Collections.Generic;
using System.Text;

namespace Crm.Domain.Entities.DapperDataModel
{
    public class OrderStatusHistoryDataModel
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public string UserName { get; set; }
        public string Comments { get; set; }
        public string OrderStatus { get; set; }
        public int StatusId { get; set; }

    }
}
