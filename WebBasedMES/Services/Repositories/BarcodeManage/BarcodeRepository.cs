using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.Barcode;
using WebBasedMES.Data.Models.Mold;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Barcode;
using WebBasedMES.ViewModels.InspectionRepair;
using WebBasedMES.ViewModels.Mold;

namespace WebBasedMES.Services.Repositories.BarcodeManage
{
    public class BarcodeRepository : IBarcodeRepository
    {
        private readonly ApiDbContext _db;

        public BarcodeRepository(ApiDbContext db)
        {
            _db = db;
        }

        public async Task<Response<IEnumerable<InBarcodeResponse>>> GetInBarcodeIssueList(InBarcodeRequest req)
        {
            try
            {
                    var res = await _db.StoreOutStoreProducts
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Receiving.IsDeleted == 0)
                    .Where(x => x.Receiving.ReceivingDate >= Convert.ToDateTime(req.ReceivingStartDate))
                    .Where(x => x.Receiving.ReceivingDate <= Convert.ToDateTime(req.ReceivingEndDate))
                    .Where(x => x.LotName.Contains(req.ProductLOT))
                    .Where(x => req.PartnerId == 0 ? true : x.Receiving.Partner.Id == req.PartnerId)
                    .Where(x => req.ProductId == 0 ? true : x.OutStoreProduct.Product.Id == req.ProductId)
                    .Where(x => req.ReIssue ? x.Barcode !=null : true)
                    .OrderBy(x=>x.Receiving.ReceivingDate)
                    .Select(x => new InBarcodeResponse
                    {

                        StoreOutStoreProductId = x.StoreOutStoreProductId,
                        ReceivingId = x.Receiving.ReceivingId,
                        OutStoreProductId = x.OutStoreProduct.OutStoreProductId,
                        ProductId = x.OutStoreProduct.Product.Id,
                        OutStoreNo = x.OutStoreProduct.OutStore.OutStoreNo,
                        ProductCode = x.OutStoreProduct.Product.Code,
                        ProductClassification = x.OutStoreProduct.Product.CommonCode != null ? x.OutStoreProduct.Product.CommonCode.Name : "",
                        ProductName = x.OutStoreProduct.Product.Name,
                        ProductUnit = x.OutStoreProduct.Product.Unit,
                        ProductStandard = x.OutStoreProduct.Product.Standard,
                        ProductStandardUnit = x.ProductStandardUnit,
                        ProductStandardUnitCount = x.ProductStandardUnitCount,
                        ProductTaxInfo = x.ProductTaxInfo,
                        ProductReceivingCount = x.ProductReceivingCount,
                        ProductBuyPrice = x.ProductBuyPrice,
                        ProductSupplyPrice = x.ProductSupplyPrice,
                        ProductTaxPrice = x.ProductTaxPrice,
                        ProductTotalPrice = x.ProductTotalPrice,
                        ProductLOT = x.LotName,
                        
                        ReceivingProductMemo = x.ReceivingProductMemo,
                        BarcodeIssueDate = x.Barcode !=null? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                        BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                        BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                        
                        PartnerName = x.Receiving.Partner.Name,
                        ReceivingDate = x.Receiving.ReceivingDate.ToString("yyyy-MM-dd")

                    }).ToListAsync();

                    var Res = new Response<IEnumerable<InBarcodeResponse>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res
                    };

