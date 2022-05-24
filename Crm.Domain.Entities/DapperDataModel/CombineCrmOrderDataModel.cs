using System;
using System.Collections.Generic;
using System.Text;

namespace Crm.Domain.Entities.DapperDataModel
{
    public class CombineCrmOrderDataModel
    {
        public IEnumerable<OrderCrmDataModel> OrderCrmDataModel { get; set; }
        public OrderCrmCountDataModel OrderCrmCountDataModel { get; set; }
    }
}
