using Microsoft.AspNetCore.Mvc.Rendering;

namespace BestFriendQuiz.Models
{
    public class SurveyCreateViewModel
    {
        public int NumberOfQuestions { get; set; }
        public List<QuestionWithOptionsViewModel> Questions { get; set; }
     
        public class QuestionWithOptionsViewModel
        {
            public int QuestionId { get; set; }
            public string QuestionText { get; set; }
            public int? SelectedQuestionId { get; set; } // Seçilen sorunun Id'sini tutacak özellik
            public List<SelectListItem> Options { get; set; } // Soru için seçenekleri tutacak özellik
            public string Option { get; set; } // Soru için seçenekleri tutacak özellik

        }
    }

}
