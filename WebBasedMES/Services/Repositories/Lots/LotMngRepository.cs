using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.Lots;


using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Lot;

namespace WebBasedMES.Services.Repositories.Lots
{
    public class LotMngRepository : ILotMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public LotMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        public async Task<Response<bool>> CreateLot(LotRequestCrud lotRequest)
        {
            try
            {

                var pro = await _db.ProcessProgresses.Where(x => x.ProcessProgressId == lotRequest.processProgressId).FirstOrDefaultAsync();
                var _lot = new LotEntity();
                {
                    _lot.LotName = lotRequest.lotName;
                    _lot.ProcessProgress = pro;
                    _lot.ProcessType = lotRequest.processType;
                    _lot.IsDeleted = lotRequest.isDeleted;
                };



                var result = await _db.Lots.AddAsync(_lot);
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

        public async Task<Response<bool>> UpdateLot(LotRequestCrud lotRequest)
        {
            try
            {

                var pro = await _db.ProcessProgresses.FindAsync(lotRequest.processProgressId);

                var _lot = new LotEntity();
                {
                    _lot.LotId = lotRequest.lotId; //수주ID
                    _lot.IsDeleted = lotRequest.isDeleted;
                    _lot.ProcessProgress = pro;
                    _lot.LotName = lotRequest.lotName;
                    _lot.IsDeleted = lotRequest.isDeleted;
                    _lot.ProcessType = lotRequest.processType;
                };

                _db.Lots.Update(_lot);

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
        public async Task<Response<bool>> DeleteLot(LotRequestCrud lotRequest)
        {
            try
            {

                foreach (int item in lotRequest.lotIdArray)
                {
                    var subItem = await _db.LotCounts.Where(x => x.Lot.LotId == item).ToListAsync();

                    foreach (LotCount sub in subItem)
                    {
                        sub.IsDeleted = 1;
                    }
                    _db.LotCounts.UpdateRange(subItem);
                    var mst = await _db.Lots.FindAsync(item);
                    mst.IsDeleted = 1;
                    _db.Lots.Update(mst);
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

        public async Task<Response<IEnumerable<LotResponse01>>> invenList(LotRequestCrud lotRequest)
        {
            try
            {

                //제품 Processes Counts >0
                if(lotRequest.InvenType == "PRD")
                {
                    var _prd = await _db.Products
                        .Where(x=>x.IsDeleted == false)
                        .Where(x=>x.Processes.Where(y=>y.IsDeleted == 0).Count()>0)
                        .Include(x => x.Item).ThenInclude(x => x.CommonCode)
                        .Where(x => lotRequest.productId == 0 ? true : x.Id == lotRequest.productId)
                        .Where(x => lotRequest.productIsUsingStr == "ALL" ? true : (lotRequest.productIsUsingStr == "Y" ? x.IsUsing == true : x.IsUsing == false))
                        .Where(x => x.Name.Contains(lotRequest.SearchStr) || x.Code.Contains(lotRequest.SearchStr) || x.Standard.Contains(lotRequest.SearchStr) ||  x.Memo.Contains(lotRequest.SearchStr))
                        .Where(x => lotRequest.TypeStr == "" ? true : x.CommonCode.Name.Contains(lotRequest.TypeStr))
                        .OrderBy(x => x.Code)//내림차순
                        .Select(x => new LotResponse01
                        {
                            productId = x.Id,
                            productCode = x.Code,
                            productClassification = x.CommonCode.Name,
                            productName = x.Name,
                            productStandard = x.Standard,
                            productUnit = x.Unit,
                            optimumStock = x.OptimumStock,

                            inventory = (_db.LotCounts
                                .Where(o1 => o1.IsDeleted == 0)
                                .Where(o1 => x.Id == o1.Product.Id)
                                .Select(o1 => o1.StoreOutCount + o1.ProduceCount + o1.ModifyCount - o1.ConsumeCount - o1.DefectiveCount - o1.OutOrderCount)
                                .Sum()),
                            productMemo = x.Memo,
                            productIsUsing = x.IsUsing,

                        }).ToListAsync();

                    var Res1 = new Response<IEnumerable<LotResponse01>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _prd
                    };

                    return Res1;
                }
                if(lotRequest.InvenType == "ITEM")
                {
                    var _item = await _db.Products
                        .Where(x => x.IsDeleted == false)
                        .Where(x => x.Processes.Where(y=>y.IsDeleted == 0).Count() == 0)
                        .Include(x => x.Item)
                        .ThenInclude(x => x.CommonCode)
                        .Where(x => lotRequest.productId == 0 ? true : x.Id == lotRequest.productId)
                        .Where(x => lotRequest.productIsUsingStr == "ALL" ? true : (lotRequest.productIsUsingStr == "Y" ? x.IsUsing == true : x.IsUsing == false))
                                                .Where(x => x.Name.Contains(lotRequest.SearchStr) || x.Code.Contains(lotRequest.SearchStr) || x.Standard.Contains(lotRequest.SearchStr) || x.Memo.Contains(lotRequest.SearchStr))
                        .Where(x => lotRequest.TypeStr == "" ? true : x.CommonCode.Name.Contains(lotRequest.TypeStr))
                        .OrderBy(x => x.Code) //내림차순
                        .Select(x => new LotResponse01
                        {
                            productId = x.Id,
                            productCode = x.Code,
                            productClassification = x.CommonCode.Name,
                            productName = x.Name,
                            productStandard = x.Standard,
                            productUnit = x.Unit,
                            optimumStock = x.OptimumStock,
                    
                            inventory = (_db.LotCounts
                                .Where(o1 => o1.IsDeleted == 0)
                                .Where(o1 => x.Id == o1.Product.Id)
                                .Select(o1=>o1.StoreOutCount + o1.ProduceCount + o1.ModifyCount - o1.ConsumeCount - o1.DefectiveCount - o1.OutOrderCount)
                                .Sum()),

                            productMemo = x.Memo,
                            productIsUsing = x.IsUsing,
                    
                     }).ToListAsync();

                    var Res2 = new Response<IEnumerable<LotResponse01>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _item
                    };
                    return Res2;
                }



                var _result = await _db.Products
                    .Include(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.CommonCode)
                    .Where(x => lotRequest.productId == 0 ? true : x.Id == lotRequest.productId)
                    .Where(x => lotRequest.productIsUsingStr == "ALL" ? true : (lotRequest.productIsUsingStr == "Y" ? x.IsUsing == true : x.IsUsing == false))
                    .Where(x => x.IsDeleted == false)
                                            .Where(x => x.Name.Contains(lotRequest.SearchStr) || x.Code.Contains(lotRequest.SearchStr) || x.Standard.Contains(lotRequest.SearchStr) || x.Memo.Contains(lotRequest.SearchStr))
                        .Where(x => lotRequest.TypeStr == "" ? true : x.CommonCode.Name.Contains(lotRequest.TypeStr))
                    .OrderBy(x => x.Code).Reverse() //내림차순
                    .Select(x => new LotResponse01
                    {
                        productId = x.Id,
                        productCode = x.Code,
                        productClassification = x.CommonCode.Name,
                        productName = x.Name,
                        productStandard = x.Standard,
                        productUnit = x.Unit,
                        optimumStock = x.OptimumStock,

                        inventory = (_db.LotCounts
                                .Where(o1 => o1.IsDeleted == 0)
                                .Where(o1 => x.Id == o1.Product.Id)
                                .Select(o1 => o1.StoreOutCount + o1.ProduceCount + o1.ModifyCount - o1.ConsumeCount - o1.DefectiveCount - o1.OutOrderCount)
                                .Sum()),
                        productMemo = x.Memo,
                        productIsUsing = x.IsUsing,

                    }).ToListAsync();

                var Res = new Response<IEnumerable<LotResponse01>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<LotResponse01>>()
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


        public async Task<Response<IEnumerable<LotCountResponse01>>> getLots(LotRequestCrud lotRequest)
        {
            try
            {
                var _result = _db.LotCounts
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Product.Id == lotRequest.productId)
                    .OrderBy(x => x.Lot.LotName)
                    .Select(x => new LotCountResponse01
                    {
                        productId = x.Product.Id,
                        lotCountId = x.LotCountId,
                        lotId = x.Lot.LotId,
                        productName = x.Product.Name,
                        productCode = x.Product.Code,
                        productClassification = x.Product.CommonCode.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,
                        productLOT = x.Lot.LotName,
                        inventory = (x.StoreOutCount + x.ProduceCount + x.ModifyCount) - (x.OutOrderCount + x.ConsumeCount + x.DefectiveCount),
                    })
                    .ToList()
                    .GroupBy(x => x.productLOT)
                    .Select(x => new LotCountResponse01
                    {
                        lotCountId = x.FirstOrDefault().lotCountId,
                        productName = x.FirstOrDefault().productName,
                        productCode = x.FirstOrDefault().productCode,
                        isSelected = 0,
                        lotId = x.FirstOrDefault().lotId,
                        productClassification = x.FirstOrDefault().productClassification,
                        productId = x.FirstOrDefault().productId,
                        productLOT = x.Key,
                        productStandard = x.FirstOrDefault().productStandard,
                        productUnit = x.FirstOrDefault().productUnit,
                        quantity = 0,  //Modify Quantity
                        inventory = x.Sum(y => y.inventory)
                    }).ToList();


               // var filteredResult = _result.Where(x => x.inventory > 0).ToList();

                var ordered = _result.OrderBy(x => x.productLOT);

                var Res = new Response<IEnumerable<LotCountResponse01>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = ordered
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<LotCountResponse01>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<LotCountResponse02>>> getLotCounts(LotRequestCrud lotRequest)
        {
            try
            {
                if(lotRequest.InvenType == "PRD")
                {
                    var _resInItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() > 0)
                        .Where(x => x.Lot.ProcessType == "I")
                        .Where(x => x.StoreOutCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : _db.StoreOutStoreProducts.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().Receiving.ReceivingDate >= Convert.ToDateTime(lotRequest.registerStartDate)
                            )
                        .Where(x => lotRequest.registerEndDate == "" ? true : _db.StoreOutStoreProducts.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().Receiving.ReceivingDate <= Convert.ToDateTime(lotRequest.registerEndDate)
                            )
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))


                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            productId = x.Product.Id,

                            registerDate = _db.StoreOutStoreProducts.Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().Receiving.ReceivingDate.ToString("yyyy-MM-dd"),
                            type = "입고",
                            details = "제품생산",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,
                            partnerId = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.Partner.Id).FirstOrDefault(),
                            partnerName = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.Partner.Name).FirstOrDefault(),
                            partnerCode = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.Partner.Code).FirstOrDefault(),
                            docuNo = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.ReceivingNo).FirstOrDefault(),
                            count = x.StoreOutCount,
                            productTaxInfo = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductTaxInfo,
                            unitPrice = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductBuyPrice,
                            productStandardUnit = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductStandardUnit,
                            productStandardUnitCount = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductStandardUnitCount,
                            productLOT = x.Lot.LotName,
                           
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            inputQuantity = x.ConsumeCount,
                            defectiveQuantity = x.DefectiveCount,


                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.ProduceCount

                        }).ToListAsync();


                    var _resOutItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() > 0)
                        .Where(x => x.Lot.ProcessType == "O")
                        .Where(x => x.OutOrderCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : _db.OutOrderProductsStocks.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().OutOrderProduct.OutOrder.ShipmentDate >= Convert.ToDateTime(lotRequest.registerStartDate)
                            )
                        .Where(x => lotRequest.registerEndDate == "" ? true : _db.OutOrderProductsStocks.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().OutOrderProduct.OutOrder.ShipmentDate <= Convert.ToDateTime(lotRequest.registerEndDate)
                            )
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))


                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                            type = "출고",
                            details = "제품출고",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,


                            partnerId = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.Id,
                            partnerName = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.Name,
                            partnerCode = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.Code,

                            docuNo = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.ShipmentNo ?? "",

                            count = x.OutOrderCount,

                            productTaxInfo = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.TaxInfo,
                            unitPrice = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.ProductSellPrice,


                            productLOT = x.Lot.LotName,
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,

                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.OutOrderCount

                        }).ToListAsync();


                    var _resConsumeItem = await _db.LotCounts
                         .Where(x => x.IsDeleted == 0)
                         .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() > 0)
                         .Where(x => x.Lot.ProcessType == "C")
                         .Where(x => x.ConsumeCount > 0)
                         .Where(x => lotRequest.registerStartDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(lotRequest.registerStartDate))
                         .Where(x => lotRequest.registerEndDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(lotRequest.registerEndDate))
                         .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))
                         .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                         .Where(x => x.Product != null)
                         .Select(x => new LotCountResponse02
                         {
                             lotCountId = x.LotCountId,
                             registerDate = x.Lot.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd"),

                             type = "투입",
                             details = "공정투입",

                             productCode = x.Product.Code,
                             productClassification = x.Product.CommonCode.Name,
                             productName = x.Product.Name,
                             productStandard = x.Product.Standard,
                             productUnit = x.Product.Unit,

                             partnerId = 0,
                             partnerName = "",
                             partnerCode = "",

                             docuNo = x.Lot.ProcessProgress != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo : "-",
                             count = x.ConsumeCount,
                             productTaxInfo = x.Product.TaxType,
                             unitPrice = x.Product.SellPrice,
                             productLOT = x.Lot.LotName,
                             receivingQuantity = x.StoreOutCount,
                             shipmentQuantity = x.OutOrderCount,
                             modifyQuantity = x.ModifyCount,
                             defectiveQuantity = x.DefectiveCount,
                             inputQuantity = x.ConsumeCount,

                             inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                             priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.ConsumeCount
                         }).ToListAsync();

                    var _resProduceItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() > 0)
                        .Where(x => x.Lot.ProcessType == "P")
                        .Where(x => x.ProduceCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(lotRequest.registerStartDate))
                        .Where(x => lotRequest.registerEndDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(lotRequest.registerEndDate))
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))
                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = x.Lot.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd"),
                            type = "입고",
                            details = "제품생산",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,

                            partnerId = 0,
                            partnerName = "",
                            partnerCode = "",

                            docuNo = x.Lot.ProcessProgress != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo : "-",
                            count = x.ProduceCount,
                            productTaxInfo = x.Product.TaxType,
                            unitPrice = x.Product.SellPrice,
                            productLOT = x.Lot.LotName,
                           // priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,
                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.ProduceCount
                        }).ToListAsync();


                     var _resDefItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() > 0)
                        .Where(x => x.Lot.ProcessType == "E")
                        .Where(x => x.OutOrderCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : _db.EtcDefectivesDetails.Where(sosp => sosp.Lot.LotName == x.Lot.LotName)
                            .FirstOrDefault().EtcDefective.DefectiveDate >= Convert.ToDateTime(lotRequest.registerStartDate)
                            )
                        .Where(x => lotRequest.registerEndDate == "" ? true : _db.EtcDefectivesDetails.Where(sosp => sosp.Lot.LotName == x.Lot.LotName)
                            .FirstOrDefault().EtcDefective.DefectiveDate <= Convert.ToDateTime(lotRequest.registerEndDate)
                            )
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))

                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = _db.EtcDefectivesDetails.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().EtcDefective.DefectiveDate.ToString("yyyy-MM-dd"),
                            type = "출고",
                            details = "불량발생",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,


                            partnerId = 0,
                            partnerName ="-",
                            partnerCode ="-",

                            docuNo = "",
                            count = x.DefectiveCount,
                            productTaxInfo = x.Product.TaxType,
                            unitPrice = x.Product.SellPrice,

                            productLOT = x.Lot.LotName,
                           // priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,

                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.DefectiveCount

                        }).ToListAsync();


                    List<LotCountResponse02> resItem = new List<LotCountResponse02>();

                    resItem.AddRange(_resInItem);
                    resItem.AddRange(_resOutItem);
                    resItem.AddRange(_resConsumeItem);
                    resItem.AddRange(_resProduceItem);
                    resItem.AddRange(_resDefItem);


                    var _resItem = resItem
                        .Where(x => lotRequest.partnerId == 0 ? true : x.partnerId == lotRequest.partnerId)
                        .Where(x => lotRequest.docuNo == null ? true : x.docuNo.Contains(lotRequest.docuNo))
                        .Where(x => lotRequest.selectType == "ALL" ? true : (lotRequest.selectType == "I" ? x.type == "입고" : (lotRequest.selectType == "O" ? x.type == "출고" : true)))
                        .OrderByDescending(x => x.registerDate)
                        .ThenBy(x => x.partnerCode);

                    var ResItem = new Response<IEnumerable<LotCountResponse02>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _resItem
                    };
                    return ResItem;
                }
                else if (lotRequest.InvenType == "ITEM")
                {
                    var _resInItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() == 0)
                        .Where(x => x.Lot.ProcessType == "I")
                        .Where(x => x.StoreOutCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : _db.StoreOutStoreProducts.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().Receiving.ReceivingDate >= Convert.ToDateTime(lotRequest.registerStartDate)
                            )
                        .Where(x => lotRequest.registerEndDate == "" ? true : _db.StoreOutStoreProducts.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().Receiving.ReceivingDate <= Convert.ToDateTime(lotRequest.registerEndDate)
                            )
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))


                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            productId = x.Product.Id,

                            registerDate = _db.StoreOutStoreProducts.Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().Receiving.ReceivingDate.ToString("yyyy-MM-dd"),
                            type = "입고",
                            details = "제품입고",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,
                            partnerId = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.Partner.Id).FirstOrDefault(),
                            partnerName = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.Partner.Name).FirstOrDefault(),
                            partnerCode = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.Partner.Code).FirstOrDefault(),
                            docuNo = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.ReceivingNo).FirstOrDefault(),
                            count = x.StoreOutCount,
                            productTaxInfo = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductTaxInfo,
                            unitPrice = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductBuyPrice,
                            productStandardUnit = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductStandardUnit,
                            productStandardUnitCount = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductStandardUnitCount,
                            productLOT = x.Lot.LotName,
                           // priorInventory = (x.OutOrderCount - x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            inputQuantity = x.ConsumeCount,
                            defectiveQuantity = x.DefectiveCount,
                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.StoreOutCount

                        }).ToListAsync();


                    var _resOutItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() == 0)
                        .Where(x => x.Lot.ProcessType == "O")
                        .Where(x => x.OutOrderCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : _db.OutOrderProductsStocks.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().OutOrderProduct.OutOrder.ShipmentDate >= Convert.ToDateTime(lotRequest.registerStartDate)
                            )
                        .Where(x => lotRequest.registerEndDate == "" ? true : _db.OutOrderProductsStocks.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().OutOrderProduct.OutOrder.ShipmentDate <= Convert.ToDateTime(lotRequest.registerEndDate)
                            )
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))


                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                            type = "출고",
                            details = "제품출고",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,


                            partnerId = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.Id,
                            partnerName = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.Name,
                            partnerCode = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.Code,

                            docuNo = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.ShipmentNo?? "",

                            count = x.OutOrderCount,

                            productTaxInfo = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.TaxInfo,
                            unitPrice = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.ProductSellPrice,


                            productLOT = x.Lot.LotName,
                           // priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,
                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.OutOrderCount

                        }).ToListAsync();


                   var _resConsumeItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() == 0)
                        .Where(x => x.Lot.ProcessType == "C")
                        .Where(x => x.ConsumeCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(lotRequest.registerStartDate))
                        .Where(x => lotRequest.registerEndDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(lotRequest.registerEndDate))
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))
                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = x.Lot.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd"),

                            type = "투입",
                            details = "공정투입",

                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,

                            partnerId = 0,
                            partnerName = "",
                            partnerCode = "",

                            docuNo = x.Lot.ProcessProgress != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo : "-",
                            count = x.ConsumeCount,
                            productTaxInfo = x.Product.TaxType,
                            unitPrice = x.Product.SellPrice,
                            productLOT = x.Lot.LotName,
                            //priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,

                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.ConsumeCount

                        }).ToListAsync();

                    var _resProduceItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() == 0)
                        .Where(x => x.Lot.ProcessType == "P")
                        .Where(x => x.ProduceCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(lotRequest.registerStartDate))
                        .Where(x => lotRequest.registerEndDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(lotRequest.registerEndDate))
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))
                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = x.Lot.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd"),
                            type = "입고",
                            details = "제품생산",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,

                            partnerId = 0,
                            partnerName = "",
                            partnerCode = "",

                            docuNo = x.Lot.ProcessProgress != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo : "-",
                            count = x.ProduceCount,
                            productTaxInfo = x.Product.TaxType,
                            unitPrice = x.Product.SellPrice,
                            productLOT = x.Lot.LotName,
                           // priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,

                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.ProduceCount

                        }).ToListAsync();

                    var _resDefItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() == 0)
                        .Where(x => x.Lot.ProcessType == "E")
                        .Where(x => x.OutOrderCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : _db.EtcDefectivesDetails.Where(sosp => sosp.Lot.LotName == x.Lot.LotName)
                            .FirstOrDefault().EtcDefective.DefectiveDate >= Convert.ToDateTime(lotRequest.registerStartDate)
                            )
                        .Where(x => lotRequest.registerEndDate == "" ? true : _db.EtcDefectivesDetails.Where(sosp => sosp.Lot.LotName == x.Lot.LotName)
                            .FirstOrDefault().EtcDefective.DefectiveDate <= Convert.ToDateTime(lotRequest.registerEndDate)
                            )
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))

                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = _db.EtcDefectivesDetails.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().EtcDefective.DefectiveDate.ToString("yyyy-MM-dd"),
                            type = "출고",
                            details = "불량발생",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,


                            partnerId = 0,
                            partnerName ="-",
                            partnerCode ="-",

                            docuNo = "",
                            count = x.DefectiveCount,
                            productTaxInfo = x.Product.TaxType,
                            unitPrice = x.Product.SellPrice,

                            productLOT = x.Lot.LotName,
                            //priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,
                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.DefectiveCount

                        }).ToListAsync();

                    List<LotCountResponse02> resItem = new List<LotCountResponse02>();


                        resItem.AddRange(_resInItem);
                        resItem.AddRange(_resOutItem);
                        resItem.AddRange(_resConsumeItem);
                        resItem.AddRange(_resProduceItem);
                        resItem.AddRange(_resDefItem);


                    var _resItem = resItem
                        .Where(x => lotRequest.partnerId == 0 ? true : x.partnerId == lotRequest.partnerId)
                        .Where(x => lotRequest.docuNo==null? true : x.docuNo.Contains(lotRequest.docuNo))
                        .Where(x => lotRequest.selectType == "ALL" ? true : (lotRequest.selectType == "I" ? x.type == "입고" : (lotRequest.selectType == "O" ? x.type == "출고" : true)))
                        .OrderByDescending(x => x.registerDate)
                        .ThenBy(x => x.partnerCode);

                    var ResItem = new Response<IEnumerable<LotCountResponse02>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _resItem
                    };
                    return ResItem;
                }
                else
                {
                    var _resInItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Lot.ProcessType == "I")
                        .Where(x => x.StoreOutCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : _db.StoreOutStoreProducts.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().Receiving.ReceivingDate >= Convert.ToDateTime(lotRequest.registerStartDate)
                            )
                        .Where(x => lotRequest.registerEndDate == "" ? true : _db.StoreOutStoreProducts.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().Receiving.ReceivingDate <= Convert.ToDateTime(lotRequest.registerEndDate)
                            )
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))


                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            productId = x.Product.Id,

                            registerDate = _db.StoreOutStoreProducts.Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().Receiving.ReceivingDate.ToString("yyyy-MM-dd"),
                            type = "입고",
                            details = "제품입고",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,
                            partnerId = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.Partner.Id).FirstOrDefault(),
                            partnerName = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.Partner.Name).FirstOrDefault(),
                            partnerCode = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.Partner.Code).FirstOrDefault(),
                            docuNo = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).Select(y => y.Receiving.ReceivingNo).FirstOrDefault(),
                            count = x.StoreOutCount,
                            productTaxInfo = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductTaxInfo,
                            unitPrice = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductBuyPrice,
                            productStandardUnit = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductStandardUnit,
                            productStandardUnitCount = _db.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(sosp => sosp.LotName == x.Lot.LotName).FirstOrDefault().ProductStandardUnitCount,
                            productLOT = x.Lot.LotName,
                            //priorInventory = (x.OutOrderCount - x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            inputQuantity = x.ConsumeCount,
                            defectiveQuantity = x.DefectiveCount,
                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.StoreOutCount

                        }).ToListAsync();


                    var _resOutItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Lot.ProcessType == "O")
                        .Where(x => x.OutOrderCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : _db.OutOrderProductsStocks.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().OutOrderProduct.OutOrder.ShipmentDate >= Convert.ToDateTime(lotRequest.registerStartDate)
                            )
                        .Where(x => lotRequest.registerEndDate == "" ? true : _db.OutOrderProductsStocks.Where(sosp => sosp.LotName == x.Lot.LotName)
                            .FirstOrDefault().OutOrderProduct.OutOrder.ShipmentDate <= Convert.ToDateTime(lotRequest.registerEndDate)
                            )
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))


                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                            type = "출고",
                            details = "제품출고",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,


                            partnerId = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.Id,
                            partnerName = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.Name,
                            partnerCode = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.Code,

                            docuNo = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.ShipmentNo ?? "",

                            count = x.OutOrderCount,

                            productTaxInfo = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.OutOrder.Partner.TaxInfo,
                            unitPrice = _db.OutOrderProductsStocks.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().OutOrderProduct.ProductSellPrice,


                            productLOT = x.Lot.LotName,
                           // priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,
                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.OutOrderCount

                        }).ToListAsync();


                    var _resConsumeItem = await _db.LotCounts
                         .Where(x => x.IsDeleted == 0)
                         .Where(x => x.Lot.ProcessType == "C")
                         .Where(x => x.ConsumeCount > 0)
                         .Where(x => lotRequest.registerStartDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(lotRequest.registerStartDate))
                         .Where(x => lotRequest.registerEndDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(lotRequest.registerEndDate))
                         .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))
                         .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                         .Where(x => x.Product != null)
                         .Select(x => new LotCountResponse02
                         {
                             lotCountId = x.LotCountId,
                             registerDate = x.Lot.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd"),

                             type = "투입",
                             details = "공정투입",

                             productCode = x.Product.Code,
                             productClassification = x.Product.CommonCode.Name,
                             productName = x.Product.Name,
                             productStandard = x.Product.Standard,
                             productUnit = x.Product.Unit,

                             partnerId = 0,
                             partnerName = "",
                             partnerCode = "",

                             docuNo = x.Lot.ProcessProgress != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo : "-",
                             count = x.ConsumeCount,
                             productTaxInfo = x.Product.TaxType,
                             unitPrice = x.Product.SellPrice,
                             productLOT = x.Lot.LotName,
                            // priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                             receivingQuantity = x.StoreOutCount,
                             shipmentQuantity = x.OutOrderCount,
                             modifyQuantity = x.ModifyCount,
                             defectiveQuantity = x.DefectiveCount,
                             inputQuantity = x.ConsumeCount,
                             inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                             priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.ConsumeCount

                         }).ToListAsync();

                    var _resProduceItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Lot.ProcessType == "P")
                        .Where(x => x.ProduceCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(lotRequest.registerStartDate))
                        .Where(x => lotRequest.registerEndDate == "" ? true : x.Lot.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(lotRequest.registerEndDate))
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))
                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = x.Lot.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd"),
                            type = "입고",
                            details = "제품생산",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,

                            partnerId = 0,
                            partnerName = "",
                            partnerCode = "",

                            docuNo = x.Lot.ProcessProgress != null ? x.Lot.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo : "-",
                            count = x.ProduceCount,
                            productTaxInfo = x.Product.TaxType,
                            unitPrice = x.Product.SellPrice,
                            productLOT = x.Lot.LotName,
                            //priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,
                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.ProduceCount

                        }).ToListAsync();

                    var _resDefItem = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Lot.ProcessType == "E")
                        .Where(x => x.OutOrderCount > 0)
                        .Where(x => lotRequest.registerStartDate == "" ? true : _db.EtcDefectivesDetails.Where(sosp => sosp.Lot.LotName == x.Lot.LotName)
                            .FirstOrDefault().EtcDefective.DefectiveDate >= Convert.ToDateTime(lotRequest.registerStartDate)
                            )
                        .Where(x => lotRequest.registerEndDate == "" ? true : _db.EtcDefectivesDetails.Where(sosp => sosp.Lot.LotName == x.Lot.LotName)
                            .FirstOrDefault().EtcDefective.DefectiveDate <= Convert.ToDateTime(lotRequest.registerEndDate)
                            )
                        .Where(x => lotRequest.lotName == "" || lotRequest.lotName == null ? true : x.Lot.LotName.Contains(lotRequest.lotName))

                        .Where(x => lotRequest.productId == 0 ? true : lotRequest.productId == x.Product.Id)
                        .Where(x => x.Product != null)
                        .Select(x => new LotCountResponse02
                        {
                            lotCountId = x.LotCountId,
                            registerDate = _db.EtcDefectivesDetails.Where(y => y.IsDeleted == 0).Where(oops => oops.Lot.LotName == x.Lot.LotName).FirstOrDefault().EtcDefective.DefectiveDate.ToString("yyyy-MM-dd"),
                            type = "출고",
                            details = "불량발생",
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,


                            partnerId = 0,
                            partnerName = "-",
                            partnerCode = "-",

                            docuNo = "",
                            count = x.DefectiveCount,
                            productTaxInfo = x.Product.TaxType,
                            unitPrice = x.Product.SellPrice,

                            productLOT = x.Lot.LotName,
                           // priorInventory = (x.StoreOutCount + x.OutOrderCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                            receivingQuantity = x.StoreOutCount,
                            shipmentQuantity = x.OutOrderCount,
                            modifyQuantity = x.ModifyCount,
                            defectiveQuantity = x.DefectiveCount,
                            inputQuantity = x.ConsumeCount,
                            inventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum(),
                            priorInventory = _db.Inventory.Where(y => y.LotName == x.Lot.LotName).Select(y => y.Total).Sum() - x.DefectiveCount

                        }).ToListAsync();

                    List<LotCountResponse02> resItem = new List<LotCountResponse02>();


                    resItem.AddRange(_resInItem);
                    resItem.AddRange(_resOutItem);
                    resItem.AddRange(_resConsumeItem);
                    resItem.AddRange(_resProduceItem);
                    resItem.AddRange(_resDefItem);


                    var _resItem = resItem
                        .Where(x => lotRequest.partnerId == 0 ? true : x.partnerId == lotRequest.partnerId)
                        .Where(x => lotRequest.docuNo == null ? true : x.docuNo.Contains(lotRequest.docuNo))
                        .Where(x => lotRequest.selectType == "ALL" ? true : (lotRequest.selectType == "I" ? x.type == "입고" : (lotRequest.selectType == "O" ? x.type == "출고" : true)))
                        .OrderByDescending(x => x.registerDate)
                        .ThenBy(x => x.partnerCode);

                    var ResItem = new Response<IEnumerable<LotCountResponse02>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _resItem
                    };
                    return ResItem;
                }


                    var Res1 = new Response<IEnumerable<LotCountResponse02>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = null
                    };
                    return Res1;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<LotCountResponse02>>()
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
