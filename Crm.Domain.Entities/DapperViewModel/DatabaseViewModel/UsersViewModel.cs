using System;
using System.Collections.Generic;
using System.Text;

namespace Crm.Domain.Entities.DapperViewModel.DatabaseViewModel
{
    public class UsersViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public byte AdminType { get; set; }
        public string Passwrd { get; set; }
        public byte IsActive { get; set; }
    }
}
