using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Services.Repositories.SystemManage;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.File;
using WebBasedMES.ViewModels.SystemManage;
using WebBasedMES.ViewModels.User;

namespace WebBasedMES.Controllers.SystemManage
{
    public class SystemManageController : ControllerBase
    {
        private readonly ILogger<SystemManageController> _logger;
        private readonly ISystemManageRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public SystemManageController(ILogger<SystemManageController> logger, ISystemManageRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }
        #region 사용자 관리





        [HttpPost("/api/system/user-manage/getUsersPopup")]
        public async Task<IActionResult> GetUsersBySearch([FromBody] UserManageRequest user, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetUsersPopupBySearch(user);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/system/user-manage/getUsers")]
        public async Task<IActionResult> GetUsersPopup([FromBody] UserManageRequest user, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetUsersBySearch(user);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }



        [HttpGet("/api/system/user-manage")]
        public async Task<IActionResult> GetAllUsers([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetAllUsers();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        [HttpPost("/api/system/user-manage/getUser")]
        public async Task<IActionResult> GetUser([FromBody] UserManageRequest user, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetUser(user.Uuid);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/system/user-manage/createUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserManageRequest user, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateUser(user);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        [HttpPost("/api/system/user-manage/updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserManageRequest user, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateUser(user, user.Uuid);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        [HttpPost("/api/system/user-manage/deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] UserManageRequest user, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteUser(user.Uuids);
            if (result.IsSuccess)
            {
                string dataSize ="0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/system/user-manage/init-password")]
        public async Task<IActionResult> InitUserPassword([FromBody] UserManageRequest user, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.InitializeUserPassword(user.Uuids);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/system/user-manage/excel-download")]
        public async Task<IActionResult> DownloadUser([FromBody] UserManageRequest req)
        {
            var _users = await _repo.GetUsersBySearch(req);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "사용자 정보.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("사용자");
                worksheet.Cell("A1").Value = "고용형태";
                worksheet.Cell("B1").Value = "사번";
                worksheet.Cell("C1").Value = "이름";
                worksheet.Cell("D1").Value = "권한";
                worksheet.Cell("E1").Value = "부서";
                worksheet.Cell("F1").Value = "직급";
                worksheet.Cell("G1").Value = "입사일";
                worksheet.Cell("H1").Value = "이메일주소";
                worksheet.Cell("I1").Value = "연락처";
                worksheet.Cell("J1").Value = "승인여부";

                int i = 0;
                foreach (var user in _users.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 2).ToString()).Value = user.EmployType;
                    worksheet.Cell("B" + (i + 2).ToString()).Value = user.IdNumber;
                    worksheet.Cell("C" + (i + 2).ToString()).Value = user.FullName;
                    worksheet.Cell("D" + (i + 2).ToString()).Value = user.UserRole;
                    worksheet.Cell("E" + (i + 2).ToString()).Value = user.Department;
                    worksheet.Cell("F" + (i + 2).ToString()).Value = user.Position;
                    worksheet.Cell("G" + (i + 2).ToString()).Value = user.InDate;
                    worksheet.Cell("H" + (i + 2).ToString()).Value = user.Email;
                    worksheet.Cell("I" + (i + 2).ToString()).Value = user.PhoneNumber;
                    worksheet.Cell("J" + (i + 2).ToString()).Value = user.IsApproved == 1? "승인":"미승인";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/system/user-manage/upload-user")]
        public async Task<IActionResult> ReplaceUser([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<UserManageRequest> users = new List<UserManageRequest>();

            for (int i = 2; i < range.RowCount(); i++)
            {
                var user = new UserManageRequest
                {
                    EmployType = workSheet.Cell(i + 1, 1).Value.ToString(),
                    IdNumber = workSheet.Cell(i + 1, 2).Value.ToString(),
                    FullName = workSheet.Cell(i + 1, 3).Value.ToString(),
                    UserRole = workSheet.Cell(i + 1, 4).Value.ToString(),
                    Department = workSheet.Cell(i + 1, 5).Value.ToString(),
                    Position = workSheet.Cell(i + 1, 6).Value.ToString(),
                    InDate = workSheet.Cell(i + 1, 7).Value.ToString(),
                    Email = workSheet.Cell(i + 1, 8).Value.ToString(),
                    PhoneNumber = workSheet.Cell(i + 1, 9).Value.ToString(),
                    IsApproved = workSheet.Cell(i + 1, 10).Value.ToString() == "승인" ? 1 : 0
                };
                users.Add(user);
            }

            if (users.Count() < 1)
            {
                return Ok();
            }
            else
            {
                var result = await _repo.ReplaceUser(users);
                if (result.IsSuccess)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
        }


        #endregion 사용자 관리

        #region 사용자 메뉴 관리(액세스 권한 / 즐겨찾기)

        //Get User Info로 가져와서 실제로는 사용하지 않은 API입니다.
        [HttpGet("/api/system/auth-manage/{uuid}")]
        public async Task<IActionResult> GetUserMenus(string uuid)
        {
            var result = await _repo.GetUserMenus(uuid);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/system/auth-manage/updateUser")]
        public async Task<IActionResult> UpdateUserMenus([FromBody] UserMenuRequest menu, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateUserMenus(menu, menu.Uuid);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/system/auth-manage/updateAccess")]
        public async Task<IActionResult> UpdateUserMenusByPost([FromBody] UserMenuRequest menu, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateUserMenusByPost(menu);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        #endregion 사용자 메뉴 관리(액세스 권한 / 즐겨찾기)

        #region 사업자 관리
        [HttpGet("/api/system/business-manage/info")]
        public async Task<IActionResult> GetBusinessInfo([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetBusinessInfo();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
            {
                return Ok(result);
            }
        }

        [HttpPost("/api/system/business-manage/info")]

        public async Task<IActionResult> UpdateBusinessInfo([FromBody] BusinessInfo info, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateBusinessInfo(info);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
            {
                return BadRequest(result);
            }
        }


        #endregion

        #region 공지사항관리

        [HttpPost("/api/system/notice-manage/getNotices")]
        public async Task<IActionResult> GetNoticesBySearch([FromBody] NoticeManageRequest notice, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetNoticesBySearch(notice);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/system/notice-manage/getNoticesToMain")]
        public async Task<IActionResult> GetNoticesToMain([FromBody] NoticeManageRequest notice, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetNoticesToMain(notice);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/system/notice-manage/getNotice")]
        public async Task<IActionResult> GetNoticesById([FromBody] NoticeManageRequest notice, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetNotice(notice.Id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/system/notice-manage/createNotice")]
        public async Task<IActionResult> CreateNotice_edit([FromBody] NoticeManageRequest notice, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateNotice(notice);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        [HttpPost("/api/system/notice-manage/deleteNotice")]

        public async Task<IActionResult> DeleteNoticeById([FromBody] NoticeManageRequest notice, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteNotice(notice);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        [HttpPost("/api/system/notice-manage/updateNotice")]
        public async Task<IActionResult> UpdateNotice([FromBody] NoticeManageRequest notice, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateNotice(notice);
            if (result.IsSuccess)
            {
                string dataSize ="0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        #endregion 공지사항관리


        [HttpPost("/api/system/userLogs")]
        public async Task<IActionResult> GetUserLogs([FromBody] UserLogRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetUserLogs(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        public async Task SmartFactoryLog(string uuid, string type, string dataSize)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(uuid);
                var _company = await _db.BusinessInfo.FirstOrDefaultAsync();
                if (_user != null)
                {
                    string uri = "https://log.smart-factory.kr/apisvc/sendLogData.json?";
                    uri += ("crtfcKey=" + _company.CrtfcKey + "&");
                    uri += ("logDt=" + DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd HH:mm:ss.fff") + "&");
                    uri += ("useSe=" + type + "&");
                    uri += ("conectIp=" + _user.LoginIp + "&");
                    uri += ("sysUser=" + _user.IdNumber + "&");
                    uri += ("dataUsgqty=" + dataSize);
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.Timeout = 3 * 1000;

                    using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                    {
                        HttpStatusCode status = res.StatusCode;

                        var encoding = Encoding.GetEncoding(res.CharacterSet);
                        Stream respStream = res.GetResponseStream();

                        var reader = new StreamReader(respStream, encoding);
                        string test = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
