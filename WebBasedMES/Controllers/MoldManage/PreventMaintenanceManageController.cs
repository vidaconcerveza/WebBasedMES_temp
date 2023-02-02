using ClosedXML.Excel;
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
using WebBasedMES.Services.Repositories.MoldManage;
using WebBasedMES.ViewModels.Mold;

namespace WebBasedMES.Controllers.MoldManage
{
    public class PreventMaintenanceManageController : ControllerBase
    {
        private readonly ILogger<PreventMaintenanceManageController> _logger;
        private readonly IPreventiveMaintenanceRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public PreventMaintenanceManageController(ILogger<PreventMaintenanceManageController> logger, IPreventiveMaintenanceRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }


        [HttpPost("/api/mold/preventive-maintenance-facility/getPreventiveMaintenanceFacility")]
        public async Task<IActionResult> GetPreventiveMaintenanceFacility([FromBody] PreventiveMaintenanceFacilityRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPreventiveMaintnanceFacility(_req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/preventive-maintenance-facility/getPreventiveMaintenanceFacilitys")]
        public async Task<IActionResult> GetPreventiveMaintenanceFacilitys([FromBody] PreventiveMaintenanceFacilityRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPreventiveMaintnanceFacilitys(_req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/preventive-maintenance-facility/createPreventiveMaintenanceFacility")]
        public async Task<IActionResult> CreatePreventiveMaintenanceFacilitys([FromBody] PreventiveMaintenanceFacilityRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreatePreventiveMaintnanceFacilitys(_req);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/preventive-maintenance-facility/updatePreventiveMaintenanceFacility")]
        public async Task<IActionResult> UpdatePreventiveMaintenanceFacilitys([FromBody] PreventiveMaintenanceFacilityRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdatePreventiveMaintnanceFacilitys(_req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/preventive-maintenance-facility/deletePreventiveMaintenanceFacility")]
        public async Task<IActionResult> DeletePreventiveMaintenanceFacilitys([FromBody] PreventiveMaintenanceFacilityRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeletePreventiveMaintnanceFacilitys(_req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/preventive-maintenance-mold/getPreventiveMaintenanceMold")]
        public async Task<IActionResult> GetPreventiveMaintenanceMold([FromBody] PreventiveMaintenanceMoldRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPreventiveMaintnanceMold(_req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/preventive-maintenance-mold/getPreventiveMaintenanceMolds")]
        public async Task<IActionResult> GetPreventiveMaintenanceMolds([FromBody] PreventiveMaintenanceMoldRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPreventiveMaintnanceMolds(_req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/preventive-maintenance-mold/createPreventiveMaintenanceMold")]
        public async Task<IActionResult> CreatePreventiveMaintenanceMolds([FromBody] PreventiveMaintenanceMoldRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreatePreventiveMaintnanceMolds(_req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/mold/preventive-maintenance-mold/updatePreventiveMaintenanceMold")]
        public async Task<IActionResult> UpdatePreventiveMaintenanceMolds([FromBody] PreventiveMaintenanceMoldRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdatePreventiveMaintnanceMolds(_req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/preventive-maintenance-mold/deletePreventiveMaintenanceMold")]
        public async Task<IActionResult> DeletePreventiveMaintenanceMolds([FromBody] PreventiveMaintenanceMoldRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeletePreventiveMaintnanceMolds(_req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }




        //예방보존관리

        [HttpPost("/api/mold/perventive-maintenance/getPreventiveMaintenanceManageFacilitys")]
        public async Task<IActionResult> GetPreventiveMaintenanceManageFacilitys([FromBody] PreventiveMaintenanceFacilityRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPreventiveMaintenanceManageFacilitys(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/mold/perventive-maintenance/getPreventiveMaintenanceManageMolds")]
        public async Task<IActionResult> GetPreventiveMaintenanceManageMolds([FromBody] PreventiveMaintenanceMoldRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetPreventiveMaintenanceManageMolds(req);
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
