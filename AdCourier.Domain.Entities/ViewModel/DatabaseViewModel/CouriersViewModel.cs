using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class CouriersViewModel
    {
        public int CourierId { set; get; }
        public string CourierName { set; get; }
        public string ContactNo { set; get; }
        public string ContactAddress { set; get; }
        public bool IsActive { get; set; } = true;
        public int PostedBy { get; set; } = 0;
        public DateTime? PostedOn { get; set; }
        public int UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedOn { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public bool? IsPresent { get; set; }
        public bool? IsOwnTiger { get; set; }

        //public CouriersViewModel(DataModel.Couriers c)
        //{
        //    CourierId = c.CourierId;
        //    CourierName = c.CourierName;
        //    ContactNo = c.ContactNo;
        //    ContactAddress = c.ContactAddress;

        //}
    }

}
