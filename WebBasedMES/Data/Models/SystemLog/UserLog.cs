using System;

namespace WebBasedMES.Data.Models.SystemLog
{
    public class UserLog
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsLogIn { get; set; }
        public bool IsLogOut { get;set; }

        public string ResultMessage { get; set; }
        public string Location { get; set; }
        public string Ipv4 { get; set; }
    }
}
