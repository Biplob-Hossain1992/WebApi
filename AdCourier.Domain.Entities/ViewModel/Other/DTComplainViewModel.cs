using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.Other
{
    public class DTComplainViewModel
    {
        public string OrderId { get; set; }
        public string Comments { get; set; }
        public string ComplainFrom { get; set; } = "";
        public int AnswerBy { get; set; } = 0;
        public string CompanyName { get; set; } = "";
        public string Mobile { get; set; } = "";
    }
}
