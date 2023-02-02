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
    public class ProcessStatusRepository : IProcessStatusRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProcessStatusRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        #region 비가동내역관리
        public async Task<Response<IEnumerable<ProcessNotWorkQueryResponse>>> GetProcessNotWorksQuery(ProcessNotWorkQueryRequest param)
      {
            try
            {
                DateTime now = DateTime.UtcNow.AddHours(9);

                var res = await _db.ProcessNotWork
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ShutdownStartDateTime >= Convert.ToDateTime(param.shutdownStartDate))
                    .Where(x => x.ShutdownStartDateTime <= Convert.ToDateTime(param.shutdownEndDate).AddDays(1))
                    .Select(x => new ProcessNotWorkQueryResponse
                    {
                        shutdownCodeId = x.ShutdownCodeId,
                        shutdownStartDateTime = x.ShutdownStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                        shutdownEndDateTime = x.ShutdownStartDateTime == x.ShutdownEndDateTime ? "" : x.ShutdownEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        downtime = x.ShutdownStartDateTime == x.ShutdownEndDateTime ? EF.Functions.DateDiffMinute(x.ShutdownStartDateTime, now) : EF.Functions.DateDiffMinute(x.ShutdownStartDateTime, x.ShutdownEndDateTime),

                        shutdownCode = _db.CommonCodes.Where(y=>y.Id == x.ShutdownCodeId).Select(y=>y.Code).FirstOrDefault(),
                        shutdownName = _db.CommonCodes.Where(y=>y.Id == x.ShutdownCodeId).Select(y=>y.Name).FirstOrDefault(),
                        processShutdownMemo = x.ProcessShutdownMemo,
                        
                        workOrderDate = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        workOrderNo = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,

                       
                        isOutSourcing = x.ProcessProgress.WorkOrderProducePlan.InOutSourcing == 0? false: true,
                        partnerName = x.ProcessProgress.WorkOrderProducePlan.Partner.Name ?? "",
                        facilitiesCode = x.ProcessProgress.WorkOrderProducePlan.Facility.Code?? "",
                        facilitiesName = x.ProcessProgress.WorkOrderProducePlan.Facility.Name?? "",

                        moldName = x.ProcessProgress.WorkOrderProducePlan.Mold.Name ?? "",
                        moldCode = x.ProcessProgress.WorkOrderProducePlan.Mold.Code ?? "",

                        workerName = x.ProcessProgress.WorkOrderProducePlan.Register.FullName,
                        productionQuantity = x.ProcessProgress.ProductionQuantity,
                       
                       productLOT = _db.Lots
                           .Where(y=>y.IsDeleted == 0)
                           .Where(y=>y.ProcessType == "P")
                           .Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                           .Select(y=>y.LotName).FirstOrDefault(),
                        
                       processElapsedTime = x.ProcessProgress.WorkEndDateTime == x.ProcessProgress.WorkStartDateTime ? EF.Functions.DateDiffMinute(x.ProcessProgress.WorkStartDateTime, now) : EF.Functions.DateDiffMinute(x.ProcessProgress.WorkStartDateTime, x.ProcessProgress.WorkEndDateTime),
                       
                        processId = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                       
                       processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        processOrder = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.ProcessOrder :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.ProcessOrder).FirstOrDefault(),
                        
                    }).ToListAsync();


                var res2 = res
                     .Where(x => param.processId == 0 ? true : x.processId == param.processId)
                     .Where(x => param.shutdownCodeId == 0 ? true : x.shutdownCodeId == param.shutdownCodeId );



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


        #endregion 비가동내역관리

        #region 공정별생산량관리
        public async Task<Response<IEnumerable<ProductionManageByProcessProcessListResponse>>> GetProductionManageByProcesses(ProductionManageByProcessRequest _req)
        {
            try
            {
                var res = await _db.ProcessProgresses
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkEndDateTime >= Convert.ToDateTime(_req.WorkStartDate))
                    .Where(x => x.WorkEndDateTime <= Convert.ToDateTime(_req.WorkEndDate).AddDays(1))
                   // .Where(x => x.ProducePlansProcess.ProducePlansProcessId != 1)
                   // .Where(x => res.processId == 0 ? true : x.ProducePlansProcess.ProductProcess.ProcessId == res.processId)
                    .Select(x => new ProductionManageByProcessProcessListResponse
                    {
                        WorkEndDateTime = x.WorkEndDateTime.ToString("yyyy-MM-dd"),
                        ProcessId = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.ProcessId :
                            _db.WorkerOrderWithoutPlans.Where(y=>y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y=>y.ProductProcess.ProcessId).FirstOrDefault(),
                        
                        ProcessCode = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),
                        ProcessName = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        
                        ProcessMemo = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Memo :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Memo).FirstOrDefault(),
                    }).ToListAsync();


                var res2 = res.Where(x => _req.ProcessId == 0 ? true : x.ProcessId == _req.ProcessId);
                var res3 = res2.GroupBy(x => x.ProcessCode).ToList();

                List<ProductionManageByProcessProcessListResponse> res4 = new List<ProductionManageByProcessProcessListResponse>();
                
                
                foreach (var r in res3)
                {
                    var _temp = new ProductionManageByProcessProcessListResponse { };
                    _temp.WorkEndDateTime = "2000-01-01";

                    foreach (var x in r)
                    {
                        _temp.ProcessId = x.ProcessId;
                        _temp.WorkEndDateTime = Convert.ToDateTime(_temp.WorkEndDateTime) <= Convert.ToDateTime(x.WorkEndDateTime) ? x.WorkEndDateTime : _temp.WorkEndDateTime;
                        _temp.ProcessMemo = x.ProcessMemo;
                        _temp.ProcessCode = x.ProcessCode;
                        _temp.ProcessName = x.ProcessName;
                    }
                    res4.Add(_temp);
                }
                
                var res5 = res4.OrderBy(x => x.ProcessId);


                var Res = new Response<IEnumerable<ProductionManageByProcessProcessListResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res5
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductionManageByProcessProcessListResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        public async Task<Response<IEnumerable<ProductionManageByProcessWorkOrderListResponse>>> GetProductionManageByProcess(ProductionManageByProcessRequest _req)
        {
            try
            {
                var res = await _db.ProcessProgresses
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkEndDateTime >= Convert.ToDateTime(_req.WorkStartDate))
                    .Where(x => x.WorkEndDateTime <= Convert.ToDateTime(_req.WorkEndDate))
                    // .Where(x => x.ProducePlansProcess.ProducePlansProcessId != 1)
                    // .Where(x => res.processId == 0 ? true : x.ProducePlansProcess.ProductProcess.ProcessId == res.processId)
                    .Select(x => new ProductionManageByProcessWorkOrderListResponse
                    {
                        WorkOrderNo = x.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,
                        WorkEndDate = x.WorkEndDateTime.ToString("yyyy-MM-dd"),
                        WorkOrderDate = x.WorkOrderProducePlan.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        WorkOrderSequence = x.WorkOrderProducePlan.WorkerOrder.WorkOrderSequence,
                        IsOutSourcing = x.WorkOrderProducePlan.InOutSourcing == 1? true: false,
                        PartnerName = x.WorkOrderProducePlan.Partner != null? x.WorkOrderProducePlan.Partner.Name : "",
                        FacilitiesName = x.WorkOrderProducePlan.Facility !=null? x.WorkOrderProducePlan.Facility.Name : "",
                        FacilitiesCode = x.WorkOrderProducePlan.Facility !=null? x.WorkOrderProducePlan.Facility.Code : "",
                        MoldName = x.WorkOrderProducePlan.Mold != null ? x.WorkOrderProducePlan.Mold.Name : "",
                        MoldCode = x.WorkOrderProducePlan.Mold != null ? x.WorkOrderProducePlan.Mold.Code : "",
                        ProcessWorkQuantity = x.WorkOrderProducePlan.ProcessWorkQuantity,
                        ProductionQuantity = x.ProductionQuantity,
                        ProcessCheck = x.WorkOrderProducePlan.ProcessElapsedTime == 1? true:false,
                        WorkerName = x.WorkOrderProducePlan.Register.FullName, 

                        ProductCode = x.WorkOrderProducePlan.Product.Code,
                        ProductName = x.WorkOrderProducePlan.Product.Name,
                        ProductStandard = x.WorkOrderProducePlan.Product.Standard,
                        ProductUnit = x.WorkOrderProducePlan.Product.Unit,
                        ProductClassification = x.WorkOrderProducePlan.Product.CommonCode.Name,

                        ProductLOT = _db.Lots.Where(y=>y.ProcessProgress == x).Where(y=>y.IsDeleted == 0).Select(y=>y.LotName).FirstOrDefault(),

                        ProductBadQuantity = _db.ProcessDefectives
                            .Where(y=>y.ProcessProgress == x)
                            .Where(y=>y.IsDeleted == 0)
                            .Select(y => y.DefectiveCount).Sum(),

                        ProcessId = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.ProcessId :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.ProcessId).FirstOrDefault(),

                    }).ToListAsync();


                var res2 = res.Where(x => _req.ProcessId == 0 ? true : x.ProcessId == _req.ProcessId);
                var Res = new Response<IEnumerable<ProductionManageByProcessWorkOrderListResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;


            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductionManageByProcessWorkOrderListResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        #endregion 공정별생산량관리

        #region LOT 이력 추적
        public async Task<Response<LotInfoResponse>> GetLotInfo(LotInfoRequest _req)
        {
            try
            {
                var res = await _db.Lots
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.LotName == _req.productLOT)
                    .Where(x => x.ProcessType == "P")
                    .Select(x => new LotInfoResponse
                    {
                        ProductLOT = x.LotName,
                        ProductCode = x.LotCounts.Where(y => y.LotId == x.LotId).Select(y => y.Product.Code).FirstOrDefault(),
                        ProductName = x.LotCounts.Where(y => y.LotId == x.LotId).Select(y => y.Product.Name).FirstOrDefault(),
                        ProductUnit = x.LotCounts.Where(y => y.LotId == x.LotId).Select(y => y.Product.Unit).FirstOrDefault(),
                        ProductStandard = x.LotCounts.Where(y => y.LotId == x.LotId).Select(y => y.Product.Standard).FirstOrDefault(),
                        ProductClassification = x.LotCounts.Where(y => y.LotId == x.LotId).Select(y => y.Product.CommonCode.Name).FirstOrDefault(),


                        WorkOrderDate = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        WorkOrderNo = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,
                        TotalElapsedTime = (x.ProcessProgress.WorkEndDateTime - x.ProcessProgress.WorkStartDateTime).Minutes,
                        WorkOrderStatus = x.ProcessProgress.WorkStatus,

                        ProductionQuantity = x.ProcessProgress.ProductionQuantity,
                        ProductDefectiveQuantity = _db.ProcessDefectives
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                            .Select(y => y.DefectiveCount)
                            .Sum(),

                        ProductWorkQuantity = x.ProcessProgress.WorkOrderProducePlan.ProcessWorkQuantity,
                        ProductGoodQuantity = x.ProcessProgress.ProductionQuantity - _db.ProcessDefectives
                                                                                        .Where(y => y.IsDeleted == 0)
                                                                                        .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                                                                                        .Select(y => y.DefectiveCount)
                                                                                        .Sum(),

                        LotProcessInfo = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkerOrderId)
                            .Select(y => new LotProcessInfo
                            {
                                WorkStartDateTime = y.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                                WorkEndDateTime = y.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                                WorkerName = y.WorkOrderProducePlan.Register.FullName,

                                FacilitiesCode = y.WorkOrderProducePlan.Facility != null ? y.WorkOrderProducePlan.Facility.Code : "",
                                FacilitiesName = y.WorkOrderProducePlan.Facility != null ? y.WorkOrderProducePlan.Facility.Name : "",

                                MoldCode = y.WorkOrderProducePlan.Mold != null ? y.WorkOrderProducePlan.Mold.Code : "",
                                MoldName = y.WorkOrderProducePlan.Mold != null ? y.WorkOrderProducePlan.Mold.Name : "",

                                ProcessOrder = y.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? y.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.ProcessOrder
                                    : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.ProcessOrder).FirstOrDefault(),
                                ProcessName = y.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? y.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name
                                    : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Name).FirstOrDefault(),
                                ProcessCode = y.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? y.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code
                                    : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Code).FirstOrDefault(),

                                IsOutSourcing = y.WorkOrderProducePlan.InOutSourcing == 1 ? true : false,
                                PartnerName = y.WorkOrderProducePlan.Partner.Name,

                                ProductionQuantity = y.ProductionQuantity,
                                ProductLOT = _db.Lots
                                    .Where(z => z.ProcessProgress.ProcessProgressId == y.ProcessProgressId)
                                    .Where(z => z.IsDeleted == 0)
                                    .Where(z => z.ProcessType == "P")
                                    .Select(z => z.LotName).FirstOrDefault(),

                                ProductDefectiveQuantity = _db.ProcessDefectives
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.ProcessProgress.ProcessProgressId == y.ProcessProgressId)
                                        .Select(z => z.DefectiveCount)
                                        .Sum(),

                                ProductGoodQuantity = y.ProductionQuantity - _db.ProcessDefectives
                                                                                    .Where(z => z.IsDeleted == 0)
                                                                                    .Where(z => z.ProcessProgress.ProcessProgressId == y.ProcessProgressId)
                                                                                    .Select(z => z.DefectiveCount)
                                                                                    .Sum(),
                            }).ToList()

                    }).FirstOrDefaultAsync();


                var Res = new Response<LotInfoResponse>()
                {
                    IsSuccess = res != null ? true : false,
                    ErrorMessage = res != null ? "" : "검색한 LOT이 존재하지 않습니다.",
                    Data = res != null ? res : new LotInfoResponse { }
                };

                return Res;

            }
            catch(Exception ex)
            {
                var Res = new Response<LotInfoResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<LotInfoInputItemResponse>>> GetLotInputItems(LotInfoRequest param)
        {
            try
            {
                //현재 해당 prodctLOT을 생산하는 공정에 대해서만.

                

                var lotInfo = await _db.Lots
                    .Include(x => x.ProcessProgress)
                    .ThenInclude(x =>x.WorkOrderProducePlan)
                    .ThenInclude(x =>x.WorkerOrder)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.LotName == param.productLOT)
                    .Where(x => x.ProcessType == "P")
                    .FirstOrDefaultAsync();



                if (lotInfo != null)
                {
                    if(lotInfo.ProcessProgress == null)
                    {
                        var ResErr = new Response<IEnumerable<LotInfoInputItemResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = "투입 공정이 존재하지 않습니다.",
                            Data = null
                        };

                        return ResErr;



                    }

                    var lotid = lotInfo.ProcessProgress.ProcessProgressId;

                    var res2 = await _db.ProcessProgresses
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.ProcessProgressId == lotInfo.ProcessProgress.ProcessProgressId)
                        .Select(x => x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Items
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => new LotInfoInputItemResponse
                            {
                                ItemClassification = y.Product.CommonCode.Name,
                                ItemCode = y.Product.Code,
                                ItemName = y.Product.Name,
                                ItemStandard = y.Product.Standard,
                                ItemUnit = y.Product.Unit,
                                ProcessCode = y.ProductProcess.Process.Code,
                                ProcessName = y.ProductProcess.Process.Name,
                                RequiredQuantity = (float)y.Require,
                                LOSS = (float)y.Loss,
                                TotalRequiredQuantity = (float)(y.Require * y.Loss),
                                ProductionQuantity = x.ProductionQuantity,
                                
                                TotalInputQuantity = _db.LotCounts
                                    .Where(z => z.IsDeleted == 0)
                                    .Where(z => z.Lot.ProcessType == "C")
                                    .Where(z => z.Lot.ProcessProgress.ProcessProgressId == lotid)
                                    .Where(z => z.Product.Id == y.Product.Id)
                                    .Select(z => z.ConsumeCount).Sum(),
                                
                                ItemLOT = String.Join(",", _db.LotCounts
                                    .Where(z => z.IsDeleted == 0)
                                    .Where(z => z.Lot.ProcessType == "C")
                                    .Where(z => z.Lot.ProcessProgress.ProcessProgressId == lotid)
                                    .Where(z => z.Product.Id == y.Product.Id)
                                    .Select(z => z.Lot.LotName).ToList())
                                
                            }).ToList() :

                        _db.WorkerOrderWithoutPlans
                            .Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId)
                            .Select(y => y.ProductProcess.Items
                                .Where(y => y.IsDeleted == 0)
                                .Select(y => new LotInfoInputItemResponse
                                {
                                    ItemClassification = y.Product.CommonCode.Name,
                                    ItemCode = y.Product.Code,
                                    ItemName = y.Product.Name,
                                    ItemStandard = y.Product.Standard,
                                    ItemUnit = y.Product.Unit,
                                    ProcessCode = y.ProductProcess.Process.Code,
                                    ProcessName = y.ProductProcess.Process.Name,
                                    RequiredQuantity = (float)y.Require,
                                    LOSS = (float)y.Loss,
                                    TotalRequiredQuantity = (float)(y.Require * y.Loss),
                                    ProductionQuantity = x.ProductionQuantity,
                                    
                                    TotalInputQuantity = _db.LotCounts
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.Lot.ProcessType == "C")
                                        .Where(z => z.Lot.ProcessProgress.ProcessProgressId == lotid)
                                        .Where(z => z.Product.Id == y.Product.Id)
                                        .Select(z => z.ConsumeCount).Sum(),
                                    


                                    ItemLOT = String.Join(",", _db.LotCounts
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.Lot.ProcessType == "C")
                                        .Where(z => z.Lot.ProcessProgress.ProcessProgressId == lotid)
                                        .Where(z => z.Product.Id == y.Product.Id)
                                        .Select(z => z.Lot.LotName).ToList())
                                    
                                }).ToList()
                                ).FirstOrDefault()
                            ).FirstOrDefaultAsync();

                    var Res = new Response<IEnumerable<LotInfoInputItemResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res2
                    };

                    return Res;
                }
                else
                {
                    var Res2 = new Response<IEnumerable<LotInfoInputItemResponse>>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "",
                        Data = null
                    };
                    return Res2;
                }

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<LotInfoInputItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<LotInfoProcessDefectiveResponse>>> GetLotProcessDefective(LotInfoRequest param)
        {
            try
            {

                var lotInfo = await _db.Lots
                    .Include(x => x.ProcessProgress)
                    .ThenInclude(x => x.WorkOrderProducePlan)
                    .ThenInclude(x => x.WorkerOrder)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.LotName == param.productLOT)
                    .Where(x => x.ProcessType == "P")
                    .FirstOrDefaultAsync();


                if(lotInfo != null)
                {
                    var res = await _db.ProcessDefectives
                        .Where(x => x.ProcessProgress == lotInfo.ProcessProgress)
                        .Where(x => x.IsDeleted == 0)
                        .Select(x => new LotInfoProcessDefectiveResponse
                        {
                            DefectiveCode = x.Defective.Code,
                            DefectiveName = x.Defective.Name,
                            DefectiveProductMemo = x.DefectiveProductMemo,
                            DefectiveQuantity = x.DefectiveCount,
                            PartnerName = x.ProcessProgress.WorkOrderProducePlan.Partner !=null? x.ProcessProgress.WorkOrderProducePlan.Partner.Name : "",
                            IsOutSourcing = x.ProcessProgress.WorkOrderProducePlan.InOutSourcing == 1 ? true : false,


                            ProcessName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name
                                    : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Name).FirstOrDefault(),
                            ProcessCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code
                                    : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Code).FirstOrDefault(),

                        }).ToListAsync();

                    var Res = new Response<IEnumerable<LotInfoProcessDefectiveResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;
                }
                else
                {
                    var Res2 = new Response<IEnumerable<LotInfoProcessDefectiveResponse>>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "",
                        Data = null
                    };

                    return Res2;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<LotInfoProcessDefectiveResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<LotInfoProcessInspectionResponse>>> GetLotProcessInspection(LotInfoRequest param)
        {
            try
            {
                 var lotInfo = await _db.Lots
                    .Include(x => x.ProcessProgress)
                    .ThenInclude(x => x.WorkOrderProducePlan)
                    .ThenInclude(x => x.WorkerOrder)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.LotName == param.productLOT)
                    .Where(x => x.ProcessType == "P")
                    .FirstOrDefaultAsync();


                if(lotInfo != null)
                {


                        if (lotInfo.ProcessProgress == null)
                        {
                            var ResErr = new Response<IEnumerable<LotInfoProcessInspectionResponse>>()
                            {
                                IsSuccess = false,
                                ErrorMessage = "공정이 존재하지 않습니다.",
                                Data = null
                            };

                            return ResErr;
                        }

                        var res = await _db.ProcessProgresses
                        .Where(x => x.ProcessProgressId == lotInfo.ProcessProgress.ProcessProgressId)
                        .Select(x => new LotInfoProcessInspectionResponse
                        {

                            WorkerName = x.WorkOrderProducePlan.Register.FullName,

                            FacilitiesCode = x.WorkOrderProducePlan.Facility != null ? x.WorkOrderProducePlan.Facility.Code : "",
                            FacilitiesName = x.WorkOrderProducePlan.Facility != null ? x.WorkOrderProducePlan.Facility.Name : "",

                            MoldCode = x.WorkOrderProducePlan.Mold != null ? x.WorkOrderProducePlan.Mold.Code : "",
                            MoldName = x.WorkOrderProducePlan.Mold != null ? x.WorkOrderProducePlan.Mold.Name : "",

                            ProcessName = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name
                                    : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Name).FirstOrDefault(),
                            ProcessCode = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ? x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code
                                    : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Code).FirstOrDefault(),

                            IsOutSourcing = x.WorkOrderProducePlan.InOutSourcing == 1 ? true : false,
                            PartnerName = x.WorkOrderProducePlan.Partner.Name,

                            ProductionQuantity = x.ProductionQuantity,
                            ProcessCheck = x.WorkOrderProducePlan.ProcessCheck == 1? true : false,
                            ProcessCheckResult = x.WorkOrderProducePlan.ProcessCheckResult == 0 ? "합격" : x.WorkOrderProducePlan.ProcessCheckResult == 1? "부분합격" : x.WorkOrderProducePlan.ProcessCheckResult == 2 ? "불합격" : "미검사",
                            WorkProcessMemo = x.WorkOrderProducePlan.WorkProcessMemo


                        }).ToListAsync();

                    var Res2 = new Response<IEnumerable<LotInfoProcessInspectionResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res2;


                }


                var Res = new Response<IEnumerable<LotInfoProcessInspectionResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<LotInfoProcessInspectionResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<LotInfoProductOutResponse>>> GetLotOutProduct(LotInfoRequest param)
        {
            try
            {
                var lotInfo = await _db.Lots
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.LotName == param.productLOT)
                    .Where(x => x.ProcessType == "O")
                    .Select(x=>x.LotId)
                    .ToListAsync();

                if(lotInfo.Count >=0)
                {
                    List<LotInfoProductOutResponse> res = new List<LotInfoProductOutResponse>();
                    foreach(var i in lotInfo)
                    {
                        var item = await _db.OutOrderProductsStocks
                            .Where(x => x.IsDeleted == 0)
                            .Where(x => x.Lot.LotId == i)
                            .Select(x => new LotInfoProductOutResponse
                            {
                                PartnerName = x.OutOrderProduct.OutOrder.Partner.Name,
                                ShipmentDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                                ProductClassification = x.OutOrderProduct.Product.CommonCode.Name,
                                ProductCode = x.OutOrderProduct.Product.Code,
                                ProductName = x.OutOrderProduct.Product.Name,
                                ProductShipmentCheck = x.OutOrderProduct.Product.ExportCheck,
                                ProductShipmentCheckResult = x.ProductShipmentCheckResult,
                                ProductLOT = x.Lot != null? x.Lot.LotName : x.LotName,
                                ProductStandard = x.OutOrderProduct.Product.Standard,
                                ProductUnit = x.OutOrderProduct.Product.Unit,
                                Quantity = x.Lot != null ? x.Lot.LotCounts.Select(y=>y.OutOrderCount).FirstOrDefault():0,
                                RegisterName = x.OutOrderProduct.OutOrder.Register.FullName,
                                ShipmentNo = x.OutOrderProduct.OutOrder.ShipmentNo,
                                ShipmentProductMemo = x.OutOrderProduct.ShipmentProductMemo,
                            }).FirstOrDefaultAsync();
                        if (item != null)
                        {
                            res.Add(item);
                        }
                    }

                    var Res = new Response<IEnumerable<LotInfoProductOutResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;
                }
                else
                {
                    List<LotInfoProductOutResponse> testRes = new List<LotInfoProductOutResponse>();

                    var test = new LotInfoProductOutResponse
                    {
                        PartnerName = "",
                        ShipmentDate = "",
                        ProductClassification = "",
                        ProductCode = "",
                        ProductName = "",
                        ProductShipmentCheck = true,
                        ProductShipmentCheckResult = "",
                        ProductLOT = "",
                        ProductStandard = "",
                        ProductUnit = "",
                        Quantity = 1,
                        RegisterName = "",
                        ShipmentNo = "",
                        ShipmentProductMemo = "",
                    };

                    testRes.Add(test);


                    var Res2 = new Response<IEnumerable<LotInfoProductOutResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = testRes
                    };

                    return Res2;
                }

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<LotInfoProductOutResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion LOT 이력 추적

        #region 일일생산현황
        public async Task<Response<IEnumerable<DailyProductionResponse>>> GetDailyProductions(DailyProductionRequest _req)
        {
            try
            {
                var res = await _db.ProcessProgresses
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkStatus == "작업완료")
                    .Where(x => x.WorkEndDateTime >= Convert.ToDateTime(_req.WorkEndDateTime))
                    .Where(x => x.WorkEndDateTime <= Convert.ToDateTime(_req.WorkEndDateTime).AddDays(1))
                    .Where(x => _req.ProductClassification == "ALL"? true : x.WorkOrderProducePlan.Product.CommonCode.Name.Contains(_req.ProductClassification))
                    .Where(x => _req.ProductId == 0? true : x.WorkOrderProducePlan.Product.Id == _req.ProductId)
                    .Where(x => x.WorkOrderProducePlan.Product != null)
                    .Where(x => x.ProductionQuantity > 0)
                    .Select(x => new DailyProductionResponse
                    {
                        WorkOrderNo = x.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,
                        WorkEndDateTime = x.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        
                        ProductCode = x.WorkOrderProducePlan.Product.Code,
                        ProductClassification = x.WorkOrderProducePlan.Product.CommonCode.Name, 
                        ProductName = x.WorkOrderProducePlan.Product.Name,
                        ProductStandard = x.WorkOrderProducePlan.Product.Standard,
                        ProductUnit = x.WorkOrderProducePlan.Product.Unit,

                        ProductionQuantity = x.ProductionQuantity,
                        
                        ProductDefectiveQuantity = _db.ProcessDefectives
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                            .Select(y=>y.DefectiveCount)
                            .Sum(),

                        ProductGoodQuantity = x.ProductionQuantity - _db.ProcessDefectives.Where(y => y.IsDeleted == 0).Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgressId).Select(y => y.DefectiveCount).Sum(),

                        ProductLOT = _db.Lots
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessType == "P")
                            .Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                            .Select(y=>y.LotName).FirstOrDefault(),

                        TimeFlag = x.WorkEndDateTime
                    })
                    .OrderBy(x=>x.TimeFlag)
                    .ToListAsync();

                var Res = new Response<IEnumerable<DailyProductionResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<IEnumerable<DailyProductionResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 일일생산현황

        #region 일일불량현황
        public async Task<Response<DailyDefectiveSummaryResponse>> GetDailyDefectiveSummary(DailyDefectiveRequest param)
        {
            try
            {
                int _processDefectiveQuantity = await _db.ProcessDefectives
                     .Where(x => x.IsDeleted == 0)
                     .Where(x => x.ProcessProgress.IsDeleted == 0)
                     .Where(x => x.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(param.DefectiveDate))
                     .Where(x => x.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(param.DefectiveDate).AddDays(1))
                     .Select(x => x.DefectiveCount)
                     .SumAsync();

                int _outOrderProductDefectiveQuantity = await _db.OutOrderProductsDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.OutOrderProduct.IsDeleted == 0)
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate >= Convert.ToDateTime(param.DefectiveDate))
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate <= Convert.ToDateTime(param.DefectiveDate).AddDays(1))
                    .Select(x => x.DefectiveQuantity)
                    .SumAsync();

                int _etcDefectiveQuantity = await _db.EtcDefectivesDetails
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.EtcDefective.IsDeleted == 0)
                    .Where(x => x.EtcDefective.DefectiveDate >= Convert.ToDateTime(param.DefectiveDate))
                    .Where(x => x.EtcDefective.DefectiveDate <= Convert.ToDateTime(param.DefectiveDate).AddDays(1))
                    .Select(x => x.DefectiveQuantity)
                    .SumAsync();

                int _storeProductDefectiveQuantity = await _db.StoreOutStoreProductDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.StoreOutStoreProduct.IsDeleted == 0)
                    .Where(x => x.StoreOutStoreProduct.Receiving.ReceivingDate >= Convert.ToDateTime(param.DefectiveDate))
                    .Where(x => x.StoreOutStoreProduct.Receiving.ReceivingDate <= Convert.ToDateTime(param.DefectiveDate).AddDays(1))
                    .Select(x => x.DefectiveQuantity)
                    .SumAsync();


                var res = new DailyDefectiveSummaryResponse
                {
                    TotalDefectiveQuantity = _etcDefectiveQuantity + _outOrderProductDefectiveQuantity + _processDefectiveQuantity + _storeProductDefectiveQuantity,
                    EtcDefectiveQuantity = _etcDefectiveQuantity,
                    OutOrderProductDefectiveQuantity = _outOrderProductDefectiveQuantity,
                    ProcessDefectiveQuantity = _processDefectiveQuantity,
                    StoreProductDefectiveQuantity = _storeProductDefectiveQuantity,
                };


                var Res = new Response<DailyDefectiveSummaryResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<DailyDefectiveSummaryResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<DailyDefectiveDetailResponse>>> GetDailyDefective(DailyDefectiveRequest _req)
        {
            try
            {
                var _processDefectives = await _db.ProcessDefectives
                     .Where(x => x.IsDeleted == 0)
                     .Where(x => x.ProcessProgress.IsDeleted == 0)
                     .Where(x => x.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(_req.DefectiveDate))
                     .Where(x => x.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(_req.DefectiveDate).AddDays(1))
                     .Where(x=> _req.DefectiveId == 0 ? true : x.Defective.Id == _req.DefectiveId)
                     .Select(x => new DailyDefectiveDetailResponse
                     {
                         DefectiveCode = x.Defective.Code,
                         DefectiveDate = x.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd"),
                         DefectiveName = x.Defective.Name,
                         ProductDefectiveQuantity = x.DefectiveCount,
                         DefectiveUnit = "",
                         Type = "공정",
                         
                         ProductCode = x.ProcessProgress.WorkOrderProducePlan.Product.Code,
                         ProductClassification = x.ProcessProgress.WorkOrderProducePlan.Product.CommonCode.Name,
                         ProductName =x.ProcessProgress.WorkOrderProducePlan.Product.Name,
                         ProductStandard = x.ProcessProgress.WorkOrderProducePlan.Product.Standard,
                         ProductUnit = x.ProcessProgress.WorkOrderProducePlan.Product.Unit,

                         RegisterDate = "",
                         PartnerName = "",

                         Number = "",
                         WorkOrderNo = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,
                         WorkStartDateTime = x.ProcessProgress.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                         ProcessName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProcessProgress.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.ProcessProgress.WorkOrderProducePlan.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                         ProductLOT = _db.Lots
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessType == "P")
                            .Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                            .Select(y=>y.LotName)
                            .FirstOrDefault(),
                         
                         Memo = x.DefectiveProductMemo
                     })
                     .ToListAsync();



                var _outOrderProductDefectives = await _db.OutOrderProductsDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate >= Convert.ToDateTime(_req.DefectiveDate))
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate <= Convert.ToDateTime(_req.DefectiveDate).AddDays(1))
                     .Where(x => _req.DefectiveId == 0 ? true : x.Defective.Id == _req.DefectiveId)

                     .Select(x => new DailyDefectiveDetailResponse
                     {
                         DefectiveCode = x.Defective.Code,
                         DefectiveDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                         DefectiveName = x.Defective.Name,
                         ProductDefectiveQuantity = x.DefectiveQuantity,
                         DefectiveUnit = "",
                         Type = "출고",

                         ProductCode = x.OutOrderProduct.Product.Code,
                         ProductClassification = x.OutOrderProduct.Product.CommonCode.Name,
                         ProductName = x.OutOrderProduct.Product.Name,
                         ProductStandard = x.OutOrderProduct.Product.Standard,
                         ProductUnit = x.OutOrderProduct.Product.Unit,

                         Number = x.OutOrderProduct.OutOrder.ShipmentNo,
                         RegisterDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                         PartnerName = x.OutOrderProduct.OutOrder.Partner.Name,

                         WorkOrderNo = "",
                         WorkStartDateTime = "",
                         ProcessName ="",
                         ProductLOT = _db.Lots
                            .Where(y=>y.LotId == x.Lot.LotId)
                            .Where(x=>x.IsDeleted == 0)
                            .Select(y=>y.LotName)
                            .FirstOrDefault(),
                         Memo = x.DefectiveProductMemo
                     })
                     .ToListAsync();

                var _etcDefectives = await _db.EtcDefectivesDetails
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.EtcDefective.IsDeleted == 0)
                    .Where(x => x.EtcDefective.DefectiveDate >= Convert.ToDateTime(_req.DefectiveDate))
                    .Where(x => x.EtcDefective.DefectiveDate <= Convert.ToDateTime(_req.DefectiveDate).AddDays(1))
                     .Where(x => _req.DefectiveId == 0 ? true : x.EtcDefective.Defective.Id == _req.DefectiveId)

                     .Select(x => new DailyDefectiveDetailResponse
                     {
                         DefectiveCode = x.EtcDefective.Defective.Code,
                         DefectiveDate = x.EtcDefective.DefectiveDate.ToString("yyyy-MM-dd"),
                         DefectiveName = x.EtcDefective.Defective.Name,
                         ProductDefectiveQuantity = x.DefectiveQuantity,
                         DefectiveUnit = "",
                         Type = "기타",

                         ProductCode = x.EtcDefective.Product.Code,
                         ProductClassification = x.EtcDefective.Product.CommonCode.Name,
                         ProductName = x.EtcDefective.Product.Name,
                         ProductStandard = x.EtcDefective.Product.Standard,
                         ProductUnit = x.EtcDefective.Product.Unit,

                         Number = "",
                         RegisterDate = "",
                         PartnerName = "",

                         WorkOrderNo = "",
                         WorkStartDateTime = "",
                         ProcessName = "",
                         ProductLOT = _db.Lots
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.LotId == x.Lot.LotId)
                            .Select(y => y.LotName)
                            .FirstOrDefault(),

                         Memo = x.EtcDefective.EtcDefectiveMemo
                     })
                     .ToListAsync();
               
                var _storeProductDefectives = await _db.StoreOutStoreProductDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.StoreOutStoreProduct.IsDeleted == 0)
                    .Where(x => x.StoreOutStoreProduct.Receiving.ReceivingDate >= Convert.ToDateTime(_req.DefectiveDate))
                    .Where(x => x.StoreOutStoreProduct.Receiving.ReceivingDate <= Convert.ToDateTime(_req.DefectiveDate).AddDays(1))
                     .Where(x => _req.DefectiveId == 0 ? true : x.Defective.Id == _req.DefectiveId)

                     .Select(x => new DailyDefectiveDetailResponse
                     {
                         DefectiveCode = x.Defective.Code,
                         DefectiveDate = x.StoreOutStoreProduct.Receiving.ReceivingDate.ToString("yyyy-MM-dd"),
                         DefectiveName = x.Defective.Name,
                         ProductDefectiveQuantity = x.DefectiveQuantity,
                         DefectiveUnit = "",
                         Type = "입고",

                         ProductCode = x.StoreOutStoreProduct.OutStoreProduct.Product.Code,
                         ProductClassification = x.StoreOutStoreProduct.OutStoreProduct.Product.CommonCode.Name,
                         ProductName = x.StoreOutStoreProduct.OutStoreProduct.Product.Name,
                         ProductStandard = x.StoreOutStoreProduct.OutStoreProduct.Product.Standard,
                         ProductUnit = x.StoreOutStoreProduct.OutStoreProduct.Product.Unit,

                         Number = x.StoreOutStoreProduct.Receiving.ReceivingNo,
                         RegisterDate = x.StoreOutStoreProduct.Receiving.ReceivingDate.ToString("yyyy-MM-dd"),
                         PartnerName = x.StoreOutStoreProduct.OutStoreProduct.OutStore.Partner.Name,

                         WorkStartDateTime = "",
                         ProcessName = "",
                         ProductLOT = _db.Lots
                            .Where(y => y.LotId == x.Lot.LotId)
                            .Where(x => x.IsDeleted == 0)
                            .Select(y => y.LotName)
                            .FirstOrDefault(),
                         Memo = "",
                     })
                     .ToListAsync();



                List<DailyDefectiveDetailResponse> res = new List<DailyDefectiveDetailResponse>();


                foreach(var i in _processDefectives)
                {
                    res.Add(i);
                }

                foreach (var i in _outOrderProductDefectives)
                {
                    res.Add(i);
                }

                foreach (var i in _storeProductDefectives)
                {
                    res.Add(i);
                }

                foreach (var i in _etcDefectives)
                {
                    res.Add(i);
                }

                var res2 = res.Where(x => x.Type.Contains(_req.Type));

                var Res = new Response<IEnumerable<DailyDefectiveDetailResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<DailyDefectiveDetailResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 일일불량현황

        #region 일일출하현황
        public async Task<Response<IEnumerable<DailyOutOrderResponse>>> GetDailyOutOrder(DailyOutOrderRequest _req)
        {
            try
            {
                var res = await _db.OutOrderProductsStocks
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate == Convert.ToDateTime(_req.ShipmentDate))

                    .Where(x => _req.PartnerId == 0? true : x.OutOrderProduct.OutOrder.Partner.Id == _req.PartnerId)
                    .Where(x => _req.ProductId == 0? true : x.OutOrderProduct.Product.Id == _req.ProductId)
                    
                    .Select(x => new DailyOutOrderResponse
                    {
                       
                        ShipmentNo = x.OutOrderProduct.OutOrder.ShipmentNo,
                        ShipmentDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                        PartnerName = x.OutOrderProduct.OutOrder.Partner.Name,
                        
                       ProductCode = x.OutOrderProduct.Product.Code,
                       ProductName = x.OutOrderProduct.Product.Name,
                       ProductClassification = x.OutOrderProduct.Product.CommonCode.Name,
                       ProductUnit = x.OutOrderProduct.Product.Unit,
                       ProductStandard = x.OutOrderProduct.Product.Standard,
                        
                        ProductStandardUnit = x.OutOrderProduct.OrderProduct!=null? x.OutOrderProduct.OrderProduct.ProductStandardUnit : "",
                        ProductStandardUnitCount = x.OutOrderProduct.OrderProduct != null ? x.OutOrderProduct.OrderProduct.ProductStandardUnitCount : 1,
                        
                        ProductLOT = x.Lot != null? x.Lot.LotName : "",
                        ProductSellPrice = x.OutOrderProduct.ProductSellPrice,
                        ProductShipmentCount = x.OutOrderProduct.ProductShipmentCount,
                        ProductSupplyPrice = x.OutOrderProduct.ProductSupplyPrice,
                        ShipmentProductMemo = x.OutOrderProduct.ShipmentProductMemo,
                        ProductTaxPrice = x.OutOrderProduct.ProductTaxPrice,
                        ProductTotalPrice = x.OutOrderProduct.ProductTotalPrice,
                        
                    }).ToListAsync();

                var res2 = res.GroupBy(x => x.ProductCode);

                var res3 = res2.Select(x => new DailyOutOrderResponse
                {
                    ShipmentNo = x.FirstOrDefault().ShipmentNo,
                    ShipmentDate = x.FirstOrDefault().ShipmentDate,
                    PartnerName = x.FirstOrDefault().PartnerName,
                    ProductCode = x.Key,
                    ProductName = x.FirstOrDefault().ProductName,
                    ProductClassification = x.FirstOrDefault().ProductClassification,
                    ProductUnit = x.FirstOrDefault().ProductUnit,
                    ProductStandard = x.FirstOrDefault().ProductStandard,
                    ProductStandardUnit = x.FirstOrDefault().ProductStandardUnit,
                    ProductStandardUnitCount = x.FirstOrDefault().ProductStandardUnitCount,
                    ProductLOT = String.Join(",",x.Select(y=>y.ProductLOT)),
                    ProductSellPrice = x.FirstOrDefault().ProductSellPrice,
                    ProductShipmentCount = x.FirstOrDefault().ProductShipmentCount,
                    ProductSupplyPrice = x.FirstOrDefault().ProductSupplyPrice,
                    ShipmentProductMemo = String.Join(",", x.Select(y => y.ShipmentProductMemo)),
                    ProductTaxPrice = x.FirstOrDefault().ProductTaxPrice,
                    ProductTotalPrice = x.FirstOrDefault().ProductTotalPrice
                }).ToList();


                var Res = new Response<IEnumerable<DailyOutOrderResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res3
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<DailyOutOrderResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        #endregion 일일출하현황


        #region 설비별생산현황
        public async Task<Response<IEnumerable<ProductionManageByFacilityResponse>>> GetProductionManageByFacility(ProductionManageByFacilityRequest _req)
        {
            try
            {
                var res = await _db.ProcessProgresses
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkEndDateTime >= Convert.ToDateTime(_req.WorkStartDate))
                    .Where(x => x.WorkEndDateTime <= Convert.ToDateTime(_req.WorkEndDate).AddDays(1))
                    .Where(x => _req.FacilityId == 0 ? true : x.WorkOrderProducePlan.Facility.Id == _req.FacilityId)
                    .Where(x => _req.MoldId == 0 ? true : x.WorkOrderProducePlan.Mold.Id == _req.MoldId) 
                    .Where(x => x.WorkStatus == "작업완료")
                    .Where(x => x.WorkOrderProducePlan.Facility != null)
                    .Select(x => new ProductionManageByFacilityResponse
                    {
                        RegisterDate = x.WorkEndDateTime.ToString("yyyy-MM-dd"),
                        WorkOrderNo = x.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,
                        FacilityCode = x.WorkOrderProducePlan.Facility.Code,
                        FacilityName = x.WorkOrderProducePlan.Facility.Name,

                        MoldCode = x.WorkOrderProducePlan.Mold.Code,
                        MoldName = x.WorkOrderProducePlan.Mold.Name,

                        ProductCode = x.WorkOrderProducePlan.Product.Code,
                        ProductName = x.WorkOrderProducePlan.Product.Name,
                        ProductClassification = x.WorkOrderProducePlan.Product.CommonCode.Name,
                        ProductUnit = x.WorkOrderProducePlan.Product.Unit,
                        ProductStandard = x.WorkOrderProducePlan.Product.Standard,

                        ProductionQuantity = x.ProductionQuantity,
                        ProductionElapseTime = EF.Functions.DateDiffMinute(x.WorkStartDateTime, x.WorkEndDateTime),
                        ProductionDownTime = x.ProcessNotWorks.Where(y=>y.IsDeleted == 0).Select(y => EF.Functions.DateDiffMinute(y.ShutdownStartDateTime, y.ShutdownEndDateTime)).Sum(),
                        WorkerName = x.WorkOrderProducePlan.Register.FullName,
                        ProductLOT = _db.Lots.Where(y=>y.ProcessType == "P").Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgressId).FirstOrDefault().LotName ?? ""

                    }).ToListAsync();


                var resFiltered = res.Where(x => x.ProductLOT.Contains(_req.ProductLOT));


                var Res = new Response<IEnumerable<ProductionManageByFacilityResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = resFiltered
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductionManageByFacilityResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }



        #endregion


        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
