using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebBasedMES.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public DateTime InDate { get; set; }
        public DateTime OutDate { get; set; }
        public DateTime BirthDay { get; set; }
        public Position Position { get; set; }
        public Department Department { get; set; }
        public UserRole UserRole { get; set; }
        public EmployType EmployType { get; set; }
        public bool IsApproved { get; set; }
        public string TokenValidTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsOut { get; set; } = false;
        public List<Menu> Menu { get; set; }

        public string LoginIp { get; set; } = "";
        public string Memo { get; set; }
        public int LoginFailCount { get; set; } = 0;
        public DateTime LoginFailTimeout { get; set; } = DateTime.UtcNow.AddHours(9);
    }

    public class Menu
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public bool IsAbleToAccess { get; set; } = true;
        public bool IsAbleToRead { get; set; } = true;
        public bool IsAbleToReadWrite { get; set; } = true;
        public bool IsAbleToDelete { get; set; } = true;

        public List<SubMenu> Submenu { get; set; }
        public string Type { get; set; } = "";

    }
    public class SubMenu
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsAbleToAccess { get; set; } = true;
        public bool IsAbleToRead { get; set; } = true;
        public bool IsAbleToReadWrite { get; set; } = true;
        public bool IsAbleToDelete { get; set; } = true;
        public bool IsFavorite { get; set; } = false;
        public string Type { get; set; } = "";

    }

    public class EmployType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;
    }

    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
    }
}
