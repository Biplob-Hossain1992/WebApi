using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdCourier.Domain.Entities.DataModel
{
    [Table("SurveyQuestion", Schema = "DT")]
    public class SurveyQuestion
    {
        [Key]
        public int SurveyQuestionId { get; set; }
        public string QuestionName { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public int Ordering { get; set; }
        public bool IsMultipleAnswer { get; set; }
    }
}
