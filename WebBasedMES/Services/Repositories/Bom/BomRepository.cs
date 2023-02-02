using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.Bom;

namespace WebBasedMES.Services.Repositories.Bom
{
    public class BomRepository : IBomRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public BomRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<Response<IEnumerable<ProductInputItemResponse>>> GetProductInputItems(ProductInputItemRequest req)
        {
            try
            {

                var res = await _db.ProductItems
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProductProcess.Product.Id == req.ProductId)
                    .Select(x => new ProductInputItemResponse
                    {
                        ProductId = x.ProductId,
                        LOSS = x.Loss,
                        RequireQuantity = x.Require,
                        TotalRequire = x.Loss + x.Require,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,

                    }).ToListAsync();

                if(res.Count == 0)
                {
                     res = await _db.Products
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.Id == req.ProductId)
                    .Select(x => new ProductInputItemResponse
                    {
                        ProductId = x.Id,
                        LOSS = 0,
                        RequireQuantity = 0,
                        TotalRequire = 0,
                        ProductClassification = x.CommonCode.Name,
                        ProductCode = x.Code,
                        ProductName = x.Name,
                        ProductStandard = x.Standard,
                        ProductUnit = x.Unit,
                    }).ToListAsync();
                }




                foreach(var p in res)
                {
                    var stk = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Id == p.ProductId)
                        .Select(z => new ProductInputItemStock
                        {
                            StockCount = (z.StoreOutCount - z.DefectiveCount - z.ConsumeCount + z.ProduceCount - z.OutOrderCount + z.ModifyCount),
                            LotId = z.Lot.LotId,
                            LotName = z.Lot.LotName,


                        }).ToListAsync();

                    var stk2 = stk.GroupBy(x => x.LotName)
                                .Select(x => new ProductInputItemStock
                                {
                                    LotName = x.Key,
                                    LotId = x.FirstOrDefault().LotId,
                                    StockCount = x.Sum(k => k.StockCount),
                                    InputQuantity = 0,
                                    ProductCode = p.ProductCode,
                                    ProductName = p.ProductName,
                                    ProductStandard = p.ProductStandard,
                                    ProductUnit = p.ProductUnit,
                                    IsSelected = false,

                                }).ToList();

                    var stk3 = stk2.Where(x => x.StockCount > 0);

