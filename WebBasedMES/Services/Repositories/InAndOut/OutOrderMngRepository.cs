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
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public class OutOrderMngRepository : IOutOrderMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public OutOrderMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            
        }

        public async Task<Response<IEnumerable<OutOrderRes001>>> outOrderMstList(OutOrderReq001 outOrderRequest)
        {
            try
            {

                var _outOrder = await _db.OutOrders

                    .Include(x => x.OutOrderProducts).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                   .Include(x => x.OutOrderProducts).ThenInclude(x => x.OrderProduct.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                    .Where(x => outOrderRequest.shipmentNo == "" ? true : x.ShipmentNo.Contains(outOrderRequest.shipmentNo))
                    .Where(x => outOrderRequest.shipmentStartDate == "" ? x.ShipmentDate >= DateTime.Today : x.ShipmentDate >= Convert.ToDateTime(outOrderRequest.shipmentStartDate))
                    .Where(x => outOrderRequest.shipmentEndDate == "" ? x.ShipmentDate <= DateTime.Today.AddMonths(1) : x.ShipmentDate <= Convert.ToDateTime(outOrderRequest.shipmentEndDate))
                    .Where(x => outOrderRequest.partnerId == 0 ? true : x.Partner.Id == outOrderRequest.partnerId)

                    .Where(x => outOrderRequest.productId == 0 ? true :

                        x.OutOrderProducts
                        .Where(x => x.Product.Id == outOrderRequest.productId)
                        .Select(x => x.Product.Id).ToArray()
                        .Contains(outOrderRequest.productId)
                    )
                    .OrderByDescending(x => x.ShipmentNo)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new OutOrderRes001
                    {
                        outOrderId = x.OutOrderId,
                        shipmentNo = x.ShipmentNo,
                        shipmentDate = x.ShipmentDate.ToString("yyyy-MM-dd"),
                        partnerName = x.Partner.Name, // 거래처이름
                        partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                        shipmentProductCount = x.OutOrderProducts.Where(y => y.IsDeleted == 0).Count(),
                        shipmentSellPrice = x.OutOrderProducts.Where(y => y.IsDeleted == 0).Select(OOP => OOP.ProductSellPrice).Sum(),
                        shipmentSupplyPrice = x.OutOrderProducts.Where(y => y.IsDeleted == 0).Select(OOP =>OOP.ProductSellPrice*OOP.ProductShipmentCount).Sum(),
                        shipmentTaxPrice = x.OutOrderProducts.Where(y => y.IsDeleted == 0).Select(OOP => OOP.Product.TaxType == "과세"? (OOP.ProductSellPrice*OOP.ProductShipmentCount)/10 : 0).Sum(),
                        shipmentTotalPrice = x.OutOrderProducts.Where(y => y.IsDeleted == 0).Select(OOP => OOP.Product.TaxType == "과세"? Convert.ToInt32((OOP.ProductSellPrice*OOP.ProductShipmentCount)*1.1) : (OOP.ProductSellPrice * OOP.ProductShipmentCount)).Sum(),

                        registerName = x.Register.FullName,//등록자
                        shipmentMemo = x.ShipmentMemo,
                        uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                    }).ToListAsync();

                var Res = new Response<IEnumerable<OutOrderRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _outOrder
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OutOrderRes002>>> outOrderProductList(OutOrderReq001 outOrderRequest)
        {
            try
            {

                var result = await _db.OutOrderProducts
                    //.Include(x => x.OrderProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    //.Include(x => x.OrderProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                   // .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    .Where(x => x.OutOrder.OutOrderId == outOrderRequest.outOrderId)
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x=>x.Product.Code)
                    .Select(x => new OutOrderRes002
                    {

                        outOrderProductId = x.OutOrderProductId,
                        outOrderId = x.OutOrder.OutOrderId,
                        orderProductId = x.OrderProduct != null ? x.OrderProduct.OrderProductId:0,
                        productId = x.Product != null ? x.Product.Id : 0,
                       
                        orderNo = x.OrderProduct != null ? x.OrderProduct.Order.OrderNo:"",
                        productCode = x.OrderProduct != null ? x.OrderProduct.Product.Code : x.Product.Code,
                        productClassification = x.Product.CommonCode.Name,
                        //productClassification = x.OrderProduct != null ? x.OrderProduct.Product.CommonCode.Code : x.Product.CommonCode.Code,
                        productName = x.Product.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,
                        productStandardUnit = x.OrderProduct != null? x.OrderProduct.ProductStandardUnit : "",

                        productTaxInfo = x.Product.TaxType,
                        productOrderCount = x.OrderProduct != null ? x.OrderProduct.ProductOrderCount : 0,
                        productShipmentCount = x.ProductShipmentCount,
                        productSellPrice = x.ProductSellPrice,
                        productSupplyPrice = x.ProductSellPrice * x.ProductShipmentCount,
                        productTaxPrice =  x.Product.TaxType == "과세" ? Convert.ToInt32(x.ProductSellPrice * x.ProductShipmentCount) : 0,
                        productTotalPrice = x.Product.TaxType == "과세" ? Convert.ToInt32(x.ProductSellPrice * x.ProductShipmentCount * 1.1) : x.ProductSellPrice * x.ProductShipmentCount,
                        shipmentProductMemo = x.ShipmentProductMemo,

                        ModelName = x.Product.Model != null ? x.Product.Model.Name : "",
                        PartnerName = x.Product.Partner != null ? x.Product.Partner.Name : "",


                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<OutOrderRes002>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderRes002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<OutOrderRes003>> outOrderMstPop(OutOrderReq001 outOrderRequest)
        {
            try
            {
                var _outOrder = await _db.OutOrders

                    
                    .Include(x => x.Partner)
                    .Where(x => x.OutOrderId == outOrderRequest.outOrderId)
                     .Where(x => x.IsDeleted == 0)
                    .Select(x => new OutOrderRes003
                    {
                        outOrderId = x.OutOrderId,
                        shipmentNo = x.ShipmentNo,
                        shipmentDate = x.ShipmentDate.ToString("yyyy-MM-dd"),
                        partnerId = x.Partner.Id,
                        partnerCode = x.Partner.Code,
                        partnerName = x.Partner.Name,
                        partnerTaxInfo = x.Partner.TaxInfo,
                        contactName = x.Partner.ContactName,
                        telephoneNumber = x.Partner.TelephoneNumber,
                        faxNumber = x.Partner.FaxNumber,
                        contactEmail = x.Partner.ContactEmail,
                        registerId = x.Register.Id,
                        registerName = x.Register.FullName,
                        shipmentMemo = x.ShipmentMemo,
                        uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                        UploadFiles = x.UploadFiles.ToList()

                    }).FirstOrDefaultAsync();

                var Res = new Response<OutOrderRes003>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _outOrder
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<OutOrderRes003>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OutOrderRes004>>> outOrderProductListPop(OutOrderReq001 outOrderRequest)
        {
            try
            {

                var result = await _db.OutOrderProducts
                   .Include(x => x.OrderProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                   .Include(x => x.OrderProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                   .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                   //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                   .Where(x => x.OutOrder.OutOrderId == outOrderRequest.outOrderId)
                   .Where(x => x.IsDeleted == 0)
                   .Select(x => new OutOrderRes004
                   {
                       outOrderProductId = x.OutOrderProductId,
                       outOrderId = x.OutOrder.OutOrderId,
                       orderProductId = x.OrderProduct != null ? x.OrderProduct.OrderProductId : 0,
                       productId = x.Product != null ? x.Product.Id : 0,

                       orderNo = x.OrderProduct != null ? x.OrderProduct.Order.OrderNo : "",
                       productCode = x.OrderProduct != null ? x.OrderProduct.Product.Code : x.Product.Code,
                       productClassification = x.Product.CommonCode.Name,
                        //productClassification = x.OrderProduct != null ? x.OrderProduct.Product.CommonCode.Code : x.Product.CommonCode.Code,
                       productName = x.Product.Name,
                       productStandard = x.Product.Standard,
                       productUnit = x.Product.Unit,
                       productStandardUnit = x.OrderProduct != null ? x.OrderProduct.ProductStandardUnit : "",

                       productTaxInfo = x.Product.TaxType,
                       productOrderCount = x.OrderProduct != null ? x.OrderProduct.ProductOrderCount : 0,
                       productShipmentCount = x.ProductShipmentCount,
                       productSellPrice = x.OrderProduct != null ? x.OrderProduct.ProductSellPrice : x.Product.SellPrice,
                       productSupplyPrice = x.OrderProduct != null ? x.OrderProduct.ProductSupplyPrice : x.Product.SellPrice + x.Product.SellPrice / 10,
                       productTaxPrice = x.OrderProduct != null ? x.OrderProduct.ProductTaxPrice : x.Product.SellPrice / 10,
                       productTotalPrice = x.OrderProduct != null ? x.OrderProduct.ProductTotalPrice : (x.Product.SellPrice + x.Product.SellPrice / 10) * x.ProductShipmentCount,
                       shipmentProductMemo = x.ShipmentProductMemo,
                       productOrderCountRemain = x.OrderProduct != null ? x.OrderProduct.ProductOrderCount - x.ProductShipmentCount : 0,
                       productStandardUnitCount = x.OrderProduct != null ? x.OrderProduct.ProductStandardUnitCount : 0,
                       outOrderProductDefectives = _db.OutOrderProductsDefectives
                           .Where(y => y.IsDeleted == 0)
                           .Where(y => y.OutOrderProduct.OutOrderProductId == x.OutOrderProductId)
                           .Select(y=> new OutOrderProductDefectiveInterface
                           {
                               defectiveId = y.Defective.Id,
                               outOrderProductDefectiveId = y.OutOrderProductDefectiveId,
                               defectiveCode = y.Defective.Code,
                               defectiveName = y.Defective.Name,
                               defectiveProductMemo = y.DefectiveProductMemo,
                               defectiveQuantity = y.DefectiveQuantity,
                               productLOT = y.Lot.LotName,
                               productId = _db.LotCounts.Where(z=>z.Lot == y.Lot).Select(z=>z.Product.Id).FirstOrDefault(),
                           }).ToList(),

                       outOrderProductStocks = x.OutOrderProductStock
                           .Where(y => y.IsDeleted == 0)
                           .Select(y => new OutOrderProductStockInterface
                           {
                               outOrderProductsStockId = y.OutOrderProductStockId,
                               productLOT = y.Lot.LotName,
                               productClassification = x.Product.CommonCode.Name,
                               productCode = x.Product.Code,
                               productName = x.Product.Name,
                               productId = x.Product.Id,
                               productShipmentCheckResult = y.ProductShipmentCheckResult,
                               productStandard = x.Product.Standard,
                               quantity = _db.LotCounts.Where(z=>z.Lot == y.Lot).Select(z=>z.OutOrderCount).FirstOrDefault(),
                               tempQuantity = _db.LotCounts.Where(z=>z.Lot == y.Lot).Select(z=>z.OutOrderCount).FirstOrDefault(),
                               productShipmentCheck = x.Product.ExportCheck,
                               productUnit = x.Product.Unit,
                               isSelected = true,
                               Checked = true,
                               inventory = _db.LotCounts.Where(z=>z.IsDeleted == 0).Where(z=>z.Lot.LotName == y.Lot.LotName).Select(z=> z.ModifyCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount - z.DefectiveCount).Sum(), //+ _db.LotCounts.Where(z => z.Lot == y.Lot).Select(z => z.OutOrderCount).FirstOrDefault(),
                               
                           }).ToList()
                   }).ToListAsync();

                /*
                var lots = _db.LotCounts
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => (y.StoreOutCount + y.ProduceCount - y.OutOrderCount - y.DefectiveCount - y.ConsumeCount + y.ModifyCount) > 0)
                            .Select(y => new OutOrderProductStockInterface
                            {
                                productLOT = y.Lot.LotName,
                                inventory = y.StoreOutCount + y.ProduceCount - y.OutOrderCount - y.DefectiveCount - y.ConsumeCount + y.ModifyCount,
                                productId = y.Product.Id,
                            })
                            .ToList()
                            .GroupBy(y => y.productLOT)
                            .Select(y => new OutOrderProductStockInterface
                            {
                                outOrderProductsStockId = 0,
                                productLOT = y.Key,
                                inventory = y.Sum(z => z.inventory),
                                isSelected = false,
                                productId = y.Select(z => z.productId).FirstOrDefault(),
                                quantity = 0,
                                tempQuantity = 0,
                                LotCountId = 0,

                            }).ToList();
                */

                var lots = _db.Inventory.Select(y => new OutOrderProductStockInterface
                {
                    outOrderProductsStockId = 0,
                    productLOT = y.LotName,
                    inventory = y.Total,
                    isSelected = false,
                    productId = y.Product.Id,
                    quantity = 0,
                    tempQuantity = 0,
                    LotCountId = 0,
                }).ToList();


                foreach (var i in result)
                {
                    List<OutOrderProductStockInterface> newLot = new List<OutOrderProductStockInterface>();
                    List<OutOrderProductStockInterface> orgLot = new List<OutOrderProductStockInterface>();

                    foreach (var j in i.outOrderProductStocks)
                    {
                        foreach (var k in lots)
                        {

                                if(k.productId == j.productId)
                                {
                                    var stk = new OutOrderProductStockInterface
                                    {
                                        productId = j.productId,
                                        isSelected = false,
                                        Checked = false,
                                        inventory = k.inventory,
                                        outOrderProductsStockId = 0, //j.outOrderProductsStockId,
                                        productClassification = j.productClassification,
                                        quantity = 0,
                                        tempQuantity = 0,
                                        productLOT = k.productLOT,
                                        productCode = j.productCode,
                                        productName = j.productName,
                                        productShipmentCheckResult = j.productShipmentCheck ? "" : "미검사",
                                        productShipmentCheck = j.productShipmentCheck,
                                        productStandard = j.productStandard,
                                        productUnit = j.productUnit,
                                        
                                    };
                                    
                                    newLot.Add(stk);
                                    //i.outOrderProductStocks.Append(stk);
                                }
                            
                        }
                        orgLot.Add(j);
                    }
                    bool flag = false;
                    foreach(var x in newLot)
                    {
                        flag = false;
                        foreach(var y in orgLot)
                        {
                            if(x.productLOT == y.productLOT)
                            {
                                flag = true;
                            }
                        }

                        if (!flag)
                        {
                            orgLot.Add(x);
                        }
                    }

                   // orgLot.AddRange(newLot);
                    i.outOrderProductStocks = orgLot;
                }


                //.ToListAsync();

                var Res = new Response<IEnumerable<OutOrderRes004>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderRes004>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        //수주 리스트 팝업
        public async Task<Response<IEnumerable<OutOrderRes006>>> orderProductListPop(OutOrderReq003 outOrderRequest)
        {
            try
            {
                if(outOrderRequest.requestType == "PP")
                {
                    var resultPP = await _db.OrderProducts
                        .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                        .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                        .Where(x=>x.Product.Processes.Where(x=>x.IsDeleted == 0).Count()>0)
                        .Where(x => outOrderRequest.partnerId == 0 ? true : x.Order.Partner.Id == outOrderRequest.partnerId)
                        .Where(x => outOrderRequest.shipmentStatus == "" ? true : true) //todo 출고여부 넣어줘야함.
                        .Where(x => outOrderRequest.searchInput == "" ? true :
                        x.Order.OrderNo.Contains(outOrderRequest.searchInput) ||
                        x.Order.Partner.Name.Contains(outOrderRequest.searchInput) ||
                        x.Order.Partner.TaxInfo.Contains(outOrderRequest.searchInput) ||
                        x.Product.Code.Contains(outOrderRequest.searchInput) ||
                        x.Product.Item.CommonCode.Code.Contains(outOrderRequest.searchInput) ||
                        x.Product.Name.Contains(outOrderRequest.searchInput) ||
                        x.Product.Unit.Contains(outOrderRequest.searchInput) ||
                        x.ProductStandardUnit.Contains(outOrderRequest.searchInput)
                                    )
                        .Where(x => x.IsDeleted == 0)
                        .OrderBy(x=>x.Order.OrderDate)
                        .Select(x => new OutOrderRes006
                        {

                            orderProductId = x.OrderProductId,
                            orderNo = x.Order.OrderNo,
                            orderDate = x.Order.OrderDate.ToString("yyyy-MM-dd"),
                            requestDeliveryDate = x.Order.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                            partnerName = x.Order.Partner.Name,
                            partnerTaxInfo = x.Order.Partner.TaxInfo,
                            shipmentStatus = x.Order.OrderFinish, //todo 출고여부
                            productId = x.Product.Id,
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,
                            productStandardUnit = x.ProductStandardUnit,
                            productOrderCount = x.ProductOrderCount,
                            productOrderCountRemain = x.ProductOrderCount - _db.OutOrderProducts
                            .Where(y => y.OrderProduct.OrderProductId == x.OrderProductId)
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => y.ProductShipmentCount)
                            .Sum(),
                            productSellPrice = x.ProductSellPrice,
                            productShipmentCheck = x.Product.ExportCheck,
                            Inventory = _db.LotCounts.Where(z => z.Product.Id == x.Product.Id).Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                            OptimumStock = x.Product.OptimumStock,
                            
                            producePlanProcesses = x.Product.Processes
                                .Where(x => x.IsDeleted == 0)
                                .OrderBy(x => x.ProcessOrder)
                                .Select(y => new ProductProcessInterface2
                                {
                                    ProductProcessId = y.ProductProcessId,
                                    ProcessId = y.ProcessId,
                                    ProcessCode = y.Process.Code,
                                    ProcessName = y.Process.Name,
                                    ProcessOrder = y.ProcessOrder,

                                    ProductId = y.ProduceProductId,
                                    ProductUnit = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Unit).FirstOrDefault(),
                                    ProductClassification = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.CommonCode.Name).FirstOrDefault(),
                                    ProductCode = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Code).FirstOrDefault(),
                                    ProductName = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Name).FirstOrDefault(),
                                    ProductStandard = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Standard).FirstOrDefault(),

                                    Inventory = _db.LotCounts.Where(z => z.Product.Id == y.ProduceProductId).Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                                    ProcessPlanQuantity = 0,

                                    ProcessInputItems = y.Items
                                        .Where(z => z.IsDeleted == 0)
                                        .Select(z => new ProductItemInterface2
                                        {
                                            ProcessId = z.ProcessId,
                                            ProcessCode = _db.Processes.Where(k => k.Id == z.ProcessId).Select(k => k.Code).FirstOrDefault(),
                                            ProcessName = _db.Processes.Where(k => k.Id == z.ProcessId).Select(k => k.Name).FirstOrDefault(),

                                            ItemId = z.ProductId,
                                            ItemCode = z.Product.Code,
                                            ItemName = z.Product.Name,
                                            ItemClassification = z.Product.CommonCode.Name,
                                            ItemStandard = z.Product.Standard,
                                            ItemUnit = z.Product.Unit,
                                            Loss = (float)z.Loss,
                                            ProcessPlanQuantity = 0,
                                            RequiredQuantity = (float)z.Require,
                                            TotalRequiredQuantity = (float)(z.Loss * z.Require),
                                            TotalInputQuantity = 0,
                                            Inventory = _db.LotCounts.Where(k => k.Product.Id == z.Product.Id).Where(k => k.IsDeleted == 0).Select(k => (0 - k.DefectiveCount - k.ConsumeCount - k.OutOrderCount + k.StoreOutCount + k.ProduceCount + k.ModifyCount)).Sum(),
                                        }).ToList()

                                }
                            
                            ).ToList(),
                            
                            outOrderProductDefectives = _db.OutOrderProductsDefectives
                                .Where(y => y.OutOrderProductDefectiveId == 0)
                                .Select(y => new OutOrderProductDefectiveInterface
                                {

                                }).ToList(),

                        }).ToListAsync();

                    var ResPP = new Response<IEnumerable<OutOrderRes006>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = resultPP
                    };

                    return ResPP;
                }

                var result = await _db.OrderProducts
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Where(x=>x.Product.Processes.Where(x=>x.IsDeleted == 0).Count()>0)
                    .Where(x => outOrderRequest.partnerId == 0 ? true : x.Order.Partner.Id == outOrderRequest.partnerId)
                    .Where(x => outOrderRequest.shipmentStatus == "" ? true : true) //todo 출고여부 넣어줘야함.
                    .Where(x => outOrderRequest.searchInput == "" ? true :
                    x.Order.OrderNo.Contains(outOrderRequest.searchInput) ||
                    x.Order.Partner.Name.Contains(outOrderRequest.searchInput) ||
                    x.Order.Partner.TaxInfo.Contains(outOrderRequest.searchInput) ||
                    x.Product.Code.Contains(outOrderRequest.searchInput) ||
                    x.Product.Item.CommonCode.Code.Contains(outOrderRequest.searchInput) ||
                    x.Product.Name.Contains(outOrderRequest.searchInput) ||
                    x.Product.Unit.Contains(outOrderRequest.searchInput) ||
                    x.ProductStandardUnit.Contains(outOrderRequest.searchInput) 
                                )
                    .Where(x => x.IsDeleted == 0)
                        .OrderBy(x => x.Order.OrderDate)

                    .Select(x => new OutOrderRes006
                    {
                        orderProductId = x.OrderProductId,
                        orderNo =  x.Order.OrderNo,
                        orderDate = x.Order.OrderDate.ToString("yyyy-MM-dd"),
                        requestDeliveryDate = x.Order.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                        partnerName = x.Order.Partner.Name,
                        partnerTaxInfo = x.Order.Partner.TaxInfo,
                        shipmentStatus = x.Order.OrderFinish, //todo 출고여부
                        productId = x.Product.Id,
                        productCode = x.Product.Code,
                        productClassification = x.Product.CommonCode.Name,
                        productName = x.Product.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,
                        productStandardUnit = x.ProductStandardUnit,
                        productOrderCount = x.ProductOrderCount,
                        productOrderCountRemain = x.ProductOrderCount - _db.OutOrderProducts
                            .Where(y=>y.OrderProduct.OrderProductId == x.OrderProductId)
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => y.ProductShipmentCount)
                            .Sum(),

                        productSellPrice = x.ProductSellPrice,
                        productShipmentCheck = x.Product.ExportCheck,

                        producePlanProcesses = x.Product.Processes
                            .Where(x => x.IsDeleted == 0)
                            .Select(y => new ProductProcessInterface2
                            {
                                ProductProcessId = y.ProductProcessId,
                                ProcessId = y.ProcessId,
                                ProcessCode = y.Process.Code,
                                ProcessName = y.Process.Name,
                                ProcessOrder = y.ProcessOrder,

                                ProductId = y.ProduceProductId,
                                ProductUnit = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Unit).FirstOrDefault(),
                                ProductClassification = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.CommonCode.Name).FirstOrDefault(),
                                ProductCode = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Code).FirstOrDefault(),
                                ProductName = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Name).FirstOrDefault(),
                                ProductStandard = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Standard).FirstOrDefault(),

                                Inventory = _db.LotCounts.Where(z => z.Product.Id == y.Product.Id).Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                                ProcessPlanQuantity = 0,

                                ProcessInputItems = y.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Select(z => new ProductItemInterface2
                                    {
                                        ProcessId = z.ProcessId,
                                        ProcessCode = _db.Processes.Where(k => k.Id == z.ProcessId).Select(k => k.Code).FirstOrDefault(),
                                        ProcessName = _db.Processes.Where(k => k.Id == z.ProcessId).Select(k => k.Name).FirstOrDefault(),

                                        ItemId = z.ProductId,
                                        ItemCode = z.Product.Code,
                                        ItemName = z.Product.Name,
                                        ItemClassification = z.Product.CommonCode.Name,
                                        ItemStandard = z.Product.Standard,
                                        ItemUnit = z.Product.Unit,
                                        Loss = (float)z.Loss,
                                        ProcessPlanQuantity = 0,
                                        RequiredQuantity = (float)z.Require,
                                        TotalRequiredQuantity = (float)(z.Loss * z.Require),
                                        TotalInputQuantity = 0,
                                        Inventory = _db.LotCounts.Where(k => k.Product.Id == z.Product.Id).Where(k => k.IsDeleted == 0).Select(k => (0 - k.DefectiveCount - k.ConsumeCount - k.OutOrderCount + k.StoreOutCount + k.ProduceCount + k.ModifyCount)).Sum(),
                                    }).ToList()

                            }).ToList(),

                        outOrderProductDefectives = _db.OutOrderProductsDefectives
                            .Where(y => y.OutOrderProductDefectiveId == 0)
                            .Select(y => new OutOrderProductDefectiveInterface
                            {

                            }).ToList(),

                    }).ToListAsync();



                var Res = new Response<IEnumerable<OutOrderRes006>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderRes006>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        //수주 리스트 -> LOT Callback (성능 개선)
        public async Task<Response<IEnumerable<ProductDetailResponse>>> orderProductLotList(ProductDetailRequest req)
        { 
            try
            {
                var lots = await _db.Inventory
                    .Where(x => req.ProductIds.Contains(x.Product.Id))
                    .Select(x => new OutOrderProductStockInterface
                    {
                        productId = x.Product.Id,
                        outOrderProductsStockId = 0,
                        productLOT = x.LotName,
                        inventory = x.Total,
                        isSelected = false,
                        quantity = 0,
                        LotCountId = 0,

                        productName = x.Product.Name,
                        productCode = x.Product.Code,
                        productUnit = x.Product.Unit,
                        productClassification = x.Product.CommonCode.Name,
                        productStandard = x.Product.Standard,
                        productShipmentCheck = x.Product.ExportCheck,
                        productShipmentCheckResult = x.Product.ExportCheck ? "" : "미검사"
                }).ToArrayAsync();

                var grouped = lots.GroupBy(x => x.productId).Select(x => new ProductDetailResponse
                {
                    ProductId = x.Key,
                    Stocks = x
                });


                var Res = new Response<IEnumerable<ProductDetailResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = grouped
                };

                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<IEnumerable<ProductDetailResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<OutOrderRes006>>> orderProductListPopProcessList(OutOrderReq002 outOrderRequest)
        {
            try
            {
                var res = await _db.OrderProducts
                    .Where(x => outOrderRequest.outOrderProductIds.Contains(x.OrderProductId))
                    .Select(x => new OutOrderRes006
                    {
                        orderProductId = x.OrderProductId,
                        producePlanProcesses = x.Product.Processes
                                .Where(x => x.IsDeleted == 0)
                                .OrderBy(x=>x.ProcessOrder)
                                .Select(y => new ProductProcessInterface2
                                {
                                    ProductProcessId = y.ProductProcessId,
                                    ProcessId = y.ProcessId,
                                    ProcessCode = y.Process.Code,
                                    ProcessName = y.Process.Name,
                                    ProcessOrder = y.ProcessOrder,

                                    ProductId = y.ProduceProductId,
                                    ProductUnit = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Unit).FirstOrDefault(),
                                    ProductClassification = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.CommonCode.Name).FirstOrDefault(),
                                    ProductCode = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Code).FirstOrDefault(),
                                    ProductName = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Name).FirstOrDefault(),
                                    ProductStandard = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Standard).FirstOrDefault(),

                                    Inventory = _db.LotCounts.Where(z => z.Product.Id == y.ProduceProductId).Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                                    ProcessPlanQuantity = 0,

                                    ProcessInputItems = y.Items
                                        .Where(z => z.IsDeleted == 0)
                                        .Select(z => new ProductItemInterface2
                                        {
                                            ProcessId = z.ProcessId,
                                            ProcessCode = _db.Processes.Where(k => k.Id == z.ProcessId).Select(k => k.Code).FirstOrDefault(),
                                            ProcessName = _db.Processes.Where(k => k.Id == z.ProcessId).Select(k => k.Name).FirstOrDefault(),

                                            ItemId = z.ProductId,
                                            ItemCode = z.Product.Code,
                                            ItemName = z.Product.Name,
                                            ItemClassification = z.Product.CommonCode.Name,
                                            ItemStandard = z.Product.Standard,
                                            ItemUnit = z.Product.Unit,
                                            Loss = (float)z.Loss,
                                            ProcessPlanQuantity = 0,
                                            RequiredQuantity = (float)z.Require,
                                            TotalRequiredQuantity = (float)(z.Loss * z.Require),
                                            TotalInputQuantity = 0,
                                            Inventory = _db.LotCounts.Where(k => k.Product.Id == z.Product.Id).Where(k => k.IsDeleted == 0).Select(k => (0 - k.DefectiveCount - k.ConsumeCount - k.OutOrderCount + k.StoreOutCount + k.ProduceCount + k.ModifyCount)).Sum(),
                                        }).ToList()

                                }).ToList(),

                    }).ToListAsync();

                var Res = new Response<IEnumerable<OutOrderRes006>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<OutOrderRes006>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<OutOrderRes005>>> outOrderProductDefectiveListPop(OutOrderReq002 outOrderRequest)
        {
            try
            {
                var result = await _db.OutOrderProductsDefectives

                    .Where(x => x.OutOrderProduct.OutOrderProductId == outOrderRequest.outOrderProductId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new OutOrderRes005
                    {
                        outOrderProductDefectiveId = x.OutOrderProductDefectiveId,
                        defectiveId = x.Defective.Id,
                        productLOT = x.LotName,
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,
                        defectiveQuantity = x.DefectiveQuantity,
                        defectiveProductMemo = x.Defective.Memo
                    }).ToListAsync();

                var Res = new Response<IEnumerable<OutOrderRes005>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderRes005>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<int>> CreateOutOrder(OutOrderRequestCrud outOrderRequest)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(outOrderRequest.registerId);
                var _partner = await _db.Partners.Where(y => y.Id == outOrderRequest.partnerId).FirstOrDefaultAsync();
                DateTime ShipmentDate = DateTime.ParseExact(outOrderRequest.shipmentDate, "yyyy-MM-dd", null);
                var lastShipment = await _db.OutOrders.Where(x=>x.ShipmentDate == ShipmentDate).OrderByDescending(x => x.ShipmentNo).FirstOrDefaultAsync();
                var _uploadFile = await _db.UploadFiles.Where(y => y.Id == outOrderRequest.uploadFileId).FirstOrDefaultAsync();

                string formatNo = "SN" + ShipmentDate.ToString("yyyyMMdd");
                var _outOrderNumberFormat = string.Format(formatNo + "{0:0000#}", (lastShipment == null? 1 : Convert.ToInt32(lastShipment.ShipmentNo.Substring(lastShipment.ShipmentNo.Length - 5)) + 1));

                var _outOrder = new OutOrder()
                {
                    Partner = _partner, //거래처
                    UploadFiles = outOrderRequest.UploadFiles, //업로드파일
                    Register = _user, //등록자
                    ShipmentNo = _outOrderNumberFormat, //출고번호
                    ShipmentDate = ShipmentDate, //출고일
                    ShipmentMemo = outOrderRequest.shipmentMemo, //비고

                };

                var result = await _db.OutOrders.AddAsync(_outOrder);
                await Save();

                var Res = new Response<int>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _outOrder.OutOrderId,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<int>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = 0,
                };
                return Res;
            }
        }

        public async Task<Response<int>> UpdateOutOrder(OutOrderRequestCrud outOrderRequest)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(outOrderRequest.registerId);
                var _partner = await _db.Partners.Where(y => y.Id == outOrderRequest.partnerId).FirstOrDefaultAsync();
                DateTime ShipmentDate = DateTime.ParseExact(outOrderRequest.shipmentDate, "yyyy-MM-dd", null);
               
                var _uploadFile = await _db.UploadFiles.Where(y => y.Id == outOrderRequest.uploadFileId).FirstOrDefaultAsync();
                var _outOrder = await _db.OutOrders.Include(x=>x.UploadFiles).Where(x => x.OutOrderId == outOrderRequest.outOrderId).FirstOrDefaultAsync();

                _outOrder.Partner = _partner;//거래처


                if (_outOrder.UploadFiles != null)
                {
                    _db.UploadFiles.RemoveRange(_outOrder.UploadFiles);
                }
                _outOrder.UploadFiles = outOrderRequest.UploadFiles; //업로드파일

                _outOrder.ShipmentMemo = outOrderRequest.shipmentMemo; //비고
                _outOrder.Register = _user; //등록자
              //  _outOrder.ShipmentNo = outOrderRequest.shipmentNo; //입고번호
                _outOrder.ShipmentDate = ShipmentDate; //출고일
                
                _db.OutOrders.Update(_outOrder);

                await Save();

                var Res = new Response<int>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = outOrderRequest.outOrderId,
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<int>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = 0,
                };
                return Res;
            }
        }

        public async Task<Response<bool>> DeleteOutOrder(OutOrderRequestCrud outOrderRequest)
        {
            try
            {
                foreach (int item in outOrderRequest.outOrderIdArray)
                {
                    var subItem = await _db.OutOrderProducts
                        .Include(x=>x.OutOrderProductStock)
                        .ThenInclude(x=>x.Lot)
                        .ThenInclude(x=>x.LotCounts)
                        .Where(x => x.OutOrder.OutOrderId == item).ToListAsync();

                    foreach (var sub in subItem)
                    {
                        sub.IsDeleted = 1;

                        foreach(var i in sub.OutOrderProductStock)
                        {
                            i.IsDeleted = 1;
                            if (i.Lot != null)
                            {
                                i.Lot.IsDeleted = 1;
                                if (i.Lot.LotCounts != null)
                                {
                                    i.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;
                                }
                            }
                        }

                        var defs = _db.OutOrderProductsDefectives
                            .Include(x => x.Lot)
                            .ThenInclude(x => x.LotCounts)
                            .Where(x => x.OutOrderProduct == sub)
                            .ToList();

                        foreach(var i in defs)
                        {
                            i.IsDeleted = 1;
                            if (i.Lot != null)
                            {
                                i.Lot.IsDeleted = 1;
                                if(i.Lot.LotCounts != null)
                                {
                                    i.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;
                                }
                            }
                        }
                    }
                    _db.OutOrderProducts.UpdateRange(subItem);



                   
                    var mst = await _db.OutOrders.FindAsync(item);
                    mst.IsDeleted = 1;
                    _db.OutOrders.Update(mst);
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

        public async Task<Response<bool>> CreateOutOrderProductLot(OutOrderProductRequestCrud outOrderProductRequest)
        {
            try
            {
                var _outOrder = await _db.OutOrders.Where(x => x.OutOrderId == outOrderProductRequest.outOrderId).FirstOrDefaultAsync();

                foreach (var outOrderProducts in outOrderProductRequest.outOrderProductArray)
                {
                    var _orderProduct = await _db.OrderProducts.Include(x => x.Product).Where(x => x.OrderProductId == outOrderProducts.orderProductId).FirstOrDefaultAsync();
                    var _outOrderProduct = new OutOrderProduct()
                    {
                        OutOrder = _outOrder,
                        OrderProduct = _orderProduct,
                        ProductId = _orderProduct != null ? _orderProduct.ProductId : outOrderProducts.productId,
                        Product = _orderProduct != null ? _orderProduct.Product : _db.Products.Where(x => x.Id == outOrderProducts.productId).FirstOrDefault(), // not mapped
                        Quantity = outOrderProducts.quantity,
                        ProductShipmentCount = outOrderProducts.productShipmentCount,
                        ProductSellPrice = outOrderProducts.productSellPrice,
                        ProductSupplyPrice = outOrderProducts.productSupplyPrice,
                        ProductTaxPrice = outOrderProducts.productTaxPrice,
                        ProductTotalPrice = outOrderProducts.ProductTotalPrice,
                        ShipmentProductMemo = outOrderProducts.shipmentProductMemo,
                    };

                    var result_outPrd = await _db.OutOrderProducts.AddAsync(_outOrderProduct);


                    foreach(var stock in outOrderProducts.OutOrderProductStocks)
                    {
                        if(!stock.isSelected && stock.outOrderProductsStockId == 0)
                        {
                            continue;
                        }

                        if(stock.outOrderProductsStockId == 0 && stock.isSelected)
                        {
                            var result_outPrdStkLot = _db.Lots.Add(new LotEntity
                            {
                                LotName = stock.productLOT,
                                ProcessType = "O"
                            });

                            _db.LotCounts.Add(new LotCount
                            {
                                Lot = result_outPrdStkLot.Entity,
                                Product = _db.Products.Where(x=>x.Id == stock.productId).FirstOrDefault(),
                                OutOrderCount = stock.quantity,
                            });

                            var result_outPrdStk =  await _db.OutOrderProductsStocks.AddAsync(new OutOrderProductStock
                            {
                                Lot = result_outPrdStkLot.Entity,
                                OutOrderProduct = result_outPrd.Entity,
                                LotName = stock.productLOT,
                                ProductShipmentCheckResult = stock.productShipmentCheckResult,
                            });
                        }
                        if(stock.outOrderProductsStockId != 0 && !stock.isSelected)
                        {
                            var delete_stk = _db.OutOrderProductsStocks
                                .Include(x=>x.Lot).ThenInclude(x=>x.LotCounts)
                                .Where(x => x.OutOrderProductStockId == stock.outOrderProductsStockId)
                                .FirstOrDefault();

                            delete_stk.IsDeleted = 1;
                            delete_stk.Lot.IsDeleted = 1;
                            foreach(var i in delete_stk.Lot.LotCounts)
                            {
                                i.IsDeleted = 1;
                            }
                            _db.OutOrderProductsStocks.Update(delete_stk);


                        }
                        if(stock.outOrderProductsStockId != 0 && stock.isSelected)
                        {
                            var update_stk = _db.OutOrderProductsStocks
                                .Include(x => x.Lot).ThenInclude(x => x.LotCounts)
                                .Where(x => x.OutOrderProductStockId == stock.outOrderProductsStockId)
                                .FirstOrDefault();

                            update_stk.ProductShipmentCheckResult = stock.productShipmentCheckResult;

                            foreach (var i in update_stk.Lot.LotCounts)
                            {
                                i.OutOrderCount = stock.quantity;
                            }
                            _db.OutOrderProductsStocks.Update(update_stk);
                        }
                    }

                   // bool flag = false;

                   // var prevDefectives = _db.OutOrderProductsDefectives.Where(x=>x.OutOrderProduct ==).ToList

                    foreach(var def in outOrderProducts.OutOrderProductDefectives)
                    {
                     //   flag = false;
                        if(def.outOrderProductDefectiveId != 0)
                        {
                            //Update
                        }

                        if(def.outOrderProductDefectiveId == 0)
                        {
                            var result = _db.Lots.Add(new LotEntity
                            {
                                LotName = def.productLOT,
                                ProcessType = "O"
                            });

                            _db.LotCounts.Add(new LotCount
                            {
                                Product = _db.Products.Where(x=>x.Id == outOrderProducts.productId).FirstOrDefault(),
                                Lot = result.Entity,
                                DefectiveCount = def.defectiveQuantity,
                            });

                            _db.OutOrderProductsDefectives.Add(new OutOrderProductDefective
                            {
                                Defective = _db.Defectives
                                    .Where(x=>x.IsDeleted == false)
                                    .Where(x=>x.Code == def.defectiveCode)
                                    .FirstOrDefault(),
                                DefectiveProductMemo = def.defectiveProductMemo,
                                DefectiveQuantity = def.defectiveQuantity,
                                Lot = result.Entity,
                                LotName = def.productLOT,
                                OutOrderProduct = result_outPrd.Entity,
                                
                            });
                        }
                    }
                    //Delete
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
        public async Task<Response<bool>> CreateOutOrderProductforProduct(OutOrderProductRequestCrud outOrderProductRequest)
        {
            try
            {
                var _product = await _db.Products.Where(x => x.Id == outOrderProductRequest.productId).FirstOrDefaultAsync();
                var _outOrder = await _db.OutOrders.Where(x => x.OutOrderId == outOrderProductRequest.outOrderId).FirstOrDefaultAsync();

                var _outOrderProduct = new OutOrderProduct()
                {
                    OutOrder = _outOrder,
                    Product = _product,
                    Quantity = 0,//todo
                    ProductShipmentCount = outOrderProductRequest.productShipmentCount,
                    ProductSellPrice = outOrderProductRequest.productSellPrice,
                    ProductSupplyPrice = 0, //todo
                    ProductTaxPrice = 0, //todo
                    ProductTotalPrice = 0, //todo
                    ShipmentProductMemo = outOrderProductRequest.shipmentProductMemo,
                };
                await _db.OutOrderProducts.AddAsync(_outOrderProduct);

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

        public async Task<Response<bool>> UpdateOutOrderProduct(OutOrderProductRequestCrud outOrderProductRequest)
        {
            try
            {
                var _outOrder = await _db.OutOrders.Where(x => x.OutOrderId == outOrderProductRequest.outOrderId).FirstOrDefaultAsync();

                var orgPrds = _db.OutOrderProducts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.OutOrder == _outOrder)
                    .Select(x => x.OutOrderProductId)
                    .ToList();

                List<int> ids = new List<int>();

                foreach (var outOrderProducts in outOrderProductRequest.outOrderProductArray)
                {
                    ids.Add(outOrderProducts.outOrderProductId);

                    if(outOrderProducts.outOrderProductId == 0)
                    {
                        var _createOrderPrd = await _db.OrderProducts.Include(x => x.Product).Where(x => x.OrderProductId == outOrderProducts.orderProductId).FirstOrDefaultAsync();
                        var _createOutOrderProduct = new OutOrderProduct()
                        {
                            OutOrder = _outOrder,
                            OrderProduct = _createOrderPrd,
                            ProductId = _createOrderPrd != null ? _createOrderPrd.ProductId : outOrderProducts.productId,
                            Product = _createOrderPrd != null ? _createOrderPrd.Product : _db.Products.Where(x => x.Id == outOrderProducts.productId).FirstOrDefault(), // not mapped
                            Quantity = outOrderProducts.quantity,
                            ProductShipmentCount = outOrderProducts.productShipmentCount,
                            ProductSellPrice = outOrderProducts.productSellPrice,
                            ProductSupplyPrice = outOrderProducts.productSupplyPrice,
                            ProductTaxPrice = outOrderProducts.productTaxPrice,
                            ProductTotalPrice = outOrderProducts.ProductTotalPrice,
                            ShipmentProductMemo = outOrderProducts.shipmentProductMemo,
                        };

                        var result_createOutPrd = await _db.OutOrderProducts.AddAsync(_createOutOrderProduct);

                        foreach (var stock in outOrderProducts.OutOrderProductStocks)
                        {
                            if (!stock.isSelected && stock.outOrderProductsStockId == 0)
                            {
                                continue;
                            }

                            if (stock.outOrderProductsStockId == 0 && stock.isSelected)
                            {
                                var result_outPrdStkLot = _db.Lots.Add(new LotEntity
                                {
                                    LotName = stock.productLOT,
                                    ProcessType = "O"
                                });

                                _db.LotCounts.Add(new LotCount
                                {
                                    Lot = result_outPrdStkLot.Entity,
                                    Product = _db.Products.Where(x => x.Id == stock.productId).FirstOrDefault(),
                                    OutOrderCount = stock.quantity,
                                });

                                var result_outPrdStk = await _db.OutOrderProductsStocks.AddAsync(new OutOrderProductStock
                                {
                                    Lot = result_outPrdStkLot.Entity,
                                    OutOrderProduct = result_createOutPrd.Entity,
                                    LotName = stock.productLOT,
                                    ProductShipmentCheckResult = stock.productShipmentCheckResult,
                                });
                            }
                        }


                        foreach (var def in outOrderProducts.OutOrderProductDefectives)
                        {
                            if (def.outOrderProductDefectiveId == 0)
                            {
                                var result = _db.Lots.Add(new LotEntity
                                {
                                    LotName = def.productLOT,
                                    ProcessType = "O"
                                });

                                _db.LotCounts.Add(new LotCount
                                {
                                    Product = _db.Products.Where(x => x.Id == outOrderProducts.productId).FirstOrDefault(),
                                    Lot = result.Entity,
                                    DefectiveCount = def.defectiveQuantity,
                                });

                                _db.OutOrderProductsDefectives.Add(new OutOrderProductDefective
                                {
                                    Defective = _db.Defectives
                                        .Where(x => x.IsDeleted == false)
                                        .Where(x => x.Code == def.defectiveCode)
                                        .FirstOrDefault(),
                                    DefectiveProductMemo = def.defectiveProductMemo,
                                    DefectiveQuantity = def.defectiveQuantity,
                                    Lot = result.Entity,
                                    LotName = def.productLOT,
                                    OutOrderProduct = result_createOutPrd.Entity,
                                });
                            }
                        }

                    }
                    //Update outOrderPrd
                    else
                    {
                        var _orderProduct = await _db.OrderProducts.Include(x => x.Product).Where(x => x.OrderProductId == outOrderProducts.orderProductId).FirstOrDefaultAsync();
                        var _outOrderProduct = new OutOrderProduct()
                        {
                            OutOrder = _outOrder,
                            OrderProduct = _orderProduct,
                            ProductId = _orderProduct != null ? _orderProduct.ProductId : outOrderProducts.productId,
                            Product = _orderProduct != null ? _orderProduct.Product : _db.Products.Where(x => x.Id == outOrderProducts.productId).FirstOrDefault(), // not mapped
                            Quantity = outOrderProducts.quantity,
                            ProductShipmentCount = outOrderProducts.productShipmentCount,
                            ProductSellPrice = outOrderProducts.productSellPrice,
                            ProductSupplyPrice = outOrderProducts.productSupplyPrice,
                            ProductTaxPrice = outOrderProducts.productTaxPrice,
                            ProductTotalPrice = outOrderProducts.ProductTotalPrice,
                            ShipmentProductMemo = outOrderProducts.shipmentProductMemo,
                        };

                        var _updateOutOrderProduct = _db.OutOrderProducts.Where(x=>x.OutOrderProductId == outOrderProducts.outOrderProductId).FirstOrDefault();
                        _updateOutOrderProduct.Quantity = outOrderProducts.quantity;
                        _updateOutOrderProduct.ProductShipmentCount = outOrderProducts.productShipmentCount;
                        _updateOutOrderProduct.ProductSellPrice = outOrderProducts.productSellPrice;
                        _updateOutOrderProduct.ProductSupplyPrice = outOrderProducts.productSupplyPrice;
                        _updateOutOrderProduct.ProductTaxPrice = outOrderProducts.productTaxPrice;
                        _updateOutOrderProduct.ProductTotalPrice = outOrderProducts.ProductTotalPrice;
                        _updateOutOrderProduct.ShipmentProductMemo = outOrderProducts.shipmentProductMemo;

                        _db.OutOrderProducts.Update(_updateOutOrderProduct);



                        foreach (var stock in outOrderProducts.OutOrderProductStocks)
                        {
                            if (!stock.isSelected && stock.outOrderProductsStockId == 0)
                            {
                                continue;
                            }

                            if (stock.outOrderProductsStockId == 0 && stock.isSelected)
                            {
                                var result_outPrdStkLot = _db.Lots.Add(new LotEntity
                                {
                                    LotName = stock.productLOT,
                                    ProcessType = "O"
                                });

                                _db.LotCounts.Add(new LotCount
                                {
                                    Lot = result_outPrdStkLot.Entity,
                                    Product = _db.Products.Where(x => x.Id == stock.productId).FirstOrDefault(),
                                    OutOrderCount = stock.quantity,
                                });

                                var result_outPrdStk = await _db.OutOrderProductsStocks.AddAsync(new OutOrderProductStock
                                {
                                    Lot = result_outPrdStkLot.Entity,
                                    OutOrderProduct = _updateOutOrderProduct,
                                    LotName = stock.productLOT,
                                    ProductShipmentCheckResult = stock.productShipmentCheckResult,
                                });
                            }
                            if (stock.outOrderProductsStockId != 0 && !stock.isSelected)
                            {
                                var delete_stk = _db.OutOrderProductsStocks
                                    .Include(x => x.Lot).ThenInclude(x => x.LotCounts)
                                    .Where(x => x.OutOrderProductStockId == stock.outOrderProductsStockId)
                                    .FirstOrDefault();

                                delete_stk.IsDeleted = 1;
                                delete_stk.Lot.IsDeleted = 1;
                                foreach (var i in delete_stk.Lot.LotCounts)
                                {
                                    i.IsDeleted = 1;
                                }
                                _db.OutOrderProductsStocks.Update(delete_stk);
                            }
                            if (stock.outOrderProductsStockId != 0 && stock.isSelected)
                            {
                                var update_stk = _db.OutOrderProductsStocks
                                    .Include(x => x.Lot).ThenInclude(x => x.LotCounts)
                                    .Where(x => x.OutOrderProductStockId == stock.outOrderProductsStockId)
                                    .FirstOrDefault();

                                update_stk.ProductShipmentCheckResult = stock.productShipmentCheckResult;

                                foreach (var i in update_stk.Lot.LotCounts)
                                {
                                    i.OutOrderCount = stock.quantity;
                                }
                                _db.OutOrderProductsStocks.Update(update_stk);
                            }
                        }

                        var orgDef = _db.OutOrderProductsDefectives.Where(x => x.OutOrderProduct.OutOrderProductId == outOrderProducts.outOrderProductId).Select(x => x.OutOrderProductDefectiveId).ToList();
                        bool flag1 = false;

                        foreach(var def in outOrderProducts.OutOrderProductDefectives)
                        {
                            if (def.outOrderProductDefectiveId != 0)
                            {
                                var _updateDef = _db.OutOrderProductsDefectives.Include(x=>x.Lot).ThenInclude(x=>x.LotCounts).Where(x => x.OutOrderProductDefectiveId == def.outOrderProductDefectiveId).FirstOrDefault();
                                _updateDef.DefectiveQuantity = def.defectiveQuantity;
                                _updateDef.DefectiveProductMemo = def.defectiveProductMemo;
                                _updateDef.Lot.LotCounts.FirstOrDefault().DefectiveCount = def.defectiveQuantity;
                            }
                            else
                            {
                                var result = _db.Lots.Add(new LotEntity
                                {
                                    LotName = def.productLOT,
                                    ProcessType = "O"
                                });

                                _db.LotCounts.Add(new LotCount
                                {
                                    Product = _db.Products.Where(x => x.Id == outOrderProducts.productId).FirstOrDefault(),
                                    Lot = result.Entity,
                                    DefectiveCount = def.defectiveQuantity,
                                });

                                _db.OutOrderProductsDefectives.Add(new OutOrderProductDefective
                                {
                                    Defective = _db.Defectives
                                        .Where(x => x.IsDeleted == false)
                                        .Where(x => x.Code == def.defectiveCode)
                                        .FirstOrDefault(),
                                    DefectiveProductMemo = def.defectiveProductMemo,
                                    DefectiveQuantity = def.defectiveQuantity,
                                    Lot = result.Entity,
                                    LotName = def.productLOT,
                                    OutOrderProduct = _updateOutOrderProduct,
                                });
                            }
                        }

                        foreach(var i in orgDef)
                        {
                            flag1 = false;
                            foreach (var def in outOrderProducts.OutOrderProductDefectives)
                            {
                                if (i == def.outOrderProductDefectiveId)
                                {
                                    flag1 = true;
                                }
                            }

                            if (!flag1)
                            {
                                var _deleteDef = _db.OutOrderProductsDefectives.Include(x=>x.Lot).ThenInclude(x=>x.LotCounts).Where(x => x.OutOrderProductDefectiveId == i).FirstOrDefault();

                                _deleteDef.IsDeleted = 1;
                                _deleteDef.Lot.IsDeleted = 1;

                                foreach(var j in _deleteDef.Lot.LotCounts)
                                {
                                    j.IsDeleted = 1;
                                }
                                _db.OutOrderProductsDefectives.Update(_deleteDef);
                            }
                        }
                  
                    }
                }


                bool flag = false;
                foreach(int z in orgPrds)
                {
                    flag = false;
                    foreach(var y in outOrderProductRequest.outOrderProductArray)
                    {
                        if(z == y.outOrderProductId)
                        {
                            flag = true;
                        }
                    }

                    if (!flag)
                    {
                        var _prdDelete = _db.OutOrderProducts
                            .Include(x => x.IsDeleted == 0)
                            .Include(x => x.OutOrderProductStock)
                            .ThenInclude(x => x.Lot)
                            .ThenInclude(x => x.LotCounts)
                            .Where(x => x.OutOrderProductId == z)
                            .FirstOrDefault();

                        _prdDelete.IsDeleted = 1;
                        
                        foreach(var i in _prdDelete.OutOrderProductStock)
                        {
                            i.IsDeleted = 1;
                            i.Lot.IsDeleted = 1;
                            i.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;
                        }

                        _db.OutOrderProducts.Update(_prdDelete);


                        var _defDelete = _db.OutOrderProductsDefectives
                            .Include(x => x.Lot)
                            .ThenInclude(x => x.LotCounts)
                            .Where(x => x.IsDeleted == 0)
                            .Where(x => x.OutOrderProduct == _prdDelete)
                            .ToList();

                        foreach(var i in _defDelete)
                        {
                            i.IsDeleted = 1;
                            i.Lot.IsDeleted = 1;
                            i.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;
                        }

                        _db.OutOrderProductsDefectives.UpdateRange(_defDelete);
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
        public async Task<Response<bool>> UpdateOutOrderProductforProduct(OutOrderProductRequestCrud outOrderProductRequest)
        {
            try
            {

                var _product = await _db.Products.Where(x => x.Id == outOrderProductRequest.productId).FirstOrDefaultAsync();
                var _outOrder = await _db.OutOrders.Where(x => x.OutOrderId == outOrderProductRequest.outOrderId).FirstOrDefaultAsync();
                var _outOrderProduct = new OutOrderProduct()
                {
                    OutOrderProductId = outOrderProductRequest.outOrderProductId,
                    OutOrder = _outOrder,
                    Product = _product,
                    Quantity = 0,//todo
                    ProductShipmentCount = outOrderProductRequest.productShipmentCount,
                    ProductSellPrice = _product.Item != null ? _product.Item.SellPrice : 0,
                    ProductSupplyPrice = 0, //todo
                    ProductTaxPrice = 0, //todo
                    ProductTotalPrice = 0, //todo
                    ShipmentProductMemo = outOrderProductRequest.shipmentProductMemo,
                };
                _db.OutOrderProducts.Update(_outOrderProduct);

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

        public async Task<Response<bool>> DeleteOutOrderProduct(OutOrderProductRequestCrud outOrderProductRequest)
        {
            try
            {

                foreach (int item in outOrderProductRequest.outOrderProductIdArray)
                {
                    var _items = await _db.OutOrderProducts.FindAsync(item);
                    _items.IsDeleted = 1;
                    _db.OutOrderProducts.Update(_items);
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
        
        public async Task<Response<bool>> CreateOutOrderProductDefective(OutOrderReq002 outOrderReq002)
        {
            try
            {
                foreach (OutOrderReq002 req in outOrderReq002.defectiveArray)
                {
                    var _outOrderProduct = await _db.OutOrderProducts.Where(x => x.OutOrderProductId == outOrderReq002.outOrderProductId).FirstOrDefaultAsync();
                    var _defectives = await _db.Defectives.Where(x => x.Id == req.defectiveId).FirstOrDefaultAsync();
                    var _outOrderProductDefective = new OutOrderProductDefective()
                    {
                        OutOrderProduct = _outOrderProduct,
                        Defective = _defectives,
                        DefectiveQuantity = req.defectiveQuantity,
                        LotName = req.lotName
                    };
                    await _db.OutOrderProductsDefectives.AddAsync(_outOrderProductDefective);

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
        public async Task<Response<bool>> UpdateOutOrderProductDefective(OutOrderReq002 outOrderReq002)
        {
            try
            {
    
                    var _outOrderProduct = await _db.OutOrderProducts.Where(x => x.OutOrderProductId == outOrderReq002.outOrderProductId).FirstOrDefaultAsync();
                    var _defectives = await _db.Defectives.Where(x => x.Id == outOrderReq002.defectiveId).FirstOrDefaultAsync();
                    var _outOrderProductDefective = new OutOrderProductDefective()
                    {
                        OutOrderProductDefectiveId = outOrderReq002.outOrderProductDefectiveId,
                        OutOrderProduct = _outOrderProduct,
                        Defective = _defectives,
                        DefectiveQuantity = outOrderReq002.defectiveQuantity,
                        LotName = outOrderReq002.lotName
                    };
                     _db.OutOrderProductsDefectives.Update(_outOrderProductDefective);



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
        public async Task<Response<bool>> DeleteOutOrderProductDefective(OutOrderReq002 outOrderReq002)
        {
            try
            {
                foreach (int item in outOrderReq002.outOrderProductDefectiveIdArray)
                {
                    var _items = await _db.OutOrderProductsDefectives.FindAsync(item);
                    _items.IsDeleted = 1;
                    _db.OutOrderProductsDefectives.Update(_items);
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
        

        public async Task<Response<bool>> CreateOutOrderProductsStock2(OutOrderReq006 OutOrderReq006)
        {
            try
            {


                foreach (OutOrderReq006 req in OutOrderReq006.outOrderProductsStockArray)
                {
                    var _outOrderProduct = await _db.OutOrderProducts.Where(x => x.OutOrderProductId == req.outOrderProductId).FirstOrDefaultAsync();
                    // lot 정보조회
                    var _lotEntity = await _db.Lots.Where(x => req.productLOT == x.LotName).FirstOrDefaultAsync();
                    var _outOrderProductStock = new OutOrderProductStock()
                    {
                        OutOrderProduct = _outOrderProduct,
                        ProductShipmentCheckResult = req.ProductShipmentCheckResult,
                        LotName = req.productLOT,
                        Lot = _lotEntity // lot 정보 세
                    };
                    await _db.OutOrderProductsStocks.AddAsync(_outOrderProductStock);

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
        public async Task<Response<bool>> UpdateOutOrderProductsStock(OutOrderReq006 OutOrderReq006)
        {
            try
            {


                    var _outOrderProduct = await _db.OutOrderProducts.Where(x => x.OutOrderProductId == OutOrderReq006.outOrderProductId).FirstOrDefaultAsync();
                    var _outOrderProductStock = new OutOrderProductStock()
                    {
                        OutOrderProductStockId = OutOrderReq006.outOrderProductStockId,
                        OutOrderProduct = _outOrderProduct,
                        ProductShipmentCheckResult = OutOrderReq006.ProductShipmentCheckResult,
                        LotName = OutOrderReq006.productLOT,
                        IsDeleted = OutOrderReq006.isDeleted
                    };
                    _db.OutOrderProductsStocks.Update(_outOrderProductStock);


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
        public async Task<Response<bool>> DeleteOutOrderProductsStock(OutOrderReq006 OutOrderReq006)
        {
            try
            {
                foreach (int item in OutOrderReq006.outOrderProductStockIdArray)
                {
                    var _items = await _db.OutOrderProductsStocks.FindAsync(item);
                    _items.IsDeleted = 1;
                    _db.OutOrderProductsStocks.Update(_items);
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

        public async Task<Response<bool>> CreateOutOrderProductStock(OutOrderReq005 outOrderReq005)
        {
            try
            {
                int quantity = 0;
                int outOrderProductId = 0;
                foreach (OutOrderReq005 req in outOrderReq005.outOrderProductsStockArray)
                {
                    var _outOrderProduct = await _db.OutOrderProducts.Where(x => x.OutOrderProductId == req.outOrderProductId).FirstOrDefaultAsync();
                    var _outOrderProductStocks = new OutOrderProductStock()
                    {
                        
                        OutOrderProduct = _outOrderProduct,
                        LotName = req.productLOT,
                        ProductShipmentCheckResult = req.productShipmentCheckResult,

                    };
                    await _db.OutOrderProductsStocks.AddAsync(_outOrderProductStocks);
                    quantity += req.quantity;
                    outOrderProductId = req.outOrderProductId;

                    //lot insert event
                    //1. lotname으로 찾는다.
                    //2. 동일한데 type을 출고로만 수정 후, lot, lotcount에 넣어준다.

                    var lot = await _db.Lots.Where(x=>x.ProcessType == "I" || x.ProcessType == "P").Where(x => x.LotName == req.productLOT).FirstOrDefaultAsync();
                    var lotcount = await _db.LotCounts.Include(x=>x.Product).Where(x => x.Lot.LotId == lot.LotId).FirstOrDefaultAsync();

                    lot.LotId = 0;
                    lot.ProcessType = "O";
                    var result = await _db.Lots.AddAsync(lot);

                    var _lotCount = new LotCount();
                    {
                        _lotCount.Product = lotcount.Product;
                        _lotCount.Lot = result.Entity;
                        _lotCount.OutOrderCount = req.quantity;
                    };

                    await _db.LotCounts.AddAsync(_lotCount);



                }
                var _outOrderProduct2 = await _db.OutOrderProducts.Include(x=>x.OrderProduct) .Include(x=>x.Product).Where(x => x.OutOrderProductId == outOrderProductId).FirstOrDefaultAsync();
                {
                    _outOrderProduct2.Quantity = quantity;
                }
                _db.OutOrderProducts.Update(_outOrderProduct2);
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





        public async Task<Response<IEnumerable<OutOrderRes007>>> orderMstList(OutOrderReq004 outOrderReq004)
        {
            try
            {

                var _order = await _db.Orders

                    .Include(x => x.OrderProducts).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)

                 //   .Where(x => outOrderReq004.orderNo == "" ? true : x.OrderNo == outOrderReq004.orderNo)
                    .Where(x => outOrderReq004.orderStartDate == "" ? true : x.OrderDate >= Convert.ToDateTime(outOrderReq004.orderStartDate))
                    .Where(x => outOrderReq004.orderEndDate == "" ? true : x.OrderDate <= Convert.ToDateTime(outOrderReq004.orderEndDate))
                    .Where(x => outOrderReq004.partnerId == 0 ? true : x.Partner.Id == outOrderReq004.partnerId)
                    .Where(x => outOrderReq004.orderStatus == "" ? true : false)
                    .Where(x => outOrderReq004.productId == 0 ? true :

                        x.OrderProducts.Where(x => x.Product.Id == outOrderReq004.productId)
                        .Select(x => x.Product.Id).ToArray()
                        .Contains(outOrderReq004.productId)
                    )
                    .Where(x => x.OrderNo.Contains(outOrderReq004.orderNo))
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x => x.OrderDate)
                    .Select(x => new OutOrderRes007
                    {
                        orderId = x.OrderId,
                        orderNo = x.OrderNo, //수주번호
                        orderDate = x.OrderDate.ToString("yyyy-MM-dd"), //수주일
                        requestDeliveryDate = x.RequestDeliveryDate.ToString("yyyy-MM-dd"), //남품요청일
                        partnerName = x.Partner.Name, // 거래처이름
                        partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                        orderProductCount = x.OrderProducts.Select(x => x.ProductOrderCount).Count(),
                        shipmentCompletedCount = x.OrderProducts.Select(op => op).Where(x => x.OrderStatus == "출하완료").Count(),
                        orderStatus = x.OrderProducts.Where(y => y.IsDeleted == 0).Count() == x.OrderProducts.Where(y => y.IsDeleted == 0).Where(y => y.OrderStatus == "수주등록").Count() ? "수주등록" : x.OrderProducts.Where(y => y.IsDeleted == 0).Count() == x.OrderProducts.Where(y => y.IsDeleted == 0).Where(y => y.OrderStatus == "출하완료").Count() ? "출하완료" : "출하대기",
                        orderMemo = x.OrderMemo,//비고
                    }).ToListAsync();

                var Res = new Response<IEnumerable<OutOrderRes007>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _order
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderRes007>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<OutOrderRes008>> orderMstPop(OutOrderReq004 outOrderReq004)
        {
            try
            {

                var _order = await _db.Orders
                    //.Include(x => x.pro).ThenInclude(OrderItems => OrderItems.Item).ThenInclude(item => item.CommonCode)
                    .Include(x => x.OrderProducts).ThenInclude(x => x.Product)
                    .Include(x=> x.UploadFiles)
                    //.Include(x => x.CommonCode)
                    .Include(x => x.Partner)
                    .Where(x => x.OrderId == outOrderReq004.orderId)
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x => x.OrderDate)
                    .Select(x => new OutOrderRes008
                    {

                        orderId = x.OrderId,
                        orderNo = x.OrderNo,
                        orderDate = x.OrderDate.ToString("yyyy-MM-dd"),
                        requestDeliveryDate = x.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                        orderStatus = x.OrderProducts.Where(y => y.IsDeleted == 0).Count() == x.OrderProducts.Where(y => y.IsDeleted == 0).Where(y => y.OrderStatus == "수주등록").Count() ? "수주등록" : x.OrderProducts.Where(y => y.IsDeleted == 0).Count() == x.OrderProducts.Where(y => y.IsDeleted == 0).Where(y => y.OrderStatus == "출하완료").Count() ? "출하완료" : "출하대기",

                        registerId = x.Register.Id,
                        registerName = x.Register.FullName,
                        partnerId = x.Partner.Id,
                        partnerCode = x.Partner.Code,
                        partnerName = x.Partner.Name,
                        contactName = x.Partner.ContactName,
                        telephoneNumber = x.Partner.TelephoneNumber,
                        faxNumber = x.Partner.FaxNumber,
                        contactEmail = x.Partner.ContactEmail,
                        partnerTaxInfo = x.Partner.TaxInfo,
                        orderMemo = x.OrderMemo,
                        UploadFiles = x.UploadFiles.ToArray(),

                        shipmentManageMemo = ""

                    }).FirstOrDefaultAsync();

                var Res = new Response<OutOrderRes008>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _order
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<OutOrderRes008>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        //출고관리
        //1) 수주품목 클릭, 출고마스터리스트
        public async Task<Response<IEnumerable<OutOrderRes001>>> outOrderMstList2(OutOrderReq001 outOrderRequest)
        {
            try
            {

                var _outOrderProduct = await _db.OutOrderProducts

                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                    .Include(x => x.OrderProduct.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                    .Where(x => x.OrderProduct.OrderProductId == outOrderRequest.orderProductId)
                    .OrderBy(x => x.OutOrder.ShipmentDate)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new OutOrderRes001
                    {
                        outOrderId = x.OutOrder.OutOrderId,
                        shipmentNo = x.OutOrder.ShipmentNo,
                        shipmentDate = x.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                        partnerName = x.OutOrder.Partner.Name, // 거래처이름
                        partnerTaxInfo = x.OutOrder.Partner.TaxInfo,//거래처과세정보
                        shipmentProductCount = x.OutOrder.OutOrderProducts.Select(OOP => OOP).Count(),
                        shipmentSupplyPrice = x.OutOrder.OutOrderProducts.Select(OOP => OOP.OrderProduct.ProductSupplyPrice).Sum(),
                        shipmentTaxPrice = x.OutOrder.OutOrderProducts.Select(OOP => OOP.OrderProduct.ProductTaxPrice).Sum(),
                        shipmentTotalPrice = x.OutOrder.OutOrderProducts.Select(OOP => OOP.OrderProduct.ProductTotalPrice).Sum(),
                        registerName = x.OutOrder.Register.FullName,//등록자
                        shipmentMemo = x.OutOrder.ShipmentMemo,
                       // uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                      //  uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                    }).ToListAsync();

                var Res = new Response<IEnumerable<OutOrderRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _outOrderProduct
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<OutOrderRes010>>> outOrderProductList2(OutOrderReq001 outOrderRequest)
        {
            try
            {

                var _outOrderProduct = await _db.OutOrderProducts

                   .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                   .Include(x => x.OrderProduct.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                    .Where(x =>  x.OutOrder.ShipmentNo.Contains(outOrderRequest.shipmentNo))
                    .Where(x => outOrderRequest.shipmentStartDate == "" ? x.OutOrder.ShipmentDate >= DateTime.Today : x.OutOrder.ShipmentDate >= Convert.ToDateTime(outOrderRequest.shipmentStartDate))
                    .Where(x => outOrderRequest.shipmentEndDate == "" ? x.OutOrder.ShipmentDate <= DateTime.Today.AddMonths(1) : x.OutOrder.ShipmentDate <= Convert.ToDateTime(outOrderRequest.shipmentEndDate))
                    .Where(x => outOrderRequest.partnerId == 0 ? true : x.OutOrder.Partner.Id == outOrderRequest.partnerId)


                    .Where(x => outOrderRequest.productId == 0 ? true : x.Product.Id == outOrderRequest.productId)
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x => x.OutOrder.ShipmentDate)

                    .Select(x => new OutOrderRes010
                    {
                        outOrderId = x.OutOrder.OutOrderId,
                        outOrderProductId = x.OutOrderProductId,
                        shipmentNo = x.OutOrder.ShipmentNo,
                        shipmentDate = x.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                        partnerName = x.OutOrder.Partner.Name, // 거래처이름
                        productCode = x.OrderProduct != null ? x.OrderProduct.Product.Code : x.Product.Code,
                        productClassification = x.OrderProduct != null ? x.OrderProduct.Product.CommonCode.Name : x.Product.CommonCode.Name,
                        productName = x.OrderProduct != null ? x.OrderProduct.Product.Name : x.Product.Name,
                        productStandard = x.OrderProduct != null ? x.OrderProduct.Product.Standard : x.Product.Standard,
                        productUnit = x.OrderProduct != null ? x.OrderProduct.Product.Unit : x.Product.Unit,
                        productTaxInfo = x.Product.TaxType,
                        productStandardUnit = x.OrderProduct != null ? x.OrderProduct.ProductStandardUnit : "",
                        productStandardUnitCount = x.OrderProduct != null ? x.OrderProduct.ProductStandardUnitCount : 0,
                        productShipmentProductCount = x.ProductShipmentCount,
                        productSellPrice = x.ProductSellPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        productTotalPrice = x.ProductTotalPrice,
                        productLOT = String.Join(",", x.OutOrderProductStock.Where(y => y.IsDeleted == 0).Select(y => y.Lot.LotName).ToList()),

                    }).ToListAsync();

                var filtered = _outOrderProduct.Where(x => x.productLOT.Contains(outOrderRequest.productLOT));

                var Res = new Response<IEnumerable<OutOrderRes010>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filtered
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderRes010>>()
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


        public async Task<Response<IEnumerable<OutOrderRes011>>> outOrderProductListLOT(OutOrderReq001 outOrderReq001)
        {
            try
            {
                var _outorderproduct = await _db.OutOrderProducts
                    .Include(x => x.Product)
                    .Where(x => x.OutOrderProductId == outOrderReq001.outOrderProductId).FirstOrDefaultAsync();
                var result = await _db.LotCounts
                    
                    .Where(x => x.IsDeleted == 0)
                    .Where(x=>x.Product.Id == _outorderproduct.Product.Id)
                    .Select(x => new OutOrderRes011
                    {
                        lotId = x.Lot.LotId,
                        lotCountId = x.LotCountId,
                        productLOT = x.Lot.LotName,
                        productCode = x.Product.Code,
                        outOrderProductId = _outorderproduct.OutOrderProductId,
                        productClassification = x.Product.CommonCode.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,
                        inventory = ( x.StoreOutCount  + x.ProduceCount) - (x.OutOrderCount + x.ConsumeCount + x.DefectiveCount) + x.ModifyCount,
                        isSelected = 0,
                        quantity = 0,
                        productShipmentCheck = x.Product.Item.ExportCheck

                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<OutOrderRes011>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutOrderRes011>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
    }
}
