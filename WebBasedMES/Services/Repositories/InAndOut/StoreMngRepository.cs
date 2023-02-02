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

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public class StoreMngRepository : IStoreMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public StoreMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        public async Task<Response<int>> CreateStore(StoreRequestCrud storeRequest)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(storeRequest.registerId);
                var _partner = await _db.Partners.Where(y => y.Id == storeRequest.partnerId).FirstOrDefaultAsync();
                DateTime ReceivingDate = DateTime.ParseExact(storeRequest.receivingDate, "yyyy-MM-dd", null);
                var lastStore = await _db.Stores.Where(x=>x.ReceivingDate == ReceivingDate).OrderByDescending(x => x.ReceivingNo).FirstOrDefaultAsync();

                var _uploadFile = await _db.UploadFiles.Where(y => y.Id == storeRequest.uploadFileId).FirstOrDefaultAsync();
               
                string formatNo = "WN" + ReceivingDate.ToString("yyyyMMdd");
                var _storeNumberFormat = string.Format(formatNo + "{0:0000#}", (lastStore == null? 1 : Convert.ToInt32(lastStore.ReceivingNo.Substring(lastStore.ReceivingNo.Length - 5))+1));
                
                var _store = new Store()
                {
                    Partner = _partner, //거래처
                    UploadFiles = storeRequest.UploadFiles, //업로드파일
                    ReceivingMemo = storeRequest.receivingMemo, //비고
                    Register = _user, //등록자
                    ReceivingNo = _storeNumberFormat, //입고번호
                    ReceivingDate = ReceivingDate, //입고일
                    IsDeleted = storeRequest.isDeleted,
                };

                var result = await _db.Stores.AddAsync(_store);
                await Save();

                //var _currentStore = _db.Stores.Where(x => x.ReceivingNo == _storeNumberFormat).Select(x => x.ReceivingId).FirstOrDefault();

                var Res = new Response<int>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result.Entity.ReceivingId,
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

        public async Task<Response<bool>> UpdateStore(StoreRequestCrud storeRequest)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(storeRequest.registerId);
                var _partner = await _db.Partners.Where(y => y.Id == storeRequest.partnerId).FirstOrDefaultAsync();
                DateTime ReceivingDate = DateTime.ParseExact(storeRequest.receivingDate, "yyyy-MM-dd", null);
                var _uploadFile = await _db.UploadFiles.Where(y => y.Id == storeRequest.uploadFileId).FirstOrDefaultAsync();

                var _store = await _db.Stores.Include(x=>x.UploadFiles).Where(x => x.ReceivingId == storeRequest.receivingId).FirstOrDefaultAsync();

                //  ReceivingId = storeRequest.receivingId, //발주
                _store.Partner = _partner; //거래처

                if (_store.UploadFiles != null)
                {
                    _db.UploadFiles.RemoveRange(_store.UploadFiles);
                }

                _store.UploadFiles = storeRequest.UploadFiles;
                _store.ReceivingMemo = storeRequest.receivingMemo; //비고
                _store.Register = _user; //등록자
                _store.ReceivingDate = ReceivingDate; //입고일
                   // IsDeleted = storeRequest.isDeleted,


                _db.Stores.Update(_store);

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

        public async Task<Response<bool>> DeleteStore(StoreRequestCrud storeRequest)
        {
            try
            {

                List<int> ids = new List<int>();


                foreach (int item in storeRequest.receivingIdArray)
                {
                    //var subItem = _db.StoreOutStoreProducts.Where(x => x.Receiving.ReceivingId == item).ToArrayAsync();
                    //_db.StoreOutStoreProducts.RemoveRange(subItem.Result);
                    //var _stores = await _db.Stores.FindAsync(item);
                    //_db.Stores.Remove(_stores);

                    var subItem = await _db.StoreOutStoreProducts
                        .Include(x=>x.Lot).ThenInclude(x=>x.LotCounts)
                        .Include(x=>x.StoreOutStoreProductDefectives).ThenInclude(x=>x.Lot).ThenInclude(x=>x.LotCounts)
                        .Include(x=>x.storeOutStoreProductInspections)
                        .Include(x=>x.OutStoreProduct)
                        .Where(x => x.Receiving.ReceivingId == item).ToListAsync();

                    foreach (StoreOutStoreProduct sub in subItem)
                    {
                        ids.Add(sub.OutStoreProduct.OutStoreProductId);

                        sub.IsDeleted = 1;
                        sub.Lot.IsDeleted = 1;
                        sub.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;

                        foreach(var def in sub.StoreOutStoreProductDefectives)
                        {
                            def.IsDeleted = 1;
                            def.Lot.IsDeleted = 1;
                            def.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;
                        }

                        foreach(var insp in sub.storeOutStoreProductInspections)
                        {
                            insp.IsDeleted = 1;
                        }
                        
                    }
                    _db.StoreOutStoreProducts.UpdateRange(subItem);
                    var mst = await _db.Stores.FindAsync(item);
                    mst.IsDeleted = 1;
                    _db.Stores.Update(mst);

                }

                await Save();

                foreach (var i in ids)
                {
                    var _outStorePrd = _db.OutStoreProducts.Include(x => x.StoreOutStoreProducts).Where(x => x.OutStoreProductId == i).FirstOrDefault();
                    var _sum = _db.StoreOutStoreProducts.Where(x => x.IsDeleted == 0).Where(x => x.OutStoreProduct.OutStoreProductId == i).Select(x => x.ProductReceivingCount).Sum();
                    _outStorePrd.OutStoreStatus = _sum == 0 ? "입고대기" : _sum >= _outStorePrd.ProductOutStoreCount ? "입고완료" : "입고중";
                    _db.OutStoreProducts.Update(_outStorePrd);

                    await Save();
                }

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

        public async Task<Response<bool>> CreateStoreOutStoreProduct(StoreOutStoreProductRequestCrud storeOutStoreProductRequest)
        {
            try
            {

                var _outStoreProduct = await _db.OutStoreProducts.Where(x => x.OutStoreProductId == storeOutStoreProductRequest.outStoreProductId).FirstOrDefaultAsync();
                var _store = await _db.Stores.Where(x => x.ReceivingId == storeOutStoreProductRequest.receivingId).FirstOrDefaultAsync();

                var _storeOutStoreProduct = new StoreOutStoreProduct()
                {
                    OutStoreProduct = _outStoreProduct,
                    Receiving = _store,
                    ProductReceivingCount = storeOutStoreProductRequest.productReceivingCount,
                    ProductBuyPrice = storeOutStoreProductRequest.productBuyPrice,
                    ProductSupplyPrice = _outStoreProduct.ProductSupplyPrice,
                    ProductTaxPrice = _outStoreProduct.ProductTaxPrice,
                    ProductTotalPrice = _outStoreProduct.ProductTotalPrice,
                    ProductImportCheckResult = storeOutStoreProductRequest.productImportCheck == true? 1:0,
                    ReceivingProductMemo = storeOutStoreProductRequest.receivingProductMemo,
                    LotName = storeOutStoreProductRequest.productLOT,
                    IsDeleted = storeOutStoreProductRequest.isDeleted,
                };
                await _db.StoreOutStoreProducts.AddAsync(_storeOutStoreProduct);

                // Lot insert
                var _lot = new LotEntity();
                {
                    _lot.LotName = storeOutStoreProductRequest.productLOT;
                    _lot.ProcessType = "I";

                };
                var result = await _db.Lots.AddAsync(_lot);
                var _Item = await _db.OutStoreProducts.Include(x => x.Product).Where(x => x.OutStoreProductId == storeOutStoreProductRequest.outStoreProductId).FirstOrDefaultAsync();

                // LotCount insert
                var _lotCount = new LotCount();
                {
                    _lotCount.Product = _Item.Product;
                    _lotCount.Lot = result.Entity;
                    _lotCount.StoreOutCount = storeOutStoreProductRequest.productReceivingCount;
                };

                await _db.LotCounts.AddAsync(_lotCount);

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

        public async Task<Response<bool>> UpdateStoreOutStoreProduct(StoreOutStoreProductRequestCrud storeOutStoreProductRequest)
        {
            //사용??
            try
            {

                var _outStoreProduct = await _db.OutStoreProducts.Where(x => x.OutStoreProductId == storeOutStoreProductRequest.outStoreProductId).FirstOrDefaultAsync();
                var _store = await _db.Stores.Where(x => x.ReceivingId == storeOutStoreProductRequest.receivingId).FirstOrDefaultAsync();
                var _storeOutStoreProduct = new StoreOutStoreProduct()
                {
                    StoreOutStoreProductId = storeOutStoreProductRequest.storeOutStoreProductId,
                    OutStoreProduct = _outStoreProduct,
                    Receiving = _store,
                    ProductReceivingCount = storeOutStoreProductRequest.productReceivingCount,
                    ProductBuyPrice = storeOutStoreProductRequest.productBuyPrice,
                    ProductSupplyPrice = _outStoreProduct.ProductSupplyPrice,
                    ProductTaxPrice = _outStoreProduct.ProductTaxPrice,
                    ProductTotalPrice = _outStoreProduct.ProductTotalPrice,
                    ProductImportCheckResult = storeOutStoreProductRequest.productImportCheck == true? 1:0,
                    ReceivingProductMemo = storeOutStoreProductRequest.receivingProductMemo,
                    LotName = storeOutStoreProductRequest.productLOT,
                    IsDeleted = storeOutStoreProductRequest.isDeleted,
                };
                _db.StoreOutStoreProducts.Update(_storeOutStoreProduct);

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

        public async Task<Response<bool>> DeleteStoreOutStoreProduct(StoreOutStoreProductRequestCrud storeOutStoreProductRequest)
        {
            try
            {

                foreach (int item in storeOutStoreProductRequest.storeOutStoreProductIdArray)
                {
                    var _items = await _db.StoreOutStoreProducts.Include(x=>x.Lot).Where(x=>x.StoreOutStoreProductId == item).FirstOrDefaultAsync();
                    _items.IsDeleted = 1;
                    _db.StoreOutStoreProducts.UpdateRange(_items);

                    var _lots = await _db.Lots.Where(x => x.LotId == _items.Lot.LotId).FirstOrDefaultAsync();
                    _lots.IsDeleted = 1;

                    _db.Lots.Update(_lots);

                    var _lotCnt = _db.LotCounts.Where(x => x.Lot.LotId == _items.Lot.LotId).FirstOrDefault();
                    _lotCnt.IsDeleted = 1; ;
                    _db.LotCounts.Update(_lotCnt);
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



        public async Task<Response<bool>> StoreOutStoreProductEditSave(StoreOutStoreProductRequestCrud storeOutStoreProductRequest)
        {
            try
            {
                //var _user = await _userManager.FindByIdAsync(storeOutStoreProductRequest.store.registerId);
                var _store = await _db.Stores.Where(x => x.ReceivingId == storeOutStoreProductRequest.receivingId).FirstOrDefaultAsync();

               // _store.Register = _user;
               // _store.ReceivingDate = Convert.ToDateTime(storeOutStoreProductRequest.store.receivingDate);
               // _store.ReceivingMemo = storeOutStoreProductRequest.store.receivingMemo;

                _db.Stores.Update(_store);
                await Save();


                var storeOutStoreProducts = await _db.StoreOutStoreProducts.Where(x => x.Receiving.ReceivingId == storeOutStoreProductRequest.receivingId).ToListAsync();
                var regDate = _store.ReceivingDate.ToString("yyMMdd");
                var regDate_lot = await _db.Lots
                       .Where(x => x.IsDeleted == 0)
                       .OrderBy(x => x.LotName.Substring(2, 10))
                       .Where(x => x.LotName.Substring(2, 6) == regDate)
                       .Select(x => x.LotName.Substring(2, 10))
                       .LastOrDefaultAsync();

                var lotNameHead = "L0";
                var lotNameHeadSeq = "";
                var lotNameFull = "";
                var lotSeq = "0000";

                if (regDate_lot != null)
                {
                    lotSeq = regDate_lot.Substring(6, 4);
                }


                var orgPrds = _db.StoreOutStoreProducts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Receiving == _store)
                        .Select(x => x.StoreOutStoreProductId)
                        .ToList();
                List<int> ids = new List<int>();



                foreach (StoreOutStoreProductRequestCrud req in storeOutStoreProductRequest.storeOutStoreProductArray)
                {
                    var _Item = await _db.OutStoreProducts.Include(x=>x.Product).Where(x => x.OutStoreProductId == req.outStoreProductId).FirstOrDefaultAsync();
                    ids.Add(req.outStoreProductId);

                    if (req.storeOutStoreProductId == 0)
                    {
                        lotSeq = string.Format("{0:000#}", Convert.ToInt64(lotSeq) + 1);
                        lotNameHeadSeq = string.Format(lotNameHead + regDate + "{0:000#}", Convert.ToInt64(lotSeq));
                        lotNameFull = string.Format(lotNameHeadSeq + "{0:000#}", (_Item.Product.Id));

                        var _lot = new LotEntity();
                        {
                            _lot.LotName = lotNameFull;
                            _lot.ProcessType = "I";

                        };
                        var result = await _db.Lots.AddAsync(_lot);

                        var _lotCount = new LotCount()
                        {
                            Product = _Item.Product,
                            Lot = result.Entity,
                            StoreOutCount = req.productReceivingCount * req.productStandardUnitCount,
                        };
                        await _db.LotCounts.AddAsync(_lotCount);

                        var _storeOutStoreProduct = new StoreOutStoreProduct()
                        {
                            Receiving = _store,
                            OutStoreProduct = _Item,
                            ProductReceivingCount = req.productReceivingCount,
                            ProductBuyPrice = req.productBuyPrice,
                            ProductSupplyPrice = _Item.ProductSupplyPrice,
                            ProductTaxPrice = _Item.ProductTaxPrice,
                            ProductTotalPrice = _Item.ProductTotalPrice,
                           // ProductImportCheckResult = req.productImportCheckResult == "미검사" ? 3 : req.productImportCheckResult == "합격" ? 0 : req.productImportCheckResult == "불합격" ? 2 : 1,
                            ReceivingProductMemo = req.receivingProductMemo,
                            ProductTaxInfo = req.productTaxInfo,
                            ProductStandardUnit = req.productStandardUnit,
                            ProductStandardUnitCount = req.productStandardUnitCount,

                            LotName = lotNameFull,
                            Lot = result.Entity
                        };
                        var saveResult = await _db.StoreOutStoreProducts.AddAsync(_storeOutStoreProduct);
                        if(req.storeOutStoreProductDefectives != null)
                        {
                            foreach (var def in req.storeOutStoreProductDefectives)
                            {
                                var _defLot = _db.Lots.Add(new LotEntity
                                {
                                    LotName = lotNameFull,
                                    ProcessType = "I",
                                });

                                _db.LotCounts.Add(new LotCount
                                {
                                    Lot = _defLot.Entity,
                                    Product = _Item.Product,
                                    DefectiveCount = def.defectiveQuantity
                                });

                                _db.StoreOutStoreProductDefectives.Add(new StoreOutStoreProductDefective
                                {
                                    Defective = _db.Defectives.Where(x => x.Id == def.defectiveId).FirstOrDefault(),
                                    DefectiveQuantity = def.defectiveQuantity,
                                    DefectiveUnit = def.defectiveUnit,
                                    DefectiveMemo = def.defectiveMemo,
                                    Lot = _defLot.Entity,
                                    StoreOutStoreProduct = saveResult.Entity,
                                });
                            }
                        }

                        if(req.storeOutStoreProductInspections != null)
                        {
                            foreach (var insp in req.storeOutStoreProductInspections)
                            {
                                _db.StoreOutStoreProductInspectionTypes.Add(new StoreOutStoreProductInspectionType
                                {
                                    InspectionResult = insp.inspectionResult == "합격" ? 0 : insp.inspectionResult == "부분합격" ? 1 : insp.inspectionResult == "불합격" ? 2 : 3,
                                    InspectionResultText = insp.inspectionResultText,
                                    InspectionType = _db.InspectionTypes.Where(x => x.Id == insp.InspectionTypeId).FirstOrDefault(),
                                    StoreOutStoreProduct = saveResult.Entity,
                                });
                            }
                        }

                    }
                    else
                    {
                        var _storeOutStoreProduct = _db.StoreOutStoreProducts.Include(x=>x.Lot).Where(x => x.StoreOutStoreProductId == req.storeOutStoreProductId).FirstOrDefault();

                        _storeOutStoreProduct.ProductReceivingCount = req.productReceivingCount;
                        _storeOutStoreProduct.ProductBuyPrice = req.productBuyPrice;
                        _storeOutStoreProduct.ProductSupplyPrice = _Item.ProductSupplyPrice;
                        _storeOutStoreProduct.ProductTaxPrice = _Item.ProductTaxPrice;
                        _storeOutStoreProduct.ProductTotalPrice = _Item.ProductTotalPrice;
                        _storeOutStoreProduct.ProductImportCheckResult = req.productImportCheckResult == "미검사" ? 3 : req.productImportCheckResult == "합격" ? 0 : req.productImportCheckResult == "불합격" ? 2 : 1;
                        _storeOutStoreProduct.ReceivingProductMemo = req.receivingProductMemo;
                        _storeOutStoreProduct.ProductTaxInfo = req.productTaxInfo;
                        _storeOutStoreProduct.ProductStandardUnit = req.productStandardUnit;
                        _storeOutStoreProduct.ProductStandardUnitCount = req.productStandardUnitCount;

                        _db.StoreOutStoreProducts.Update(_storeOutStoreProduct);

                        
                        var _lotCnt = _db.LotCounts.Where(x => x.LotId == _storeOutStoreProduct.Lot.LotId).FirstOrDefault();
                        if(_lotCnt != null)
                        {
                            _lotCnt.StoreOutCount = req.productReceivingCount * req.productStandardUnitCount;
                            _db.LotCounts.Update(_lotCnt);
                        }

                        if (req.storeOutStoreProductInspections != null)
                        {
                            foreach (var insp in req.storeOutStoreProductInspections)
                            {
                                var _insp = _db.StoreOutStoreProductInspectionTypes.Where(x => x.StoreOutStoreProductInspectionId == insp.storeOutStoreProductInspectionId).FirstOrDefault();

                                _insp.InspectionResult = insp.inspectionResult == "합격" ? 0 : insp.inspectionResult == "부분합격" ? 1 : insp.inspectionResult == "불합격" ? 2 : 3;
                                _insp.InspectionResultText = insp.inspectionResultText;
                                _db.StoreOutStoreProductInspectionTypes.Update(_insp);
                            }
                        }

                        if (req.storeOutStoreProductDefectives!= null)
                        {
                            var origDefList = _db.StoreOutStoreProductDefectives
                                .Where(x => x.IsDeleted == 0)
                                .Where(x => x.StoreOutStoreProduct == _storeOutStoreProduct)
                                .Select(x => x.StoreOutStoreProductDefectiveId).ToList();

                            bool flag1 = false;

                            List<int> newDeflist = new List<int>();

                            foreach (var def in req.storeOutStoreProductDefectives)
                            {
                                if (def.storeOutStoreProductDefectiveId == 0)
                                {
                                    var _defLot = _db.Lots.Add(new LotEntity
                                    {
                                        LotName = lotNameFull,
                                        ProcessType = "I",
                                    });

                                    _db.LotCounts.Add(new LotCount
                                    {
                                        Lot = _defLot.Entity,
                                        Product = _Item.Product,
                                        DefectiveCount = def.defectiveQuantity
                                    });

                                    _db.StoreOutStoreProductDefectives.Add(new StoreOutStoreProductDefective
                                    {
                                        Defective = _db.Defectives.Where(x => x.Id == def.defectiveId).FirstOrDefault(),
                                        DefectiveQuantity = def.defectiveQuantity,
                                        DefectiveUnit = def.defectiveUnit,
                                        DefectiveMemo = def.defectiveMemo,

                                        Lot = _defLot.Entity,
                                        StoreOutStoreProduct = _storeOutStoreProduct,
                                    });
                                }
                                else
                                {
                                    var _defUpdate = _db.StoreOutStoreProductDefectives
                                        .Include(x => x.Lot)
                                        .ThenInclude(x => x.LotCounts)
                                        .Where(x => x.StoreOutStoreProductDefectiveId == def.storeOutStoreProductDefectiveId)
                                        .FirstOrDefault();

                                    _defUpdate.DefectiveUnit = def.defectiveUnit;
                                    _defUpdate.DefectiveQuantity = def.defectiveQuantity;
                                    _defUpdate.DefectiveMemo = def.defectiveMemo;
                                    _defUpdate.Lot.LotCounts.FirstOrDefault().DefectiveCount = def.defectiveQuantity;
                                    _db.StoreOutStoreProductDefectives.Update(_defUpdate);

                                    newDeflist.Add(def.storeOutStoreProductDefectiveId);
                                }
                            }


                            foreach (int x in origDefList)
                            {
                                flag1 = false;
                                foreach (int y in newDeflist)
                                {
                                    if (x == y)
                                    {
                                        flag1 = true;
                                    }
                                }

                                if (!flag1)
                                {
                                    var _defDelete = _db.StoreOutStoreProductDefectives
                                        .Include(z => z.Lot)
                                        .ThenInclude(z => z.LotCounts)
                                        .Where(z => z.StoreOutStoreProductDefectiveId == x)
                                        .FirstOrDefault();

                                    _defDelete.IsDeleted = 1;
                                    _defDelete.Lot.IsDeleted = 1;
                                    _defDelete.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;

                                    _db.StoreOutStoreProductDefectives.Update(_defDelete);
                                }
                            }
                        }
                    }
                }

                bool flag = false;

                //제품 삭제...
                foreach (int z in orgPrds)
                {
                    flag = false;
                    foreach (var y in storeOutStoreProductRequest.storeOutStoreProductArray)
                    {
                        if (z == y.storeOutStoreProductId)
                        {
                            flag = true;
                        }
                    }

                    if (!flag)
                    {
                        var _prdDelete = _db.StoreOutStoreProducts
                            .Include(x => x.Lot)
                            .ThenInclude(x => x.LotCounts)
                            .Include(x => x.StoreOutStoreProductDefectives)
                            .ThenInclude(x => x.Lot)
                            .ThenInclude(x => x.LotCounts)
                            .Where(x => x.StoreOutStoreProductId == z)
                            .FirstOrDefault();

                        _prdDelete.IsDeleted = 1;
                        _prdDelete.Lot.IsDeleted = 1;
                        _prdDelete.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;
                        
                        foreach(var i in _prdDelete.StoreOutStoreProductDefectives)
                        {
                            i.IsDeleted = 1;
                            i.Lot.IsDeleted = 1;
                            i.Lot.LotCounts.FirstOrDefault().IsDeleted = 1;
                        }

                        _db.StoreOutStoreProducts.Update(_prdDelete);
                    }
                }

                await Save();




                if(ids.Count > 0)
                {
                    foreach (var i in ids)
                    {
                        var _outStorePrd = _db.OutStoreProducts.Include(x=>x.StoreOutStoreProducts).Where(x => x.OutStoreProductId == i).FirstOrDefault();
                        var _sum = _db.StoreOutStoreProducts.Where(x=>x.IsDeleted == 0).Where(x => x.OutStoreProduct.OutStoreProductId == i).Select(x => x.ProductReceivingCount).Sum();
                        _outStorePrd.OutStoreStatus = _sum == 0 ? "입고대기" : _sum >= _outStorePrd.ProductOutStoreCount ? "입고완료" : "입고중";
                        _db.OutStoreProducts.Update(_outStorePrd);

                        await Save();
                    }

                }



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

        public async Task<Response<IEnumerable<StoreRes001>>> storeMstList(StoreReq001 storeRequest)
        {
            try
            {
                if(storeRequest.invenType == "ITEM")
                {
                    var _storeItem = await _db.Stores

                        .Include(x => x.StoreOutStoreProducts).ThenInclude(x => x.OutStoreProduct).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                        .Include(x=>x.UploadFiles)
                        .Where(x => x.StoreOutStoreProducts.Where(y=>y.IsDeleted ==0).Where(z=>z.OutStoreProduct.Product.Processes.Count()==0).Count()>0)
                        .Where(x => x.ReceivingNo.Contains(storeRequest.receivingNo))

                        .Where(x => storeRequest.receivingStartDate == "" ? x.ReceivingDate >= DateTime.Today : x.ReceivingDate >= Convert.ToDateTime(storeRequest.receivingStartDate))
                        .Where(x => storeRequest.receivingEndDate == "" ? x.ReceivingDate <= DateTime.Today.AddMonths(1) : x.ReceivingDate <= Convert.ToDateTime(storeRequest.receivingEndDate))
                        .Where(x => storeRequest.partnerId == 0 ? true : x.Partner.Id == storeRequest.partnerId)
                        .Where(x => x.IsDeleted == 0)
                        .OrderByDescending(x => x.ReceivingNo)
                        .Select(x => new StoreRes001
                        {

                            receivingId = x.ReceivingId,
                            receivingNo = x.ReceivingNo,
                            receivingDate = x.ReceivingDate.ToString("yyyy-MM-dd"),
                            partnerName = x.Partner.Name, // 거래처이름
                            partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                          
                            receivingProductCount = x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Count(),
                            receivingSupplyPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),
                            receivingTaxPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount / 10) : 0).Sum(),
                            receivingTotalPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount * 11/ 10) : SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),
                           
                            registerName = x.Register.FullName,//등록자
                            receivingMemo = x.ReceivingMemo,
                            uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                            uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                            UploadFiles = x.UploadFiles.ToList(),

                            productIds = x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Select(y => y.OutStoreProduct.Product.Id).ToArray(),


                        }).ToListAsync();

                    var filteredStoreItem = _storeItem
                        .Where(x => storeRequest.productId == 0 ? true : x.productIds.Contains(storeRequest.productId))

                        .ToList();

                    var ResItem = new Response<IEnumerable<StoreRes001>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _storeItem
                    };

                    return ResItem;
                }
                if(storeRequest.invenType == "PRD")
                {
                    var _storePrd = await _db.Stores

                        .Include(x => x.StoreOutStoreProducts).ThenInclude(x => x.OutStoreProduct).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                        .Include(x => x.UploadFiles)

                        .Where(x => x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(z => z.OutStoreProduct.Product.Processes.Count() > 0).Count() == x.StoreOutStoreProducts.Where(x => x.IsDeleted == 0).Count())
                        .Where(x => storeRequest.receivingNo == "" ? true : x.ReceivingNo.Contains(storeRequest.receivingNo))

                        .Where(x => storeRequest.receivingStartDate == "" ? x.ReceivingDate >= DateTime.Today : x.ReceivingDate >= Convert.ToDateTime(storeRequest.receivingStartDate))
                        .Where(x => storeRequest.receivingEndDate == "" ? x.ReceivingDate <= DateTime.Today.AddMonths(1) : x.ReceivingDate <= Convert.ToDateTime(storeRequest.receivingEndDate))
                        .Where(x => storeRequest.partnerId == 0 ? true : x.Partner.Id == storeRequest.partnerId)

                        .Where(x => storeRequest.productId == 0 ? true :

                            x.StoreOutStoreProducts.Where(x => x.OutStoreProduct.Product.Id == storeRequest.productId)
                            .Select(x => x.OutStoreProduct.Product.Id).ToArray()
                            .Contains(storeRequest.productId)
                        )
                        .Where(x => x.IsDeleted == 0)
                        .OrderByDescending(x => x.ReceivingNo)
                        .Select(x => new StoreRes001
                        {

                            receivingId = x.ReceivingId,
                            receivingNo = x.ReceivingNo,
                            receivingDate = x.ReceivingDate.ToString("yyyy-MM-dd"),
                            partnerName = x.Partner.Name, // 거래처이름
                            partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                            receivingProductCount = x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Select(SSP => SSP).Count(),

                            receivingSupplyPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),
                            receivingTaxPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount / 10) : 0).Sum(),
                            receivingTotalPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount * 11 / 10) : SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),

                            registerName = x.Register.FullName,//등록자
                            receivingMemo = x.ReceivingMemo,
                            uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                            uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                            UploadFiles = x.UploadFiles.ToList(),


                        }).ToListAsync();

                    var ResPrd = new Response<IEnumerable<StoreRes001>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = _storePrd
                    };

                    return ResPrd;
                }

                var _store = await _db.Stores

                    .Include(x => x.StoreOutStoreProducts).ThenInclude(x => x.OutStoreProduct).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                    .Include(x => x.UploadFiles)

                    .Where(x => storeRequest.receivingNo == "" ? true : x.ReceivingNo.Contains(storeRequest.receivingNo))
                    .Where(x => storeRequest.receivingStartDate == "" ? x.ReceivingDate >= DateTime.Today : x.ReceivingDate >= Convert.ToDateTime(storeRequest.receivingStartDate))
                    .Where(x => storeRequest.receivingEndDate == "" ? x.ReceivingDate <= DateTime.Today.AddMonths(1) : x.ReceivingDate <= Convert.ToDateTime(storeRequest.receivingEndDate))
                    .Where(x => storeRequest.partnerId == 0 ? true : x.Partner.Id == storeRequest.partnerId)

                    .Where(x => storeRequest.productId == 0 ? true :

                        x.StoreOutStoreProducts.Where(x => x.OutStoreProduct.Product.Id == storeRequest.productId)
                        .Select(x => x.OutStoreProduct.Product.Id).ToArray()
                        .Contains(storeRequest.productId)
                    )
                    .Where(x => x.IsDeleted == 0)
                    .OrderByDescending(x => x.ReceivingNo)
                    .Select(x => new StoreRes001
                    {
                        
                        receivingId = x.ReceivingId,
                        receivingNo = x.ReceivingNo,
                        receivingDate = x.ReceivingDate.ToString("yyyy-MM-dd"),
                        partnerName = x.Partner.Name, // 거래처이름
                        partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                        receivingProductCount = x.StoreOutStoreProducts.Where(y=>y.IsDeleted == 0).Select(SSP => SSP).Count(),
                        receivingSupplyPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),
                        receivingTaxPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount / 10) : 0).Sum(),
                        receivingTotalPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount * 11 / 10) : SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),

                        registerName = x.Register.FullName,//등록자
                        receivingMemo = x.ReceivingMemo,
                        uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                        UploadFiles = x.UploadFiles.ToList(),


                    }).ToListAsync();

                var Res = new Response<IEnumerable<StoreRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _store
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes002>>> storeOutStoreProductList(StoreReq001 storeRequest)
        {
            try
            {
                
                var result = await _db.StoreOutStoreProducts
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                    .Where(x => x.Receiving.ReceivingId == storeRequest.receivingId)
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x=>x.OutStoreProduct.OutStore.OutStoreNo).ThenBy(x=>x.OutStoreProduct.Product.Code)
                    .Select(x => new StoreRes002
                    {

                        storeOutStoreProductId = x.StoreOutStoreProductId,
                        receivingId = x.Receiving.ReceivingId,
                        outStoreProductId = x.OutStoreProduct.OutStoreProductId,
                        productId = x.OutStoreProduct.Product.Id,
                        outStoreNo = x.OutStoreProduct.OutStore.OutStoreNo,
                        productCode = x.OutStoreProduct.Product.Code,
                        productClassification = x.OutStoreProduct.Product.CommonCode != null ? x.OutStoreProduct.Product.CommonCode.Name : "",
                        productName = x.OutStoreProduct.Product.Name,
                        productStandard = x.OutStoreProduct.Product.Standard,
                        productStandardUnit =  x.ProductStandardUnit,
                        productStandardUnitCount = x.ProductStandardUnitCount,
                        productTaxInfo = x.ProductTaxInfo,
                        productReceivingCount = x.ProductReceivingCount,
                        productBuyPrice = x.ProductBuyPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        productTotalPrice = x.ProductTotalPrice,
                        productLOT = x.LotName,
                        productDefectiveQuantity = x.StoreOutStoreProductDefectives.Where(SPD => SPD.IsDeleted == 0).Select(SPD => SPD.DefectiveQuantity).Sum(),
                        productGoodQuantity = x.ProductReceivingCount - x.StoreOutStoreProductDefectives.Where(SPD => SPD.IsDeleted == 0).Select(SPD => SPD.DefectiveQuantity).Sum(), 

                        //productDefectiveQuantity = 0, //todo
                        
                        productImportCheckResult = x.OutStoreProduct.Product.ImportCheck == false? "해당없음":  
                            x.storeOutStoreProductInspections.Where(y=>y.InspectionResult == 0).Count() == x.storeOutStoreProductInspections.Count() ? "합격" : 
                                x.storeOutStoreProductInspections.Where(y => y.InspectionResult == 2).Count() == x.storeOutStoreProductInspections.Count() ? "불합격":
                                    x.storeOutStoreProductInspections.Where(y=>y.InspectionResult ==3).Count()>1? "미검사" : "부분합격",

                        receivingProductMemo = x.ReceivingProductMemo,

                        ModelName = x.OutStoreProduct.Product.Model != null ? x.OutStoreProduct.Product.Model.Name : "",
                        PartnerName = x.OutStoreProduct.Product.Partner != null ? x.OutStoreProduct.Product.Partner.Name : "",

                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<StoreRes002>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<StoreRes003>> storeMstPop(StoreReq001 storeRequest)
        {
            try
            {
                var _store = await _db.Stores

                    .Include(x => x.StoreOutStoreProducts).ThenInclude(x => x.OutStoreProduct).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                    .Include(x => x.Partner)
                    .Where(x => x.ReceivingId == storeRequest.receivingId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new StoreRes003
                    {
                        receivingId = x.ReceivingId,
                        receivingDate = x.ReceivingDate.ToString("yyyy-MM-dd"),
                        partnerId = x.Partner.Id,
                        partnerCode = x.Partner.Code,
                        partnerName = x.Partner.Name,
                        contactName = x.Partner.ContactName,
                        telephoneNumber = x.Partner.TelephoneNumber,
                        faxNumber = x.Partner.FaxNumber,
                        contactEmail = x.Partner.ContactEmail,
                        partnerTaxInfo = x.Partner.TaxInfo,
                        registerId = x.Register.Id,
                        registerName = x.Register.FullName,
                        receivingMemo = x.ReceivingMemo,
                        uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                        UploadFiles = x.UploadFiles.ToList()

                    }).FirstOrDefaultAsync();

                var Res = new Response<StoreRes003>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _store
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<StoreRes003>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes004>>> storeOutStoreProductListPop(StoreReq001 storeRequest)
        {
            try
            {



                var result = await _db.StoreOutStoreProducts
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                    .Where(x => x.Receiving.ReceivingId == storeRequest.receivingId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new StoreRes004
                    {

                        storeOutStoreProductId = x.StoreOutStoreProductId,
                        receivingId = x.Receiving.ReceivingId,
                        outStoreProductId = x.OutStoreProduct.OutStoreProductId,
                        productId = x.OutStoreProduct.Product.Id,
                        outStoreNo = x.OutStoreProduct.OutStore.OutStoreNo,
                        productCode = x.OutStoreProduct.Product.Code,
                        productClassification = x.OutStoreProduct.Product.CommonCode != null ? x.OutStoreProduct.Product.CommonCode.Name : "",
                        productName = x.OutStoreProduct.Product.Name,
                        productStandard = x.OutStoreProduct.Product.Standard,
                        productUnit = x.OutStoreProduct.Product.Unit,
                        productStandardUnit = x.ProductStandardUnit,
                        productStandardUnitCount = x.ProductStandardUnitCount,
                        productTaxInfo = x.ProductTaxInfo,
                        productOutStoreCount = x.OutStoreProduct.ProductOutStoreCount,
                        productReceivingCount = x.ProductReceivingCount,
                        productBuyPrice = x.ProductBuyPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        productTotalPrice = x.ProductTotalPrice,
                        productLOT = x.LotName,
                        productDefectiveQuantity = x.StoreOutStoreProductDefectives.Where(SPD => SPD.IsDeleted == 0).Select(SPD => SPD.DefectiveQuantity).Sum(),
                        productGoodQuantity = x.ProductReceivingCount - x.StoreOutStoreProductDefectives.Where(SPD => SPD.IsDeleted == 0).Select(SPD => SPD.DefectiveQuantity).Sum(),
                        productImportCheckResult = x.OutStoreProduct.Product.ImportCheck == false ? "해당없음" :
                            x.storeOutStoreProductInspections.Where(y => y.InspectionResult == 0).Count() == x.storeOutStoreProductInspections.Count() ? "합격" :
                                x.storeOutStoreProductInspections.Where(y => y.InspectionResult == 2).Count() == x.storeOutStoreProductInspections.Count() ? "불합격" : "부분합격",


                        receivingProductMemo = x.ReceivingProductMemo,
                        productImportCheck = x.OutStoreProduct.Product.ImportCheck,
                        
                        storeOutStoreProductDefectives = x.StoreOutStoreProductDefectives
                            .Where(y=>y.IsDeleted == 0)
                            .Select(y=> new StoreOutStoreProductDefectiveInterface
                            {
                                defectiveCode = y.Defective.Code,
                                defectiveName = y.Defective.Name,
                                defectiveId = y.Defective.Id,
                                storeOutStoreProductDefectiveId = y.StoreOutStoreProductDefectiveId,
                                defectiveMemo = y.DefectiveMemo,
                                defectiveQuantity = y.DefectiveQuantity,
                                defectiveUnit = y.DefectiveUnit,
                                storeOutStoreProductId = x.StoreOutStoreProductId,
                                Checked = false,
                            }).ToList(),
                        
                        storeOutStoreProductInspections = x.storeOutStoreProductInspections
                        .Select(y=>new StoreOutStoreProductInspectionInterface
                        {
                            inspectionCode = y.InspectionType == null? "" : y.InspectionType.Code,
                            inspectionId = y.InspectionType == null ? 0 : y.InspectionType.Id,
                            InspectionTypeId = y.InspectionType == null ? 0 : y.InspectionType.Id,
                            inspectionMethod = y.InspectionType == null ? "" : y.InspectionType.InspectionMethod,
                            storeOutStoreProductId = x.StoreOutStoreProductId,
                            storeOutStoreProductInspectionId = y.StoreOutStoreProductInspectionId,
                            inspectionName = y.InspectionType == null ? "" : y.InspectionType.Name,
                            inspectionResult = y.InspectionResult == 0? "합격" : y.InspectionResult == 1? "부분합격" : y.InspectionResult == 2 ? "불합격" : "미검사",
                            inspectionResultText = y.InspectionResultText,
                            inspectionStandard = y.InspectionType == null ? "" : y.InspectionType.InspectionStandard
                        }).ToList()
                        


                      //  productImportCheck = x.OutStoreProduct.


                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<StoreRes004>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes004>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes005>>> defectiveList(StoreReq002 storeRequest)
        {
            try
            {

                var result = await _db.Defectives
                    .Where(x => (storeRequest.defectiveIsUsing == "ALL" || storeRequest.defectiveIsUsing == "") ? true : (storeRequest.defectiveIsUsing == "Y" ? x.IsUsing == true : (storeRequest.defectiveIsUsing == "N" ? x.IsUsing == false : false)))
                    .Where(x => storeRequest.searchInput == "" ? true :

                     x.Code.Contains(storeRequest.searchInput) ||
                     x.Name.Contains(storeRequest.searchInput) ||
                     x.Memo.Contains(storeRequest.searchInput)

                    )
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.IsUsing == true)
                    .Select(x => new StoreRes005
                    {
                        defectiveId = x.Id,
                        defectiveCode = x.Code,
                        defectiveMemo = x.Memo,
                        defectiveIsUsing = x.IsUsing,
                        defectiveName = x.Name,

                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<StoreRes005>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes005>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes006>>> inspectionList(StoreReq002 storeRequest)
        {
            try
            {

                var result = await _db.InspectionTypes
                    .Where(x => (storeRequest.inspectionIsUsing == "ALL" || storeRequest.inspectionIsUsing == "") ? true : (storeRequest.inspectionIsUsing == "Y" ? x.IsUsing == true : (storeRequest.inspectionIsUsing == "N" ? x.IsUsing == false : false)))
                    .Where(x => storeRequest.searchInput == "" ? true :

                     x.Code.Contains(storeRequest.searchInput) ||
                     x.Name.Contains(storeRequest.searchInput) ||
                     x.InspectionStandard.Contains(storeRequest.searchInput) ||
                     x.InspectionMethod.Contains(storeRequest.searchInput)

                    )
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new StoreRes006
                    {
                        inspectionTypeId = x.Id,
                        inspectionCode = x.Code,
                        inspectionName = x.Name,
                        inspectionStandard = x.InspectionStandard,
                        inspectionMethod = x.InspectionMethod,

                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<StoreRes006>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes006>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<bool>> CreateStoreOutStoreProductDefective(StoreReq003 storeReq003)
        {
            try
            {
                foreach (StoreReq003 req in storeReq003.defectiveArray)
                {
                    var _storeOutStoreProduct = await _db.StoreOutStoreProducts.Where(x => x.StoreOutStoreProductId == storeReq003.storeoutStoreProductId).FirstOrDefaultAsync();
                    var _defectives = await _db.Defectives.Where(x => x.Id == req.defectiveId).FirstOrDefaultAsync();
                    var _storeOutStoreProductDefective = new StoreOutStoreProductDefective()
                    {
                        StoreOutStoreProduct = _storeOutStoreProduct,
                        Defective = _defectives,
                        DefectiveQuantity = req.defectiveQuantity,
                        DefectiveUnit = req.defectiveUnit
                    };
                    await _db.StoreOutStoreProductDefectives.AddAsync(_storeOutStoreProductDefective);

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
        public async Task<Response<bool>> CreateStoreOutStoreProductInspection(StoreReq004 storeReq004)
        {
            try
            {
                foreach (StoreReq004 req in storeReq004.inspectionArray)
                {
                    var _storeOutStoreProduct = await _db.StoreOutStoreProducts.Where(x => x.StoreOutStoreProductId == storeReq004.storeoutStoreProductId).FirstOrDefaultAsync();
                    var _inspectiontypes = await _db.InspectionTypes.Where(x => x.Id == req.inspectionTypeId).FirstOrDefaultAsync();
                    var _storeOutStoreProductInspectionType = new StoreOutStoreProductInspectionType()
                    {
                        StoreOutStoreProduct = _storeOutStoreProduct,
                        InspectionType = _inspectiontypes,
                        InspectionResult = req.inspectionResult,
                        InspectionResultText = req.inspectionResultText
                    };
                    await _db.StoreOutStoreProductInspectionTypes.AddAsync(_storeOutStoreProductInspectionType);

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

        public async Task<Response<IEnumerable<StoreRes007>>> storeOutStoreProductDefectiveListPop(StoreReq003 storeRequest)
        {
            try
            {
                var result = await _db.StoreOutStoreProductDefectives

                    .Where(x => x.StoreOutStoreProduct.StoreOutStoreProductId == storeRequest.storeoutStoreProductId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new StoreRes007
                    {
                        storeOutStoreProductDefectiveId = x.StoreOutStoreProductDefectiveId,
                        defectiveId = x.Defective.Id,
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,
                        defectiveQuantity = x.DefectiveQuantity,
                        defectiveUnit = x.DefectiveUnit,
                        defectiveMemo = x.Defective.Memo
                    }).ToListAsync();

                var Res = new Response<IEnumerable<StoreRes007>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes007>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes006>>> storeOutStoreProductInspectionTypeListPop(StoreReq004 storeRequest)
        {
            try
            {
                var result = await _db.StoreOutStoreProductInspectionTypes

                    .Where(x => x.StoreOutStoreProduct.StoreOutStoreProductId == storeRequest.storeoutStoreProductId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new StoreRes006
                    {
                        storeOutStoreProductInspectionTypeId = x.StoreOutStoreProductInspectionId,
                        inspectionTypeId = x.InspectionType.Id,
                        inspectionCode = x.InspectionType.Code,
                        inspectionName = x.InspectionType.Name,
                        inspectionStandard = x.InspectionType.InspectionStandard,
                        inspectionMethod = x.InspectionType.InspectionMethod,
                        inspectionResult = x.InspectionResult,
                        inspectionResultText = x.InspectionResultText
                    }).ToListAsync();

                var Res = new Response<IEnumerable<StoreRes006>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes006>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<bool>> DeleteStoreOutStoreProductDefective(StoreReq003 storeRequest)
        {
            try
            {
                foreach (int item in storeRequest.storeOutStoreProductDefectiveIdArray)
                {
                    var _items = await _db.StoreOutStoreProductDefectives.FindAsync(item);
                    _items.IsDeleted = 1;
                    _db.StoreOutStoreProductDefectives.UpdateRange(_items);
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
        public async Task<Response<bool>> DeleteStoreOutStoreProductInspection(StoreReq004 storeRequest)
        {
            try
            {
                foreach (int item in storeRequest.storeOutStoreProductInspectionIdArray)
                {
                    var _items = await _db.StoreOutStoreProductInspectionTypes.FindAsync(item);
                    _items.IsDeleted = 1;
                    _db.StoreOutStoreProductInspectionTypes.UpdateRange(_items);
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

        //4) 1) 입고관리 메인 화면
        public async Task<Response<IEnumerable<StoreRes008>>> getOutStoreMsts(OutStoreReq001 OutStoreReq001)
        {
            try
            {
                var result =
                    _db.OutStores
                                        .Include(x => x.OutStoreProducts).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.CommonCode)
                    .Where(x => OutStoreReq001.outStoreNo == "" ? true : x.OutStoreNo.Contains(OutStoreReq001.outStoreNo))
                    .Where(x => OutStoreReq001.outStoreStartDate == "" ? true : x.OutStoreDate >= Convert.ToDateTime(OutStoreReq001.outStoreStartDate))
                    .Where(x => OutStoreReq001.outStoreEndDate == "" ? true : x.OutStoreDate <= Convert.ToDateTime(OutStoreReq001.outStoreEndDate))
                    .Where(x => OutStoreReq001.partnerId == 0 ? true : x.Partner.Id == OutStoreReq001.partnerId)
                    .Where(x => OutStoreReq001.productId == 0 ? true :

                        x.OutStoreProducts.Where(x => x.Product.Id == OutStoreReq001.productId)
                        .Select(x => x.Product.Id).ToArray()
                        .Contains(OutStoreReq001.productId)
                    )
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x => x.OutStoreNo).Reverse() //내림차순
                    .Select(x => new StoreRes008
                    {
                        outStoreId = x.OutStoreId,
                        outStoreNo = x.OutStoreNo,
                        requestDeliveryDate = x.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                        partnerId = x.Partner.Id,
                        partnerName = x.Partner.Name,
                        partnerTaxInfo = x.Partner.TaxInfo,

                        outStoreDate = x.OutStoreDate.ToString("yyyy-MM-dd"),
                        outStoreProductCount = x.OutStoreProducts.Where(y=>y.IsDeleted == 0).Count(),

                        outStoreStatus = x.OutStoreProducts.Where(y => y.IsDeleted == 0).Where(y => y.OutStoreStatus == "입고대기").Count() == x.OutStoreProducts.Where(y => y.IsDeleted == 0).Count() ?
                            "입고대기" : x.OutStoreProducts.Where(y => y.IsDeleted == 0).Where(y => y.OutStoreStatus == "입고완료").Count() >= x.OutStoreProducts.Where(y => y.IsDeleted == 0).Count() ? "입고완료" : "입고중",//상태

                        receivingCompletedCount = _db.StoreOutStoreProducts
                            .Where(y=>y.OutStoreProduct.OutStore.OutStoreId == x.OutStoreId)    
                            .Where(y => y.IsDeleted == 0)
                            .Select(y=> y.ProductReceivingCount)
                            .Sum()
                            
                            
                        //receivingCompletedCount =
                        //x.OutStoreProducts
                        //    .Where(osp => osp.IsDeleted == 0)
                        //    //.Sum(osp=> osp.ProductOutStoreCount),
                        //    .Sum(osp => ( osp.StoreOutStoreProducts.Where(osp => osp.IsDeleted == 0)
                        //        .Select(osp => osp.ProductReceivingCount).FirstOrDefault() 
                        //         == 
                        //        osp.StoreOutStoreProducts.Where(osp => osp.IsDeleted == 0)
                        //        .Select(osp => osp.ProductReceivingCount).Sum() ) ? 1 : 0
                        //        ),
                    });


                var filteredResult = result.Where(x => OutStoreReq001.outStoreStatus == "" ? true : x.outStoreStatus == OutStoreReq001.outStoreStatus).ToList();
    

                var Res = new Response<IEnumerable<StoreRes008>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filteredResult
                };

                    return Res;

               
            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes008>>()
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

        public async Task<Response<StoreRes009>> getStoreOutStore(OutStoreReq001 OutStoreReq001)
        {

            try
            {

                var _outStore = await _db.OutStores
                    .Include(x => x.OutStoreProducts).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    .Include(x => x.OutStoreProducts).ThenInclude(x => x.Product)
                    .Include(x => x.Partner)
                    .Where(x => x.OutStoreId == OutStoreReq001.outStoreId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new StoreRes009
                    {

                        outStoreId = x.OutStoreId,
                        uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

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
                        outStoreFinish = x.OutStoreFinish,
                        
                        //UploadFiles = x.UploadFiles

                    }).FirstOrDefaultAsync();


                var Res = new Response<StoreRes009>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _outStore
                };

                return Res;


            }
            catch (Exception ex)
            {

                var Res = new Response<StoreRes009>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes010>>> getStoreOutStoreItems(OutStoreReq001 OutStoreReq001)
        {
            try
            {


                //var _resultb = await _db.OutStores.FromSqlRaw(query)
                //    .ToListAsync();

                //var _result = _resultb.Select(x => new StoreRes008 {
                //    outStoreNo = x.ReceivingNo
                //});

                var result = await _db.OutStoreProducts
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.OutStore).ThenInclude(x => x.CommonCode)
                    .Where(x => x.OutStore.OutStoreId == OutStoreReq001.outStoreId)
                    .Where(x => x.IsDeleted == 0)
                    .OrderBy(x => x.Product.Code)
                    .Select(x => new StoreRes010
                    {
                        taskStatus =
                        (x.StoreOutStoreProducts.Where(sosp => sosp.IsDeleted == 0).Sum(sosp => sosp.ProductReceivingCount) == 0 ) ? "미입고" :
                        ((x.ProductOutStoreCount <= x.StoreOutStoreProducts.Where(sosp => sosp.IsDeleted == 0).Sum(sosp => sosp.ProductReceivingCount))
                        ? "입고완료" : "입고중"),

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

                        productTaxInfo = x.Product.TaxType,
                        productOutStoreCount = x.ProductOutStoreCount,
                        productInCount = _db.StoreOutStoreProducts.Where(y=>y.IsDeleted ==0).Where(y=>y.OutStoreProduct.OutStoreProductId == x.OutStoreProductId).Select(y=>y.ProductReceivingCount).Sum(),
                        productBuyPrice = x.ProductBuyPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        productTotalPrice = x.ProductTotalPrice,
                        outStoreProductMemo = x.OutStoreProductMemo,
                        productImportCheck = x.Product.ImportCheck,
                        
                        //uploadFileId = x.Product.UploadFile != null ? x.Product.UploadFile.Id : 0,
                        //uploadFileName = x.Product.UploadFile != null ? x.Product.UploadFile.FileName : "",
                        //uploadFileUrl = x.Product.UploadFile != null ? x.Product.UploadFile.FileUrl : "",


                    }).ToListAsync();
                //.ToListAsync();
                

                var Res = new Response<IEnumerable<StoreRes010>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;


            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes010>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes011>>> storeMstList2(OutStoreReq001 OutStoreReq001)
        {
            try
            {

                var _store = await _db.Stores

                    .Include(x => x.StoreOutStoreProducts).ThenInclude(x => x.OutStoreProduct).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                    .Where(x=>x.IsDeleted ==0)
                    .Where(x=>
                        x.StoreOutStoreProducts
                            .Where(x=>x.IsDeleted ==0)
                            .Select(sosp=>sosp.OutStoreProduct.OutStoreProductId)
                            .ToArray().Contains(OutStoreReq001.outStoreProductId) )

                    .OrderBy(x => x.ReceivingDate)
                    .Select(x => new StoreRes011
                    {
                        receivingId = x.ReceivingId,
                        receivingNo = x.ReceivingNo,
                        receivingDate = x.ReceivingDate.ToString("yyyy-MM-dd"),
                        partnerName = x.Partner.Name, // 거래처이름
                        partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                        receivingProductCount = x.StoreOutStoreProducts.Select(SSP => SSP).Count(),
                        receivingSupplyPrice = x.StoreOutStoreProducts.Select(SSP => SSP.OutStoreProduct.ProductSupplyPrice).Sum(),
                        receivingTaxPrice = x.StoreOutStoreProducts.Select(SSP => SSP.OutStoreProduct.ProductTaxPrice).Sum(),
                        receivingTotalPrice = x.StoreOutStoreProducts.Select(SSP => SSP.OutStoreProduct.ProductTotalPrice).Sum(),
                        registerName = x.Register.FullName,//등록자
                        receivingMemo = x.ReceivingMemo,
                        uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                    }).ToListAsync();

                var Res = new Response<IEnumerable<StoreRes011>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _store
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes011>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes011>>> storeMstList3(StoreReq001 storeRequest)
        {
            try
            {

                if(storeRequest.invenType == "PRD")
                {
                     var _storePrd = await _db.Stores

                     .Include(x => x.StoreOutStoreProducts).ThenInclude(x => x.OutStoreProduct).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                     .Where(x=>x.IsDeleted == 0)
                     .Where(x => storeRequest.receivingId == 0 ? true : x.ReceivingId == storeRequest.receivingId)
                     .Where(x => storeRequest.receivingStartDate == "" ? x.ReceivingDate >= DateTime.Today : x.ReceivingDate >= Convert.ToDateTime(storeRequest.receivingStartDate))
                     .Where(x => storeRequest.receivingEndDate == "" ? x.ReceivingDate <= DateTime.Today.AddMonths(1) : x.ReceivingDate <= Convert.ToDateTime(storeRequest.receivingEndDate))
                     .Where(x => storeRequest.partnerId == 0 ? true : x.Partner.Id == storeRequest.partnerId)
                     .Where(x=> x.StoreOutStoreProducts.Where(y=>y.IsDeleted == 0).Where(y=>y.OutStoreProduct.Product.Processes.Count()>0).Count()>0)
                     .Where(x=>x.ReceivingNo.Contains(storeRequest.receivingNo))
                    /*
                     .Where(x => storeRequest.productId == 0 ? true :

                         x.StoreOutStoreProducts.Where(x => x.OutStoreProduct.Product.Id == storeRequest.productId)
                         .Select(x => x.OutStoreProduct.Product.Id).ToList()
                         .Contains(storeRequest.productId)
                     )*/
                     .OrderByDescending(x => x.ReceivingNo)
                     .Select(x => new StoreRes011
                     {
                         receivingId = x.ReceivingId,
                         receivingNo = x.ReceivingNo,
                         receivingDate = x.ReceivingDate.ToString("yyyy-MM-dd"),
                         partnerName = x.Partner.Name, // 거래처이름
                         partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                         receivingProductCount = x.StoreOutStoreProducts.Where(y=>y.IsDeleted == 0).Select(SSP => SSP).Count(),

                         receivingSupplyPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),
                         receivingTaxPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount / 10) : 0).Sum(),
                         receivingTotalPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount * 11 / 10) : SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),

                         registerName = x.Register.FullName,//등록자
                         receivingMemo = x.ReceivingMemo,
                         uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                         uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",
                         tempLot = String.Join(",", x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Select(y => y.LotName).ToList()),
                         productIds = x.StoreOutStoreProducts.Where(y=>y.IsDeleted ==0).Select(y=>y.OutStoreProduct.Product.Id).ToArray(),

                     }).ToListAsync();

                var filteredStorePrd = _storePrd
                    .Where(x => storeRequest.LOT == "" ? true : x.tempLot.Contains(storeRequest.LOT))
                    .Where(x => storeRequest.productId == 0 ? true : x.productIds.Contains(storeRequest.productId))

                    .ToList();


                var ResPrd = new Response<IEnumerable<StoreRes011>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filteredStorePrd
                };
                return ResPrd;
                }
                else if(storeRequest.invenType == "ITEM")
                {
                    var _storeItem = await _db.Stores

                    .Include(x => x.StoreOutStoreProducts).ThenInclude(x => x.OutStoreProduct).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => storeRequest.receivingId == 0 ? true : x.ReceivingId == storeRequest.receivingId)
                    .Where(x => storeRequest.receivingStartDate == "" ? x.ReceivingDate >= DateTime.Today : x.ReceivingDate >= Convert.ToDateTime(storeRequest.receivingStartDate))
                    .Where(x => storeRequest.receivingEndDate == "" ? x.ReceivingDate <= DateTime.Today.AddMonths(1) : x.ReceivingDate <= Convert.ToDateTime(storeRequest.receivingEndDate))
                    .Where(x => storeRequest.partnerId == 0 ? true : x.Partner.Id == storeRequest.partnerId)
                    .Where(x => x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Where(y => y.OutStoreProduct.Product.Processes.Count() == 0).Count() > 0)

                     .Where(x => x.ReceivingNo.Contains(storeRequest.receivingNo))
                    .OrderByDescending(x => x.ReceivingNo)
                    .Select(x => new StoreRes011
                    {
                        receivingId = x.ReceivingId,
                        receivingNo = x.ReceivingNo,
                        receivingDate = x.ReceivingDate.ToString("yyyy-MM-dd"),
                        partnerName = x.Partner.Name, // 거래처이름
                        partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                        receivingProductCount = x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Select(SSP => SSP).Count(),

                        receivingSupplyPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),
                        receivingTaxPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount / 10) : 0).Sum(),
                        receivingTotalPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount * 11 / 10) : SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),

                        registerName = x.Register.FullName,//등록자
                        receivingMemo = x.ReceivingMemo,
                        uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",
                        tempLot = String.Join(",", x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Select(y => y.LotName).ToList()),
                        productIds = x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Select(y => y.OutStoreProduct.Product.Id).ToArray(),
                    }).ToListAsync();

                    var filteredStoreItem = _storeItem
                        .Where(x => storeRequest.LOT == "" ? true : x.tempLot.Contains(storeRequest.LOT))
                        .Where(x => storeRequest.productId == 0 ? true : x.productIds.Contains(storeRequest.productId))

                        .ToList();


                    var ResPrd = new Response<IEnumerable<StoreRes011>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = filteredStoreItem
                    };
                    return ResPrd;
                }

                

                var _store = await _db.Stores

                     .Include(x => x.StoreOutStoreProducts).ThenInclude(x => x.OutStoreProduct).ThenInclude(x => x.Product).ThenInclude(x => x.Item).ThenInclude(item => item.CommonCode)
                     .Where(x=>x.IsDeleted == 0)
                     .Where(x => storeRequest.receivingId == 0 ? true : x.ReceivingId == storeRequest.receivingId)
                     .Where(x => storeRequest.receivingStartDate == "" ? x.ReceivingDate >= DateTime.Today : x.ReceivingDate >= Convert.ToDateTime(storeRequest.receivingStartDate))
                     .Where(x => storeRequest.receivingEndDate == "" ? x.ReceivingDate <= DateTime.Today.AddMonths(1) : x.ReceivingDate <= Convert.ToDateTime(storeRequest.receivingEndDate))
                     .Where(x => storeRequest.partnerId == 0 ? true : x.Partner.Id == storeRequest.partnerId)
                     .Where(x => x.ReceivingNo.Contains(storeRequest.receivingNo))
                     .OrderByDescending(x => x.ReceivingNo)
                     .Select(x => new StoreRes011
                     {
                         receivingId = x.ReceivingId,
                         receivingNo = x.ReceivingNo,
                         receivingDate = x.ReceivingDate.ToString("yyyy-MM-dd"),
                         partnerName = x.Partner.Name, // 거래처이름
                         partnerTaxInfo = x.Partner.TaxInfo,//거래처과세정보
                         receivingProductCount = x.StoreOutStoreProducts.Where(y=>y.IsDeleted == 0).Select(SSP => SSP).Count(),

                         receivingSupplyPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),
                         receivingTaxPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount / 10) : 0).Sum(),
                         receivingTotalPrice = x.StoreOutStoreProducts.Select(SSP => SSP.ProductTaxInfo == "과세" ? (SSP.ProductBuyPrice * SSP.ProductReceivingCount * 11 / 10) : SSP.ProductBuyPrice * SSP.ProductReceivingCount).Sum(),

                         registerName = x.Register.FullName,//등록자
                         receivingMemo = x.ReceivingMemo,
                         uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                         uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",
                         tempLot = String.Join(",", x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Select(y => y.LotName).ToList()),
                         productIds = x.StoreOutStoreProducts.Where(y=>y.IsDeleted ==0).Select(y=>y.OutStoreProduct.Product.Id).ToArray(),
                     }).ToListAsync();

                var filteredStore = _store
                    .Where(x => storeRequest.LOT == "" ? true : x.tempLot.Contains(storeRequest.LOT))
                    .Where(x => storeRequest.productId == 0 ? true : x.productIds.Contains(storeRequest.productId))

                    .ToList();


                var Res = new Response<IEnumerable<StoreRes011>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filteredStore
                };
                return Res;
            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes011>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes012>>> storeOutStoreProductList2(StoreReq001 storeRequest)
        {
            try
            {

                if (storeRequest.invenType == "PRD")
                {

                    var resultPrd = await _db.StoreOutStoreProducts
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                    .Where(x => x.Receiving.ReceivingId == storeRequest.receivingId)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x=>x.OutStoreProduct.Product.Processes.Where(y=>y.IsDeleted == 0).Count() > 0)
                    .Select(x => new StoreRes012
                    {

                        storeOutStoreProductId = x.StoreOutStoreProductId,
                        receivingId = x.Receiving.ReceivingId,
                        outStoreProductId = x.OutStoreProduct.OutStoreProductId,
                        productId = x.OutStoreProduct.Product.Id,
                        outStoreNo = x.OutStoreProduct.OutStore.OutStoreNo,
                        productCode = x.OutStoreProduct.Product.Code,
                        productClassification = x.OutStoreProduct.Product.CommonCode != null ? x.OutStoreProduct.Product.CommonCode.Name : "",
                        productName = x.OutStoreProduct.Product.Name,
                        productUnit = x.OutStoreProduct.Product.Unit,
                        productStandard = x.OutStoreProduct.Product.Standard,
                        productStandardUnit = x.ProductStandardUnit,
                        productTaxInfo = x.ProductTaxInfo,
                        productReceivingCount = x.ProductReceivingCount,
                        productBuyPrice = x.ProductBuyPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        productTotalPrice = x.ProductTotalPrice,
                        productLOT = x.LotName,

                        receivingProductMemo = x.ReceivingProductMemo,

                    }).ToListAsync();

                    var ResPrd = new Response<IEnumerable<StoreRes012>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = resultPrd
                    };

                    return ResPrd;
                }else if(storeRequest.invenType == "ITEM")
                {
                    var resultItem = await _db.StoreOutStoreProducts
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                    .Where(x => x.Receiving.ReceivingId == storeRequest.receivingId)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.OutStoreProduct.Product.Processes.Where(y => y.IsDeleted == 0).Count() == 0)
                    .Select(x => new StoreRes012
                    {

                        storeOutStoreProductId = x.StoreOutStoreProductId,
                        receivingId = x.Receiving.ReceivingId,
                        outStoreProductId = x.OutStoreProduct.OutStoreProductId,
                        productId = x.OutStoreProduct.Product.Id,
                        outStoreNo = x.OutStoreProduct.OutStore.OutStoreNo,
                        productCode = x.OutStoreProduct.Product.Code,
                        productClassification = x.OutStoreProduct.Product.CommonCode != null ? x.OutStoreProduct.Product.CommonCode.Name : "",
                        productName = x.OutStoreProduct.Product.Name,
                        productUnit = x.OutStoreProduct.Product.Unit,
                        productStandard = x.OutStoreProduct.Product.Standard,
                        productStandardUnit = x.ProductStandardUnit,
                        productTaxInfo = x.ProductTaxInfo,
                        productReceivingCount = x.ProductReceivingCount,
                        productBuyPrice = x.ProductBuyPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        productTotalPrice = x.ProductTotalPrice,
                        productLOT = x.LotName,

                        receivingProductMemo = x.ReceivingProductMemo,

                    }).ToListAsync();

                    var ResPrd = new Response<IEnumerable<StoreRes012>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = resultItem
                    };

                    return ResPrd;
                }





                    var result = await _db.StoreOutStoreProducts
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                    .Where(x => x.Receiving.ReceivingId == storeRequest.receivingId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new StoreRes012
                    {

                        storeOutStoreProductId = x.StoreOutStoreProductId,
                        receivingId = x.Receiving.ReceivingId,
                        outStoreProductId = x.OutStoreProduct.OutStoreProductId,
                        productId = x.OutStoreProduct.Product.Id,
                        outStoreNo = x.OutStoreProduct.OutStore.OutStoreNo,
                        productCode = x.OutStoreProduct.Product.Code,
                        productClassification = x.OutStoreProduct.Product.CommonCode != null ? x.OutStoreProduct.Product.CommonCode.Name : "",
                        productName = x.OutStoreProduct.Product.Name,
                        productUnit = x.OutStoreProduct.Product.Unit,
                        productStandard = x.OutStoreProduct.Product.Standard,
                        productStandardUnit = x.ProductStandardUnit,
                        productTaxInfo = x.ProductTaxInfo,
                        productReceivingCount = x.ProductReceivingCount,
                        productBuyPrice = x.ProductBuyPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        productTotalPrice = x.ProductTotalPrice,
                        productLOT = x.LotName,

                        receivingProductMemo = x.ReceivingProductMemo,

                    }).ToListAsync();

                var Res = new Response<IEnumerable<StoreRes012>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes012>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<StoreRes014>>> storeOutStoreProductList3(StoreReq001 storeRequest)
        {
            try
            {

                var result = await _db.StoreOutStoreProducts
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.UploadFile)
                    .Include(x => x.OutStoreProduct.Product).ThenInclude(x => x.Item).ThenInclude(x => x.CommonCode)
                    .Include(x => x.Receiving)
                    //.Include(x => x.Order).ThenInclude(x => x.CommonCode)
                    .Where(x => x.Receiving.ReceivingId == storeRequest.receivingId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new StoreRes014
                    {

                        storeOutStoreProductId = x.StoreOutStoreProductId,
                        receivingId = x.Receiving.ReceivingId,
                        outStoreProductId = x.OutStoreProduct.OutStoreProductId,
                        productId = x.OutStoreProduct.Product.Id,
                        receivingNo = x.Receiving.ReceivingNo,
                        productCode = x.OutStoreProduct.Product.Code,
                        productClassification = x.OutStoreProduct.Product.CommonCode != null ? x.OutStoreProduct.Product.CommonCode.Name : "",
                        productName = x.OutStoreProduct.Product.Name,
                        productStandard = x.OutStoreProduct.Product.Standard,
                        productStandardUnit = _db.CommonCodes.Where(common => (common.Code == x.OutStoreProduct.ProductStandardUnit)).FirstOrDefault().Name,
                        productTaxInfo = x.OutStoreProduct.Product.Item != null ? x.OutStoreProduct.Product.Item.TaxType : "",
                        productReceivingCount = x.ProductReceivingCount,
                        productBuyPrice = x.ProductBuyPrice,
                        productSupplyPrice = x.ProductSupplyPrice,
                        productTaxPrice = x.ProductTaxPrice,
                        productTotalPrice = x.ProductTotalPrice,
                        productLOT = x.LotName,
                        //productGoodQuantity = 0, //todo,
                        //productDefectiveQuantity = 0, //todo
                        //productImportCheckResult = x.ProductImportCheckResult,

                        receivingProductMemo = x.ReceivingProductMemo,

                    }).ToListAsync();
                //.ToListAsync();

                var Res = new Response<IEnumerable<StoreRes014>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes014>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<StoreRes013>>> outStoreProductListPop(StoreReq005 storeRequest)
        {
            try
            {

                if (storeRequest.invenType == "ITEM")
                {
                    var resItem = await _db.OutStoreProducts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() == 0)
                        .Where(x => x.OutStore.OutStoreNo.Contains(storeRequest.searchInput) || x.Product.Code.Contains(storeRequest.searchInput) || x.Product.Name.Contains(storeRequest.searchInput) || x.Product.Standard.Contains(storeRequest.searchInput) || x.Product.Memo.Contains(storeRequest.searchInput) || x.Product.CommonCode.Name.Contains(storeRequest.searchInput))
                        .Where(x => storeRequest.partnerId == 0 ? true : x.OutStore.Partner.Id == storeRequest.partnerId)
                        .Where(x => storeRequest.taskStatus == "" ? true : storeRequest.taskStatus == "입고" ? x.OutStoreStatus == "입고완료" : x.OutStoreStatus == "입고중" || x.OutStoreStatus == "입고대기")
                        .Select(x => new StoreRes013
                        {
                            outStoreId = x.OutStore.OutStoreId,
                            outStoreDate = x.OutStore.OutStoreDate.ToString("yyyy-MM-dd"),
                            outStoreNo = x.OutStore.OutStoreNo,
                            requestDeliveryDate = x.OutStore.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                            outStoreProductId = x.OutStoreProductId,
                            outStoreProductMemo = x.OutStoreProductMemo,
                            partnerId = x.OutStore.Partner.Id,
                            partnerName = x.OutStore.Partner.Name,
                            productClassification = x.Product.CommonCode.Name,
                            productCode = x.Product.Code,
                            productId = x.Product.Id,
                            productName = x.Product.Name,
                            productOutStoreCount = x.ProductOutStoreCount,
                            productReceivingCount = x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Select(y => y.ProductReceivingCount).Sum(),
                            productStandard = x.Product.Standard,
                            productStandardUnit = x.ProductStandardUnit,
                            productUnit = x.Product.Unit,
                            taskStatus = x.OutStoreStatus,
                            productImportCheck = x.Product.ImportCheck,
                            productTaxInfo = x.ProductTaxInfo,
                            productBuyPrice = x.ProductBuyPrice,
                            productStandardUnitCount = x.ProductStandardUnitCount,
                            storeOutStoreProductDefectives = new List<StoreOutStoreProductDefectiveInterface>(),
                            storeOutStoreProductInspections = x.Product.ImportCheck ? _db.InspectionTypes
                                .Where(y => y.IsDeleted == false)
                                .Where(y => y.Type == "수입검사")
                                .Where(y=>y.IsUsing == true)
                                .Select(y => new StoreOutStoreProductInspectionInterface
                                {
                                    inspectionCode = y.Code,
                                    inspectionMethod = y.InspectionMethod,
                                    inspectionResult = "",
                                    storeOutStoreProductId = 0,
                                    storeOutStoreProductInspectionId = 0,
                                    inspectionId = y.Id,
                                    inspectionStandard = y.InspectionStandard,
                                    inspectionResultText = "",
                                    InspectionTypeId = y.Id,
                                    inspectionName = y.InspectionItem,
                                }).ToList() : new List<StoreOutStoreProductInspectionInterface>()

                        })
                        .ToListAsync();


                    var ResItem = new Response<IEnumerable<StoreRes013>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = resItem
                    };

                    return ResItem;
                }


                if (storeRequest.invenType == "PRD")
                {
                    var resPrd = await _db.OutStoreProducts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Processes.Where(y => y.IsDeleted == 0).Count() > 0)
                        .Where(x => x.OutStore.OutStoreNo.Contains(storeRequest.searchInput))
                        .Where(x => storeRequest.partnerId == 0 ? true : x.OutStore.Partner.Id == storeRequest.partnerId)
                        .Where(x => storeRequest.taskStatus == "" ? true : storeRequest.taskStatus == "입고" ? x.OutStoreStatus == "입고완료" : x.OutStoreStatus == "입고중" || x.OutStoreStatus == "입고대기")
                        .Select(x => new StoreRes013
                        {
                            outStoreId = x.OutStore.OutStoreId,
                            outStoreDate = x.OutStore.OutStoreDate.ToString("yyyy-MM-dd"),
                            outStoreNo = x.OutStore.OutStoreNo,
                            requestDeliveryDate = x.OutStore.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                            outStoreProductId = x.OutStoreProductId,
                            outStoreProductMemo = x.OutStoreProductMemo,
                            partnerId = x.OutStore.Partner.Id,
                            partnerName = x.OutStore.Partner.Name,
                            productClassification = x.Product.CommonCode.Name,
                            productCode = x.Product.Code,
                            productId = x.Product.Id,
                            productName = x.Product.Name,
                            productOutStoreCount = x.ProductOutStoreCount,
                            productReceivingCount = x.StoreOutStoreProducts.Where(y => y.IsDeleted == 0).Select(y => y.ProductReceivingCount).Sum(),
                            productStandard = x.Product.Standard,
                            productStandardUnit = x.ProductStandardUnit,
                            productUnit = x.Product.Unit,
                            taskStatus = x.OutStoreStatus,
                            productImportCheck = x.Product.ImportCheck,
                            productTaxInfo = x.ProductTaxInfo,
                            productBuyPrice = x.ProductBuyPrice,
                            productStandardUnitCount = x.ProductStandardUnitCount,
                            storeOutStoreProductDefectives = new List<StoreOutStoreProductDefectiveInterface>(),
                            storeOutStoreProductInspections = x.Product.ImportCheck ? _db.InspectionTypes
                                .Where(y => y.IsDeleted == false)
                                .Where(y => y.Type == "수입검사")
                                .Where(y => y.IsUsing == true)

                                .Select(y => new StoreOutStoreProductInspectionInterface
                                {
                                    inspectionCode = y.Code,
                                    inspectionMethod = y.InspectionMethod,
                                    inspectionResult = "",
                                    storeOutStoreProductId = 0,
                                    storeOutStoreProductInspectionId = 0,
                                    inspectionId = y.Id,
                                    inspectionStandard = y.InspectionStandard,
                                    inspectionResultText = "",
                                    InspectionTypeId = y.Id,
                                    inspectionName = y.InspectionItem,
                                }).ToList() : new List<StoreOutStoreProductInspectionInterface>()

                        })
                        .ToListAsync();


                    var ResPrd = new Response<IEnumerable<StoreRes013>>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = resPrd
                    };

                    return ResPrd;
                }


                var res = await _db.OutStoreProducts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.OutStore.OutStoreNo.Contains(storeRequest.searchInput) || x.Product.Code.Contains(storeRequest.searchInput) || x.Product.Name.Contains(storeRequest.searchInput) || x.Product.Standard.Contains(storeRequest.searchInput) || x.Product.Memo.Contains(storeRequest.searchInput) || x.Product.CommonCode.Name.Contains(storeRequest.searchInput))

                    .Where(x => storeRequest.partnerId == 0 ? true : x.OutStore.Partner.Id == storeRequest.partnerId )
                    .Where(x => storeRequest.taskStatus == "" ? true : storeRequest.taskStatus == "입고" ? x.OutStoreStatus == "입고완료" : x.OutStoreStatus == "입고중" || x.OutStoreStatus == "입고대기")
                    .Select(x => new StoreRes013
                    {
                        outStoreId = x.OutStore.OutStoreId,
                        outStoreDate = x.OutStore.OutStoreDate.ToString("yyyy-MM-dd"),
                        outStoreNo = x.OutStore.OutStoreNo,
                        requestDeliveryDate = x.OutStore.RequestDeliveryDate.ToString("yyyy-MM-dd"),
                        outStoreProductId = x.OutStoreProductId,
                        outStoreProductMemo = x.OutStoreProductMemo,
                        partnerId = x.OutStore.Partner.Id,
                        partnerName = x.OutStore.Partner.Name,
                        productClassification = x.Product.CommonCode.Name,
                        productCode = x.Product.Code,
                        productId = x.Product.Id,
                        productName = x.Product.Name,
                        productOutStoreCount = x.ProductOutStoreCount,
                        productReceivingCount = x.StoreOutStoreProducts.Where(y=>y.IsDeleted == 0).Select(y=>y.ProductReceivingCount).Sum(),
                        productStandard = x.Product.Standard,
                        productStandardUnit = x.ProductStandardUnit,
                        productUnit = x.Product.Unit,
                        taskStatus = x.OutStoreStatus,
                        productImportCheck = x.Product.ImportCheck,
                        productTaxInfo = x.ProductTaxInfo,
                        productBuyPrice = x.ProductBuyPrice,
                        productStandardUnitCount = x.ProductStandardUnitCount,
                        storeOutStoreProductDefectives = new List<StoreOutStoreProductDefectiveInterface>(),
                        storeOutStoreProductInspections = x.Product.ImportCheck ? _db.InspectionTypes
                            .Where(y=>y.IsDeleted == false)
                            .Where(y=>y.Type == "수입검사")
                            .Select(y=> new StoreOutStoreProductInspectionInterface
                            {
                                inspectionCode = y.Code,
                                inspectionMethod = y.InspectionMethod,
                                inspectionResult = "",
                                storeOutStoreProductId = 0,
                                storeOutStoreProductInspectionId = 0,
                                inspectionId = y.Id,
                                inspectionStandard = y.InspectionStandard,
                                inspectionResultText = "",
                                InspectionTypeId = y.Id,
                                inspectionName = y.InspectionItem,
                            }).ToList() : new List<StoreOutStoreProductInspectionInterface>()

                    })
                    .ToListAsync();


                var Res = new Response<IEnumerable<StoreRes013>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<StoreRes013>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<bool>> eventOutStoreFinish(StoreRequestCrud id)
        {
            try
            {



                var outStoreProducts = await _db.OutStoreProducts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.OutStore.OutStoreId == id.outStoreId)
                        .ToListAsync();


                foreach(var outStoreProduct in outStoreProducts)
                {
                    if(outStoreProduct.OutStoreStatus == "입고대기")
                    {
                        
                    }

                    if(outStoreProduct.OutStoreStatus == "입고중")
                    {

                    }

                    outStoreProduct.OutStoreStatus = "입고완료";
                    _db.OutStoreProducts.Update(outStoreProduct);
                }


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

    }
}
