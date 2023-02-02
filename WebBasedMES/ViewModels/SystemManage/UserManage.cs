using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.SystemManage
{
    public class UserManageResponse
    {
        public string Uuid { get; set; }
        public string IdNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public int IsApproved { get; set; }
        public string PhoneNumber { get; set; }
        public int IsOut { get; set; }
        public string BirthDay { get; set; }
        public string InDate { get; set; }
        public string OutDate { get; set; }
        public string EmployType { get; set; }
        public string UserRole { get; set; }
        public string Memo { get; set; }
        public List<Menu> Menus { get; set; }
        public List<SubMenu> FavoriteMenu { get; set; }

    }
    public class UserManageRequest
    {
        public string Uuid { get; set; }
        public string[] Uuids { get; set; }
        public string IdNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public int IsApproved { get; set; }
        public string PhoneNumber { get; set; }
        public int IsOut { get; set; }
        public string BirthDay { get; set; }
        public string InDate { get; set; }
        public string OutDate { get; set; }
        public string EmployType { get; set; }
        public string UserRole { get; set; }
        public string Memo { get; set; }

        public string SearchInput { get; set; }
        public string SearchStr { get; set; }
        public string IsApprovedStr { get; set; }
        public string RegisterIsApproved { get; set; }
    }

    public class UserMenuResponse
    {
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public List<Menu> Menus { get; set; }
    }

    public class UserMenuRequest
    {
        public List<Menu> Menus { get; set; }
        public string Uuid { get; set; }
    }


    public class MenuResponse
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
        public List<SubMenuResponse> FavoriteMenu { get; set; }
        public List<SubMenuResponse> Submenu { get; set; }
        public string Type { get; set; } = "";

    }
    public class SubMenuResponse
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

    public class UserPopupResponse
    {
        public string RegisterName { get; set; }
        public string RegisterId { get; set; }
        public string RegisterDepartment { get; set; }
        public string RegisterIsApproved { get; set; }
        public string RegisterPhoneNumber { get; set; }
        public string RegisterEmail { get; set; }
        public string RegisterNo { get; set; }

        public string UserFullName { get; set; }
        public string UserId { get; set; }
        public string UserDepartment { get; set; }
        public string UserPosition { get; set; }
        public string UserIsApproved { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public string UserNo { get; set; }
    }

    public class UserLogRequest
    {
        public string SearchStr { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class UserLogResponse
    {
        public string CreateTime { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string UserName { get; set; }

        public string Message { get; set; }
        public string Ipv4 { get; set; }
        public string Location { get; set; }
    }



}
