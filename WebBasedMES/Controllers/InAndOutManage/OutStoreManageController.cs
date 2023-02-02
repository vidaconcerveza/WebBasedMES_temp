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
using WebBasedMES.Services.Repositories.InAndOut;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Controllers.InAndOutManage
{
    public class OutStoreManageController : ControllerBase
    {
        private readonly ILogger<OutStoreManageController> _logger;
        private readonly IOutStoreMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public OutStoreManageController(ILogger<OutStoreManageController> logger, IOutStoreMngRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }


        [HttpPost("/api/inAndOut/createOutStore")]
        public async Task<IActionResult> CreateOutStore([FromBody] OutStoreRequestCrud OutStoreRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateOutStore(OutStoreRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/updateOutStore")]
        public async Task<IActionResult> UpdateOutStore([FromBody] OutStoreRequestCrud OutStoreRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOutStore(OutStoreRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteOutStore")]
        public async Task<IActionResult> DeleteOutStore([FromBody] OutStoreRequestCrud OutStoreRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteOutStore(OutStoreRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/createOutStoreProduct")]
        public async Task<IActionResult> CreateOutStoreProduct([FromBody] OutStoreProductRequestCrud OutStoreProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateOutStoreProduct(OutStoreProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteOutStoreProduct")]
        public async Task<IActionResult> DeleteOutStoreProduct([FromBody] OutStoreProductRequestCrud OutStoreProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteOutStoreProduct(OutStoreProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/updateOutStoreProduct")]
        public async Task<IActionResult> UpdateOutStoreProduct([FromBody] OutStoreProductRequestCrud OutStoreProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOutStoreProduct(OutStoreProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/inAndOut/OutStore/mst/list")]
        public async Task<IActionResult> OutStoreMstList([FromBody] OutStoreReq001 OutStoreRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outStoreMstList(OutStoreRequest);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/OutStoreProduct/list")]
        public async Task<IActionResult> view03([FromBody] OutStoreReq001 OutStoreReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outStoreProductList(OutStoreReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/OutStore/mstPop")]
        public async Task<IActionResult> OutStoreMstPop([FromBody] OutStoreReq001 OutStoreRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outStoreMstPop(OutStoreRequest);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/OutStoreProduct/listPop")]
        public async Task<IActionResult> OutStoreProductListPop([FromBody] OutStoreReq001 OutStoreRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outStoreProductListPop(OutStoreRequest);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        ////new) 2) 수주등록 등록/수정 버튼 팝업 -품목리스트
        //[HttpPost("/api/inAndOut/productList")]
        //public async Task<IActionResult> productList([FromBody] OutStoreReq002 OutStoreReq002)
        //{
        //    var result = await _repo.productList(OutStoreReq002);
        //    if (result.IsSuccess)
        //        return Ok(result);
        //    else
        //        return BadRequest(result);
        //}
        ////) 3) 2)에서 등록자 검색 버튼 클릭 팝업 리스트
        //[HttpPost("/api/inAndOut/user/list")]
        //public async Task<IActionResult> userList([FromBody] OutStoreReq003 OutStoreReq003)
        //{
        //    var result = await _repo.userList(OutStoreReq003);
        //    if (result.IsSuccess)
        //        return Ok(result);
        //    else
        //        return BadRequest(result);
        //}
        ////) 3) 2)에서 등록자 검색 버튼 클릭 팝업 리스트
        //[HttpPost("/api/inAndOut/partner/list")]
        //public async Task<IActionResult> partnerList([FromBody] OutStoreReq004 OutStoreReq004)
        //{
        //    var result = await _repo.partnerList(OutStoreReq004);
        //    if (result.IsSuccess)
        //        return Ok(result);
        //    else
        //        return BadRequest(result);
        //}

        //) 3) 2)에서 등록자 검색 버튼 클릭 팝업 리스트
        [HttpPost("/api/inAndOut/outstore/editSave")]
        public async Task<IActionResult> OutStoreProductEditSave([FromBody] OutStoreProductRequestCrud OutStoreProductRequestCrud, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.OutStoreProductEditSave(OutStoreProductRequestCrud);
            if (result.IsSuccess)
            {
                string dataSize = "0";
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
