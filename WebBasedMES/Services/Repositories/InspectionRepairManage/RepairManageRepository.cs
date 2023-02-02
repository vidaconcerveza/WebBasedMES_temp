using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InspectionRepair;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebBasedMES.Data.Models.InspectionRepair;

namespace WebBasedMES.Services.Repositories.InspectionRepairManage
{
    public class RepairManageRepository: IRepairManageRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public RepairManageRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

        #region 수리요청

        public async Task<Response<IEnumerable<RepairAsksResponse>>> GetRepairAsks(RepairAskRequest req)
        {
            try
            {
                var res = await _db.RepairAsks
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.FacilityId == 0 ? true : (req.RepairTarget == "FACILITY"? x.Facility.Id == req.FacilityId : true))
                    .Where(x => req.MoldId == 0 ? true : (req.RepairTarget == "MOLD"? x.Mold.Id == req.MoldId : true))
                    .Where(x => x.RepairAskNo.Contains(req.RepairAskNo))
                    .Where(x => x.RegisterDate >= Convert.ToDateTime(req.RegisterStartDate))
                    .Where(x => x.RegisterDate <= Convert.ToDateTime(req.RegisterEndDate))
                    .Where(x => x.RepairTarget == req.RepairTarget)
                    .OrderBy(x=>x.RepairAskNo)
                    .Select(x => new RepairAsksResponse
                    {

                        RepairAskId = x.RepairAskId,
                        IsOutSourcing = x.IsOutSourcing,
                        FacilityType = x.Facility.CommonCode.Name,
                        FacilityCode = x.Facility.Code,
                        FacilityName = x.Facility.Name,
                        MoldyType = x.Mold.CommonCode.Name,
                        MoldCode = x.Mold.Code,
                        MoldName = x.Mold.Name,
                        RepairAskNo = x.RepairAskNo,
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        RegisterName = x.Register.FullName,
                        RepairAskMemo = x.RepairAskMemo,
                        RepairResult = x.RepairLog != null ? _db.CommonCodes.Where(y => y.Id == x.RepairLog.RepairResult).FirstOrDefault().Name : "",
                        UploadFiles = x.RepairLog != null? x.RepairLog.UploadFiles.ToList() : new List<UploadFile>(),
                    }).ToListAsync();


                var Res = new Response<IEnumerable<RepairAsksResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<RepairAsksResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<RepairAsksResponse>> GetRepairAsk(RepairAskRequest req)
        {
            try 
            { 
                var res = await _db.RepairAsks
                    .Include(x=>x.Facility)
                    .Include(x=>x.Mold)
                    .Where(x=>x.RepairAskId == req.RepairAskId)
                    .Select(x => new RepairAsksResponse
                    {
                        RepairAskId = x.RepairAskId,
                        IsOutSourcing = x.IsOutSourcing,
                        FacilityId = x.Facility != null ? x.Facility.Id : 0,
                        FacilityType = x.Facility !=null? x.Facility.CommonCode.Name : "",
                        FacilityCode = x.Facility != null ? x.Facility.Code : "",
                        FacilityName = x.Facility != null ? x.Facility.Name : "",
                        MoldId = x.Mold != null ?  x.Mold.Id : 0,
                        MoldyType = x.Mold != null ? x.Mold.CommonCode.Name : "",
                        MoldCode = x.Mold != null ? x.Mold.Code: "",
                        MoldName = x.Mold != null ? x.Mold.Name:"",
                        RepairAskNo = x.RepairAskNo,
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        RegisterId = x.Register.Id,
                        RegisterName = x.Register.FullName,
                        RepairAskMemo = x.RepairAskMemo,

                    }).FirstOrDefaultAsync();


                var Res = new Response<RepairAsksResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<RepairAsksResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<bool>> CreateRepairAsk(RepairAskCreateUpdateRequest req)
        {
            try
            {
                var _register = await _userManager.FindByIdAsync(req.RegisterId);

                var _prevRepairAsk = await _db.RepairAsks
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.RegisterDate == Convert.ToDateTime(req.RegisterDate))
                    .OrderByDescending(x => x.RepairAskNo)
                    .FirstOrDefaultAsync();

                string _repairNo = "";
                if(_prevRepairAsk != null)
                {
                    _repairNo = "RE" + Convert.ToDateTime(req.RegisterDate).ToString("yyMMdd") + (Convert.ToInt32(_prevRepairAsk.RepairAskNo.Substring(8, 4)) + 1).ToString("0000");
                }
                else
                {
                    _repairNo = "RE" + Convert.ToDateTime(req.RegisterDate).ToString("yyMMdd") + "0001";
                }

                var repairAsk = new RepairAsk
                {
                    RepairAskNo = _repairNo,
                    RegisterDate = Convert.ToDateTime(req.RegisterDate),
                    Facility = _db.Facilitys.Where(x=>x.Id == req.FacilityId).FirstOrDefault(),
                    Mold = _db.Molds.Where(x=>x.Id == req.MoldId).FirstOrDefault(),
                    IsOutSourcing = req.IsOutSourcing,
                    Register = _register,
                    RepairAskMemo = req.RepairAskMemo,
                    RepairResult = "",
                    RepairTarget = req.RepairTarget,
                };

                await _db.RepairAsks.AddAsync(repairAsk);
                await Save();


                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true,
                };

                return Res;

            }
            catch (Exception ex)
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
        public async Task<Response<bool>> UpdateRepairAsk(RepairAskCreateUpdateRequest req)
        {
            try
            {
                var _register = await _userManager.FindByIdAsync(req.RegisterId);
                var _repairAsk =await  _db.RepairAsks.Where(x => x.RepairAskId == req.RepairAskId).FirstOrDefaultAsync();

                _repairAsk.RegisterDate = Convert.ToDateTime(req.RegisterDate);
                _repairAsk.Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault();
                _repairAsk.Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault();
                _repairAsk.IsOutSourcing = req.IsOutSourcing;
                _repairAsk.Register = _register;
                _repairAsk.RepairAskMemo = req.RepairAskMemo;
                //_repairAsk.RepairTarget = req.RepairTarget;

                _db.RepairAsks.Update(_repairAsk);

                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true,
                };

                return Res;

            }
            catch (Exception ex)
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


        public async Task<Response<bool>> DeleteRepairAsk(RepairAskRequest req)
        {
            try
            {

                foreach(var i in req.RepairAskIds)
                {
                    var _ask = await _db.RepairAsks.Where(x => x.RepairAskId == i).FirstOrDefaultAsync();
                    _ask.IsDeleted = 1;

                    _db.RepairAsks.Update(_ask);
                }

                await Save();

                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true,
                };

                return Res;

            }
            catch (Exception ex)
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

        #endregion 수리요청

        #region 수리일지

  
        public async Task<Response<RepairLogResponse>> GetRepairLog(RepairLogRequest req)
        {
            try
            {
                var res = await _db.RepairAsks
                    .Where(x => x.RepairAskId == req.RepairAskId)
                    .Select(x => new RepairLogResponse
                    {
                        RepairAskId = x.RepairAskId,
                        IsOutSourcing = x.IsOutSourcing,
                        FacilityId = x.Facility != null ? x.Facility.Id : 0,
                        FacilityType = x.Facility != null ? x.Facility.CommonCode.Name : "",
                        FacilityCode = x.Facility != null ? x.Facility.Code : "",
                        FacilityName = x.Facility != null ? x.Facility.Name : "",
                        MoldId = x.Mold != null ? x.Mold.Id : 0,
                        MoldType = x.Mold != null ? x.Mold.CommonCode.Name : "",
                        MoldCode = x.Mold != null ? x.Mold.Code : "",
                        MoldName = x.Mold != null ? x.Mold.Name : "",
                        RepairAskNo = x.RepairAskNo,
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        RegisterName = x.Register.FullName,
                        RepairAskMemo = x.RepairAskMemo,


                        RepairLogId = x.RepairLog != null? x.RepairLog.RepairLogId : 0,
                        RepairResult = x.RepairLog != null ? x.RepairLog.RepairResult : _db.CommonCodes
                            .Where(y=>y.IsDeleted == false)
                            .Where(x=>x.SortCode.Code == "G0013").FirstOrDefault().Id,

                        RepairResultText = x.RepairLog != null ? _db.CommonCodes.Where(y=> y.Id== x.RepairLog.RepairResult).FirstOrDefault().Name : _db.CommonCodes
                            .Where(y => y.IsDeleted == false)
                            .Where(x => x.SortCode.Code == "G0013").FirstOrDefault().Name,

                        RepairFinishDate = x.RepairLog != null ? x.RepairLog.RepairFinishDate.ToString("yyyy-MM-dd") : DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd"),
                        PartnerId = x.RepairLog != null ? (x.RepairLog.Partner == null ? 0 : x.RepairLog.Partner.Id) : 0,
                        PartnerName = x.RepairLog != null ? (x.RepairLog.Partner == null? "" : x.RepairLog.Partner.Name) : "",

                        RepairType = x.RepairLog != null ? x.RepairLog.RepairType : _db.CommonCodes
                            .Where(y => y.IsDeleted == false)
                            .Where(x => x.SortCode.Code == "G0008").FirstOrDefault().Id,
                        RepairClassification = x.RepairLog != null ? x.RepairLog.RepairClassification : _db.CommonCodes
                            .Where(y => y.IsDeleted == false)
                            .Where(x => x.SortCode.Code == "G0009").FirstOrDefault().Id,

                        WorkerName = x.RepairLog != null ? x.RepairLog.WorkerName: "",
                        //UploadFiles = x.UploadFiles.ToArray(),
                        UploadFiles = x.RepairLog != null ? x.RepairLog.UploadFiles.ToList() : new List<UploadFile>(),


                        CauseOfRepair = x.RepairLog != null ? x.RepairLog.CauseOfRepair : "",
                        CommentOfRepair = x.RepairLog != null ? x.RepairLog.CommentOfRepair : "",

                    }).FirstOrDefaultAsync();


                var Res = new Response<RepairLogResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<RepairLogResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<bool>> CreateUpdateRepairLog(RepairLogCreateUpdateRequest req)
        {
            try
            {
                var _repairAsk = await _db.RepairAsks.Where(x => x.RepairAskId == req.RepairAskId).FirstOrDefaultAsync();

                if(_repairAsk.RepairLog == null)
                {
                    var _newRepairLog = new RepairLog
                    {
                        RepairFinishDate = Convert.ToDateTime(req.RepairFinishDate),
                        CauseOfRepair = req.CauseOfRepair,
                        CommentOfRepair = req.CommentOfRepair,
                        Partner = _db.Partners.Where(x=>x.Id == req.PartnerId).FirstOrDefault(),
                        RepairClassification = req.RepairClassification,
                        RepairResult = req.RepairResult,
                        RepairType = req.RepairType,
                        WorkerName = req.WorkerName,
                        UploadFiles = req.UploadFiles,
                    };

                    _repairAsk.RepairLog = _newRepairLog;
                    _db.RepairAsks.Update(_repairAsk);

                }
                else
                {
                    var _repairLog = _db.RepairLogs
                        .Include(x=>x.UploadFiles)
                        .Where(x => x.RepairLogId == req.RepairLogId).FirstOrDefault();
                    _repairLog.RepairFinishDate = Convert.ToDateTime(req.RepairFinishDate);
                    _repairLog.CauseOfRepair = req.CauseOfRepair;
                    _repairLog.CommentOfRepair = req.CommentOfRepair;
                    _repairLog.Partner = _db.Partners.Where(x => x.Id == req.PartnerId).FirstOrDefault();
                    _repairLog.RepairClassification = req.RepairClassification;
                    _repairLog.RepairResult = req.RepairResult;
                    _repairLog.RepairType = req.RepairType;
                    _repairLog.WorkerName = req.WorkerName;
                    if (_repairLog.UploadFiles != null)
                    {
                        _db.UploadFiles.RemoveRange(_repairLog.UploadFiles);
                    }

                    _repairLog.UploadFiles = req.UploadFiles;

                    _db.RepairLogs.Update(_repairLog);
                }

                await Save();


                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true,
                };

                return Res;

            }
            catch (Exception ex)
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


        #endregion 수리요청

        #region 수리현황
        public async Task<Response<IEnumerable<RepairLogsResponse>>> GetRepairLogs(RepairAskRequest req)
        {
            try
            {
                var res = await _db.RepairAsks
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.FacilityId == 0 ? true : (req.RepairTarget == "FACILITY" ? x.Facility.Id == req.FacilityId : true))
                    .Where(x => req.MoldId == 0 ? true : (req.RepairTarget == "MOLD" ? x.Mold.Id == req.MoldId : true))
                    .Where(x => x.RepairAskNo.Contains(req.RepairAskNo))
                    .Where(x=>x.RepairLog != null)
                    .Where(x => x.RepairLog.RepairFinishDate >= Convert.ToDateTime(req.RegisterStartDate))
                    .Where(x => x.RepairLog.RepairFinishDate <= Convert.ToDateTime(req.RegisterEndDate))
                    .Where(x => x.RepairTarget == req.RepairTarget)
                   // .Where(x => req.RepairResult == 0 ? true : x.RepairResult == _db.CommonCodes.Where(y => y.Id == req.RepairResult).FirstOrDefault().Name)
                    .Select(x => new RepairLogsResponse
                    {

                        RepairAskId = x.RepairAskId,
                        IsOutSourcing = x.IsOutSourcing,
                        FacilityType = x.Facility.CommonCode.Name,
                        FacilityCode = x.Facility.Code,
                        FacilityName = x.Facility.Name,
                        MoldType = x.Mold.CommonCode.Name,
                        MoldCode = x.Mold.Code,
                        MoldName = x.Mold.Name,
                        RepairAskNo = x.RepairAskNo,
                        RegisterDate = x.RepairLog.RepairFinishDate.ToString("yyyy-MM-dd"),
                        RegisterName = x.Register.FullName,
                        RepairAskMemo = x.RepairAskMemo,
                        RepairResult = x.RepairLog != null ? _db.CommonCodes.Where(y => y.Id == x.RepairLog.RepairResult).FirstOrDefault().Name : "",
                        RepairType = x.RepairLog != null ? _db.CommonCodes.Where(y => y.Id == x.RepairLog.RepairType).FirstOrDefault().Name : "",
                        RepairClassification = x.RepairLog != null ? _db.CommonCodes.Where(y => y.Id == x.RepairLog.RepairClassification).FirstOrDefault().Name : "",
                        CauseOfRepair = x.RepairLog != null ? x.RepairLog.CauseOfRepair : "",
                        CommentOfRepair = x.RepairLog != null ? x.RepairLog.CommentOfRepair : "",
                    }).ToListAsync();

                string repairResult = await _db.CommonCodes.Where(x => x.Id == req.RepairResult).Select(x => x.Name).FirstOrDefaultAsync();

                var res2 = res.Where(x => req.RepairResult == 0 ? true : x.RepairResult == repairResult);


                var Res = new Response<IEnumerable<RepairLogsResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<RepairLogsResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }
        #endregion 수리현황


        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }


    }
}
