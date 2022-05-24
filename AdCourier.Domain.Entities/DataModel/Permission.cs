using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("Permission", Schema = "DT")]
    public class Permission
    {
        [Key]
        [Column("PermissionId")]
        public int PermissionId { get; set; }
        public int MerchantId { get; set; }
        public int StatusId { get; set; }
        public bool Email { get; set; } 
        public bool Sms { get; set; } 
        public string PermissionType { get; set; }
    }
}
