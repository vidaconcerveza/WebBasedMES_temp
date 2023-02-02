using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.Mold;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InspectionRepair;
using WebBasedMES.ViewModels.Mold;

namespace WebBasedMES.Services.Repositories.MoldManage
{
    public class MoldRepository : IMoldRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoldRepository (ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task Save ()
        {
            await _db.SaveChangesAsync();
        }

        #region Mold Manage

        public async Task<Response<IEnumerable<MoldResponse002>>> GetMoldsBySearch(MoldRequest002 _req)
        {
            try
            {
                if (_req.inspectionType == "DAILY" || _req.inspectionType == "PERIOD")
                {
                    var res = await _db.Molds
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _req.MoldStatus == "ALL" ? true : x.Status == _req.MoldStatus)
                    .Where(x => x.Code.Contains(_req.SearchInput) || x.Name.Contains(_req.SearchInput) || x.Memo.Contains(_req.SearchInput))

                    .Select(x => new MoldResponse002
                    {
                        MoldId = x.Id,
                        MoldCode = x.Code,
                        MoldClassification = x.CommonCode.Name,
                        MoldName = x.Name,
                        MoldGuaranteeCount = x.GuranteeCount,
                        MoldStartCount = x.StartCount,

                        MoldCurrentCount = _db.ProcessProgresses
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.WorkOrderProducePlan.Mold.Id == x.Id)
                            .Select(y => y.ProductionQuantity)
                            .Sum(),

                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        MoldDiscardDate = x.DiscardDate.ToString("yyyy-MM-dd"),
                        PartnerName = x.Owener != null ? x.Owener.Name : "",
                        MoldMemo = x.Memo,
                        MoldStatus = x.Status,
                        MoldWeight = x.Weight,
                        MoldMaterial = x.Material,
                        MoldStandard = x.Standard,

                        MoldInspectionItems = _db.InspectionItems
                                .Where(y => y.IsDeleted == false)
                                .Where(y => y.Classify == "금형")
                                .Where(y => _req.inspectionType == "DAILY" ? y.Type == "일상점검" : y.Type == "정기점검")
                                .Select(y => new MoldInspectionItemInterface
                                {
                                    CauseOfError = "",
                                    ErrorManagementResult = "",
                                    MoldInspectionId = 0,
                                    MoldInspectionItemId = 0,
                                    InsepctionName = y.InspectionItems,
                                    InspectionCode = y.Code,
                                    InspectionCountCriteria = y.InspectionCount,
                                    InspectionItem = y.InspectionItems,
                                    InspectionItemId = y.Id,
                                    InspectionJudgement = y.JudgeStandard,
                                    InspectionMethod = y.JudgeMethod,
                                    InspectionPeriod = y.CommonCode.Name,
                                    InspectionResult = "",
                                }).ToList()
                    })
                    .ToListAsync();


                    var Res = new Response<IEnumerable<MoldResponse002>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;
                }
                else
                {
                    var res = await _db.Molds
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _req.MoldStatus == "ALL" ? true : x.Status == _req.MoldStatus)
                    .Where(x => x.Code.Contains(_req.SearchInput) || x.Name.Contains(_req.SearchInput) || x.Memo.Contains(_req.SearchInput))

                    .Select(x => new MoldResponse002
                    {
                        MoldId = x.Id,
                        MoldCode = x.Code,
                        MoldClassification = x.CommonCode.Name,
                        MoldName = x.Name,
                        MoldGuaranteeCount = x.GuranteeCount,
                        MoldStartCount = x.StartCount,
                        MoldCurrentCount = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Mold.Id == x.Id)
                            .Select(y => y.ProductionQuantity)
                            .Sum(),
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        MoldDiscardDate = x.DiscardDate.ToString("yyyy-MM-dd"),
                        PartnerName = x.Owener.Name,
                        MoldMemo = x.Memo,
                        MoldStatus = x.Status
                    })
                    .ToListAsync();


