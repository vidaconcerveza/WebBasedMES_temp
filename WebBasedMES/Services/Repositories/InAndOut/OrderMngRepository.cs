using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public class OrderMngRepository : IOrderMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public OrderMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            
        }

        public async Task<Response<IEnumerable<OrderPopupResponse>>> GetOrdersPopupBySearch(OrderPopupRequest _req)
        {
            try
            {
                var res = await _db.OrderProducts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Order.OrderNo.Contains(_req.SearchInput) || x.Order.Partner.Name.Contains(_req.SearchInput) || x.Product.Name.Contains(_req.SearchInput))
                    .Select(x => new OrderPopupResponse
                    {
                        OrderId = x.Order.OrderId,
                        OrderProductId = x.OrderProductId,
                        OrderDate = x.Order.OrderDate.ToString("yyyy-MM-dd"),
                        ShipmentStatus = x.OrderStatus,   //
                        OrderNo = x.Order.OrderNo,
                        RequestDeliveryDate = x.Order.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                        PartnerName = x.Order.Partner.Name,

                        ProductId = x.Product.Id,
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,
                        ProductStandardUnit = x.ProductStandardUnit,
                        ProductStandardUnitCount = x.ProductStandardUnitCount,

                        OptimumStock = x.Product.OptimumStock,
                        Inventory = _db.LotCounts.Where(y=>y.Product.Id == x.Product.Id).Select(y=> (0 - y.DefectiveCount - y.ConsumeCount - y.OutOrderCount + y.StoreOutCount + y.ProduceCount + y.ModifyCount)).Sum(),

                        ProductOrderCount = x.ProductOrderCount,
                        ProductOrderCountRemain = _db.OutOrderProducts.Where(y => y.OrderProduct.OrderProductId == x.OrderProductId).Select(y => y.ProductShipmentCount).FirstOrDefault(),
                        OrderProductMemo = x.OrderProductMemo,
                        
                        ProductProcesses = x.Product.Processes
                            .Where(x=> x.IsDeleted == 0)
                            .Select( y=> new ProductProcessInterface2
                            {
                                ProductProcessId = y.ProductProcessId,
                                ProcessId = y.ProcessId,
                                ProcessCode = y.Process.Code,
                                ProcessName = y.Process.Name,
                                ProductId = y.ProduceProductId,
                                ProductUnit = _db.Products.Where(z=>z.Id == y.ProduceProductId).Select(z=>z.Unit).FirstOrDefault(),
                                ProductClassification = _db.Products.Where(z=>z.Id == y.ProduceProductId).Select(z=>z.CommonCode.Name).FirstOrDefault(),
                                ProductName  = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Name).FirstOrDefault(),
                                ProductStandard = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Standard).FirstOrDefault(),
                                
                                Inventory = _db.LotCounts.Where(z => z.Product.Id == y.Product.Id).Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                                ProcessPlanQuantity = 0,
                                /*
                                ProcessInputItems = y.Items
                                    .Where(z=> z.IsDeleted == 0)
                                    .Select(z => new ProductItemInterface2
                                    {
                                        ProcessId = z.ProcessId,
                                        ProcessCode = _db.Processes.Where(k=>k.Id == z.ProcessId).Select(k=>k.Code).FirstOrDefault(),
                                        ProcessName = _db.Processes.Where(k=>k.Id == z.ProcessId).Select(k=>k.Name).FirstOrDefault(),
                                        
                                        ItemId = z.ProductId,
                                        ItemCode = z.Product.Code,  
                                        ItemName = z.Product.Name,
                                        ItemClassification = z.Product.CommonCode.Name,
                                        ItemStandard = z.Product.Standard,
                                        ItemUnit = z.Product.Unit,
                                        Loss = z.Loss, 
                                        ProcessPlanQuantity = 0,
                                        RequiredQuantity = z.Require,
                                        TotalRequiredQuantity = z.Loss * z.Require,
                                        TotalInputQuantity = 0, 
                                        Inventory = _db.LotCounts.Where(k => k.Product.Id == z.Product.Id).Select(k => (0 - k.DefectiveCount - k.ConsumeCount - k.OutOrderCount + k.StoreOutCount + k.ProduceCount + k.ModifyCount)).Sum(),
                                    }).ToList()
                                */
                            }).ToList(),
                    })
                    .ToListAsync();

                var res2 = res.Where(x => _req.ShipmentStatus == "ALL" ? true : x.ShipmentStatus == _req.ShipmentStatus);

                var Res = new Response<IEnumerable<OrderPopupResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<IEnumerable<OrderPopupResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<OrderRes002>> orderMstPop(OrderReq001 OrderReq01)
        {
            try
            {

                var _order = await _db.Orders
                    //.Include(x => x.pro).ThenInclude(OrderItems => OrderItems.Item).ThenInclude(item => item.CommonCode)
                    .Include(x => x.OrderProducts).ThenInclude(x => x.Product)
                    //.Include(x => x.CommonCode)
                    .Include(x => x.Partner)
                    .Include(x => x.UploadFiles)

                    .Where(x => x.OrderId == OrderReq01.orderId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new OrderRes002
                    {

                        orderId = x.OrderId,
                        orderNo = x.OrderNo,
                        orderDate = x.OrderDate.ToString("yyyy-MM-dd"),
                        requestDeliveryDate = x.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                        orderStatus = x.OrderProducts.Where(y=>y.IsDeleted == 0).Count() == x.OrderProducts.Where(y=>y.IsDeleted == 0).Where(y=>y.OrderStatus == "수주등록").Count() ? "수주등록" : x.OrderProducts.Where(y => y.IsDeleted == 0).Count() == x.OrderProducts.Where(y => y.IsDeleted == 0).Where(y => y.OrderStatus == "출하완료").Count()?"출하완료" : "출하대기",


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
                        orderMemo = x.OrderMemo,

                        UploadFiles = x.UploadFiles.ToArray(),

                    }).FirstOrDefaultAsync();

                var Res = new Response<OrderRes002>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _order
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<OrderRes002>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OrderRes001>>> orderMstList(OrderReq001 orderRequest)
        {
            try
            {
                var _order = await _db.Orders
                    .Include(x => x.OrderProducts).ThenInclude(x => x.Product).ThenInclude(x=>x.Item).ThenInclude(item => item.CommonCode)
                    .Where(x => orderRequest.orderNo == "" ? true : x.OrderNo.Contains(orderRequest.orderNo))
                    .Where(x => orderRequest.orderStartDate == "" ? true : x.OrderDate >= Convert.ToDateTime(orderRequest.orderStartDate))
                    .Where(x => orderRequest.orderEndDate == "" ? true : x.OrderDate <= Convert.ToDateTime(orderRequest.orderEndDate))
                    .Where(x => orderRequest.partnerId == 0 ? true : x.Partner.Id == orderRequest.partnerId)
                    .Where(x => orderRequest.productId == 0 ? true :
                        x.OrderProducts.Where(x => x.Product.Id == orderRequest.productId)
                        .Select(x => x.Product.Id).ToArray()
                        .Contains(orderRequest.productId)
                    )
                    .Where(x => x.IsDeleted == 0)
                    .OrderByDescending(x => x.OrderNo)
                    .Select(x => new OrderRes001
                    {

                        orderId = x.OrderId,
                        orderNo = x.OrderNo, //수주번호
                        orderDate = x.OrderDate.ToString("yyyy-MM-dd"), //수주일
                        requestDeliveryDate = x.RequestDeliveryDate.ToString("yyyy-MM-dd"), //남품요청일
                        partnerName = x.Partner.Name, // 거래처이름
                        partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                        orderProductCount = x.OrderProducts.Where(y=>y.IsDeleted == 0).Select(x => x).Count(),
                        orderSellPrice = x.OrderProducts.Where(y => y.IsDeleted == 0).Select(x => x).Sum(x => x.ProductSellPrice * x.ProductOrderCount),

                        orderTaxPrice = x.OrderProducts.Where(y=>y.IsDeleted == 0).Select(x => x).Sum(x => x.ProductTaxPrice),
                        orderTotalPrice = x.OrderProducts.Where(y => y.IsDeleted == 0).Select(x => x).Sum(x => x.ProductTotalPrice),

                        orderStatus = x.OrderProducts.Where(y=>y.IsDeleted ==0).Count() == x.OrderProducts.Where(y => y.IsDeleted == 0).Where(y=>y.OrderStatus== "수주등록").Select(y => y.OrderStatus).Count() ? "수주등록" : 
                            x.OrderProducts.Where(y => y.IsDeleted == 0).Count() == x.OrderProducts.Where(y => y.IsDeleted == 0).Where(y => y.OrderStatus == "출하완료").Select(y => y.OrderStatus).Count() ? "출하완료" : "출하대기",
                        registerName = x.Register.FullName,//등록자
                        orderMemo = x.OrderMemo,//비고
                        uploadFileUrl = x.UploadFiles.Count()>0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count()>0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                    }).ToListAsync();

                var res = _order.Where(x => x.orderStatus.Contains(orderRequest.orderStatus));

                var Res = new Response<IEnumerable<OrderRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OrderRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<OrderRes003>>> orderProductList(OrderReq001 OrderReq001)
        {
            try
            {

                var result = await _db.OrderProducts
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                    .Where(x => x.Order.OrderId == OrderReq001.orderId)
                    .Where(x=>x.IsDeleted == 0)
                    .OrderBy(x=>x.Product.Code)
                    .Select(x => new OrderRes003
                    {

                        orderProductId = x.OrderProductId,
                        orderId = x.Order.OrderId,
                        productId = x.Product.Id,
                        shipmentStatus = x.OrderStatus,

                       // shipmentStatus = x.OrderStatus,
                        productCode = x.Product.Code,
                        productClassification = x.Product.CommonCode != null ? x.Product.CommonCode.Name : "",
                        //productClassification = x.Product.CommonCode != null ? x.Product.CommonCode.Code : "",
                        productName = x.Product.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,

                        productStandardUnit = x.ProductStandardUnit,
                        //productStandardUnit = _db.CommonCodes.Where(common => (common.Code == x.ProductStandardUnit)).FirstOrDefault().Code,

                        productTaxInfo = x.ProductTaxInfo,
                        productOrderCount = x.ProductOrderCount,
                        productOrderRemain = x.ProductOrderCount - _db.OutOrderProducts.Where(y=>y.IsDeleted == 0).Where(y=>y.OrderProduct.OrderProductId == x.OrderProductId).Select(y=>y.ProductShipmentCount).Count(),
                        productSellPrice = x.ProductSellPrice,
                        //productSupplyPrice = x.OrderCount * x.SellPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        //productTaxPrice = ((x.OrderCount * x.SellPrice) * 0.1),
                        //productTotalPrice = (x.OrderCount * x.SellPrice) + ((x.OrderCount * x.SellPrice) * 0.1),
                        productTotalPrice = x.ProductTotalPrice,
                        orderProductMemo = x.OrderProductMemo,
                        productShipmentCheck = x.Product.ExportCheck,

                        ModelName = x.Product.Model != null ? x.Product.Model.Name : "",
                        PartnerName = x.Product.Partner != null ? x.Product.Partner.Name : "",
                    }).ToListAsync();




                var Res = new Response<IEnumerable<OrderRes003>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OrderRes003>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OrderRes004>>> orderProductListPop(OrderReq001 OrderReq001)
        {
            try
            {

                var result = await _db.OrderProducts
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                    .Where(x => x.Order.OrderId == OrderReq001.orderId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new OrderRes004
                    {

                        orderProductId = x.OrderProductId,
                        orderId = x.Order.OrderId,
                        productId = x.Product.Id,

                        uploadFileId = x.Product.UploadFile != null ? x.Product.UploadFile.Id : 0,
                        uploadFileName = x.Product.UploadFile != null ? x.Product.UploadFile.FileName : "",
                        uploadFileUrl = x.Product.UploadFile != null ? x.Product.UploadFile.FileUrl : "",


                        productCode = x.Product.Code,
                        productClassification = x.Product.CommonCode != null ? x.Product.CommonCode.Name : "",
                        //productClassification = x.Product.CommonCode != null ? x.Product.CommonCode.Code : "",
                        productName = x.Product.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,

                        //productStandardUnit = x.Unit.Name,
                        productStandardUnit = x.ProductStandardUnit,
                        //productStandardUnit = _db.CommonCodes.Where(common => (common.Code == x.ProductStandardUnit)).FirstOrDefault().Code,
                        productStandardUnitCount = x.ProductStandardUnitCount,


                        productTaxInfo = x.ProductTaxInfo,
                        productOrderCount = x.ProductOrderCount,
                        productSellPrice = x.ProductSellPrice,
                        //productSupplyPrice = x.OrderCount * x.SellPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        //productTaxPrice = ((x.OrderCount * x.SellPrice) * 0.1),
                        productTaxPrice = x.ProductTaxPrice,
                        //productTotalPrice = (x.OrderCount * x.SellPrice) + ((x.OrderCount * x.SellPrice) * 0.1),
                        productTotalPrice = x.ProductTotalPrice,
                        orderProductMemo = x.OrderProductMemo,

                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<OrderRes004>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OrderRes004>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OrderRes005>>> productList(OrderReq002 OrderReq002)
        {
            try
            {
                if(OrderReq002.invenType == "PRD")
                {
                     var resPrd = await _db.Products
                        .Where(x => x.IsDeleted == false)
                        .Where(x => x.Processes.Where(y=>y.IsDeleted == 0).Count()>0)
                        .Where(x => OrderReq002.productClassification == "ALL" ? true : x.CommonCode.Name.Contains(OrderReq002.productClassification))
                        .Where(x => OrderReq002.productIsUsingStr == "ALL" ? true : OrderReq002.productIsUsingStr == "Y" ? x.IsUsing == true : false)
                        .Where(x => x.Name.Contains(OrderReq002.productCode) || x.Code.Contains(OrderReq002.productCode))
                        .Select(x => new OrderRes005
                        {
                            productBuyPrice = x.BuyPrice,
                            productSellPrice = x.SellPrice,

                            productClassification = x.CommonCode.Name,
                            productIsUsing = x.IsUsing,
                            productCode = x.Code,
                            productName = x.Name,
                            productId = x.Id,
                            productImportCheck = x.ImportCheck,
                            productMemo = x.Memo,
                            productShipmentCheck = x.ExportCheck,
                            productStandard = x.Standard,
                            productTaxInfo = x.TaxType,
                            productUnit = x.Unit,
                            uploadFileUrl = x.UploadFile.FileUrl,
                            uploadFileId = x.UploadFile.Id,
                            uploadFileName = x.UploadFile.FileName,

                            ModelId = x.Model != null ? x.Model.Id : 0,
                            ModelName = x.Model != null ? x.Model.Name : "",
                            ModelCode = x.Model != null ? x.Model.Code : "",
                            PartnerId = x.Partner != null ? x.Partner.Id : 0,
                            PartnerName = x.Partner != null ? x.Partner.Name : "",
                            PartnerCode = x.Partner != null ? x.Partner.Code : "",

                            /*
                            outOrderProductDefectives = _db.OutOrderProductsDefectives
                                .Where(y=>y.OutOrderProductDefectiveId == 0)
                                .Select(y=>new OutOrderProductDefectiveInterface
                                {

                                }).ToList(),*/
                        }).ToListAsync();

                    var ResPrd = new Response<IEnumerable<OrderRes005>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = resPrd
                    };

                    return ResPrd;
                }

                if(OrderReq002.invenType == "ITEM")
                {
                    var resPrd = await _db.Products
                       .Where(x => x.IsDeleted == false)
                       .Where(x => x.Processes.Where(y => y.IsDeleted == 0).Count() == 0)
                       .Where(x => OrderReq002.productClassification == "ALL" ? true : x.CommonCode.Name.Contains(OrderReq002.productClassification))
                       .Where(x => OrderReq002.productIsUsingStr == "ALL" ? true : OrderReq002.productIsUsingStr == "Y" ? x.IsUsing == true : false)
                       .Where(x => x.Name.Contains(OrderReq002.productCode) || x.Code.Contains(OrderReq002.productCode))
                       .Select(x => new OrderRes005
                       {
                           productBuyPrice = x.BuyPrice,
                           productSellPrice = x.SellPrice,

                           productClassification = x.CommonCode.Name,
                           productIsUsing = x.IsUsing,
                           productCode = x.Code,
                           productName = x.Name,
                           productId = x.Id,
                           productImportCheck = x.ImportCheck,
                           productMemo = x.Memo,
                           productShipmentCheck = x.ExportCheck,
                           productStandard = x.Standard,
                           productTaxInfo = x.TaxType,
                           productUnit = x.Unit,
                           uploadFileUrl = x.UploadFile.FileUrl,
                           uploadFileId = x.UploadFile.Id,
                           uploadFileName = x.UploadFile.FileName,

                           ModelId = x.Model != null ? x.Model.Id : 0,
                           ModelName = x.Model != null ? x.Model.Name : "",
                           ModelCode = x.Model != null ? x.Model.Code : "",
                           PartnerId = x.Partner != null ? x.Partner.Id : 0,
                           PartnerName = x.Partner != null ? x.Partner.Name : "",
                           PartnerCode = x.Partner != null ? x.Partner.Code : "",


                           /*
                                                      outOrderProductDefectives = _db.OutOrderProductsDefectives
                                                          .Where(y => y.OutOrderProductDefectiveId == 0)
                                                          .Select(y => new OutOrderProductDefectiveInterface
                                                          {

                                                          }).ToList(),*/
                       }).ToListAsync();


                    var ResPrd = new Response<IEnumerable<OrderRes005>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = resPrd
                    };

                    return ResPrd;

                }

                if(OrderReq002.invenType == "DEFAULT")
                {
                    var resDefault = await _db.Products
                    .Where(x => x.IsDeleted == false)
                    .Where(x => OrderReq002.productClassification == "ALL" ? true : x.CommonCode.Name.Contains(OrderReq002.productClassification))
                    .Where(x => OrderReq002.productIsUsingStr == "ALL" ? true : OrderReq002.productIsUsingStr == "Y" ? x.IsUsing == true : false)
                    .Where(x => x.Name.Contains(OrderReq002.productCode) || x.Code.Contains(OrderReq002.productCode))
                    .Select(x => new OrderRes005
                    {
                        productBuyPrice = x.BuyPrice,
                        productSellPrice = x.SellPrice,
                        productClassification = x.CommonCode.Name,
                        productIsUsing = x.IsUsing,
                        productCode = x.Code,
                        productName = x.Name,
                        productId = x.Id,
                        productImportCheck = x.ImportCheck,
                        productMemo = x.Memo,
                        productShipmentCheck = x.ExportCheck,
                        productStandard = x.Standard,
                        productTaxInfo = x.TaxType,
                        productUnit = x.Unit,
                        uploadFileUrl = x.UploadFile.FileUrl,
                        uploadFileId = x.UploadFile.Id,
                        uploadFileName = x.UploadFile.FileName,

                        ModelId = x.Model != null ? x.Model.Id : 0,
                        ModelName = x.Model != null ? x.Model.Name : "",
                        ModelCode = x.Model != null ? x.Model.Code : "",
                        PartnerId = x.Partner != null ? x.Partner.Id : 0,
                        PartnerName = x.Partner != null ? x.Partner.Name : "",
                        PartnerCode = x.Partner != null ? x.Partner.Code : "",
                    }).ToListAsync();


                    var ResD = new Response<IEnumerable<OrderRes005>>()
                    {
                     IsSuccess = true,
                        ErrorMessage = "",
                        Data = resDefault
                    };

                    return ResD;
                }

                var res = await _db.Products
                    .Where(x => x.IsDeleted == false)
                    .Where(x => OrderReq002.productClassification == "ALL" ? true : x.CommonCode.Name.Contains(OrderReq002.productClassification))
                    .Where(x => OrderReq002.productIsUsingStr == "ALL" ? true : OrderReq002.productIsUsingStr == "Y" ? x.IsUsing == true : false)
                    .Where(x => x.Name.Contains(OrderReq002.productCode) || x.Code.Contains(OrderReq002.productCode))
                    .Select(x => new OrderRes005
                    {
                        productBuyPrice = x.BuyPrice,
                        productSellPrice = x.SellPrice,

                        productClassification = x.CommonCode.Name,
                        productIsUsing = x.IsUsing,
                        productCode = x.Code,
                        productName = x.Name,
                        productId = x.Id,
                        productImportCheck = x.ImportCheck,
                        productMemo = x.Memo,
                        productShipmentCheck = x.ExportCheck,
                        productStandard = x.Standard,
                        productTaxInfo = x.TaxType,
                        productUnit = x.Unit,
                        uploadFileUrl = x.UploadFile.FileUrl,
                        uploadFileId = x.UploadFile.Id,
                        uploadFileName = x.UploadFile.FileName,

                        ModelId = x.Model != null ? x.Model.Id : 0,
                        ModelName = x.Model != null ? x.Model.Name : "",
                        ModelCode = x.Model != null ? x.Model.Code : "",
                        PartnerId = x.Partner != null ? x.Partner.Id : 0,
                        PartnerName = x.Partner != null ? x.Partner.Name : "",
                        PartnerCode = x.Partner != null ? x.Partner.Code : "",
                        /*
                        outOrderProductDefectives = _db.OutOrderProductsDefectives
                            .Where(y=>y.OutOrderProductDefectiveId == 0)
                            .Select(y=>new OutOrderProductDefectiveInterface
                            {

                            }).ToList(),*/
                    }).ToListAsync();


                var Res = new Response<IEnumerable<OrderRes005>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OrderRes005>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OrderRes006>>> userList(OrderReq003 OrderReq003)
        {
            try
            {

                var result = await _db.Users
                    .Where(x => (OrderReq003.userIsApprovedStr == "ALL" || OrderReq003.userIsApprovedStr == "") ? true : (OrderReq003.userIsApprovedStr == "Y" ? x.IsApproved == true : (OrderReq003.userIsApprovedStr == "N" ? x.IsApproved == false : false)))
                    .Where(x => x.IsDeleted == false)
                    .Where(x => OrderReq003.searchInput == "" ? true :

                     x.IdNumber.Contains(OrderReq003.searchInput) ||
                     x.FullName.Contains(OrderReq003.searchInput) ||
                     x.UserRole.Name.Contains(OrderReq003.searchInput) ||
                     x.Department.Name.Contains(OrderReq003.searchInput) ||
                     x.Position.Name.Contains(OrderReq003.searchInput) ||
                     x.Email.Contains(OrderReq003.searchInput) ||
                     x.PhoneNumber.Contains(OrderReq003.searchInput)


                    )
                    .Select(x => new OrderRes006
                    {
                        userId = x.Id,
                        userNo = x.IdNumber,//이름
                        userRole = x.UserRole.Name,//권한
                        userFullName = x.FullName,

                        userDepartment = x.Department.Name,//부서
                        userPosition = x.Position.Name,//직급
                        userEmail = x.Email,//이메일
                        userPhoneNumber = x.PhoneNumber, //연락처
                        userIsApproved = x.IsApproved,//승인여부

                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<OrderRes006>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OrderRes006>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<OrderRes007>>> partnerList(OrderReq004 OrderReq004)
        {
            try
            {
                var _partner = await _db.Partners
                    .Where(x => x.IsDeleted == false)
                    .Where(x => (OrderReq004.partnerStatusStr == "ALL" || OrderReq004.partnerStatusStr == null) ? true : (OrderReq004.partnerStatusStr == "Y" ? x.IsUsing == true : x.IsUsing == false))
                    .Where(x => (OrderReq004.searchInput == "매입" || OrderReq004.searchInput == "매출" || OrderReq004.searchInput == "금융" || OrderReq004.searchInput == "기타") ? true :

                     (x.CommonCode.Name.Contains(OrderReq004.searchInput) ||
                     x.Code.Contains(OrderReq004.searchInput) ||
                     x.Name.Contains(OrderReq004.searchInput) ||
                     x.BusinessNumber.Contains(OrderReq004.searchInput) ||
                     x.President.Contains(OrderReq004.searchInput) ||
                     x.TelephoneNumber.Contains(OrderReq004.searchInput) ||
                     x.ContactName.Contains(OrderReq004.searchInput) ||
                     x.FaxNumber.Contains(OrderReq004.searchInput))
                    )
                    .Where(x => OrderReq004.searchInput == "매입" ? x.Group_Buy == true : true)
                    .Where(x => OrderReq004.searchInput == "매출" ? x.Group_Sell == true : true)
                    .Where(x => OrderReq004.searchInput == "금융" ? x.Group_Finance == true : true)
                    .Where(x => OrderReq004.searchInput == "기타" ? x.Group_Etc == true : true)
                    .Select(x => new OrderRes007
                    {
                        partnerId = x.Id,
                        partnerClassification = x.CommonCode.Name,
                        partnerCode = x.Code,
                        partnerName = x.Name,
                        presidentName = x.President,
                        businessNumber = x.BusinessNumber,
                        telephoneNumber = x.TelephoneNumber,
                        faxNumber = x.FaxNumber,
                        contactName = x.ContactName,
                        contactEmail = x.ContactEmail,
                        partnerTaxInfo = x.TaxInfo,
                        
                        partnerMemo = x.Memo,
                        partnerStatus = x.IsUsing,

                        Group_Buy = x.Group_Buy,
                        Group_Sell = x.Group_Sell,
                        Group_Finance = x.Group_Finance,
                        Group_Etc = x.Group_Etc,
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<OrderRes007>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _partner
                };
                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<OrderRes007>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<int>> CreateOrder(OrderRequestCrud orderRequest)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(orderRequest.registerId);
                var _partner = await _db.Partners.Where(y => y.Id == orderRequest.partnerId).FirstOrDefaultAsync();
                var _commonCode = await _db.CommonCodes.Where(y => y.Id == orderRequest.orderStatusId).FirstOrDefaultAsync();
                DateTime OrderDate = DateTime.ParseExact(orderRequest.orderDate, "yyyy-MM-dd", null);
                DateTime RequestDeliveryDate = DateTime.ParseExact(orderRequest.requestDeliveryDate, "yyyy-MM-dd", null);


                var _prevOrder = await _db.Orders.Where(x=>x.OrderDate == OrderDate).OrderByDescending(x => x.OrderNo).FirstOrDefaultAsync();

                string formatNo = "ON" + OrderDate.ToString("yyyyMMdd");
                string _storeNumberFormat = "";

               // var _uploadFile = await _db.UploadFiles.Where(y => y.Id == orderRequest.uploadFileId).FirstOrDefaultAsync();
                if (_prevOrder == null)
                {
                    _storeNumberFormat = string.Format(formatNo + "{0:0000#}", 1);
                    _prevOrder = new Order();
                }
                else
                {
                   _storeNumberFormat = string.Format(formatNo + "{0:0000#}", (Convert.ToInt32(_prevOrder.OrderNo.Substring(11, 4)) + 1));
                }



                var _order = new Order()
                {
                    
                    OrderNo = _storeNumberFormat, //수주번호
                    OrderDate = OrderDate, //수주일
                    RequestDeliveryDate = RequestDeliveryDate, //납품요청일
                    Partner = _partner, //거래처
                    OrderMemo = orderRequest.orderMemo, //비고
                    //CommonCode = _commonCode, //수주상태
                    //UploadFile = _uploadFile, //업로드파일

                    UploadFiles = orderRequest.UploadFiles,

                    OrderFinish = "수주등록",
                    //IsUsing = orderRequest.orderIsUsing,//사용중
                    //IsDeleted = false,//삭제여부
                    //order = DateTime.Now, //생성일
                    IsDeleted = orderRequest.isDeleted,
                    Register = _user, //등록자
                    

                };

                var result = await _db.Orders.AddAsync(_order);
                await Save();

                var Res = new Response<int>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _order.OrderId,
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

        public async Task<Response<bool>> UpdateOrder(OrderRequestCrud orderRequest)
        {
            try
            {


                    var _user = await _userManager.FindByIdAsync(orderRequest.registerId);
                    var _partner = await _db.Partners.Where(y => y.Id == orderRequest.partnerId).FirstOrDefaultAsync();
                    var _uploadFile = await _db.UploadFiles.Where(y => y.Id == orderRequest.uploadFileId).FirstOrDefaultAsync();
                    DateTime OrderDate = DateTime.ParseExact(orderRequest.orderDate, "yyyy-MM-dd", null);
                    DateTime RequestDeliveryDate = DateTime.ParseExact(orderRequest.requestDeliveryDate, "yyyy-MM-dd", null);


                var _order = await _db.Orders.Include(x=>x.UploadFiles).Where(x => x.OrderId == orderRequest.orderId).FirstOrDefaultAsync();



                _order.OrderDate = Convert.ToDateTime(orderRequest.orderDate);
                _order.RequestDeliveryDate = Convert.ToDateTime(orderRequest.requestDeliveryDate);
                _order.Partner = _partner;
                _order.OrderMemo = orderRequest.orderMemo;
                //     _order.UploadFile = _uploadFile;

                if(_order.UploadFiles != null)
                {
                    _db.UploadFiles.RemoveRange(_order.UploadFiles);
                }

                _order.UploadFiles = orderRequest.UploadFiles;
                _order.Register = _user;

                    _db.Orders.Update(_order);
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

        public async Task<Response<bool>> DeleteOrder(OrderRequestCrud orderRequest)
        {
            try
            {
                foreach (int item in orderRequest.orderIdArray)
                {
                    var subItem = await _db.OrderProducts.Where(x => x.Order.OrderId == item).ToListAsync();

                    foreach (OrderProduct sub in subItem)
                    {
                        sub.IsDeleted = 1;
                    }
                    _db.OrderProducts.UpdateRange(subItem);
                    var _outStores = await _db.Orders.FindAsync(item);
                    var mst = await _db.Orders.FindAsync(item);
                    mst.IsDeleted = 1;
                    _db.Orders.Update(mst);
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

        public async Task<Response<bool>> CreateOrderProduct(OrderProductRequestCrud orderProduct)
        {
            try
            {

                var _order = await _db.Orders.Where(x => x.OrderId == orderProduct.orderId).FirstOrDefaultAsync();
                //var _unit = await _db.CommonCodes.Where(x => x.Id == orderProduct.productStandardUnitId).FirstOrDefaultAsync();
                
                
                foreach(var prd in orderProduct.orderProductArray)
                {
                    var _Item = await _db.Products.Where(x => x.Id == prd.productId).FirstOrDefaultAsync();
                    var _orderProduct = new OrderProduct()
                    {
                        Order = _order,
                        Product = _Item,
                        ProductStandardUnit = prd.productStandardUnit,
                        ProductStandardUnitCount = prd.productStandardUnitCount,
                        ProductTaxInfo = prd.productTaxInfo,
                        ProductOrderCount = prd.productOrderCount,
                        ProductSellPrice = prd.productSellPrice,
                        ProductSupplyPrice = prd.productSupplyPrice,
                        ProductTaxPrice = prd.productTaxPrice,
                        ProductTotalPrice = prd.productTotalPrice,
                        OrderProductMemo = prd.orderProductMemo,
                        IsDeleted = 0,
                        OrderStatus = "수주등록",
                    };
                    await _db.OrderProducts.AddAsync(_orderProduct);
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

        public async Task<Response<bool>> DeleteOrderProduct(OrderProductRequestCrud OrderProductRequest)
        {
            try
            {

                foreach (int item in OrderProductRequest.orderProductIdArray)
                {
                    var _items = await _db.OrderProducts.FindAsync(item);
                    _items.IsDeleted = 1;
                    _db.OrderProducts.Update(_items);
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

        public async Task<Response<bool>> UpdateOrderProduct(OrderProductRequestCrud orderProduct)
        {
            try
            {

                var _order = await _db.Orders.Where(x => x.OrderId == orderProduct.orderId).FirstOrDefaultAsync();
               // var _Item = await _db.Products.Where(x => x.Id == orderProduct.productId).FirstOrDefaultAsync();


                var _beforeOrderPrds = await _db.OrderProducts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Order.OrderId == orderProduct.orderId)
                    .Select(x => x.OrderProductId)
                    .ToListAsync();


                //1. orderProductId == 0 ? 추가
                //2. orderProductId 없으면 삭제
                //3. orderProductId == 동일 UPDATE;



                foreach(var i in orderProduct.orderProductArray)
                {
                    if(i.orderProductId == 0)
                    {
                        var _Item = await _db.Products.Where(x => x.Id == i.productId).FirstOrDefaultAsync();
                        var _orderProduct = new OrderProduct()
                        {
                            Order = _order,
                            Product = _Item,
                            ProductStandardUnit = i.productStandardUnit,
                            ProductStandardUnitCount = i.productStandardUnitCount,
                            ProductTaxInfo = i.productTaxInfo,
                            ProductOrderCount = i.productOrderCount,
                            ProductSellPrice = i.productSellPrice,
                            ProductSupplyPrice = i.productSupplyPrice,
                            ProductTaxPrice = i.productTaxPrice,
                            ProductTotalPrice = i.productTotalPrice,
                            OrderProductMemo = i.orderProductMemo,
                            IsDeleted = 0,
                        };
                        await _db.OrderProducts.AddAsync(_orderProduct);
                    }
                }

                await Save();





                bool flag = false;
                foreach(var i in _beforeOrderPrds)
                {
                    flag = false;
                    foreach(var j in orderProduct.orderProductArray)
                    {
                        if(j.orderProductId == i)
                        {
                            //Update
                            flag = true;

                            var _Item = await _db.Products.Where(x => x.Id == j.productId).FirstOrDefaultAsync();
                            var _updateOrderPrd = await _db.OrderProducts.Where(x => x.OrderProductId == j.orderProductId).FirstOrDefaultAsync();
                            
                            _updateOrderPrd.Order = _order; 
                            _updateOrderPrd.Product = _Item;
                            _updateOrderPrd.ProductStandardUnit = j.productStandardUnit;
                            _updateOrderPrd.ProductStandardUnitCount = j.productStandardUnitCount;
                            _updateOrderPrd.ProductTaxInfo = j.productTaxInfo;
                            _updateOrderPrd.ProductOrderCount = j.productOrderCount;
                            _updateOrderPrd.ProductSellPrice = j.productSellPrice;
                            _updateOrderPrd.ProductSupplyPrice = j.productSupplyPrice;
                            _updateOrderPrd.ProductTaxPrice = j.productTaxPrice;
                            _updateOrderPrd.ProductTotalPrice = j.productTotalPrice;
                            _updateOrderPrd.OrderProductMemo = j.orderProductMemo;
                            _updateOrderPrd.IsDeleted = 0;

                            _db.OrderProducts.Update(_updateOrderPrd);
                        }
                    }

                    if(flag == false)
                    {
                        var _deleteOrderPrd = await _db.OrderProducts.Where(x => x.OrderProductId == i).FirstOrDefaultAsync();
                        _deleteOrderPrd.IsDeleted = 1;


                        _db.OrderProducts.Update(_deleteOrderPrd);
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

        public async Task<Response<bool>> OutOrderFinishEvent(OrderProductRequestCrud req)
        {
            try
            {
                var res = await _db.OrderProducts.Where(x => x.Order.OrderId == req.orderId).ToListAsync();
                foreach(var i in res)
                {
                    i.OrderStatus = "출하완료";
                    _db.OrderProducts.Update(i);
                };

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
                    ErrorMessage = ex.Message.ToString(),
                    Data = true
                };

                return Res;
            }
        }



        public async Task<Response<bool>> OrderProductEditSave(OrderProductRequestCrud OrderProductRequestCrud)
        {
            try
            {
                int create_orderId = 0;
                Order targetMst = null;
                if (OrderProductRequestCrud.order.orderId != 0)
                {
                    var _user = await _userManager.FindByIdAsync(OrderProductRequestCrud.order.registerId);
                    var _partner = await _db.Partners.Where(y => y.Id == OrderProductRequestCrud.order.partnerId).FirstOrDefaultAsync();
                    var _commonCode = await _db.CommonCodes.Where(y => y.Id == OrderProductRequestCrud.order.orderStatusId).FirstOrDefaultAsync();
                    DateTime OrderDate = DateTime.ParseExact(OrderProductRequestCrud.order.orderDate, "yyyy-MM-dd", null);
                    DateTime RequestDeliveryDate = DateTime.ParseExact(OrderProductRequestCrud.order.requestDeliveryDate, "yyyy-MM-dd", null);
                    var _order = await _db.Orders.OrderByDescending(x => x.OrderNo).FirstOrDefaultAsync();
                    var _uploadFile = await _db.UploadFiles.Where(y => y.Id == OrderProductRequestCrud.order.uploadFileId).FirstOrDefaultAsync();
                    if (_order == null)
                    {
                        _order = new Order();
                    }

                    string formatNo = "ON" + DateTime.Now.ToString("yyyyMMdd");
                    var _storeNumberFormat = string.Format(formatNo + "{0:0000#}", (_order.OrderId + 1));

                    _order = new Order()
                    {

                        OrderNo = _storeNumberFormat, //수주번호
                        OrderDate = OrderDate, //수주일
                        RequestDeliveryDate = RequestDeliveryDate, //납품요청일
                        Partner = _partner, //거래처
                        OrderMemo = OrderProductRequestCrud.order.orderMemo, //비고
                                                                             //CommonCode = _commonCode, //수주상태
                   //     UploadFiles = _uploadFile, //업로드파일
                                                  //IsUsing = orderRequest.orderIsUsing,//사용중
                                                  //IsDeleted = false,//삭제여부
                                                  //order = DateTime.Now, //생성일
                        IsDeleted = OrderProductRequestCrud.order.isDeleted,
                        Register = _user //등록자

                    };

                    var result = await _db.Orders.AddAsync(_order);
                }

                else
                {
                    var _user = await _userManager.FindByIdAsync(OrderProductRequestCrud.order.registerId);
                    var _partner = await _db.Partners.Where(y => y.Id == OrderProductRequestCrud.order.partnerId).FirstOrDefaultAsync();
                    var _uploadFile = await _db.UploadFiles.Where(y => y.Id == OrderProductRequestCrud.order.uploadFileId).FirstOrDefaultAsync();

                    DateTime OrderDate = DateTime.ParseExact(OrderProductRequestCrud.order.orderDate, "yyyy-MM-dd", null);
                    DateTime RequestDeliveryDate = DateTime.ParseExact(OrderProductRequestCrud.order.requestDeliveryDate, "yyyy-MM-dd", null);

                    var _order = new Order()
                    {
                        OrderId = OrderProductRequestCrud.order.orderId, //수주ID
                        OrderNo = OrderProductRequestCrud.order.orderNo, //수주번호
                        OrderDate = OrderDate, //수주일
                        RequestDeliveryDate = RequestDeliveryDate, //납품요청일
                        Partner = _partner, //거래처
                        OrderMemo = OrderProductRequestCrud.order.orderMemo, //비고
                   //     UploadFiles = _uploadFile, //업로드파일
                        IsDeleted = OrderProductRequestCrud.order.isDeleted,
                        Register = _user //등록자
                    };

                    _db.Orders.Update(_order);

                }

                    var orderProducts = await _db.OrderProducts.Where(x => x.Order.OrderId == OrderProductRequestCrud.order.orderId).ToListAsync();

                    foreach (OrderProduct item in orderProducts)
                    {
                        item.IsDeleted = 1;
                        _db.OrderProducts.Update(item);
                    }
                

                //string query = "";
                ////query += "select * from OrderProducts ";
                ////query += "where 1 = 1 ";
                ////query += "and OrderId = @orderId ";

                //query+=" UPDATE OrderProducts                           ";
                //query+=" set                                            ";
                //query+= "     OrderProducts.IsDeleted = 1      ";
                //query+=" from Orders, OrderProducts                     ";
                //query+= " where Orders.OrderId = OrderProducts.OrderId   ";
                //query += " and Orders.OrderId = @orderId   ";

                //UPDATE Table명
                //SET
                //필드명1 = 수정할 데이터1, 필드명2 = 수정할 데이터2, 
                //WHERE 조건필드명 = 데이터 조건



                //orderProducts.ForEach(delegate(OrderProduct item){
                //    item.IsDeleted = 1;
                //    _db.OrderProducts.Update(item);
                //});



                    //var count = _db.Database.ExecuteSqlRaw(query, new SqlParameter("@orderId", OrderProductRequestCrud.orderId));
                //var count = _db.OrderProducts.FromSqlRaw(query, new SqlParameter("@orderId", orderProduct.orderId)).Count();
                if(OrderProductRequestCrud.orderProductArray != null)
                {
                    foreach (OrderProductRequestCrud orderProduct in OrderProductRequestCrud.orderProductArray)
                    {

                        //Order _order = targetMst;
                        Order _order = await _db.Orders.Where(x => x.OrderId == orderProduct.orderId).FirstOrDefaultAsync();
                        var _Item = await _db.Products.Where(x => x.Id == orderProduct.productId).FirstOrDefaultAsync();
                        //var _unit = await _db.CommonCodes.Where(x => x.Id == orderProduct.productStandardUnitId).FirstOrDefaultAsync();
                        var _orderProduct = new OrderProduct()
                        {
                            Order = _order,
                            Product = _Item,
                            ProductStandardUnit = orderProduct.productStandardUnit,
                            ProductStandardUnitCount = orderProduct.productStandardUnitCount,
                            ProductTaxInfo = orderProduct.productTaxInfo,
                            ProductOrderCount = orderProduct.productOrderCount,
                            ProductSellPrice = orderProduct.productSellPrice,
                            ProductSupplyPrice = orderProduct.productSupplyPrice,
                            ProductTaxPrice = orderProduct.productTaxPrice,
                            ProductTotalPrice = orderProduct.productTotalPrice,
                            OrderProductMemo = orderProduct.orderProductMemo,
                            IsDeleted = orderProduct.isDeleted,
                        };
                        await _db.OrderProducts.AddAsync(_orderProduct);
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
