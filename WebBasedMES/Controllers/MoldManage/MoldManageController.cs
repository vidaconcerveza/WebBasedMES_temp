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
    public class MoldManageController : ControllerBase
    {
        private readonly ILogger<MoldManageController> _logger;
        private readonly IMoldRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public MoldManageController(ILogger<MoldManageController> logger, IMoldRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;

        }

        #region Mold Manage
        [HttpPost("/api/mold/mold-master/getMoldMasterList")]
        public async Task<IActionResult> GetMoldMasterList([FromBody] MoldRequest002 _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldMasterList(_req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/mold/mold-master/getMolds")]
        public async Task<IActionResult> GetMoldsBySearch([FromBody] MoldRequest002 _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldsBySearch(_req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/mold/mold-master/getMold")]
        public async Task<IActionResult> GetMold2([FromBody] MoldRequest002 _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMold(_req.MoldId);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpGet("/api/mold/mold-master/{id}")]
        public async Task<IActionResult> GetMold(int id, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMold(id);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpGet("/api/mold/mold-master/all/{code}")]
        public async Task<IActionResult> GetMolds(int code, [FromHeader(Name = "Uuid")] string Uuid)
        {
            //Code 는 공통코드. 공통코드에 따라 내보내줍니다.
            var result = await _repo.GetMolds(code);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/mold-master/createMold")]
        public async Task<IActionResult> CreateMold([FromBody] MoldRequest mold, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateMold(mold);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/mold-master/updateMold")]
        public async Task<IActionResult> UpdateMold([FromBody] MoldRequest mold, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateMold(mold, mold.MoldId);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/mold-master/deleteMold")]
        public async Task<IActionResult> DeleteMold([FromBody] MoldRequest mold, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteMolds(mold.MoldIds);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        /*
        [HttpGet("/api/mold/mold-master/file")]
        public async Task<IActionResult> DownloadMoldList ()
        {
            var _mold = _repo.GetMolds(0);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "품목리스트.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("설비");

                worksheet.Cell("A1").Value = "설비코드";
                worksheet.Cell("B1").Value = "설비구분";
                worksheet.Cell("C1").Value = "설비이름";
                worksheet.Cell("D1").Value = "규격";
                worksheet.Cell("E1").Value = "브랜드명";
                worksheet.Cell("F1").Value = "모델명";
                worksheet.Cell("G1").Value = "구매처";
                worksheet.Cell("H1").Value = "구매일";
                worksheet.Cell("I1").Value = "구매금액";
                worksheet.Cell("J1").Value = "UID(IP)";
                worksheet.Cell("K1").Value = "전류최대값";
                worksheet.Cell("L1").Value = "톤최대값";
                worksheet.Cell("M1").Value = "비고";
                worksheet.Cell("N1").Value = "사용여부";

                int i = 0;
                foreach (var mold in _mold.Result.Data)
                {
                    i++;
                    worksheet.Cell("A" + (i + 1).ToString()).Value = mold.Code;
                    worksheet.Cell("B" + (i + 1).ToString()).Value = mold.CommonCodeName;
                    worksheet.Cell("C" + (i + 1).ToString()).Value = mold.Name;
                    worksheet.Cell("D" + (i + 1).ToString()).Value = mold.Standard;
                    worksheet.Cell("E" + (i + 1).ToString()).Value = mold.Brand;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = mold.Model;
                    worksheet.Cell("F" + (i + 1).ToString()).Value = mold.Agent;
                    worksheet.Cell("G" + (i + 1).ToString()).Value = mold.PurchaseDate;
                    worksheet.Cell("H" + (i + 1).ToString()).Value = mold.Price.ToString("C");
                    worksheet.Cell("I" + (i + 1).ToString()).Value = mold.Uid;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = mold.MaxCurrent;
                    worksheet.Cell("J" + (i + 1).ToString()).Value = mold.MaxTon;
                    worksheet.Cell("K" + (i + 1).ToString()).Value = mold.Memo;
                    worksheet.Cell("L" + (i + 1).ToString()).Value = mold.IsUsing ? "사용" : "미사용";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost("/api/mold/mold-master-manage/upload")]
        public async Task<IActionResult> UpdateMolds ([FromForm] FileModel file)
        {
            var _file = file;
            var stream = file.FormFile.OpenReadStream();

            var workbook = new XLWorkbook(stream);
            var workSheet = workbook.Worksheets.First();
            var range = workSheet.RangeUsed();

            List<MoldResponse> mold = new List<MoldResponse>();
            try
            {
                for (int i = 1; i < range.RowCount(); i++)
                {
                    var _mold = new MoldResponse
                    {
                        Code = workSheet.Cell(i + 1, 1).Value.ToString(),
                        CommonCodeName = workSheet.Cell(i + 1, 2).Value.ToString(),
                        Name = workSheet.Cell(i + 1, 3).Value.ToString(),
                        Standard = workSheet.Cell(i + 1, 4).Value.ToString(),
                        Brand = workSheet.Cell(i + 1, 5).Value.ToString(),
                        Model = workSheet.Cell(i + 1, 6).Value.ToString(),
                        Agent = workSheet.Cell(i + 1, 7).Value.ToString(),
                        PurchaseDate = workSheet.Cell(i + 1, 8).Value.ToString(),
                        Price = int.Parse(workSheet.Cell(i + 1, 9).Value.ToString(), System.Globalization.NumberStyles.Currency),
                        Uid = workSheet.Cell(i + 1, 10).Value.ToString(),
                        MaxCurrent = workSheet.Cell(i + 1, 11).Value.ToString(),
                        MaxTon = workSheet.Cell(i + 1, 12).Value.ToString(),
                        Memo = workSheet.Cell(i + 1, 13).Value.ToString(),
                        IsUsing = workSheet.Cell(i + 1, 14).Value.ToString() == "사용" ? true : false
                    };
                    mold.Add(_mold);
                }

                if (mold.Count() < 1)
                {
                    return Ok();
                }
                else
                {
                    var result = await _repo.UpdateMolds(mold);
                    if (result.IsSuccess)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }
       */



        #endregion Mold Manage

        #region Mold Group
        [HttpPost("/api/mold/mold-group/getMoldGroup")]
        public async Task<IActionResult> GetMoldGroup([FromBody] MoldGroupRequest mold, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldGroup(mold);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/mold-group/getMoldGroupDetail")]
        public async Task<IActionResult> GetMoldGroupDetail([FromBody] MoldGroupRequest mold, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldGroupDetail(mold);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/mold-group/getMoldGroups")]
        public async Task<IActionResult> GetMoldGroups([FromBody] MoldGroupRequest mold, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldGroups(mold);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/mold-group/createMoldGroup")]
        public async Task<IActionResult> CreateMoldGroup([FromBody] MoldGroupRequest mold, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateMoldGroup(mold);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/mold-group/{id}")]
        public async Task<IActionResult> UpdateMoldGroup([FromBody] MoldGroupRequest mold, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateMoldGroup(mold);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/mold-group/deleteMoldGroup")]
        public async Task<IActionResult> DeleteMoldGroup([FromBody] MoldGroupRequest mold, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteMoldGroups(mold);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        #endregion Mold Group

        #region Mold Cleaning
        [HttpPost("/api/mold/mold-cleaning/getMoldCleaning")]
        public async Task<IActionResult> GetMoldCleaning([FromBody] MoldCleaningRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldCleanings(req);

            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/mold/mold-cleaning/updateMoldCleaning")]
        public async Task<IActionResult> UpdateMoldCleaning([FromBody] MoldCleaningRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateMoldCleaing(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        #endregion Mold Cleaning

        #region Mold Draft
        [HttpPost("/api/mold/mold-draft/getMoldDraft")]
        public async Task<IActionResult> GetMoldDraft([FromBody] MoldDraftRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldDrafts(req);

            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }




        [HttpPost("/api/mold/mold-draft/updateMoldDraft")]
        public async Task<IActionResult> UpdateMoldDraft([FromBody] MoldDraftRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateMoldDraft(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }



        #endregion Mold Draft

        #region Mold Location(Position)
        [HttpPost("/api/mold/mold-location/getMoldLocation")]
        public async Task<IActionResult> GetMoldLocation([FromBody] MoldLocationRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldLocation(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/mold/mold-location/updateMoldLocation")]
        public async Task<IActionResult> UpdateMoldLocation([FromBody] MoldLocationRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateMoldLocation(req);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        #endregion


        [HttpPost("/api/mold/mold-usage-record/getMoldUsageMaster")]
        public async Task<IActionResult> GetMoldUsageList([FromBody] MoldUsageRecordRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldUsageList(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/mold/mold-usage-record/getMoldUsageRecords")]
        public async Task<IActionResult> GetMoldUsageRecords([FromBody] MoldUsageRecordRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetMoldUsageRecords(req);
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
