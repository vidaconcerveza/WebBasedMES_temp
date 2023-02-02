using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.Auth
{
    public class AuthResult
    {
        public string AccessToken { get; set; }
        //public string RefreshToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsSuccess { get; set; }

        public UserInfo UserInfo { get; set; }

        public string Errors { get; set; }
    }

    public class UserInfo
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public string Uuid { get; set; }
        public string UserId { get; set; }
        public string Company { get; set; }
        public string CrtfcKey { get; set; }
        public string ConectIp { get; set; }
        public bool UseModel { get; set; }
        public bool UseMold { get; set; }
        public bool UseTemp { get; set; }
        //public List<MenuResponse> Menus { get; set; }
        public List<AuthMainMenu> Menus { get; set; }
        public List<SubMenu> FavoriteMenu { get; set; }

    }


    public class AuthMainMenu
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }

        public string Icon { get; set; }
        public List<AuthSubMenu> SubMenu { get; set; }

        public bool IsAbleToAccess { get; set; }
        public bool IsAbleToRead { get; set; }
        public bool IsAbleToReadWrite { get; set; }
        public bool IsAbleToDelete { get; set; }
    }

    public class AuthSubMenu
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }

        public string Url { get; set; }

        public bool IsAbleToAccess { get; set; }
        public bool IsAbleToRead { get; set; }
        public bool IsAbleToReadWrite { get; set; }
        public bool IsAbleToDelete { get; set; }
    }
}
