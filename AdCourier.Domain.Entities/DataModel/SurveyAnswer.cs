using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("SurveyAnswer", Schema = "DT")]
    public class SurveyAnswer
    {
        [Key]
        public int SurveyAnswerId { get; set; }
        public int SurveyQuestionId { get; set; }
        public string AnswerName { get; set; }
        public bool IsActive { get; set; }
        public int SurveyRedirectNextQuestionId { get; set; }
        //public int SurveyRedirectPreviousQuestionId { get; set; }
    }
}
