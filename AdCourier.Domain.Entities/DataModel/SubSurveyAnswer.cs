using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("SubSurveyAnswer", Schema = "DT")]
    public class SubSurveyAnswer
    {
        [Key]
        [Column("SubSurveyAnswerId")]
        public int SubSurveyAnswerId { get; set; }
        public int SurveyAnswerId { get; set; }
        public string SubAnswerName { get; set; }
        public bool IsActive { get; set; }
    }
}
