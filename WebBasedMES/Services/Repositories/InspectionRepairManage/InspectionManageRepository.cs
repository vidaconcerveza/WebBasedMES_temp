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
    public class InspectionManageRepository:IInspectionManageRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public InspectionManageRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        #region 설비점검
        public async Task<Response<IEnumerable<FacilityInspectionsResponse>>> GetFacilityInspections(FacilityInspectionRequest req)
        {
            try
            {
                var res = await _db.FacilityInspections
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.FacilityId == 0 ? true : x.Facility.Id == req.FacilityId)
                    .Where(x => x.RegisterDate >= Convert.ToDateTime(req.RegisterStartDate))
                    .Where(x => x.RegisterDate <= Convert.ToDateTime(req.RegisterEndDate))
                    .Where(x => x.Type == req.InspectionType)
                    .OrderByDescending(x=>x.RegisterDate)
                    .Select(x => new FacilityInspectionsResponse
                    {
                        FacilityInspectionId = x.FacilityInspectionId,
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        FacilityCode = x.Facility.Code,
                        FacilityName = x.Facility.Name,
                        FacilityType = x.Facility.CommonCode.Name,
                        InspectionContents = x.Contents,
                        RegisterName = x.Register.FullName,
                        UploadFiles = x.UploadFiles.ToArray(),
                        FacilityMemo = x.Memo
                        
                    }).ToListAsync();

                
                var Res = new Response<IEnumerable<FacilityInspectionsResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityInspectionsResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }
        public async Task<Response<FacilityInspectionResponse>> GetFacilityInsepction(FacilityInspectionRequest req)
        {

            try
            {
                var res = await _db.FacilityInspections
                    .Include(x=>x.Register)
                    .Where(x => x.FacilityInspectionId == req.FacilityInspectionId)
                    .Select(x => new FacilityInspectionResponse
                    {
                        FacilityInspectionId = x.FacilityInspectionId,
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        FacilityCode = x.Facility.Code,
                        FacilityName = x.Facility.Name,
                        FacilityType = x.Facility.CommonCode.Name,
                        InspectionContents = x.Contents,
                        RegisterName = x.Register.FullName,
                        RegisterId = x.Register.Id,
                        UploadFiles = x.UploadFiles.ToList(),
                        FacilityMemo = x.Facility.Memo,
                        FacilityInspectionItems = x.FacilityInspectionItems
                        .Select(y => new FacilityInspectionItemInterface
                        {
                            FacilityInspectionId = x.FacilityInspectionId,
                            FacilityInspectionItemId = y.FacilityInspectionItemId,
                            CauseOfError = y.CauseOfError,
                            ErrorManagementResult = y.ErrorManagementResult,
                            InsepctionName = y.InspectionItem == null ?"" :y.InspectionItem.Name,
                            InspectionCode = y.InspectionItem == null ? "" : y.InspectionItem.Code,
                            InspectionCountCriteria = y.InspectionItem == null ? 0 : y.InspectionItem.InspectionCount,
                            InspectionItem = y.InspectionItem == null ? "" : y.InspectionItem.InspectionItems,
                            InspectionItemId = y.InspectionItem == null ? 0 : y.InspectionItem.Id,
                            InspectionJudgement = y.InspectionItem == null ? "" : y.InspectionItem.JudgeStandard,
                            InspectionMethod = y.InspectionItem == null ? "" : y.InspectionItem.JudgeMethod,
                            InspectionPeriod = y.InspectionItem == null ? "" : y.InspectionItem.CommonCode.Name,
                            InspectionResult = y.InspectionResult,
                            
                        })

                    }).FirstOrDefaultAsync();

                var Res = new Response<FacilityInspectionResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<FacilityInspectionResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<IEnumerable<FacilityInspectionItemInterface>>> GetFacilityInspectionItems(FacilityInspectionRequest req)
        {
            try
            {
                var res = await _db.FacilityInspectionItems
                    .Where(x => x.FacilityInspection.FacilityInspectionId == req.FacilityInspectionId)
                    .Select(y => new FacilityInspectionItemInterface
                    {
                        FacilityInspectionId = y.FacilityInspection.FacilityInspectionId,
                        FacilityInspectionItemId = y.FacilityInspectionItemId,
                        CauseOfError = y.CauseOfError,
                        ErrorManagementResult = y.ErrorManagementResult,
                        InsepctionName = y.InspectionItem.Name,
                        InspectionCode = y.InspectionItem.Code,
                        InspectionCountCriteria = y.InspectionItem.InspectionCount,
                        InspectionItem = y.InspectionItem.InspectionItems,
                        InspectionItemId = y.InspectionItem.Id,
                        InspectionJudgement = y.InspectionItem.JudgeStandard,
                        InspectionMethod = y.InspectionItem.JudgeMethod,
                        InspectionPeriod = y.InspectionItem.CommonCode.Name,
                        InspectionResult = y.InspectionResult
                    }).ToListAsync();

                var Res = new Response<IEnumerable<FacilityInspectionItemInterface>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FacilityInspectionItemInterface>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }



        public async Task<Response<bool>> CreateFacilityInspection(FacilityInspectionCreateUpdateRequest req)
        {
            try
            {

                List<FacilityInspectionItem> _items = new List<FacilityInspectionItem>();

                foreach(var i in req.FacilityInspectionItems)
                {
                    _items.Add(new FacilityInspectionItem
                    {
                        CauseOfError = i.CauseOfError,
                        ErrorManagementResult = i.ErrorManagementResult,
                        InspectionItem = _db.InspectionItems.Where(x=>x.Id == i.InspectionItemId).FirstOrDefault(),
                        InspectionResult = i.InspectionResult,
                    });
                }

                var facilityInspection = new FacilityInspection
                {
                    Contents = req.InspectionContents,
                    RegisterDate = Convert.ToDateTime(req.RegisterDate),
                    Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault(),
                    Register = await _userManager.FindByIdAsync(req.RegisterId),
                    UploadFiles = req.UploadFiles,
                    Type = req.InspectionType,
                    FacilityInspectionItems = _items,
                    Memo = req.FacilityMemo
                };
                _db.FacilityInspections.Add(facilityInspection);


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
        public async Task<Response<bool>> UpdateFacilityInspection(FacilityInspectionCreateUpdateRequest req)
        {
            try
            {
                foreach (var i in req.FacilityInspectionItems)
                {
                    
                    var item = _db.FacilityInspectionItems.Where(x => x.FacilityInspectionItemId == i.FacilityInspectionItemId).FirstOrDefault();

                    item.CauseOfError = i.CauseOfError;
                    item.ErrorManagementResult = i.ErrorManagementResult;
                    item.InspectionResult = i.InspectionResult;

                    _db.FacilityInspectionItems.Update(item);
                }

                var facilityInspection = _db.FacilityInspections.Where(x => x.FacilityInspectionId == req.FacilityInspectionId).FirstOrDefault();

                facilityInspection.Contents = req.InspectionContents;
                facilityInspection.RegisterDate = Convert.ToDateTime(req.RegisterDate);
                facilityInspection.Facility = _db.Facilitys.Where(x => x.Id == req.FacilityId).FirstOrDefault();
                facilityInspection.Register = await _userManager.FindByIdAsync(req.RegisterId);
                facilityInspection.UploadFiles = req.UploadFiles;
                facilityInspection.Memo = req.FacilityMemo;
                _db.FacilityInspections.Update(facilityInspection);

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
        public async Task<Response<bool>> DeleteFacilityInspection(FacilityInspectionRequest req)
        {
            try
            {
                foreach(var i in req.FacilityInspectionIds)
                {
                    var facilityInspection =await _db.FacilityInspections.Where(x => x.FacilityInspectionId == i).FirstOrDefaultAsync();

                    facilityInspection.IsDeleted = 1;
                    _db.FacilityInspections.Update(facilityInspection);
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

        #endregion 설비점검

        #region 금형점검
        public async Task<Response<IEnumerable<MoldInspectionsResponse>>> GetMoldInspections(MoldInspectionRequest req)
        {
            try
            {
                var res = await _db.MoldInspections
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.MoldId == 0 ? true : x.Mold.Id == req.MoldId)
                    .Where(x => x.RegisterDate >= Convert.ToDateTime(req.RegisterStartDate))
                    .Where(x => x.RegisterDate <= Convert.ToDateTime(req.RegisterEndDate))
                    .Where(x => x.Type == req.InspectionType)
                    .OrderByDescending(x => x.RegisterDate)

                    .Select(x => new MoldInspectionsResponse
                    {
                        MoldInspectionId = x.MoldInspectionId,
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        MoldCode = x.Mold.Code,
                        MoldName = x.Mold.Name,
                        MoldType = x.Mold.CommonCode.Name,
                        InspectionContents = x.Contents,
                        RegisterName = x.Register.FullName,
                        UploadFiles = x.UploadFiles.ToArray(),
                        MoldMemo = x.Memo
                    }).ToListAsync();


                var Res = new Response<IEnumerable<MoldInspectionsResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldInspectionsResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }
        public async Task<Response<MoldInspectionResponse>> GetMoldInsepction(MoldInspectionRequest req)
        {

            try
            {
                var res = await _db.MoldInspections
                    .Include(x => x.Register)
                    .Include(x => x.Mold).ThenInclude(x=>x.Owener)
                    .Where(x => x.MoldInspectionId == req.MoldInspectionId)
                    .Select(x => new MoldInspectionResponse
                    {
                        MoldInspectionId = x.MoldInspectionId,
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        MoldCode = x.Mold.Code,
                        MoldName = x.Mold.Name,
                        MoldType = x.Mold.CommonCode.Name,
                        InspectionContents = x.Contents,
                        RegisterName = x.Register.FullName,
                        RegisterId = x.Register.Id,
                        UploadFiles = x.UploadFiles.ToList(),
                        MoldPartnerName = x.Mold.Owener.Name,
                        MoldMaterial = x.Mold.Material,
                        MoldMemo = x.Mold.Memo,
                        MoldStandard = x.Mold.Standard,
                        MoldWeight = x.Mold.Weight,


                        MoldInspectionItems = x.MoldInspectionItems
                        .Select(y => new MoldInspectionItemInterface
                        {
                            MoldInspectionId = x.MoldInspectionId,
                            MoldInspectionItemId = y.MoldInspectionItemId,
                            CauseOfError = y.CauseOfError,
                            ErrorManagementResult = y.ErrorManagementResult,
                            InsepctionName = y.InspectionItem.Name,
                            InspectionCode = y.InspectionItem.Code,
                            InspectionCountCriteria = y.InspectionItem.InspectionCount,
                            InspectionItem = y.InspectionItem.InspectionItems,
                            InspectionItemId = y.InspectionItem.Id,
                            InspectionJudgement = y.InspectionItem.JudgeStandard,
                            InspectionMethod = y.InspectionItem.JudgeMethod,
                            InspectionPeriod = y.InspectionItem.CommonCode.Name,
                            InspectionResult = y.InspectionResult

                        })

                    }).FirstOrDefaultAsync();

                var Res = new Response<MoldInspectionResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;



            }
            catch (Exception ex)
            {
                var Res = new Response<MoldInspectionResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }

        }

        public async Task<Response<IEnumerable<MoldInspectionItemInterface>>> GetMoldInspectionItems(MoldInspectionRequest req)
        {
            try
            {
                var res = await _db.MoldInspectionItems
                    .Where(x => x.MoldInspection.MoldInspectionId == req.MoldInspectionId)
                    .Select(y => new MoldInspectionItemInterface
                    {
                        MoldInspectionId = y.MoldInspection.MoldInspectionId,
                        MoldInspectionItemId = y.MoldInspectionItemId,
                        CauseOfError = y.CauseOfError,
                        ErrorManagementResult = y.ErrorManagementResult,
                        InsepctionName = y.InspectionItem.Name,
                        InspectionCode = y.InspectionItem.Code,
                        InspectionCountCriteria = y.InspectionItem.InspectionCount,
                        InspectionItem = y.InspectionItem.InspectionItems,
                        InspectionItemId = y.InspectionItem.Id,
                        InspectionJudgement = y.InspectionItem.JudgeStandard,
                        InspectionMethod = y.InspectionItem.JudgeMethod,
                        InspectionPeriod = y.InspectionItem.CommonCode.Name,
                        InspectionResult = y.InspectionResult
                    }).ToListAsync();

                var Res = new Response<IEnumerable<MoldInspectionItemInterface>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<MoldInspectionItemInterface>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }

        public async Task<Response<bool>> CreateMoldInspection(MoldInspectionCreateUpdateRequest req)
        {
            try
            {

                List<MoldInspectionItem> _items = new List<MoldInspectionItem>();

                foreach (var i in req.MoldInspectionItems)
                {
                    _items.Add(new MoldInspectionItem
                    {
                        CauseOfError = i.CauseOfError,
                        ErrorManagementResult = i.ErrorManagementResult,
                        InspectionItem = _db.InspectionItems.Where(x => x.Id == i.InspectionItemId).FirstOrDefault(),
                        InspectionResult = i.InspectionResult,
                    });
                }

                var MoldInspection = new MoldInspection
                {
                    Contents = req.InspectionContents,
                    RegisterDate = Convert.ToDateTime(req.RegisterDate),
                    Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault(),
                    Register = await _userManager.FindByIdAsync(req.RegisterId),
                    UploadFiles = req.UploadFiles,
                    Type = req.InspectionType,
                    MoldInspectionItems = _items
                };
                _db.MoldInspections.Add(MoldInspection);


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
        public async Task<Response<bool>> UpdateMoldInspection(MoldInspectionCreateUpdateRequest req)
        {
            try
            {
                foreach (var i in req.MoldInspectionItems)
                {

                    var item = _db.MoldInspectionItems.Where(x => x.MoldInspectionItemId == i.MoldInspectionItemId).FirstOrDefault();

                    item.CauseOfError = i.CauseOfError;
                    item.ErrorManagementResult = i.ErrorManagementResult;
                    item.InspectionResult = i.InspectionResult;

                    _db.MoldInspectionItems.Update(item);
                }

                var MoldInspection = _db.MoldInspections.Where(x => x.MoldInspectionId == req.MoldInspectionId).FirstOrDefault();

                MoldInspection.Contents = req.InspectionContents;
                MoldInspection.RegisterDate = Convert.ToDateTime(req.RegisterDate);
                MoldInspection.Mold = _db.Molds.Where(x => x.Id == req.MoldId).FirstOrDefault();
                MoldInspection.Register = await _userManager.FindByIdAsync(req.RegisterId);
                MoldInspection.UploadFiles = req.UploadFiles;

                _db.MoldInspections.Update(MoldInspection);

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
        public async Task<Response<bool>> DeleteMoldInspection(MoldInspectionRequest req)
        {
            try
            {
                foreach (var i in req.MoldInspectionIds)
                {
                    var MoldInspection = await _db.MoldInspections.Where(x => x.MoldInspectionId == i).FirstOrDefaultAsync();

                    MoldInspection.IsDeleted = 1;
                    _db.MoldInspections.Update(MoldInspection);
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

        #endregion 금형점검


        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }


    }
}
