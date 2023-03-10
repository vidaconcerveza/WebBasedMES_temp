using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebBasedMES.ViewModels.Lot;
using WebBasedMES.Services.Repositories.Lots;
using Microsoft.AspNetCore.Identity;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net;
using System;

namespace WebBasedMES.Controllers.LotManage
{
    public class LotManageController : ControllerBase
    {
        private readonly ILogger<LotManageController> _logger;
        private readonly ILotMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public LotManageController(ILogger<LotManageController> logger, ILotMngRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;

        }


        [HttpPost("/api/lot/createLot")]
        public async Task<IActionResult> CreateLot([FromBody] LotRequestCrud lotRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateLot(lotRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/lot/updateLot")]
        public async Task<IActionResult> UpdateLot([FromBody] LotRequestCrud lotRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateLot(lotRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/lot/deleteLot")]
        public async Task<IActionResult> DeleteLot([FromBody] LotRequestCrud lotRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteLot(lotRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/lot/invenList")]
        public async Task<IActionResult> invenList([FromBody] LotRequestCrud lotRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.invenList(lotRequest);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/lot/getLots")]
        public async Task<IActionResult> getLots([FromBody] LotRequestCrud lotRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.getLots(lotRequest);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/lot/getLotCounts")]
        public async Task<IActionResult> getLotCounts([FromBody] LotRequestCrud lotRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.getLotCounts(lotRequest);
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