                    return Res;


            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<InBarcodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<InBarcodeResponse>> GetInBarcodeIssueData(InBarcodeRequest req)
        {
            try
            {
                var res = await _db.StoreOutStoreProducts
                .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                .Where(x=>x.StoreOutStoreProductId == req.StoreOutStoreProductId)
                .Select(x => new InBarcodeResponse
                {
                    StoreOutStoreProductId = x.StoreOutStoreProductId,
                    ReceivingId = x.Receiving.ReceivingId,
                    OutStoreProductId = x.OutStoreProduct.OutStoreProductId,
                    ProductId = x.OutStoreProduct.Product.Id,
                    OutStoreNo = x.OutStoreProduct.OutStore.OutStoreNo,
                    ProductCode = x.OutStoreProduct.Product.Code,
                    ProductClassification = x.OutStoreProduct.Product.CommonCode != null ? x.OutStoreProduct.Product.CommonCode.Name : "",
                    ProductName = x.OutStoreProduct.Product.Name,
                    ProductUnit = x.OutStoreProduct.Product.Unit,
                    ProductStandard = x.OutStoreProduct.Product.Standard,
                    ProductStandardUnit = x.ProductStandardUnit,
                    ProductStandardUnitCount = x.ProductStandardUnitCount,
                    ProductTaxInfo = x.ProductTaxInfo,
                    ProductReceivingCount = x.ProductReceivingCount,
                    ProductBuyPrice = x.ProductBuyPrice,
                    ProductSupplyPrice = x.ProductSupplyPrice,
                    ProductTaxPrice = x.ProductTaxPrice,
                    ProductTotalPrice = x.ProductTotalPrice,
                    ProductLOT = x.LotName,

                    ReceivingProductMemo = x.ReceivingProductMemo,
                    BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                    BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                    BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                    PartnerName = x.Receiving.Partner.Name,
                    ReceivingDate = x.Receiving.ReceivingDate.ToString("yyyy-MM-dd")

                }).FirstOrDefaultAsync();

                var Res = new Response<InBarcodeResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;


            }
            catch (Exception ex)
            {

                var Res = new Response<InBarcodeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<bool>> UpdateInBarcodeIssue(InBarcodeRequest req)
        {
            try
            {
                var storePrd = await _db.StoreOutStoreProducts.Include(x=>x.Barcode)
                    .Where(x => x.StoreOutStoreProductId == req.StoreOutStoreProductId)
                    .FirstOrDefaultAsync();


                if(storePrd.Barcode == null)
                {
                    var newBm = new BarcodeMaster
                    {
                        BarcodeIssueDate = DateTime.UtcNow.AddHours(9),
                        BarcodeIssueCount = req.BarcodeIssueCount,
                        BarcodeReIssueCount = 0,
                    };

                    storePrd.Barcode = newBm;

                }
                else
                {
                    storePrd.Barcode.BarcodeIssueCount = req.BarcodeIssueCount;
                    storePrd.Barcode.BarcodeReIssueCount = req.BarcodeReIssueCount;
                }

                _db.StoreOutStoreProducts.Update(storePrd);
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
                    Data = false
                };

                return Res;
            }
        }



        public async Task<Response<IEnumerable<OutBarcodeResponse>>> GetOutBarcodeIssueList(OutBarcodeRequest req)
        {
            try
            {

                var _outOrderProduct = await _db.OutOrderProductsStocks
                   .Include(x => x.OutOrderProduct).ThenInclude(x=>x.Product)
                    .Where(x => req.ShipmentNo == "" ? true : x.OutOrderProduct.OutOrder.ShipmentNo.Contains(req.ShipmentNo))
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate >= Convert.ToDateTime(req.ShipmentStartDate))
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate <= Convert.ToDateTime(req.ShipmentEndDate))
                    .Where(x => req.PartnerId == 0 ? true : x.OutOrderProduct.OutOrder.Partner.Id == req.PartnerId)
                    .Where(x => req.ReIssue ? x.Barcode != null : true)
                    .Where(x => req.ProductId == 0 ? true : x.OutOrderProduct.Product.Id == req.ProductId)
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x => x.OutOrderProduct.OutOrder.ShipmentDate)
                    .Select(x => new OutBarcodeResponse
                    {
                        OutOrderId = x.OutOrderProduct.OutOrder.OutOrderId,
                        OutOrderProductStockId = x.OutOrderProductStockId,
                        ShipmentNo = x.OutOrderProduct.OutOrder.ShipmentNo,
                        OrderNo = x.OutOrderProduct.OrderProduct == null ? "" : x.OutOrderProduct.OrderProduct.Order.OrderNo,
                        ShipmentDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                        PartnerName = x.OutOrderProduct.OutOrder.Partner.Name, // 거래처이름
                        ProductCode = x.OutOrderProduct.Product.Code,
                        ProductClassification = x.OutOrderProduct.Product.CommonCode.Name,
                        ProductName = x.OutOrderProduct.Product.Name,
                        ProductStandard = x.OutOrderProduct.Product.Standard,
                        ProductUnit = x.OutOrderProduct.Product.Unit,
                        ProductStandardUnit = x.OutOrderProduct.OrderProduct == null? "": x.OutOrderProduct.OrderProduct.ProductStandardUnit,
                        ProductStandardUnitCount = x.OutOrderProduct.OrderProduct == null? 0: x.OutOrderProduct.OrderProduct.ProductStandardUnitCount,
                        ProductShipmentProductCount = x.OutOrderProduct.ProductShipmentCount, 
                        ProductLOT = x.Lot.LotName,

                        BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                        BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                        BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,

                    }).ToListAsync();

