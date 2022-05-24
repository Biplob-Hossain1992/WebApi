using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.CodCollection
{
    public class CodCollectionDetails
    {
        public int Id { get; set; }
        public string CourierOrdersId { get; set; }
        public int CourierId { get; set; }
        public string CustomerName { get; set; }
        public int Status { set; get; }
        public string DashboardStatusGroup { get; set; }
        public string OrderTrackStatusGroup { get; set; }
        public string CustomerMobile { get; set; }
        public string OtherMobile { get; set; }
        public string CustomerAddress { get; set; }
        public string PaymentType { get; set; }
        public string OrderType { get; set; }
        public string Weight { get; set; }
        public string CollectionName { get; set; }
        public decimal CollectionAmount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal BreakableCharge { get; set; }
        public decimal CodCharge { get; set; }
        public decimal CollectionCharge { get; set; }
        public decimal ReturnCharge { get; set; }
        public DateTime UpdatedOn { set; get; }
        public DateTime ConfirmationDate { set; get; }
        public DateTime PostedOn { set; get; }
        public DateTime OrderDate { get; set; }
        public string Comment { set; get; }
        public string PodNumber { set; get; }
        public int MerchantId { get; set; }
        public string CollectAddress { get; set; }
        public string StatusNameEng { get; set; }
        public string StatusNameBng { set; get; }
        public string StatusType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string HubAddress { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string HubMobile { get; set; }
        public string UserName { get; set; } 
        public string Mobile { get; set; } 
        public string Address { set; get; }
        public string EmailAddress { get; set; }
        public bool IsSms { get; set; } = true;
        public bool IsEmail { get; set; } = true;
        public string CourierName { set; get; }
        public string DistrictName { get; set; }
        public string DistrictNameEng { get; set; }
        public string ThanaName { get; set; }
        public string ThanaNameEng { get; set; }
        public string AreaName { get; set; }
        public string AreaNameEng { get; set; }
        public int DistrictId { get; set; }
        public int ThanaId { get; set; }
        public int AreaId { get; set; }
        public string ThanaPostalCode { get; set; }
        public string AreaPostalCode { get; set; }
        public int CollectAddressDistrictId { get; set; }
        public int CollectAddressThanaId { get; set; }
        public string CollectDistrictName { get; set; }
        public string CollectThanaName { get; set; }
        public bool OfficeDrop { get; set; }
        public int WeightRangeId { get; set; }
        public decimal PaymentServiceCharge { get; set; }
        public byte PaymentServiceType { get; set; }

    }

    public class CodCollection
    {
        public IEnumerable<CodCollectionDetails> CodCollectionDetails { get; set; }
        public CodCollectionTotal CodCollectionTotal { get; set; }
    }

    public class CodCollectionTotal
    {
        public int CodCollectionTotalCount { get; set; }
    }
}
