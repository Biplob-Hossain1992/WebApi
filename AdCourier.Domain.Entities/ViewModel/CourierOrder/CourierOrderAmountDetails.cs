using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.CourierOrder
{
    public class CourierOrderAmountDetails
    {
        public string CustomerName { set; get; }
        public string CourierOrdersId { set; get; }
        public decimal CollectionAmount { set; get; }
        public decimal DeliveryCharge { set; get; }
        public decimal BreakableCharge { set; get; }
        public decimal CODCharge { set; get; }
        public string Status { set; get; }
        public string ServiceBillingStatus { get { return ServiceBillingStatusText(this.Status); } }

        public decimal CollectionCharge { set; get; }
        public decimal ReturnCharge { set; get; }




        public string StatusTextColorClass { get { return StatusTextColor(this.Status); } }

        private string StatusTextColor(string status)
        {
            string text = string.Empty;
            if (status.ToLower() == "paid".ToLower())
            {
                text = "paidColor";
            }
            else if (status.ToLower() == "unpaid".ToLower())
            {
                text = "unPaidColor";
            }
            else
            {

            }
            return text;
        }

        private string ServiceBillingStatusText(string status)
        {
            string text = string.Empty;
            if (status.ToLower() == "paid".ToLower())
            {
                text = "পেইড";
            }
            else if (status.ToLower() == "unpaid".ToLower())
            {
                text = "আনপেইড";
            }
            else
            {

            }
            return text;
        }

        public decimal TotalAmount { get { return SumOfChargeAMount(this.CODCharge, this.DeliveryCharge, this.BreakableCharge, this.CollectionCharge, this.ReturnCharge); } }
        public string ClassName { get { return ClassNameRetrive(this.Status); } }
        public string PaidUnpaidColor
        {
            get { return PaidUnpaidColorClass(this.Status); }
        }

        private string PaidUnpaidColorClass(string status)
        {
            string color = string.Empty;
            if (status.ToLower() == "paid")
            {
                color = "paidColor";
            }
            else if (status.ToLower() == "unpaid")
            {
                color = "unPaidColor";
            }
            else
            {
                color = "";
            }
            return color;
        }

        private string ClassNameRetrive(string input)
        {
            string color = string.Empty;
            if (input.ToLower() == "paid")
            {
                color = "success";
            }
            else if (input.ToLower() == "unpaid")
            {
                color = "danger";
            }
            else
            {
                color = "";
            }
            return color;
        }

        public decimal SumOfChargeAMount(decimal codCharge, decimal deliveryCharge, decimal breakableChange, decimal collectionCharge, decimal returnCharge)
        {
            return codCharge + deliveryCharge + breakableChange + collectionCharge + returnCharge;
        }
    }
    public class CourierAmountDetailsResponse
    {
        public decimal TotalAmountOnlyDelivery { set; get; }
        public decimal TotalAmountDeliveryTakaCollection { set; get; }
        public int TotalData { set;get; }
        public decimal TotalAmount { set; get; }
        public int TotalDataCount { set; get; }
        public List<CourierOrderAmountDetails> CourierOrderAmountDetails { set; get; }
    }
}
