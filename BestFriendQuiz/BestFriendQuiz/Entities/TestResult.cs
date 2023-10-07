using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestFriendQuiz.EntityLayer.Concrete
{
    public class TestResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Note { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
