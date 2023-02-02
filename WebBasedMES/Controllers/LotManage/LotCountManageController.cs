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
using WebBasedMES.Services.Repositories.Lots;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Lot;

namespace WebBasedMES.Controllers.LotManage
{
    public class LotCountManageController : ControllerBase
    {
        private readonly ILogger<LotCountManageController> _logger;
        private readonly ILotCountMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public LotCountManageController(ILogger<LotCountManageController> logger, ILotCountMngRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;


            _db = db;
            _userManager = userManager;
        }


        [HttpPost("/api/lot/createLotCount")]
        public async Task<IActionResult> CreateLotCount([FromBody] LotCountRequestCrud lotCountRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateLotCount(lotCountRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/lot/updateLotCount")]
        public async Task<IActionResult> UpdateLotCount([FromBody] LotCountRequestCrud lotCountRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateLotCount(lotCountRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/lot/deleteLotCount")] 
        public async Task<IActionResult> DeleteLotCount([FromBody] LotCountRequestCrud lotCountRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteLotCount(lotCountRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/lotCount/editEvent")]
        public async Task<IActionResult> editEvent([FromBody] LotCountRequestCrud lotCountRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.editEvent(lotCountRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //[HttpPost("/api/lotCount/procesureT1s")]
        //public async Task<IActionResult> procesureT1s([FromBody] LotCountRequestCrud lotCountRequest)
        //{
        //    var result = await _repo.procesureT1s(lotCountRequest);
        //    if (result.IsSuccess)
        //        return Ok(result);
        //    else
        //        return BadRequest(result);
        //}

        [HttpPost("/api/lotCount/testProduct")]
        public async Task<IActionResult> productList([FromBody] OrderReq002 OrderReq002, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.productList(OrderReq002);
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
