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
using WebBasedMES.Services.Repositories.Monitor;

namespace WebBasedMES.Controllers.MonitorManage
{
    public class MonitorManageController : Controller
    {
        private readonly ILogger<MonitorManageController> _logger;
        private readonly IMonitorManageRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public MonitorManageController(ILogger<MonitorManageController> logger, IMonitorManageRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }


        [HttpGet("/api/monitor/process-progress-record")]
        public async Task<IActionResult> GetProcessProgressRecord([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessProgressRecord();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/monitor/process-operation-record")]
        public async Task<IActionResult> GetProcessOperationRecord([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessOperationRecord();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/monitor/production-record-by-contract")]
        public async Task<IActionResult> GetProductionRecordByContract([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductionRecordByContract();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/monitor/production-record-by-plan")]
        public async Task<IActionResult> GetProductionRecordByPlan([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductionRecordByPlan();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/monitor/facility-manage-record")]
        public async Task<IActionResult> GetFacilityManageRecord([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityManageRecord();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/monitor/mold-manage-record-inspection")]
        public async Task<IActionResult> GetMoldManageRecordInspection([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldManageRecordInspection();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/monitor/mold-manage-record-cleaning")]
        public async Task<IActionResult> GetMoldManageRecordCleaning([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldManageRecordWash();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpGet("/api/monitor/facility-temp-record")]
        public async Task<IActionResult> GetFacilityTempRecord([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityTempRecord();
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("/api/monitor/facility-operation-record")]
        public async Task<IActionResult> GetFacilityOperationRecord([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityOperationRecord();
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/monitor/facility-operation-record-save")]
        public async Task<IActionResult> GetFacilityOperationRecordSave([FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityOperationRecordSave();
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
