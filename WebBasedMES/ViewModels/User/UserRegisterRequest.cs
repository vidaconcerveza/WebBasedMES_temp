using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.ViewModels.User
{
    public class UserRegisterRequest
    {
        public string UserId { get; set; }
        public string IdNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int Position { get; set; }
        public int Department { get; set; }
    }
}