                    var Res = new Response<IEnumerable<MoldResponse002>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;
                }

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldResponse002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<MoldResponse>> GetMold (int id)
        {
            try
            {
                var _mold = await _db.Molds
                    .Include(x => x.CommonCode)
                    .Where(x => x.Id == id)
                    .Select(x => new MoldResponse
                    {
                        Id = x.Id,
                        MoldId = x.Id,
                        MoldCode = x.Code,
                        MoldName = x.Name,
                        
                        CommonCode = x.CommonCode.Code,
                        CommonCodeName = x.CommonCode.Name,
                        Code = x.Code,
                        Name = x.Name,
                        Standard = x.Standard,
                        Material = x.Material,
                        Weight = x.Weight,
                        Price = x.Price,
                        MoldCreateDate = x.MoldCreateDate.ToString("yyyy-MM-dd"),
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        PartnerCode = x.Owener != null? x.Owener.Code : "",
                        PartnerName = x.Owener != null? x.Owener.Name : "",
                        PartnerId = x.Owener!=null? x.Owener.Id : 0,
                        RegisterName = x.Registerer.FullName,
                        RegisterId = x.Registerer.Id,
                        GuranteeCount = x.GuranteeCount,
                        StartCount = x.StartCount,
                        CurrentCount = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Mold.Id == x.Id)
                            .Select(y => y.ProductionQuantity)
                            .Sum(),
                        CleaningCycle = x.CleaningCycle == null ? 0 : x.CleaningCycle.Id,
                        CleaningCycleName = x.CleaningCycle == null ? "" : x.CleaningCycle.Name,
                        Status = x.Status,
                        Memo = x.Memo,

                        UploadFile = x.UploadFile,
                        UploadFiles = x.UploadFiles.ToArray(),
                        Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,

                        DailyInspection = x.DailyInspection,
                        DailyInspectionThreshold = x.DailyInspectionThreshold,
                        RegularInspection = x.RegularInspection,
                        RegularInspectionThreshold = x.RegularInspectionThreshold,

                        DiscardRegistererName = x.DiscardRegisterer.FullName,
                        DiscardRegisterer = x.DiscardRegisterer.Id,
                        DiscardDate = x.DiscardDate.ToString("yyyy-MM-dd"),
                        DescardReason = x.DiscardReason,
                        CurrentCleaningDate = x.MoldCleanings.Where(y => y.IsDeleted == false).Count() > 0 ? x.MoldCleanings.Where(y => y.IsDeleted == false).OrderByDescending(y => y.CleaningDate).FirstOrDefault().CleaningDate.ToString("yyyy-MM-dd") : "-",

                        MoldLocationArea = x.MoldLocation == null ? "-" : x.MoldLocation.AreaName,
                        MoldLocationColumn = x.MoldLocation == null ? "-" : x.MoldLocation.Column,
                        MoldLocationRow = x.MoldLocation == null ? "-" : x.MoldLocation.Row,
                        MoldLocationComment = x.MoldLocation == null ? "-" : x.MoldLocation.Comment,
                        MoldLocationIsUsing = x.MoldLocation == null ? -1 : x.MoldLocation.IsUsing
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<MoldResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _mold
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<MoldResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<MoldResponse>>> GetMolds (int code)
        {
            try
            {
                if (code == 0) { 
                    var _mold = await _db.Molds
                        .Where(x => x.IsDeleted == false)
                        .Select(x => new MoldResponse
                        {
                            Id = x.Id,
                            CommonCode = x.CommonCode.Code,
                            CommonCodeName = x.CommonCode.Name,
                            Code = x.Code,
                            Name = x.Name,
                            Standard = x.Standard,
                            Material = x.Material,
                            Weight = x.Weight,
                            Price = x.Price,
                            MoldCreateDate = x.MoldCreateDate.ToString("yyyy-MM-dd"),
                            RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                            PartnerCode = x.Owener.Code,
                            PartnerName = x.Owener.Name,
                            RegisterName = x.Registerer.FullName,
                            RegisterId = x.Registerer.Id,
                            GuranteeCount = x.GuranteeCount,
                            StartCount = x.StartCount,
                            CurrentCount = x.CurrentCount,
                            CleaningCycle = x.CleaningCycle.Id,
                            CleaningCycleName = x.CleaningCycle.Name,
                            Status = x.Status,
                            Memo = x.Memo == null? "" : x.Memo,

                            UploadFile = x.UploadFile,
                            UploadFiles = x.UploadFiles.ToArray(),
                            Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,

                            DailyInspection = x.DailyInspection,
                            DailyInspectionThreshold = x.DailyInspectionThreshold,
                            RegularInspection = x.RegularInspection,
                            RegularInspectionThreshold = x.RegularInspectionThreshold,

                            DiscardRegistererName = x.DiscardRegisterer.FullName,
                            DiscardRegisterer = x.DiscardRegisterer.Id,
                            DiscardDate = x.DiscardDate.ToString("yyyy-MM-dd"),
                            DescardReason = x.DiscardReason,
                            CurrentCleaningDate = x.MoldCleanings.Where(y => y.IsDeleted == false).Count() > 0 ? x.MoldCleanings.Where(y => y.IsDeleted == false).OrderByDescending(y => y.CleaningDate).FirstOrDefault().CleaningDate.ToString("yyyy-MM-dd") : "-",

                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<MoldResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _mold
                    };
                    return Res;
                }
                else
                {
                    var _mold = await _db.Molds
                        .Include(x => x.CommonCode)
                        .Where(x => x.IsDeleted == false && x.CommonCode.Id == code)
                        .Select(x => new MoldResponse
                        {
                            Id = x.Id,
                            CommonCode = x.CommonCode.Code,
                            CommonCodeName = x.CommonCode.Name,
                            Code = x.Code,
                            Name = x.Name,
                            Standard = x.Standard,
                            Material = x.Material,
                            Weight = x.Weight,
                            Price = x.Price,
                            MoldCreateDate = x.MoldCreateDate.ToString("yyyy-MM-dd"),
                            RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                            PartnerCode = x.Owener.Code,
                            PartnerName = x.Owener.Name,
                            PartnerId = x.Owener.Id,
                            RegisterName = x.Registerer.FullName,
                            RegisterId = x.Registerer.Id,
                            GuranteeCount = x.GuranteeCount,
                            StartCount = x.StartCount,
                            CurrentCount = x.CurrentCount,
                            CleaningCycle = x.CleaningCycle.Id,
                            CleaningCycleName = x.CleaningCycle.Name,
                            Status = x.Status,
                            Memo = x.Memo == null? "": x.Memo,

                            UploadFile = x.UploadFile,
                            UploadFiles = x.UploadFiles.ToArray(),
                            Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,

                            DailyInspection = x.DailyInspection,
                            DailyInspectionThreshold = x.DailyInspectionThreshold,
                            RegularInspection = x.RegularInspection,
                            RegularInspectionThreshold = x.RegularInspectionThreshold,

                            DiscardRegistererName = x.DiscardRegisterer.FullName,
                            DiscardRegisterer = x.DiscardRegisterer.Id,
                            DiscardDate = x.DiscardDate.ToString("yyyy-MM-dd"),
                            DescardReason = x.DiscardReason,
                            CurrentCleaningDate = x.MoldCleanings.Where(y => y.IsDeleted == false).Count() > 0 ? x.MoldCleanings.Where(y => y.IsDeleted == false).OrderByDescending(y => y.CleaningDate).FirstOrDefault().CleaningDate.ToString("yyyy-MM-dd") : "-",

                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<MoldResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _mold
                    };
                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }



        public async Task<Response<IEnumerable<MoldResponse>>> GetMoldMasterList(MoldRequest002 _req)
        {
            try
            {

                    var _mold = await _db.Molds
                        .Where(x => x.IsDeleted == false)
                        .Where(x => (_req.SearchInput == "" || _req.SearchInput == null)? true : x.Code.Contains(_req.SearchInput) || x.Name.Contains(_req.SearchInput) || x.Memo.Contains(_req.SearchInput))
                        .Where(x => (_req.IsUsingStr == "" || _req.IsUsingStr == null)? true : _req.IsUsingStr == "Y"? (x.MoldLocation == null? false:x.MoldLocation.IsUsing == 1) : (x.MoldLocation == null ? false : x.MoldLocation.IsUsing == 0))
                        .Where(x => (_req.SearchMoldPositionInput == "" || _req.SearchMoldPositionInput == null)? true : x.MoldLocation == null? false : (x.MoldLocation.AreaName.Contains(_req.SearchMoldPositionInput)|| x.MoldLocation.Column.Contains(_req.SearchMoldPositionInput) || x.MoldLocation.Row.Contains(_req.SearchMoldPositionInput)))
                        .Where(x => _req.MoldId == 0? true : x.Id == _req.MoldId)
                        .Select(x => new MoldResponse
                        {
                            Id = x.Id,
                            MoldId = x.Id,
                            CommonCode = x.CommonCode.Code,
                            CommonCodeName = x.CommonCode.Name,
                            Code = x.Code,
                            MoldCode = x.Code,
                            Name = x.Name,
                            MoldName = x.Name,
                            Standard = x.Standard,
                            Material = x.Material,
                            Weight = x.Weight,
                            Price = x.Price,
                            MoldCreateDate = x.MoldCreateDate.ToString("yyyy-MM-dd"),
                            RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                            PartnerCode = x.Owener != null ? x.Owener.Code : "",
                            PartnerName = x.Owener != null ? x.Owener.Name : "",
                            PartnerId = x.Owener != null ? x.Owener.Id : 0,
                            RegisterName = x.Registerer.FullName,
                            RegisterId = x.Registerer.Id,
                            GuranteeCount = x.GuranteeCount,
                            StartCount = x.StartCount,
                            CurrentCount = x.CurrentCount,
                            CleaningCycle = x.CleaningCycle == null ? 0 : x.CleaningCycle.Id,
                            CleaningCycleName = x.CleaningCycle == null? "" : x.CleaningCycle.Name,
                            Status = x.Status,
                            Memo = x.Memo == null ? "" : x.Memo,

                            UploadFile = x.UploadFile,
                            UploadFiles = x.UploadFiles.ToArray(),
                            Picture = x.UploadFile == null ? "" : x.UploadFile.FileUrl,

                            DailyInspection = x.DailyInspection,
                            DailyInspectionThreshold = x.DailyInspectionThreshold,
                            RegularInspection = x.RegularInspection,
                            RegularInspectionThreshold = x.RegularInspectionThreshold,

                            DiscardRegistererName = x.DiscardRegisterer.FullName,
                            DiscardRegisterer = x.DiscardRegisterer.Id,
                            DiscardDate = x.DiscardDate.ToString("yyyy-MM-dd"),
                            DescardReason = x.DiscardReason,
                            CurrentCleaningDate = x.MoldCleanings
                                .Where(y => y.IsDeleted == false).Count() > 0 ? 
                                    x.MoldCleanings
                                        .Where(y => y.IsDeleted == false)
                                        .OrderByDescending(y => y.CleaningDate)
                                        .FirstOrDefault().CleaningDate.ToString("yyyy-MM-dd") : "-",

                            MoldLocationArea = x.MoldLocation == null? "-" : x.MoldLocation.AreaName,
                            MoldLocationColumn = x.MoldLocation == null ? "-" : x.MoldLocation.Column,
                            MoldLocationRow = x.MoldLocation == null ? "-" : x.MoldLocation.Row,
                            MoldLocationComment = x.MoldLocation == null ? "-" : x.MoldLocation.Comment,
                            MoldLocationIsUsing = x.MoldLocation == null ? -1 : x.MoldLocation.IsUsing,

                            MoldDraftRegisterDate = x.MoldDrafts
                                .Where(y => y.IsDeleted == false).Count() > 0 ?
                                    x.MoldDrafts
                                        .Where(y => y.IsDeleted == false)
                                        .OrderBy(y => y.RegisterDate)
                                        .FirstOrDefault().RegisterDate.ToString("yyyy-MM-dd") : "-",


                            MoldDraftEditedDate = x.MoldDrafts
                                .Where(y => y.IsDeleted == false).Count() > 0 ?
                                    x.MoldDrafts
                                        .Where(y => y.IsDeleted == false)
                                        .OrderByDescending(y => y.RegisterDate)
                                        .FirstOrDefault().RegisterDate.ToString("yyyy-MM-dd") : "-",




                        })
                        .ToArrayAsync();

                    var Res = new Response<IEnumerable<MoldResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _mold
                    };
                    return Res;
                
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }



        public async Task<Response<IEnumerable<MoldResponse>>> CreateMold (MoldRequest x)
        {
            try
            {
                //Duplicate Checkup

                string _code = x.Code;

                if (x.AutoCode)
                {
                    var prevCodes = await _db.Molds
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Where(y => y.Code.Substring(0, 1) == "M")
                        .Select(y => y.Code.Substring(1, 4))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();


                    if (prevCodes == null)
                    {
                        _code = "M0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "M" + (Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "M0001";
                        }
                    }
                }


                if (!x.AutoCode)
                {
                    var codeCheck = await _db.Molds
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == x.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<MoldResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = x.Code + "는 이미 존재하는 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }
                }

                var _common = await _db.CommonCodes.Where(y => y.Id == x.CommonCode && y.IsDeleted == false).FirstOrDefaultAsync();
                var _cleaning = await _db.CommonCodes.Where(y => y.Id == x.CleaningCycle && y.IsDeleted == false).FirstOrDefaultAsync();

                var _owener = await _db.Partners.Where(y => y.Id == x.PartnerId && y.IsDeleted == false).FirstOrDefaultAsync();
                var _register = await _userManager.FindByIdAsync(x.RegisterId);
                var _discardUser = await _userManager.FindByIdAsync(x.DiscardRegisterer);

                var _mold = new Mold
                {
                    //Id = x.Id,
                    CommonCode = _common,
                    Code = _code,
                    Name = x.Name,
                    Standard = x.Standard,
                    Material = x.Material,
                    Weight = x.Weight,
                    Price = x.Price,
                    MoldCreateDate = Convert.ToDateTime(x.MoldCreateDate),
                    RegisterDate = Convert.ToDateTime(x.RegisterDate),
                    Owener = _owener,
                    Registerer = _register,
                    GuranteeCount = x.GuranteeCount,
                    StartCount = x.StartCount,
                    CurrentCount = x.CurrentCount,
                    CleaningCycle = _cleaning,
                    Status = x.Status,
                    Memo = x.Memo,

                    //UploadFile = x.UploadFile,
                    //UploadFiles = x.UploadFiles.ToArray(),

                    DailyInspection = x.DailyInspection,
                    DailyInspectionThreshold = x.DailyInspectionThreshold,
                    RegularInspection = x.RegularInspection,
                    RegularInspectionThreshold = x.RegularInspectionThreshold,

                    DiscardRegisterer = _discardUser,
                    DiscardDate = Convert.ToDateTime(x.DiscardDate),
                    DiscardReason = x.DiscardReason
                };

                await _db.Molds.AddAsync(_mold);
                await Save();




                var Res = new Response<IEnumerable<MoldResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }
        public async Task<Response<IEnumerable<MoldResponse>>> UpdateMold (MoldRequest mold, int id)
        {
            try
            {
                var _common = await _db.CommonCodes.Where(y => y.Id == mold.CommonCode && y.IsDeleted == false).FirstOrDefaultAsync();
                var _owener = await _db.Partners.Where(y => y.Code == mold.Owener && y.IsDeleted == false).FirstOrDefaultAsync();
                var _register = await _userManager.FindByIdAsync(mold.RegisterId);
                var _discardUser = await _userManager.FindByIdAsync(mold.DiscardRegisterer);
                var _cleaning = await _db.CommonCodes.Where(y => y.Id == mold.CleaningCycle && y.IsDeleted == false).FirstOrDefaultAsync();


                var _mold = await _db.Molds.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (mold.Code != _mold.Code)
                {
                    var codeCheck = await _db.Molds
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == mold.Code)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<IEnumerable<MoldResponse>>()
                        {
                            IsSuccess = false,
                            ErrorMessage = mold.Code + "는 이미 사용중인 코드입니다.",
                            Data = null
                        };

                        return ErrorReturn;
                    }
                }



                _mold.CommonCode = _common;
                _mold.Code = mold.Code;
                _mold.Name = mold.Name;
                _mold.Standard = mold.Standard;
                _mold.Material = mold.Material;
                _mold.Weight = mold.Weight;
                _mold.Price = mold.Price;
                _mold.MoldCreateDate = Convert.ToDateTime(mold.MoldCreateDate); 
                _mold.RegisterDate = Convert.ToDateTime(mold.RegisterDate);
                _mold.Owener = _owener;
                _mold.Registerer = _register;
                _mold.GuranteeCount = mold.GuranteeCount;
                _mold.StartCount = mold.StartCount;
                _mold.CurrentCount = mold.CurrentCount;
                _mold.CleaningCycle = _cleaning;
                _mold.Status = mold.Status;
                _mold.Memo = mold.Memo;

                _mold.UploadFile = mold.UploadFile;
                _mold.UploadFiles = mold.UploadFiles.ToArray();

                _mold.DailyInspection = mold.DailyInspection;
                _mold.DailyInspectionThreshold = mold.DailyInspectionThreshold;
                _mold.RegularInspection = mold.RegularInspection;
                _mold.RegularInspectionThreshold = mold.RegularInspectionThreshold;

                _mold.DiscardRegisterer = _discardUser;
                _mold.DiscardDate = Convert.ToDateTime(mold.DiscardDate);
                _mold.DiscardReason = mold.DiscardReason;
                _db.Molds.Update(_mold);
                                
                await Save();

                var Res = new Response<IEnumerable<MoldResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<MoldResponse>>> DeleteMolds (int[] id)
        {
            try
            {
                List<Mold> _molds = new List<Mold>();
                foreach (var i in id)
                {
                    var _mold = await _db.Molds.Include(x => x.CommonCode).Where(x => x.Id == i).FirstOrDefaultAsync();
                    if (_mold != null)
                    {
                        _mold.IsDeleted = true;
                        _molds.Add(_mold);
                    }
                }

                if (_molds.Count > 0)
                {
                    _db.Molds.UpdateRange(_molds);
                    await Save();
                }



                var Res = new Response<IEnumerable<MoldResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }


        #endregion Mold Manage


        #region MoldGroup
        public async Task<Response<MoldGroupResponse>> GetMoldGroup (MoldGroupRequest req)
        {
            try
            {
                var _mold = await _db.MoldGroups
                    .Where(x => x.Id == req.MoldGroupId)
                    .Select(x => new MoldGroupResponse
                    {
                        MoldGroupId = x.Id,
                        MoldGroupCode = x.Code,
                        MoldGroupName = x.Name,
                        IsAuto = x.IsAuto,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo,
                        MoldGroupElements = x.MoldGroupList.Select(y => new MoldGroupInterface
                        {
                            MoldGroupElementId = y.Id,
                            FacilityId = y.Facility.Id,
                            FacilityCode = y.Facility.Code,
                            FacilityName = y.Facility.Name,
                            FacilityStandard = y.Facility.Standard,
                            FacilityType = y.Facility.CommonCode.Name,
                            MoldId = y.Mold.Id,
                            MoldCode = y.Mold.Code,
                            MoldName = y.Mold.Name,
                            MoldStandard = y.Mold.Standard,
                            MoldType = y.Mold.CommonCode.Name,
                            Memo = y.Memo,
                            IsUsing = y.IsUsing
                        })
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<MoldGroupResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _mold
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<MoldGroupResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<MoldGroupResponse>>> GetMoldGroups (MoldGroupRequest req)
        {
            try
            {
                var _mold = await _db.MoldGroups
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new MoldGroupResponse
                    {
                        MoldGroupId = x.Id,
                        MoldGroupCode = x.Code,
                        MoldGroupName = x.Name,
                        IsAuto = x.IsAuto,
                        IsUsing = x.IsUsing,
                        Memo = x.Memo,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<MoldGroupResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _mold
                    };
                    return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldGroupResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> CreateMoldGroup (MoldGroupRequest req)
        {
            try
            {
                string _code = req.MoldGroupCode;

                if (req.AutoCode)
                {
                    var prevCodes = await _db.MoldGroups
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code.Length == 5)
                        .Where(y => y.Code.Substring(0, 1) == "G")
                        .Select(y => y.Code.Substring(1, 4))
                        .OrderByDescending(y => y)
                        .FirstOrDefaultAsync();

                    if (prevCodes == null)
                    {
                        _code = "G0001";
                    }
                    else
                    {
                        try
                        {
                            _code = "G" + (Convert.ToInt32(prevCodes) + 1).ToString("0000");
                        }
                        catch
                        {
                            _code = "G0001";
                        }
                    }
                }


                if (!req.AutoCode)
                {
                    var codeCheck = await _db.MoldGroups
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == req.MoldGroupCode)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<bool>()
                        {
                            IsSuccess = false,
                            ErrorMessage = req.MoldGroupCode + "는 이미 존재하는 코드입니다.",
                            Data = false
                        };
                        return ErrorReturn;
                    }
                }

                List<MoldGroupElement> _elements = new List<MoldGroupElement>();
                foreach (var i in req.MoldGroupElements)
                {
                    var _element = new MoldGroupElement
                    {
                        Facility = _db.Facilitys.Where(z => z.Id == i.FacilityId).FirstOrDefault(),
                        Mold = _db.Molds.Where(z => z.Id == i.MoldId).FirstOrDefault(),
                        IsUsing = i.IsUsing,
                        Memo = i.Memo
                    };

                    _elements.Add(_element);
                }


                var _newmoldGroup = new MoldGroup
                {
                    Name = req.MoldGroupName,
                    Code = _code,
                    Memo = req.Memo,
                    IsAuto = req.IsAuto,
                    IsUsing = req.IsUsing,
                    MoldGroupList = _elements
                };

                var result = await _db.MoldGroups.AddAsync(_newmoldGroup);
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
        public async Task<Response<bool>> UpdateMoldGroup (MoldGroupRequest req)
        {
            try
            {
                 var moldGroup = await _db.MoldGroups
                    .Include(x => x.MoldGroupList)
                    .Where(x => x.Id == req.MoldGroupId).FirstOrDefaultAsync();

                if (moldGroup.Code != req.MoldGroupCode)
                {
                    var codeCheck = await _db.MoldGroups
                        .Where(y => y.IsDeleted == false)
                        .Where(y => y.Code == req.MoldGroupCode)
                        .FirstOrDefaultAsync();

                    if (codeCheck != null)
                    {
                        var ErrorReturn = new Response<bool>()
                        {
                            IsSuccess = false,
                            ErrorMessage = req.MoldGroupCode + "는 이미 사용중인 코드입니다.",
                            Data = false
                        };

                        return ErrorReturn;
                    }
                }

                moldGroup.Code = req.MoldGroupCode;
                moldGroup.Name = req.MoldGroupName;
                moldGroup.IsUsing = req.IsUsing;
                moldGroup.IsAuto = req.IsAuto;
                moldGroup.Memo = req.Memo;


                if (moldGroup.MoldGroupList.Count() > 0)
                {
                    _db.MoldGroupElements.RemoveRange(moldGroup.MoldGroupList);
                    moldGroup.MoldGroupList = null;
                }

                await Save();


                if (req.MoldGroupElements.Count() > 0)
                {
                    var moldGroup2 = await _db.MoldGroups
                       .Include(x => x.MoldGroupList)
                       .Where(x => x.Id == req.MoldGroupId).FirstOrDefaultAsync();

                    List<MoldGroupElement> _elements = new List<MoldGroupElement>();
                    foreach (var i in req.MoldGroupElements)
                    {
                        var _element = new MoldGroupElement
                        {
                            Facility = _db.Facilitys.Where(z => z.Id == i.FacilityId).FirstOrDefault(),
                            Mold = _db.Molds.Where(z => z.Id == i.MoldId).FirstOrDefault(),
                            IsUsing = i.IsUsing,
                            Memo = i.Memo
                        };

                        _elements.Add(_element);
                    }

                    moldGroup2.MoldGroupList = _elements;

                    _db.MoldGroups.Update(moldGroup2);

                    await Save();
                }



                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = false
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
        public async Task<Response<bool>> DeleteMoldGroups (MoldGroupRequest mold)
        {
            try
            {
                foreach(var i in mold.MoldGroupIds)
                {
                    var moldGroup = await _db.MoldGroups.Where(x => x.Id == i).FirstOrDefaultAsync();

                    moldGroup.IsDeleted = true;

                    _db.MoldGroups.Update(moldGroup);

                }

                await Save();


                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = false
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

        public async Task<Response<IEnumerable<MoldGroupInterface>>> GetMoldGroupDetail(MoldGroupRequest req)
        {
            try
            {
                var detail = await _db.MoldGroupElements
                    .Where(x => x.MoldGroup.Id == req.MoldGroupId)
                    .Select(x => new MoldGroupInterface
                    {
                        FacilityCode = x.Facility.Code,
                        FacilityName = x.Facility.Name,
                        FacilityStandard = x.Facility.Standard,
                        FacilityType = x.Facility.CommonCode.Name,
                        MoldCode = x.Mold.Code,
                        MoldName = x.Mold.Name,
                        MoldStandard = x.Mold.Standard,
                        MoldType = x.Mold.CommonCode.Name,
                        Memo = x.Memo,
                        IsUsing = x.IsUsing,
                        FacilityId = x.Facility.Id,
                        MoldId = x.Mold.Id
                    }).ToListAsync();




                var Res = new Response<IEnumerable<MoldGroupInterface>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = detail
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldGroupInterface>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }



        #endregion MoldGroup


        //금형세척 리스트...
        public async Task<Response<IEnumerable<MoldCleaningResponse>>> GetMoldCleanings(MoldCleaningRequest req)
        {
            try
            {
                var res = await _db.MoldCleanings
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Mold.Id == req.MoldId)
                    .Select(x => new MoldCleaningResponse
                    {
                        MoldCleaningId = x.Id,
                        MoldId = x.Mold.Id,
                        CleaningClassification = x.CleaningClassification,
                        CleaningClassificationName = _db.CommonCodes.Where(y => y.Id == x.CleaningClassification).FirstOrDefault().Name,
                        CleaningType = x.CleaningType,
                        CleaningTypeName = _db.CommonCodes.Where(y => y.Id == x.CleaningType).FirstOrDefault().Name,
                        CurrentCount = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Mold == x.Mold)
                            .Select(y => y.ProductionQuantity)
                            .Sum(),
                        GauranteeCount = x.Mold.GuranteeCount,
                        CleaningDate = x.CleaningDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,
                        StartCount = x.Mold.StartCount,
                        WorkerId = x.Worker.Id,
                        WorkerName = x.Worker.FullName,

                    }).ToListAsync();

                var Res = new Response<IEnumerable<MoldCleaningResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<IEnumerable<MoldCleaningResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };
                return Res;
            }
        }

        public async Task<Response<bool>> UpdateMoldCleaing(MoldCleaningRequest req)
        {
            try
            {
                var res = await _db.MoldCleanings
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Mold.Id == req.MoldId)
                    .Select(x => x.Id)
                    .ToListAsync();

                List<int> editedList = new List<int>();

                foreach (var cleaning in req.MoldCleaningList)
                {
                    if (cleaning.MoldCleaningId == 0)
                    {
                        _db.MoldCleanings.Add(new MoldCleaning
                        {
                            CleaningDate = Convert.ToDateTime(cleaning.CleaningDate),
                            CleaningClassification = cleaning.CleaningClassification,
                            CleaningType = cleaning.CleaningType,
                            CurrentCount = cleaning.CurrentCount,
                            GuaranteeCount = cleaning.GuaranteeCount,
                            Memo = cleaning.Memo,
                            Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault(),
                            StartCount = cleaning.StartCount,
                            Worker = await _userManager.FindByIdAsync(cleaning.WorkerId),
                        });
                    }
                    else
                    {
                        editedList.Add(cleaning.MoldCleaningId);

                        var oldData = _db.MoldCleanings.Where(x => x.Id == cleaning.MoldCleaningId).FirstOrDefault();
                        oldData.CleaningDate = Convert.ToDateTime(cleaning.CleaningDate);
                        oldData.Memo = cleaning.Memo;
                        oldData.Worker = await _userManager.FindByIdAsync(cleaning.WorkerId);
                        oldData.CleaningClassification = cleaning.CleaningClassification;
                        oldData.CleaningType = cleaning.CleaningType;

                        _db.MoldCleanings.Update(oldData);
                    }
                }



                bool flag = false;

  
                    foreach (int x in res)
                    {
                        flag = false;
                        foreach (int y in editedList)
                        {
                            if (x == y)
                            {
                                flag = true;
                            }
                        }

                        if (!flag)
                        {
                            var deledtedCleaning = _db.MoldCleanings.Where(z => z.Id == x).FirstOrDefault();
                            deledtedCleaning.IsDeleted = true;
                            _db.MoldCleanings.Update(deledtedCleaning);

                        }
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
            catch(Exception ex)
            {
                var Res = new Response<bool>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = false
                };
                return Res;

            }

        }




        #region Mold Draft
        public async Task<Response<IEnumerable<MoldDraftResponse>>> GetMoldDrafts(MoldDraftRequest req)
        {
            try
            {
                var res = await _db.MoldDrafts
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Mold.Id == req.MoldId)
                    .Select(x => new MoldDraftResponse
                    {
                        MoldDraftId = x.Id,
                        MoldId = x.Mold.Id,
                        DraftClassification = x.DraftClassification,
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        Memo = x.Memo,
                        RegisterId = x.Register.Id,
                        RegisterName = x.Register.FullName,
                        UploadFile = x.UploadFile,

                    }).ToListAsync();

                var Res = new Response<IEnumerable<MoldDraftResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldDraftResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString(),
                    Data = null
                };
                return Res;
            }
        }

        public async Task<Response<bool>> UpdateMoldDraft(MoldDraftRequest req)
        {
            try
            {
                var res = await _db.MoldDrafts
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Mold.Id == req.MoldId)
                    .Select(x => x.Id)
                    .ToListAsync();

                List<int> editedList = new List<int>();

                foreach (var draft in req.MoldDraftList)
                {
                    if (draft.MoldDraftId == 0)
                    {
                        _db.MoldDrafts.Add(new MoldDraft
                        {
                            RegisterDate = Convert.ToDateTime(draft.RegisterDate),
                            DraftClassification = draft.DraftClassification,
                            Memo = draft.Memo,
                            Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault(),
                            Register = await _userManager.FindByIdAsync(draft.RegisterId),
                            UploadFile = draft.UploadFile,
                        });
                    }
                    else
                    {
                        editedList.Add(draft.MoldDraftId);

                        var oldData = _db.MoldDrafts.Where(x => x.Id == draft.MoldDraftId).FirstOrDefault();
                        oldData.RegisterDate = Convert.ToDateTime(draft.RegisterDate);
                        oldData.Memo = draft.Memo;
                        oldData.Register = await _userManager.FindByIdAsync(draft.RegisterId);
                        oldData.DraftClassification = draft.DraftClassification;
                        oldData.UploadFile = draft.UploadFile;


                        _db.MoldDrafts.Update(oldData);
                    }
                }



                bool flag = false;


                    foreach (int x in res)
                    {
                        flag = false;
                        foreach (int y in editedList)
                        {
                            if (x == y)
                            {
                                flag = true;
                            }
                        }

                        if (!flag)
                        {
                            var deledtedDraft= _db.MoldDrafts.Where(z => z.Id == x).FirstOrDefault();
                            deledtedDraft.IsDeleted = true;
                            _db.MoldDrafts.Update(deledtedDraft);

                        }
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
                    ErrorMessage = ex.ToString(),
                    Data = false
                };
                return Res;

            }

        }


        #endregion Mold Draft

        #region MoldPosition
        public async Task<Response<MoldLocationResponse>> GetMoldLocation (MoldLocationRequest req)
        {
            try
            {
                var _mold = await _db.Molds
                    .Where(x => x.Id == req.MoldId)
                    .Select(x =>  new MoldLocationResponse
                    {
                        MoldLocationArea = x.MoldLocation.AreaName,
                        MoldLocationColumn = x.MoldLocation.Column,
                        MoldLocationRow = x.MoldLocation.Row,
                        MoldLocationComment = x.MoldLocation.Comment,
                        MoldLocationIsUsing = x.MoldLocation.IsUsing,
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<MoldLocationResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _mold
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<MoldLocationResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<bool>> UpdateMoldLocation (MoldLocationRequest req)
        {
            try
            {
                var _mold = await _db.Molds
                    .Include(x => x.MoldLocation)
                    .Where(x => x.Id == req.MoldId)
                    .FirstOrDefaultAsync();

                if(_mold.MoldLocation == null)
                {
                    var _moldLoc = new MoldLocation
                    {
                        AreaName = req.MoldLocationArea,
                        Column = req.MoldLocationColumn,
                        Comment = req.MoldLocationComment,
                        IsUsing = req.MoldLocationIsUsing,
                        Row = req.MoldLocationRow,

                    };

                    _mold.MoldLocation = _moldLoc;

                }
                else
                {
                    _mold.MoldLocation.AreaName = req.MoldLocationArea;
                    _mold.MoldLocation.Column = req.MoldLocationColumn;
                    _mold.MoldLocation.Comment = req.MoldLocationComment;
                    _mold.MoldLocation.IsUsing = req.MoldLocationIsUsing;
                    _mold.MoldLocation.Row = req.MoldLocationRow;

                }

                _db.Molds.Update(_mold);

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

        #region 금형사용현황

        public async Task<Response<IEnumerable<MoldUsageListResponse>>> GetMoldUsageList(MoldUsageRecordRequest req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Mold != null)
                    .Where(x => req.MoldId ==0 ?true : x.Mold.Id == req.MoldId)
                    .Where(x => x.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(req.WorkStartDate))
                    .Where(x => x.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(req.WorkEndDate).AddDays(1))
                    .Where(x => req.MoldStatusStr == "" ? true : x.Mold.Status == req.MoldStatusStr)
                    .OrderByDescending(x => x.ProcessProgress.WorkEndDateTime)
                    .Select(x => new MoldUsageListResponse
                    {
                        GuaranteeCount = x.Mold.GuranteeCount,
                        CurrentCount = _db.ProcessProgresses
                            .Where(y=>y.IsDeleted ==0)
                            .Where(y => y.WorkOrderProducePlan.Mold == x.Mold)
                            .Select(y => y.ProductionQuantity)
                            .Sum(),
                        MoldCode = x.Mold.Code,
                        MoldId = x.Mold.Id,
                        MoldName = x.Mold.Name,
                        MoldClassification = x.Mold.CommonCode.Name,
                        Status = x.Mold.Status,
                        CurrentWorkDate = x.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd")

                    }).ToListAsync();

                var res2 = res
                    .GroupBy(x => x.MoldId)
                    .Select(x => new MoldUsageListResponse
                    {
                        MoldId = x.Key,
                        GuaranteeCount = x.FirstOrDefault().GuaranteeCount,
                        CurrentCount = x.FirstOrDefault().CurrentCount,
                        MoldCode = x.FirstOrDefault().MoldCode,
                        MoldName = x.FirstOrDefault().MoldName,
                        MoldClassification = x.FirstOrDefault().MoldClassification,
                        Status = x.FirstOrDefault().Status,
                        CurrentWorkDate = x.FirstOrDefault().CurrentWorkDate
                    }).ToList();


                var res3 = await _db.FacilityOperations
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Mold != null)
                    .Where(x => req.MoldId == 0? true : x.Mold.Id == req.MoldId)
                    .Select(x => new MoldUsageListResponse
                    {
                        MoldId = x.Mold.Id,
                        GuaranteeCount = x.Mold.GuranteeCount,
                        CurrentCount = x.ProductionQuantity,
                        CurrentWorkDate = x.Date.ToString("yyyy-MM-dd"),
                        MoldClassification = x.Mold.CommonCode.Name,
                        MoldCode = x.Mold.Code,
                        MoldName = x.Mold.Name,
                        Status = x.Mold.Status
                    }).ToListAsync();

                var res4 = res3
                    .GroupBy(x => x.MoldId)
                    .Select(x => new MoldUsageListResponse
                    {
                        MoldId = x.Key,
                        GuaranteeCount = x.FirstOrDefault().GuaranteeCount,
                        CurrentCount = x.Sum(y => y.CurrentCount),
                        MoldCode = x.FirstOrDefault().MoldCode,
                        MoldName = x.FirstOrDefault().MoldName,
                        MoldClassification = x.FirstOrDefault().MoldClassification,
                        Status = x.FirstOrDefault().Status,
                        CurrentWorkDate = x.FirstOrDefault().CurrentWorkDate
                    }).ToList();

                if (res4.Count > 0)
                {
                    res2.AddRange(res4);
                }

                var res5 = res2
                    .GroupBy(x=>x.MoldId)
                    .Select(x => new MoldUsageListResponse
                    {
                        MoldId = x.Key,
                        GuaranteeCount = x.FirstOrDefault().GuaranteeCount,
                        CurrentCount = x.Sum(y => y.CurrentCount),
                        MoldCode = x.FirstOrDefault().MoldCode,
                        MoldName = x.FirstOrDefault().MoldName,
                        MoldClassification = x.FirstOrDefault().MoldClassification,
                        Status = x.FirstOrDefault().Status,
                        CurrentWorkDate = x.FirstOrDefault().CurrentWorkDate
                    }).ToList();

                var Res = new Response<IEnumerable<MoldUsageListResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res5
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldUsageListResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }



        public async Task<Response<IEnumerable<MoldUsageRecordResponse>>> GetMoldUsageRecords(MoldUsageRecordRequest req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Mold.Id == req.MoldId)
                    .Where(x => x.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(req.WorkStartDate))
                    .Where(x => x.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(req.WorkEndDate).AddDays(1))

                    .Select(x => new MoldUsageRecordResponse
                    {
                        WorkOrderNo = x.WorkerOrder.WorkOrderNo,
                        WorkOrderDate = x.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd"),
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,

                        ProduceQuantity = x.ProcessProgress.ProductionQuantity,
                        
                        DownTime = _db.ProcessNotWork
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgressId == x.ProcessProgress.ProcessProgressId)
                            .Select(y => EF.Functions.DateDiffMinute(y.ShutdownStartDateTime, y.ShutdownEndDateTime)).Sum(),

                        ElapsedTime = EF.Functions.DateDiffMinute(x.ProcessProgress.WorkStartDateTime, x.ProcessProgress.WorkEndDateTime),
                      

                    }).ToListAsync();


                var res2 = await _db.FacilityOperations
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Mold.Id == req.MoldId)
                    .Select(x => new MoldUsageRecordResponse
                    {
                        WorkOrderNo = "",
                        WorkOrderDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        DownTime = 0,
                        ElapsedTime =x.ElapsedTime,
                        ProduceQuantity = x.ProductionQuantity,
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,
                        
                    }).ToListAsync();

                if (res2.Count>0)
                {
                    res.AddRange(res2);

                }

                var Res = new Response<IEnumerable<MoldUsageRecordResponse>> ()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldUsageRecordResponse>>()
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
