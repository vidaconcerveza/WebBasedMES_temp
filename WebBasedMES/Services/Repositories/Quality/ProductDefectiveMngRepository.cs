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
    public class ProductDefectiveMngRepository : IProductDefectiveMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public ProductDefectiveMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }



        public async Task<Response<IEnumerable<ProductDefectiveRes001>>> productDefectiveMstList(ProductDefectiveReq001 req)
        {
            try
            {
                List<ProductDefectiveRes001> res = new List<ProductDefectiveRes001>();

                //공정   => 저기도 수정해야겠다... 
                if (req.type == "P" || req.type == "ALL")
                {
                    var resOut = await _db.ProcessDefectives
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(req.defectiveStartDate))
                        .Where(x => x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(req.defectiveEndDate))
                        .Where(x => req.productId == 0 ? true : x.ProcessProgress.WorkOrderProducePlan.Product.Id == req.productId)
                        .Where(x => x.Lot.LotName.Contains(req.productLOT))
                        .Where(x => req.defectiveId == 0 ? true : x.Defective.Id == req.defectiveId)
                        .Where(x=>x.ProcessProgress.WorkOrderProducePlan.Product !=null)
                        .Select(x => new ProductDefectiveRes001
                        {
                            type = "공정",
                            productId = x.ProcessProgress.WorkOrderProducePlan.Product.Id,

                            productCode = x.ProcessProgress.WorkOrderProducePlan.Product.Code,
                            productName = x.ProcessProgress.WorkOrderProducePlan.Product.Name,
                            productClassification = x.ProcessProgress.WorkOrderProducePlan.Product.CommonCode.Name,
                            productStandard = x.ProcessProgress.WorkOrderProducePlan.Product.Standard,
                            productUnit = x.ProcessProgress.WorkOrderProducePlan.Product.Unit,
                            productLOT = x.Lot.LotName,
                            defectiveId = x.Defective.Id,
                            defectiveCode = x.Defective.Code,
                            defectiveName = x.Defective.Name,
                            productDefectiveQuantity = x.DefectiveCount,
                            productStandardUnit = "-",
                            defectiveProductMemo = x.DefectiveProductMemo,
                            registerName = x.ProcessProgress.WorkOrderProducePlan.Register.FullName,
                            defectiveDate = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd")

                        })
                        .ToListAsync();

                    if (resOut != null) res.AddRange(resOut);
                }

                //2.입고 불량
                if (req.type == "I" || req.type == "ALL")
                {
                    var resIn = await _db.StoreOutStoreProductDefectives
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.StoreOutStoreProduct.Receiving.ReceivingDate >= Convert.ToDateTime(req.defectiveStartDate))
                        .Where(x => x.StoreOutStoreProduct.Receiving.ReceivingDate <= Convert.ToDateTime(req.defectiveEndDate))
                        .Where(x => req.productId == 0 ? true : x.StoreOutStoreProduct.OutStoreProduct.Product.Id == req.productId)
                        .Where(x => x.Lot.LotName.Contains(req.productLOT))
                        .Where(x => req.defectiveId == 0 ? true : x.Defective.Id == req.defectiveId)
                        .Where(x => x.StoreOutStoreProduct.OutStoreProduct.Product!=null)
                        .Select(x => new ProductDefectiveRes001
                        {
                            type = "입고",
                            productId = x.StoreOutStoreProduct.OutStoreProduct.Product.Id,
                            productCode = x.StoreOutStoreProduct.OutStoreProduct.Product.Code,
                            productName = x.StoreOutStoreProduct.OutStoreProduct.Product.Name,
                            productClassification = x.StoreOutStoreProduct.OutStoreProduct.Product.CommonCode.Name,
                            productStandard = x.StoreOutStoreProduct.OutStoreProduct.Product.Standard,
                            productUnit = x.StoreOutStoreProduct.OutStoreProduct.Product.Unit,
                            productLOT = x.Lot.LotName,
                            defectiveId = x.Defective.Id,
                            defectiveCode = x.Defective.Code,
                            defectiveName = x.Defective.Name,
                            productDefectiveQuantity = x.DefectiveQuantity,
                            productStandardUnit = x.StoreOutStoreProduct.ProductStandardUnit,
                            defectiveProductMemo = x.DefectiveMemo,
                            registerName = x.StoreOutStoreProduct.Receiving.Register.FullName,
                            defectiveDate = x.StoreOutStoreProduct.Receiving.ReceivingDate.ToString("yyyy-MM-dd"),
                        })
                        .ToListAsync();


                    if (resIn != null) res.AddRange(resIn);
                }

                //.출고 불량
                if (req.type == "O" || req.type == "ALL")
                {
                    var resOut = await _db.OutOrderProductsDefectives
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate >= Convert.ToDateTime(req.defectiveStartDate))
                        .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate <= Convert.ToDateTime(req.defectiveEndDate))
                        .Where(x => req.productId == 0 ? true : x.OutOrderProduct.Product.Id == req.productId)
                        .Where(x => x.Lot.LotName.Contains(req.productLOT))
                        .Where(x => req.defectiveId == 0 ? true : x.Defective.Id == req.defectiveId)
                        .Where(x =>  x.OutOrderProduct.Product!=null)
                        .Select(x => new ProductDefectiveRes001
                        {
                            type = "출고",

                            productCode = x.OutOrderProduct.Product.Code,
                            productId = x.OutOrderProduct.Product.Id,
                            productName = x.OutOrderProduct.Product.Name,
                            productClassification = x.OutOrderProduct.Product.CommonCode.Name,
                            productStandard = x.OutOrderProduct.Product.Standard,
                            productUnit = x.OutOrderProduct.Product.Unit,
                            productLOT = x.Lot.LotName,

                            productStandardUnit = "-",
                            productDefectiveQuantity = x.DefectiveQuantity,
                            defectiveId = x.Defective.Id,
                            defectiveCode = x.Defective.Code,
                            defectiveName = x.Defective.Name,
                            registerName = x.OutOrderProduct.OutOrder.Register.FullName,
                            defectiveDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                            defectiveProductMemo = x.DefectiveProductMemo

                        })
                        .ToListAsync();

                    if (resOut != null) res.AddRange(resOut);
                }



                if (req.type == "E" || req.type == "ALL")
                {
                    var resEtc = await _db.EtcDefectivesDetails
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.EtcDefective.DefectiveDate >= Convert.ToDateTime(req.defectiveStartDate))
                        .Where(x => x.EtcDefective.DefectiveDate <= Convert.ToDateTime(req.defectiveEndDate))
                        .Where(x => req.defectiveId == 0 ? true : x.EtcDefective.Defective.Id == req.defectiveId)
                        .Where(x => req.productId == 0 ? true : x.EtcDefective.Product.Id == req.productId)
                        .Where(x => x.Lot.LotName.Contains(req.productLOT))
                        .Where(x => req.defectiveId == 0 ? true : x.EtcDefective.Defective.Id == req.defectiveId)
                        .Where(x => x.EtcDefective.Product != null)
                        .Select(x =>
                            new ProductDefectiveRes001
                            {
                                type = "기타불량",
                                defectiveId = x.EtcDefective.Defective.Id,
                                defectiveCode = x.EtcDefective.Defective.Code,
                                defectiveDate = x.EtcDefective.DefectiveDate.ToString("yyyy-MM-dd"),
                                defectiveName = x.EtcDefective.Defective.Name,
                                productId = x.EtcDefective.Product.Id,
                                productCode = x.EtcDefective.Product.Code,
                                productClassification = x.EtcDefective.Product.CommonCode.Name,
                                productName = x.EtcDefective.Product.Name,
                                productStandard = x.EtcDefective.Product.Standard,
                                productUnit = x.EtcDefective.Product.Unit,
                                productStandardUnit = x.ProductStandardUnit.Name,
                                productDefectiveQuantity = x.DefectiveQuantity,
                                productLOT = x.Lot.LotName,
                                defectiveProductMemo = x.EtcDefective.EtcDefectiveMemo,
                                registerName = "-",
                            }
                        ).ToListAsync();
                    if (resEtc != null) res.AddRange(resEtc);
                }


                var res2 = res.OrderBy(x => x.productCode).ThenBy(x=>x.defectiveDate);

                var Res = new Response<IEnumerable<ProductDefectiveRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<ProductDefectiveRes001>>()
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
