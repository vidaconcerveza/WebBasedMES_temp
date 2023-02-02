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
    public class OutOrderManageController : ControllerBase
    {
        private readonly ILogger<OutOrderManageController> _logger;
        private readonly IOutOrderMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public OutOrderManageController(ILogger<OutOrderManageController> logger, IOutOrderMngRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }

        //new) 1) 출고등록 메인 화면 -목록
        [HttpPost("/api/inAndOut/outOrder/mst/list")]
        public async Task<IActionResult> outOrderMstList([FromBody] OutOrderReq001 outOrderReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outOrderMstList(outOrderReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //new) 1) 출고등록 메인화면 상세내역 리스트
        [HttpPost("/api/inAndOut/outOrderProduct/list")]
        public async Task<IActionResult> outOrderProductList([FromBody] OutOrderReq001 outOrderReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outOrderProductList(outOrderReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        //new) 2) 출고등록 등록/수정 버튼 팝업 -마스터
        [HttpPost("/api/inAndOut/outOrder/mstPop")]
        public async Task<IActionResult> outOrderMstPop([FromBody] OutOrderReq001 outOrderReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outOrderMstPop(outOrderReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //new) 2) 출고등록 등록/수정 버튼 팝업 -품목리스트
        [HttpPost("/api/inAndOut/OutOrderProduct/listPop")]
        public async Task<IActionResult> outOrderProductListPop([FromBody] OutOrderReq001 outOrderReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outOrderProductListPop(outOrderReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //new) 3) 2) 수주목록에 수주정보 추가버튼 클릭팝업
        [HttpPost("/api/inAndOut/outOrderProduct/orderProductList")]
        public async Task<IActionResult> orderProductListPop([FromBody] OutOrderReq003 outOrderReq003, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.orderProductListPop(outOrderReq003);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/outOrderProduct/orderProductLotList")]
        public async Task<IActionResult> orderProductPopLotList([FromBody] ProductDetailRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.orderProductLotList(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/outOrderProduct/orderProductProcessList")]
        public async Task<IActionResult> orderProductListPopProcessList([FromBody] OutOrderReq002 outOrderReq002, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.orderProductListPopProcessList(outOrderReq002);
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
        [HttpPost("/api/inAndOut/outOrderProductDefective/listPop")]
        public async Task<IActionResult> outOrderProductDefectiveListPop([FromBody] OutOrderReq002 outOrderReq002, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outOrderProductDefectiveListPop(outOrderReq002);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/createOutOrder")]
        public async Task<IActionResult> CreateOutOrder([FromBody] OutOrderRequestCrud outOrderRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateOutOrder(outOrderRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/updateOutOrder")]
        public async Task<IActionResult> UpdateOutOrder([FromBody] OutOrderRequestCrud outOrderRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOutOrder(outOrderRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteOutOrder")]
        public async Task<IActionResult> DeleteOutOrder([FromBody] OutOrderRequestCrud outOrderRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteOutOrder(outOrderRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        
        [HttpPost("/api/inAndOut/createOutOrderProduct")]
        public async Task<IActionResult> CreateOutOrderProduct([FromBody] OutOrderProductRequestCrud outOrderProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {

            var result = await _repo.CreateOutOrderProductLot(outOrderProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        /*
        [HttpPost("/api/inAndOut/createOutOrderProduct")]
        public async Task<IActionResult> CreateOutOrderProduct([FromBody] OutOrderProductRequestCrud outOrderProductRequest)
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var json2 = json.Replace("$id", "id");

            OutOrderProductRequestCrud _req = JsonSerializer.Deserialize<OutOrderProductRequestCrud>(json2);


            var result = await _repo.CreateOutOrderProductLot(_req);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
         */
        [HttpPost("/api/inAndOut/createOutOrderProductforProduct")]
        public async Task<IActionResult> CreateOutOrderProductforProduct([FromBody] OutOrderProductRequestCrud outOrderProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateOutOrderProductforProduct(outOrderProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteOutOrderProduct")]
        public async Task<IActionResult> DeleteOutOrderProduct([FromBody] OutOrderProductRequestCrud outOrderProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteOutOrderProduct(outOrderProductRequest);
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
        [HttpPost("/api/inAndOut/updateOutOrderProduct")]
        public async Task<IActionResult> UpdateOutOrderProduct()
        {

            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var json2 = json.Replace("$id", "id");

            OutOrderProductRequestCrud _req = JsonSerializer.Deserialize<OutOrderProductRequestCrud>(json2);

            var result = await _repo.UpdateOutOrderProduct(_req);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
        */

        [HttpPost("/api/inAndOut/updateOutOrderProduct")]
        public async Task<IActionResult> UpdateOutOrderProduct([FromBody] OutOrderProductRequestCrud outOrderProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOutOrderProduct(outOrderProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        
        [HttpPost("/api/inAndOut/updateOutOrderProductforProduct")]
        public async Task<IActionResult> UpdateOutOrderProductforProduct([FromBody] OutOrderProductRequestCrud outOrderProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOutOrderProductforProduct(outOrderProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        // 불량유형 추가버튼 팝업 저장버튼 event
        [HttpPost("/api/inAndOut/createOutOrderProductDefective")]
        public async Task<IActionResult> CreateOutOrderProductDefective([FromBody] OutOrderReq002 outOrderReq002, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateOutOrderProductDefective(outOrderReq002);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //new) 2) 출고등록 등록/수정 버튼 팝업 -불량유형리스트 삭제
        [HttpPost("/api/inAndOut/deleteOutOrderProductDefective")]
        public async Task<IActionResult> DeleteOutOrderProductDefective([FromBody] OutOrderReq002 outOrderReq002, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteOutOrderProductDefective(outOrderReq002);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //new) 2) 출고등록 등록/수정 버튼 팝업 -불량유형리스트 수정
        [HttpPost("/api/inAndOut/updateOutOrderProductDefective")]
        public async Task<IActionResult> UpdateOutOrderProductDefective([FromBody] OutOrderReq002 outOrderReq002, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOutOrderProductDefective(outOrderReq002);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/createOutOrderProductStockTable")]
        public async Task<IActionResult> CreateOutOrderProductsStock2([FromBody] OutOrderReq006 OutOrderReq006, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateOutOrderProductsStock2(OutOrderReq006);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteOutOrderProductStockTable")]
        public async Task<IActionResult> DeleteOutOrderProductsStock([FromBody] OutOrderReq006 OutOrderReq006, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteOutOrderProductsStock(OutOrderReq006);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/updateOutOrderProductStockTable")]
        public async Task<IActionResult> UpdateOutOrderProductsStock([FromBody] OutOrderReq006 OutOrderReq006, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOutOrderProductsStock(OutOrderReq006);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        // 품목재고 추가버튼 팝업 저장버튼 event
        [HttpPost("/api/inAndOut/createOutOrderProductStock")]
        public async Task<IActionResult> CreateOutOrderProductStock([FromBody] OutOrderReq005 outOrderReq005, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateOutOrderProductStock(outOrderReq005);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //출고관리 
        //1) 출고관리 메인화면 수주목록 리스트
        [HttpPost("/api/inAndOut/outOrder/order/mst/list")]
        public async Task<IActionResult> orderMstList([FromBody] OutOrderReq004 outOrderReq004, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.orderMstList(outOrderReq004);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //출고관리 
       // 1) 수주목록 클릭, 수주마스터정보
        [HttpPost("/api/inAndOut/outOrder/order/mstPop")]
        public async Task<IActionResult> orderMstPop([FromBody] OutOrderReq004 outOrderReq004, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.orderMstPop(outOrderReq004);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //출고관리 
        //new) 1) 수주품목 클릭, 출고마스터리스트
        [HttpPost("/api/inAndOut/outOrder/mst/getList")]
        public async Task<IActionResult> outOrderMstList2([FromBody] OutOrderReq001 outOrderReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outOrderMstList2(outOrderReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //출고현황
        //1) 출고현황 메인화면(품목별)
        [HttpPost("/api/inAndOut/outOrderProduct/getList2")]
        public async Task<IActionResult> outOrderProductList2([FromBody] OutOrderReq001 outOrderReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outOrderProductList2(outOrderReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        

        [HttpPost("/api/inAndOut/outOrderProduct/outOrderProductListLOT")]
        public async Task<IActionResult> outOrderProductListLOT([FromBody] OutOrderReq001 outOrderReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.outOrderProductListLOT(outOrderReq001);
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
        //// 검사항목 추가버튼 팝업 저장버튼 event
        //[HttpPost("/api/inAndOut/createStoreOutStoreProductInspectionType")]
        //public async Task<IActionResult> CreateStoreOutStoreProductInspection([FromBody] StoreReq004 storeReq004)
        //{
        //    var result = await _repo.CreateStoreOutStoreProductInspection(storeReq004);
        //    if (result.IsSuccess)
        //        return Ok(result);
        //    else
        //        return BadRequest(result);
        //}







        //[HttpPost("/api/inAndOut/store/editSave")]
        //public async Task<IActionResult> StoreOutStoreProductEditSave([FromBody] StoreOutStoreProductRequestCrud storeOutStoreProductRequest)
        //{
        //    var result = await _repo.StoreOutStoreProductEditSave(storeOutStoreProductRequest);
        //    if (result.IsSuccess)
        //        return Ok(result);
        //    else
        //        return BadRequest(result);
        //}
    }
}
