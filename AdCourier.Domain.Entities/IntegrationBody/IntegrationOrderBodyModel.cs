using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdCourier.Domain.Entities.IntegrationBody
{
    public class IntegrationOrderBodyModel
    {
        [Required]
        public string CustomerName { get; set; }
        [Required]
        [MinLength(11)]
        [MaxLength(11, ErrorMessage = "Up to 11 chars")]
        public string Mobile { get; set; }
        [MaxLength(11, ErrorMessage = "Up to 11 chars")]
        [MinLength(11)]
        public string OtherMobile { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        public int DistrictId { get; set; }
        [Required]
        public int ThanaId { get; set; }
        [Required]
        public int AreaId { get; set; }
        [Required]
        public int CollectAddressDistrictId { get; set; }
        [Required]
        public int CollectAddressThanaId { get; set; }

        [Required]
        public string CollectionName { get; set; }

        [Required]
        public decimal CollectionAmount { get; set; }
        [Required]
        public decimal ActualPackagePrice { get; set; }
        public string OrderType { get => this.CollectionAmount > 0 ? "Delivery Taka Collection" : "Only Delivery"; }
        public decimal CodCharge { get => calCodCharge(); }
        public decimal CollectionCharge { get => charge(); }
        private decimal charge()
        {
            return this.OfficeDrop == true ? 0 : 5;
        }
        private decimal calCodCharge()
        {
            if (this.CollectionAmount > 0)
            {
                if (this.DistrictId.Equals(14))
                {
                    return 5;
                }
                else
                {
                    return this.CollectionAmount <= 1000 ? 10 : (this.CollectionAmount * 1) / 100;
                }
            }
            else
            {
                return 0;
            }
        }

        [Required]
        public string Weight { get; set; }
        [Required]
        public int WeightRangeId { get; set; }
        [Required]
        public string PodNumber { get; set; }
        //public DateTime OrderDate { set; get; } = DateTime.Now;
        [Required]
        public string CollectAddress { get; set; }

        [Required]
        public int CollectionTimeSlotId { get; set; }
        [Required]
        public DateTime CollectionTime { get; set; }
        [Required]
        public string OrderFrom { get; set; }
        [Required]
        public bool OfficeDrop { get; set; }
        public string ServiceType { get => getServiceType(); }


        private string getServiceType()
        {
            return this.DistrictId == CollectAddressDistrictId ? "citytocity" : "alltoall";
        }
        //public int DeliveryRangeId { get; set; } = 21;
        //public decimal DeliveryCharge { get; set; } = 70;
        public string Note { get; set; } = "";
        //public int Status { set; get; } = 0;
        //public bool IsActive { get; set; } = true;
       // public int MerchantId { set; get; }
    }
}
