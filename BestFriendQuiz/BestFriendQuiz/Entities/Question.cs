using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestFriendQuiz.EntityLayer.Concrete
{
    public class Question
    {
        public int Id { get; set; } 

        public string? Quest { get; set; }
        public string OptionA { get; set; }

        public string OptionB { get; set; }

        public string OptionC { get; set; }

        public string OptionD { get; set; }

        public string OptionE { get; set; }

        public bool Approval { get; set; }

    }
}
