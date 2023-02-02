using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Data.Models.FacilityManage;
using WebBasedMES.Data.Models.Lots;
using WebBasedMES.Data.Models.Mold;
using WebBasedMES.Services.Repositories.FacilityManage;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.FacilityManage;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.Lots
{
    public class FacilityManageRepository : IFacilityManageRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public FacilityManageRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        public async Task<Response<FacilityBaseInfoResponse>> GetFacilityBaseInfo(FacilityBaseInfoRequest req)
        {
            try
            {
                
                var res = await _db.FacilityBaseInfos
                    .Where(x => x.FacilityBaseInfoId == req.FacilityBaseInfoId)
                    .Select(x => new FacilityBaseInfoResponse
                    {
                        FacilityBaseInfoId = x.FacilityBaseInfoId,

                        FacilityId = x.Facility != null ? x.Facility.Id : 0,
                        FacilityCode = x.Facility != null ? x.Facility.Code : "",
                        FacilityName = x.Facility != null ? x.Facility.Name : "",
                        FacilityType = x.Facility != null ? x.Facility.CommonCode.Name : "",
                        FacilityMemo = x.Facility != null ? x.Facility.Memo : "",
                        MoldId = x.Mold != null ? x.Mold.Id : 0,
                        MoldCode = x.Mold != null ? x.Mold.Code : "",
                        MoldName = x.Mold != null ? x.Mold.Name : "",
                        MoldType = x.Mold != null ? x.Mold.CommonCode.Name : "",

                        RegisterId = x.Register.Id,
                        RegisterName = x.Register.FullName,
                        MoldPositionSensor = x.MoldPositionSensor,

                        BottomDeadPoint = x.BottomDeadPoint,
                        Slide = x.Slide,
                        Ton = x.TON,

                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,

                        RegisterName2 = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.Register.FullName).FirstOrDefault(),
                        BottomDeadPointDL = _db.FacilityControls.Where(y=>y.IsDeleted == false).Where(y=>y.Facility.Id == x.Facility.Id).Select(y=>y.BottomDeadPointDL).FirstOrDefault(),
                        BottomDeadPointUL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.BottomDeadPointUL).FirstOrDefault(),
                        SlideDL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.SlideDL).FirstOrDefault(),
                        SlideUL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.SlideUL).FirstOrDefault(),

                        TonDL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.TonDL).FirstOrDefault(),
                        TonUL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.TonUL).FirstOrDefault()

                    }).FirstOrDefaultAsync();

                var Res = new Response<FacilityBaseInfoResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<FacilityBaseInfoResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<IEnumerable<FacilityBaseInfoResponse>>> GetFacilityBaseInfos(FacilityBaseInfoRequest req)
        {
            try
            {
                var res = await _db.FacilityBaseInfos
                    .Where(x => req.FacilityId == 0? true : x.Facility.Id == req.FacilityId)
                    .Where(x => req.MoldId == 0? true : x.Mold.Id == req.MoldId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new FacilityBaseInfoResponse
                    {
                        FacilityBaseInfoId = x.FacilityBaseInfoId,

                        FacilityId = x.Facility != null ?  x.Facility.Id : 0,
                        FacilityCode = x.Facility != null ? x.Facility.Code:"",
                        FacilityName = x.Facility != null ? x.Facility.Name : "",
                        FacilityType = x.Facility != null ? x.Facility.CommonCode.Name : "",
                        FacilityMemo = x.Facility != null ? x.Facility.Memo : "",
                        Temp = _db.FacilityStatus.Where(y=>y.UID == x.Facility.Uid).Select(y=> (Convert.ToInt32(y.LW000,16)/10).ToString("0.0")).FirstOrDefault(),
                        Humid = _db.FacilityStatus.Where(y=>y.UID == x.Facility.Uid).Select(y=> (Convert.ToInt32(y.DW0020,16)).ToString("0.0")).FirstOrDefault(),

                        MoldId = x.Mold != null ? x.Mold.Id : 0,
                        MoldCode = x.Mold != null ? x.Mold.Code : "",
                        MoldName = x.Mold != null ? x.Mold.Name : "",
                        MoldType = x.Mold != null ? x.Mold.CommonCode.Name : "",

                        RegisterId = x.Register.Id,
                        RegisterName = x.Register.FullName,
                        MoldPositionSensor = x.MoldPositionSensor,

                        BottomDeadPoint = x.BottomDeadPoint,
                        Slide = x.Slide,
                        Ton = x.TON,

                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,

                        RegisterName2 = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.Register.FullName).FirstOrDefault(),
                        BottomDeadPointDL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.BottomDeadPointDL).FirstOrDefault(),
                        BottomDeadPointUL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.BottomDeadPointUL).FirstOrDefault(),
                        SlideDL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.SlideDL).FirstOrDefault(),
                        SlideUL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.SlideUL).FirstOrDefault(),
                        TonDL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.TonDL).FirstOrDefault(),
                        TonUL = _db.FacilityControls.Where(y => y.IsDeleted == false).Where(y => y.Facility.Id == x.Facility.Id).Select(y => y.TonUL).FirstOrDefault()

                    }).ToListAsync();

                var Res = new Response<IEnumerable<FacilityBaseInfoResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityBaseInfoResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<bool>> CreateFacilityBaseInfo(FacilityBaseInfoRequest req)
        {
            try
            {
                var _register = await _userManager.FindByIdAsync(req.RegisterId);

                var fbi = new FacilityBaseInfo
                {
                    BottomDeadPoint = req.BottomDeadPoint,
                    TON = req.Ton,
                    Slide = req.Slide,

                    RegisterDate = Convert.ToDateTime(req.RegisterDate),
                    //UpdateDate = Convert.ToDateTime(req.RegisterDate),

                    Facility = _db.Facilitys.Where(x=>x.Id == req.FacilityId).FirstOrDefault(),
                    Mold = _db.Molds.Where(x=>x.Id == req.MoldId).FirstOrDefault(),
                    Memo = req.Memo,
                    Register = _register,
                    MoldPositionSensor = req.MoldPositionSensor,
                    
                    
                };

                await _db.FacilityBaseInfos.AddAsync(fbi);
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

        public async Task<Response<bool>> UpdateFacilityBaseInfo(FacilityBaseInfoRequest req)
        {
            try
            {
                var _register = await _userManager.FindByIdAsync(req.RegisterId);

                var _fbi = await _db.FacilityBaseInfos.Where(x => x.FacilityBaseInfoId == req.FacilityBaseInfoId).FirstOrDefaultAsync();

                _fbi.BottomDeadPoint = req.BottomDeadPoint;
                _fbi.TON = req.Ton;
                _fbi.Slide = req.Slide;

                _fbi.RegisterDate = Convert.ToDateTime(req.RegisterDate);

                _fbi.Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault();
                _fbi.Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault();
                _fbi.Memo = req.Memo;
                _fbi.Register = _register;
                _fbi.MoldPositionSensor = req.MoldPositionSensor;


                _db.FacilityBaseInfos.Update(_fbi);
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

        public async Task<Response<bool>> DeleteFacilityBaseInfo(FacilityBaseInfoRequest req)
        {
            try
            {

                foreach(var i in req.FacilityBaseInfoIds)
                {
                    var _fbi = await _db.FacilityBaseInfos.Where(x => x.FacilityBaseInfoId == i).FirstOrDefaultAsync();
                    _fbi.IsDeleted = 1;
                    _db.FacilityBaseInfos.Update(_fbi);
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


        public async Task<Response<FacilityControlResponse>> GetFacilityControl(FacilityControlRequest req)
        {
            try
            {
                var res = await _db.FacilityControls
                    .Where(x => x.FacilityControlId == req.FacilityControlId)

                    .Select(x => new FacilityControlResponse
                    {
                        FacilityControlId = x.FacilityControlId,

                        FacilityId = x != null ? x.Facility.Id : 0,
                        FacilityCode = x.Facility != null ? x.Facility.Code :"",
                        FacilityName = x.Facility != null ? x.Facility.Name :"",
                        FacilityType = x.Facility != null ? x.Facility.CommonCode.Name:"",
                        MoldId = x.Mold != null ? x.Mold.Id : 0,
                        MoldCode = x.Mold != null ? x.Mold.Code : "",
                        MoldName = x.Mold != null ? x.Mold.Name : "",
                        MoldType = x.Mold != null ? x.Mold.CommonCode.Name : "",

                        MoldPositionSensor = _db.FacilityBaseInfos.Where(y=>y.Facility.Id == x.Facility.Id).Where(y=>y.IsDeleted == 0).Select(y=>y.MoldPositionSensor).FirstOrDefault(),
                        BottomDeadPoint = _db.FacilityBaseInfos.Where(y => y.Facility.Id == x.Facility.Id).Where(y => y.IsDeleted == 0).Select(y => y.BottomDeadPoint).FirstOrDefault(),
                        Slide = _db.FacilityBaseInfos.Where(y => y.Facility.Id == x.Facility.Id).Where(y => y.IsDeleted == 0).Select(y => y.Slide).FirstOrDefault(),
                        Ton = _db.FacilityBaseInfos.Where(y => y.Facility.Id == x.Facility.Id).Where(y => y.IsDeleted == 0).Select(y => y.TON).FirstOrDefault(),

                        RegisterId = x.Register != null ? x.Register.Id : "",
                        RegisterName = x.Register != null ? x.Register.FullName : "",
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),

                        BottomDeadPointDL = x.BottomDeadPointDL,
                        BottomDeadPointUL = x.BottomDeadPointUL,
                        SlideDL = x.SlideDL,
                        SlideUL = x.SlideUL,

                        TonDL = x.TonDL,
                        TonUL = x.TonUL
                    }).FirstOrDefaultAsync();

                var Res = new Response<FacilityControlResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<FacilityControlResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }


        public async Task<Response<IEnumerable<FacilityControlResponse>>> GetFacilityControls(FacilityControlRequest req)
        {
            try
            {
                var res = await _db.FacilityControls
                    .Where(x => req.FacilityId == 0 ? true : x.Facility.Id == req.FacilityId)
                    .Where(x => req.MoldId == 0 ? true : x.Mold.Id == req.MoldId)
                    .Where(x=>x.IsDeleted == false)
                    .Select(x => new FacilityControlResponse
                    {
                        FacilityControlId = x.FacilityControlId,

                        FacilityId = x != null ? x.Facility.Id : 0,
                        FacilityCode = x.Facility != null ? x.Facility.Code : "",
                        FacilityName = x.Facility != null ? x.Facility.Name : "",
                        FacilityType = x.Facility != null ? x.Facility.CommonCode.Name : "",
                        MoldId = x.Mold != null ? x.Mold.Id : 0,
                        MoldCode = x.Mold != null ? x.Mold.Code : "",
                        MoldName = x.Mold != null ? x.Mold.Name : "",
                        MoldType = x.Mold != null ? x.Mold.CommonCode.Name : "",

                        MoldPositionSensor = _db.FacilityBaseInfos.Where(y => y.Facility.Id == x.Facility.Id).Where(y => y.IsDeleted == 0).Select(y => y.MoldPositionSensor).FirstOrDefault(),
                        BottomDeadPoint = _db.FacilityBaseInfos.Where(y => y.Facility.Id == x.Facility.Id).Where(y => y.IsDeleted == 0).Select(y => y.BottomDeadPoint).FirstOrDefault(),
                        Slide = _db.FacilityBaseInfos.Where(y => y.Facility.Id == x.Facility.Id).Where(y => y.IsDeleted == 0).Select(y => y.Slide).FirstOrDefault(),
                        Ton = _db.FacilityBaseInfos.Where(y => y.Facility.Id == x.Facility.Id).Where(y => y.IsDeleted == 0).Select(y => y.TON).FirstOrDefault(),

                        RegisterId = x.Register != null ? x.Register.Id : "",
                        RegisterName = x.Register != null ? x.Register.FullName : "",
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),

                        BottomDeadPointDL = x.BottomDeadPointDL,
                        BottomDeadPointUL = x.BottomDeadPointUL,
                        SlideDL = x.SlideDL,
                        SlideUL = x.SlideUL,

                        TonDL = x.TonDL,
                        TonUL = x.TonUL
                    }).ToListAsync();

                var Res = new Response<IEnumerable<FacilityControlResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityControlResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<bool>> CreateFacilityControl(FacilityControlRequest req)
        {
            try
            {
                var _register = await _userManager.FindByIdAsync(req.RegisterId);


                await _db.FacilityControls.AddAsync(new FacilityControl
                {
                    BottomDeadPointDL = req.BottomDeadPointDL,
                    BottomDeadPointUL = req.BottomDeadPointUL,
                    IsDeleted = false,
                    RegisterDate = Convert.ToDateTime(req.RegisterDate),
                    SlideDL = req.SlideDL,
                    SlideUL = req.SlideUL,
                    TonDL = req.TonDL,
                    TonUL = req.TonUL,
                    Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault(),
                    Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault(),
                    Register = _register
                });

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


        public async Task<Response<bool>> UpdateFacilityControl(FacilityControlRequest req)
        {
            try
            {
                var _register = await _userManager.FindByIdAsync(req.RegisterId);


                var result = await _db.FacilityControls.Where(x => x.FacilityControlId == req.FacilityControlId).FirstOrDefaultAsync();


                result.Register = _register;
                result.RegisterDate = Convert.ToDateTime(req.RegisterDate);
                result.BottomDeadPointUL = req.BottomDeadPointUL;
                result.BottomDeadPointDL = req.BottomDeadPointDL;
                result.SlideDL = req.SlideDL;
                result.SlideUL = req.SlideUL;
                result.TonDL = req.TonDL;
                result.TonUL = req.TonUL;
                result.Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault();
                result.Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault();

                //Facility Id / Mold Id 변경 금지됨.

                _db.FacilityControls.Update(result);
                

                await Save();



                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
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


        public async Task<Response<bool>> DeleteFacilityControl(FacilityControlRequest req)
        {
            try
            {

                foreach(var i in req.FacilityControlIds)
                {
                    var fc = await _db.FacilityControls.Where(x => x.FacilityControlId == i).FirstOrDefaultAsync();
                    fc.IsDeleted = true;
                    _db.FacilityControls.Update(fc);
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


        public async Task<Response<IEnumerable<FacilityErrorLogResponse>>> GetFacilityErrorLog(FacilityErrorLogRequest req)
        {
            try
            {
                var res = await _db.FacilityErrorLog
                    .Where(x => x.ErrorOccuredDate >= Convert.ToDateTime(req.LogStartDate))
                    .Where(x => x.ErrorOccuredDate <= Convert.ToDateTime(req.LogEndDate))
                    .Where(x => req.FacilityId == 0 ? true : _db.Facilitys.Where(y => y.Id == req.FacilityId).Select(y => y.Uid).FirstOrDefault() == x.UID)
                    .OrderByDescending(x => x.ErrorOccuredDate)
                    .Select(x => new FacilityErrorLogResponse
                    {
                        FacilityErrorLogId = x.FacilityErrorLogId,
                        BDP = Convert.ToInt32(x.BottomDeadPoint,16).ToString(),
                        SLIDE = Convert.ToInt32(x.Slide,16).ToString(),
                        TON = Convert.ToInt32(x.Ton,16).ToString(),
                        FacilityCode = _db.Facilitys.Where(y => y.Uid == x.UID).Select(y => y.Code).FirstOrDefault(),
                        FacilityName = _db.Facilitys.Where(y => y.Uid == x.UID).Select(y => y.Name).FirstOrDefault(),
                        MoldCode = "",
                        MoldName = "",
                        Temp = _db.FacilityStatus.Where(y => y.UID == x.UID).Select(y => (Convert.ToInt32(y.LW000, 16) / 10).ToString("0.0")).FirstOrDefault(),
                        Humid = _db.FacilityStatus.Where(y => y.UID == x.UID).Select(y => (Convert.ToInt32(y.DW0020, 16)).ToString("0.0")).FirstOrDefault(),

                        CreateDateTime = x.ErrorOccuredDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        ErrorNote = Convert.ToInt32(x.FacilityErrorCodeId, 16).ToString()
                        //ErrorNote = _db.FacilityErrorCode
                        //    .Where(y=>y.ErrorCode == Convert.ToInt32(x.FacilityErrorCodeId, 16).ToString() ).FirstOrDefault().Description

                        //    .Select(y=>y.Description).FirstOrDefault(),
                    }).ToListAsync();


                foreach(var i in res)
                {
                    i.ErrorNote = _db.FacilityErrorCode.Where(x => x.ErrorCode == i.ErrorNote).Select(x => x.Description).FirstOrDefault();
                }


                var Res = new Response<IEnumerable<FacilityErrorLogResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityErrorLogResponse>>()

                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<IEnumerable<FacilityOperationResponse>>> GetFacilityOperations(FacilityOperationRequest req)
        {
            try
            {
                var res = await _db.FacilityOperations
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.FacilityId == 0 ? true : x.Facility.Id == req.FacilityId)
                    .Where(x => req.MoldId == 0 ? true : x.Mold.Id == req.MoldId)
                    .Where(x => x.Lot.LotName.Contains(req.ProductLOT))
                    .Where(x => x.Date >= Convert.ToDateTime(req.WorkStartDate))
                    .Where(x => x.Date <= Convert.ToDateTime(req.WorkEndDate).AddDays(1))
                    .Select(x => new FacilityOperationResponse
                    {
                        FacilityOperationId = x.FacilityOperationId,
                        Date = x.Date.ToString("yyyy-MM-dd"),
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        ElapsedTime = x.ElapsedTime,
                        FacilityId = x.Facility.Id,
                        FacilityCode = x.Facility.Code,
                        FacilityName = x.Facility.Name,
                        MoldId = x.Mold != null ? x.Mold.Id : 0,
                        MoldCode = x.Mold != null? x.Mold.Code : "",
                        MoldName = x.Mold != null? x.Mold.Name : "",
                        ProductId = x.Product.Id,
                        ProductCode = x.Product.Code,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductName = x.Product.Name,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,
                        ProductionQuantity = x.ProductionQuantity,
                        WorkerId = x.Worker.Id,
                        WorkerName = x.Worker.FullName,
                        RegisterId = x.Register.Id,
                        RegisterName = x.Register.FullName,

                        ProductLOT = x.Lot.LotName,
                        Memo = x.Memo,


                    }).ToListAsync();



                var Res = new Response<IEnumerable<FacilityOperationResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityOperationResponse>>()

                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }
        public async Task<Response<FacilityOperationResponse>> GetFacilityOperation(FacilityOperationRequest req)
        {
            try
            {
                var res = await _db.FacilityOperations
                    .Where(x => x.FacilityOperationId == req.FacilityOperationId)
                    .Select(x => new FacilityOperationResponse
                    {
                        FacilityOperationId = x.FacilityOperationId,
                        Date = x.Date.ToString("yyyy-MM-dd"),
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        ElapsedTime = x.ElapsedTime,
                        FacilityId = x.Facility.Id,
                        MoldId = x.Mold != null ? x.Mold.Id:0,
                        ProductId = x.Product.Id,
                        FacilityCode = x.Facility.Code,
                        FacilityName = x.Facility.Name,
                        MoldCode = x.Mold != null ? x.Mold.Code : "",
                        MoldName = x.Mold != null ? x.Mold.Name : "",
                        ProductCode = x.Product.Code,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductName = x.Product.Name,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,
                        ProductionQuantity = x.ProductionQuantity,
                        WorkerId = x.Worker.Id,
                        WorkerName = x.Worker.FullName,
                        RegisterId = x.Register.Id,
                        RegisterName = x.Register.FullName,

                        ProductLOT = x.Lot.LotName,
                        Memo = x.Memo,

                        ProductInputItems = x.FacilityOperationInputItems
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => new OperationInputItem
                            {
                                LOSS = y.LOSS,
                                RequireQuantity = y.RequireQuantity,
                                TotalRequire = (y.LOSS + y.RequireQuantity),
                                ProductId = y.Product.Id,
                                ProductClassification = y.Product.CommonCode.Name,
                                ProductCode = y.Product.Code,
                                ProductName = y.Product.Name,   
                                ProductionQuantity = x.ProductionQuantity,
                                ProductUnit = y.Product.Unit,
                                ProductStandard = y.Product.Standard,
                                ProductLOT = String.Join(",", y.FacilityOperationInputItemStocks
                                    .Where(z => z.IsDeleted == 0)
                                    .Select(z => z.Lot.LotName).ToList()),
                                FacilityOperationInputItemId = y.FacilityOperationtInputItemId,

                                ProductInputItemStocks = y.FacilityOperationInputItemStocks
                                    .Where(k=>k.IsDeleted == 0)
                                    .Select(k=> new OperationInputItemStock
                                    {
                                        InputQuantity = k.InputQuantity,
                                        LotName = k.Lot.LotName,
                                        IsSelected = true,
                                        ProductClassification = y.Product.CommonCode.Name,
                                        ProductCode = y.Product.Code,
                                        ProductName = y.Product.Name,
                                        ProductUnit = y.Product.Unit,
                                        ProductStandard = y.Product.Standard,
                                        StockCount = _db.LotCounts
                                            .Where(l=>l.IsDeleted == 0)
                                            .Where(l=>l.Product == y.Product)
                                            .Where(l=>l.Lot.LotName == k.Lot.LotName)
                                            .Select(l => l.StoreOutCount - l.DefectiveCount - l.ConsumeCount + l.ProduceCount - l.OutOrderCount + l.ModifyCount).Sum() + k.InputQuantity,
                                    }).ToList()
                            }).ToList()
                    }).FirstOrDefaultAsync();



                foreach(var stx in res.ProductInputItems)
                {
                    var stk = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Id == stx.ProductId)

                        .Select(z => new OperationInputItemStock
                        {
                            StockCount = (z.StoreOutCount - z.DefectiveCount - z.ConsumeCount + z.ProduceCount - z.OutOrderCount + z.ModifyCount),
                            LotName = z.Lot.LotName,

                        }).ToListAsync();

                    var stk2 = stk.GroupBy(x => x.LotName)
                                .Select(x => new OperationInputItemStock
                                {
                                    LotName = x.Key,
                                    StockCount = x.Sum(k => k.StockCount),
                                    InputQuantity = 0,
                                    ProductCode = stx.ProductCode,
                                    ProductName = stx.ProductName,
                                    ProductStandard = stx.ProductStandard,
                                    ProductUnit = stx.ProductUnit,
                                    IsSelected = false,
                                    ProductClassification = stx.ProductClassification,
                                }).ToList();

                    List<OperationInputItemStock> newList = new List<OperationInputItemStock>();

                    
                    foreach(var i in stx.ProductInputItemStocks)
                    {
                        newList.Add(i);
                    }
                    
                    bool flag = false;
                    foreach(var i in stk2)
                    {
                        flag = false;
                        foreach(var j in stx.ProductInputItemStocks)
                        {
                            if(i.LotName == j.LotName)
                            {
                                flag = true;
                            }
                        }

                        if (!flag && i.StockCount>0)
                        {
                            newList.Add(i);
                        }
                    }
                    stx.ProductInputItemStocks = newList;
                }

                var Res = new Response<FacilityOperationResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<FacilityOperationResponse>()

                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }
        public async Task<Response<bool>> UpdateFacilityOperation(FacilityOperationRequest req)
        {
            try
            {
                var register = await _userManager.FindByIdAsync(req.RegisterId);
                var worker = await _userManager.FindByIdAsync(req.WorkerId);

                var op = await _db.FacilityOperations
                        .Include(x => x.FacilityOperationInputItems)
                        .ThenInclude(x => x.FacilityOperationInputItemStocks)
                        .ThenInclude(x => x.Lot)
                        .ThenInclude(x => x.LotCounts)

                        .Include(x => x.Lot)
                        .ThenInclude(x => x.LotCounts)
                        .Where(x => x.FacilityOperationId == req.FacilityOperationId)
                        .FirstOrDefaultAsync();

                op.Lot.LotName = req.ProductLOT;
                op.Lot.LotCounts.FirstOrDefault().ProduceCount = req.ProductionQuantity;

                op.Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault();

                op.Date = Convert.ToDateTime(req.Date);
                op.RegisterDate = Convert.ToDateTime(req.RegisterDate);
                op.ElapsedTime = req.ElapsedTime;
                op.Memo = req.Memo;
                op.Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault();
                op.Product = _db.Products.Where(x => x.Id == req.ProductId).FirstOrDefault();
                op.Register = register;
                op.Worker = worker;
                op.ProductionQuantity = req.ProductionQuantity;
               
                
                    foreach (var input in op.FacilityOperationInputItems)
                    {
                        input.IsDeleted = 1;

                        foreach (var stk in input.FacilityOperationInputItemStocks)
                        {
                            stk.IsDeleted = 1;
                            stk.Lot.IsDeleted = 1;
                            stk.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;
                        }
                    }
             //  op.FacilityOperationInputItems = null;
                    _db.FacilityOperations.Update(op);

              //  await Save();


                List<FacilityOperationInputItem> inputItems = new List<FacilityOperationInputItem>();
                foreach (var item in req.ProductInputItems)
                {
                    var inputPrd = await _db.Products.Where(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                    List<FacilityOperationInputItemStock> inputStocks = new List<FacilityOperationInputItemStock>();
                    foreach (var stk in item.ProductInputItemStocks)
                    {
                        if (stk.IsSelected)
                        {
                            var _lot = new LotEntity
                            {
                                LotName = stk.LotName,
                                ProcessType = "P",
                            };

                            var _lotResult = await _db.Lots.AddAsync(_lot);

                            var _lotCount = new LotCount
                            {
                                ConsumeCount = stk.InputQuantity,
                                IsDeleted = 0,
                                Product = inputPrd,
                                Lot = _lotResult.Entity,

                            };
                            await _db.LotCounts.AddAsync(_lotCount);


                            var _stk = new FacilityOperationInputItemStock
                            {
                                IsDeleted = 0,
                                InputQuantity = stk.InputQuantity,
                                Lot = _lotResult.Entity,
                            };

                            inputStocks.Add(_stk);
                        }
                    }

                    var _item = new FacilityOperationInputItem
                    {
                        IsDeleted = 0,
                        RequireQuantity = item.RequireQuantity,
                        LOSS = item.LOSS,
                        Product = _db.Products.Where(x => x.Id == item.ProductId).FirstOrDefault(),
                        FacilityOperationInputItemStocks = inputStocks,
                        FacilityOperation = op
                    };

                    inputItems.Add(_item);
                }

                _db.FacilityOperationInputItems.AddRange(inputItems);

              //  op.FacilityOperationInputItems = inputItems;

               // _db.FacilityOperations.Update(op);

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
        public async Task<Response<bool>> CreateFacilityOperation(FacilityOperationRequest req)
        {
            try
            {
                List<FacilityOperationInputItem> inputItems = new List<FacilityOperationInputItem>();
                foreach (var item in req.ProductInputItems)
                {
                    var inputPrd = await _db.Products.Where(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                    List<FacilityOperationInputItemStock> inputStocks = new List<FacilityOperationInputItemStock>();
                    foreach (var stk in item.ProductInputItemStocks)
                    {
                        if (stk.IsSelected)
                        {
                            var _lot = new LotEntity
                            {
                                LotName = stk.LotName,
                                ProcessType = "P",
                            };

                            var _lotResult = await _db.Lots.AddAsync(_lot);

                            var _lotCount = new LotCount
                            {
                                ConsumeCount = stk.InputQuantity,
                                IsDeleted = 0,
                                Product = inputPrd,
                                Lot = _lotResult.Entity,

                            };
                            await _db.LotCounts.AddAsync(_lotCount);


                            var _stk = new FacilityOperationInputItemStock
                            {
                                IsDeleted = 0,
                                InputQuantity = stk.InputQuantity,
                                Lot = _lotResult.Entity,
                            };

                            inputStocks.Add(_stk);
                        }
                    }

                    var _item = new FacilityOperationInputItem
                    {
                        IsDeleted = 0,
                        RequireQuantity = item.RequireQuantity,
                        LOSS = item.LOSS,
                        Product = _db.Products.Where(x => x.Id == item.ProductId).FirstOrDefault(),
                        FacilityOperationInputItemStocks = inputStocks
                    };

                    inputItems.Add(_item);
                }

                var lot = new LotEntity
                {
                    IsDeleted = 0,
                    LotName = req.ProductLOT,
                    ProcessType = "P",
                };

                var result = await _db.Lots.AddAsync(lot);

                var lotCnt = new LotCount
                {
                    ProduceCount = req.ProductionQuantity,
                    Lot = result.Entity,
                    IsDeleted = 0,
                    Product = _db.Products.Where(x => x.Id == req.ProductId).FirstOrDefault(),
                };

                await _db.LotCounts.AddAsync(lotCnt);

                var register = await _userManager.FindByIdAsync(req.RegisterId);
                var worker = await _userManager.FindByIdAsync(req.WorkerId);

                var newOp = new FacilityOperation
                {
                    Date = Convert.ToDateTime(req.Date),
                    IsDeleted = 0,
                    RegisterDate = Convert.ToDateTime(req.RegisterDate),
                    ElapsedTime = req.ElapsedTime,
                    Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault(),
                    Lot = result.Entity,
                    Memo = req.Memo,
                    Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault(),
                    Product = _db.Products.Where(x => x.Id == req.ProductId).FirstOrDefault(),
                    Register = register,
                    Worker = worker,
                    ProductionQuantity = req.ProductionQuantity,
                    FacilityOperationInputItems = inputItems
                };

                var result2 = await _db.FacilityOperations.AddAsync(newOp);


                await Save();



                var Res = new Response<bool>()

                {
                    IsSuccess = true,
                    ErrorMessage = "",
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
        public async Task<Response<bool>> DeleteFacilityOperation(FacilityOperationRequest req)
        {
            try
            {
                foreach(var i in req.FacilityOperationIds)
                {
                    var op = await _db.FacilityOperations
                        .Include(x => x.FacilityOperationInputItems)
                        .ThenInclude(x => x.FacilityOperationInputItemStocks)
                        .ThenInclude(x=>x.Lot)
                        .ThenInclude(x => x.LotCounts)

                        .Include(x=>x.Lot)
                        .ThenInclude(x=>x.LotCounts)
                        .Where(x => x.FacilityOperationId == i)
                        .FirstOrDefaultAsync();


                    op.IsDeleted = 1;
                    op.Lot.IsDeleted = 1;
                    op.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;

                    foreach(var input in op.FacilityOperationInputItems)
                    {
                        input.IsDeleted = 1;

                        foreach(var stk in input.FacilityOperationInputItemStocks)
                        {
                            stk.IsDeleted = 1;
                            stk.Lot.IsDeleted = 1;
                            stk.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;
                        }
                    }

                    _db.FacilityOperations.Update(op);
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
