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
    public class ProcessCheckDefectiveManageController : ControllerBase
    {
        private readonly ILogger<ProcessCheckDefectiveManageController> _logger;
        private readonly IProcessCheckDefectiveMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProcessCheckDefectiveManageController(ILogger<ProcessCheckDefectiveManageController> logger, IProcessCheckDefectiveMngRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }
        // 1) 공정검사 불량현황 메인화면
        [HttpPost("/api/quality/processCheckDefective/mst/list")]
        public async Task<IActionResult> processCheckDefectiveMstList([FromBody] ProcessCheckDefectiveReq001 processCheckDefectiveReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            
            var result = await _repo.processCheckDefectiveMstList(processCheckDefectiveReq001);
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
        [HttpPost("/api/quality/processCheckDefective/mstPop")]
        public async Task<IActionResult> processCheckDefectiveMstPop([FromBody] ProcessCheckDefectiveReq001 processCheckDefectiveReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.processCheckDefectiveMstPop(processCheckDefectiveReq001);
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
        [HttpPost("/api/quality/processCheckDefective/defective/list")]
        public async Task<IActionResult> processCheckDefectiveList([FromBody] ProcessCheckDefectiveReq001 processCheckDefectiveReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {

            var result = await _repo.processCheckDefectiveList(processCheckDefectiveReq001);
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
