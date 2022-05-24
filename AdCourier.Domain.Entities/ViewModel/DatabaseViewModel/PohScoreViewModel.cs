using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class PohScoreViewModel
    {
        public int CourierUserId { get; set; }
        public int Score { get; set; } = 0;
        public string ScoreEligibility { get; set; } = "";
        public string CreditLimit { get; set; } = "0";
    }
}
