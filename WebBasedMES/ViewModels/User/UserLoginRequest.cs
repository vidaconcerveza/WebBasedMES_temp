using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.ViewModels.User
{
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Ipv4 { get; set; }
        public string Location { get; set; }
        public string CountryCode { get; set; }
    }

    public class UserPasswordChangeRequest
    {
        public string Uuid { get; set; }
        public string CurrentPassword { get; set; }
        public string ChangePassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
