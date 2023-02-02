using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
//using WebBasedMES.Data.Models.FacilityManage;
using WebBasedMES.Data.Models.Lots;
using WebBasedMES.Services.Repositories.InAndOut;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Lot;

namespace WebBasedMES.Services.Repositories.Monitor
{
    public class MonitorManageRepository : IMonitorManageRepository
    {
        private readonly ApiDbContext _db;


        public MonitorManageRepository(ApiDbContext db)
        {
            _db = db;

        }

        public async Task<Response<IEnumerable<ProcessProgressRecordResponse>>> GetProcessProgressRecord()
        {
            try
            {

                DateTime startTime = Convert.ToDateTime(DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd"));
                //DateTime startTime = Convert.ToDateTime("2022-09-18");

                //현재 작업중인 공정이 있거나 또는 작업 시작일이 오늘날짜인 경우로 필터링.
                var res = await _db.WorkerOrders
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrderProducePlans
                        .Where(y=>y.IsDeleted == 0)
                        .Where(y=> y.ProcessProgress.WorkStartDateTime >= startTime)
                        .Count()>0  || 
                        
                        x.WorkerOrderProducePlans
                        .Where(y => y.IsDeleted == 0)
                        .Where(y => y.ProcessProgress.WorkStatus == "작업중" || y.ProcessProgress.WorkStatus == "작업완료")
                        .Count() > 0
                        )
                    .Select(x => new ProcessProgressRecordResponse
                    {
                        WorkOrderNo = x.WorkOrderNo,
                        ProductName = x.Product.Name,
                        PlanWorkQuantity = x.ProductWorkQuantity,


                        ProcessList = x.WorkerOrderProducePlans
                            .Select(y=> new ProcessProgressRecordList
                            {
                                WorkStartDateTime = y.ProcessProgress.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                                WorkEndDateTime = y.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                                
                                FacilityName = y.Facility.Name,
                                MoldName = y.Mold.Name,
                                ProcessStatus = y.ProcessProgress.WorkStatus,
                                
                                OrderQuantity = y.ProcessWorkQuantity,
                                
                                ProcessOrder = x.ProducePlansProduct == null? 
                                    _db.WorkerOrderWithoutPlans.Where(z=>z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(y=>y.ProductProcess.ProcessOrder).FirstOrDefault() : y.ProducePlansProcess.ProductProcess.ProcessOrder,
                                ProcessName = x.ProducePlansProduct == null ?
                                    _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault() : y.ProducePlansProcess.ProductProcess.Process.Name,
                                ProcessCode = x.ProducePlansProduct == null ?
                                    _db.WorkerOrderWithoutPlans.Where(z => z.WorkerOrderProducePlan.WorkerOrderProducePlanId == y.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault() : y.ProducePlansProcess.ProductProcess.Process.Code,
                                
                                WorkStatus = y.ProcessProgress.WorkStatus,

                                ProductionQuantity = y.ProcessProgress.ProductionQuantity,
                                DefectiveQuantity = _db.ProcessDefectives.Where(z=>z.ProcessProgress.ProcessProgressId == y.ProcessProgress.ProcessProgressId).Select(z=>z.DefectiveCount).Sum(),
                                GoodQuantity = y.ProcessProgress.ProductionQuantity - _db.ProcessDefectives.Where(z => z.ProcessProgress.ProcessProgressId == y.ProcessProgress.ProcessProgressId).Select(z => z.DefectiveCount).Sum(),
                                
                            }).OrderBy(y=>y.ProcessOrder).ToList(),


                    }).ToListAsync();

                var Res = new Response<IEnumerable<ProcessProgressRecordResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;

            }
            catch(Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessProgressRecordResponse>> ()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<IEnumerable<ProcessOperationRecordResponse>>> GetProcessOperationRecord()
        {
            try
            {

                DateTime today = Convert.ToDateTime(DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd"));


                var res = await _db.ProcessProgresses
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => (x.WorkOrderProducePlan.WorkerOrder.WorkOrderDate <= today.AddDays(1) && x.WorkOrderProducePlan.WorkerOrder.WorkOrderDate >= today)
                        || (x.WorkOrderProducePlan.WorkerOrder.WorkOrderDate < today && x.WorkStatus != "작업완료"))
                    .Select(x => new ProcessOperationRecordResponse
                    {
                        ProcessName = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct == null ?
                                    _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault() : x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name,
                        ProcessCode = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct == null ?
                                    _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault() : x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code,
                        
                        WorkOrderDate = x.WorkOrderProducePlan.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        FacilityName = x.WorkOrderProducePlan.Facility.Name,
                        MoldName = x.WorkOrderProducePlan.Mold.Name,

                        ProductionQuantity = x.ProductionQuantity,
                        DefectiveQuantity = _db.ProcessDefectives.Where(z => z.ProcessProgress.ProcessProgressId == x.ProcessProgressId).Select(z => z.DefectiveCount).Sum(),
                        GoodQuantity = x.ProductionQuantity - _db.ProcessDefectives.Where(z => z.ProcessProgress.ProcessProgressId == x.ProcessProgressId).Select(z => z.DefectiveCount).Sum(),
                        OrderQuantity = x.WorkOrderProducePlan.ProcessWorkQuantity,
                        ProductName = x.WorkOrderProducePlan.Product.Name,
                        WorkOrderNo = x.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,
                        WorkStatus = x.WorkStatus,
                        Ratio = x.WorkOrderProducePlan.ProcessWorkQuantity != 0 ?  (x.ProductionQuantity / x.WorkOrderProducePlan.ProcessWorkQuantity)*100 : 0
                    }).ToListAsync();


                if (res.Count > 0)
                {

                    var grouped = res.GroupBy(x => x.ProcessCode)
                        .Select(x => new ProcessOperationRecordResponse
                        {
                            ProcessCode = x.Key,
                            ProcessName = x.FirstOrDefault().ProcessName,

                            ProcessOperationWorks = x.Select(y => new ProcessOperationWork
                            {
                                WorkOrderDate = y.WorkOrderDate,
                                DefectiveQuantity = y.DefectiveQuantity,
                                FacilityName = y.FacilityName,
                                MoldName = y.MoldName,
                                ProductName = y.ProductName,

                                GoodQuantity = y.GoodQuantity,
                                OrderQuantity = y.OrderQuantity,
                                ProductionQuantity = y.ProductionQuantity,

                                Ratio = y.Ratio,
                                WorkOrderNo = y.WorkOrderNo,
                                WorkStatus = y.WorkStatus,

                            }).ToList()

                        }).ToList();

                    var Res = new Response<IEnumerable<ProcessOperationRecordResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = grouped,
                    };
                    return Res;
                }
                else
                {
                    List<ProcessOperationRecordResponse> temp = new List<ProcessOperationRecordResponse>();

                    var Res = new Response<IEnumerable<ProcessOperationRecordResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = temp
                    };
                    return Res;
                }




            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProcessOperationRecordResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }




        public async Task<Response<IEnumerable<ProductionRecordByContractResponse>>> GetProductionRecordByContract()
        {
            try
            {
                DateTime today = Convert.ToDateTime(DateTime.UtcNow.AddHours(9).ToString("yyyy-01-dd"));

                var res = await _db.WorkerOrders
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProducePlansProduct != null)
                    .Where(x => x.ProducePlansProduct.OrderProduct.Order.OrderDate >= today)
                    .Select(x => new ProductionRecordByContractResponse
                    {
                        OrderDate = x.ProducePlansProduct.OrderProduct.Order.OrderDate.ToString("yyyy-MM-dd"),
                        OrderNo = x.ProducePlansProduct.OrderProduct.Order.OrderNo,
                        PartnerCode = x.ProducePlansProduct.OrderProduct.Order.Partner.Code,
                        PartnerName = x.ProducePlansProduct.OrderProduct.Order.Partner.Name,

                        ShipmentDate = _db.OutOrderProducts
                            .Where(y=>y.OrderProduct.OrderProductId == x.ProducePlansProduct.OrderProduct.OrderProductId)
                            .Select(y=>y.OutOrder.ShipmentDate.ToString("yyyy-MM-dd")).FirstOrDefault(),
                        OrderProductQuantity = x.ProducePlansProduct.OrderProduct.ProductOrderCount,
                        OrderProductCount = x.ProducePlansProduct.OrderProduct.Order.OrderProducts.Where(y=>y.IsDeleted==0).Count(),
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        FinishProductCount = x.WorkerOrderProducePlans.OrderByDescending(y => y.WorkerOrderProducePlanId).Select(y=>y.ProcessProgress.ProductionQuantity).FirstOrDefault(),
                    }).ToListAsync();


                var grouped = res.GroupBy(x => x.OrderNo)
                    .Select(x => new ProductionRecordByContractResponse
                    {
                        OrderNo = x.Key,
                        OrderDate = x.FirstOrDefault().OrderDate,
                        PartnerCode = x.FirstOrDefault().PartnerCode,
                        PartnerName = x.FirstOrDefault().PartnerName,
                        ShipmentDate = x.FirstOrDefault().ShipmentDate,
                        OrderProductCount = x.Count(),
                        ProductionRecordByContractProducts = x
                            .Select(y=>new ProductionRecordByContractProduct
                            {
                                ProductCode = y.ProductCode,
                                ProductName = y.ProductName,
                                FinishRatio = y.FinishProductCount / x.FirstOrDefault().OrderProductQuantity * 100
                            }).ToList().GroupBy(x=>x.ProductCode).Select(z=> new ProductionRecordByContractProduct
                            {
                                ProductCode = z.Key,
                                ProductName = z.First().ProductName,
                                FinishRatio = z.First().FinishRatio
                            })


                    }).ToList();

                var Res = new Response<IEnumerable<ProductionRecordByContractResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = grouped,
                };
                return Res;



            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductionRecordByContractResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<IEnumerable<ProductionRecordByPlanResponse>>> GetProductionRecordByPlan()
        {
            try
            {
                DateTime today = Convert.ToDateTime(DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd"));


                var res = await _db.WorkerOrders
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProducePlansProduct != null)
                    .Where(x => (x.ProducePlansProduct.ProducePlan.ProductionPlanStartDate <= today && x.ProducePlansProduct.ProducePlan.ProductionPlanEndDate >= today) || x.ProducePlansProduct.ProducePlan.ProductionPlanStatus == "계획등록" || x.ProducePlansProduct.ProducePlan.ProductionPlanStatus == "작업대기")
                    .Select(x => new ProductionRecordByPlanResponse
                    {
                        PlanStartDate = x.ProducePlansProduct.ProducePlan.ProductionPlanStartDate.ToString("yyyy-MM-dd"),
                        PlanEndDate = x.ProducePlansProduct.ProducePlan.ProductionPlanEndDate.ToString("yyyy-MM-dd"),
                        PlanNo = x.ProducePlansProduct.ProducePlan.ProductionPlanNo,
                        PlanProductCount = x.ProducePlansProduct.ProducePlan.ProducePlanProducts.Where(y=>y.IsDeleted == 0).Count(),
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        ProductionQuantity = x.WorkerOrderProducePlans.OrderByDescending(y => y.WorkerOrderProducePlanId).Select(y => y.ProcessProgress.ProductionQuantity).FirstOrDefault(),
                        ProductionPlanQuantity = x.ProducePlansProduct.ProductPlanQuantity,

                    }).ToListAsync();


                var grouped = res.GroupBy(x => x.PlanNo)
                    .Select(x => new ProductionRecordByPlanResponse
                    {
                        PlanNo = x.Key,
                        PlanEndDate = x.FirstOrDefault().PlanEndDate,
                        PlanStartDate = x.FirstOrDefault().PlanStartDate,
                        PlanProductCount = x.Count(),
                        ProductionRecordByPlanProducts = x.Select(y=>new ProductionRecordByContractProduct
                        {
                            ProductCode = y.ProductCode,
                            ProductName = y.ProductName,
                            FinishRatio = y.ProductionQuantity / x.FirstOrDefault().ProductionPlanQuantity * 100
                        }).ToList()

                    }).ToList();

                var Res = new Response<IEnumerable<ProductionRecordByPlanResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = grouped,
                };
                return Res;



            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductionRecordByPlanResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<IEnumerable<FacilityManageRecordResponse>>> GetFacilityManageRecord()
        {
            try
            {
                var res = await _db.Facilitys
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.PeriodicalInspection == true)
                    
                    .Select(x => new FacilityManageRecordResponse
                    {
                        FacilityCode = x.Code,
                        FacilityName = x.Name,
                        FacilityClassification = x.CommonCode.Name,
                        CurrentInspectionDate = _db.FacilityInspections
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.Facility == x)
                            .OrderByDescending(y=>y.RegisterDate)
                            .Select(y=>y.RegisterDate.ToString("yyyy-MM-dd"))
                            .FirstOrDefault(),
                        NextInspectionDate = _db.FacilityInspections
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.Facility == x)
                            .OrderByDescending(y => y.RegisterDate)
                            .Select(y => y.RegisterDate.ToString("yyyy-MM-dd"))
                            .FirstOrDefault() != null?
                        _db.FacilityInspections
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.Facility == x)
                            .OrderByDescending(y => y.RegisterDate)
                            .Select(y => y.RegisterDate.AddMonths(1).ToString("yyyy-MM-dd") + " ( D-"+ (y.RegisterDate.AddMonths(1) - DateTime.UtcNow.AddHours(9)).Days.ToString() + " )")
                            .FirstOrDefault() :"",
                        InspectionPeriod = _db.CommonCodes.Where(z=>z.Id == _db.PreventiveMaintenanceFacilitys.Where(y => y.Facility == x).Select(y => y.RegularInspectionPeriod).FirstOrDefault()).Select(y=>y.Name).FirstOrDefault(),
                        InspectionAlert = _db.FacilityInspections
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.Facility == x)
                            .OrderByDescending(y => y.RegisterDate)
                            .Select(y => y.RegisterDate.ToString("yyyy-MM-dd"))
                            .FirstOrDefault() != null ? _db.FacilityInspections
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.Facility == x)
                            .OrderByDescending(y => y.RegisterDate)
                            .Select(y => (y.RegisterDate.AddMonths(1) - DateTime.UtcNow.AddHours(9)).Days <=7 ? true : false )
                            .FirstOrDefault() : false,

                    }).ToListAsync();

                    
                var Res = new Response<IEnumerable<FacilityManageRecordResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityManageRecordResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }

        }

        public async Task<Response<IEnumerable<MoldManageRecordInspectionResponse>>> GetMoldManageRecordInspection()
        {
            try
            {
                var res = await _db.Molds
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.RegularInspection == true)
                    .Select(x => new MoldManageRecordInspectionResponse
                    {

                        MoldCode = x.Code,
                        MoldName = x.Name,
                        MoldClassification = x.CommonCode.Name,
                        CurrentInspectionDate = _db.MoldInspections
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.RegisterDate)
                            .Select(y => y.RegisterDate.ToString("yyyy-MM-dd"))
                            .FirstOrDefault(),
                        NextInspectionDate = _db.MoldInspections
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.RegisterDate)
                            .Select(y => y.RegisterDate.ToString("yyyy-MM-dd"))
                            .FirstOrDefault() != null ?
                        _db.MoldInspections
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.RegisterDate)
                            .Select(y => y.RegisterDate.AddMonths(1).ToString("yyyy-MM-dd") + " ( D-" + (y.RegisterDate.AddMonths(1) - DateTime.UtcNow.AddHours(9)).Days.ToString() + " )")
                            .FirstOrDefault() : "",
                        InspectionPeriod = _db.CommonCodes.Where(z => z.Id == _db.PreventiveMaintenanceMolds.Where(y => y.Mold == x).Select(y => y.RegularInspectionPeriod).FirstOrDefault()).Select(y => y.Name).FirstOrDefault(),
                        InspectionAlert = _db.MoldInspections
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.RegisterDate)
                            .Select(y => y.RegisterDate.ToString("yyyy-MM-dd"))
                            .FirstOrDefault() != null ? _db.MoldInspections
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.RegisterDate)
                            .Select(y => (y.RegisterDate.AddMonths(1) - DateTime.UtcNow.AddHours(9)).Days <= 7 ? true : false)
                            .FirstOrDefault() : false,

                        GuaranteeCount = x.GuranteeCount,
                        InspectionCount = 1000,
                        AfterInspectionCount = 0 + 1000,
                        CurrentCount = 0,

                    }).ToListAsync();


                var Res = new Response<IEnumerable<MoldManageRecordInspectionResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldManageRecordInspectionResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }

        }

        public async Task<Response<IEnumerable<MoldManageRecordCleaningResponse>>> GetMoldManageRecordWash()
        {
            try
            {
                var res = await _db.Molds
                    .Where(x => x.IsDeleted == false)
                   // .Where(x => x.RegularInspection == true)
                    .Select(x => new MoldManageRecordCleaningResponse
                    {
                        MoldCode = x.Code,
                        MoldName = x.Name,
                        MoldClassification = x.CommonCode.Name,
                        CurrentWashDate = _db.MoldCleanings
                            .Where(y => y.IsDeleted == false)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.CleaningDate)
                            .Select(y => y.CleaningDate.ToString("yyyy-MM-dd"))
                            .FirstOrDefault(),
                        NextWashDate = _db.MoldCleanings
                            .Where(y => y.IsDeleted == false)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.CleaningDate)
                            .Select(y => y.CleaningDate.ToString("yyyy-MM-dd"))
                            .FirstOrDefault() != null ?
                        _db.MoldCleanings
                            .Where(y => y.IsDeleted == false)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.CleaningDate)
                            .Select(y => y.CleaningDate.AddMonths(1).ToString("yyyy-MM-dd") + " ( D-" + (y.CleaningDate.AddMonths(1) - DateTime.UtcNow.AddHours(9)).Days.ToString() + " )")
                            .FirstOrDefault() : "",
                        WashPeriod = _db.CommonCodes.Where(z => z.Id == _db.PreventiveMaintenanceMolds.Where(y => y.Mold == x).Select(y => y.MoldCleaningPeriod).FirstOrDefault()).Select(y => y.Name).FirstOrDefault(),
                        WashAlert = _db.MoldCleanings
                            .Where(y => y.IsDeleted == false)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.CleaningDate)
                            .Select(y => y.CleaningDate.ToString("yyyy-MM-dd"))
                            .FirstOrDefault() != null ? _db.MoldCleanings
                            .Where(y => y.IsDeleted == false)
                            .Where(y => y.Mold == x)
                            .OrderByDescending(y => y.CleaningDate)
                            .Select(y => (y.CleaningDate.AddMonths(1) - DateTime.UtcNow.AddHours(9)).Days <= 7 ? true : false)
                            .FirstOrDefault() : false,

                        GuaranteeCount = x.GuranteeCount,
                        WashCount = 1000,
                        AfterWashCount = 0 + 1000,
                        CurrentCount = 0,

                    }).ToListAsync();


                var Res = new Response<IEnumerable<MoldManageRecordCleaningResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldManageRecordCleaningResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<IEnumerable<FacilityStatus>>> GetFacilityTempRecord()
        {
            try
            {

                var facilities = await _db.Facilitys
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFiles)
                    .Where(x => x.IsUsing == true)
                    .Where(x => x.Uid != "")
                    .Where(x => x.IsDeleted == false).ToListAsync();




                int facilityCnt = facilities.Count();

                List<FacilityStatus> facilityStatuses = new List<FacilityStatus>();

                DateTime now = DateTime.UtcNow.AddHours(9);

                foreach (var facility in facilities)
                {
                    var facilityStatus = new FacilityStatus();
                    facilityStatus.FacilityCode = facility.Code;
                    facilityStatus.FacilityName = facility.Name;
                    facilityStatus.MaxAmp = Convert.ToInt32(facility.MaxCurrent == "" ? "0" : facility.MaxCurrent);
                    facilityStatus.MaxTon = Convert.ToInt32(facility.MaxTon == "" ? "0" : facility.MaxTon);
                    facilityStatus.Uid = facility.Uid;

                    var facilityStatusLog = await _db.FacilityStatus.Where(x => x.UID == facility.Uid).FirstOrDefaultAsync();


                    if (facilityStatusLog != null)
                    {
                        if ((now - facilityStatusLog.UpdateTime).TotalMinutes > 3)
                        {
                            facilityStatus.Production_Current = Convert.ToInt32(facilityStatusLog.CW010, 16).ToString();
                            facilityStatus.Production_Target = Convert.ToInt32(facilityStatusLog.DW4020, 16).ToString();
                            facilityStatus.Production_Total = Convert.ToInt32(facilityStatusLog.CW012, 16).ToString();
                            facilityStatus.Production_Mold = Convert.ToInt32(facilityStatusLog.CW030, 16).ToString();
                            facilityStatus.OnOffStatus = Convert.ToInt32(facilityStatusLog.FW004, 16) % 2 == 1 ? true : false;
                            facilityStatus.MotorSpm = Convert.ToInt32(facilityStatusLog.LW058, 16).ToString();
                            facilityStatus.TonValue = Convert.ToInt32(facilityStatusLog.LW056, 16);
                            facilityStatus.Amp_Slide = Convert.ToInt32(facilityStatusLog.LW060, 16).ToString();
                            facilityStatus.Amp_MainMotor = Convert.ToInt32(facilityStatusLog.LW059, 16);
                            facilityStatus.Slide_Value = (Convert.ToInt32(facilityStatusLog.LW000, 16) / 10).ToString("0.0");
                            facilityStatus.Slide_UL = (Convert.ToInt32(facilityStatusLog.DW4101, 16)).ToString("0.0");
                            facilityStatus.Slide_LL = (Convert.ToInt32(facilityStatusLog.DW4102, 16)).ToString("0.0");
                            facilityStatus.StartStop = Convert.ToInt32(facilityStatusLog.MW900, 16) % 2 == 1 ? true : false;
                            facilityStatus.ErrorCode = "98";

                            facilityStatus.Temp = (Convert.ToInt32(facilityStatusLog.LW000, 16) / 10).ToString("0.0") + "ºC";
                            facilityStatus.Humid = Convert.ToInt32(facilityStatusLog.DW0020, 16).ToString() + "%";
                            



                            facilityStatus.ErrorDescription = "PLC 통신 두절(" + facility.Uid + ")";

                            byte[] temp = new byte[10];
                            temp[0] = facilityStatusLog.DW500 != null ? Convert.ToByte("0x" + facilityStatusLog.DW500.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[1] = facilityStatusLog.DW500 != null ? Convert.ToByte("0x" + facilityStatusLog.DW500.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[2] = facilityStatusLog.DW501 != null ? Convert.ToByte("0x" + facilityStatusLog.DW501.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[3] = facilityStatusLog.DW501 != null ? Convert.ToByte("0x" + facilityStatusLog.DW501.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[4] = facilityStatusLog.DW502 != null ? Convert.ToByte("0x" + facilityStatusLog.DW502.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[5] = facilityStatusLog.DW502 != null ? Convert.ToByte("0x" + facilityStatusLog.DW502.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[6] = facilityStatusLog.DW503 != null ? Convert.ToByte("0x" + facilityStatusLog.DW503.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[7] = facilityStatusLog.DW503 != null ? Convert.ToByte("0x" + facilityStatusLog.DW503.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[8] = facilityStatusLog.DW504 != null ? Convert.ToByte("0x" + facilityStatusLog.DW504.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[9] = facilityStatusLog.DW504 != null ? Convert.ToByte("0x" + facilityStatusLog.DW504.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);

                            facilityStatus.MoldName = Encoding.UTF8.GetString(temp);
                            facilityStatus.UpdateDate = facilityStatusLog.UpdateTime.ToString("yyyy-MM-dd");
                            
                            facilityStatus.StatusColor = "Red";
                        }
                        else
                        {
                            facilityStatus.Production_Current = Convert.ToInt32(facilityStatusLog.CW010, 16).ToString();
                            facilityStatus.Production_Target = Convert.ToInt32(facilityStatusLog.DW4020, 16).ToString();
                            facilityStatus.Production_Total = Convert.ToInt32(facilityStatusLog.CW012, 16).ToString();
                            facilityStatus.Production_Mold = Convert.ToInt32(facilityStatusLog.CW030, 16).ToString();
                            facilityStatus.OnOffStatus = Convert.ToInt32(facilityStatusLog.FW004, 16) % 2 == 1 ? true : false;
                            facilityStatus.MotorSpm = Convert.ToInt32(facilityStatusLog.LW058, 16).ToString();
                            facilityStatus.TonValue = Convert.ToInt32(facilityStatusLog.LW056, 16);
                            facilityStatus.Amp_Slide = Convert.ToInt32(facilityStatusLog.LW060, 16).ToString();
                            facilityStatus.Amp_MainMotor = Convert.ToInt32(facilityStatusLog.LW059, 16);
                            facilityStatus.Slide_Value = (Convert.ToInt32(facilityStatusLog.LW000, 16) / 10).ToString("0.0");
                            facilityStatus.Slide_UL = (Convert.ToInt32(facilityStatusLog.DW4101, 16)).ToString("0.0");
                            facilityStatus.Slide_LL = (Convert.ToInt32(facilityStatusLog.DW4102, 16)).ToString("0.0");
                            facilityStatus.StartStop = Convert.ToInt32(facilityStatusLog.MW900, 16) % 2 == 1 ? true : false;
                            facilityStatus.ErrorCode = Convert.ToInt32(facilityStatusLog.FW050, 16).ToString();

                            facilityStatus.Temp = (Convert.ToInt32(facilityStatusLog.LW000, 16) / 10).ToString("0.0") + "ºC";
                            facilityStatus.Humid = Convert.ToInt32(facilityStatusLog.DW0020, 16).ToString() + "%";


                            facilityStatus.ErrorDescription = _db.FacilityErrorCode.Where(x => x.ErrorCode == (Convert.ToInt32(facilityStatusLog.FW050.Substring(0, 4), 16)).ToString()).Select(x => x.Description).FirstOrDefault();


                            byte[] temp = new byte[10];
                            temp[0] = facilityStatusLog.DW500 != null ? Convert.ToByte("0x" + facilityStatusLog.DW500.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[1] = facilityStatusLog.DW500 != null ? Convert.ToByte("0x" + facilityStatusLog.DW500.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[2] = facilityStatusLog.DW501 != null ? Convert.ToByte("0x" + facilityStatusLog.DW501.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[3] = facilityStatusLog.DW501 != null ? Convert.ToByte("0x" + facilityStatusLog.DW501.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[4] = facilityStatusLog.DW502 != null ? Convert.ToByte("0x" + facilityStatusLog.DW502.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[5] = facilityStatusLog.DW502 != null ? Convert.ToByte("0x" + facilityStatusLog.DW502.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[6] = facilityStatusLog.DW503 != null ? Convert.ToByte("0x" + facilityStatusLog.DW503.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[7] = facilityStatusLog.DW503 != null ? Convert.ToByte("0x" + facilityStatusLog.DW503.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[8] = facilityStatusLog.DW504 != null ? Convert.ToByte("0x" + facilityStatusLog.DW504.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[9] = facilityStatusLog.DW504 != null ? Convert.ToByte("0x" + facilityStatusLog.DW504.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);

                            facilityStatus.MoldName = Encoding.UTF8.GetString(temp);
                            facilityStatus.UpdateDate = facilityStatusLog.UpdateTime.ToString("yyyy-MM-dd");

                            facilityStatus.StatusColor = "Green";

                            if (Convert.ToInt32(facilityStatusLog.DW0020, 16) >= 80)
                            {
                                facilityStatus.StatusColor = "Yellow";
                            }

                            var criteria = _db.FacilityControls
                                .Where(x => x.Facility.Code == facilityStatus.FacilityCode)
                                .Select(x => new
                                {
                                    UL = x.BottomDeadPointUL,
                                    DL = x.BottomDeadPointDL,
                                }).FirstOrDefault();

                            if (criteria != null)
                            {
                                if ((Convert.ToInt32(facilityStatusLog.LW000, 16) / 10) >= criteria.UL || (Convert.ToInt32(facilityStatusLog.LW000, 16) / 10) <= criteria.DL)
                                {
                                    facilityStatus.StatusColor = "Yellow";
                                }
                            }
                        }
                    }
                    else
                    {
                        facilityStatus.Production_Current = "";
                        facilityStatus.Production_Target = "";
                        facilityStatus.Production_Total = "";
                        facilityStatus.Production_Mold = "";
                        facilityStatus.OnOffStatus = false;
                        facilityStatus.MotorSpm = "";
                        facilityStatus.TonValue = 0;
                        facilityStatus.Amp_Slide = "";
                        facilityStatus.Amp_MainMotor = 0;
                        facilityStatus.Slide_Value = "";
                        facilityStatus.Slide_UL = "";
                        facilityStatus.Slide_LL = "";
                        facilityStatus.StartStop = false;
                        facilityStatus.ErrorCode = "99";
                        facilityStatus.Temp = _db.FacilityBaseInfos.Where(x=>x.IsDeleted == 0).Where(x => x.Facility.Code == facilityStatus.FacilityCode).Select(x => x.BottomDeadPoint).FirstOrDefault().ToString("0.0") + "ºC"; ;
                        facilityStatus.Humid = _db.FacilityBaseInfos.Where(x => x.IsDeleted == 0).Where(x => x.Facility.Code == facilityStatus.FacilityCode).Select(x => x.Slide).FirstOrDefault().ToString("0.0") + "ºC"; ;
                       // facilityStatus. = "-";
                        facilityStatus.ErrorDescription = "설비 데이터 없음(" + facility.Uid + ")";
                        facilityStatus.StatusColor = "Red";

                        facilityStatus.MoldName = "";
                        facilityStatus.UpdateDate = "";


                    }

                    facilityStatuses.Add(facilityStatus);
                }



                var Res = new Response<IEnumerable<FacilityStatus>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = facilityStatuses
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityStatus>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }

        }

        public async Task<Response<IEnumerable<FacilityStatus>>> GetFacilityOperationRecord()
        {
            try
            {

                var facilities = await _db.Facilitys
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFiles)
                    .Where(x => x.CommonCode.Name == "프레스")
                    .Where(x => x.IsUsing == true)
                    .Where(x => x.Uid != "")
                    .Where(x => x.IsDeleted == false).ToListAsync();

                int facilityCnt = facilities.Count();

                List<FacilityStatus> facilityStatuses = new List<FacilityStatus>();

                DateTime now = DateTime.UtcNow.AddHours(9);

                foreach (var facility in facilities)
                {
                    var facilityStatus = new FacilityStatus();
                    facilityStatus.FacilityCode = facility.Code;
                    facilityStatus.FacilityName = facility.Name;
                    facilityStatus.MaxAmp = Convert.ToInt32(facility.MaxCurrent == "" ? "0" : facility.MaxCurrent);
                    facilityStatus.MaxTon = Convert.ToInt32(facility.MaxTon == "" ? "0" : facility.MaxTon);
                    facilityStatus.Uid = facility.Uid;

                    var facilityStatusLog = await _db.FacilityStatus.Where(x => x.UID == facility.Uid).FirstOrDefaultAsync();


                    if (facilityStatusLog != null)
                    {
                        if ((now - facilityStatusLog.UpdateTime).TotalMinutes > 3)
                        {
                            facilityStatus.Production_Current = Convert.ToInt32(facilityStatusLog.CW010, 16).ToString();
                            facilityStatus.Production_Target = Convert.ToInt32(facilityStatusLog.DW4020, 16).ToString();
                            facilityStatus.Production_Total = Convert.ToInt32(facilityStatusLog.CW012, 16).ToString();
                            facilityStatus.Production_Mold = Convert.ToInt32(facilityStatusLog.CW030, 16).ToString();
                            facilityStatus.OnOffStatus = Convert.ToInt32(facilityStatusLog.FW004, 16) % 2 == 1 ? true : false;
                            facilityStatus.MotorSpm = Convert.ToInt32(facilityStatusLog.LW058, 16).ToString();
                            facilityStatus.TonValue = Convert.ToInt32(facilityStatusLog.LW056, 16);
                            facilityStatus.Amp_Slide = Convert.ToInt32(facilityStatusLog.LW060, 16).ToString();
                            facilityStatus.Amp_MainMotor = Convert.ToInt32(facilityStatusLog.LW059, 16);
                            facilityStatus.Slide_Value = (Convert.ToInt32(facilityStatusLog.LW000, 16) / 10).ToString("0.0");
                            facilityStatus.Slide_UL = (Convert.ToInt32(facilityStatusLog.DW4101, 16)).ToString("0.0");
                            facilityStatus.Slide_LL = (Convert.ToInt32(facilityStatusLog.DW4102, 16)).ToString("0.0");
                            facilityStatus.StartStop = Convert.ToInt32(facilityStatusLog.MW900, 16) % 2 == 1 ? true : false;
                            facilityStatus.ErrorCode = "98";

                            facilityStatus.ErrorDescription = "PLC 통신 두절(" + facility.Uid + ")";

                            byte[] temp = new byte[10];
                            temp[0] = facilityStatusLog.DW500 != null ? Convert.ToByte("0x" + facilityStatusLog.DW500.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[1] = facilityStatusLog.DW500 != null ? Convert.ToByte("0x" + facilityStatusLog.DW500.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[2] = facilityStatusLog.DW501 != null ? Convert.ToByte("0x" + facilityStatusLog.DW501.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[3] = facilityStatusLog.DW501 != null ? Convert.ToByte("0x" + facilityStatusLog.DW501.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[4] = facilityStatusLog.DW502 != null ? Convert.ToByte("0x" + facilityStatusLog.DW502.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[5] = facilityStatusLog.DW502 != null ? Convert.ToByte("0x" + facilityStatusLog.DW502.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[6] = facilityStatusLog.DW503 != null ? Convert.ToByte("0x" + facilityStatusLog.DW503.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[7] = facilityStatusLog.DW503 != null ? Convert.ToByte("0x" + facilityStatusLog.DW503.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[8] = facilityStatusLog.DW504 != null ? Convert.ToByte("0x" + facilityStatusLog.DW504.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[9] = facilityStatusLog.DW504 != null ? Convert.ToByte("0x" + facilityStatusLog.DW504.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);

                            facilityStatus.MoldName = Encoding.UTF8.GetString(temp);
                            facilityStatus.UpdateDate = facilityStatusLog.UpdateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            facilityStatus.Production_Current = Convert.ToInt32(facilityStatusLog.CW010, 16).ToString();
                            facilityStatus.Production_Target = Convert.ToInt32(facilityStatusLog.DW4020, 16).ToString();
                            facilityStatus.Production_Total = Convert.ToInt32(facilityStatusLog.CW012, 16).ToString();
                            facilityStatus.Production_Mold = Convert.ToInt32(facilityStatusLog.CW030, 16).ToString();
                            facilityStatus.OnOffStatus = Convert.ToInt32(facilityStatusLog.FW004, 16) % 2 == 1 ? true : false;
                            facilityStatus.MotorSpm = Convert.ToInt32(facilityStatusLog.LW058, 16).ToString();
                            facilityStatus.TonValue = Convert.ToInt32(facilityStatusLog.LW056, 16);
                            facilityStatus.Amp_Slide = Convert.ToInt32(facilityStatusLog.LW060, 16).ToString();
                            facilityStatus.Amp_MainMotor = Convert.ToInt32(facilityStatusLog.LW059, 16);
                            facilityStatus.Slide_Value = (Convert.ToInt32(facilityStatusLog.LW000, 16) / 10).ToString("0.0");
                            facilityStatus.Slide_UL = (Convert.ToInt32(facilityStatusLog.DW4101, 16)).ToString("0.0");
                            facilityStatus.Slide_LL = (Convert.ToInt32(facilityStatusLog.DW4102, 16)).ToString("0.0");
                            facilityStatus.StartStop = Convert.ToInt32(facilityStatusLog.MW900, 16) % 2 == 1 ? true : false;
                            facilityStatus.ErrorCode = Convert.ToInt32(facilityStatusLog.FW050, 16).ToString();

                            facilityStatus.ErrorDescription = _db.FacilityErrorCode.Where(x => x.ErrorCode == (Convert.ToInt32(facilityStatusLog.FW050.Substring(0, 4), 16)).ToString()).Select(x => x.Description).FirstOrDefault();


                            byte[] temp = new byte[10];
                            temp[0] = facilityStatusLog.DW500 != null ? Convert.ToByte("0x" + facilityStatusLog.DW500.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[1] = facilityStatusLog.DW500 != null ? Convert.ToByte("0x" + facilityStatusLog.DW500.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[2] = facilityStatusLog.DW501 != null ? Convert.ToByte("0x" + facilityStatusLog.DW501.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[3] = facilityStatusLog.DW501 != null ? Convert.ToByte("0x" + facilityStatusLog.DW501.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[4] = facilityStatusLog.DW502 != null ? Convert.ToByte("0x" + facilityStatusLog.DW502.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[5] = facilityStatusLog.DW502 != null ? Convert.ToByte("0x" + facilityStatusLog.DW502.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[6] = facilityStatusLog.DW503 != null ? Convert.ToByte("0x" + facilityStatusLog.DW503.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[7] = facilityStatusLog.DW503 != null ? Convert.ToByte("0x" + facilityStatusLog.DW503.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[8] = facilityStatusLog.DW504 != null ? Convert.ToByte("0x" + facilityStatusLog.DW504.Substring(2, 2), 16) : Convert.ToByte("0x00", 16);
                            temp[9] = facilityStatusLog.DW504 != null ? Convert.ToByte("0x" + facilityStatusLog.DW504.Substring(0, 2), 16) : Convert.ToByte("0x00", 16);

                            facilityStatus.MoldName = Encoding.UTF8.GetString(temp);
                            facilityStatus.UpdateDate = facilityStatusLog.UpdateTime.ToString("yyyy-MM-dd");
                        }

                    }
                    else
                    {
                        facilityStatus.Production_Current = "";
                        facilityStatus.Production_Target = "";
                        facilityStatus.Production_Total = "";
                        facilityStatus.Production_Mold = "";
                        facilityStatus.OnOffStatus = false;
                        facilityStatus.MotorSpm = "";
                        facilityStatus.TonValue = 0;
                        facilityStatus.Amp_Slide = "";
                        facilityStatus.Amp_MainMotor = 0;
                        facilityStatus.Slide_Value = "";
                        facilityStatus.Slide_UL = "";
                        facilityStatus.Slide_LL = "";
                        facilityStatus.StartStop = false;
                        facilityStatus.ErrorCode = "99";

                        facilityStatus.ErrorDescription = "설비 데이터 없음(" + facility.Uid + ")";

                        facilityStatus.MoldName = "";
                        facilityStatus.UpdateDate = "";
                    }

                    facilityStatuses.Add(facilityStatus);
                }



                var Res = new Response<IEnumerable<FacilityStatus>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = facilityStatuses
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityStatus>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<bool>> GetFacilityOperationRecordSave()
        {
            try
            {
                DateTime now = DateTime.UtcNow.AddHours(9);

                var copyData = await _db.FacilityStatus.Select(x=>new WebBasedMES.Data.Models.FacilityManage.FacilityStatusLog
                {
                    DW2921 = x.DW2921,
                    DW4000 = x.DW4000,
                    CreateTime = now,
                    DW4020 = x.DW4020,
                    DW4101 = x.DW4101,
                    DW4102 = x.DW4101,
                    CW010 = x.CW010,
                    DW500 = x.DW500,
                    DW503 = x.DW503,
                    DW501 = x.DW501,
                    DW502= x.DW502,
                    DW504 = x.DW504,
                    UID = x.UID,
                    CW012 = x.CW012,
                    CW030 = x.CW030,
                    FW004 = x.FW004,
                    FW050 = x.FW004,
                    LW000 = x.LW000,
                    LW056 = x.LW056,
                    LW058 = x.LW058,
                    LW059 = x.LW059,
                    LW060 = x.LW060,
                    MW000 = x.MW000,
                    MW090 = x.MW090,
                    MW900 = x.MW900,
                    PW000 = x.PW000,
                    PW010 = x.PW010,
                    PW020 = x.PW020,
                    PW030 = x.PW030,


                }).ToListAsync();

                var result = _db.FacilityStatusLog.AddRangeAsync(copyData);

                await Save();




                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage ="",
                    Data = true,
                };
                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = false,
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
