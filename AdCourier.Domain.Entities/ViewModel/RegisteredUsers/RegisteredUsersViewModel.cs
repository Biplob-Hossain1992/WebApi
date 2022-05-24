using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.RegisteredUsers
{
    public class RegisteredUsersViewModel
    {
        public int DateRegisterMerchant { get; set; }
        public int DateOrderMerchant { get; set; }
        public int MonthRegisterMerchant { get; set; }
        public int MonthOrderMerchant { get; set; }
        public int MonthFirstOrderMerchant { get; set; }
        public int ThisMonthOrderedMerchant { get; set; }
        public int LifeTimeOrderMerchant { get; set; }
        public int LifeTimeFirstOrderMerchantMonth { get; set; }
        public int LifeTimeFirstOrderMerchantMonthAcqMgr { get; set; }
        public int LifeTimeFirstOrderMerchantDate { get; set; }
        public int LifeTimeFirstOrderMerchantDateAcqMgr { get; set; }
    }

    public class RetentionAcquisitionUsersModel
    {
        public IEnumerable<RetentionAcquisitionUsersViewModel> RetentionUserModel { get; set; }
        public IEnumerable<RetentionAcquisitionUsersViewModel> AcquisitionUserModel { get; set; }
    }
    public class RetentionAcquisitionUsersViewModel
    {
        public string CompanyName { get; set; }
        public int TotalOrder { get; set; }
        public DateTime JoinDate { get; set; }
        public int TotalMerchantCalled { get; set; }
        public int TotalMerchantVisited { get; set; }
    }
    
    public class MerchantDetailsResponseModel
    {
        public int CourierUserId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public DateTime JoinDate { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public int TotalOrder { get; set; }
        public int PickedOrder { get; set; }
        public decimal SalaryAmount { get; set; } = 0;
    }

}
