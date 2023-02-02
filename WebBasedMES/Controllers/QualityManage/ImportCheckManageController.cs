using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Services.Repositories.Quality;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Controllers.QualityManage
{
    public class ImportCheckManageController : ControllerBase
    {
        private readonly ILogger<ImportCheckManageController> _logger;
        private readonly IImportCheckMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ImportCheckManageController(ILogger<ImportCheckManageController> logger, IImportCheckMngRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db
)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }
        // 1) 수입검사현황 메인화면
        [HttpPost("/api/quality/importCheck/mst/list")]
        public async Task<IActionResult> importCheckMstList([FromBody] ImportCheckReq001 importCheckReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            
            var result = await _repo.importCheckMstList(importCheckReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        // 2) 메인화면에서 목록 중 하나 더블클릭 팝업
        [HttpPost("/api/quality/importCheck/mstPop")]
        public async Task<IActionResult> importCheckMstPop([FromBody] ImportCheckReq001 importCheckReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.importCheckMstPop(importCheckReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        // 2) 상세내역
        [HttpPost("/api/quality/importCheck/inspection/list")]
        public async Task<IActionResult> importCheckInspectionList([FromBody] ImportCheckReq001 importCheckReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {

            var result = await _repo.importCheckInspectionList(importCheckReq001);
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
                        Stream respStream = res.GetResponseStream();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }



    }
}
