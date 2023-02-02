using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Services.JwtAuth;
using WebBasedMES.Services.Repositories.BarcodeManage;
using WebBasedMES.ViewModels.Barcode;
using WebBasedMES.ViewModels.User;

namespace WebBasedMES.Controllers.BarcodeManage
{
    public class BarcodeController: ControllerBase
    {        

        private readonly ILogger<BarcodeController> _logger;
        private readonly IBarcodeRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public BarcodeController(ILogger<BarcodeController> logger, IBarcodeRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;

        }


        [HttpPost("/api/barcode/barcode-issue/getInBarcodeMaster")]
        public async Task<IActionResult> GetInBarcodeIssueList([FromBody] InBarcodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetInBarcodeIssueList(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/barcode/barcode-issue/getInBarcodeData")]
        public async Task<IActionResult> GetInBarcodeIssueData([FromBody] InBarcodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetInBarcodeIssueData(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/barcode/barcode-issue/updateInBarcodeIssue")]
        public async Task<IActionResult> UpdateInBarcodeIssue([FromBody] InBarcodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateInBarcodeIssue(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/barcode/barcode-issue/getOutBarcodeMaster")]
        public async Task<IActionResult> GetOutBarcodeIssueList([FromBody] OutBarcodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetOutBarcodeIssueList(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/barcode/barcode-issue/getOutBarcodeData")]
        public async Task<IActionResult> GetOutBarcodeIssueData([FromBody] OutBarcodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetOutBarcodeIssueData(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/barcode/barcode-issue/updateOutBarcodeIssue")]
        public async Task<IActionResult> UpdateOutBarcodeIssue([FromBody] OutBarcodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOutBarcodeIssue(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/barcode/barcode-issue/getProcessBarcodeMaster")]
        public async Task<IActionResult> GetProcessBarcodeIssueList([FromBody] ProcessBarcodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessBarcodeIssueList(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/barcode/barcode-issue/getProcessBarcodeData")]
        public async Task<IActionResult> GetProcessBarcodeIssueData([FromBody] ProcessBarcodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProcessBarcodeIssueData(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/barcode/barcode-issue/updateProcessBarcodeIssue")]
        public async Task<IActionResult> UpdateProcessBarcodeIssue([FromBody] ProcessBarcodeRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateProcessBarcodeIssue(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }




        [HttpPost("/api/barcode/barcode-issue/getMasterBarcodeMaster")]
        public async Task<IActionResult> GetMasterBarcodeIssueList([FromBody] BarcodeMasterRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMasterBarcodeIssueList(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/barcode/barcode-issue/getMasterBarcodeData")]
        public async Task<IActionResult> GetMasterBarcodeIssueData([FromBody] BarcodeMasterRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMasterBarcodeIssueData(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/barcode/barcode-issue/updateMasterBarcodeIssue")]
        public async Task<IActionResult> UpdateMasterBarcodeIssue([FromBody] BarcodeMasterRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateMasterBarcodeIssue(req);
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
