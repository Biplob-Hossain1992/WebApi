using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Other
{
    public class DtMerchantPayableDetailsViewModel
    {
        public bool Availability { get; set; } = false;
        public string AvailabilityMessage { get; set; } = "";
        public int TotalOrderCount { get; set; }
        public List<DtMerchantPayableModel> PayableOrders { get; set; }
        public int PayableOrderCount { get; set; }
        public double TotalMerchantPayable { get; set; }
        public double TotalCollectedAmount { get; set; }
        public double TotalCodServiceCharge { get; set; }
        public List<DtMerchantPayableModel> ReceivableOrders { get; set; }
        public int ReceivableOrderCount { get; set; }
        public double TotalMerchantReceivable { get; set; }
        public List<DtMerchantPayableModel> AdvAccReceiveableOrders { get; set; }
        public double AdvAccReceiveableOrderCount { get; set; }
        public double TotalAdvAccReceiveable { get; set; }
        public double NetAdjustedAmount { get => this.TotalMerchantPayable - this.TotalMerchantReceivable - this.TotalAdvAccReceiveable; }
        public string LastRequestDate { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class DtMerchantPayableModel
    {
        public string OrderCode { get; set; }
        public double MerchantPayable { get; set; }
        public double AccReceiveable { get; set; }
        public double AdvAccReceiveable { get; set; }
        public double AdjustedAmount { get; set; }
        public string FreezeDate { get; set; }
        public string OrderType { get; set; }
        public double CollectedAmount { get; set; }
        public double DeliveryCharge { get; set; }
        public double CODCharge { get; set; }
        public double BreakableCharge { get; set; }
        public double ReturnCharge { get; set; }
        public double PackagingCharge { get; set; }
        public double CollectionCharge { get; set; }
        public double TotalCharge { get; set; }
        public string SDate { get; set; } = "";
    }


    public class DTMerchantInstantPaymentStatus
    {
        public int PaymentServiceType { get; set; }
        public int PaymentMethod { get; set; }
        public int Processing { get; set; }
        public int PaymentType { get; set; }
        public int LastPaymentAmount { get; set; }
        public int LastPaymentStatus { get; set; }
        public int CurrentPaymentAmount { get; set; }
        public int CurrentPaymentStatus { get; set; }
        public string FailedTransferMsg { get; set; }
        public string SuccessSuperExpressTransferMsg { get; set; }
        public string SuccessExpressTransferMsg { get; set; }
        public string SuccessbkashTransferMsg { get; set; }
        public string ExpressBankTime { get; set; }
        public string BkashRime { get; set; }
        public string BankTime { get; set; }
        public string LastPaymentDate { get; set; }
        public string LastRequestDate { get; set; }
        public string CurrentRequestDate { get; set; }
    }
}
