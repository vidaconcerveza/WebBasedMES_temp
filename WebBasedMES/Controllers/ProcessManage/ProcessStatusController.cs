using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WebBasedMES.Services.Repositories.ProcessManage;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Process;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebBasedMES.Controllers.ProcessManage
{
    public class ProcessStatusController : ControllerBase
    {
        private readonly ILogger<ProcessStatusController> _logger;
        private readonly IProcessStatusRepository _repo;

        public ProcessStatusController(ILogger<ProcessStatusController> logger, IProcessStatusRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        #region 비가동내역 조회

        [HttpPost("/api/process-status/process-not-works")]
        public async Task<IActionResult> GetProcessNotWorksQuery([FromBody] ProcessNotWorkQueryRequest param)
        {
            var result = await _repo.GetProcessNotWorksQuery(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }
        #endregion 비가동내역 조회

        #region 생산공정별 조회
        [HttpPost("/api/process-status/get-production-manage-by-processes")]
        public async Task<IActionResult> GetProductionManageByProcesses([FromBody] ProductionManageByProcessRequest param)
        {
            var result = await _repo.GetProductionManageByProcesses(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }

        [HttpPost("/api/process-status/get-production-manage-by-process")]
        public async Task<IActionResult> GetProductionManageByProcess([FromBody] ProductionManageByProcessRequest param)
        {
            var result = await _repo.GetProductionManageByProcess(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }
        #endregion 생산공정별 조회

        #region LOT 이력 추적
        [HttpPost("/api/process-status/lot-track")]
        public async Task<IActionResult> GetLotInfo([FromBody] LotInfoRequest param)
        {
            var result = await _repo.GetLotInfo(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }

        [HttpPost("/api/process-status/lot-track/input-product")]
        public async Task<IActionResult> GetLotInputProducts([FromBody] LotInfoRequest param)
        {
            var result = await _repo.GetLotInputItems(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }

        [HttpPost("/api/process-status/lot-track/defectives")]
        public async Task<IActionResult> GetLotDefectives([FromBody] LotInfoRequest param)
        {
            var result = await _repo.GetLotProcessDefective(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }

        [HttpPost("/api/process-status/lot-track/process-inspection")]
        public async Task<IActionResult> GetLotProcessInspection([FromBody] LotInfoRequest param)
        {
            var result = await _repo.GetLotProcessInspection(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }

        [HttpPost("/api/process-status/lot-track/out-product")]
        public async Task<IActionResult> GetLotOutProduct([FromBody] LotInfoRequest param)
        {
            var result = await _repo.GetLotOutProduct(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }




        #endregion LOT 이력 추적

        #region 일일생산현황
        [HttpPost("/api/process-status/get-daily-production")]
        public async Task<IActionResult> GetDailyProductions([FromBody] DailyProductionRequest param)
        {
            var result = await _repo.GetDailyProductions(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }
        #endregion 일일생산현황

        #region 일일불량현황

        [HttpPost("/api/process-status/get-daily-defective-summary")]
        public async Task<IActionResult> GetDailyDefectiveSummary([FromBody] DailyDefectiveRequest param)
        {
            var result = await _repo.GetDailyDefectiveSummary(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }

        [HttpPost("/api/process-status/get-daily-defective")]
        public async Task<IActionResult> GetDailyDefective([FromBody] DailyDefectiveRequest param)
        {
            var result = await _repo.GetDailyDefective(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }
        #endregion 일일불량현황

        #region 일일출하현황

        [HttpPost("/api/process-status/get-daily-out-order")]
        public async Task<IActionResult> GetDailyOutOrder([FromBody] DailyOutOrderRequest param)
        {
            var result = await _repo.GetDailyOutOrder(param);
            if (result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    });
                }
                else
                {
                    return BadRequest(new Response<NullOrEmpty>
                    {
                        IsSuccess = false,
                        ErrorMessage = result.ErrorMessage,
                        Data = new NullOrEmpty()
                    });
                }
            }
            else
            {
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
        }

        #endregion 일일출하현황

        #region 설비별 생산현황
        [HttpPost("/api/process-status/get-production-manage-by-facility")]
        public async Task<IActionResult> GetProductionManageByFacility([FromBody] ProductionManageByFacilityRequest param)
        {
            var result = await _repo.GetProductionManageByFacility(param);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }

        #endregion 설비별 생산현황
    }
}
