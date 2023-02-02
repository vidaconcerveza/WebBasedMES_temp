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
    public class InspectionManageController : ControllerBase
    {
        private readonly ILogger<InspectionManageController> _logger;
        private readonly IInspectionManageRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public InspectionManageController(ILogger<InspectionManageController> logger, IInspectionManageRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }

        [HttpPost("/api/inspection-manage/getFacilityInspections")]
        public async Task<IActionResult> GetFacilityInspections([FromBody] FacilityInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityInspections(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/getFacilityInspection")]
        public async Task<IActionResult> GetFacilityInspection([FromBody] FacilityInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityInsepction(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/getFacilityInspectionItems")]
        public async Task<IActionResult> GetFacilityInspectionItems([FromBody] FacilityInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityInspectionItems(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/createFacilityInspection")]
        public async Task<IActionResult> CreateFacilityInspection([FromBody] FacilityInspectionCreateUpdateRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateFacilityInspection(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/updateFacilityInspection")]
        public async Task<IActionResult> UpdateFacilityInspection([FromBody] FacilityInspectionCreateUpdateRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateFacilityInspection(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/deleteFacilityInspection")]
        public async Task<IActionResult> DeleteFacilityInspection([FromBody] FacilityInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteFacilityInspection(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "식제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }




        [HttpPost("/api/inspection-manage/getMoldInspections")]
        public async Task<IActionResult> GetMoldInspections([FromBody] MoldInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldInspections(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/getMoldInspection")]
        public async Task<IActionResult> GetMoldInspection([FromBody] MoldInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldInsepction(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/getMoldInspectionItems")]
        public async Task<IActionResult> GetMoldInspectionItems([FromBody] MoldInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldInspectionItems(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/createMoldInspection")]
        public async Task<IActionResult> CreateMoldInspection([FromBody] MoldInspectionCreateUpdateRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateMoldInspection(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/updateMoldInspection")]
        public async Task<IActionResult> UpdateMoldInspection([FromBody] MoldInspectionCreateUpdateRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateMoldInspection(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inspection-manage/deleteMoldInspection")]
        public async Task<IActionResult> DeleteMoldInspection([FromBody] MoldInspectionRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteMoldInspection(req);
            if (result.IsSuccess)
            {
                string dataSize ="0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
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
