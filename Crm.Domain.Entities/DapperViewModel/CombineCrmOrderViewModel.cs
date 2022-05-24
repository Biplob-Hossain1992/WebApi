using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using Crm.Domain.Entities.DapperDataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crm.Domain.Entities.DapperViewModel
{
    public class CombineCrmOrderViewModel
    {
        public IEnumerable<CouponsViewModel> CouponsViewModel { get; set; }
        public OrderCrmCountDataModel CouponsCountViewModel { get; set; }
    }
}
