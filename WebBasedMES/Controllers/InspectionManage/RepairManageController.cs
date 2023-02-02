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
using WebBasedMES.Services.Repositories.Bom;
using WebBasedMES.Services.Repositories.InspectionRepairManage;
using WebBasedMES.ViewModels.Bom;
using WebBasedMES.ViewModels.InspectionRepair;

namespace WebBasedMES.Controllers.Bom
{
    public class RepairManageController : ControllerBase
    {
        private readonly ILogger<RepairManageController> _logger;
        private readonly IRepairManageRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public RepairManageController(ILogger<RepairManageController> logger, IRepairManageRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }

        [HttpPost("/api/repair-manage/getRepairAsks")]
        public async Task<IActionResult> GetRepairAsks([FromBody] RepairAskRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetRepairAsks(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/repair-manage/getRepairAsk")]
        public async Task<IActionResult> GetRepairAsk([FromBody] RepairAskRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetRepairAsk(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/repair-manage/createRepairAsk")]
        public async Task<IActionResult> CreateRepairAsk([FromBody] RepairAskCreateUpdateRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateRepairAsk(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/repair-manage/updateRepairAsk")]
        public async Task<IActionResult> UpdateRepairAsk([FromBody] RepairAskCreateUpdateRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateRepairAsk(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/repair-manage/deleteRepairAsk")]
        public async Task<IActionResult> DeleteRepairAsk([FromBody] RepairAskRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteRepairAsk(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/repair-manage/getRepairLogs")]
        public async Task<IActionResult> GetRepairLogs([FromBody] RepairAskRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetRepairLogs(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/repair-manage/getRepairLog")]
        public async Task<IActionResult> GetRepairLog([FromBody] RepairLogRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetRepairLog(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/repair-manage/updateRepairLog")]
        public async Task<IActionResult> UpdateRepairLog([FromBody] RepairLogCreateUpdateRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateUpdateRepairLog(req);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
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
