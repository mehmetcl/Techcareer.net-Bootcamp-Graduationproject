using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestFriendQuiz.EntityLayer.Concrete
{
    public class User : IdentityUser<int>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Password { get; set; } 
        public  string Email { get; set; } 
        public Guid Key { get; set; } = Guid.NewGuid();
        public int Limit { get; set; } = 5;
        public int? RoleId { get; set; } 
        public Role Role { get; set; }
    }
}


