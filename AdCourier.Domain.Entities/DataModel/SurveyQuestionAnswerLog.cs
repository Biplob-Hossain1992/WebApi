using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AdCourier.Domain.Entities.DataModel
{
    [Table("SurveyQuestionAnswerLog", Schema = "DT")]
    public class SurveyQuestionAnswerLog
    {
        [Key]
        public int SurveyQuestionAnswerLogId { get; set; }
        public int SurveyQuestionId { get; set; }
        public int SurveyAnswerId { get; set; }
        public int MerchantId { get; set; }
        public DateTime CurrentDate { get; set; } = DateTime.Now;
        public string SurveyOpenAnswer { get; set; }
        public int SubSurveyAnswerId { get; set; }
    }
}
