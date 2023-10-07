using BestFriendQuiz.EntityLayer.Concrete;

namespace BestFriendQuiz.Models
{
    public class SolveViewModel
    {
        public int Id { get; set; }
        public List<QuestionOfSurvey> QuestionsOfSurvey { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string SolverName { get; set; }
        public Test Test { get; set; }

        public List <Result> Results { get; set; }

        public class Result
        {
            public string SelectedOption { get; set; }
        }
    }
}
