using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProducePlan;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebBasedMES.Data.Models.ProducePlan;
using WebBasedMES.Data.Models.Lots;

namespace WebBasedMES.Services.Repositories.ProducePlanManage
{
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkOrderRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        #region  작업지시목록 조회
        public async Task<Response<IEnumerable<WorkOrderReponse001>>> GetWorkOrders(WorkOrderRequest001 _req)
        {
            try
            {
                var res = await _db.WorkerOrders
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => _req.productId == 0 ? true : x.Product.Id == _req.productId)
                    .Where(x => _req.registerId =="" ? true : x.Register.Id == _req.registerId)
                    .Where(x => x.RegisterDate >= Convert.ToDateTime(_req.registerStartDate))
                    .Where(x => x.RegisterDate <= Convert.ToDateTime(_req.registerEndDate))
                    .Where(x => _req.workOrderStatus == "ALL" ? true : true)
                    .Where(x => x.WorkOrderNo.Contains(_req.workOrderNo))
                    .Select(x => new WorkOrderReponse001
                    {
                        workerOrderId = x.WorkerOrderId,
                        workOrderNo = x.WorkOrderNo,//작업지시번호
                        workOrderDate = x.WorkOrderDate.ToString("yyyy-MM-dd"),//작업지시일
                        workOrderSequence = x.WorkOrderSequence,//작업순서
                        productCode = x.Product.Code,//제품코드
                        productClassification = x.Product.CommonCode.Name,//제품구분
                        productName = x.Product.Name,//제품이름
                        productStandard = x.Product.Standard,//규격
                        productUnit = x.Product.Unit,//단위
                        
                        productPlanQuanity = x.ProducePlansProduct == null? 0 : x.ProducePlansProduct.ProductPlanQuantity ,//생산계획수량
                       // productPlanBacklog = x.ProducePlansProduct.ProductPlanQuantity - x.ProductWorkQuantity,//계획수량-지시수량
                        productPlanBacklog = x.ProducePlansProduct.ProductPlanQuantity - _db.WorkerOrders
                            .Where(y=>y.ProducePlansProduct == x.ProducePlansProduct)
                            .Where(y=> y.IsDeleted == 0)
                            .Where(y => Convert.ToInt64(y.WorkOrderNo.Substring(2, 12)) <= Convert.ToInt64(x.WorkOrderNo.Substring(2, 12)))
                            .Select(y=>y.ProductWorkQuantity).Sum(),//계획수량-지시수량
                        productWorkQuantity = x.ProductWorkQuantity,//지시수량

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


                        registerDate = x.RegisterDate.ToString("yyyy-MM-dd"),//등록일
                        registerName = x.Register.FullName,//등록자
                        productionPlanNo = x.ProducePlansProduct.ProducePlan.ProductionPlanNo,//생산계획Id
                        workOrderMemo = x.WorkOrderMemo,//비고
                    })
                    .OrderByDescending(x=>x.workOrderNo)
                    .ToListAsync();

                var filter = res.Where(x => _req.workOrderStatus == "ALL" ? true : x.workOrderStatus.Contains(_req.workOrderStatus));

                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filter
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<WorkOrderReponse001>>> GetWorkOrdersMain(WorkOrderRequest001 _req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => _req.productId == 0 ? true : x.Product.Id == _req.productId)
                    .Where(x => _req.registerId == "" ? true : x.Register.Id == _req.registerId)
                    .Where(x => x.WorkerOrder.RegisterDate >= Convert.ToDateTime(_req.registerStartDate))
                    .Where(x => x.WorkerOrder.RegisterDate <= Convert.ToDateTime(_req.registerEndDate))
                    .Select(x => new WorkOrderReponse001
                    {
                        workerOrderId = x.WorkerOrder.WorkerOrderId,
                        workOrderNo = x.WorkerOrder.WorkOrderNo,//작업지시번호
                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),//작업지시일
                        workOrderSequence = x.WorkerOrder.WorkOrderSequence,//작업순서
                        productCode = x.WorkerOrder.Product.Code,//제품코드
                        productPlanQuanity = x.WorkerOrder.ProducePlansProduct.ProductPlanQuantity,//생산계획수량
                                                                                       // productPlanBacklog = x.ProducePlansProduct.ProductPlanQuantity - x.ProductWorkQuantity,//계획수량-지시수
                        productWorkQuantity = x.WorkerOrder.ProductWorkQuantity,//지시수량

                        workOrderStatus =
                            (_db.ProcessProgresses
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrder.WorkerOrderId)
                                .Select(y => y.ProductionQuantity).Sum() >= x.WorkerOrder.ProductWorkQuantity || _db.ProcessProgresses
                                                                                                    .Where(y => y.IsDeleted == 0)
                                                                                                    .Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrder.WorkerOrderId)
                                                                                                    .Where(y => y.WorkStatus == "작업완료").Count() == _db.ProcessProgresses.Where(y => y.IsDeleted == 0).Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrder.WorkerOrderId).Count()) ? "작업완료" :
                            _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrder.WorkerOrderId)
                            .Where(y => y.WorkStatus == "작업대기").Count() == _db.ProcessProgresses.Where(y => y.IsDeleted == 0).Where(y => y.WorkOrderProducePlan.WorkerOrder.WorkerOrderId == x.WorkerOrder.WorkerOrderId).Count() ? "작업대기" : "작업중",


                        registerDate = x.WorkerOrder.RegisterDate.ToString("yyyy-MM-dd"),//등록일
                        registerName = x.Register.FullName,//등록자
                        facilityName = x.Facility!= null? x.Facility.Name : "",
                        processName = x.ProducePlansProcess != null ? x.ProducePlansProcess.ProductProcess.Process.Name : _db.WorkerOrderWithoutPlans.Where(z=>z.WorkerOrderProducePlan == x).Select(z=>z.ProductProcess.Process.Name).FirstOrDefault()

                    })
                    .OrderByDescending(x => x.workOrderNo)
                    .ToListAsync();

                var filter = res.Where(x => _req.workOrderStatus == "ALL" ? true : x.workOrderStatus.Contains(_req.workOrderStatus));

                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filter
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        #endregion  작업지시목록 조회

        #region   작업지시 세부사항 조회
        public async Task<Response<WorkOrderResponse>> GetWorkOrderDetail(WorkOrderRequest _req)
        {
            try
            {

                var res = await _db.WorkerOrders
                    .Where(x => x.WorkerOrderId == _req.WorkerOrderId)
                    .Select(x => new WorkOrderResponse
                    {
                        WorkerOrderId = x.WorkerOrderId,

                        ProductId = x.Product.Id,
                        ProducePlansProductId = x.ProducePlansProduct == null ? 0 : x.ProducePlansProduct.ProducePlansProductId,
                        WorkOrderNo = x.WorkOrderNo,
                        WorkOrderDate = x.WorkOrderDate.ToString("yyyy-MM-dd"),
                        WorkOrderSequence = x.WorkOrderSequence,
                        ProductionPlanNo = x.ProducePlansProduct == null ? "" : x.ProducePlansProduct.ProducePlan.ProductionPlanNo,
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,

                        ProductPlanQuanity = x.ProducePlansProduct == null ? 0 : x.ProducePlansProduct.ProductPlanQuantity,
                        // ProductPlanBacklog = x.ProducePlansProduct == null ? 0 : x.ProducePlansProduct.ProductPlanQuantity - x.ProductWorkQuantity,
                        ProductPlanBacklog = x.ProducePlansProduct == null ? 0 : (x.ProducePlansProduct.ProductPlanQuantity - _db.WorkerOrders
                            .Where(y => y.ProducePlansProduct == x.ProducePlansProduct)
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y => Convert.ToInt64(y.WorkOrderNo.Substring(2,12))<= Convert.ToInt64(x.WorkOrderNo.Substring(2, 12)))
                            .Select(y => y.ProductWorkQuantity).Sum()),
                        ProductWorkQuantity = x.ProductWorkQuantity,
                        WorkOrderStatus =
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


                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        RegisterId = x.Register.Id,
                        RegisterName = x.Register.FullName,
                        WorkOrderMemo = x.WorkOrderMemo,

                        WorkerOrderProducePlans = _db.WorkerOrderProducePlans
                             .Where(y => y.WorkerOrder.WorkerOrderId == _req.WorkerOrderId)
                             .Where(y => y.IsDeleted == 0)
                             .Select(y => new WorkerOrderProducePlansInterface
                             {

                                 workerOrderProducePlanId = y.WorkerOrderProducePlanId,
                                 processOrder = x.ProducePlansProduct != null ? y.ProducePlansProcess.ProductProcess.ProcessOrder
                                      : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.ProcessOrder).FirstOrDefault(),//공정순서

                                 processCode = x.ProducePlansProduct != null ? y.ProducePlansProcess.ProductProcess.Process.Code //공정코드
                                      : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Code).FirstOrDefault(),//공정순서

                                 processName = x.ProducePlansProduct != null ? y.ProducePlansProcess.ProductProcess.Process.Name//공정이름
                                      : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Name).FirstOrDefault(),//공정순서

                                 processId = x.ProducePlansProduct != null ? y.ProducePlansProcess.ProductProcess.Process.Id//공정이름
                                      : _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductProcess.Process.Id).FirstOrDefault(),//공정순서

                                 isOutSourcing = y.InOutSourcing == 0 ? false : true,//자체/외주
                                 partnerId = y.Partner != null ? y.Partner.Id : 0,
                                 partnerName = y.Partner.Name ?? "",//거래처
                                 facilityId = y.Facility != null ? y.Facility.Id : 0,
                                 facilitiesCode = y.Facility.Code ?? "",//설비코드
                                 facilitiesName = y.Facility.Name ?? "",//설비이름
                                 moldId = y.Mold != null ? y.Mold.Id : 0,
                                 moldCode = y.Mold.Code ?? "",//금형코드
                                 moldName = y.Mold.Name ?? "",//금형이름
                                 workerName = y.Register.FullName,//작업자
                                 workerId = y.Register.Id,

                                 processPlanQuantity = x.ProducePlansProduct != null ? y.ProducePlansProcess.ProcessPlanQuantity : 0,//계획수량
                                 processPlanBacklog = x.ProducePlansProduct != null ? y.ProducePlansProcess.ProcessPlanQuantity - y.ProcessWorkQuantity : 0,//계획잔량=계획수량-지시수량
                                 processWorkQuantity = y.ProcessWorkQuantity,//지시수량

                                 
                                 productGoodQuantity =  _db.ProcessProgresses.Where(z => z.WorkOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductionQuantity).FirstOrDefault() 
                                                    - _db.ProcessDefectives.Where(y => y.IsDeleted == 0).Where(z => z.ProcessProgress.ProcessProgressId == _db.ProcessProgresses.Where(z => z.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProcessProgressId).FirstOrDefault()).Select(y => y.DefectiveCount).Sum(),
                                 

                                 productDefectiveQuantity = _db.ProcessDefectives
                                    .Where(y => y.IsDeleted == 0)
                                    .Where(z => z.ProcessProgress.ProcessProgressId == _db.ProcessProgresses.Where(z=>z.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z=>z.ProcessProgressId).FirstOrDefault())
                                    .Select(y => y.DefectiveCount)
                                    .Sum(), 
                                 

                                 processElapsedTime = _db.ProcessProgresses
                                    .Where(z=>z.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId)
                                    .Select(z=>  EF.Functions.DateDiffMinute(z.WorkStartDateTime, z.WorkEndDateTime))
                                    .FirstOrDefault(), //소요시간(분)
                                 
                                 processCheck = y.ProcessCheck == 0 ? false : true,//공정검사여부
                                 processCheckResult = y.ProcessCheckResult == 3? "미검사" : y.ProcessCheckResult == 0 ? "합격": y.ProcessCheckResult == 1? "부분합격":"불합격",
                                 productionQuantity = _db.ProcessProgresses.Where(z => z.WorkOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(z => z.ProductionQuantity).FirstOrDefault(),


                             }).OrderBy(y=>y.processOrder).ToList()



                    }).FirstOrDefaultAsync();

                var test = _db.WorkerOrderProducePlans
                             .Where(y => y.WorkerOrder.WorkerOrderId == _req.WorkerOrderId)
                             .Where(y => y.IsDeleted == 0)
                             .ToList();





                var Res = new Response<WorkOrderResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<WorkOrderResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion   작업지시 세부사항 조회

        #region 작업지시 등록
        public async Task<Response<IEnumerable<WorkOrderReponse001>>> CreateWorkOrder(WorkOrderRequest002 param)
        {
            try
            {
                //작업지시번호규칙 : WO+날짜(YYYYMMDD)+순서(4자리), 예시) WO202204220001
                var _workerOrderNo = await _db.WorkerOrders.Where(x=>x.RegisterDate == Convert.ToDateTime(param.registerDate)).OrderByDescending(x => x.WorkOrderNo).FirstOrDefaultAsync();
                int increase = 1;
                if (_workerOrderNo != null)
                {
                    increase = Int32.Parse(_workerOrderNo.WorkOrderNo.Substring(10, 4)) + 1;
                }
                var date = Convert.ToDateTime(param.registerDate).ToString("yyyyMMdd");
                var _newWorkerOrderNo = $"WO{date}{increase:000#}";

                var _user = await _userManager.FindByIdAsync(param.registerId);
                var _producePlanProduct = _db.ProducePlanProducts.Include(x=>x.ProducePlan).Where(x => x.ProducePlansProductId == param.producePlansProductId).FirstOrDefault();

                var _newWorkerOrder = new WorkerOrder
                {
                    WorkOrderNo = _newWorkerOrderNo,
                    WorkOrderDate = Convert.ToDateTime(param.workOrderDate),//작업지시일
                    WorkOrderSequence = param.workOrderSequence,//작업순서
                    ProducePlansProduct = _producePlanProduct, 
                    Product = _db.Products.Where(x => x.Id == param.productId).FirstOrDefault(),//제품코드 가져옴
                    ProductWorkQuantity = param.productWorkQuantity,//지시수량
                    RegisterDate = Convert.ToDateTime(param.registerDate),//등록일
                    Register = _user,//등록자
                    WorkOrderMemo = param.workOrderMemo,//비고
                    IsDeleted = 0,
                };

                await _db.WorkerOrders.AddAsync(_newWorkerOrder);

                if(_producePlanProduct != null)
                {
                    var _producePlan = _db.ProducePlans.Where(x => x.ProducePlanId == _producePlanProduct.ProducePlan.ProducePlanId).FirstOrDefault();
                    _producePlan.ProductionPlanStatus = "작업대기";

                    _db.ProducePlans.Update(_producePlan);
                }

                await Save();

                
                


                var _workerOrder = await _db.WorkerOrders.Where(y => y.WorkOrderNo == _newWorkerOrderNo).FirstOrDefaultAsync();
                var _newWorkerOrderProducePlans = new List<WorkerOrderProducePlan>();


                if (param.workerOrderProducePlans != null)
                {
                    foreach (var i in param.workerOrderProducePlans)
                    {
                        var _register = await _userManager.FindByIdAsync(i.workerId);
                        var _workerOrderProducePlan = new WorkerOrderProducePlan
                        {
                            WorkerOrder = _workerOrder,
                            ProducePlansProcess = param.producePlansProductId != 0? _db.ProducePlanProcesses.Where(x => x.ProducePlansProcessId == i.producePlansProcessId).FirstOrDefault() : _db.ProducePlanProcesses.Where(x => x.ProductProcess.ProcessOrder == i.processOrder).FirstOrDefault(),//공정
                            Product = _db.Products.Where(x => x.Id == i.productId).FirstOrDefault(),
                            InOutSourcing = i.isOutSourcing == true ? 1:0 ,//자체/외주
                            Partner =  _db.Partners.Where(x => x.Id == i.partnerId).FirstOrDefault(),//거래처
                            Facility = _db.Facilitys.Where(x => x.Id == i.facilityId).FirstOrDefault(),//설비
                            Mold = _db.Molds.Where(x => x.Id == i.moldId).FirstOrDefault(),//금형
                            Register = _register,//작업자
                            ProcessWorkQuantity = i.processWorkQuantity,//지시수량
                            ProcessElapsedTime = 0,//소요시간
                            ProcessCheck = i.processCheck == true? 1:0,//공정검사여부
                            ProcessCheckResult = 3,//공정검사결과                                                    
                            IsDeleted = 0
                        };
                        _newWorkerOrderProducePlans.Add(_workerOrderProducePlan);
                    }
                }

                _workerOrder.WorkerOrderProducePlans = _newWorkerOrderProducePlans;
                _db.WorkerOrders.Update(_workerOrder);
                await Save();

                var _workorderProducePlans = _db.WorkerOrderProducePlans.Include(x => x.ProducePlansProcess).Where(x => x.IsDeleted == 0).OrderByDescending(x => x.WorkerOrderProducePlanId).Take(param.workerOrderProducePlans.Count());
          

                if(param.producePlansProductId == 0)
                {
                    int i = 0;
                    var productProcesses = _db.ProductProcesses.Where(x => x.ProductId == param.productId).Where(x => x.IsDeleted == 0).OrderByDescending(x => x.ProcessOrder).ToArray();
                    List<WorkerOrderWithoutPlan> _workerOrderWithoutPlan = new List<WorkerOrderWithoutPlan>();

                    foreach (var plan in _workorderProducePlans)
                    {
                        _db.WorkerOrderWithoutPlans.Add(new WorkerOrderWithoutPlan { WorkerOrderProducePlan = plan, ProductProcess = productProcesses[i] });
                        i++;
                    }

                    await Save();
                }


                List<ProcessProgress> _procProgs = new List<ProcessProgress>();

                foreach (var x in _workorderProducePlans)
                {
                    var _procProg = new ProcessProgress
                    {
                        WorkOrderProducePlan = x,
                        WorkStartDateTime = Convert.ToDateTime("9999-12-31"),
                        WorkEndDateTime = Convert.ToDateTime("9999-12-31"),
                        ProductionQuantity = 0,
                        WorkStatus = "작업대기",
                        IsDeleted = 0,
                        
                    };

                    _procProgs.Add(_procProg);
                }

                _db.ProcessProgresses.AddRange(_procProgs);
                await Save();

                // LOT은 나중에 실제로 작업 완료 시점에서. 
                /*

                var pps = _db.ProcessProgresses.OrderByDescending(x => x.ProcessProgressId).Take(_procProgs.Count());

                List<LotEntity> _lots = new List<LotEntity>();
                foreach (var pp in pps)
                {
                    var _lot = new LotEntity
                    {
                        ProcessType = "P",
                        LotName = "",
                        ProcessProgress = pp,
                    };
                    _lots.Add(_lot);
                }

                _db.Lots.AddRange(_lots);

                await Save(); 

                */

                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<WorkOrderReponse001>>> UpdateWorkOrder(WorkOrderRequest002 param)
        {
            try
            {
                var _workerOrder = await _db.WorkerOrders.Where(y => y.WorkerOrderId == param.workerOrderId).FirstOrDefaultAsync();

                _workerOrder.WorkOrderMemo = param.workOrderMemo;
                _workerOrder.WorkOrderDate = Convert.ToDateTime(param.workOrderDate);
                _workerOrder.RegisterDate = Convert.ToDateTime(param.workOrderDate);
                _workerOrder.Register = await _userManager.FindByIdAsync(param.registerId);
                _workerOrder.ProductWorkQuantity = param.productWorkQuantity;
                _workerOrder.WorkOrderSequence = param.workOrderSequence;

                _db.WorkerOrders.Update(_workerOrder);
                await Save();

                if (param.workerOrderProducePlans != null)
                {
                    List<WorkerOrderProducePlan> plans = new List<WorkerOrderProducePlan>();

                    foreach(var plan in param.workerOrderProducePlans)
                    {
                        var _workPlan = await _db.WorkerOrderProducePlans.Where(x => x.WorkerOrderProducePlanId == plan.workerOrderProducePlanId).FirstOrDefaultAsync();
                        _workPlan.Facility = await _db.Facilitys.Where(x => x.Id == plan.facilityId).FirstOrDefaultAsync();
                        _workPlan.Partner = await _db.Partners.Where(x => x.Id == plan.partnerId).FirstOrDefaultAsync();
                        _workPlan.Mold = await _db.Molds.Where(x => x.Id == plan.moldId).FirstOrDefaultAsync();
                        _workPlan.Register = await _userManager.FindByIdAsync(plan.workerId);
                        _workPlan.ProcessWorkQuantity = plan.processWorkQuantity;
                        _workPlan.ProcessElapsedTime = plan.processCheck == true ? 1 : 0;

                        _workPlan.ProcessCheck = plan.processCheck == true ? 1 : 0;
                        _workPlan.InOutSourcing = plan.isOutSourcing == true ? 1 : 0;


                        plans.Add(_workPlan);
                    }

                    _db.WorkerOrderProducePlans.UpdateRange(plans);
                    await Save();
                }

  



                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<WorkOrderReponse001>>> DeleteWorkOrder(WorkOrderRequest002 param)
        {
            try
            {


                foreach(var i in param.workerOrderIds)
                {
                    var _workerOrder = await _db.WorkerOrders.Where(y => y.WorkerOrderId == i).FirstOrDefaultAsync();
                    _workerOrder.IsDeleted = 1;
                    _db.WorkerOrders.Update(_workerOrder);

                    var _deleteWorkOrderProducePlans = await _db.WorkerOrderProducePlans.Where(x => x.WorkerOrder.WorkerOrderId == i).ToArrayAsync();

                    if (_deleteWorkOrderProducePlans != null)
                    {
                        foreach (var workOrderProducePlan in _deleteWorkOrderProducePlans)
                        {
                            workOrderProducePlan.IsDeleted = 1;
                            _db.WorkerOrderProducePlans.Update(workOrderProducePlan);

                            var _deleteProcessProgress = await _db.ProcessProgresses.Where(x => x.WorkerOrderProducePlanId == workOrderProducePlan.WorkerOrderProducePlanId).FirstOrDefaultAsync();
                            if(_deleteProcessProgress != null)
                            {
                                _deleteProcessProgress.IsDeleted = 1;
                                _db.ProcessProgresses.Update(_deleteProcessProgress);
                            }
                        }
                    }

                }


                await Save();

                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<WorkOrderReponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<WorkOrderResponse005>>> GetWorkOrderProducePlans(WorkOrderRequest005 req)
        {
            try
            {
                /*
                var query = $"EXEC [dbo].[작업지시관리_목록조회_공정목록] ";
                var paramerters = new List<string>();
                if (param.workerOrderId != 0) paramerters.Add($"@workerOrderId={param.workerOrderId}");
                query = query + string.Join(",", paramerters);


                var _items = await _db.작업지시관리_목록조회_공정목록
                    .FromSqlRaw(query).ToListAsync();
                */
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.WorkerOrder.WorkerOrderId == req.workerOrderId)
                    .Select(x => new WorkOrderResponse005
                    {
                        processOrder = x.ProducePlansProcess.ProductProcess.ProcessOrder,//공정순서
                        processCode = x.ProducePlansProcess.ProductProcess.Process.Code,//공정코드
                        processName = x.ProducePlansProcess.ProductProcess.Process.Name,//공정이름
                        isOutSourcing = x.InOutSourcing,//자체/외주
                        partnerName = x.Partner.Name,//거래처
                        facilitiesCode = x.Facility.Code,//설비코드
                        facilitiesName = x.Facility.Name,//설비이름
                        moldCode = x.Mold.Code,//금형코드
                        moldName = x.Mold.Name,//금형이름
                        workerName = x.Register.FullName,//작업자
                        processPlanQuantity = x.ProducePlansProcess.ProcessPlanQuantity,//계획수량
                        processPlanBacklog = x.ProducePlansProcess.ProcessPlanQuantity - x.ProcessWorkQuantity,//계획잔량=계획수량-지시수량
                        processWorkQuantity = x.ProcessWorkQuantity,//지시수량
                        productionQuanitity = x.ProcessProgress.ProductionQuantity,
                        productGoodQuantity = x.ProcessProgress.ProductionQuantity - _db.ProcessDefectives.Where(y => y.IsDeleted == 0).Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId).Select(y => y.DefectiveCount).Sum(),//양품수량
                        productDefectiveQuantity = _db.ProcessDefectives.Where(y=>y.IsDeleted == 0).Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId).Select(y=>y.DefectiveCount).Sum(), //_db.ProcessProgresses.Where(y => y.WorkerOrderProducePlanId == x.WorkOrderProducePlanId).FirstOrDefault().ProcessProgressId,//불량품 ProcessDefective 테이블 변경되어야 함
                        processElapsedTime = x.ProcessElapsedTime, //소요시간(분)
                        processCheck = x.ProcessCheck,//공정검사여부
                        processCheckResult = x.ProcessCheckResult,//공정검사결과
                    }).ToListAsync();


                var Res = new Response<IEnumerable<WorkOrderResponse005>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<WorkOrderResponse005>>()
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
