using System.Collections.Generic;

namespace AdCourier.Domain.Entities.BodyModel.Permission
{
    public class MailSmsChargeBodyModel
    {
        public int MerchantId { get; set; }
        public decimal SmsCharge { get; set; }
        public decimal MailCharge { get; set; }
        public decimal ReturnCharge { get; set; }
        public decimal CollectionCharge { get; set; }
        public PermissionListModel permissionModel { set; get; }
    }


    public class PermissionListModel
    {
        public int MerchantId { get; set; }
        public int[] StatusId { get; set; }
        public bool Email { get; set; }
        public bool Sms { get; set; }
        public bool CustomerSms { get; set; }
        public bool CustomerEmail { get; set; }
        public string PermissionType { get; set; }
    }
}
