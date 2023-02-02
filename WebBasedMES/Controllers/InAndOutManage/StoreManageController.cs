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
using WebBasedMES.Services.Repositories.InAndOut;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Controllers.InAndOutManage
{
    public class StoreManageController : ControllerBase
    {
        private readonly ILogger<StoreManageController> _logger;
        private readonly IStoreMngRepository _repo;
        private readonly IInvenMngRepository _repo2;

        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public StoreManageController(ILogger<StoreManageController> logger, IStoreMngRepository repo, IInvenMngRepository repo2, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _repo2 = repo2;

            _db = db;
            _userManager = userManager;
        }


        [HttpPost("/api/inAndOut/createStore")]
        public async Task<IActionResult> CreateStore([FromBody] StoreRequestCrud StoreRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateStore(StoreRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/updateStore")]
        public async Task<IActionResult> UpdateStore([FromBody] StoreRequestCrud storeRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateStore(storeRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteStore")]
        public async Task<IActionResult> DeleteStore([FromBody] StoreRequestCrud storeRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteStore(storeRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/createStoreOutStoreProduct")]
        public async Task<IActionResult> CreateStoreOutStoreProduct([FromBody] StoreOutStoreProductRequestCrud storeOutStoreProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateStoreOutStoreProduct(storeOutStoreProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteStoreOutStoreProduct")]
        public async Task<IActionResult> DeleteStoreOutStoreProduct([FromBody] StoreOutStoreProductRequestCrud storeOutStoreProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteStoreOutStoreProduct(storeOutStoreProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/updateStoreOutStoreProduct")]
        public async Task<IActionResult> UpdateStoreOutStoreProduct([FromBody] StoreOutStoreProductRequestCrud storeOutStoreProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateStoreOutStoreProduct(storeOutStoreProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
       
        [HttpPost("/api/inAndOut/store/editSave")]
        public async Task<IActionResult> StoreOutStoreProductEditSave([FromBody] StoreOutStoreProductRequestCrud storeOutStoreProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {

            //var json = await new StreamReader(Request.Body).ReadToEndAsync();
            //var json2 = json.Replace("$id", "id");

          //  StoreOutStoreProductRequestCrud _req = JsonSerializer.Deserialize<StoreOutStoreProductRequestCrud>(storeOutStoreProductRequest);



            var result = await _repo.StoreOutStoreProductEditSave(storeOutStoreProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        //new) 1) 입고등록 메인 화면 -목록
        [HttpPost("/api/inAndOut/store/mst/list")]
        public async Task<IActionResult> storeMstList([FromBody] StoreReq001 storeReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeMstList(storeReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        //new) 1) 입고등록 메인 화면 -상세내역
        [HttpPost("/api/inAndOut/storeOutStoreProduct/list")]
        public async Task<IActionResult> storeOutStoreProductList([FromBody] StoreReq001 storeReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeOutStoreProductList(storeReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        //new) 2) 입고등록 등록/수정 버튼 팝업 -마스터
        [HttpPost("/api/inAndOut/store/mstPop")]
        public async Task<IActionResult> storeMstPop([FromBody] StoreReq001 storeReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeMstPop(storeReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        //new) 2) 입고등록 등록/수정 버튼 팝업 -품목리스트
        [HttpPost("/api/inAndOut/storeOutStoreProduct/listPop")]
        public async Task<IActionResult> storeOutStoreProductListPop([FromBody] StoreReq001 storeReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeOutStoreProductListPop(storeReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        //new) 2)등록/수정 버튼 클릭화면 -품목 상세내역- 추가버튼 - 입고등록 팝업
        [HttpPost("/api/inAndOut/storeOutStoreProduct/outStoreProductListPop")]
        public async Task<IActionResult> outStoreProductListPop([FromBody] StoreReq005 storeReq005, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outStoreProductListPop(storeReq005);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        //new) 2) 입고등록 등록/수정 버튼 팝업 -불량유형 리스트
        [HttpPost("/api/inAndOut/storeOutStoreProductDefective/listPop")]
        public async Task<IActionResult> storeOutStoreProductDefectiveListPop([FromBody] StoreReq003 storeReq003, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeOutStoreProductDefectiveListPop(storeReq003);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        //new) 2) 입고등록 등록/수정 버튼 팝업 -검사항목 리스트
        [HttpPost("/api/inAndOut/storeOutStoreProductInspectionType/listPop")]
        public async Task<IActionResult> storeOutStoreProductInspectionTypeListPop([FromBody] StoreReq004 storeReq004, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeOutStoreProductInspectionTypeListPop(storeReq004);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        //new) 불량유형 추가버튼 팝업
        [HttpPost("/api/inAndOut/defective/list")]
        public async Task<IActionResult> defectiveList([FromBody] StoreReq002 storeReq002, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.defectiveList(storeReq002);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        //new) 검사항목 추가버튼 팝업
        [HttpPost("/api/inAndOut/inspection/list")]
        public async Task<IActionResult> inspectionList([FromBody] StoreReq002 storeReq002, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.inspectionList(storeReq002);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        // 불량유형 추가버튼 팝업 저장버튼 event
        [HttpPost("/api/inAndOut/createStoreOutStoreProductDefective")]
        public async Task<IActionResult> CreateStoreOutStoreProductDefective([FromBody] StoreReq003 storeReq003, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateStoreOutStoreProductDefective(storeReq003);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        // 검사항목 추가버튼 팝업 저장버튼 event
        [HttpPost("/api/inAndOut/createStoreOutStoreProductInspectionType")]
        public async Task<IActionResult> CreateStoreOutStoreProductInspection([FromBody] StoreReq004 storeReq004, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateStoreOutStoreProductInspection(storeReq004);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
       
        [HttpPost("/api/inAndOut/deleteStoreOutStoreProductDefective")]
        public async Task<IActionResult> DeleteStoreOutStoreProductDefective([FromBody] StoreReq003 storeReq003, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteStoreOutStoreProductDefective(storeReq003);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteStoreOutStoreProductInspectionType")]
        public async Task<IActionResult> DeleteStoreOutStoreProductInspection([FromBody] StoreReq004 storeReq004, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteStoreOutStoreProductInspection(storeReq004);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/getOutStoreMsts")]
        public async Task<IActionResult> getOutStoreMsts([FromBody] OutStoreReq001 OutStoreReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.getOutStoreMsts(OutStoreReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/getStoreOutStore")]
        public async Task<IActionResult> getStoreOutStore([FromBody] OutStoreReq001 OutStoreReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.getStoreOutStore(OutStoreReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/getStoreOutStoreItems")]
        public async Task<IActionResult> getStoreOutStoreItems([FromBody] OutStoreReq001 OutStoreReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.getStoreOutStoreItems(OutStoreReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/storeMstList2")]
        public async Task<IActionResult> storeMstList2([FromBody] OutStoreReq001 OutStoreReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeMstList2(OutStoreReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/storeMstList3")]
        public async Task<IActionResult> storeMstList3([FromBody] StoreReq001 storeReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeMstList3(storeReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/storeOutStoreProductList2")]
        public async Task<IActionResult> storeOutStoreProductList2([FromBody] StoreReq001 storeReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeOutStoreProductList2(storeReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/storeOutStoreProductList3")]
        public async Task<IActionResult> storeOutStoreProductList3([FromBody] StoreReq001 storeReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.storeOutStoreProductList3(storeReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/getProgressList")]
        public async Task<IActionResult> getInvenConsumeList([FromBody] InvenMngModelReq0001 InvenMngModelReq0001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo2.getProgressList(InvenMngModelReq0001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }

            else
                return BadRequest(result);
        }
        
        [HttpPost("api/inAndOut/outStoreFinish")]
        public async Task<IActionResult> eventOutStoreFinish([FromBody] StoreRequestCrud req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.eventOutStoreFinish(req);

            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
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
