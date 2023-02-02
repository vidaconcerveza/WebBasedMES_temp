using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public class OutStoreMngRepository : IOutStoreMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public OutStoreMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            
        }

        public async Task<Response<OutStoreRes002>> outStoreMstPop(OutStoreReq001 OutStoreReq01)
        {
            try
            {

                var _outStore = await _db.OutStores
                    .Include(x => x.OutStoreProducts).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    .Include(x => x.OutStoreProducts).ThenInclude(x => x.Product)
                    .Include(x => x.Partner)
                    .Where(x => x.OutStoreId == OutStoreReq01.outStoreId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new OutStoreRes002
                    {

                        outStoreId = x.OutStoreId,
                        UploadFiles = x.UploadFiles.ToArray(),

                        outStoreNo = x.OutStoreNo,
                        outStoreDate = x.OutStoreDate.ToString("yyyy-MM-dd"),
                        requestDeliveryDate = x.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                        outStoreStatus = x.OutStoreProducts.Where(y => y.IsDeleted == 0).Where(y => y.OutStoreStatus == "입고대기").Count() == x.OutStoreProducts.Where(y => y.IsDeleted == 0).Count() ?
                            "입고대기" : x.OutStoreProducts.Where(y => y.IsDeleted == 0).Where(y => y.OutStoreStatus == "입고완료").Count() >= x.OutStoreProducts.Where(y => y.IsDeleted == 0).Count() ? "입고완료" : "입고중",//상태

                        registerId = x.Register.Id,
                        registerName = x.Register.FullName,

                        partnerId = x.Partner.Id,
                        partnerCode = x.Partner.Code,
                        partnerName = x.Partner.Name,
                        partnerTaxInfo = x.Partner.TaxInfo,

                        contactName = x.Partner.ContactName,
                        contactEmail = x.Partner.ContactEmail,
                        telephoneNumber = x.Partner.TelephoneNumber,
                        faxNumber = x.Partner.FaxNumber,
                        outStoreMemo = x.OutStoreMemo,

                    }).FirstOrDefaultAsync();

                var Res = new Response<OutStoreRes002>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _outStore
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<OutStoreRes002>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OutStoreRes001>>> outStoreMstList(OutStoreReq001 outStoreRequest)
        {
            try
            {

                var _outStore = await _db.OutStores
                    .Include(x => x.OutStoreProducts).ThenInclude(x => x.Product).ThenInclude(x=>x.Item).ThenInclude(x=>x.CommonCode)
                    //.Include(x => x.CommonCode)
                    .Where(x => outStoreRequest.outStoreNo == "" ? true : x.OutStoreNo.Contains(outStoreRequest.outStoreNo))
                    .Where(x => outStoreRequest.outStoreStartDate == "" ? true : x.OutStoreDate >= Convert.ToDateTime(outStoreRequest.outStoreStartDate))
                    .Where(x => outStoreRequest.outStoreEndDate == "" ? true : x.OutStoreDate <= Convert.ToDateTime(outStoreRequest.outStoreEndDate))
                    .Where(x => outStoreRequest.partnerId == 0 ? true : x.Partner.Id == outStoreRequest.partnerId)
                   // .Where(x => outStoreRequest.outStoreStatus == "" ? true : false)
                    .Where(x => outStoreRequest.productId == 0 ? true :

                        x.OutStoreProducts.Where(x => x.Product.Id == outStoreRequest.productId)
                        .Select(x => x.Product.Id).ToArray()
                        .Contains(outStoreRequest.productId)
                    )
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x => x.OutStoreNo).Reverse() //내림차순
                    .Select(x => new OutStoreRes001
                    {

                        outStoreId = x.OutStoreId,
                        outStoreNo = x.OutStoreNo, //발주번호
                        outStoreDate = x.OutStoreDate.ToString("yyyy-MM-dd"), //발주일
                        requestDeliveryDate = x.RequestDeliveryDate.ToString("yyyy-MM-dd"), //남품요청일
                        partnerName = x.Partner.Name, // 거래처이름
                        partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                        outStoreProductCount = x.OutStoreProducts.Where(y=>y.IsDeleted == 0).Select(x => x).Count(),
                        outStoreSupplyPrice = x.OutStoreProducts.Where(y=>y.IsDeleted == 0).Select(x => x).Sum(x => x.ProductOutStoreCount*x.ProductBuyPrice),

                        outStoreTaxPrice = x.OutStoreProducts.Where(y => y.IsDeleted == 0).Select(x => x.ProductTaxInfo == "과세" ? x.ProductBuyPrice * x.ProductOutStoreCount / 10 : 0).Sum(),
                        outStoreTotalPrice = x.OutStoreProducts.Where(y => y.IsDeleted == 0).Select(x => x.ProductTaxInfo == "과세" ? x.ProductBuyPrice * x.ProductOutStoreCount / 10 * 11 : x.ProductBuyPrice * x.ProductOutStoreCount).Sum(),

                        outStoreStatus = x.OutStoreProducts.Where(y=>y.IsDeleted == 0).Where(y=>y.OutStoreStatus == "입고대기").Count() ==  x.OutStoreProducts.Where(y => y.IsDeleted == 0).Count() ? 
                            "입고대기" : x.OutStoreProducts.Where(y => y.IsDeleted == 0).Where(y => y.OutStoreStatus == "입고완료").Count() >= x.OutStoreProducts.Where(y => y.IsDeleted == 0).Count() ? "입고완료" : "입고중",//상태

                        registerName = x.Register.FullName,//등록자
                        outStoreMemo = x.OutStoreMemo,//비고
                        uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                    }).ToListAsync();

                var _filteredOutStores = _outStore.Where(x => outStoreRequest.outStoreStatus ==""? true : x.outStoreStatus == outStoreRequest.outStoreStatus).ToList();
                

                var Res = new Response<IEnumerable<OutStoreRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _filteredOutStores
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutStoreRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<OutStoreRes003>>> outStoreProductList(OutStoreReq001 OutStoreReq001)
        {
            try
            {

                var result = await _db.OutStoreProducts
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.OutStore).ThenInclude(x => x.CommonCode)
                    .Where(x => x.OutStore.OutStoreId == OutStoreReq001.outStoreId)
                    .Where(x=>x.IsDeleted == 0)
                    .OrderBy(x => x.Product.Code) //오름차순

                    .Select(x => new OutStoreRes003
                    {

                        outStoreProductId = x.OutStoreProductId,
                        outStoreId = x.OutStore.OutStoreId,
                        productId = x.Product.Id,

                        taskStatus = x.OutStoreStatus,
                        productCode = x.Product.Code,
                        productClassification = x.Product.CommonCode != null ? x.Product.CommonCode.Name : "",
                        productName = x.Product.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,

                        productStandardUnit = x.ProductStandardUnit,

                        productTaxInfo = x.ProductTaxInfo == "과세" ? x.ProductTaxInfo : "비과세",
                        productOutStoreCount = x.ProductOutStoreCount,

                        productBuyPrice = x.ProductBuyPrice,
                        //productSupplyPrice = x.OutStoreCount * x.BuyPrice,
                        productSupplyPrice = x.ProductBuyPrice * x.ProductOutStoreCount,
                        //productTaxPrice = ((x.OutStoreCount * x.BuyPrice) * 0.1),
                        productTaxPrice = x.ProductTaxInfo == "과세" ? Convert.ToInt32(x.ProductBuyPrice * x.ProductOutStoreCount * 0.1) : 0,
                        //productTotalPrice = (x.OutStoreCount * x.BuyPrice) + ((x.OutStoreCount * x.BuyPrice) * 0.1),
                        productTotalPrice = x.ProductTaxInfo == "과세" ? Convert.ToInt32((x.ProductBuyPrice * x.ProductOutStoreCount * 1.1)) : Convert.ToInt32(x.ProductBuyPrice * x.ProductOutStoreCount),
                        outStoreProductMemo = x.OutStoreProductMemo,
                        productImportCheck = x.Product.ImportCheck,

                        uploadFileId = x.Product.UploadFile != null ? x.Product.UploadFile.Id : 0,
                        uploadFileName = x.Product.UploadFile != null ? x.Product.UploadFile.FileName : "",
                        uploadFileUrl = x.Product.UploadFile != null ? x.Product.UploadFile.FileUrl : "",

                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<OutStoreRes003>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutStoreRes003>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OutStoreRes004>>> outStoreProductListPop(OutStoreReq001 OutStoreReq001)
        {
            try
            {

                var result = await _db.OutStoreProducts
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.OutStore).ThenInclude(x => x.CommonCode)
                    .Where(x => x.OutStore.OutStoreId == OutStoreReq001.outStoreId)
                    .Where(x=>x.IsDeleted==0)
                    .OrderBy(x => x.Product.Code)
                    .Select(x => new OutStoreRes004
                    {

                        outStoreProductId = x.OutStoreProductId,
                        outStoreId = x.OutStore.OutStoreId,
                        productId = x.Product.Id,

                        productCode = x.Product.Code,
                        productClassification = x.Product.CommonCode != null ? x.Product.CommonCode.Name : "",
                        productName = x.Product.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,

                        productStandardUnit = x.ProductStandardUnit,
                        productStandardUnitCount = x.ProductStandardUnitCount,

                        productTaxInfo = x.ProductTaxInfo == "과세" ? "과세": "비과세",
                        productOutStoreCount = x.ProductOutStoreCount,
                        
                        productBuyPrice = x.ProductBuyPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        productTotalPrice = x.ProductTotalPrice,
                        outStoreProductMemo = x.OutStoreProductMemo,
                        productImportCheck = x.Product.ImportCheck,

                        uploadFileId = x.Product.UploadFile != null ? x.Product.UploadFile.Id : 0,
                        uploadFileName = x.Product.UploadFile != null ? x.Product.UploadFile.FileName : "",
                        uploadFileUrl = x.Product.UploadFile != null ? x.Product.UploadFile.FileUrl : "",


                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<OutStoreRes004>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutStoreRes004>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        ////public async Task<Response<IEnumerable<OutStoreRes005>>> productList(OutStoreReq002 OutStoreReq002)
        ////{
        ////    try
        ////    {

        ////        var result = await _db.Products
        ////            .Include(x => x.CommonCode)
        ////            .Include(x => x.UploadFile)
        ////            .Include(x => x.Item).ThenInclude(x => x.CommonCode)
        ////            .Where(x => x.Item != null)
        ////            .Where(x => (OutStoreReq002.productIsUsingStr == "ALL" || OutStoreReq002.productIsUsingStr == "") ? true : (OutStoreReq002.productIsUsingStr == "Y" ? x.IsUsing == true : x.IsUsing == false))
        ////            .Where(x => OutStoreReq002.productCode == "" ? true : OutStoreReq002.productCode == x.Code)
        ////            .Where(x => OutStoreReq002.productName == "" ? true : OutStoreReq002.productName == x.Name)
        ////            .Where(x => OutStoreReq002.productClassification == "" ? true : OutStoreReq002.productClassification == x.Standard)

        ////            .Where(x => !x.IsDeleted)
        ////            .Select(x => new OutStoreRes005
        ////            {
        ////                productId = x.Id,
        ////                uploadFileId = x.UploadFile != null ? x.UploadFile.Id : 0,
        ////                uploadFileName = x.UploadFile != null ? x.UploadFile.FileName : "",
        ////                uploadFileUrl = x.UploadFile != null ? x.UploadFile.FileUrl : "",

        ////                productCode = x.Code,
        ////                productClassification = x.CommonCode.Name,
        ////                productName = x.Name,
        ////                productStandard = x.Standard,
        ////                productUnit = x.Unit,
        ////                productTaxInfo = x.Item.TaxType,
        ////                productImportCheck = x.Item.ImportCheck,
        ////                productMemo = x.Memo,
        ////                productIsUsing = x.IsUsing

        ////            }).ToListAsync();

        ////        var Res = new Response<IEnumerable<OutStoreRes005>>()
        ////        {
        ////            IsSuccess = true,
        ////            ErrorMessage = "",
        ////            Data = result
        ////        };

        ////        return Res;

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        var Res = new Response<IEnumerable<OutStoreRes005>>()
        ////        {
        ////            IsSuccess = false,
        ////            ErrorMessage = ex.Message.ToString(),
        ////            Data = null
        ////        };

        ////        return Res;
        ////    }
        ////}

        ////public async Task<Response<IEnumerable<OutStoreRes006>>> userList(OutStoreReq003 OutStoreReq003)
        ////{
        ////    try
        ////    {

        ////        var result = await _db.Users
        ////            .Where(x => (OutStoreReq003.userIsApprovedStr == "ALL" || OutStoreReq003.userIsApprovedStr == "") ? true : (OutStoreReq003.userIsApprovedStr == "Y" ? x.IsApproved == true : (OutStoreReq003.userIsApprovedStr == "N" ? x.IsApproved == false : false)))
        ////            .Where(x => OutStoreReq003.searchInput == "" ? true :

        ////             x.IdNumber.Contains(OutStoreReq003.searchInput) ||
        ////             x.FullName.Contains(OutStoreReq003.searchInput) ||
        ////             x.UserRole.Name.Contains(OutStoreReq003.searchInput) ||
        ////             x.Department.Name.Contains(OutStoreReq003.searchInput) ||
        ////             x.Position.Name.Contains(OutStoreReq003.searchInput) ||
        ////             x.Email.Contains(OutStoreReq003.searchInput) ||
        ////             x.PhoneNumber.Contains(OutStoreReq003.searchInput)


        ////            )
        ////            .Select(x => new OutStoreRes006
        ////            {
        ////                userId = x.Id,
        ////                userNo = x.IdNumber,//이름
        ////                userRole = x.UserRole.Name,//권한
        ////                userDepartment = x.Department.Name,//부서
        ////                userPosition = x.Position.Name,//직급
        ////                userEmail = x.Email,//이메일
        ////                userPhoneNumber = x.PhoneNumber, //연락처
        ////                userIsApproved = x.IsApproved,//승인여부

        ////            }).ToListAsync();
        ////        //.ToListAsync();

        ////        var Res = new Response<IEnumerable<OutStoreRes006>>()
        ////        {
        ////            IsSuccess = true,
        ////            ErrorMessage = "",
        ////            Data = result
        ////        };

        ////        return Res;

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        var Res = new Response<IEnumerable<OutStoreRes006>>()
        ////        {
        ////            IsSuccess = false,
        ////            ErrorMessage = ex.Message.ToString(),
        ////            Data = null
        ////        };

        ////        return Res;
        ////    }
        ////}

        ////public async Task<Response<IEnumerable<OutStoreRes007>>> partnerList(OutStoreReq004 OutStoreReq004)
        ////{
        ////    try
        ////    {
        ////        var _partner = await _db.Partners
        ////            .Where(x => x.IsDeleted == false)
        ////            .Where(x => (OutStoreReq004.partnerStatusStr == "ALL" || OutStoreReq004.partnerStatusStr == null) ? true : (OutStoreReq004.partnerStatusStr == "Y" ? x.IsUsing == true : x.IsUsing == false))
        ////            .Where(x => OutStoreReq004.searchInput == null ? true :

        ////            x.CommonCode.Name.Contains(OutStoreReq004.searchInput) ||
        ////             x.Code.Contains(OutStoreReq004.searchInput) ||
        ////             x.Name.Contains(OutStoreReq004.searchInput) ||
        ////             x.BusinessNumber.Contains(OutStoreReq004.searchInput) ||
        ////             x.President.Contains(OutStoreReq004.searchInput) ||
        ////             x.TelephoneNumber.Contains(OutStoreReq004.searchInput) ||
        ////             x.ContactName.Contains(OutStoreReq004.searchInput) ||
        ////             x.FaxNumber.Contains(OutStoreReq004.searchInput)

        ////            )
        ////            .Select(x => new OutStoreRes007
        ////            {
        ////                partnerId = x.Id,
        ////                partnerGroup = x.CommonCode.Name,
        ////                partnerCode = x.Code,
        ////                partnerName = x.Name,
        ////                presidentName = x.President,
        ////                businessNumber = x.BusinessNumber,
        ////                telephoneNumber = x.TelephoneNumber,
        ////                faxNumber = x.FaxNumber,
        ////                partnerMemo = x.Memo,
        ////                partnerStatus = x.IsUsing,
        ////            })
        ////            .ToArrayAsync();

        ////        var Res = new Response<IEnumerable<OutStoreRes007>>()
        ////        {
        ////            IsSuccess = true,
        ////            ErrorMessage = "",
        ////            Data = _partner
        ////        };
        ////        return Res;

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        var Res = new Response<IEnumerable<OutStoreRes007>>()
        ////        {
        ////            IsSuccess = false,
        ////            ErrorMessage = ex.Message.ToString(),
        ////            Data = null
        ////        };

        ////        return Res;
        ////    }
        ////}

        public async Task<Response<int>> CreateOutStore(OutStoreRequestCrud outStoreRequest)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(outStoreRequest.registerId);
                var _partner = await _db.Partners.Where(y => y.Id == outStoreRequest.partnerId).FirstOrDefaultAsync();
                var _commonCode = await _db.CommonCodes.Where(y => y.Id == outStoreRequest.outStoreStatusId).FirstOrDefaultAsync();

                DateTime OutStoreDate = DateTime.ParseExact(outStoreRequest.outStoreDate, "yyyy-MM-dd", null);
                DateTime RequestDeliveryDate = DateTime.ParseExact(outStoreRequest.requestDeliveryDate, "yyyy-MM-dd", null);
                
                var lastOrder = await _db.OutStores.Where(x=>x.OutStoreDate == OutStoreDate).OrderByDescending(x => x.OutStoreNo).FirstOrDefaultAsync();

                var _uploadFile = await _db.UploadFiles.Where(y => y.Id == outStoreRequest.uploadFileId).FirstOrDefaultAsync();


                string formatNo = "PO" + OutStoreDate.ToString("yyyyMMdd");
                var _storeNumberFormat = string.Format(formatNo + "{0:0000#}", (lastOrder == null? 1 : Convert.ToInt32(lastOrder.OutStoreNo.Substring(lastOrder.OutStoreNo.Length - 5))+1 ));

                var _outStore = new OutStore()
                {
                    OutStoreNo = _storeNumberFormat, //발주번호
                    OutStoreDate = OutStoreDate, //발주일
                    RequestDeliveryDate = RequestDeliveryDate, //납품요청일
                    Partner = _partner, //거래처
                    OutStoreMemo = outStoreRequest.outStoreMemo, //비고
                    //CommonCode = _commonCode, //발주상태
                    OutStoreFinish = "입고대기",
                    UploadFiles = outStoreRequest.UploadFiles, //업로드파일
                    //IsUsing = outStoreRequest.outStoreIsUsing,//사용중
                    //IsDeleted = false,//삭제여부
                    //CreateDate = DateTime.Now, //생성일
                    IsDeleted = outStoreRequest.isDeleted,
                    Register = _user //등록자
                    

                };

                var result = await _db.OutStores.AddAsync(_outStore);
                await Save();

               // int outStoreId = await _db.OutStores.Where(x => x.OutStoreNo == _storeNumberFormat).Select(x => x.OutStoreId).FirstOrDefaultAsync();

                var Res = new Response<int>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result.Entity.OutStoreId,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<int>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = -1,
                };
                return Res;
            }
        }

        public async Task<Response<bool>> UpdateOutStore(OutStoreRequestCrud outStoreRequest)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(outStoreRequest.registerId);
                var _partner = await _db.Partners.Where(y => y.Id == outStoreRequest.partnerId).FirstOrDefaultAsync();
               // var _uploadFile = await _db.UploadFiles.Where(y => y.Id == outStoreRequest.uploadFileId).FirstOrDefaultAsync();
                DateTime OutStoreDate = DateTime.ParseExact(outStoreRequest.outStoreDate, "yyyy-MM-dd", null);
                DateTime RequestDeliveryDate = DateTime.ParseExact(outStoreRequest.requestDeliveryDate, "yyyy-MM-dd", null);





                var _outStore = await _db.OutStores.Include(x=>x.UploadFiles).Where(x => x.OutStoreId == outStoreRequest.outStoreId).FirstOrDefaultAsync();

                _outStore.OutStoreNo = outStoreRequest.outStoreNo; //발주번호
                _outStore.OutStoreDate = OutStoreDate; //발주일
                _outStore.RequestDeliveryDate = RequestDeliveryDate; //납품요청일
                _outStore.Partner = _partner; //거래처
                _outStore.OutStoreMemo = outStoreRequest.outStoreMemo; //비고
              
                if(_outStore.UploadFiles != null)
                {
                    _db.UploadFiles.RemoveRange(_outStore.UploadFiles);
                }

                _outStore.UploadFiles = outStoreRequest.UploadFiles;

                _outStore.Register = _user; //등록자
                
                _db.OutStores.Update(_outStore);

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

        public async Task<Response<bool>> DeleteOutStore(OutStoreRequestCrud outStoreRequest)
        {
            try
            {

                foreach (int item in outStoreRequest.outStoreIdArray)
                {
                    //var subItem = _db.OutStoreProducts.Where(x => x.OutStore.OutStoreId == item).ToArrayAsync();
                    var subItem = await _db.OutStoreProducts.Where(x => x.OutStore.OutStoreId == item).ToListAsync();

                        foreach (OutStoreProduct sub in subItem)
                    {
                        sub.IsDeleted = 1;
                    }
                    _db.OutStoreProducts.UpdateRange(subItem);
                    //_db.OutStoreProducts.RemoveRange(subItem.Result);
                    //_db.OutStoreProducts.UpdateRange(subItem.Result);
                    var _outStores = await _db.OutStores.FindAsync(item);
                    var mst = await _db.OutStores.FindAsync(item);
                    mst.IsDeleted = 1;
                    _db.OutStores.Update(mst);

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

        public async Task<Response<bool>> CreateOutStoreProduct(OutStoreProductRequestCrud outStoreProduct)
        {
            try
            {

                var _outStore = await _db.OutStores.Where(x => x.OutStoreId == outStoreProduct.outStoreId).FirstOrDefaultAsync();

                foreach(var i in outStoreProduct.outStoreProductArray)
                {
                    var _Item = await _db.Products.Where(x => x.Id == i.productId).FirstOrDefaultAsync();

                    var _outStoreProduct = new OutStoreProduct()
                    {
                        OutStore = _outStore,
                        Product = _Item,

                        ProductStandardUnit = i.productStandardUnit,
                        ProductStandardUnitCount = i.productStandardUnitCount,
                        ProductTaxInfo = i.productTaxInfo,
                        ProductOutStoreCount = i.productOutStoreCount,
                        ProductBuyPrice = i.productBuyPrice,
                        ProductSupplyPrice = i.productSupplyPrice,
                        ProductTaxPrice = i.productTaxPrice,
                        ProductTotalPrice = i.productTaxPrice,
                        OutStoreProductMemo = i.outStoreProductMemo,
                    };
                    await _db.OutStoreProducts.AddAsync(_outStoreProduct);
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

        public async Task<Response<bool>> DeleteOutStoreProduct(OutStoreProductRequestCrud OutStoreProductRequest)
        {
            try
            {
                foreach (int item in OutStoreProductRequest.outStoreProductIdArray)
                {
                    var _items = await _db.OutStoreProducts.FindAsync(item);
                    _items.IsDeleted = 1;
                    _db.OutStoreProducts.Update(_items); ;
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
                    ErrorMessage = "",
                    Data = false,
                };
                return Res;
            }
        }

        public async Task<Response<bool>> UpdateOutStoreProduct(OutStoreProductRequestCrud OutStoreProductRequest)
        {
            try
            {
                var _outStore = await _db.OutStores.Where(x => x.OutStoreId == OutStoreProductRequest.outStoreId).FirstOrDefaultAsync();
                
               foreach(var i in OutStoreProductRequest.outStoreProductArray)
                {
                    var _Item = await _db.Products.Where(x => x.Id == i.productId).FirstOrDefaultAsync();
                    if(i.outStoreProductId != 0)
                    {
                        var _outStoreProduct = await _db.OutStoreProducts.Where(x => x.OutStoreProductId == i.outStoreProductId).FirstOrDefaultAsync();

                            _outStoreProduct.OutStore = _outStore;
                            _outStoreProduct.Product = _Item;
                            _outStoreProduct.ProductStandardUnit = i.productStandardUnit;
                            _outStoreProduct.ProductStandardUnitCount = i.productStandardUnitCount;
                            _outStoreProduct.ProductTaxInfo = i.productTaxInfo;
                            _outStoreProduct.ProductOutStoreCount = i.productOutStoreCount;
                            _outStoreProduct.ProductBuyPrice = i.productBuyPrice;
                            _outStoreProduct.ProductSupplyPrice = i.productSupplyPrice;
                            _outStoreProduct.ProductTaxPrice = i.productTaxPrice;
                            _outStoreProduct.ProductTotalPrice = i.productTaxPrice;
                            _outStoreProduct.OutStoreProductMemo = i.outStoreProductMemo;
                            _outStoreProduct.IsDeleted = i.isDeleted;
                        
                        _db.OutStoreProducts.Update(_outStoreProduct);
                    }
                    else
                    {
                        var _outStoreProduct = new OutStoreProduct()
                        {
                            OutStore = _outStore,
                            Product = _Item,

                            ProductStandardUnit = i.productStandardUnit,
                            ProductStandardUnitCount = i.productStandardUnitCount,
                            ProductTaxInfo = i.productTaxInfo,
                            ProductOutStoreCount = i.productOutStoreCount,
                            ProductBuyPrice = i.productBuyPrice,
                            ProductSupplyPrice = i.productSupplyPrice,
                            ProductTaxPrice = i.productTaxPrice,
                            ProductTotalPrice = i.productTaxPrice,
                            OutStoreProductMemo = i.outStoreProductMemo,
                        };
                        await _db.OutStoreProducts.AddAsync(_outStoreProduct);
                    }

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

        public async Task<Response<bool>> OutStoreProductEditSave(OutStoreProductRequestCrud OutStoreProductRequestCrud)
        {
            try
            {

                /*
                var _user = await _userManager.FindByIdAsync(OutStoreProductRequestCrud.outStore.registerId);
                var _partner = await _db.Partners.Where(y => y.Id == OutStoreProductRequestCrud.outStore.partnerId).FirstOrDefaultAsync();
                var _uploadFile = await _db.UploadFiles.Where(y => y.Id == OutStoreProductRequestCrud.outStore.uploadFileId).FirstOrDefaultAsync();
                DateTime OutStoreDate = DateTime.ParseExact(OutStoreProductRequestCrud.outStore.outStoreDate, "yyyy-MM-dd", null);
                DateTime RequestDeliveryDate = DateTime.ParseExact(OutStoreProductRequestCrud.outStore.requestDeliveryDate, "yyyy-MM-dd", null);

                var _outStore = new OutStore()
                {
                    OutStoreId = OutStoreProductRequestCrud.outStore.outStoreId, //발주
                    OutStoreNo = OutStoreProductRequestCrud.outStore.outStoreNo, //발주번호
                    OutStoreDate = OutStoreDate, //발주일
                    RequestDeliveryDate = RequestDeliveryDate, //납품요청일
                    Partner = _partner, //거래처
                    OutStoreMemo = OutStoreProductRequestCrud.outStore.outStoreMemo, //비고
                    UploadFile = _uploadFile, //업로드파일
                    IsDeleted = OutStoreProductRequestCrud.outStore.isDeleted,
                    Register = _user //등록자
                };
                _db.OutStores.Update(_outStore);

                
                foreach (OutStoreProduct item in outStoreProducts)
                {
                    item.IsDeleted = 1;
                    _db.OutStoreProducts.Update(item);
                }
                */

                var _outStore = await _db.OutStores.Where(x => x.OutStoreId == OutStoreProductRequestCrud.outStore.outStoreId).FirstOrDefaultAsync();
                var outStoreProducts = await _db.OutStoreProducts.Where(x => x.OutStore.OutStoreId == OutStoreProductRequestCrud.outStore.outStoreId).ToListAsync();

                bool flag = false;

                foreach (OutStoreProductRequestCrud OutStoreProductRequest in OutStoreProductRequestCrud.outStoreProductArray)
                {
                    var _Item = await _db.Products.Where(x => x.Id == OutStoreProductRequest.productId).FirstOrDefaultAsync();
                    

                    if(OutStoreProductRequest.outStoreProductId != 0)
                    {
                        var _outStoreProduct = _db.OutStoreProducts.Where(x => x.OutStoreProductId == OutStoreProductRequest.outStoreProductId).FirstOrDefault();
                        //_outStoreProduct.OutStore = _outStore;
                        _outStoreProduct.Product = _Item;
                        _outStoreProduct.ProductStandardUnit = OutStoreProductRequest.productStandardUnit;
                        _outStoreProduct.ProductStandardUnitCount = OutStoreProductRequest.productStandardUnitCount;
                        _outStoreProduct.ProductTaxInfo = OutStoreProductRequest.productTaxInfo;
                        _outStoreProduct.ProductOutStoreCount = OutStoreProductRequest.productOutStoreCount;
                        _outStoreProduct.ProductBuyPrice = OutStoreProductRequest.productBuyPrice;
                        _outStoreProduct.ProductSupplyPrice = OutStoreProductRequest.productSupplyPrice;
                        _outStoreProduct.ProductTaxPrice = OutStoreProductRequest.productTaxPrice;
                        _outStoreProduct.ProductTotalPrice = OutStoreProductRequest.productTotalPrice;
                        _outStoreProduct.OutStoreProductMemo = OutStoreProductRequest.outStoreProductMemo;
                        _outStoreProduct.IsDeleted = OutStoreProductRequest.isDeleted;

                        _db.OutStoreProducts.Update(_outStoreProduct);
                    }
                    else
                    {
                        _db.OutStoreProducts.Add(new OutStoreProduct
                        {
                            OutStore = _outStore,
                            Product = _Item,
                            ProductStandardUnit = OutStoreProductRequest.productStandardUnit,
                            ProductStandardUnitCount = OutStoreProductRequest.productStandardUnitCount,
                            ProductTaxInfo = OutStoreProductRequest.productTaxInfo,
                            ProductOutStoreCount = OutStoreProductRequest.productOutStoreCount,
                            ProductBuyPrice = OutStoreProductRequest.productBuyPrice,
                            ProductSupplyPrice = OutStoreProductRequest.productSupplyPrice,
                            ProductTaxPrice = OutStoreProductRequest.productTaxPrice,
                            ProductTotalPrice = OutStoreProductRequest.productTotalPrice,
                            OutStoreProductMemo = OutStoreProductRequest.outStoreProductMemo,
                        });
                    }
                }


                foreach(var orig in outStoreProducts)
                {
                    flag = false;
                    foreach(var newPrd in OutStoreProductRequestCrud.outStoreProductArray)
                    {
                        if(orig.OutStoreProductId == newPrd.outStoreProductId)
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        orig.IsDeleted = 1;
                    }

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

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
