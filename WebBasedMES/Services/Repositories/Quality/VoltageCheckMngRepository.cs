using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.FacilityManage;
using WebBasedMES.Data.Models.Lots;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public class VoltageCheckMngRepository : IVoltageCheckMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public VoltageCheckMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

        public async Task<Response<IEnumerable<VoltageCheckRes001>>> QualityVoltageCheckManage(VoltageCheckReq001 req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Include(x=>x.ProcessProgress)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(req.workOrderStartDate))
                    .Where(x => x.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(req.workOrderEndDate))
                    .Where(x => x.Facility != null)
                    .Where(x => x.Facility.CommonCode.Name == "프레스")
                    .Where(x => x.Facility.Uid != "")
                    .Where(x => x.Facility.IsUsing == true)
                    .Where(x => req.facilityId == 0 ? true : x.Facility.Id == req.facilityId)
                    .Where(x => req.moldId == 0 ? true : x.Mold.Id == req.moldId)
                    .Where(x => x.ProcessProgress.WorkStatus != "작업대기")
                    .Select(x => new VoltageCheckRes001
                    {
                        workOrderNo = x.WorkerOrder.WorkOrderNo,
                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        

                        processId = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),

                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        productLOT = _db.Lots
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=> y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                            .Where(y=>y.ProcessType == "P")
                            .Select(y=>y.LotName).FirstOrDefault(),
                         

                        slideElectricCurrent = "",
                        spm = "",

                        
                        facilitiesCode = x.Facility !=null ? x.Facility.Code : "",
                        facilitiesName = x.Facility != null ? x.Facility.Name : "",
                        facilitiesUID = x.Facility != null ? x.Facility.Uid:"",
                        moldCode = x.Mold !=null ? x.Mold.Code : "",
                        moldName = x.Mold!=null? x.Mold.Name:"" ,
                        motorElectricCurrent = "",
                        
                        dateFlag = x.WorkerOrder.WorkOrderDate,
                        WorkStartDateTime = x.ProcessProgress != null? x.ProcessProgress.WorkStartDateTime : DateTime.Now
                    })
                    .OrderByDescending(x=>x.dateFlag)
                    .ToListAsync();

                var res2 = res.Where(x => req.processId == 0 ? true : x.processId == req.processId);
                var res3 = res2.Where(x => req.productLOT == "" ? true : x.productLOT.Contains(req.productLOT));

                foreach(var x in res3)
                {
                    if(x.facilitiesCode != null && x.facilitiesCode != "")
                    {
                        var logData = await _db.FacilityStatusLog
                            .Where(m=>m.UID == x.facilitiesUID)
                            .Where(m => m.CreateTime >= x.WorkStartDateTime)
                            .OrderBy(m => m.CreateTime)
                            .FirstOrDefaultAsync();

                        if(logData != null)
                        {
                            x.slideElectricCurrent = Convert.ToInt32(logData.LW060, 16).ToString();
                            x.spm = Convert.ToInt32(logData.LW058, 16).ToString();
                            x.motorElectricCurrent = Convert.ToInt32(logData.LW059, 16).ToString();
                        }
                        else
                        {
                            x.slideElectricCurrent = "no data";
                            x.spm = "no data";
                            x.motorElectricCurrent = "no data";
                        }
                    }
                }

                
                var Res = new Response<IEnumerable<VoltageCheckRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res3
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<VoltageCheckRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
       
        }
        public async Task<Response<IEnumerable<BottomDeadPointResponse>>> QualityBottomDeadPointManage(VoltageCheckReq001 req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(req.workOrderStartDate))
                    .Where(x => x.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(req.workOrderEndDate))
                    .Where(x => x.Facility != null)
                    .Where(x => req.facilityId == 0 ? true : x.Facility.Id == req.facilityId)
                    .Where(x => req.moldId == 0 ? true : x.Mold.Id == req.moldId)
                    .Where(x => x.ProcessProgress.WorkStatus != "작업대기")
                    .Select(x => new BottomDeadPointResponse
                    {
                        workOrderNo = x.WorkerOrder.WorkOrderNo,
                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),


                        processId = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),

                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        productLOT = _db.Lots
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                            .Where(y => y.ProcessType == "P")
                            .Select(y => y.LotName).FirstOrDefault(),
                        
                        productCode = x.Product.Code,
                        productName = x.Product.Name,
                        productClassification = x.Product.CommonCode.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,


                        facilitiesCode = x.Facility != null ? x.Facility.Code : "",
                        facilitiesName = x.Facility != null ? x.Facility.Name : "",
                        facilitiesUID = x.Facility != null ? x.Facility.Uid : "",
                        moldCode = x.Mold != null ? x.Mold.Code : "",
                        moldName = x.Mold != null ? x.Mold.Name : "",
                       

                        motorElectricCurrent = "",
                        spm = "",

                        bottomDeadPoint = "-",
                        
                        bottomDeadPoint_LL = _db.FacilityControls.FirstOrDefault() != null ? _db.FacilityControls.FirstOrDefault().BottomDeadPointDL.ToString("0.000") : "",
                        bottomDeadPoint_UL = _db.FacilityControls.FirstOrDefault() != null ? _db.FacilityControls.FirstOrDefault().BottomDeadPointUL.ToString("0.000") : "",
                        
                        dateFlag = x.WorkerOrder.WorkOrderDate,
                        WorkStartDateTime = x.ProcessProgress != null ? x.ProcessProgress.WorkStartDateTime : DateTime.Now

                    })
                    .OrderByDescending(x => x.dateFlag)
                    .ToListAsync();

                var res2 = res.Where(x => req.processId == 0 ? true : x.processId == req.processId);
                var res3 = res2.Where(x => req.productLOT == "" ? true : x.productLOT.Contains(req.productLOT));

                foreach (var x in res3)
                {
                    if (x.facilitiesCode != null && x.facilitiesCode != "")
                    {
                        var logData = await _db.FacilityStatusLog
                            .Where(m => m.UID == x.facilitiesUID)
                            .Where(m => m.CreateTime >= x.WorkStartDateTime)
                            .OrderBy(m => m.CreateTime)
                            .FirstOrDefaultAsync();

                        if (logData != null)
                        {
                            x.spm = Convert.ToInt32(logData.LW058, 16).ToString();
                            x.motorElectricCurrent = Convert.ToInt32(logData.LW059, 16).ToString();
                        }
                        else
                        {
                            x.spm = "no data";
                            x.motorElectricCurrent = "no data";
                            x.bottomDeadPoint = "-";
                        }
                    }
                }


                var Res = new Response<IEnumerable<BottomDeadPointResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res3
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<BottomDeadPointResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<SlideCurrentResponse>>> QualitySlideManage(VoltageCheckReq001 req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(req.workOrderStartDate))
                    .Where(x => x.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(req.workOrderEndDate))
                    .Where(x => x.Facility != null)
                    .Where(x => x.Facility.CommonCode.Name == "프레스")
                    .Where(x => x.Facility.Uid != "")
                    .Where(x => x.Facility.IsUsing == true)
                    .Where(x => req.facilityId == 0 ? true : x.Facility.Id == req.facilityId)
                    .Where(x => req.moldId == 0 ? true : x.Mold.Id == req.moldId)
                    .Where(x => x.ProcessProgress.WorkStatus != "작업대기")
                    .Select(x => new SlideCurrentResponse
                    {
                        workOrderNo = x.WorkerOrder.WorkOrderNo,
                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),

                        processId = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),

                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        productLOT = _db.Lots
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                            .Where(y => y.ProcessType == "P")
                            .Select(y => y.LotName).FirstOrDefault(),

                        productCode = x.Product.Code,
                        productName = x.Product.Name,
                        productClassification = x.Product.CommonCode.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,


                        // facilityId = x.Facility.Id,
                        facilitiesCode = x.Facility.Code,
                        facilitiesName = x.Facility.Name,
                        facilitiesUID = x.Facility.Uid,
                        moldCode = x.Mold.Code,
                        moldName = x.Mold.Name,

                        slideElectricCurrent = "",
                        slideValue = "",
                        slideValue_LL = "",
                        slideValue_UL = "",

                        dateFlag = x.WorkerOrder.WorkOrderDate,
                        WorkStartDateTime = x.ProcessProgress != null ? x.ProcessProgress.WorkStartDateTime : DateTime.Now
                    })
                    .OrderByDescending(x => x.dateFlag)
                    .ToListAsync();

                var res2 = res.Where(x => req.processId == 0 ? true : x.processId == req.processId);
                var res3 = res2.Where(x => req.productLOT == "" ? true : x.productLOT.Contains(req.productLOT));

                foreach (var x in res3)
                {
                    if (x.facilitiesCode != null && x.facilitiesCode != "")
                    {
                        var logData = await _db.FacilityStatusLog
                            .Where(m => m.UID == x.facilitiesUID)
                            .Where(m => m.CreateTime >= x.WorkStartDateTime)
                            .OrderBy(m => m.CreateTime)
                            .FirstOrDefaultAsync();

                        if (logData != null)
                        {
                            x.slideElectricCurrent = Convert.ToInt32(logData.LW060, 16).ToString();
                            x.slideValue = Convert.ToInt32(logData.LW000, 16).ToString();
                            x.slideValue_LL = Convert.ToInt32(logData.DW4101, 16).ToString();
                            x.slideValue_UL = Convert.ToInt32(logData.DW4102, 16).ToString();
                        }
                        else
                        {
                            x.slideElectricCurrent = "no data";
                            x.slideValue = "no data";
                            x.slideValue_LL = "no data";
                            x.slideValue_UL = "no data";
                        }
                    }
                }


                var Res = new Response<IEnumerable<SlideCurrentResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res3
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<SlideCurrentResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<TonCheckResponse>>> QualityTonManage(VoltageCheckReq001 req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(req.workOrderStartDate))
                    .Where(x => x.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(req.workOrderEndDate))
                    .Where(x => x.Facility != null)
                    .Where(x => req.facilityId == 0 ? true : x.Facility.Id == req.facilityId)
                    .Where(x => req.moldId == 0 ? true : x.Mold.Id == req.moldId)
                    .Where(x => x.ProcessProgress.WorkStatus != "작업대기")
                    .Select(x => new TonCheckResponse
                    {
                        workOrderNo = x.WorkerOrder.WorkOrderNo,
                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),


                        processId = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        processCode = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),

                        processName = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        productLOT = _db.Lots
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                            .Where(y => y.ProcessType == "P")
                            .Select(y => y.LotName).FirstOrDefault(),


                        productCode = x.Product.Code,
                        productName = x.Product.Name,
                        productClassification = x.Product.CommonCode.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,


                        spm = "",
                        motorElectricCurrent = "",
                        tonValue = "",
                        tonValue_LL = _db.FacilityControls.FirstOrDefault() != null ? _db.FacilityControls.FirstOrDefault().TonDL.ToString("0.000") : "",
                        tonValue_UL = _db.FacilityControls.FirstOrDefault() != null ? _db.FacilityControls.FirstOrDefault().TonUL.ToString("0.000") : "",

                        // facilityId = x.Facility.Id,
                        facilitiesCode = x.Facility.Code,
                        facilitiesName = x.Facility.Name,
                        facilitiesUID = x.Facility.Uid,
                        moldCode = x.Mold.Code,
                        moldName = x.Mold.Name,

                        dateFlag = x.WorkerOrder.WorkOrderDate,

                        WorkStartDateTime = x.ProcessProgress != null ? x.ProcessProgress.WorkStartDateTime : DateTime.Now

                    })
                    .OrderByDescending(x => x.dateFlag)
                    .ToListAsync();

                var res2 = res.Where(x => req.processId == 0 ? true : x.processId == req.processId);
                var res3 = res2.Where(x => req.productLOT == "" ? true : x.productLOT.Contains(req.productLOT));

                foreach (var x in res3)
                {
                    if (x.facilitiesCode != null && x.facilitiesCode != "")
                    {
                        var logData = await _db.FacilityStatusLog
                            .Where(m => m.UID == x.facilitiesUID)
                            .Where(m => m.CreateTime >= x.WorkStartDateTime)
                            .OrderBy(m => m.CreateTime)
                            .FirstOrDefaultAsync();

                        if (logData != null)
                        {
                            x.spm = Convert.ToInt32(logData.LW058, 16).ToString();
                            x.motorElectricCurrent = Convert.ToInt32(logData.LW059, 16).ToString();
                            x.tonValue = Convert.ToInt32(logData.LW056, 16).ToString();
                        }
                        else
                        {
                            x.spm = "no data";
                            x.motorElectricCurrent = "no data";
                            x.tonValue = "no data";
                        }
                    }
                }


                var Res = new Response<IEnumerable<TonCheckResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res3
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<TonCheckResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }

        public async Task<Response<IEnumerable<VoltageInspectionResponse>>> GetVoltageInspections(VoltageInspectionRequest req)
        {
            try
            {
                var res = await _db.VoltageInspections
                    .Where(x => x.InspectionDate >= Convert.ToDateTime(req.InspectionStartDate))
                    .Where(x => x.InspectionDate <= Convert.ToDateTime(req.InspectionEndDate))
                    .Where(x => req.ProductId == 0 ? true : x.Product.Id == req.ProductId)
                    .Where(x => x.Lot.Contains(req.Lot))
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new VoltageInspectionResponse
                    {
                        VoltageInspectionId = x.VoltageInspectionId,
                        InspectionDate = x.InspectionDate.ToString("yyyy-MM-dd"),

                        LoadCell = x.LoadCellTon,
                        Ton = x.Ton,
                        Lot = x.Lot,
                        MainMotorAmp = x.MainMotorAmp,
                        MainMotorOverAmp = x.MainMotorOverAmp,
                        MotorSpm = x.MotorSpm,
                        Memo = x.Memo,
                        PressMaxSpm = x.PressMaxSpm,
                        ProductId = x.Product.Id,
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        ProductClassification = x.Product.CommonCode.Name,

                        SlideAmp = x.SlideAmp,
                        SlideMotorOverAmp = x.SlideMotorOverAmp,
                        Slp = x.Slp,

                        A1Slp = x.A1Slp,
                        A1Speed = x.A1Speed,
                        A2Slp = x.A1Slp,
                        A2Speed = x.A2Speed,

                        A3Slp = x.A3Slp,
                        A3Speed = x.A3Speed,
                        
                        A4Speed = x.A4Speed,
                        A4Slp = x.A4Slp,
                        A5Speed = x.A5Speed,
                        A5Slp = x.A5Slp,

                    }).ToListAsync();

                var Res = new Response<IEnumerable<VoltageInspectionResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<VoltageInspectionResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<bool>> CreateVoltageInspections(VoltageInspectionRequest req)
        {
            try
            {

                var voltageInspection = new VoltageInspection
                {
                    InspectionDate = Convert.ToDateTime(req.InspectionDate),
                    IsDeleted = false,
                    A1Slp = req.A1Slp,
                    A1Speed = req.A1Speed,
                    A2Slp = req.A2Slp,
                    A2Speed = req.A2Speed,
                    A3Slp = req.A3Slp,
                    A3Speed = req.A3Speed,
                    A4Slp = req.A4Slp,
                    A4Speed = req.A4Speed,
                    A5Slp = req.A5Slp,
                    A5Speed = req.A5Speed,
                    LoadCellTon = req.LoadCell,
                    Lot = req.Lot,
                    MainMotorAmp = req.MainMotorAmp,
                    MainMotorOverAmp = req.MainMotorOverAmp,
                    Memo = req.Memo,
                    MotorSpm = req.MotorSpm,
                    PressMaxSpm = req.PressMaxSpm,
                    Product = _db.Products.Where(x=>x.Id == req.ProductId).FirstOrDefault(),
                    SlideAmp = req.SlideAmp,
                    SlideMotorOverAmp = req.SlideMotorOverAmp,
                    Slp = req.Slp,
                    Ton = req.Ton,

                };

                var result = await _db.VoltageInspections.AddAsync(voltageInspection);

                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = false
                };

                return Res;
            }
        }
        public async Task<Response<bool>> DeleteVoltageInspections(VoltageInspectionRequest req)
        {
            try
            {
                var voltageInspections = await _db.VoltageInspections
                    .Include(x=>x.Product)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => req.VoltageInspectionIds.Contains(x.VoltageInspectionId))
                    .ToArrayAsync();
                
                foreach(var insp in voltageInspections)
                {
                    insp.IsDeleted = true;
                }

                _db.VoltageInspections.UpdateRange(voltageInspections);
                await Save();


                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = false
                };

                return Res;
            }
        }
        public async Task<Response<InspectionDataResponse>> GetInspectionData()
        {
            try
            {
                var res = await _db.FacilityStatus_Inspection
                    .Select(x => new InspectionDataResponse
                    {
                        MainMotorAmp = Convert.ToInt32(x.DW037, 16).ToString(),
                        SlideAmp = Convert.ToInt32(x.DW038, 16).ToString(),
                        MotorSpm = Convert.ToInt32(x.LW067, 16).ToString(),
                        Slp = Convert.ToInt32(x.DW040, 16).ToString(),

                        MainMotorOverAmp = Convert.ToInt32(x.DW4663, 16).ToString(),
                        SlideMotorOverAmp = (Convert.ToInt32(x.DW4669, 16)/10).ToString("0.0"),
                        PressMaxSpm = Convert.ToInt32(x.DW4522, 16).ToString(),

                        A1Speed = Convert.ToInt32(x.DW4530, 16).ToString(),
                        A1Slp = Convert.ToInt32(x.DW4531, 16).ToString(),
                        A2Speed = Convert.ToInt32(x.DW4532, 16).ToString(),
                        A2Slp = Convert.ToInt32(x.DW4533, 16).ToString(),
                        A3Speed = Convert.ToInt32(x.DW4534, 16).ToString(),
                        A3Slp = Convert.ToInt32(x.DW4535, 16).ToString(),
                        A4Speed = Convert.ToInt32(x.DW4536, 16).ToString(),
                        A4Slp = Convert.ToInt32(x.DW4537, 16).ToString(),
                        A5Speed = Convert.ToInt32(x.DW4538, 16).ToString(),
                        A5Slp = Convert.ToInt32(x.DW4539, 16).ToString(),
                        
                        UpdateTime = x.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")

                    }).FirstOrDefaultAsync();

                var Res = new Response<InspectionDataResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<InspectionDataResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<TempHumidResponse>>> GetTempHumids(TempHumidRequest req)
        {
            try
            {
                var res = await _db.Facilitys
                    .Where(x => req.FacilityId == 0 ? true : x.Id == req.FacilityId)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.CommonCode.Name == "ICT 설비")
                    .Select(x => new TempHumidResponse
                    {
                        Date = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd HH:mm:ss"),
                        FacilityCode = x.Code,
                        FacilityName = x.Name,
                        FacilityType = x.CommonCode.Name,
                        Temp = _db.FacilityStatus
                            .Where(y=>y.UID == x.Uid)
                            .Select(y=> (Convert.ToInt32(y.LW000, 16) / 10).ToString("0.0")).FirstOrDefault(),
                        Humid = _db.FacilityStatus
                            .Where(y => y.UID == x.Uid)
                            .Select(y => (Convert.ToInt32(y.DW0020, 16)).ToString("0.0")).FirstOrDefault(),

                        Temp_LL = _db.FacilityControls.Where(y=>y.Facility.Id == x.Id).Select(y=>y.BottomDeadPointDL).FirstOrDefault().ToString("0.0"),
                        Temp_UL = _db.FacilityControls.Where(y=>y.Facility.Id == x.Id).Select(y=>y.BottomDeadPointUL).FirstOrDefault().ToString("0.0"),

                    }).ToListAsync();

                var Res = new Response<IEnumerable<TempHumidResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<IEnumerable<TempHumidResponse>>()
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