                    p.ProductInputItemStocks = stk3;
                }

                var Res = new Response<IEnumerable<ProductInputItemResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res != null ? res : new List<ProductInputItemResponse>()
                };
                return Res;
            }


            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductInputItemResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }


        public async Task<Response<ProductInputItemResponse>> GetProductInputItemAdd(ProductInputItemRequest req)
        {
            try
            {

                var res = await _db.Products
                    .Where(x => x.Id == req.ProductId)
                    .Select(x => new ProductInputItemResponse
                    {
                        ProductId = x.Id,
                        LOSS = 1,
                        RequireQuantity = 1,
                        ProductClassification = x.CommonCode.Name,
                        ProductCode = x.Code,
                        ProductName = x.Name,
                        ProductStandard = x.Standard,
                        ProductUnit = x.Unit,

                    }).FirstOrDefaultAsync();

                    var stk = await _db.LotCounts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.Product.Id == req.ProductId)
                        .Select(z => new ProductInputItemStock
                        {
                            StockCount = (z.StoreOutCount - z.DefectiveCount - z.ConsumeCount + z.ProduceCount - z.OutOrderCount + z.ModifyCount),
                            LotId = z.Lot.LotId,
                            LotName = z.Lot.LotName,

                        }).ToListAsync();

                    var stk2 = stk.GroupBy(x => x.LotName)
                                .Select(x => new ProductInputItemStock
                                {
                                    LotName = x.Key,
                                    LotId = x.FirstOrDefault().LotId,
                                    StockCount = x.Sum(k => k.StockCount),
                                    InputQuantity = 0,
                                    ProductCode = res.ProductCode,
                                    ProductName = res.ProductName,
                                    ProductStandard = res.ProductStandard,
                                    ProductUnit = res.ProductUnit,
                                    IsSelected = false,

                                }).ToList();

                    var stk3 = stk2.Where(x => x.StockCount > 0);

                    res.ProductInputItemStocks = stk3;
                

                var Res = new Response<ProductInputItemResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;
            }


            catch (Exception ex)
            {
                var Res = new Response<ProductInputItemResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null,
                };
                return Res;
            }
        }



        public async Task<Response<IEnumerable<BomResponseSortByProcess>>> DownloadBomExcelFile(ProductRequest req)
        {
            try
            {
                var prd = await _db.Products
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFile)
                    .Include(x => x.Processes).ThenInclude(x => x.Items)
                    //.Where(x=>x.Processes.Where(y=>y.IsDeleted == 0).Count()>0)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => req.IsUsingStr == "ALL" ? true : x.IsUsing == (req.IsUsingStr == "Y" ? true : false))
                    .Where(x => req.TypeStr == "ALL" ?true : x.CommonCode.Name.Contains(req.TypeStr))
                    .Where(x => x.Name.Contains(req.SearchStr) || x.Code.Contains(req.SearchStr) || x.Memo.Contains(req.SearchStr) || x.Standard.Contains(req.SearchStr) || x.Model.Name.Contains(req.SearchStr) || x.Partner.Name.Contains(req.SearchStr))
                    .Select(x => new BomResponseSortByProcess
                    {
                        ProductId = x.Id,  //Product(BOM) ID
                        ProductCode = x.Code, //품목코드
                        ProductName = x.Name,
                        ProductCommonCodeName = x.CommonCode.Name,//품목구분
                        ProductUnit = x.Unit,
                        ProductIsUsing = x.IsUsing,
                        ProductStandard = x.Standard,



                        ProcessCount = x.Processes.Where(y => y.IsDeleted == 0).Count(),

                        ProductProcesses = x.Processes
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => new BomProductProcessResponseByProcess
                            {
                                ProductProcessId = y.ProductProcessId,
                                ProcessOrder = y.ProcessOrder,
                                ProcessCode = y.Process.Code,
                                ProcessName = y.Process.Name,
                                ProcessCommonCodeName = y.Process.CommonCode.Name,
                                IsUsing = y.Process.IsUsing,
                                IsFinal = y.IsFinal,
                                Memo = y.Memo,
                                IsOutSourcing = y.IsOutSourcing,
                                ProductProducedId = y.ProduceProductId,

                                ProductProducedCode = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Code).First() : "",
                                ProductProducedName = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Name).First() : "",
                                ProductProducedCommonCodeName = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.CommonCode.Name).First() : "",
                                ProductProducedStandard = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Standard).First() : "",
                                ProductProducedUnit = y.ProduceProductId != 0 ? _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Unit).First() : "",


                                
                                ProductItems = y.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Select(z => new BomProductItemReponseByProcess
                                    {
                                        ProductItemId = z.ProductItemId,
                                        ProductProcessId = y.ProductProcessId,

                                        ProcessId = y.Process.Id,
                                        ProcessName = y.Process.Name,
                                        ProcessCode = y.Process.Code,
                                        ProcessOrder = z.ProcessOrder,

                                        ProductId = z.Product.Id,
                                        ProductCode = z.Product.Code,
                                        ProductName = z.Product.Name,
                                        ProductUnit = z.Product.Unit,
                                        ProductStandard = z.Product.Standard,
                                        ProductCommonCodeName = z.Product.CommonCode.Name,

                                        Loss = (float)z.Loss,
                                        Memo = z.Memo,
                                        Require = (float)z.Require,
                                        Priority = z.Priority
                                    })
                                    .ToList()
                                
                            }).ToList(),
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<BomResponseSortByProcess>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = prd
                };

                return Res;
            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<BomResponseSortByProcess>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }




        public async Task<Response<bool>> UploadBomExcelFile(List<BomUpdateInterface> req)
        {
            try
            {

                //Interface data to... 
                var data = req
                    .GroupBy(x => x.ProductCode)
                    .Select(x => new BomUpdateRequest
                    {
                        ProductId = _db.Products.Where(k => k.IsDeleted == false).Where(k => k.Code == x.Key).Select(k => k.Id).FirstOrDefault(),
                        ProductProcesses = x.GroupBy(k => k.ProcessCode).Select(y => new ProductProcessUpdate
                        {
                            IsFinal = false,
                            IsOutSourcing = false,
                            Memo = "",
                            PartnerId = 0,
                            ProcessId = _db.Processes.Where(z => z.IsDeleted == false).Where(z => z.Code == y.Key).Select(z => z.Id).FirstOrDefault(),
                            ProcessOrder = y.FirstOrDefault().ProcessOrder,
                            
                            ProduceProductId = _db.Products
                                .Where(k => k.IsDeleted == false)
                                .Where(k => k.Code == y.FirstOrDefault().ProducedProductCode)
                                .Select(k => k.Id)
                                .FirstOrDefault(),
                        }).ToList(),

                        ProductItems = x.Select(y => new ProductItemUpdate
                        {
                            Loss = (float)y.Loss,
                            Require = (float)y.Require,
                            Memo = "",
                            ProcessId = _db.Processes.Where(z=>z.IsDeleted == false).Where(z=>z.Code == y.ProcessCode).Select(z=>z.Id).FirstOrDefault(),
                            picture = "",
                            Priority = 0,
                            ProductItemId = _db.Products.Where(k=>k.IsDeleted == false).Where(k=>k.Code == y.InputItemCode).Select(k=>k.Id).FirstOrDefault(),
                            ProcessOrder = y.ProcessOrder,
                            ProductId = _db.Products.Where(k => k.IsDeleted == false).Where(k => k.Code == x.Key).Select(k => k.Id).FirstOrDefault(),

                        }).ToList()

                    })
                    .ToArray();

                foreach(var prd in data)
                {
                    var orig = await _db.Products
                        .Include(x => x.Processes)
                        .ThenInclude(x => x.Items)
                        .Where(x => x.Id == prd.ProductId)
                        .FirstOrDefaultAsync();

                    foreach(var originProcess in orig.Processes)
                    {
                        originProcess.IsDeleted = 1;

                        foreach(var originItem in originProcess.Items)
                        {
                            originItem.IsDeleted = 1;
                        }
                    }

                    _db.Products.Update(orig);
                }

                await Save();


                foreach(var prd in data)
                {
                    var orig = await _db.Products
                        .Include(x => x.Processes)
                        .ThenInclude(x => x.Items)
                        .Where(x => x.Id == prd.ProductId)
                        .FirstOrDefaultAsync();


                    foreach(var proc in prd.ProductProcesses)
                    {
                        //Item Add
                        List<ProductItem> newItems = new List<ProductItem>();
                        foreach (var item in prd.ProductItems)
                        {
                            if(item.ProcessId == proc.ProcessId)
                            {
                                var _item = new ProductItem
                                {
                                    Loss = item.Loss,
                                    Require = item.Require,
                                    IsDeleted = 0,
                                    Memo = "",
                                    Priority = 1,
                                    ProcessId = item.ProcessId,
                                    Product = _db.Products.Where(x => x.Id == item.ProductItemId).FirstOrDefault(),
                                    ProcessOrder = item.ProcessOrder
                                };
                                if (_item.Product != null)
                                {
                                    newItems.Add(_item);
                                }
                            }
                        }

                        var newProc = new ProductProcess
                        {
                            IsDeleted = 0,
                            Items = newItems,
                            IsFinal = false,
                            IsOutSourcing = false,
                            Memo = "",
                            ProcessOrder = proc.ProcessOrder,
                            Process = _db.Processes.Where(x=>x.Id == proc.ProcessId).FirstOrDefault(),
                            Product = orig,
                        };

                        _db.ProductProcesses.Add(newProc);

                    }
                }

                await Save();


                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true
                };

                return Res;



                /*

                var originData = await _db.Products
                    .Include(x => x.CommonCode)
                    .Include(x => x.UploadFile)
                    .Include(x => x.Processes).ThenInclude(x => x.Items)
                    .Where(x => x.Id == bom.ProductId)
                    .Select(x => new BomResponseSortByProcess
                    {
                        ProductId = x.Id,  //Product(BOM) ID
                        ProductCode = x.Code, //품목코드
                        ProductName = x.Name,
                        ProductCommonCodeName = x.CommonCode.Name,//품목구분
                        ProductUnit = x.Unit,
                        ProductIsUsing = x.IsUsing,
                        ProductStandard = x.Standard,
                        ProductProcesses = x.Processes
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => new BomProductProcessResponseByProcess
                            {
                                ProductProcessId = y.ProductProcessId,

                                ProcessOrder = y.ProcessOrder,
                                ProcessCode = y.Process.Code,
                                ProcessName = y.Process.Name,
                                ProcessCommonCodeName = y.Process.CommonCode.Name,

                                IsUsing = y.Process.IsUsing,
                                IsFinal = y.IsFinal,
                                Memo = y.Memo,


                                ProductProducedId = y.ProduceProductId,
                                ProductProducedCode = y.Product.Code,
                                ProductProducedName = y.Product.Name,
                                ProductProducedCommonCodeName = y.Product.CommonCode.Name,
                                ProductProducedStandard = y.Product.Standard,
                                ProductProducedUnit = y.Product.Standard,

                                ProductItems = y.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Select(z => new BomProductItemReponseByProcess
                                    {
                                        ProductItemId = z.ProductItemId,

                                        ProcessName = y.Process.Name,
                                        ProcessId = y.Process.Id,
                                        ProcessCode = y.Process.Code,
                                        ProcessOrder = z.ProcessOrder,

                                        ProductId = z.ProductId,
                                        ProductCode = z.Product.Code,
                                        ProductName = z.Product.Name,
                                        ProductUnit = z.Product.Unit,
                                        ProductStandard = z.Product.Standard,
                                        ProductCommonCodeName = z.Product.CommonCode.Name,
                                        Loss = (float)z.Loss,
                                        Memo = z.Memo,
                                        Require = (float)z.Require,
                                        Priority = z.Priority
                                    })
                                    .ToList()
                            }).ToList(),

                    })
                    .FirstOrDefaultAsync();

                //0. 아이템 분류
                List<int> _deleteProductProcesses = new List<int>();
                List<ProductProcessUpdate> _updateProductProcesses = new List<ProductProcessUpdate>();
                List<ProductProcessUpdate> _createProductProcesses = new List<ProductProcessUpdate>();

                List<int> _deleteProductItems = new List<int>();
                List<ProductItemUpdate> _updateProductItems = new List<ProductItemUpdate>();
                List<ProductItemUpdate> _createProductItems = new List<ProductItemUpdate>();

                bool checkFlag = false;
                foreach (var _orig in originData.ProductProcesses)
                {
                    checkFlag = false;
                    foreach (var _proc in bom.ProductProcesses)
                    {
                        if (_proc.ProductProcessId == _orig.ProductProcessId)
                        {
                            _updateProductProcesses.Add(_proc);
                            checkFlag = true;
                        }
                    }
                    if (!checkFlag)
                    {
                        _deleteProductProcesses.Add(_orig.ProductProcessId);
                    }
                }

                foreach (var _proc in bom.ProductProcesses)
                {
                    if (_proc.ProductProcessId == 0)
                    {
                        _createProductProcesses.Add(_proc);
                    }
                }

                foreach (var _orig in originData.ProductProcesses)
                {

                    foreach (var _item in _orig.ProductItems)
                    {
                        checkFlag = false;

                        foreach (var _bomItem in bom.ProductItems)
                        {
                            if (_bomItem.ProductItemId == _item.ProductItemId)
                            {
                                _updateProductItems.Add(_bomItem);
                                checkFlag = true;
                            }
                        }

                        if (!checkFlag)
                        {
                            _deleteProductItems.Add(_item.ProductItemId);
                        }
                    }
                }


                foreach (var _item in bom.ProductItems)
                {
                    if (_item.ProductItemId == 0)
                    {
                        _createProductItems.Add(_item);
                    }
                }

                foreach (var i in _deleteProductProcesses)
                {
                    var _item = await _db.ProductProcesses.Where(x => x.ProductProcessId == i).FirstOrDefaultAsync();
                    _item.IsDeleted = 1;

                    _db.ProductProcesses.Update(_item);
                    await Save();
                }

                foreach (var _proc in _updateProductProcesses)
                {
                    var _item = await _db.ProductProcesses.Where(x => x.ProductProcessId == _proc.ProductProcessId).FirstOrDefaultAsync();
                    _item.ProcessOrder = _proc.ProcessOrder;
                    _item.PartnerId = _proc.PartnerId;
                    _item.ProcessId = _proc.ProcessId;
                    _item.IsFinal = _proc.IsFinal;
                    _item.ProduceProductId = _proc.ProduceProductId;
                    _item.IsOutSourcing = _proc.IsOutSourcing;
                    _item.Memo = _proc.Memo;
                    _db.ProductProcesses.Update(_item);
                    await Save();
                }

                foreach (var _proc in _createProductProcesses)
                {
                    var _product = _db.Products.Where(x => x.Id == bom.ProductId).FirstOrDefault();

                    var _item = new ProductProcess
                    {
                        ProcessOrder = _proc.ProcessOrder,
                        PartnerId = _proc.PartnerId,
                        ProcessId = _proc.ProcessId,
                        Product = _product,
                        IsFinal = _proc.IsFinal,
                        ProduceProductId = _proc.ProduceProductId,
                        IsOutSourcing = _proc.IsOutSourcing,
                        Memo = _proc.Memo
                    };

                    _db.ProductProcesses.Add(_item);
                    await Save();
                }

                foreach (var i in _deleteProductItems)
                {
                    var _item = await _db.ProductItems.Where(x => x.ProductItemId == i).FirstOrDefaultAsync();

                    _item.IsDeleted = 1;
                    _db.ProductItems.Update(_item);
                    await Save();
                }


                foreach (var _proc in _updateProductItems)
                {
                    var _item = await _db.ProductItems.Where(x => x.ProductItemId == _proc.ProductItemId).FirstOrDefaultAsync();


                    _item.ProcessOrder = _proc.ProcessOrder;
                    _item.ProductProcessId = _proc.ProductProcessId == 0 ? _db.ProductProcesses.Where(x => x.ProcessOrder == _proc.ProcessOrder && bom.ProductId == x.ProductId && x.IsDeleted == 0).Select(x => x.ProductProcessId).FirstOrDefault() : _proc.ProductProcessId;
                    _item.ProcessId = _proc.ProcessId;
                    _item.Product = _db.Products.Where(x => x.Id == _proc.ProductId).FirstOrDefault();
                    _item.ProductId = _proc.ProductId;

                    _item.Require = _proc.Require;
                    _item.Loss = _proc.Loss;
                    _item.Memo = _proc.Memo;
                    _item.Priority = _proc.Priority;

                    _db.ProductItems.Update(_item);
                    await Save();
                }


                foreach (var _proc in _createProductItems)
                {
                    var _item = new ProductItem
                    {
                        ProcessOrder = _proc.ProcessOrder,
                        ProductProcessId = _proc.ProductProcessId == 0 ? _db.ProductProcesses.Where(x => x.ProcessOrder == _proc.ProcessOrder && bom.ProductId == x.ProductId && x.IsDeleted == 0).Select(x => x.ProductProcessId).FirstOrDefault() : _proc.ProductProcessId,
                        ///ProductProcessId = _proc.ProductProcessId,
                        ProcessId = _proc.ProcessId,

                        ProductId = _proc.ProductId,
                        Product = _db.Products.Where(x => x.Id == _proc.ProductId).FirstOrDefault(),

                        IsDeleted = 0,
                        Require = _proc.Require,
                        Loss = _proc.Loss,
                        Memo = _proc.Memo,
                        Priority = _proc.Priority,
                    };

                    _db.ProductItems.Add(_item);
                    await Save();
                }

                var Res = new Response<bool>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = true
                };

                return Res;

                */
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



    }
}
