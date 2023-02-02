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
    public class ProducePlanManageController : ControllerBase
    {
        private readonly ILogger<ProducePlanManageController> _logger;
        private readonly IProducePlanRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProducePlanManageController(ILogger<ProducePlanManageController> logger, IProducePlanRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }


        [HttpPost("/api/produce/production-plan/popup")]
        public async Task<IActionResult> GetProducePlansPopup([FromBody] ProducePlanPopupRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProducePlansPopup(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-plan")]
        public async Task<IActionResult> GetProducePlans([FromBody] ProducePlanRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProducePlans(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-plan-product")]
        public async Task<IActionResult> GetProducePlansProducts([FromBody] ProducePlanRequest002 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProducePlansProducts(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-plan/create")]
        public async Task<IActionResult> CreateProducePlan([FromBody] ProducePlanRequest003 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            /*
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var json2 = json.Replace("$id", "id");

            ProducePlanRequest003 _req = JsonSerializer.Deserialize<ProducePlanRequest003>(json2);
            */

            var result = await _repo.CreateProducePlan(param);
            if (result.IsSuccess)
            {
                string dataSize =  "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-plan/update")]
        public async Task<IActionResult> UpdateProducePlan([FromBody] ProducePlanRequest003 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            /*
                var json = await new StreamReader(Request.Body).ReadToEndAsync();
                var json2 = json.Replace("$id", "id");

                ProducePlanRequest003 _req = JsonSerializer.Deserialize<ProducePlanRequest003>(json2);
            */
                var result = await _repo.UpdateProducePlan(param);
            if (result.IsSuccess)
            {
                string dataSize = "0";

                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                    return BadRequest(result);

        }

        [HttpPost("/api/produce/production-plan/delete")]
        public async Task<IActionResult> DeleteProducePlan([FromBody] ProducePlanRequest003 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteProducePlan(param);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-plan-process")]
        public async Task<IActionResult> GetProducePlanProcesses([FromBody] ProducePlanRequest004 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProducePlanProcesses(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-plan-process/create")]
        public async Task<IActionResult> CreateProducePlanProcesses([FromBody] ProducePlanRequest004 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.CreateProducePlanProcesses(param);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "등록", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-plan-process/update")]
        public async Task<IActionResult> UpdateProducePlanProcesses([FromBody] ProducePlanRequest004 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.UpdateProducePlanProcesses(param);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "수정", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-plan-process/delete")]
        public async Task<IActionResult> DeleteProducePlanProcesses([FromBody] ProducePlanRequest004 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.DeleteProducePlanProcesses(param);
            if (result.IsSuccess)
            {
                string dataSize = "0";
                await SmartFactoryLog(Uuid, "삭제", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-plan-process/product-item")]
        public async Task<IActionResult> GetProductItems([FromBody] ProducePlanRequest005 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductItems(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/produce/production-plan-process/produce-product-detail")]
        public async Task<IActionResult> GetProducePlanProductDetail([FromBody] ProducePlanProduceDetailRequest req, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProducePlanProductDetail(req);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/produce/required-amout")]
        public async Task<IActionResult> GetRequiredAmounts([FromBody] GetRequiredAmountsRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetRequiredAmounts(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/report-by-process")]
        public async Task<IActionResult> GetReportByProcesses([FromBody] GetReportByProcessesRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetReportByProcesses(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/report-by-process/work-order")]
        public async Task<IActionResult> GetReportByProcessWorkOrders([FromBody] GetReportByProcessesRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetReportByProcessWorkOrders(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/report-by-process/work-order-produce-plan")]
        public async Task<IActionResult> GetReportByProcessWorkOrderProducePlans([FromBody] GetReportByProcessesRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetReportByProcessWorkOrderProducePlans(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/report-by-process/work-order-produce-plan-process")]
        public async Task<IActionResult> GetReportByProcessWorkOrderProducePlansProcess([FromBody] GetReportByProcessesRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetReportByProcessWorkOrderProducePlansProcess(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost("/api/produce/production-manage-by-product")]
        public async Task<IActionResult> GetProductionManageByProducts([FromBody] GetProductionManageByProductsRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductionManageByProducts(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-manage-by-period")]
        public async Task<IActionResult> GetProductionManageByPeriods([FromBody] GetProductionManageByPeriodsRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductionManageByPeriods(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/defective-manage-by-period")]
        public async Task<IActionResult> GetDefectiveManageByPeriods([FromBody] GetDefectiveManageByPeriodsRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetDefectiveManageByPeriods(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/production-manage-by-month")]
        public async Task<IActionResult> GetProductionManageByMonth([FromBody] GetProductionManageByMonthRequest001 param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductionManageByMonth(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/kpi/production-by-hour")]
        public async Task<IActionResult> GetProductionByHour([FromBody] KpiRequest param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetProductionByHour(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/kpi/defective-by-hour")]
        public async Task<IActionResult> GetDefectiveByHour([FromBody] KpiRequest param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetDefectiveByHour(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/kpi/kpi-by-month")]
        public async Task<IActionResult> GetKpiByMonth([FromBody] KpiRequest param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetKpiByMonth(param);
            if (result.IsSuccess)
            {
                string dataSize = result.Data != null ? result.Data.ToString().Length.ToString() : "0";
                await SmartFactoryLog(Uuid, "조회", dataSize);
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("/api/produce/kpi/work-by-month")]
        public async Task<IActionResult> GetWorkByMonth([FromBody] KpiRequest param, [FromHeader(Name = "Uuid")] string Uuid)
        {
            var result = await _repo.GetWorkByMonth(param);
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