                var Res = new Response<IEnumerable<OutBarcodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _outOrderProduct
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OutBarcodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<OutBarcodeResponse>> GetOutBarcodeIssueData(OutBarcodeRequest req)
        {
            try
            {
                var _outOrderProduct = await _db.OutOrderProductsStocks
                    .Where(x=>x.OutOrderProductStockId == req.OutOrderProductStockId)
                    .Select(x => new OutBarcodeResponse
                    {
                        OutOrderId = x.OutOrderProduct.OutOrder.OutOrderId,
                        OutOrderProductStockId = x.OutOrderProductStockId,
                        ShipmentNo = x.OutOrderProduct.OutOrder.ShipmentNo,
                        ShipmentDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                        PartnerName = x.OutOrderProduct.OutOrder.Partner.Name, // 거래처이름
                        ProductCode = x.OutOrderProduct.Product.Code,
                        ProductClassification = x.OutOrderProduct.Product.CommonCode.Name,
                        ProductName = x.OutOrderProduct.Product.Name,
                        ProductStandard = x.OutOrderProduct.Product.Standard,
                        ProductUnit = x.OutOrderProduct.Product.Unit,
                        ProductStandardUnit = x.OutOrderProduct.OrderProduct.ProductStandardUnit,
                        ProductStandardUnitCount = x.OutOrderProduct.OrderProduct.ProductStandardUnitCount,
                        ProductShipmentProductCount = x.OutOrderProduct.ProductShipmentCount,
                        ProductLOT = x.Lot.LotName,
                        OrderNo = x.OutOrderProduct.OrderProduct == null ? "" : x.OutOrderProduct.OrderProduct.Order.OrderNo,

                        BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                        BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                        BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,

                    }).FirstOrDefaultAsync();

                var Res = new Response<OutBarcodeResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _outOrderProduct
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<OutBarcodeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> UpdateOutBarcodeIssue(OutBarcodeRequest req)
        {
            try
            {
                var outPrd = await _db.OutOrderProductsStocks.Include(x=>x.Barcode)
                    .Where(x => x.OutOrderProductStockId == req.OutOrderProductStockId)
                    .FirstOrDefaultAsync();


                if (outPrd.Barcode == null)
                {
                    var newBm = new BarcodeMaster
                    {
                        BarcodeIssueDate = DateTime.UtcNow.AddHours(9),
                        BarcodeIssueCount = req.BarcodeIssueCount,
                        BarcodeReIssueCount = 0,
                    };

                    outPrd.Barcode = newBm;

                }
                else
                {
                    outPrd.Barcode.BarcodeIssueCount = req.BarcodeIssueCount;
                    outPrd.Barcode.BarcodeReIssueCount = req.BarcodeReIssueCount;
                }

                _db.OutOrderProductsStocks.Update(outPrd);
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

        public async Task<Response<IEnumerable<ProcessBarcodeResponse>>> GetProcessBarcodeIssueList(ProcessBarcodeRequest req)
        {
            try
            {
                var _processProgress = await _db.ProcessProgresses
                    .Where(x => x.WorkEndDateTime >= Convert.ToDateTime(req.WorkStartDate))
                    .Where(x => x.WorkEndDateTime <= Convert.ToDateTime(req.WorkEndDate).AddDays(1))
                    .Where(x => req.ProductId == 0 ? true : x.WorkOrderProducePlan.Product.Id == req.ProductId)
                    .Where(x => req.FacilityId == 0 ? true : x.WorkOrderProducePlan.Facility == null? false : x.WorkOrderProducePlan.Facility.Id == req.FacilityId)
                    .Where(x => req.MoldId == 0 ? true : x.WorkOrderProducePlan.Mold == null? false : x.WorkOrderProducePlan.Mold.Id == req.MoldId)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => req.ReIssue ? x.Barcode != null : true)

                    .OrderBy(x => x.WorkEndDateTime)
                    .Select(x => new ProcessBarcodeResponse
                    {
                        WorkStartDate = x.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                        WorkEndDate = x.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        ProcessProgressId = x.ProcessProgressId,
                        FacilityCode = x.WorkOrderProducePlan.Facility.Code,
                        FacilityName = x.WorkOrderProducePlan.Facility.Name,
                        MoldCode = x.WorkOrderProducePlan.Mold.Code,
                        MoldName = x.WorkOrderProducePlan.Mold.Name,

                        ProductCode = x.WorkOrderProducePlan.Product.Code,
                        ProductClassification = x.WorkOrderProducePlan.Product.CommonCode.Name,
                        ProductName = x.WorkOrderProducePlan.Product.Name,
                        ProductStandard = x.WorkOrderProducePlan.Product.Standard,
                        ProductUnit = x.WorkOrderProducePlan.Product.Unit,

                        ProcessId = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        ProcessCode = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        ProcessName = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        ProductionCount = x.ProductionQuantity,
                        ProductLOT = _db.Lots
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessType == "P")
                            .Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                            .Select(y=>y.LotName).FirstOrDefault(),

                        BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                        BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                        BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                    })
                .ToListAsync();

                //LOT FILTER

                var filtered = _processProgress
                    .Where(x => req.ProcessId == 0 ? true : x.ProcessId == req.ProcessId)
                    .Where(x => x.ProductLOT == null? true : x.ProductLOT.Contains(req.ProductLOT));

                var Res = new Response<IEnumerable<ProcessBarcodeResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filtered
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<ProcessBarcodeResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<ProcessBarcodeResponse>> GetProcessBarcodeIssueData(ProcessBarcodeRequest req)
        {
            try
            {
                var _processProgress = await _db.ProcessProgresses
                    .Where(x => x.ProcessProgressId == req.ProcessProgressId)
                    .Select(x => new ProcessBarcodeResponse
                    {
                        WorkStartDate = x.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                        WorkEndDate = x.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        ProcessProgressId = x.ProcessProgressId,
                        FacilityCode = x.WorkOrderProducePlan.Facility.Code,
                        FacilityName = x.WorkOrderProducePlan.Facility.Name,
                        MoldCode = x.WorkOrderProducePlan.Mold.Code,
                        MoldName = x.WorkOrderProducePlan.Mold.Name,

                        ProductCode = x.WorkOrderProducePlan.Product.Code,
                        ProductClassification = x.WorkOrderProducePlan.Product.CommonCode.Name,
                        ProductName = x.WorkOrderProducePlan.Product.Name,
                        ProductStandard = x.WorkOrderProducePlan.Product.Standard,
                        ProductUnit = x.WorkOrderProducePlan.Product.Unit,

                        ProcessId = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        ProcessCode = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Code).FirstOrDefault(),
                        ProcessName = x.WorkOrderProducePlan.WorkerOrder.ProducePlansProduct != null ?
                            x.WorkOrderProducePlan.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),

                        ProductionCount = x.ProductionQuantity,
                        ProductLOT = _db.Lots
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessType == "P")
                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                            .Select(y => y.LotName).FirstOrDefault(),

                        BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                        BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                        BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                    })
                .FirstOrDefaultAsync();

                //LOT FILTER


                var Res = new Response<ProcessBarcodeResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _processProgress
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<ProcessBarcodeResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> UpdateProcessBarcodeIssue(ProcessBarcodeRequest req)
        {
            try
            {
                var _processProgress = await _db.ProcessProgresses.Include(x=>x.Barcode)
                    .Where(x => x.ProcessProgressId == req.ProcessProgressId)
                    .FirstOrDefaultAsync();

                if (_processProgress.Barcode == null)
                {
                    var newBm = new BarcodeMaster
                    {
                        BarcodeIssueDate = DateTime.UtcNow.AddHours(9),
                        BarcodeIssueCount = req.BarcodeIssueCount,
                        BarcodeReIssueCount = 0,
                    };

                    _processProgress.Barcode = newBm;

                }
                else
                {
                    _processProgress.Barcode.BarcodeIssueCount = req.BarcodeIssueCount;
                    _processProgress.Barcode.BarcodeReIssueCount = req.BarcodeReIssueCount;
                }

                _db.ProcessProgresses.Update(_processProgress);

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



        public async Task<Response<IEnumerable<BarcodeMasterResponse>>> GetMasterBarcodeIssueList(BarcodeMasterRequest req)
        {
            try
            {
                //1.거래처
                //2.품목
                //3.설비
                //4.금형
                //5.Defective
                //6.공정

                var _partners = await _db.Partners
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Name.Contains(req.SearchString) || x.Code.Contains(req.SearchString) || x.CommonCode.Name.Contains(req.SearchString))
                    .Where(x => req.ReIssue ? x.Barcode != null : true)
                    .Where(x => (req.Type == "" || req.Type == "거래처") ? true : false)
                    .Select(x => new BarcodeMasterResponse
                    {
                        DefectiveId = 0,
                        FacilityId = 0,
                        ProcessId = 0,
                        ProductId = 0,
                        MoldId = 0,
                        PartnerId = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Classification = x.CommonCode.Name,
                        Memo = x.Memo,
                        Standard = "-",
                        Type = "거래처",
                        Unit = "-",
                        IsUsing = x.IsUsing ? "사용" : "미사용",

                        BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                        BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                        BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                    }).ToListAsync();


                 var _process = await _db.Processes
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Name.Contains(req.SearchString) || x.Code.Contains(req.SearchString) || x.CommonCode.Name.Contains(req.SearchString))
                    .Where(x => req.ReIssue ? x.Barcode != null : true)
                    .Where(x => (req.Type == "" || req.Type == "공정") ? true : false)
                    .Select(x => new BarcodeMasterResponse
                    {
                        DefectiveId = 0,
                        FacilityId = 0,
                        ProcessId = x.Id,
                        ProductId = 0,
                        MoldId = 0,
                        PartnerId = 0,
                        Code = x.Code,
                        Name = x.Name,
                        Classification = x.CommonCode.Name,
                        Memo = x.Memo,
                        Standard = "-",
                        Type = "공정",
                        Unit = "-",
                        IsUsing = x.IsUsing ? "사용" : "미사용",


                        BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                        BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                        BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                    }).ToListAsync();

                var _facility = await _db.Facilitys
                   .Where(x => x.IsDeleted == false)
                   .Where(x => x.Name.Contains(req.SearchString) || x.Code.Contains(req.SearchString) || x.CommonCode.Name.Contains(req.SearchString))
                   .Where(x => req.ReIssue ? x.Barcode != null : true)
                   .Where(x => (req.Type == "" || req.Type == "설비") ? true : false)
                   .Select(x => new BarcodeMasterResponse
                   {
                       DefectiveId = 0,
                       FacilityId = x.Id,
                       ProcessId = 0,
                       ProductId = 0,
                       MoldId = 0,
                       PartnerId = 0,
                       Code = x.Code,
                       Name = x.Name,
                       Classification = x.CommonCode.Name,
                       Memo = x.Memo,
                       Standard = "-",
                       Type = "설비",
                       Unit = "-",
                       IsUsing = x.IsUsing ? "사용" : "미사용",

                       BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                       BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                       BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                   }).ToListAsync();


                var _mold = await _db.Molds
                   .Where(x => x.IsDeleted == false)
                   .Where(x => x.Name.Contains(req.SearchString) || x.Code.Contains(req.SearchString) || x.CommonCode.Name.Contains(req.SearchString))
                   .Where(x => req.ReIssue ? x.Barcode != null : true)
                   .Where(x => (req.Type == "" || req.Type == "금형") ? true : false)
                   .Select(x => new BarcodeMasterResponse
                   {
                       DefectiveId = 0,
                       FacilityId = 0,
                       ProcessId = 0,
                       ProductId = 0,
                       MoldId = x.Id,
                       PartnerId = 0,
                       Code = x.Code,
                       Name = x.Name,
                       Classification = x.CommonCode.Name,
                       Memo = x.Memo,
                       Standard = "-",
                       Type = "금형",
                       Unit = "-",
                       IsUsing = x.Status,

                       BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                       BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                       BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                   }).ToListAsync();

                var _defective = await _db.Defectives
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Name.Contains(req.SearchString) || x.Code.Contains(req.SearchString) || x.CommonCode.Name.Contains(req.SearchString))
                    .Where(x => req.ReIssue ? x.Barcode != null : true)
                    .Where(x => (req.Type == "" || req.Type == "불량") ? true : false)
                    .Select(x => new BarcodeMasterResponse
                    {
                        DefectiveId = x.Id,
                        FacilityId = 0,
                        ProcessId = 0,
                        ProductId = 0,
                        MoldId = 0,
                        PartnerId = 0,
                           Code = x.Code,
                        Name = x.Name,
                        Classification = x.CommonCode.Name,
                        Memo = x.Memo,
                        Standard = "-",
                           Type = "불량",
                        Unit = "-",
                        IsUsing = x.IsUsing ? "사용" : "미사용",
                    
                        BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                        BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                        BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                    }).ToListAsync();


                var _product = await _db.Products
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Name.Contains(req.SearchString) || x.Code.Contains(req.SearchString) || x.CommonCode.Name.Contains(req.SearchString))
                    .Where(x => req.ReIssue ? x.Barcode != null : true)
                    .Where(x => (req.Type == "" || req.Type == "품목") ? true : false)
                    .Select(x => new BarcodeMasterResponse
                    {
                        DefectiveId = 0,
                        FacilityId = 0,
                        ProcessId = 0,
                        ProductId = x.Id,
                        MoldId = 0,
                        PartnerId = 0,
                        Code = x.Code,
                        Name = x.Name,
                        Classification = x.CommonCode.Name,
                        Memo = x.Memo,
                        Standard = x.Standard,
                        Type = "품목",
                        Unit = x.Unit,
                        IsUsing = x.IsUsing ? "사용" : "미사용",

                        BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                        BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                        BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                    }).ToListAsync();

                //LOT FILTER

                List<BarcodeMasterResponse> resTotal = new List<BarcodeMasterResponse>();

                resTotal.AddRange(_partners);
                resTotal.AddRange(_process);
                resTotal.AddRange(_facility);
                resTotal.AddRange(_mold);
                resTotal.AddRange(_defective);
                resTotal.AddRange(_product);

                var Res = new Response<IEnumerable<BarcodeMasterResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = resTotal
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<BarcodeMasterResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<BarcodeMasterResponse>> GetMasterBarcodeIssueData(BarcodeMasterRequest req)
        {
            try
            {
                if(req.PartnerId != 0)
                {
                    var _res = await _db.Partners
                        .Where(x => x.Id == req.PartnerId)
                        .Select(x => new BarcodeMasterResponse
                        {
                            DefectiveId = 0,
                            FacilityId = 0,
                            ProcessId = 0,
                            ProductId = 0,
                            MoldId = 0,
                            PartnerId = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Classification = x.CommonCode.Name,
                            Memo = x.Memo,
                            Standard = "-",
                            Type = "거래처",
                            Unit = "-",
                            IsUsing = x.IsUsing ? "사용" : "미사용",

                            BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                            BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                            BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                        }).FirstOrDefaultAsync();

                    var Res = new Response<BarcodeMasterResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _res
                    };

                    return Res;

                }
                else if (req.ProcessId != 0)
                {
                    var _res = await _db.Processes
                        .Where(x => x.Id == req.ProcessId)
                        .Select(x => new BarcodeMasterResponse
                        {
                            DefectiveId = 0,
                            FacilityId = 0,
                            ProcessId = x.Id,
                            ProductId = 0,
                            MoldId = 0,
                            PartnerId = 0,
                            Code = x.Code,
                            Name = x.Name,
                            Classification = x.CommonCode.Name,
                            Memo = x.Memo,
                            Standard = "-",
                            Type = "공정",
                            Unit = "-",
                            IsUsing = x.IsUsing ? "사용" : "미사용",

                            BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                            BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                            BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                        }).FirstOrDefaultAsync();

                    var Res = new Response<BarcodeMasterResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _res
                    };

                    return Res;
                }
                else if (req.FacilityId != 0)
                {
                    var _res = await _db.Facilitys
                        .Where(x => x.Id == req.FacilityId)
                        .Select(x => new BarcodeMasterResponse
                        {
                            DefectiveId = 0,
                            FacilityId = x.Id,
                            ProcessId = 0,
                            ProductId = 0,
                            MoldId = 0,
                            PartnerId = 0,
                            Code = x.Code,
                            Name = x.Name,
                            Classification = x.CommonCode.Name,
                            Memo = x.Memo,
                            Standard = "-",
                            Type = "설비",
                            Unit = "-",
                            IsUsing = x.IsUsing ? "사용" : "미사용",

                            BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                            BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                            BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                        }).FirstOrDefaultAsync();

                    var Res = new Response<BarcodeMasterResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _res
                    };

                    return Res;
                }
                else if (req.MoldId != 0)
                {
                    var _res = await _db.Molds
                        .Where(x => x.Id == req.MoldId)
                        .Select(x => new BarcodeMasterResponse
                        {
                            DefectiveId = 0,
                            FacilityId = 0,
                            ProcessId = 0,
                            ProductId = 0,
                            MoldId = x.Id,
                            PartnerId = 0,
                            Code = x.Code,
                            Name = x.Name,
                            Classification = x.CommonCode.Name,
                            Memo = x.Memo,
                            Standard = "-",
                            Type = "금형",
                            Unit = "-",
                            IsUsing = x.Status,

                            BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                            BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                            BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                        }).FirstOrDefaultAsync();

                    var Res = new Response<BarcodeMasterResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _res
                    };

                    return Res;
                }

                else if (req.DefectiveId != 0)
                {
                    var _res = await _db.Defectives
                        .Where(x => x.Id == req.DefectiveId)
                        .Select(x => new BarcodeMasterResponse
                        {
                            DefectiveId = x.Id,
                            FacilityId = 0,
                            ProcessId = 0,
                            ProductId = 0,
                            MoldId = 0,
                            PartnerId = 0,
                            Code = x.Code,
                            Name = x.Name,
                            Classification = x.CommonCode.Name,
                            Memo = x.Memo,
                            Standard = "-",
                            Type = "불량",
                            Unit = "-",
                            IsUsing = x.IsUsing ? "사용" : "미사용",


                            BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                            BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                            BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                        }).FirstOrDefaultAsync();

                    var Res = new Response<BarcodeMasterResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _res
                    };

                    return Res;
                }
                else
                {
                    var _res = await _db.Products
                        .Where(x => x.Id == req.ProductId)
                        .Select(x => new BarcodeMasterResponse
                        {
                            DefectiveId = 0,
                            FacilityId = 0,
                            ProcessId = 0,
                            ProductId = x.Id,
                            MoldId = 0,
                            PartnerId = 0,
                            Code = x.Code,
                            Name = x.Name,
                            Classification = x.CommonCode.Name,
                            Memo = x.Memo,
                            Standard = x.Standard,
                            Type = "품목",
                            Unit = x.Unit,
                            IsUsing = x.IsUsing ? "사용" : "미사용",


                            BarcodeIssueDate = x.Barcode != null ? x.Barcode.BarcodeIssueDate.ToString("yyyy-MM-dd") : "-",
                            BarcodeIssueCount = x.Barcode != null ? x.Barcode.BarcodeIssueCount : 0,
                            BarcodeReIssueCount = x.Barcode != null ? x.Barcode.BarcodeReIssueCount : 0,
                        }).FirstOrDefaultAsync();

                    var Res = new Response<BarcodeMasterResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _res
                    };

