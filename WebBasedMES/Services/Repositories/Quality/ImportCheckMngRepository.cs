using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.Lots;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public class ImportCheckMngRepository : IImportCheckMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public ImportCheckMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
     

        public async Task<Response<IEnumerable<ImportCheckRes001>>> importCheckMstList(ImportCheckReq001 _req)
        {
            try
            {

                //CheckResultText 
                //      0 = 합격, 1 = 부분합격, 2 = 불합격, 3 = 미검사 5 = 전체
                
                var res = await _db.StoreOutStoreProducts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => _req.receivingNo == "" ? true : x.Receiving.ReceivingNo.Contains(_req.receivingNo))
                    .Where(x => x.Receiving.ReceivingDate >= Convert.ToDateTime(_req.receivingStartDate))
                    .Where(x => x.Receiving.ReceivingDate <= Convert.ToDateTime(_req.receivingEndDate))
                    .Where(x => _req.productId == 0 ? true: x.OutStoreProduct.Product.Id == _req.productId)
                    .Where(x => _req.partnerId == 0? true : x.Receiving.Partner.Id == _req.partnerId)

                    .Where(x => x.LotName.Contains(_req.productLOT))
                    .Where(x => x.OutStoreProduct.Product.ImportCheck == true)
                    .Where(x => x.storeOutStoreProductInspections.Count()>0)
                    .OrderBy(x=>x.Receiving.ReceivingNo)
                    .Select(x => new ImportCheckRes001
                    {
                        storeOutStoreProductId = x.StoreOutStoreProductId,
                        receivingId = x.Receiving.ReceivingId,
                        receivingNo = x.Receiving.ReceivingNo??"",
                        receivingDate = x.Receiving.ReceivingDate.ToString("yyyy-MM-dd"),

                        partnerId = x.Receiving.Partner !=null? x.Receiving.Partner.Id : 0,
                        partnerName = x.Receiving.Partner.Name?? "",

                        productCode = x.OutStoreProduct.Product.Code??"",
                        productName = x.OutStoreProduct.Product.Name??"",
                        productStandard = x.OutStoreProduct.Product.Standard??"",
                        productStandardUnit = x.OutStoreProduct.ProductStandardUnit??"",
                        
                        productReceivingCount = x.ProductReceivingCount,
                        productImportCheckResult = x.storeOutStoreProductInspections.Where(y => y.InspectionResult == 0).Count() == x.storeOutStoreProductInspections.Count() ? 0 :
                                x.storeOutStoreProductInspections.Where(y => y.InspectionResult == 2).Count() == x.storeOutStoreProductInspections.Count() ? 2 :
                                x.storeOutStoreProductInspections.Where(y => y.InspectionResult == 3).Count() == x.storeOutStoreProductInspections.Count() ? 3 :
                                1,
                        registerId = x.Receiving.Register.Id??"",
                        registerName = x.Receiving.Register.FullName??"",

                        receivingProductMemo = x.ReceivingProductMemo??"",
                        
                        productClassification = x.OutStoreProduct.Product.CommonCode.Name??"",
                        productLOT = x.Lot.LotName ?? "",
                        flag = x.Receiving.ReceivingDate
                    })
                   // .OrderByDescending(x=> x.flag)
                    .ToListAsync();


                var res2 = res.Where(x => _req.productImportCheckResult == 5 ? true : x.productImportCheckResult == _req.productImportCheckResult);


                var Res = new Response<IEnumerable<ImportCheckRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<ImportCheckRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<ImportCheckRes002>> importCheckMstPop(ImportCheckReq001 req)
        {
            try
            {
                var res = await _db.StoreOutStoreProducts.Where(x => x.StoreOutStoreProductId == req.storeOutStoreProductId)
                    .Select(x => new ImportCheckRes002
                    {
                        storeOutStoreProductId = x.StoreOutStoreProductId,
                        receivingDate = x.Receiving.ReceivingDate.ToString("yyyy-MM-dd"),
                        outStoreNo = x.OutStoreProduct.OutStore.OutStoreNo,
                        partnerCode = x.Receiving.Partner.Code,
                        partnerName = x.Receiving.Partner.Name,
                        productClassification = x.OutStoreProduct.Product.CommonCode.Name,
                        productCode = x.OutStoreProduct.Product.Code,
                        productName = x.OutStoreProduct.Product.Name,
                        productStandard = x.OutStoreProduct.Product.Standard,
                        productStandardUnit = x.ProductStandardUnit,
                        productStandardUnitCount = x.ProductStandardUnitCount,
                        productUnit = x.OutStoreProduct.Product.Unit,
                        receivingId = x.Receiving.ReceivingId,
                        receivingNo = x.Receiving.ReceivingNo,
                        receivingProductMemo = x.ReceivingProductMemo,
                        registerName = x.Receiving.Register.FullName,

                        productImportCheckResult = x.storeOutStoreProductInspections.Where(y => y.InspectionResult == 0).Count() == x.storeOutStoreProductInspections.Count() ? "합격" :
                                x.storeOutStoreProductInspections.Where(y => y.InspectionResult == 2).Count() == x.storeOutStoreProductInspections.Count() ? "불합격" : 
                                x.storeOutStoreProductInspections.Where(y => y.InspectionResult == 3).Count() == x.storeOutStoreProductInspections.Count() ? "미검사" :
                                "부분합격",

                        productLOT = x.LotName,
                        productReceivingCount = x.ProductReceivingCount,


                    }).FirstOrDefaultAsync();



                var Res = new Response<ImportCheckRes002>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<ImportCheckRes002>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<ImportCheckRes003>>> importCheckInspectionList(ImportCheckReq001 req)
        {
            try
            {

                var res = await _db.StoreOutStoreProductInspectionTypes
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.StoreOutStoreProduct.StoreOutStoreProductId == req.storeOutStoreProductId)
                    .Select(x => new ImportCheckRes003
                    {
                        storeOutStoreProductInspectionId = x.StoreOutStoreProductInspectionId,
                        storeOutStoreProductId = x.StoreOutStoreProduct.StoreOutStoreProductId,

                        inspectionId = x.InspectionType.Id,
                        inspectionCode = x.InspectionType.Code,
                        inspectionMethod = x.InspectionType.InspectionMethod,
                        inspectionName = x.InspectionType.InspectionItem,
                        inspectionResult = x.InspectionResult == 0 ? "합격" : x.InspectionResult == 1 ? "부분합격" : x.InspectionResult == 2 ?  "불합격" : "미검사",
                        inspectionStandard = x.InspectionType.InspectionStandard,
                        inspectionResultText = x.InspectionResultText
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<ImportCheckRes003>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<ImportCheckRes003>>()
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
