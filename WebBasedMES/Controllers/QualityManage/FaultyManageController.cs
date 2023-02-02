using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Services.Repositories.Quality;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Controllers.QualityManage
{
    public class FaultyManageController : ControllerBase
    {
        private readonly ILogger<FaultyManageController> _logger;
        private readonly IFaultyMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public FaultyManageController(ILogger<FaultyManageController> logger, IFaultyMngRepository repo, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }

        //new) 1) 기타불량등록 메인 화면 -목록
        [HttpPost("/api/quality/faulty/mst/list")]
        public async Task<IActionResult> faultyMstList([FromBody] FaultyReq001 faultyReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {

            var result = await _repo.faultyMstList(faultyReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        // 2) 등록/수정화면 기타불량등록 master 정보
        [HttpPost("/api/quality/faulty/mstPop")]
        public async Task<IActionResult> faultyMstPop([FromBody] FaultyReq001 faultyReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.faultyMstPop(faultyReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/quality/faulty/mstEditPop")]
        public async Task<IActionResult> EtcDefectiveUpdateSearchLot([FromBody] FaultyReq001 faultyReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.EtcDefectiveUpdateSearchLot(faultyReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        // 2) 등록/수정화면 목록
        [HttpPost("/api/quality/faulty/product/list")]
        public async Task<IActionResult> faultyProductList([FromBody] FaultyReq001 faultyReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {

            var result = await _repo.faultyProductList(faultyReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        //기타불량등록 cud
        [HttpPost("/api/quality/faulty/getEtcDefective")]
        public async Task<IActionResult> CreateEtcDefective([FromBody] FaultyReq001 faultyReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.getEtcDefective(faultyReq001);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/quality/faulty/createEtcDefective")]
        public async Task<IActionResult> CreateEtcDefective([FromBody] CreateEtcDefectiveRequest faultyReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateEtcDefective(faultyReq001);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/quality/faulty/updateEtcDefective")]
        public async Task<IActionResult> UpdateEtcDefective([FromHeader(Name = "Uuid")] string Uuid)
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var json2 =  json.Replace("$id", "id");

            CreateEtcDefectiveRequest _req = JsonSerializer.Deserialize<CreateEtcDefectiveRequest>(json2);


            var result = await _repo.UpdateEtcDefective(_req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
            
        }




        [HttpPost("/api/quality/faulty/deleteEtcDefective")]
        public async Task<IActionResult> DeleteEtcDefective([FromBody] FaultyReq001 faultyReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteEtcDefective(faultyReq001);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("api/quality/faulty/getDefectivesPop")]
        public async Task<IActionResult> GetDefectives([FromBody] DefectivePopupRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetDefectives(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("api/quality/faulty/getProductsPop")]
        public async Task<IActionResult> GetProducts([FromBody] ProductPopupRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProducts(req);
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

        //기타불량등록 cud

    }
}
