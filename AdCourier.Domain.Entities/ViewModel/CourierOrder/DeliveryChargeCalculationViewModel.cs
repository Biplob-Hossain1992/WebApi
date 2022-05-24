using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.CourierOrder
{
    public class DeliveryChargeCalculationViewModel
    {
        public string CODCharge { get; set; }
        public decimal CodChargeDhaka { get; set; }
        public decimal CodChargeOutsideDhaka { get; set; }
        public decimal CodChargePercentageDhaka { get; set; }
        public decimal CodChargePercentageOutsideDhaka { get; set; }
        public InSideOutSideDhakaModel InSideDhaka { get; set; }
        public InSideOutSideDhakaModel OutSideDhaka { get; set; }       
    }

    public class InSideOutSideDhakaModel
    {
        public string Text { get; set; }
        public int DistrictId { get; set; }
        public string Charge { get; set; }
        public string ChargePercentage { get; set; }
        public List<DeliveryRangeModel> DeliveryRange { get; set; }
    }

    public class DeliveryRangeModel
    {
        public string Name { get; set; }
        public int DaliveryRangeId { get; set; }
        public string OnImage { get; set; }
        public string OffImage { get; set; }
    }
}
