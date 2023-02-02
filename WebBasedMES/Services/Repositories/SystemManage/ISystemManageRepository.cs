using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.SystemManage;

namespace WebBasedMES.Services.Repositories.SystemManage
{
    public interface ISystemManageRepository
    {
        Task<Response<IEnumerable<UserPopupResponse>>> GetUsersPopupBySearch(UserManageRequest user);
        Task<Response<IEnumerable<UserManageResponse>>> GetUsersBySearch(UserManageRequest user);
        Task<Response<IEnumerable<UserManageResponse>>> GetAllUsers();
        Task<Response<IEnumerable<UserManageResponse>>> GetUsers(string[] uuids);
        Task<Response<UserManageResponse>> GetUser(string uuid);
        Task<Response<IEnumerable<UserManageResponse>>> CreateUser(UserManageRequest user);
        Task<Response<IEnumerable<UserManageResponse>>> ReplaceUser(List<UserManageRequest> users);
        Task<Response<IEnumerable<UserManageResponse>>> UpdateUser(UserManageRequest user, string uuid);
        Task<Response<IEnumerable<UserManageResponse>>> DeleteUser(string[] uuid);
        Task<Response<IEnumerable<UserManageResponse>>> InitializeUserPassword(string[] uuid);

        Task<Response<UserMenuResponse>> GetUserMenus(string uuid);
        Task<Response<UserManageResponse>> UpdateUserMenus(UserMenuRequest menu, string uuid);
        Task<Response<UserManageResponse>> UpdateUserMenusByPost(UserMenuRequest menu);
        Task<Response<BusinessInfo>> GetBusinessInfo();
        Task<Response<BusinessInfo>> UpdateBusinessInfo(BusinessInfo info);



        Task<Response<IEnumerable<NoticeManageResponse>>> GetNoticesBySearch(NoticeManageRequest notice);
        Task<Response<IEnumerable<NoticeManageResponse>>> GetNoticesToMain(NoticeManageRequest notice);
        Task<Response<IEnumerable<NoticeManageResponse>>> GetAllNotices();
        Task<Response<NoticeManageResponse>> GetNotice(int Id);
        Task<Response<IEnumerable<NoticeManageResponse>>> CreateNotice(NoticeManageRequest notice);
        Task<Response<IEnumerable<NoticeManageResponse>>> UpdateNotice(NoticeManageRequest notice);
        Task<Response<IEnumerable<NoticeManageResponse>>> DeleteNotice(NoticeManageRequest notice);
        Task<Response<IEnumerable<UserLogResponse>>> GetUserLogs(UserLogRequest req);


        Task Save();
    }
}