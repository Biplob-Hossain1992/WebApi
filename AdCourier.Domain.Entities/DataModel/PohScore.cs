using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("PohScore", Schema = "Dana")]
    public class PohScore
    {
        [Key]
        public int Id { get; set; }
        public int CourierUserId { get; set; }
        public int Score { get; set; } = 0;
        public string ScoreEligibility { get; set; } = "";
        public string CreditLimit { get; set; } = "0";
    }
}
