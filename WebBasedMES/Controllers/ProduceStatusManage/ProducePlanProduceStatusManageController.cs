using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Services.Repositories.ProduceStatus;
using WebBasedMES.ViewModels.ProduceStatus;
using WebBasedMES.ViewModels.Quality;
using WebBasedMES.Data.Models;

namespace WebBasedMES.Controllers.ProduceStatusManage
{
    public class ProducePlanProduceStatusManageController : ControllerBase
    {
        private readonly ILogger<ProducePlanProduceStatusManageController> _logger;
        private readonly IProducePlanProduceStatusMngRepository _repo;
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProducePlanProduceStatusManageController(ILogger<ProducePlanProduceStatusManageController> logger, IProducePlanProduceStatusMngRepository repo, UserManager<ApplicationUser> userManager, ApiDbContext db)
        {
            _logger = logger;
            _repo = repo;
            _db = db;
            _userManager = userManager;
        }
        // 1) 계획대비 생산현황 메인화면
        [HttpPost("/api/produceStatus/producePlanProduceStatus/mst/producePlanProduceStatusList")]
        public async Task<IActionResult> producePlanProduceStatusList([FromBody] ProducePlanProduceStatusReq001 producePlanProduceStatusReq001, [FromHeader(Name = "Uuid")] string Uuid)
        {
            
            var result = await _repo.producePlanProduceStatusList(producePlanProduceStatusReq001);
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
