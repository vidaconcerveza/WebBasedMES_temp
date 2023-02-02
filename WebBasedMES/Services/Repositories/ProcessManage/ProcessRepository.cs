using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Data.Models.Lots;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Process;

namespace WebBasedMES.Services.Repositories.ProcessManage
{
    public class ProcessRepository : IProcessRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProcessRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        #region 생산공정 작업목록 불러오기 GetWorkOrderProducePlans
        public async Task<Response<IEnumerable<WorkOrderProducePlanResponse001>>> GetWorkOrderProducePlans(WorkOrderProducePlanRequest001 param)
        {
            try
            {
                var res = await _db.WorkerOrders
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkOrderDate >= Convert.ToDateTime(param.workOrderStartDate))
                    .Where(x => x.WorkOrderDate <= Convert.ToDateTime(param.workOrderEndDate))
                    .Where(x => x.WorkOrderNo.Contains(param.workOrderNo))
                    .Where(x => param.productId == 0 ? true : x.Product.Id == param.productId)
                    .OrderByDescending(x => x.WorkOrderNo)
                    .ThenBy(x => x.WorkOrderSequence)
                    .Select(x => new WorkOrderProducePlanResponse001
                    {
                        workerOrderId = x.WorkerOrderId,
                        workOrderNo = x.WorkOrderNo,
                        workOrderDate = x.WorkOrderDate.ToString("yyyy-MM-dd"),
                        workOrderSequence = x.WorkOrderSequence,

                        productCode = x.Product.Code,
                        productName = x.Product.Name,
                        productClassification = x.Product.CommonCode.Name,
                        productUnit = x.Product.Unit,
                        productStandard = x.Product.Standard,
                        productWorkQuantity = x.ProductWorkQuantity,
                        /*
                        productionQuantity = x.WorkerOrderProducePlans
                            .Where(y => y.IsDeleted == 0)
                            .OrderByDescending(y => y.ProcessProgress.ProcessProgressId)
                            .Select(y => y.ProcessProgress.ProductionQuantity).FirstOrDefault(),

                        productDefectiveQuantity = _db.ProcessDefectives
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrderId)
                            .OrderByDescending(y=>y.ProcessProgress.ProcessProgressId)
                            .Select(y=>y.DefectiveCount)
                            .FirstOrDefault(),
                        */
                        productionQuantity = x.ProducePlansProduct != null ? x.WorkerOrderProducePlans.Where(y => y.IsDeleted == 0).OrderByDescending(y => y.ProducePlansProcess.ProductProcess.ProcessOrder).Select(y => y.ProcessProgress.ProductionQuantity).FirstOrDefault() :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrder == x).OrderByDescending(y => y.ProductProcess.ProcessOrder).Select(y => y.WorkerOrderProducePlan.ProcessProgress.ProductionQuantity).FirstOrDefault(),

                        productDefectiveQuantity = _db.ProcessDefectives
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrderId)
                            .Select(y => y.DefectiveCount)
                            .Sum(),


                        productGoodQuantity = x.WorkerOrderProducePlans
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => y.ProcessProgress.ProductionQuantity).Sum() - _db.ProcessDefectives
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrderId)
                            .OrderByDescending(y=>y.ProcessProgress.ProcessProgressId)
                            .Select(y => y.DefectiveCount)
                            .FirstOrDefault(),


                        totalElapsedTime = x.WorkerOrderProducePlans
                            .Where(y=>y.IsDeleted == 0)
                            .Select(y=> EF.Functions.DateDiffMinute(y.ProcessProgress.WorkStartDateTime, y.ProcessProgress.WorkEndDateTime)).Sum(),

                        workOrderProcessCheck = x.WorkerOrderProducePlans.Where(x => x.IsDeleted == 0).Select(y => y.ProcessCheck).Sum() >= 1 ? true : false,


                        workOrderStatus =
                            (_db.ProcessProgresses
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrderId)
                                .Select(y => y.ProductionQuantity).Sum() >= x.ProductWorkQuantity || _db.ProcessProgresses
                                                                                                    .Where(y => y.IsDeleted == 0)
                                                                                                    .Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrderId)
                                                                                                    .Where(y => y.WorkStatus == "작업완료").Count() == _db.ProcessProgresses.Where(y => y.IsDeleted == 0).Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrderId).Count()) ? "작업완료" :
                            _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrderId)
                            .Where(y => y.WorkStatus == "작업대기").Count() == _db.ProcessProgresses.Where(y => y.IsDeleted == 0).Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrderId).Count() ? "작업대기" : "작업중",


                    }).ToListAsync();

                var filtered = res.Where(x => param.workOrderStatus == "ALL" || param.workOrderStatus == "" ? true : x.workOrderStatus.Contains(param.workOrderStatus));

                var Res = new Response<IEnumerable<WorkOrderProducePlanResponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filtered
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<WorkOrderProducePlanResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 생산공정 작업목록 불러오기

        #region 생산실적 조회 (Main) GetProcessProgresses
        public async Task<Response<IEnumerable<ProcessProgressResponse001>>> GetProcessProgresses(ProcessProgressRequest001 param)
        {
            try
            {
                var workOrder = await _db.WorkerOrders
                    .Include(x=>x.ProducePlansProduct)
                    .Where(x => x.WorkerOrderId == param.workerOrderId)
                    .FirstOrDefaultAsync();

                if (workOrder.ProducePlansProduct != null)
                {
                    var res = await _db.WorkerOrderProducePlans
                        .Where(x => x.WorkerOrder.WorkerOrderId == param.workerOrderId)
                        //.Where(x => x.ProducePlansProcess.ProducePlansProcessId != 1)
                        .OrderBy(x => x.ProducePlansProcess.ProductProcess.ProcessOrder)
                        .Select(x => new ProcessProgressResponse001
                        {
                            
                            workStartDateTime = x.ProcessProgress.WorkStatus == "작업대기" ? "작업대기" : x.ProcessProgress.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                            workEndDateTime = x.ProcessProgress.WorkStatus == "작업완료" ? x.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm") : "작업완료전",

                            workStatus = x.ProcessProgress.WorkStatus,
                            processOrder = x.ProducePlansProcess.ProductProcess.ProcessOrder,
                            processCode = x.ProducePlansProcess.ProductProcess.Process.Code,
                            processName = x.ProducePlansProcess.ProductProcess.Process.Name,
                            isOutSourcing = x.InOutSourcing == 1 ? true : false,
                            partnerName = x.Partner.Name,
                    
                            workerName = x.Register.FullName,
                            processWorkQuantity = x.ProcessWorkQuantity,
                    
                            /*
                            productionQuantity = _db.LotCounts
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Lot.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                                .Select(y => y.ProduceCount).Sum(),
                    */

                            productionQuantity = x.ProcessProgress.ProductionQuantity,
                            productDefectiveQuantity = _db.LotCounts
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Lot.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                                .Select(y => y.DefectiveCount).Sum(),

                            productGoodQuantity = x.ProcessProgress.ProductionQuantity - _db.LotCounts
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Lot.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                                .Select(y => y.DefectiveCount).Sum(),

                            processCheck = x.ProcessCheck == 1 ? true : false,
                            processCheckResult = x.ProcessCheckResult == 0 ? "합격" : x.ProcessCheckResult == 1 ? "부분합격" : x.ProcessCheckResult == 2 ? "불합격" : "미검사",
                            productLOT = _db.Lots.Where(y => y.IsDeleted == 0).Where(y => y.ProcessProgress == x.ProcessProgress).Where(y=>y.ProcessType == "P").Select(y => y.LotName).FirstOrDefault() ?? "",
                    
                            workProcessMemo = x.WorkProcessMemo ?? ""
                        }).ToListAsync();

                    var Res = new Response<IEnumerable<ProcessProgressResponse001>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;
                }
                else
                {
                    var res1 = await _db.WorkerOrderWithoutPlans
                        .Where(x => x.WorkerOrderProducePlan.WorkerOrder.WorkerOrderId == param.workerOrderId)
                        .Select(x => new ProcessProgressResponse001
                        {

                            workStartDateTime = x.WorkerOrderProducePlan.ProcessProgress.WorkStatus == "작업대기" ? "작업대기" : x.WorkerOrderProducePlan.ProcessProgress.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                            workEndDateTime = x.WorkerOrderProducePlan.ProcessProgress.WorkStatus == "작업완료" ? x.WorkerOrderProducePlan.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm") : "작업종료전",
                            workStatus = x.WorkerOrderProducePlan.ProcessProgress.WorkStatus,


                            processOrder = x.ProductProcess.ProcessOrder,
                            processCode = x.ProductProcess.Process.Code,
                            processName = x.ProductProcess.Process.Name,


                            isOutSourcing = x.WorkerOrderProducePlan.InOutSourcing == 1 ? true : false,
                            partnerName = x.WorkerOrderProducePlan.Partner.Name,

                            workerName = x.WorkerOrderProducePlan.Register.FullName,
                            processWorkQuantity = x.WorkerOrderProducePlan.ProcessWorkQuantity,

                            productionQuantity = _db.LotCounts
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Lot.ProcessProgress.ProcessProgressId == x.WorkerOrderProducePlan.ProcessProgress.ProcessProgressId)
                                .Select(y => y.ProduceCount).Sum(),

                            productDefectiveQuantity = _db.LotCounts
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Lot.ProcessProgress.ProcessProgressId == x.WorkerOrderProducePlan.ProcessProgress.ProcessProgressId)
                                .Select(y => y.DefectiveCount).Sum(),

                            productGoodQuantity = _db.LotCounts
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Lot.ProcessProgress.ProcessProgressId == x.WorkerOrderProducePlan.ProcessProgress.ProcessProgressId)
                                .Select(y => y.ProduceCount).Sum() - _db.LotCounts
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Lot.ProcessProgress.ProcessProgressId == x.WorkerOrderProducePlan.ProcessProgress.ProcessProgressId)
                                .Select(y => y.DefectiveCount).Sum(),

                            processCheck = x.WorkerOrderProducePlan.ProcessCheck == 1 ? true : false,
                            processCheckResult = x.WorkerOrderProducePlan.ProcessCheckResult == 0 ? "합격" : x.WorkerOrderProducePlan.ProcessCheckResult == 1 ? "부분합격" : x.WorkerOrderProducePlan.ProcessCheckResult == 2 ? "불합격" : "미검사",
                            productLOT = _db.Lots.Where(y => y.IsDeleted == 0).Where(y => y.ProcessProgress == x.WorkerOrderProducePlan.ProcessProgress).Where(y => y.ProcessType == "C").Select(y => y.LotName).FirstOrDefault() ?? "",

                            workProcessMemo = x.WorkerOrderProducePlan.WorkProcessMemo ?? "",

                        }).OrderBy(x => x.processOrder).ToListAsync();

                    var Res = new Response<IEnumerable<ProcessProgressResponse001>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res1
                    };

                    return Res;
                }

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessProgressResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
       
        }
        #endregion 생산실적 조회 (Main)

        #region 비가동유형 (MAIN) GetProcessNotWorks

        public async Task<Response<IEnumerable<ProcessNotWorkResponse001>>> GetProcessNotWorks(ProcessNotWorkRequest001 param)
        {
            try
            {
                    DateTime now = DateTime.UtcNow.AddHours(9);

                    var res = await _db.ProcessNotWork
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == param.workerOrderId)
                    .Select(x => new ProcessNotWorkResponse001
                    {
                        shutdownStartDateTime = x.ShutdownStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                        shutdownEndDateTime = x.ShutdownEndDateTime == x.ShutdownStartDateTime ? "-" : x.ShutdownEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        downtime = x.ShutdownEndDateTime == x.ShutdownStartDateTime ? EF.Functions.DateDiffMinute(x.ShutdownStartDateTime, now) : EF.Functions.DateDiffMinute(x.ShutdownStartDateTime, x.ShutdownEndDateTime),
                        
                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null? 
                            x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code : _db.WorkerOrderWithoutPlans.Where(y=>y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkerOrderProducePlanId).Select(y=>y.ProductProcess.Process.Code).FirstOrDefault(),

                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name : _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),
                        
                        shutdownCode = _db.CommonCodes.Where(y=>y.Id == x.ShutdownCodeId).Select(y=> y.Code).FirstOrDefault(),
                        shutdownName = _db.CommonCodes.Where(y=>y.Id == x.ShutdownCodeId).Select(y=> y.Name).FirstOrDefault(),
                        processShutdownMemo = x.ProcessShutdownMemo
                    }).ToListAsync();

                    var Res = new Response<IEnumerable<ProcessNotWorkResponse001>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessNotWorkResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 비가동유형 (MAIN)

        #region 불량유형 (MAIN) GetProcessDefectives

        //불량유형 조회
        public async Task<Response<IEnumerable<ProcessDefectiveResponse001>>> GetProcessDefectives(ProcessDefectiveRequest001 param)
        {
            try
            {
                    var res = await _db.ProcessDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == param.workerOrderId)
                    .Select(x => new ProcessDefectiveResponse001
                    {
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,
                        defectiveQuantity = x.DefectiveCount,
                        defectiveProductMemo = x.DefectiveProductMemo,
                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null?
                            x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code : _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name : _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                    }).ToListAsync();

                    var Res = new Response<IEnumerable<ProcessDefectiveResponse001>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessDefectiveResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 불량유형 (MAIN)

        #region 투입품목 (MAIN) 진행중 GetProductItems
        public async Task<Response<IEnumerable<ProductItemsResponse002>>> GetProductItems(ProductItemsRequest001 param)
        {
            try
            {
                var workOrder = await _db.WorkerOrders
                    .Where(x => x.WorkerOrderId == param.workerOrderId)
                    .Select(x => x.WorkerOrderProducePlans
                        .Where(x => x.IsDeleted == 0)
                        .Select(y => y.ProcessProgress.ProcessProgressId).ToList()
                    ).FirstOrDefaultAsync();


                List<ProductItemsResponse002> resFin = new List<ProductItemsResponse002>();

                foreach(var i in workOrder)
                {
                    var res = await _db.ProcessProgresses
                        .Where(x => x.ProcessProgressId == i)
                        .Select(x =>
                            x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                                x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Items
                                    .Where(y => y.IsDeleted == 0)
                                    .Select(y => new ProductItemsResponse002
                                    {
                                        processProgressId = x.ProcessProgressId,
                                        processCode = x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code,
                                        processName = x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name,
                                        itemId = y.Product.Id,
                                        itemCode = y.Product.Code,
                                        itemName = y.Product.Name,
                                        itemClassification = y.Product.CommonCode.Name,
                                        itemStandard = y.Product.Standard,
                                        itemUnit = y.Product.Unit,
                                        requiredQuantity = (float)y.Require,
                                        LOSS = (float)y.Loss,
                                        totalRequiredQuantity = (float)(y.Require * y.Loss),
                                        productionQuantity = x.ProductionQuantity,
                                        totalInputQuantity = _db.LotCounts
                                            .Where(z => z.Lot.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                                            .Where(z => z.IsDeleted == 0)
                                            .Select(z => z.ConsumeCount).Sum(),

                                        inventory = _db.LotCounts
                                            .Where(z => z.IsDeleted == 0)
                                            .Where(z => z.Product.Id == y.Product.Id)
                                            .Select(z => z.StoreOutCount + z.ProduceCount - z.ConsumeCount - z.DefectiveCount - z.OutOrderCount + z.ModifyCount).Sum(),

                                        itemLOT = _db.Lots
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.ProcessProgress == x)
                                        .Where(z => z.ProcessType == "C")
                                        .Select(z => z.LotName)
                                        .FirstOrDefault()
                                        /*
                                        itemLOT = String.Join(",", _db.Lots
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.ProcessProgress == x)
                                        .Where(z => z.ProcessType == "C")
                                        .Select(z => z.LotName)
                                        .ToArray())*/
                                    }).ToList() :

                                    _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).FirstOrDefault().ProductProcess.Items
                                    .Where(y => y.IsDeleted == 0)
                                    .Select(y => new ProductItemsResponse002
                                    {
                                        processProgressId = x.ProcessProgressId,
                                        processCode = _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                                        processName = _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                                        itemId = y.Product.Id,
                                        itemCode = y.Product.Code,
                                        itemName = y.Product.Name,
                                        itemClassification = y.Product.CommonCode.Name,
                                        itemStandard = y.Product.Standard,
                                        itemUnit = y.Product.Unit,
                                        requiredQuantity = (float)y.Require,
                                        LOSS = (float)y.Loss,
                                        totalRequiredQuantity = (float)(y.Require * y.Loss),
                                        productionQuantity = x.ProductionQuantity,
                                        totalInputQuantity = _db.LotCounts
                                            .Where(z => z.Lot.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                                            .Where(z => z.IsDeleted == 0)
                                            .Select(z => z.ConsumeCount).Sum(),

                                        inventory = _db.LotCounts
                                            .Where(z => z.IsDeleted == 0)
                                            .Where(z => z.Product.Id == y.Product.Id)
                                            .Select(z => z.StoreOutCount + z.ProduceCount - z.ConsumeCount - z.DefectiveCount - z.OutOrderCount + z.ModifyCount).Sum(),

                                        itemLOT = _db.Lots
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.ProcessProgress == x)
                                        .Where(z => z.ProcessType == "C")
                                        .Select(z => z.LotName)
                                        .FirstOrDefault()
                                        /*
                                        itemLOT = String.Join(",", _db.Lots
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.ProcessProgress == x)
                                        .Where(z => z.ProcessType == "C")
                                        .Select(z => z.LotName)
                                        .ToArray())*/
                                    }).ToList()

                            ).FirstOrDefaultAsync();

                    if(res != null)
                    {
                        foreach (var j in res)
                        {
                            resFin.Add(j);
                        }
                    }
                }


                var Res = new Response<IEnumerable<ProductItemsResponse002>>()
                {
                    IsSuccess = true,
                    ErrorMessage ="",
                    Data = resFin
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductItemsResponse002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 투입품목 (MAIN) 

        #region 공정진행현황 팝업 GetWorkOrderProducePlan
        public async Task<Response<WorkOrderProducePlanResponse002>> GetWorkOrderProducePlan(WorkOrderProducePlanRequest001 param)
        {
            try
            {
                var workOrder = await _db.WorkerOrders
                    .Include(x => x.ProducePlansProduct)
                    .Where(x => x.WorkerOrderId == param.workerOrderId)
                    .FirstOrDefaultAsync();

                if (workOrder.ProducePlansProduct != null)
                {
                    var res = await _db.WorkerOrders
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.WorkerOrderId == param.workerOrderId)
                        .Select(x => new WorkOrderProducePlanResponse002
                        {
                            WorkerOrderId = x.WorkerOrderId,
                            WorkOrderNo = x.WorkOrderNo,
                            WorkOrderDate = x.WorkOrderDate.ToString("yyyy-MM-dd"),

                            ProductCode = x.Product.Code,
                            ProductClassification = x.Product.CommonCode.Name,
                            ProductName = x.Product.Name,
                            ProductStandard = x.Product.Standard,
                            ProductUnit = x.Product.Unit,

                            ProductWorkQuantity = x.ProductWorkQuantity,
                            WorkOrderProcessCheck = x.WorkerOrderProducePlans
                                .Where(y => y.IsDeleted == 0)
                                .Select(y => y.ProcessCheck).Sum() > 0 ? true : false,
                            
                            
                            
                        }).FirstOrDefaultAsync();

                    var processProgress = await _db.WorkerOrderProducePlans
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.WorkerOrder.WorkerOrderId == res.WorkerOrderId)
                        .Select(y => new ProcessProgressResponse002
                        {
                            ProcessProgressId = y.ProcessProgress.ProcessProgressId,
                            
                            WorkStatus = y.ProcessProgress.WorkStatus,
                            WorkStartDateTime = y.ProcessProgress.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm") == "9999-12-31 00:00" ? "작업대기" : y.ProcessProgress.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                            WorkEndDateTime = y.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm") == "9999-12-31 00:00" ? "작업완료전" : y.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                            ProcessOrder = y.ProducePlansProcess.ProductProcess.ProcessOrder,
                            ProcessCode = y.ProducePlansProcess.ProductProcess.Process.Code,
                            ProcessName = y.ProducePlansProcess.ProductProcess.Process.Name,
                            IsOutSourcing = y.InOutSourcing == 1? true : false,
                            PartnerName = y.Partner.Name,

                            FacilitiesName = y.Facility.Name,
                            FacilitiesCode = y.Facility.Code,

                            MoldCode = y.Mold.Code,
                            MoldName = y.Mold.Name,

                            WorkerName = y.Register.FullName,

                            ProcessWorkQuantity = y.ProcessWorkQuantity,
                            /*
                            ProductionQuantity = _db.LotCounts
                                .Where(z => z.IsDeleted == 0)
                                .Where(z => z.Lot.ProcessProgress.ProcessProgressId == y.ProcessProgress.ProcessProgressId)
                                .Select(z => z.ProduceCount).Sum(),
                            */
                            ProductionQuantity = y.ProcessProgress.ProductionQuantity,

                            ProductDefectiveQuantity = _db.ProcessDefectives.Where(z=>z.ProcessProgress == y.ProcessProgress).Select(z=>z.DefectiveCount).Sum(),
                            /*
                            ProductDefectiveQuantity = _db.LotCounts
                                .Where(z => z.IsDeleted == 0)
                                .Where(z => z.Lot.ProcessProgress.ProcessProgressId == y.ProcessProgress.ProcessProgressId)
                                .Select(z => z.DefectiveCount).Sum(),
                            */
                            ProcessCheck = y.ProcessCheck == 1? true : false,
                            ProcessCheckResult = y.ProcessCheckResult == 3 ? "미검사": (y.ProcessCheckResult == 0 ? "합격": (y.ProcessCheckResult == 1 ? "부분합격":"불합격")) ,
                            ProductLOT = _db.Lots
                                .Where(z=> z.ProcessProgress.ProcessProgressId == y.ProcessProgress.ProcessProgressId)
                                .Select(z=>z.LotName)
                                .FirstOrDefault(),

                            WorkProcessMemo = y.WorkProcessMemo

                        }).OrderBy(y => y.ProcessOrder).ToListAsync();


                    res.ProcessProgresses = processProgress;

                    var Res = new Response<WorkOrderProducePlanResponse002>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;
        }
        else
        {
            var res1 = await _db.WorkerOrders
                .Where(x => x.IsDeleted == 0)
                .Where(x => x.WorkerOrderId == param.workerOrderId)
                .Select(x => new WorkOrderProducePlanResponse002
                {
                    WorkerOrderId = x.WorkerOrderId,

                    WorkOrderNo = x.WorkOrderNo,

                    WorkOrderDate = x.WorkOrderDate.ToString("yyyy-MM-dd"),

                   ProductCode = x.Product.Code,
                   ProductClassification = x.Product.CommonCode.Name,
                   ProductName = x.Product.Name,
                   ProductStandard = x.Product.Standard,
                   ProductUnit = x.Product.Unit,

                   ProductWorkQuantity = x.ProductWorkQuantity,
                   WorkOrderProcessCheck = x.WorkerOrderProducePlans
                       .Where(y => y.IsDeleted == 0)
                       .Select(y => y.ProcessCheck).Sum() > 0 ? true : false,
                           
                }).FirstOrDefaultAsync();


                     var processProgress = await _db.WorkerOrderProducePlans
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.WorkerOrder.WorkerOrderId == res1.WorkerOrderId)
                        .Select(y => new ProcessProgressResponse002
                        {
                            ProcessProgressId = y.ProcessProgress.ProcessProgressId,
                            WorkStatus = y.ProcessProgress.WorkStatus,
                            WorkStartDateTime = y.ProcessProgress.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm") == "9999-12-31 00:00" ? "작업대기" : y.ProcessProgress.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                            WorkEndDateTime = y.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm") == "9999-12-31 00:00" ? "작업완료전" : y.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm"),


                            ProcessOrder = _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.ProcessOrder).FirstOrDefault(),
                            ProcessCode = _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Code).FirstOrDefault(),
                            ProcessName = _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Name).FirstOrDefault(),


                            IsOutSourcing = y.InOutSourcing == 1 ? true : false,


                            PartnerName = y.Partner.Name,

                            FacilitiesName = y.Facility != null ? y.Facility.Name : "-",
                            FacilitiesCode = y.Facility != null ? y.Facility.Code : "-",

                            MoldCode = y.Mold != null ? y.Mold.Code : "-",
                            MoldName = y.Mold != null ? y.Mold.Name : "-",

                            WorkerName = y.Register.FullName,
                            ElapsedTime = EF.Functions.DateDiffMinute(y.ProcessProgress.WorkStartDateTime, y.ProcessProgress.WorkEndDateTime),

                            ProcessWorkQuantity = y.ProcessWorkQuantity,
                            ProductionQuantity = y.ProcessProgress.ProductionQuantity,
                            /*
                            ProductionQuantity = _db.LotCounts
                                .Where(z => z.IsDeleted == 0)
                                .Where(z => z.Lot.ProcessProgress.ProcessProgressId == y.ProcessProgress.ProcessProgressId)
                                .Select(z => z.ProduceCount).Sum(),
                            */
                            ProductDefectiveQuantity = _db.LotCounts
                                .Where(z => z.IsDeleted == 0)
                                .Where(z => z.Lot.ProcessProgress.ProcessProgressId == y.ProcessProgress.ProcessProgressId)
                                .Select(z => z.DefectiveCount).Sum(),

                            ProcessCheck = y.ProcessCheck == 1 ? true : false,
                            ProcessCheckResult = y.ProcessCheckResult == 3 ? "미검사" : (y.ProcessCheckResult == 0 ? "합격" : (y.ProcessCheckResult == 1 ? "부분합격" : "불합격")),
                            ProductLOT = _db.Lots
                                        .Where(z => z.ProcessProgress.ProcessProgressId == y.ProcessProgress.ProcessProgressId)
                                        .Select(z => z.LotName)
                                        .FirstOrDefault(),
                            WorkProcessMemo = y.WorkProcessMemo

                        }).OrderBy(y => y.ProcessOrder).ToListAsync();


                    res1.ProcessProgresses = processProgress;

                    var Res = new Response<WorkOrderProducePlanResponse002>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res1
                    };

                    return Res;
                }

            }
            catch (Exception ex)
            {
                var Res = new Response<WorkOrderProducePlanResponse002>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        #endregion

        #region 작업시작 = 최초 작업시작인 경우, 작업중단에서 시작되는 경우.  EventWorkStart
        public async Task<Response<EventResult>> EventWorkStart(ProcessNotWorkRequest001 param)
        {
            try
            {
                DateTime now = DateTime.UtcNow.AddHours(9);

                foreach(var i in param.processProgressIdArray)
                {
                    var processProgress = await _db.ProcessProgresses
                        .Where(x => x.ProcessProgressId == i)
                        .FirstOrDefaultAsync();

                    
                    if(processProgress.WorkStatus == "작업대기" || processProgress.WorkStatus == "작업지시")
                    {
                        processProgress.WorkStartDateTime = now;
                        processProgress.WorkEndDateTime = now;

                        processProgress.WorkStatus = "작업중";
                    }

                    if(processProgress.WorkStatus == "작업중지")
                    {
                        var noWork = _db.ProcessNotWork
                            .Where(x=>x.IsDeleted == 0)
                            .Where(x => x.ProcessProgress.ProcessProgressId == processProgress.ProcessProgressId)
                            .Where(x => x.ShutdownEndDateTime == x.ShutdownStartDateTime)
                            .OrderByDescending(x => x.ProcessNotWorkId)
                            .FirstOrDefault();

                        if(noWork != null)
                        {
                            noWork.ShutdownEndDateTime = now;
                            _db.ProcessNotWork.Update(noWork);
                        }

                        processProgress.WorkStatus = "작업중";
                    }


                    _db.ProcessProgresses.Update(processProgress);

                    await Save();

                }

                    var Res = new Response<EventResult>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = new EventResult
                        {
                            result = "SUCCESS"
                        }
                    };

                    return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<EventResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 작업시작 = 최초 작업시작인 경우, 작업중단에서 시작되는 경우. 

        #region 작업중지 => 현재 작업중인 PROCESS에 대해서만 작업중지 + NoWork Save 
        public async Task<Response<EventResult>> EventWorkStop(ProcessNotWorkRequest002 param)
        {
            try
            {
                DateTime now = DateTime.UtcNow.AddHours(9);

                foreach (var i in param.processProgressIdArray)
                {
                    var processProgress = await _db.ProcessProgresses
                        .Where(x => x.ProcessProgressId == i)
                        .FirstOrDefaultAsync();



                    if(processProgress.WorkStatus == "작업중")
                    {
                        var nowork = new ProcessNotWork
                        {
                            ProcessProgress = processProgress,

                            ShutdownStartDateTime = now,
                            ShutdownEndDateTime = now,
                            IsDeleted = 0,
                            ProcessShutdownMemo = param.processShutdownMemo,
                            ShutdownCodeId = param.shutdownCodeId,
                        };


                        processProgress.WorkStatus = "작업중지";
                        _db.ProcessNotWork.Add(nowork);
                        _db.ProcessProgresses.Update(processProgress);

                        await Save();
                    }

                }

                var Res = new Response<EventResult>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;


            }
            catch (Exception ex)
            {
                var Res = new Response<EventResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 작업중지 => 현재 작업중인 PROCESS에 대해서만 작업중지 + NoWork Save SaveNotWork

        #region 비가동 목록 조회 GetNotWorks
        //작업중지조회
        public async Task<Response<IEnumerable<ProcessNotWorkResponse002>>> GetNotWorks(ProcessNotWorkRequest002 param)
        {
            try
            {
                var _processNotWork = await _db.ProcessNotWork
                    .Where(x=> x.IsDeleted == 0)
                    .Where(x => x.ProcessProgress.ProcessProgressId == param.processProgressId)
                    .Select(x => new ProcessNotWorkResponse002
                    {
                        processNotWorkId = x.ProcessNotWorkId,

                        processProgressId = x.ProcessProgress.ProcessProgressId,

                        shutdownCode = _db.CommonCodes.Where(y=>y.Id == x.ShutdownCodeId).Select(y=>y.Code).FirstOrDefault(),
                        shutdownName = _db.CommonCodes.Where(y=>y.Id == x.ShutdownCodeId).Select(y=>y.Name).FirstOrDefault(),
                        downtime = (x.ShutdownEndDateTime - x.ShutdownStartDateTime).Minutes,
                        shutdownStartDateTime = x.ShutdownStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                        shutdownEndDateTime = x.ShutdownStartDateTime == x.ShutdownEndDateTime ? "-" : x.ShutdownEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code : 
                            _db.WorkerOrderWithoutPlans.Where(y=>y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y=>y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),
                        processShutdownMemo = x.ProcessShutdownMemo,

                    }).ToListAsync();
                    
                var Res = new Response<IEnumerable<ProcessNotWorkResponse002>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _processNotWork
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessNotWorkResponse002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 비가동 목록 조회

        #region 비가동 조회 BY ID GetNotWork
        public async Task<Response<ProcessNotWorkResponse003>> GetNotWork(ProcessNotWorkRequest003 param)
        {
            try
            {
                var _processNotWork = await _db.ProcessNotWork
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProcessNotWorkId == param.processNotWorkId)
                    .Select(x => new ProcessNotWorkResponse003
                    {
                        processNotWorkId = x.ProcessNotWorkId,
                        shutdownCodeId = x.ShutdownCodeId,
                        shutdownCode = _db.CommonCodes.Where(y => y.Id == x.ShutdownCodeId).Select(y => y.Code).FirstOrDefault(),
                        shutdownName = _db.CommonCodes.Where(y => y.Id == x.ShutdownCodeId).Select(y => y.Name).FirstOrDefault(),
                        shutdownStartDateTime = x.ShutdownStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                        shutdownEndDateTime = x.ShutdownStartDateTime == x.ShutdownEndDateTime ? DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd HH:mm") : x.ShutdownEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        
                        processProgressId = x.ProcessProgress.ProcessProgressId,
                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),
                      
                        processShutdownMemo = x.ProcessShutdownMemo,

                    }).FirstOrDefaultAsync();

                var Res = new Response<ProcessNotWorkResponse003>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _processNotWork
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<ProcessNotWorkResponse003>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 비가동 조회 BY ID

        #region 비가동 수정 SaveNotWork

        public async Task<Response<EventResult>> SaveNotWork(ProcessNotWorkRequest003 param)
        {
            try
            {
                var notWork = await _db.ProcessNotWork
                    .Where(x => x.ProcessNotWorkId == param.processNotWorkId)
                    .FirstOrDefaultAsync();



                notWork.ProcessProgressId = param.processProgressId;
                notWork.ProcessShutdownMemo = param.processShutdownMemo;
                notWork.ShutdownCodeId = param.shutdownCodeId;
                notWork.ShutdownStartDateTime = Convert.ToDateTime(param.shutdownStartDateTime);
                notWork.ShutdownEndDateTime = Convert.ToDateTime(param.shutdownEndDateTime);

                _db.ProcessNotWork.Update(notWork);
                await Save();

                    var Res = new Response<EventResult>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = null
                    };

                    return Res;
                
            }
            catch (Exception ex)
            {
                var Res = new Response<EventResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 비가동 수정

        #region 비가동유형 삭제 DeleteNotWorks
        public async Task<Response<bool>> DeleteNotWorks (ProcessNotWorkRequest002 param)
        {
            try
            {
                foreach (var i in param.processNotWorkIdArray)
                {
                    var nowork = _db.ProcessNotWork.Where(x => x.ProcessNotWorkId == i).FirstOrDefault();
                    nowork.IsDeleted = 1;

                    _db.ProcessNotWork.Update(nowork);
                    await Save();
                }

                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true
                };
                return Res;
            }
            catch(Exception ex)
            {

                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = ex.Message.ToString(),
                    Data = true
                };
                return Res;
            }

        }



        #endregion 비가동유형 삭제

        #region  ** 비가동유형 조회
        public async Task<Response<IEnumerable<ProcessNotWorkTypeResponse>>> GetProcessNotWorkType()
        {
            try
            {

                var res = await _db.CommonCodes
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.SortCode.Name == "비가동유형")
                    .Select(x => new ProcessNotWorkTypeResponse
                    {
                        shutdownCodeId = x.Id,
                        shutdownCode = x.Code,
                        shutdownName = x.Name,
                    }).ToListAsync();


                var Res = new Response<IEnumerable<ProcessNotWorkTypeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessNotWorkTypeResponse>> ()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 비가동유형 조회

        #region ** 공정진행현황 - 공정 이름 SELECTOR
        public async Task<Response<IEnumerable<ProcessProgressSelectResponse>>> GetProcessProgressSelect(ProcessProgressSelectRequest param)
        {
            try
            {
                var workOrder = await _db.WorkerOrders
                    .Where(x => x.WorkerOrderId == param.WorkerOrderId)
                    .FirstOrDefaultAsync();

                if (workOrder.ProducePlansProduct != null)
                {
                    var res = await _db.ProcessProgresses
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == param.WorkerOrderId)
                        .Select(x => new ProcessProgressSelectResponse
                        {
                            processProgressId = x.ProcessProgressId,
                            processOrder = x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.ProcessOrder,
                            processCode = x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code,
                            processName = x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name,
                        })
                        .OrderBy(x => x.processOrder)
                        .ToListAsync();

                    var Res = new Response<IEnumerable<ProcessProgressSelectResponse>> ()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;
                }
                else
                {
                    var res2 = await _db.ProcessProgresses
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == param.WorkerOrderId)
                        .Select(x => new ProcessProgressSelectResponse
                        {
                            processProgressId = x.ProcessProgressId,
                            processOrder = _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(z => z.ProductProcess.ProcessOrder).FirstOrDefault(),
                            processCode = _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Code).FirstOrDefault(),
                            processName = _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Name).FirstOrDefault(),
                        })
                        .OrderBy(x=>x.processOrder)
                        .ToListAsync();

                    var Res = new Response<IEnumerable<ProcessProgressSelectResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res2
                    };

                    return Res;
                }

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessProgressSelectResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        #endregion

        #region 공정진행현황 불량 조회 BY ID
        public async Task<Response<ProcessDefectiveResponse002>> GetDefective(ProcessDefectiveRequest002 param)
        {
            try
            {
                var _processDefective = await _db.ProcessDefectives
                    .Where(x=>x.ProcessDefectiveId == param.processDefectiveId)
                    .Select(x => new ProcessDefectiveResponse002
                    {

                        processDefectiveId = x.ProcessDefectiveId,
                        processProgressId = x.ProcessProgress.ProcessProgressId,
                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),
                        
                        
                        defectiveId = x.Defective.Id,
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,
                        defectiveProductMemo = x.DefectiveProductMemo,
                        defectiveQuantity = x.DefectiveCount,
                    }).FirstOrDefaultAsync();

                //var _processNotWork = await _db.ProcessDefectives
                //    .Select(x => new ProcessDefectiveResponse002
                //    {
                //        defectiveCode = _db.Defectives.Where(x => x.Id == param.defectiveCodeId).FirstOrDefault().Code,
                //        defectiveName = _db.Defectives.Where(x => x.Id == param.defectiveCodeId).FirstOrDefault().Name,
                //        defectiveQuantity = x.DefectiveCount,
                //        defectiveProductMemo = x.DefectiveProductMemo
                //    })
                //    .FirstOrDefaultAsync();

                var Res = new Response<ProcessDefectiveResponse002>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _processDefective
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<ProcessDefectiveResponse002>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 공정진행현황 불량 조회 BY ID

        #region 공정진행현황 불량 목록 GetDefectives

        public async Task<Response<IEnumerable<ProcessDefectiveResponse001>>> GetDefectives(ProcessDefectiveRequest002 param)
        {
            try
            {
                var _processNotWork = await _db.ProcessDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProcessProgress.ProcessProgressId == param.processProgressId)
                    .Select(x => new ProcessDefectiveResponse001
                    {
                        processDefectiveId = x.ProcessDefectiveId,
                        processProgressId = x.ProcessProgress.ProcessProgressId,

                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,
                        defectiveProductMemo = x.DefectiveProductMemo,
                        defectiveQuantity = x.DefectiveCount,

                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                    }).ToListAsync();

                var Res = new Response<IEnumerable<ProcessDefectiveResponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _processNotWork
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessDefectiveResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        #endregion 공정진행현황 목록 조회

        #region 공정진행현황 불량 추가
        public async Task<Response<EventResult>> CreateDefective(ProcessDefectiveRequest002 param)
        {
            try
            {
                var def = await _db.Defectives.Where(x => x.Id == param.defectiveId).FirstOrDefaultAsync();
                var procProg = await _db.ProcessProgresses.Include(x=>x.WorkOrderProducePlan).ThenInclude(x=>x.Product).Where(x => x.ProcessProgressId == param.processProgressId).FirstOrDefaultAsync();
                
                    _db.Lots.Add(new Data.Models.Lots.LotEntity
                    {
                        IsDeleted = 0,
                        ProcessType = "P",
                        LotName = "",
                        ProcessProgress = procProg,
                    });

                    await Save();
                


                var newProcDef = new ProcessDefective
                {
                    Defective = def,
                    DefectiveCount = param.defectiveQuantity,
                    DefectiveProductMemo = param.defectiveProductMemo,
                    ProcessProgress = procProg,
                    Lot = _db.Lots.Where(x => x.ProcessProgress.ProcessProgressId == param.processProgressId).OrderByDescending(x=>x.LotId).FirstOrDefault(),
                };

                _db.ProcessDefectives.Add(newProcDef);
                await Save();

                var lotCount = new LotCount
                    {
                        Lot = _db.Lots.Where(x => x.ProcessProgress.ProcessProgressId == param.processProgressId).OrderByDescending(x => x.LotId).FirstOrDefault(),
                        DefectiveCount = param.defectiveQuantity,
                        Product = procProg.WorkOrderProducePlan.Product,
                    };

                    _db.LotCounts.Add(lotCount);
                    
                    await Save();
                

                var Res = new Response<EventResult>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<EventResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 공정진행현황 불량 추가

        #region 공정진행현황 불량 업데이트
        public async Task<Response<EventResult>> UpdateDefective(ProcessDefectiveRequest003 param)
        {
            try
            {
                var progDef = await _db.ProcessDefectives.Include(x=>x.Lot).Where(x => x.ProcessDefectiveId == param.processDefectiveId).FirstOrDefaultAsync();

                var def = await _db.Defectives.Where(x => x.Id == param.defectiveId).FirstOrDefaultAsync();
                var procProg = await _db.ProcessProgresses.Where(x => x.ProcessProgressId == param.processProgressId).FirstOrDefaultAsync();

                progDef.Defective = def;
                progDef.ProcessProgress = procProg;
                progDef.DefectiveCount = param.defectiveQuantity;

                _db.ProcessDefectives.Update(progDef);

                var lotcnt =await _db.LotCounts.Where(x => x.IsDeleted == 0).Where(x => x.Lot.LotId == progDef.Lot.LotId).FirstOrDefaultAsync();
                lotcnt.DefectiveCount = param.defectiveQuantity;

                _db.LotCounts.Update(lotcnt);

                await Save();

                var Res = new Response<EventResult>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<EventResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 공정진행현황 불량 업데이트

        #region 공정진행현황 불량 삭제

        public async Task<Response<EventResult>> DeleteDefectives(ProcessDefectiveRequest003 param)
        {
            try
            {
                foreach(var i in param.processDefectiveIdArray)
                {

                    var procDef = await _db.ProcessDefectives.Include(x=>x.Lot).Where(x => x.ProcessDefectiveId == i).FirstOrDefaultAsync();
                    procDef.IsDeleted = 1;
                    _db.ProcessDefectives.Update(procDef);

                    var lot = await _db.Lots.Where(x => x.LotId == procDef.Lot.LotId).FirstOrDefaultAsync();
                    if (lot != null)
                    {
                        lot.IsDeleted = 1;
                        _db.Lots.Update(lot);

                    }

                    var lotCnt = await _db.LotCounts.Where(x => x.Lot.LotId == lot.LotId).FirstOrDefaultAsync();
                    if(lotCnt != null)
                    {
                        lotCnt.IsDeleted = 1;
                        _db.LotCounts.Update(lotCnt);
                    }

                    await Save();
                }

                var Res = new Response<EventResult>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<EventResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 공정진행현황 불량 삭제

        #region 공정진행현황 투입품목조회
        public async Task<Response<IEnumerable<ProductItemsResponse002>>> GetProductItems(ProductItemsRequest002 param)
        {
            try
            {
                var res = await _db.ProcessProgresses
                    .Where(x => x.ProcessProgressId == param.processProgressId)
                    .Select(x =>
                        x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Items
                                .Where(y => y.IsDeleted == 0)
                                .Select(y => new ProductItemsResponse002
                                {
                                    processProgressId = x.ProcessProgressId,
                                    processCode = x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code,
                                    processName = x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name,
                                    itemId = y.Product.Id,
                                    itemCode = y.Product.Code,
                                    itemName = y.Product.Name,
                                    itemClassification = y.Product.CommonCode.Name,
                                    itemStandard = y.Product.Standard,
                                    itemUnit = y.Product.Unit,
                                    requiredQuantity = (float)y.Require,
                                    LOSS = (float)y.Loss,
                                    totalRequiredQuantity = (float)(y.Require * y.Loss),
                                    productionQuantity = x.ProductionQuantity,
                                    totalInputQuantity = _db.LotCounts
                                        .Where(z => z.Lot.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                                        .Where(z => z.IsDeleted == 0)
                                        .Select(z => z.ConsumeCount).Sum(),

                                    inventory = _db.LotCounts
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.Product.Id == y.Product.Id)
                                        .Select(z => z.StoreOutCount + z.ProduceCount - z.ConsumeCount - z.DefectiveCount - z.OutOrderCount + z.ModifyCount).Sum(),

                                    itemLOT = _db.Lots
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.ProcessProgress == x)
                                        .Where(z => z.ProcessType == "C")
                                        .Select(z => z.LotName)
                                        .FirstOrDefault()
                                }).ToList() :

                                _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).FirstOrDefault().ProductProcess.Items
                                .Where(y => y.IsDeleted == 0)
                                .Select(y => new ProductItemsResponse002
                                {
                                    processProgressId = x.ProcessProgressId,
                                    processCode = _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                                    processName = _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                                    itemId = y.Product.Id,
                                    itemCode = y.Product.Code,
                                    itemName = y.Product.Name,
                                    itemClassification = y.Product.CommonCode.Name,
                                    itemStandard = y.Product.Standard,
                                    itemUnit = y.Product.Unit,
                                    requiredQuantity = (float)y.Require,
                                    LOSS = (float)y.Loss,
                                    totalRequiredQuantity = (float)(y.Require * y.Loss),
                                    productionQuantity = x.ProductionQuantity,
                                    totalInputQuantity = _db.LotCounts
                                        .Where(z => z.Lot.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                                        .Where(z => z.IsDeleted == 0)
                                        .Select(z => z.ConsumeCount).Sum(),

                                    inventory = _db.LotCounts
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.Product.Id == y.Product.Id)
                                        .Select(z => z.StoreOutCount + z.ProduceCount - z.ConsumeCount - z.DefectiveCount - z.OutOrderCount + z.ModifyCount).Sum(),

                                    itemLOT = _db.Lots
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.ProcessProgress == x)
                                        .Where(z => z.ProcessType == "C")
                                        .Select(z => z.LotName)
                                        .FirstOrDefault()
                                }).ToList()


                        ).FirstOrDefaultAsync();


                var Res = new Response<IEnumerable<ProductItemsResponse002>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;







            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductItemsResponse002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
      
        
        }

        #endregion 공정진행현황 투입ITEM 목록 조회

        #region 공정진행현황 투입품목상세조회
        public async Task<Response<ProductItemsResponse003>> GetProductItem(ProductItemsRequest002 param)
        {
            try
            {
                var res = await _db.ProcessProgresses
                    .Where(x => x.ProcessProgressId == param.processProgressId)
                    .Select(x => new ProductItemsResponse003
                    {
                        processProgressId = x.ProcessProgressId,
                        processCode = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        itemId = param.itemId,
                        itemCode = _db.Products.Where(x=>x.Id == param.itemId).Select(y=>y.Code).FirstOrDefault(),

                        itemName = _db.Products.Where(x => x.Id == param.itemId).Select(y => y.Name).FirstOrDefault(),

                        itemStandard = _db.Products.Where(x => x.Id == param.itemId).Select(y => y.Standard).FirstOrDefault(),
                        itemUnit = _db.Products.Where(x => x.Id == param.itemId).Select(y => y.Unit).FirstOrDefault(),
                        itemClassification = _db.Products.Where(x => x.Id == param.itemId).Select(y => y.CommonCode.Name).FirstOrDefault(),

                        requiredQuantity = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                           (float)x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Items
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Product.Id == param.itemId)
                                .Select(y => y.Require).FirstOrDefault() :
                            (float)_db.WorkerOrderWithoutPlans
                                .Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId)
                                .Select(y => y.ProductProcess.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Where(z => z.Product.Id == param.itemId)
                                    .Select(z => z.Require).FirstOrDefault())
                                .FirstOrDefault(),

                        LOSS = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            (float)x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Items
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Product.Id == param.itemId)
                                .Select(y => y.Loss).FirstOrDefault() :
                           (float)_db.WorkerOrderWithoutPlans
                                .Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId)
                                .Select(y => y.ProductProcess.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Where(z => z.Product.Id == param.itemId)
                                    .Select(z => z.Loss).FirstOrDefault())
                                .FirstOrDefault(),

                        totalRequiredQuantity = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                                 (float)(x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Items
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Product.Id == param.itemId)
                                .Select(y => y.Loss).FirstOrDefault() 
                                * 
                                x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Items
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.Product.Id == param.itemId)
                                .Select(y => y.Require).FirstOrDefault())
                                :
                                (float)(_db.WorkerOrderWithoutPlans
                                .Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId)
                                .Select(y => y.ProductProcess.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Where(z => z.Product.Id == param.itemId)
                                    .Select(z => z.Require).FirstOrDefault())
                                .FirstOrDefault()
                                *
                                _db.WorkerOrderWithoutPlans
                                .Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId)
                                .Select(y => y.ProductProcess.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Where(z => z.Product.Id == param.itemId)
                                    .Select(z => z.Loss).FirstOrDefault())
                                .FirstOrDefault()),


                        productionQuantity = x.ProductionQuantity,

                        totalInputQuantity = _db.LotCounts
                                        .Where(z => z.Lot.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.Product.Id == param.itemId)
                                        .Select(z => z.ConsumeCount).Sum(),
                    }).FirstOrDefaultAsync();

                //이미 투입된 제품들
                var LotCntSelected = await _db.LotCounts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Product.Id == param.itemId)
                    .Where(x => x.Lot.ProcessProgress.ProcessProgressId == param.processProgressId)
                    .Where(x => x.Lot.ProcessType == "C")
                    //.Where(x => x.Lot.LotName.Contains(param.lotName))
                    .Select(x => new ProductItemStockInterface
                    {

                        processProgressId = param.processProgressId,
                        lotCountId = x.LotCountId,
                        itemId = x.Product.Id,
                        inputQuantity = x.ConsumeCount,
                        isSelected = true,
                        itemStandard = x.Product.Standard,
                        itemClassification = x.Product.CommonCode.Name,
                        itemCode = x.Product.Code,
                        itemLOT = x.Lot.LotName,
                        itemUnit = x.Product.Unit,
                        itemName = x.Product.Name,
                        inventory = _db.LotCounts.Where(y=> y.Lot.LotName == x.Lot.LotName).Where(x=>x.IsDeleted == 0)
                            .Select(y=>y.StoreOutCount + y.ProduceCount - y.DefectiveCount - y.ConsumeCount - y.OutOrderCount + y.ModifyCount)
                            .Sum(),
                        
                    }).ToListAsync();


                var LotCntNotSelected = await _db.LotCounts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Product.Id == param.itemId)
                    .Where(x => x.Lot.ProcessProgress.ProcessProgressId != param.processProgressId)
                    .Where(x => x.Lot.LotName.Contains(param.itemLOT))
                    .Select(x => new ProductItemStockInterface
                    {
                        processProgressId = param.processProgressId,
                        lotCountId = 0,
                        inputQuantity = 0,
                        isSelected = false,
                        itemId = x.Product.Id,
                        itemClassification = x.Product.CommonCode.Name,
                        itemCode = x.Product.Code,
                        itemLOT = x.Lot.LotName,
                        itemUnit = x.Product.Unit,
                        itemName = x.Product.Name,
                        inventory = (x.StoreOutCount + x.ProduceCount - x.DefectiveCount - x.ConsumeCount - x.OutOrderCount + x.ModifyCount),
                        itemStandard = x.Product.Standard
                    }).ToListAsync();

                var grpNotSelected = LotCntNotSelected.GroupBy(x => x.itemLOT);

                List<ProductItemStockInterface> stocks = new List<ProductItemStockInterface>();


                foreach(var i in LotCntSelected)
                {
                    stocks.Add(i);
                }


                bool flag = false;  //중복 체크.

                if(grpNotSelected != null)
                {
                    foreach (var i in grpNotSelected)
                    {
                        var stock = new ProductItemStockInterface();
                        flag = false;
                        foreach (var j in i)
                        {
                            stock.processProgressId = j.processProgressId;
                            stock.itemId = j.itemId;
                            stock.itemLOT = j.itemLOT;
                            stock.lotCountId = 0;
                            stock.inputQuantity = 0;
                            stock.isSelected = false;
                            stock.itemCode = j.itemCode;
                            stock.itemClassification = j.itemClassification;
                            stock.itemUnit = j.itemUnit;
                            stock.itemName = j.itemName;
                            stock.inventory += j.inventory;
                            stock.itemStandard = j.itemStandard;

                            foreach (var k in LotCntSelected)
                            {
                                if (k.itemLOT == j.itemLOT) flag = true;
                             
                            }

                        }
                        if(!flag)    
                            stocks.Add(stock);
                    
                    }
                }

                var res2 = res;
                res2.productItemStocks = stocks;
                
                var Res = new Response<ProductItemsResponse003>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<ProductItemsResponse003>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        #endregion 공정진행현황 투입품목상세조회

        #region 공정진행현황 투입품목 업데이트
        public async Task<Response<EventResult>> UpdateProductItems(ProductItemsResponse003 param)
        {
            try
            {
                var procProg = await _db.ProcessProgresses.Where(x => x.ProcessProgressId == param.processProgressId).FirstOrDefaultAsync();
                var product = await _db.Products.Where(x => x.Id == param.itemId).FirstOrDefaultAsync();

                var prevSelected = await _db.LotCounts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Product.Id == param.itemId)
                    .Where(x => x.Lot.ProcessProgress.ProcessProgressId == param.processProgressId)
                    .Where(x => x.Lot.ProcessType == "C")
                    .Select(x => new ProductItemStockInterface
                    {
                        lotCountId = x.LotCountId,
                    }).ToListAsync();


                List<int> deleteLotCnt = new List<int>();
                bool flag = false;
                
                
                foreach(var i in param.productItemStocks)
                {
                    if(i.lotCountId>0 && i.isSelected == false)
                    {
                        deleteLotCnt.Add(i.lotCountId);
                    }
                }
                
                
                foreach(var i in prevSelected)
                {
                    flag = false;
                    foreach(var j in param.productItemStocks)
                    {   
                        if(i.lotCountId == j.lotCountId)
                        {
                            flag = true;
                        }    
                    }

                    if (!flag)
                    {
                        deleteLotCnt.Add(i.lotCountId);
                    }
                }
                




                foreach(var i in deleteLotCnt)
                {
                    var lotcnt = _db.LotCounts.Include(x=>x.Lot).Where(x => x.LotCountId == i).FirstOrDefault();
                    lotcnt.IsDeleted = 1;
                    _db.LotCounts.Update(lotcnt);

                    var lot = _db.Lots
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.LotId == lotcnt.Lot.LotId)
                        .FirstOrDefault();

                    lot.IsDeleted = 1;

                    _db.Lots.Update(lot);
                }

                await Save();



                foreach(var i in param.productItemStocks)
                {
                    //ADD
                    if(i.lotCountId == 0 && i.isSelected)
                    {
                        var lot = new LotEntity
                        {
                            LotName = i.itemLOT,
                            ProcessProgress = procProg,
                            ProcessType = "C",
                        };
                        _db.Lots.Add(lot);
                        await Save();


                        var newLot = _db.Lots.OrderByDescending(x => x.LotId).FirstOrDefault();
                        _db.LotCounts.Add(new LotCount
                        {
                            Lot = newLot,
                            ConsumeCount = i.inputQuantity,
                            Product = product,
                        });

                        await Save();
                    }

                    //UPDATE
                    if(i.lotCountId > 0 && i.isSelected)
                    {
                        var lotCnt = _db.LotCounts.Where(x => x.LotCountId == i.lotCountId).FirstOrDefault();
                        lotCnt.ConsumeCount = i.inputQuantity;

                        _db.LotCounts.Update(lotCnt);
                        await Save();
                    }
                }


                var Res = new Response<EventResult>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = null
                    };

                    return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<EventResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 공정진행현황 투입품목 업데이트

        #region 작업종료 팝업
        public async Task<Response<WorkStopResponse001>> GetWorkStop(WorkStopRequest001 param)
        {
            try
            {
                DateTime now = DateTime.UtcNow.AddHours(9);

                var res = await _db.ProcessProgresses
                    .Where(x => x.ProcessProgressId == param.processProgressId)
                    .Select(x => new WorkStopResponse001
                    {
                        processProgressId = x.ProcessProgressId,
                        workEndDateTime = x.WorkStatus == "작업종료" ? x.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm") : now.ToString("yyyy-MM-dd HH:mm"),

                        processCheckResult = x.WorkOrderProducePlan.ProcessCheckResult,
                        productionQuantity = x.ProductionQuantity,
                        workProcessMemo = x.WorkOrderProducePlan.WorkProcessMemo,
                    }).FirstOrDefaultAsync();

                var Res = new Response<WorkStopResponse001>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<WorkStopResponse001>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 작업종료 팝업

        #region 작업종료 이벤트
        public async Task<Response<EventResult>> SaveWorkStop(WorkStopRequest001 param)
        {
            try
            {
                string _defLotName = "";

                var rst = await _db.ProcessProgresses
                    .Where(x => x.ProcessProgressId == param.processProgressId)
                    .FirstOrDefaultAsync();

                rst.WorkStatus = "작업완료";
                rst.WorkEndDateTime = Convert.ToDateTime(param.workEndDateTime);
                rst.ProductionQuantity = param.productionQuantity;
                _db.ProcessProgresses.Update(rst);

                var workplan = await _db.WorkerOrderProducePlans.Include(x=>x.Product).Where(x => x.ProcessProgress.ProcessProgressId == param.processProgressId).FirstOrDefaultAsync();
                workplan.WorkProcessMemo = param.workProcessMemo;
                workplan.ProcessCheckResult = param.processCheckResult;

                _db.WorkerOrderProducePlans.Update(workplan);
                await Save();

                if(workplan.Product == null)
                {
                    var ResWithoutPrd = new Response<EventResult>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = null
                    };
                    return ResWithoutPrd;
                }

                var regDate = Convert.ToDateTime(param.workEndDateTime).ToString("yyMMdd");
                var regDate_lot = await _db.Lots
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x => x.LotName.Substring(2, 10))
                    .Where(x => x.LotName.Substring(2, 6) == regDate)
                    .Select(x => x.LotName.Substring(2, 10))
                    .LastOrDefaultAsync();

                var lotNameHead = "L1";
                var lotNameHeadSeq = "";
                var lotNameFull = "";
                var lotSeq = "0000";

                if(regDate_lot != null)
                {
                    lotSeq = regDate_lot.Substring(6, 4);
                }

                lotSeq = String.Format("{0:000#}", Convert.ToInt64(lotSeq) + 1);
                lotNameHeadSeq = string.Format(lotNameHead + regDate + "{0:000#}", Convert.ToInt64(lotSeq));
                lotNameFull = string.Format(lotNameHeadSeq + "{0:000#}", (workplan.Product.Id));
                _defLotName = lotNameFull;

                var lot = await _db.Lots
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProcessProgress.ProcessProgressId == param.processProgressId)
                    .Where(x => x.ProcessType == "P")
                    .FirstOrDefaultAsync();

                if (lot != null)
                {
                    if(lot.LotName == "")
                    {
                        _defLotName = lotNameFull;
                    }
                    else
                    {
                        _defLotName = lot.LotName;
                    }

                    lot.LotName = lot.LotName == "" ? lotNameFull : lot.LotName;
                    
                    _db.Lots.Update(lot);

                    var lotCnt = _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.LotId == lot.LotId)
                        .FirstOrDefault();

                    if(lotCnt != null)
                    {
                        lotCnt.ProduceCount = param.productionQuantity;
                        _db.LotCounts.Update(lotCnt);
                    }
                    else
                    {
                        _db.LotCounts.Add(new LotCount
                        {
                            Product = _db.Products.Where(x => x.Id == workplan.Product.Id).FirstOrDefault(),
                            ProduceCount = param.productionQuantity,
                            Lot = lot,
                        });
                    }

                    var defectiveLots = await _db.Lots
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.ProcessProgress.ProcessProgressId == param.processProgressId)
                        .Where(x => x.LotName == "")
                        .ToListAsync();

                    foreach (var i in defectiveLots)
                    {
                        i.LotName = _defLotName;
                        _db.Lots.Update(i);
                    }


                    await Save();
                }
                else
                {
                    _db.Lots.Add(new LotEntity
                    {
                        LotName = lotNameFull,
                        ProcessProgress = rst,
                        ProcessType = "P",
                    });

                    await Save();

                    var newLot = await _db.Lots.Include(x=>x.ProcessProgress).OrderByDescending(x => x.LotId).FirstOrDefaultAsync();

                    _db.LotCounts.Add(new LotCount
                    {
                        Product = _db.Products.Where(x => x.Id == workplan.Product.Id).FirstOrDefault(),
                        ProduceCount = param.productionQuantity,
                        Lot = newLot,
                    });

                    await Save();

                    var defectiveLots = await _db.Lots
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.ProcessProgress.ProcessProgressId == newLot.ProcessProgress.ProcessProgressId)
                        .Where(x => x.LotName == "")
                        .ToListAsync();

                    foreach(var i in defectiveLots)
                    {
                        i.LotName = lotNameFull;
                        _db.Lots.Update(i);
                    }

                    await Save();
                }

               

                var Res = new Response<EventResult>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;


            }
            catch (Exception ex)
            {
                var Res = new Response<EventResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 작업종료이벤트

        //
        #region 작업수정 
        public async Task<Response<WorkStopResponse001>> GetWorkEdit(WorkStopRequest001 param)
        {
            try
            {
                DateTime now = DateTime.UtcNow.AddHours(9);

                var res = await _db.ProcessProgresses
                    .Where(x => x.ProcessProgressId == param.processProgressId)
                    .Select(x => new WorkStopResponse001
                    {
                        processProgressId = x.ProcessProgressId,
                        workEndDateTime = x.WorkStatus == "작업종료" ? x.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm") : now.ToString("yyyy-MM-dd HH:mm"),
                        workStartDateTime = x.WorkStatus != "작업대기" ? x.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm") : now.ToString("yyyy-MM-dd HH:mm"),
                        workStatus = x.WorkStatus,
                        processCheckResult = x.WorkOrderProducePlan.ProcessCheckResult,
                        productionQuantity = x.ProductionQuantity,
                        workProcessMemo = x.WorkOrderProducePlan.WorkProcessMemo,
                    })
                .FirstOrDefaultAsync();

                var Res = new Response<WorkStopResponse001>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<WorkStopResponse001>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 작업종료 팝업

        #region 작업종료 이벤트
        public async Task<Response<EventResult>> SaveWorkEdit(WorkStopRequest001 param)
        {
            try
            {
                var rst = await _db.ProcessProgresses
                    .Where(x => x.ProcessProgressId == param.processProgressId)
                    .FirstOrDefaultAsync();

                rst.WorkStatus = param.workStatus;
                rst.WorkEndDateTime = Convert.ToDateTime(param.workEndDateTime);
                rst.WorkStartDateTime = Convert.ToDateTime(param.workStartDateTime);
                rst.ProductionQuantity = param.productionQuantity;


                _db.ProcessProgresses.Update(rst);

                var workplan = await _db.WorkerOrderProducePlans.Include(x => x.Product).Where(x => x.ProcessProgress.ProcessProgressId == param.processProgressId).FirstOrDefaultAsync();
                workplan.WorkProcessMemo = param.workProcessMemo;
                workplan.ProcessCheckResult = param.processCheckResult;

                _db.WorkerOrderProducePlans.Update(workplan);

                var lotCnt = await _db.LotCounts
                    .Where(x => x.Lot.ProcessProgress == rst)
                    .FirstOrDefaultAsync();

                if (lotCnt != null)
                {
                    lotCnt.ProduceCount = param.productionQuantity;
                    _db.LotCounts.Update(lotCnt);
                }


                await Save();

                var Res = new Response<EventResult>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;


            }
            catch (Exception ex)
            {
                var Res = new Response<EventResult>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 작업종료이벤트


        public async Task<Response<IEnumerable<ProcessNotWorkQueryResponse>>> GetProcessNotWorksQuery(ProcessNotWorkQueryRequest param)
        {
            try
            {
                var res = await _db.ProcessNotWork
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ShutdownStartDateTime >= Convert.ToDateTime(param.shutdownStartDate))
                    .Where(x => x.ShutdownStartDateTime <= Convert.ToDateTime(param.shutdownEndDate))
                    .Select(x => new ProcessNotWorkQueryResponse
                    {
                        
                        shutdownStartDateTime = x.ShutdownStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                        shutdownEndDateTime = x.ShutdownEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        downtime = (x.ShutdownEndDateTime - x.ShutdownStartDateTime).Minutes,
                        shutdownCode = _db.CommonCodes.Where(y=>y.Id == x.ShutdownCodeId).Select(y=>y.Code).FirstOrDefault(),
                        shutdownName = _db.CommonCodes.Where(y=>y.Id == x.ShutdownCodeId).Select(y=>y.Name).FirstOrDefault(),
                        processShutdownMemo = x.ProcessShutdownMemo,
                        

                        workOrderDate = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        workOrderNo = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,
                        

                        //proc

                        
                        isOutSourcing = x.ProcessProgress.WorkOrderProducePlan.InOutSourcing == 0? false: true,
                        partnerName = x.ProcessProgress.WorkOrderProducePlan.Partner == null ? "-" : x.ProcessProgress.WorkOrderProducePlan.Partner.Name,
                        facilitiesCode = x.ProcessProgress.WorkOrderProducePlan.Facility == null? "-" : x.ProcessProgress.WorkOrderProducePlan.Facility.Code,
                        facilitiesName = x.ProcessProgress.WorkOrderProducePlan.Facility == null? "-" : x.ProcessProgress.WorkOrderProducePlan.Facility.Name,

                        moldName = x.ProcessProgress.WorkOrderProducePlan.Mold == null? "-" : x.ProcessProgress.WorkOrderProducePlan.Mold.Name,
                        moldCode = x.ProcessProgress.WorkOrderProducePlan.Mold == null? "-" : x.ProcessProgress.WorkOrderProducePlan.Mold.Code,

                        workerName = x.ProcessProgress.WorkOrderProducePlan.Register.FullName,
                        productionQuantity = x.ProcessProgress.ProductionQuantity,
                        
                        productLOT = _db.Lots
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessType == "P")
                            .Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                            .Select(y=>y.LotName).FirstOrDefault(),

                        processElapsedTime = x.ProcessProgress.WorkStatus == "작업완료" ? (x.ProcessProgress.WorkEndDateTime - x.ProcessProgress.WorkStartDateTime).Minutes : 0,


                        processId = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        processOrder = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.ProcessOrder :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.ProcessOrder).FirstOrDefault(),

                    }).ToListAsync();


                var res2 = res.Where(x => param.processId == 0 ? true : x.processId == param.processId);



                var Res = new Response<IEnumerable<ProcessNotWorkQueryResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;


            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessNotWorkQueryResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }





        }



        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
