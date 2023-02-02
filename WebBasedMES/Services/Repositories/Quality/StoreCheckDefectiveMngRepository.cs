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
    public class StoreCheckDefectiveMngRepository : IStoreCheckDefectiveMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public StoreCheckDefectiveMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

     

        public async Task<Response<IEnumerable<StoreCheckDefectiveRes001>>> storeCheckDefectiveMstList(StoreCheckDefectiveReq001 _req)
        {
            try
            {
                    var res = await _db.StoreOutStoreProducts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => _req.receivingNo == "" ? true : x.Receiving.ReceivingNo.Contains(_req.receivingNo))
                        .Where(x => x.Receiving.ReceivingDate >= Convert.ToDateTime(_req.receivingStartDate))
                        .Where(x => x.Receiving.ReceivingDate <= Convert.ToDateTime(_req.receivingEndDate))
                        .Where(x => _req.productId == 0 ? true : x.OutStoreProduct.Product.Id == _req.productId)
                        .Where(x => _req.partnerId == 0 ? true : x.Receiving.Partner.Id == _req.partnerId)
                        .Where(x => x.LotName.Contains(_req.productLOT))
                        .Select(x => new StoreCheckDefectiveRes001
                        {
                            storeOutStoreProductId = x.StoreOutStoreProductId,
                            receivingNo = x.Receiving.ReceivingNo??"",
                            receivingDate = x.Receiving.ReceivingDate.ToString("yyyy-MM-dd"),

                            partnerName = x.Receiving.Partner.Name??"",

                            productCode = x.OutStoreProduct.Product.Code??"",
                            productName = x.OutStoreProduct.Product.Name??"",
                            productStandard = x.OutStoreProduct.Product.Standard??"",
                            productStandardUnit = x.OutStoreProduct.ProductStandardUnit??"",

                            productReceivingCount = x.ProductReceivingCount,
                            registerId = x.Receiving.Register.Id??"",
                            registerName = x.Receiving.Register.FullName??"",

                            receivingProductMemo = x.ReceivingProductMemo??"",

                            productClassification = x.OutStoreProduct.Product.CommonCode.Name??"",
                            productLOT = x.Lot.LotName??"",
                            productImportCheck = x.OutStoreProduct.Product.ImportCheck,
                            productImportCheckResult = x.ProductImportCheckResult,
                            productDefectiveQuantity = _db.LotCounts.Where(y=>y.IsDeleted == 0).Where(y => y.Lot.LotName == x.Lot.LotName).Select(y=>y.DefectiveCount).Sum(),
                            productGoodQuantity = _db.LotCounts.Where(y=>y.IsDeleted == 0).Where(y => y.Lot.LotName == x.Lot.LotName).Select(y => y.StoreOutCount).Sum(),


                            flag = x.Receiving.ReceivingDate,
                        })
                        .OrderByDescending(x=>x.flag)
                        .ToListAsync();

                var res2 = res.Where(x => x.productDefectiveQuantity > 0);


                    var Res = new Response<IEnumerable<StoreCheckDefectiveRes001>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res2
                    };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreCheckDefectiveRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<StoreCheckDefectiveRes002>> storeCheckDefectiveMstPop(StoreCheckDefectiveReq001 req)

        {
            try
            {
                var res = await _db.StoreOutStoreProducts
                    .Where(x => x.StoreOutStoreProductId == req.storeOutStoreProductId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new StoreCheckDefectiveRes002
                    {
                        storeOutStoreProductId = x.StoreOutStoreProductId,

                        receivingNo = x.Receiving.ReceivingNo,
                        receivingDate = x.Receiving.ReceivingDate.ToString("yyyy-MM-dd"),

                        partnerCode = x.Receiving.Partner.Code,
                        partnerName = x.Receiving.Partner.Name,

                        productCode = x.OutStoreProduct.Product.Code,
                        productName = x.OutStoreProduct.Product.Name,
                        productUnit = x.OutStoreProduct.Product.Unit,
                        productStandard = x.OutStoreProduct.Product.Standard,
                        productStandardUnit = x.OutStoreProduct.ProductStandardUnit,
                        productStandardUnitCount = x.OutStoreProduct.ProductStandardUnitCount,
                        productReceivingCount = x.ProductReceivingCount,

                        productDefectiveQuantity = _db.LotCounts
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y => y.Lot.LotName == x.Lot.LotName)
                            .Select(y => y.DefectiveCount).Sum(),
                        productGoodQuantity = _db.LotCounts
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y => y.Lot.LotName == x.Lot.LotName)
                            .Select(y => y.StoreOutCount).Sum(),

                        productImportCheckResult = x.ProductImportCheckResult,
                        productImportCheck = x.OutStoreProduct.Product.ImportCheck,

                        productLOT = x.Lot.LotName,

                        outStoreNo = x.OutStoreProduct.OutStore.OutStoreNo,
                        registerId = x.Receiving.Register.Id,
                        registerName = x.Receiving.Register.FullName,

                        receivingProductMemo = x.ReceivingProductMemo,

                        productClassification = x.OutStoreProduct.Product.CommonCode.Name,
                     //   uploadFileName = x.Receiving.UploadFile.FileName,
                     //   uploadFileUrl = x.Receiving.UploadFile.FileUrl
                    })
                    .FirstOrDefaultAsync();

                var Res = new Response<StoreCheckDefectiveRes002>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<StoreCheckDefectiveRes002>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreCheckDefectiveRes003>>> storeCheckDefectiveList(StoreCheckDefectiveReq001 req)
        {
            try
            {
                var res = await _db.StoreOutStoreProductDefectives
                    .Where(x => x.StoreOutStoreProduct.StoreOutStoreProductId == req.storeOutStoreProductId)
                    .Select(x => new StoreCheckDefectiveRes003
                    {
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,
                        defectiveQuantity = x.DefectiveQuantity,
                        defectiveUnit = x.DefectiveUnit,
                        defectiveProductMemo = x.StoreOutStoreProduct.ReceivingProductMemo,
                    }).ToListAsync();


                var Res = new Response<IEnumerable<StoreCheckDefectiveRes003>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreCheckDefectiveRes003>>()
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
