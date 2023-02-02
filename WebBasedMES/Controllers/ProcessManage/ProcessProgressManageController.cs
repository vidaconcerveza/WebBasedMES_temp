using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
    public class ProcessProgressManageController : ControllerBase
    {
        private readonly ILogger<ProcessProgressManageController> _logger;
        private readonly IProcessRepository _repo;

        public ProcessProgressManageController(ILogger<ProcessProgressManageController> logger, IProcessRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        #region MAIN
        [HttpPost("/api/process/work-order-produce-plan")]
        public async Task<IActionResult> GetWorkOrderProducePlans([FromBody] WorkOrderProducePlanRequest001 param)
        {
            var result = await _repo.GetWorkOrderProducePlans(param);
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

        [HttpPost("/api/process/process-progress")]
        public async Task<IActionResult> GetProcessProgresses([FromBody] ProcessProgressRequest001 param)
        {
            var result = await _repo.GetProcessProgresses(param);
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

        [HttpPost("/api/process/process-not-work")]
        public async Task<IActionResult> GetProcessNotWorks([FromBody] ProcessNotWorkRequest001 param)
        {
            var result = await _repo.GetProcessNotWorks(param);
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

        [HttpPost("/api/process/process-defective")]
        public async Task<IActionResult> GetProcessDefectives([FromBody] ProcessDefectiveRequest001 param)
        {
            var result = await _repo.GetProcessDefectives(param);
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

        [HttpPost("/api/process/process-product-item")]
        public async Task<IActionResult> GetProductItems([FromBody] ProductItemsRequest001 param)
        {
            var result = await _repo.GetProductItems(param);
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

        #endregion

        #region 공정진행현황 조회
        [HttpPost("/api/process/work-order-produce-plan-detail")]
        public async Task<IActionResult> GetWorkOrderProducePlan([FromBody] WorkOrderProducePlanRequest001 param)
        {
            var result = await _repo.GetWorkOrderProducePlan(param);
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
        #endregion 공정진행현황 조회

        #region 작업 시작/중지/완료 Event
        [HttpPost("/api/process/event-work-start")]
        public async Task<IActionResult> EventWorkStart([FromBody] ProcessNotWorkRequest001 param)
        {
            var result = await _repo.EventWorkStart(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("/api/process/event-work-stop")]
        public async Task<IActionResult> EventWorkStop([FromBody] ProcessNotWorkRequest002 param)
        {
            var result = await _repo.EventWorkStop(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("/api/process/work-finish")]
        public async Task<IActionResult> GetWorkStop([FromBody] WorkStopRequest001 param)
        {
            var result = await _repo.GetWorkStop(param);
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

        [HttpPost("/api/process/event-work-finish")]
        public async Task<IActionResult> SaveWorkStop([FromBody] WorkStopRequest001 param)
        {
            var result = await _repo.SaveWorkStop(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("/api/process/work-edit")]
        public async Task<IActionResult> GetWorkEdit([FromBody] WorkStopRequest001 param)
        {
            var result = await _repo.GetWorkEdit(param);
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

        [HttpPost("/api/process/event-work-edit")]
        public async Task<IActionResult> SaveWorkEdit([FromBody] WorkStopRequest001 param)
        {
            var result = await _repo.SaveWorkEdit(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }


        #endregion 작업 시작/중지/완료/수정 Event

        #region 비가동 이벤트s
        [HttpPost("/api/process/process-not-work/not-works")]
        public async Task<IActionResult> GetNotWorks([FromBody] ProcessNotWorkRequest002 param)
        {
            var result = await _repo.GetNotWorks(param);
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

        [HttpPost("/api/process/process-not-work/not-work")]
        public async Task<IActionResult> GetNotWork([FromBody] ProcessNotWorkRequest003 param)
        {
            var result = await _repo.GetNotWork(param);
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

        [HttpPost("/api/process/process-not-work/not-work-update")]
        public async Task<IActionResult> SaveNotWork([FromBody] ProcessNotWorkRequest003 param)
        {
            var result = await _repo.SaveNotWork(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/api/process/process-not-work/not-work-delete")]
        public async Task<IActionResult> DeleteNotWork([FromBody] ProcessNotWorkRequest002 param)
        {
            var result = await _repo.DeleteNotWorks(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpGet("/api/process/process-not-work-type")]
        public async Task<IActionResult> GetProcessNotWorkType()
        {
            var result = await _repo.GetProcessNotWorkType();
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
        #endregion 비가동 이벤트


        #region 공정 불량 
        [HttpPost("/api/process/process-defective/defective")]
        public async Task<IActionResult> GetDefective([FromBody] ProcessDefectiveRequest002 param)
        {
            var result = await _repo.GetDefective(param);
            
            if(result.Data == null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new Response<NullOrEmpty>
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new NullOrEmpty()
                    }) ;
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

        [HttpPost("/api/process/process-defective/defectives")]
        public async Task<IActionResult> GetDefectives([FromBody] ProcessDefectiveRequest002 param)
        {
            var result = await _repo.GetDefectives(param);
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


        
        [HttpPost("/api/process/process-defective/defective-create")]
        public async Task<IActionResult> CreateDefective([FromBody] ProcessDefectiveRequest002 param)
        {
            var result = await _repo.CreateDefective(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("/api/process/process-defective/defective-update")]
        public async Task<IActionResult> UpdateDefective([FromBody] ProcessDefectiveRequest003 param)
        {
            var result = await _repo.UpdateDefective(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/api/process/process-defective/defective-delete")]
        public async Task<IActionResult> DeleteDefectives([FromBody] ProcessDefectiveRequest003 param)
        {
            var result = await _repo.DeleteDefectives(param);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        #endregion 공정진행 불량 

        #region 공정진행 - 투입품목 관리
        [HttpPost("/api/process/process-product-item/product-items")]
        public async Task<IActionResult> GetProductItems([FromBody] ProductItemsRequest002 param)
        {
            var result = await _repo.GetProductItems(param);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            
        }

        [HttpPost("/api/process/process-product-item/product-item")]
        public async Task<IActionResult> GetProductItem([FromBody] ProductItemsRequest002 param)
        {
            var result = await _repo.GetProductItem(param);
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


        [HttpPost("/api/process/process-product-item/product-item-update")]
        public async Task<IActionResult> SaveProductItems()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var json2 = json.Replace("$id", "id");

            ProductItemsResponse003 _req = JsonSerializer.Deserialize<ProductItemsResponse003>(json2);

            var result = await _repo.UpdateProductItems(_req);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }







        #endregion 공정진행 - 투입품목 관리

        [HttpPost("/api/process/workorder-process-list")]
        public async Task<IActionResult> GetProcessProgressSelect([FromBody]ProcessProgressSelectRequest param)
        {
            var result = await _repo.GetProcessProgressSelect(param);
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



        // 비가동내역부터는 ProcessStatusController 

  



    }
}
