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
    public class VoltageCheckManageController : ControllerBase
    {
        private readonly ILogger<VoltageCheckManageController> _logger;
        private readonly IVoltageCheckMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public VoltageCheckManageController(ILogger<VoltageCheckManageController> logger, IVoltageCheckMngRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }
        // 1) 전압검사 관리 메인 화면
        [HttpPost("/api/quality/voltage-check-manage")]
        public async Task<IActionResult> QualityVoltageCheckManage([FromBody] VoltageCheckReq001 voltageCheckReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            
            var result = await _repo.QualityVoltageCheckManage(voltageCheckReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        //하사점 관리
        [HttpPost("/api/quality/bottom-dead-point-manage")]
        public async Task<IActionResult> QualityBottomDeadPointManage([FromBody] VoltageCheckReq001 voltageCheckReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.QualityBottomDeadPointManage(voltageCheckReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        //슬라이드 관리
        [HttpPost("/api/quality/slide-manage")]
        public async Task<IActionResult> QualitySlideManage([FromBody] VoltageCheckReq001 voltageCheckReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.QualitySlideManage(voltageCheckReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        //슬라이드 관리
        [HttpPost("/api/quality/ton-manage")]
        public async Task<IActionResult> QualityTonManage([FromBody] VoltageCheckReq001 voltageCheckReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.QualityTonManage(voltageCheckReq001);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }



        //전압검사관리_보창프레스 전용
        [HttpPost("/api/quality/getVoltageInspections")]
        public async Task<IActionResult> GetVoltageInspections([FromBody] VoltageInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetVoltageInspections(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/quality/createVoltageInspection")]
        public async Task<IActionResult> CreateVoltageInspections([FromBody] VoltageInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateVoltageInspections(req);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/quality/deleteVoltageInspection")]
        public async Task<IActionResult> DeleteVoltageInspections([FromBody] VoltageInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteVoltageInspections(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/quality/getInspectionData")]
        public async Task<IActionResult> GetInspectionData([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetInspectionData();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/quality/temp-humid-manage")]
        public async Task<IActionResult> GetTempHumids([FromBody] TempHumidRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetTempHumids(req);
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
