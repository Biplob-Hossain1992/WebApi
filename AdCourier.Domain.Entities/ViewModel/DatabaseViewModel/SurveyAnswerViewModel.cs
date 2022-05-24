using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class SurveyAnswerViewModel
    {
        public int SurveyAnswerId { get; set; }
        public int SurveyQuestionId { get; set; }
        public string AnswerName { get; set; }
        public bool IsActive { get; set; }
    }
}
