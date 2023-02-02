using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Services.Repositories.FacilityManage;
using WebBasedMES.ViewModels.FacilityManage;
using WebBasedMES.ViewModels.Lot;
using WebBasedMES.Data.Models;

namespace WebBasedMES.Controllers.FacilityManage
{
    public class FacilityManageController : ControllerBase
    {
        private readonly ILogger<FacilityManageController> _logger;
        private readonly IFacilityManageRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public FacilityManageController(ILogger<FacilityManageController> logger, IFacilityManageRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }


        [HttpPost("/api/facility-manage/getFacilityBaseInfo")]
        public async Task<IActionResult> GetFacilityBaseInfo([FromBody] FacilityBaseInfoRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityBaseInfo(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/getFacilityBaseInfos")]
        public async Task<IActionResult> GetFacilityBaseInfos([FromBody] FacilityBaseInfoRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityBaseInfos(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/createFacilityBaseInfo")]
        public async Task<IActionResult> CreateFacilityBaseInfo([FromBody] FacilityBaseInfoRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateFacilityBaseInfo(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/updateFacilityBaseInfo")]
        public async Task<IActionResult> UpdateFacilityBaseInfo([FromBody] FacilityBaseInfoRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateFacilityBaseInfo(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/deleteFacilityBaseInfo")]
        public async Task<IActionResult> DeleteFacilityBaseInfo([FromBody] FacilityBaseInfoRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteFacilityBaseInfo(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/getFacilityControl")]
        public async Task<IActionResult> GetFacilityControl([FromBody] FacilityControlRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityControl(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/getFacilityControls")]
        public async Task<IActionResult> GetFacilityControls([FromBody] FacilityControlRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityControls(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/createFacilityControl")]
        public async Task<IActionResult> CreateFacilityControl([FromBody] FacilityControlRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateFacilityControl(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        [HttpPost("/api/facility-manage/updateFacilityControl")]
        public async Task<IActionResult> UpdateFacilityControl([FromBody] FacilityControlRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateFacilityControl(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/deleteFacilityControl")]
        public async Task<IActionResult> DeleteFacilityControl([FromBody] FacilityControlRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteFacilityControl(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        [HttpPost("/api/facility-manage/getFacilityErrorLog")]
        public async Task<IActionResult> GetFacilityErrorLog([FromBody] FacilityErrorLogRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityErrorLog(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }


        //설비가동관리
        [HttpPost("/api/facility-manage/getFacilityOperations")]
        public async Task<IActionResult> GetFacilityOperations([FromBody] FacilityOperationRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityOperations(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/getFacilityOperation")]
        public async Task<IActionResult> GetFacilityOperation([FromBody] FacilityOperationRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetFacilityOperation(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/createFacilityOperation")]
        public async Task<IActionResult> CreateFacilityOperation([FromBody] FacilityOperationRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateFacilityOperation(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/updateFacilityOperation")]
        public async Task<IActionResult> UpdateFacilityOperation([FromBody] FacilityOperationRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateFacilityOperation(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/facility-manage/deleteFacilityOperation")]
        public async Task<IActionResult> DeleteFacilityOperation([FromBody] FacilityOperationRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteFacilityOperation(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";

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
