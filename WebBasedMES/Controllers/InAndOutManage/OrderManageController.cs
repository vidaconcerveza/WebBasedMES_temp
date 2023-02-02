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
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Controllers.InAndOutMng
{
    public class OrderManageController : ControllerBase
    {
        private readonly ILogger<OrderManageController> _logger;
        private readonly IOrderMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderManageController(ILogger<OrderManageController> logger, IOrderMngRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }


        [HttpPost("/api/inAndOut/getOrdersPopup")]
        public async Task<IActionResult> GetOrdersPopupBySearch([FromBody] OrderPopupRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetOrdersPopupBySearch(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestCrud orderRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateOrder(orderRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/updateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderRequestCrud orderRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOrder(orderRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteOrder")]
        public async Task<IActionResult> DeleteOrder([FromBody] OrderRequestCrud orderRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteOrder(orderRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/inAndOut/createOrderProduct")]
        public async Task<IActionResult> CreateOrderProduct([FromBody] OrderProductRequestCrud OrderProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateOrderProduct(OrderProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/deleteOrderProduct")]
        public async Task<IActionResult> DeleteOrderProduct([FromBody] OrderProductRequestCrud OrderProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteOrderProduct(OrderProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        [HttpPost("/api/inAndOut/updateOrderProduct")]
        public async Task<IActionResult> UpdateOrderProduct([FromBody] OrderProductRequestCrud OrderProductRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateOrderProduct(OrderProductRequest);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        //new) 1) 수주등록 메인 화면 -목록
        [HttpPost("/api/inAndOut/order/mst/list")]
        public async Task<IActionResult> orderMstList([FromBody] OrderReq001 OrderRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.orderMstList(OrderRequest);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //new) 1) 수주등록 메인 화면 -상세내역
        [HttpPost("/api/inAndOut/orderProduct/list")]
        public async Task<IActionResult> view03([FromBody] OrderReq001 OrderReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.orderProductList(OrderReq001);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        //new) 2) 수주등록 등록/수정 버튼 팝업 -마스터
        [HttpPost("/api/inAndOut/order/mstPop")]
        public async Task<IActionResult> orderMstPop([FromBody] OrderReq001 OrderRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.orderMstPop(OrderRequest);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //new) 2) 수주등록 등록/수정 버튼 팝업 -품목리스트
        [HttpPost("/api/inAndOut/orderProduct/listPop")]
        public async Task<IActionResult> orderProductListPop([FromBody] OrderReq001 OrderRequest, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.orderProductListPop(OrderRequest);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //new) 2) 수주등록 등록/수정 버튼 팝업 -품목리스트
        [HttpPost("/api/inAndOut/productList")]
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
        //) 3) 2)에서 등록자 검색 버튼 클릭 팝업 리스트
        [HttpPost("/api/inAndOut/user/list")]
        public async Task<IActionResult> userList([FromBody] OrderReq003 OrderReq003, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.userList(OrderReq003);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }
        //) 3) 2)에서 등록자 검색 버튼 클릭 팝업 리스트
        [HttpPost("/api/inAndOut/partner/list")]
        public async Task<IActionResult> partnerList([FromBody] OrderReq004 OrderReq004, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.partnerList(OrderReq004);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        //) 3) 2)에서 등록자 검색 버튼 클릭 팝업 리스트
        [HttpPost("/api/inAndOut/orderProduct/editSave")]
        public async Task<IActionResult> OrderProductEditSave([FromBody] OrderProductRequestCrud OrderProductRequestCrud, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.OrderProductEditSave(OrderProductRequestCrud);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/inAndOut/outOrderFinishEvent")]
        public async Task<IActionResult> OutOrderFinishEvent([FromBody]OrderProductRequestCrud req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.OutOrderFinishEvent(req);

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
