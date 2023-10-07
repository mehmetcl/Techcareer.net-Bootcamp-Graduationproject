using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestFriendQuiz.EntityLayer.Concrete
{
   
    public class Test
    {
        public int Id { get; set; }
        public List<QuestionOfSurvey> QuestionsOfSurvey { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
