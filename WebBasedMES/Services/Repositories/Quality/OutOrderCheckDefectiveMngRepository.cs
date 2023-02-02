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
    public class OutOrderCheckDefectiveMngRepository : IOutOrderCheckDefectiveMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public OutOrderCheckDefectiveMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }



        public async Task<Response<IEnumerable<OutOrderCheckDefectiveRes001>>> outOrderCheckDefectiveMstList(OutOrderCheckDefectiveReq001 req)
        {
            try
            {

                string ckstr = "";

                switch (req.productShipmentCheckResult)
                {
                    case 0:
                        ckstr = "합격";
                        break;

                    case 1:
                        ckstr = "부분합격";
                        break;

                    case 2:
                        ckstr = "불합격";
                        break;
                    case 3:
                        ckstr = "미검사";
                        break;
                    default:
                        break;
                }


                var res = await _db.OutOrderProductsStocks
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate >= Convert.ToDateTime(req.shipmentStartDate))
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate <= Convert.ToDateTime(req.shipmentEndDate))
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentNo.Contains(req.shipmentNo))
                    .Where(x => req.productId == 0 ? true : x.OutOrderProduct.Product.Id == req.productId)
                    .Where(x => req.partnerId == 0 ? true : x.OutOrderProduct.OutOrder.Partner.Id == req.partnerId)
                    .Where(x => x.Lot.LotName.Contains(req.productLOT))
                    .OrderBy(x=> x.OutOrderProduct.OutOrder.ShipmentNo)
                    .Select(x => new OutOrderCheckDefectiveRes001
                    {
                        orderNo = x.OutOrderProduct.OrderProduct.Order.OrderNo,

                        outOrderProductId = x.OutOrderProduct.OutOrderProductId,
                        shipmentNo = x.OutOrderProduct.OutOrder.ShipmentNo,
                        shipmentDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                        partnerCode = x.OutOrderProduct.OutOrder.Partner.Code,
                        partnerName = x.OutOrderProduct.OutOrder.Partner.Name,
                        productCode = x.OutOrderProduct.Product.Code,
                        productName = x.OutOrderProduct.Product.Name,
                        productClassification = x.OutOrderProduct.Product.CommonCode.Name,
                        productStandard = x.OutOrderProduct.Product.Standard,
                        productUnit = x.OutOrderProduct.Product.Unit,
                        quantity = x.Lot.LotCounts.FirstOrDefault().OutOrderCount,
                        productLOT = x.Lot.LotName,
                        productShipmentCheckResult = x.ProductShipmentCheckResult == "합격" ? 0 : x.ProductShipmentCheckResult == "부분합격"? 1 : x.ProductShipmentCheckResult == "불합격" ? 2 :3,   // x.Prod,uctShipmentCheckResult,  // Number로 넘겨줘야함.
                        registerName = x.OutOrderProduct.OutOrder.Register.FullName,
                        shipmentProductMemo = x.OutOrderProduct.ShipmentProductMemo,
                        productDefectiveQuantity = _db.OutOrderProductsDefectives
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.OutOrderProduct.OutOrderProductId == x.OutOrderProduct.OutOrderProductId)
                            .Select(y => y.DefectiveQuantity).Sum(),
                        productShipmentCheck = x.OutOrderProduct.Product.ExportCheck,
                        dateFlag = x.OutOrderProduct.OutOrder.ShipmentDate
                    })
                    .ToListAsync();


                var res2 = res
                    .Where(x => req.productShipmentCheckResult == 5 ? true : x.productShipmentCheckResult == req.productShipmentCheckResult)
                    .Where(x => x.productDefectiveQuantity > 0);





                var Res = new Response<IEnumerable<OutOrderCheckDefectiveRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderCheckDefectiveRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<OutOrderCheckDefectiveRes002>> outOrderCheckDefectiveMstPop(OutOrderCheckDefectiveReq001 req)

        {
            try
            {
                var res = await _db.OutOrderProductsStocks
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.OutOrderProduct.OutOrderProductId == req.outOrderProductId)
                    .Select(x => new OutOrderCheckDefectiveRes002
                    {
                        outOrderProductId = x.OutOrderProduct.OutOrderProductId,
                        shipmentNo = x.OutOrderProduct.OutOrder.ShipmentNo,
                        shipmentDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                        partnerCode = x.OutOrderProduct.OutOrder.Partner.Code,
                        partnerName = x.OutOrderProduct.OutOrder.Partner.Name,
                        productCode = x.OutOrderProduct.Product.Code,
                        productName = x.OutOrderProduct.Product.Name,
                        productClassification = x.OutOrderProduct.Product.CommonCode.Name,
                        productStandard = x.OutOrderProduct.Product.Standard,
                        productUnit = x.OutOrderProduct.Product.Unit,
                        //quantity = x.OutOrderProduct.Quantity,
                        quantity = x.Lot.LotCounts.Where(y=>y.IsDeleted == 0).FirstOrDefault().OutOrderCount,
                        productLOT = x.Lot.LotName,
                        productShipmentCheckResult = x.ProductShipmentCheckResult == "합격" ? 0 : x.ProductShipmentCheckResult == "부분합격" ? 1 : x.ProductShipmentCheckResult == "불합격" ? 2 : 3,
                        registerName = x.OutOrderProduct.OutOrder.Register.FullName,
                        shipmentProductMemo = x.OutOrderProduct.ShipmentProductMemo,
                        productDefectiveQuantity = _db.OutOrderProductsDefectives
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.OutOrderProduct.OutOrderProductId == x.OutOrderProduct.OutOrderProductId)
                            .Select(y=>y.DefectiveQuantity).Sum(),
                        productShipmentCheck = x.OutOrderProduct.Product.ExportCheck,
                       // uploadFileName = x.OutOrderProduct.OutOrder.UploadFile.FileName,
                       // uploadFileUrl = x.OutOrderProduct.OutOrder.UploadFile.FileUrl,
                        orderNo = x.OutOrderProduct.OrderProduct.Order.OrderNo,
                    })
                    .FirstOrDefaultAsync();

               // var res2 = res.Where(x => x.productDefectiveQuantity > 0);


                var Res = new Response<OutOrderCheckDefectiveRes002>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<OutOrderCheckDefectiveRes002>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OutOrderCheckDefectiveRes003>>> outOrderCheckDefectiveList(OutOrderCheckDefectiveReq001 req)
        {
            try
            {

                var res = await _db.OutOrderProductsDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.OutOrderProduct.OutOrderProductId == req.outOrderProductId)
                    .Select(x => new OutOrderCheckDefectiveRes003
                    {
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,
                        defectiveQuantity = x.DefectiveQuantity,
                        defectiveProductMemo = x.DefectiveProductMemo

                    }).ToListAsync();

                var Res = new Response<IEnumerable<OutOrderCheckDefectiveRes003>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderCheckDefectiveRes003>>()
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
