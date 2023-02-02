using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Data.Models.Mold;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InspectionRepair;
using WebBasedMES.ViewModels.Mold;

namespace WebBasedMES.Services.Repositories.MoldManage
{
    public class PreventiveMaintenanceRepository : IPreventiveMaintenanceRepository
    {
        private readonly ApiDbContext _db;

        public PreventiveMaintenanceRepository(ApiDbContext db)
        {
            _db = db;
        }

        public async Task Save ()
        {
            await _db.SaveChangesAsync();
        }


        #region 설비 예방보존 기준정보
        public async Task<Response<PreventiveMaintenanceFacilityResponse>> GetPreventiveMaintnanceFacility(PreventiveMaintenanceFacilityRequest req)
        {
            try
            {
                var res = await _db.PreventiveMaintenanceFacilitys
                    .Where(x => x.PreventiveMaintenanceFacilityId == req.PreventiveMaintenanceFacilityId)
                    .Select(x => new PreventiveMaintenanceFacilityResponse
                    {
                        FacilityClassification = x.Facility.CommonCode.Name,
                        FacilityType = x.Facility.CommonCode.Name,
                        FacilityCode = x.Facility.Code,
                        FacilityId = x.Facility.Id,
                        FacilityMemo = x.Facility.Memo,
                        FacilityName = x.Facility.Name,
                        IsRegularInspectionPeriod = x.Facility.PeriodicalInspection == true ? 1 : 0,
                        IsUsing = x.Facility.IsUsing == true ? 1:0,
                        PreventiveMaintenanceFacilityId = x.PreventiveMaintenanceFacilityId,
                        RegularInspectionPeriod = x.RegularInspectionPeriod,
                        RegularInspectionPeriodName = _db.CommonCodes.Where(y=>y.Id == x.RegularInspectionPeriod).FirstOrDefault().Name,
                       
                    }).FirstOrDefaultAsync();


                var Res = new Response<PreventiveMaintenanceFacilityResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<PreventiveMaintenanceFacilityResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<PreventiveMaintenanceFacilityResponse>>> GetPreventiveMaintnanceFacilitys(PreventiveMaintenanceFacilityRequest req)
        {
            try 
            { 
                var res = await _db.PreventiveMaintenanceFacilitys
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.FacilityId == 0 ? true :  x.Facility.Id == req.FacilityId)
                    .Where(x => req.IsUsingStr == "" ? true : req.IsUsingStr == "Y" ? x.Facility.IsUsing == true : x.Facility.IsUsing == false)

                    .Select(x => new PreventiveMaintenanceFacilityResponse
                    {
                        PreventiveMaintenanceFacilityId = x.PreventiveMaintenanceFacilityId,

                        FacilityClassification = x.Facility.CommonCode.Name,
                        FacilityCode = x.Facility.Code,
                        FacilityId = x.Facility.Id,
                        FacilityMemo = x.Facility.Memo,
                        FacilityName = x.Facility.Name,
                        IsRegularInspectionPeriod = x.Facility.PeriodicalInspection == true ? 1 : 0,
                        IsUsing = x.Facility.IsUsing == true ? 1:0,
                        RegularInspectionPeriod = x.RegularInspectionPeriod,
                        RegularInspectionPeriodName = _db.CommonCodes.Where(y=>y.Id == x.RegularInspectionPeriod).FirstOrDefault().Name,

  
                    }).ToListAsync();


                var Res = new Response<IEnumerable<PreventiveMaintenanceFacilityResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PreventiveMaintenanceFacilityResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> UpdatePreventiveMaintnanceFacilitys(PreventiveMaintenanceFacilityRequest req)
        {
            try
            {
                var _facility = await _db.PreventiveMaintenanceFacilitys.Where(x => x.PreventiveMaintenanceFacilityId == req.PreventiveMaintenanceFacilityId).FirstOrDefaultAsync();

                _facility.Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault();
                _facility.RegularInspectionPeriod = req.RegularInspectionPeriod;

                _db.PreventiveMaintenanceFacilitys.Update(_facility);
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
        public async Task<Response<bool>> CreatePreventiveMaintnanceFacilitys(PreventiveMaintenanceFacilityRequest req)
        {
            try
            {
                var _prevFac = await _db.PreventiveMaintenanceFacilitys
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Facility.Id == req.FacilityId)
                    .FirstOrDefaultAsync();

                if(_prevFac != null)
                {
                    var Res2 = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "중복된 설비가 존재합니다. 다시 확인해주세요.",
                        Data = true
                    };
                    return Res2;


                }

                var _facility = new PreventiveMaintenanceFacility
                {
                    Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault(),
                    RegularInspectionPeriod  = req.RegularInspectionPeriod,
                };

                _db.PreventiveMaintenanceFacilitys.Add(_facility);
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
        public async Task<Response<bool>> DeletePreventiveMaintnanceFacilitys(PreventiveMaintenanceFacilityRequest req)
        {
            try
            {
                foreach(var i in req.PreventiveMaintenanceFacilityIds)
                {
                    var _fac = _db.PreventiveMaintenanceFacilitys.Where(x => x.PreventiveMaintenanceFacilityId == i).FirstOrDefault();
                    _fac.IsDeleted = 1;
                    _db.PreventiveMaintenanceFacilitys.Update(_fac);
                }
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

        #endregion


        #region 금형 예방보존 기준정보
        public async Task<Response<PreventiveMaintenanceMoldResponse>> GetPreventiveMaintnanceMold(PreventiveMaintenanceMoldRequest req)
        {
            try
            {
                var res = await _db.PreventiveMaintenanceMolds
                    .Where(x => x.PreventiveMaintenanceMoldId == req.PreventiveMaintenanceMoldId)

                    .Select(x => new PreventiveMaintenanceMoldResponse
                    {
                        MoldClassification = x.Mold.CommonCode.Name,
                        MoldType = x.Mold.CommonCode.Name,
                        MoldCode = x.Mold.Code,
                        MoldId = x.Mold.Id,
                        MoldMemo = x.Mold.Memo,
                        MoldName = x.Mold.Name,
                        IsRegularInspectionPeriod = x.Mold.RegularInspection == true ? 1 : 0,

                        MoldStatus = x.Mold.Status,
                        PreventiveMaintenanceMoldId = x.PreventiveMaintenanceMoldId,
                        RegularInspectionPeriod = x.RegularInspectionPeriod,
                        RegularInspectionPeriodName = _db.CommonCodes.Where(y => y.Id == x.RegularInspectionPeriod).FirstOrDefault().Name,
                        MoldCleaningCount = x.MoldCleaningCount,
                        MoldCleaningPeriod = x.MoldCleaningPeriod,
                        MoldCleaningPeriodName = _db.CommonCodes.Where(y => y.Id == x.MoldCleaningPeriod).FirstOrDefault().Name,
                        RegularInspectionCount = x.RegularInspectionCount

                    }).FirstOrDefaultAsync();


                var Res = new Response<PreventiveMaintenanceMoldResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<PreventiveMaintenanceMoldResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<PreventiveMaintenanceMoldResponse>>> GetPreventiveMaintnanceMolds(PreventiveMaintenanceMoldRequest req)
        {
            try
            {
                var res = await _db.PreventiveMaintenanceMolds
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.MoldId == 0? true :  x.Mold.Id == req.MoldId)
                    .Where(x => req.MoldStatusStr == "" ? true : x.Mold.Status == req.MoldStatusStr)

                    .Select(x => new PreventiveMaintenanceMoldResponse
                    {
                        MoldClassification = x.Mold.CommonCode.Name,
                        MoldCode = x.Mold.Code,
                        MoldId = x.Mold.Id,
                        MoldMemo = x.Mold.Memo,
                        MoldName = x.Mold.Name,
                        IsRegularInspectionPeriod = x.Mold.RegularInspection == true ? 1 : 0,

                        MoldStatus = x.Mold.Status,
                        PreventiveMaintenanceMoldId = x.PreventiveMaintenanceMoldId,
                        RegularInspectionPeriod = x.RegularInspectionPeriod,
                        RegularInspectionPeriodName = _db.CommonCodes.Where(y => y.Id == x.RegularInspectionPeriod).FirstOrDefault().Name,
                        MoldCleaningCount = x.MoldCleaningCount,
                        MoldCleaningPeriod = x.MoldCleaningPeriod,
                        MoldCleaningPeriodName = _db.CommonCodes.Where(y => y.Id == x.MoldCleaningPeriod).FirstOrDefault().Name,
                        RegularInspectionCount = x.RegularInspectionCount
                    }).ToListAsync();


                var Res = new Response<IEnumerable<PreventiveMaintenanceMoldResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PreventiveMaintenanceMoldResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> UpdatePreventiveMaintnanceMolds(PreventiveMaintenanceMoldRequest req)
        {
            try
            {
                var _mold = await _db.PreventiveMaintenanceMolds.Where(x => x.PreventiveMaintenanceMoldId == req.PreventiveMaintenanceMoldId).FirstOrDefaultAsync();

                _mold.Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault();
                _mold.RegularInspectionPeriod = req.RegularInspectionPeriod;
                _mold.RegularInspectionCount = req.RegularInspectionCount;
                _mold.MoldCleaningCount = req.MoldCleaningCount;
                _mold.MoldCleaningPeriod = req.MoldCleaningPeriod;
                

                _db.PreventiveMaintenanceMolds.Update(_mold);
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
        public async Task<Response<bool>> CreatePreventiveMaintnanceMolds(PreventiveMaintenanceMoldRequest req)
        {
            try
            {
                var _prevMold = await _db.PreventiveMaintenanceMolds
    .Where(x => x.IsDeleted == 0)
    .Where(x => x.Mold.Id == req.MoldId)
    .FirstOrDefaultAsync();

                if (_prevMold != null)
                {
                    var Res2 = new Response<bool>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "중복된 금형이 존재합니다. 다시 확인해주세요.",
                        Data = true
                    };
                    return Res2;


                }



                var _mold = new PreventiveMaintenanceMold
                {
                    Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault(),
                    RegularInspectionPeriod = req.RegularInspectionPeriod,
                    RegularInspectionCount = req.RegularInspectionCount,
                    MoldCleaningCount = req.MoldCleaningCount,
                    MoldCleaningPeriod = req.MoldCleaningPeriod,
                    
                };

                _db.PreventiveMaintenanceMolds.Add(_mold);
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
        public async Task<Response<bool>> DeletePreventiveMaintnanceMolds(PreventiveMaintenanceMoldRequest req)
        {
            try
            {
                foreach (var i in req.PreventiveMaintenanceMoldIds)
                {
                    var _mold = _db.PreventiveMaintenanceMolds.Where(x => x.PreventiveMaintenanceMoldId == i).FirstOrDefault();
                    _mold.IsDeleted = 1;
                    _db.PreventiveMaintenanceMolds.Update(_mold);
                }
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



        //예방보존관리!!
        public async Task<Response<IEnumerable<PreventiveMaintenanceFacilityResponse>>> GetPreventiveMaintenanceManageFacilitys(PreventiveMaintenanceFacilityRequest req)
        {
            try
            {
                var res = await _db.PreventiveMaintenanceFacilitys
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.FacilityId == 0 ? true :  x.Facility.Id == req.FacilityId)
                    .Where(x => req.IsUsingStr == "" ? true : req.IsUsingStr == "Y" ? x.Facility.IsUsing == true : x.Facility.IsUsing == false)

                    .Select(x => new PreventiveMaintenanceFacilityResponse
                    {
                        PreventiveMaintenanceFacilityId = x.PreventiveMaintenanceFacilityId,

                        FacilityClassification = x.Facility.CommonCode.Name,
                        FacilityCode = x.Facility.Code,
                        FacilityId = x.Facility.Id,
                        FacilityMemo = x.Facility.Memo,
                        FacilityName = x.Facility.Name,
                        IsRegularInspectionPeriod = x.Facility.PeriodicalInspection == true ? 1 : 0,
                        IsUsing = x.Facility.IsUsing == true ? 1:0,
                        RegularInspectionPeriod = x.RegularInspectionPeriod,
                        RegularInspectionPeriodName = _db.CommonCodes.Where(y=>y.Id == x.RegularInspectionPeriod).FirstOrDefault().Name,
                        LastInspectionDate = _db.FacilityInspections.Where(y => y.Facility.Id == x.Facility.Id).Where(y => y.Type == "PERIOD").Count()== 0 ? "-" :
                           _db.FacilityInspections.Where(y => y.Facility == x.Facility)
                              .Where(y => y.Type == "PERIOD")
                                .OrderByDescending(y => y.RegisterDate)
                                .Select(y => y.RegisterDate.ToString("yyyy-MM-dd")).FirstOrDefault(),

                        NextInspectionDate = "2021-12-31",

                    }).ToListAsync();


                
                foreach(var detail in res)
                {
                    if(detail.LastInspectionDate == "-")
                    {
                        detail.NextInspectionDate = "-";
                    }
                    else
                    {
                        DateTime last = Convert.ToDateTime(detail.LastInspectionDate);
                        switch (detail.RegularInspectionPeriodName)
                        {
                            case "1개월":
                                last = last.AddMonths(1);
                                break;
                            case "1주일":
                                last = last.AddDays(7);
                                break;
                            case "분기":
                                last = last.AddMonths(3);
                                break;
                            case "1년":
                                last = last.AddMonths(12);
                                break;
                            case "6개월":
                                last = last.AddMonths(6);
                                break;

                            default:
                                last = last.AddMonths(1);
                                break;
                        }
                        detail.NextInspectionDate = last.ToString("yyyy-MM-dd");
                    }
                }




                var Res = new Response<IEnumerable<PreventiveMaintenanceFacilityResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PreventiveMaintenanceFacilityResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        
        public async Task<Response<IEnumerable<PreventiveMaintenanceMoldResponse>>> GetPreventiveMaintenanceManageMolds(PreventiveMaintenanceMoldRequest req)
        {
            try
            {
                var res = await _db.PreventiveMaintenanceMolds
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.MoldId == 0 ? true : x.Mold.Id == req.MoldId)
                    .Where(x => req.MoldStatusStr == "" ? true : x.Mold.Status == req.MoldStatusStr)

                    .Select(x => new PreventiveMaintenanceMoldResponse
                    {
                        MoldClassification = x.Mold.CommonCode.Name,
                        MoldCode = x.Mold.Code,
                        MoldId = x.Mold.Id,
                        MoldMemo = x.Mold.Memo,
                        MoldName = x.Mold.Name,
                        IsRegularInspectionPeriod = x.Mold.RegularInspection == true ? 1 : 0,

                        MoldStatus = x.Mold.Status,
                        PreventiveMaintenanceMoldId = x.PreventiveMaintenanceMoldId,
                        RegularInspectionPeriod = x.RegularInspectionPeriod,
                        RegularInspectionPeriodName = _db.CommonCodes.Where(y => y.Id == x.RegularInspectionPeriod).FirstOrDefault().Name,

                        MoldCleaningCount = x.MoldCleaningCount,
                        RegularInspectionCount = x.RegularInspectionCount,
                        
                        LastInspectionDate = _db.MoldInspections.Where(y => y.Mold == x.Mold).Where(y => y.Type == "PERIOD").Count() == 0 ? "-" :
                            _db.MoldInspections.Where(y => y.Mold == x.Mold)
                                .Where(y => y.Type == "PERIOD")
                                .OrderByDescending(y => y.RegisterDate)
                                .Select(y => y.RegisterDate.ToString("yyyy-MM-dd")).FirstOrDefault(),

                        NextInspectionDate = "-",
                        
                        MoldCleaningPeriod = x.MoldCleaningPeriod,
                        MoldCleaningPeriodName = _db.CommonCodes.Where(y => y.Id == x.MoldCleaningPeriod).FirstOrDefault().Name,
                        LastCleaningDate = _db.MoldCleanings.Where(y=>y.Mold == x.Mold).Count() == 0 ? "-" : _db.MoldCleanings.Where(y=>y.Mold == x.Mold)
                            .OrderByDescending(y => y.CleaningDate).Select(y=>y.CleaningDate.ToString("yyyy-MM-dd")).FirstOrDefault(),
                        
                        NextCleaningDate = "-",

                        MoldCount = _db.WorkerOrderProducePlans
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.Mold == x.Mold)
                            .Select(y=>y.ProcessProgress.ProductionQuantity).Sum(),
                        
                        CountAfterInspection = 0,
                        CountAfterCleaning = 0,



                    }).ToListAsync();

                foreach (var detail in res)
                {
                    if (detail.LastInspectionDate == "-")
                    {
                        detail.NextInspectionDate = "-";
                    }
                    else
                    {
                        DateTime last = Convert.ToDateTime(detail.LastInspectionDate);
                        switch (detail.RegularInspectionPeriodName)
                        {
                            case "1개월":
                                last = last.AddMonths(1);
                                break;
                            case "1주일":
                                last = last.AddDays(7);
                                break;
                            case "분기":
                                last = last.AddMonths(3);
                                break;
                            case "1년":
                                last = last.AddMonths(12);
                                break;
                            case "6개월":
                                last = last.AddMonths(6);
                                break;

                            default:
                                last = last.AddMonths(1);
                                break;
                        }
                        detail.NextInspectionDate = last.ToString("yyyy-MM-dd");
                    }

                    if (detail.LastCleaningDate == "-")
                    {
                        detail.NextCleaningDate = "-";
                    }
                    else
                    {
                        DateTime last2 = Convert.ToDateTime(detail.LastCleaningDate);
                        switch (detail.MoldCleaningPeriodName)
                        {
                            case "1개월":
                                last2 = last2.AddMonths(1);
                                break;
                            case "1주일":
                                last2 = last2.AddDays(7);
                                break;
                            case "분기":
                                last2 = last2.AddMonths(3);
                                break;
                            case "1년":
                                last2 = last2.AddMonths(12);
                                break;
                            case "6개월":
                                last2 = last2.AddMonths(6);
                                break;

                            default:
                                last2 = last2.AddMonths(1);
                                break;
                        }
                        detail.NextCleaningDate = last2.ToString("yyyy-MM-dd");
                    }

                    detail.CountAfterInspection = detail.LastInspectionDate == "-" ? detail.MoldCount : _db.ProcessProgresses
                           .Where(z=>z.WorkEndDateTime>=Convert.ToDateTime(detail.LastInspectionDate))
                           .Where(z=>z.IsDeleted == 0)
                           .Where(z=>z.WorkOrderProducePlan.Mold.Id == detail.MoldId)
                           .Select(z=>z.ProductionQuantity)
                           .Sum(); //+ detail.RegularInspectionCount;

                    detail.CountAfterCleaning = detail.LastCleaningDate == "-" ? detail.MoldCount : _db.ProcessProgresses
                         .Where(z => z.WorkEndDateTime >= Convert.ToDateTime(detail.LastCleaningDate))
                        .Where(z => z.IsDeleted == 0)
                        .Where(z => z.WorkOrderProducePlan.Mold.Id == detail.MoldId)
                        .Select(z => z.ProductionQuantity)
                        .Sum(); //+ detail.RegularInspectionCount;


                    //detail.CountAfterCleaning = detail.MoldCount;// + detail.MoldCleaningCount;
                }


                var Res = new Response<IEnumerable<PreventiveMaintenanceMoldResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<PreventiveMaintenanceMoldResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        #endregion

    }
}
