using System.ComponentModel.DataAnnotations;

namespace BestFriendQuiz.Models
{
    public class GenericViewModel
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Password { get; set; }
        public string? RePassword { get; set; }
        public string? Email { get; set; }

        
     
    }
}
