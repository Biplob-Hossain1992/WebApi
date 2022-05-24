using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.BondhuApp
{
    public class BondhuAppMismatchDataViewModel
    {
        public int Id { get; set; }
        public string OrderDate { get; set; }
        public string CompanyName { get; set; }
    }

    public class DashBoardOrderViewModel
    {
        public int ShipmentCount { get; set; }
        public string ShipmentName { get; set; }
        public int ReturnCount { get; set; }
        public string ReturnName { get; set; }
        public int UnreachableCount { get; set; }
        public string UnreachableName { get; set; }
        public int SortingCount { get; set; }
        public string SortingName { get; set; }
    }

}
