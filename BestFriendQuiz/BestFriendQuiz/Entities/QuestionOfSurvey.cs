using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestFriendQuiz.EntityLayer.Concrete
{
    public class QuestionOfSurvey
    {
        public int Id { get; set; }
        public  Question Question { get; set; }
        public string RightOption { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
