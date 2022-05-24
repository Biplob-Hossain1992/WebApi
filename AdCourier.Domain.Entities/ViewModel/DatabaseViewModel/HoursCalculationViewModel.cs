using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class HoursCalculationViewModel
    {
        //Collection
        public int DistrictId { get; set; }
        public string District { get; set; }
        public int ThanaId { get; set; }
        public string Thana { get; set; }
        public int CollectAddressDistrictId { get; set; }
        public int CollectAddressThanaId { get; set; }
        public decimal? OrderPlaceToConfirm {get;set;}
        public decimal? ConfirmToRiderOrderPick { get;set;}
        public decimal? RiderOrderPickToOfficeReceive { get;set;}
        public decimal? OfficeReceiveToPackaged { get;set;}
        //Delivery
        public decimal? CourierPickToCourierLastHub { get; set; }
        public decimal? CourierLastHubToOutForDelivery { get; set; }
        public decimal? OutForDeliveryToDelivery { get; set; }
    }
}
