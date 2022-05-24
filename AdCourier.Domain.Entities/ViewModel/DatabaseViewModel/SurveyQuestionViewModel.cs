using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class SurveyQuestionViewModel
    {
        public int SurveyQuestionId { get; set; }
        public string QuestionName { get; set; }
        public bool IsActive { get; set; }
        public List<SurveyAnswerViewModel> SurveyAnswerViewModel { set; get; }
    }
}
