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
using WebBasedMES.Services.Repositories.ProducePlanManage;
using WebBasedMES.ViewModels.ProducePlan;

namespace WebBasedMES.Controllers.ProducePlanManage
{
    public class WorkOrderManageController : ControllerBase
    {
        private readonly ILogger<WorkOrderManageController> _logger;
        private readonly IWorkOrderRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public WorkOrderManageController(ILogger<WorkOrderManageController> logger, IWorkOrderRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }

        [HttpPost("/api/produce/work-order")]
        public async Task<IActionResult> GetWorkOrders([FromBody] WorkOrderRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetWorkOrders(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/work-order-main")]
        public async Task<IActionResult> GetWorkOrdersMain([FromBody] WorkOrderRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetWorkOrdersMain(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/work-order-detail")]
        public async Task<IActionResult> GetWorkOrderDetail([FromBody] WorkOrderRequest _req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetWorkOrderDetail(_req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/produce/work-order/create")]
        public async Task<IActionResult> CreateWorkOrder([FromHeader(Name = "Uuid")] string Uuid)
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var json2 = json.Replace("$id", "id");

            WorkOrderRequest002 _req = JsonSerializer.Deserialize<WorkOrderRequest002>(json2);

            var result = await _repo.CreateWorkOrder(_req);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/work-order/update")]
        public async Task<IActionResult> UpdateWorkOrder([FromHeader(Name = "Uuid")] string Uuid)
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var json2 = json.Replace("$id", "id");

            WorkOrderRequest002 _req = JsonSerializer.Deserialize<WorkOrderRequest002>(json2);

            var result = await _repo.UpdateWorkOrder(_req);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/work-order/delete")]
        public async Task<IActionResult> DeleteWorkOrder([FromBody] WorkOrderRequest002 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteWorkOrder(param);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/work-order-produce-plan")]
        public async Task<IActionResult> GetWorkOrderProducePlans([FromBody] WorkOrderRequest005 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetWorkOrderProducePlans(param);
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