                    return Res;
                }
                

            }
            catch (Exception ex)
            {

                var Res = new Response<BarcodeMasterResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
     
        
        public async Task<Response<bool>> UpdateMasterBarcodeIssue(BarcodeMasterRequest req)
        {
            try
            {
                if (req.PartnerId != 0)
                {
                    var _res = await _db.Partners
                        .Include(x => x.Barcode)
                        .Where(x => x.Id == req.PartnerId)
                        .FirstOrDefaultAsync();

                    if (_res.Barcode == null)
                    {
                        var newBm = new BarcodeMaster
                        {
                            BarcodeIssueDate = DateTime.UtcNow.AddHours(9),
                            BarcodeIssueCount = req.BarcodeIssueCount,
                            BarcodeReIssueCount = 0,
                        };

                        _res.Barcode = newBm;

                    }
                    else
                    {
                        _res.Barcode.BarcodeIssueCount = req.BarcodeIssueCount;
                        _res.Barcode.BarcodeReIssueCount = req.BarcodeReIssueCount;
                    }

                    _db.Partners.Update(_res);

                    await Save();


                    var Res = new Response<bool>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = true
                    };

                    return Res;

                }
                else if (req.ProcessId != 0)
                {
                    var _res = await _db.Processes
                        .Include(x => x.Barcode)
                        .Where(x => x.Id == req.ProcessId)
                        .FirstOrDefaultAsync();

                    if (_res.Barcode == null)
                    {
                        var newBm = new BarcodeMaster
                        {
                            BarcodeIssueDate = DateTime.UtcNow.AddHours(9),
                            BarcodeIssueCount = req.BarcodeIssueCount,
                            BarcodeReIssueCount = 0,
                        };

                        _res.Barcode = newBm;

                    }
                    else
                    {
                        _res.Barcode.BarcodeIssueCount = req.BarcodeIssueCount;
                        _res.Barcode.BarcodeReIssueCount = req.BarcodeReIssueCount;
                    }

                    _db.Processes.Update(_res);

                    await Save();


                    var Res = new Response<bool>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = true
                    };

                    return Res;
                }
                else if (req.FacilityId != 0)
                {
                    var _res = await _db.Facilitys
                        .Include(x => x.Barcode)
                        .Where(x => x.Id == req.FacilityId)
                        .FirstOrDefaultAsync();

                    if (_res.Barcode == null)
                    {
                        var newBm = new BarcodeMaster
                        {
                            BarcodeIssueDate = DateTime.UtcNow.AddHours(9),
                            BarcodeIssueCount = req.BarcodeIssueCount,
                            BarcodeReIssueCount = 0,
                        };

                        _res.Barcode = newBm;

                    }
                    else
                    {
                        _res.Barcode.BarcodeIssueCount = req.BarcodeIssueCount;
                        _res.Barcode.BarcodeReIssueCount = req.BarcodeReIssueCount;
                    }

                    _db.Facilitys.Update(_res);

                    await Save();


                    var Res = new Response<bool>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = true
                    };

                    return Res;
                }
                else if (req.MoldId != 0)
                {
                    var _res = await _db.Molds
                        .Include(x => x.Barcode)
                        .Where(x => x.Id == req.MoldId)
                        .FirstOrDefaultAsync();

                    if (_res.Barcode == null)
                    {
                        var newBm = new BarcodeMaster
                        {
                            BarcodeIssueDate = DateTime.UtcNow.AddHours(9),
                            BarcodeIssueCount = req.BarcodeIssueCount,
                            BarcodeReIssueCount = 0,
                        };

                        _res.Barcode = newBm;

                    }
                    else
                    {
                        _res.Barcode.BarcodeIssueCount = req.BarcodeIssueCount;
                        _res.Barcode.BarcodeReIssueCount = req.BarcodeReIssueCount;
                    }

                    _db.Molds.Update(_res);

                    await Save();


                    var Res = new Response<bool>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = true
                    };

                    return Res;
                }

                else if (req.DefectiveId != 0)
                {
                    var _res = await _db.Defectives
                        .Include(x => x.Barcode)
                        .Where(x => x.Id == req.DefectiveId)
                        .FirstOrDefaultAsync();

                    if (_res.Barcode == null)
                    {
                        var newBm = new BarcodeMaster
                        {
                            BarcodeIssueDate = DateTime.UtcNow.AddHours(9),
                            BarcodeIssueCount = req.BarcodeIssueCount,
                            BarcodeReIssueCount = 0,
                        };

                        _res.Barcode = newBm;

                    }
                    else
                    {
                        _res.Barcode.BarcodeIssueCount = req.BarcodeIssueCount;
                        _res.Barcode.BarcodeReIssueCount = req.BarcodeReIssueCount;
                    }

                    _db.Defectives.Update(_res);

                    await Save();


                    var Res = new Response<bool>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = true
                    };

                    return Res;
                }
                else
                {
                    var _res = await _db.Products
                        .Include(x => x.Barcode)
                        .Where(x => x.Id == req.ProductId)
                        .FirstOrDefaultAsync();

                    if (_res.Barcode == null)
                    {
                        var newBm = new BarcodeMaster
                        {
                            BarcodeIssueDate = DateTime.UtcNow.AddHours(9),
                            BarcodeIssueCount = req.BarcodeIssueCount,
                            BarcodeReIssueCount = 0,
                        };

                        _res.Barcode = newBm;

                    }
                    else
                    {
                        _res.Barcode.BarcodeIssueCount = req.BarcodeIssueCount;
                        _res.Barcode.BarcodeReIssueCount = req.BarcodeReIssueCount;
                    }

                    _db.Products.Update(_res);

                    await Save();


                    var Res = new Response<bool>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = true
                    };

                    return Res;
                }
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


        public async Task Save ()
        {
            await _db.SaveChangesAsync();
        }

    }
}
