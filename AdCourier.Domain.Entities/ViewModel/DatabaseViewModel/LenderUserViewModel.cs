using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class LenderUserViewModel
    {
        public int LenderUserId { get; set; } = 0;
        public string UserName { get; set; } = "";
        public string LenderName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string RoleName { get; set; } = "";
        public string Token { get; set; }
    }
}
