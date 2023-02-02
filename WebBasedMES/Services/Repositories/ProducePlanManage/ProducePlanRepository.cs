using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.Data;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProducePlan;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebBasedMES.Data.Models.ProducePlan;
using System.Text.Json;
using WebBasedMES.ViewModels.BaseInfo;

namespace WebBasedMES.Services.Repositories.ProducePlanManage
{
    public class ProducePlanRepository : IProducePlanRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProducePlanRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        #region 1) 생산계획관리

        #region 생산계획목록 조회
        public async Task<Response<IEnumerable<ProducePlanReponse001>>> GetProducePlans(ProducePlanRequest001 param)
        {
            try
            {
                var res = await _db.ProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.RegisterDate >= Convert.ToDateTime(param.registerStartDate))
                    .Where(x => x.RegisterDate <= Convert.ToDateTime(param.registerEndDate))
                    .Where(x => param.registerId == ""? true : x.Register.Id == param.registerId)
                    .Where(x => x.ProductionPlanNo.Contains(param.productionPlanNo))
                    .Where(x => param.productionPlanStatus =="ALL"? true : x.ProductionPlanStatus.Contains(param.productionPlanStatus))
                    .Where(x => param.productId == 0? true: x.ProducePlanProducts.Where(y=>y.IsDeleted == 0).Select(y=> y.Product.Id).Contains(param.productId) ? true:false)
                    .OrderByDescending(x=>x.ProductionPlanNo)
                    .Select(x => new ProducePlanReponse001
                    {
                        producePlanId = x.ProducePlanId,
                        productionPlanNo = x.ProductionPlanNo,
                        productionPlanStartDate = x.ProductionPlanStartDate.ToString("yyyy-MM-dd"),
                        productionPlanEndDate = x.ProductionPlanEndDate.ToString("yyyy-MM-dd"),
                        productionPlanProductCount = x.ProducePlanProducts.Where(y=>y.IsDeleted == 0).Count(),
                        productionPlanTotalAmount = x.ProducePlanProducts.Count() == 0? 0 : x.ProducePlanProducts.Where(y=>y.IsDeleted !=1).Select(y=>y.ProductPlanQuantity).Sum(),
                        productionPlanStatus = x.ProductionPlanStatus,

                        registerDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        registerName = x.Register.FullName,
                        uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                        uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",
                        productionPlanMemo = x.ProductionPlanMemo,
                    }).ToArrayAsync();

                var Res = new Response<IEnumerable<ProducePlanReponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanReponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 생산계획목록 조회
        //생산계획 팝업
        public async Task<Response<IEnumerable<ProducePlanPopupResponse>>> GetProducePlansPopup(ProducePlanPopupRequest _req)
        {
            try
            {
                var _res = await _db.ProducePlanProducts
                    .Include(x => x.ProducePlansProcesses).ThenInclude(x=>x.ProductProcess)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProducePlan.IsDeleted == 0)
                    .Where(x => x.ProducePlan.ProductionPlanNo.Contains(_req.searchInput) || x.Product.Code.Contains(_req.searchInput))
                    .Select(x => new ProducePlanPopupResponse
                    {
                        ProducePlanId = x.ProducePlanId,
                        ProductPlansProductId = x.ProducePlansProductId,
                        ProductionPlanNo = x.ProducePlan.ProductionPlanNo,
                        ProductionPlanStatus = x.ProducePlan.ProductionPlanStatus,
                        Priority = x.Priority,
                        ProductionPlanDate = x.ProductionPlanDate.ToString("yyyy-MM-dd"),
                        ProductId = x.Product.Id,
                        ProductCode = x.Product.Code,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductName = x.Product.Name,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,
                        ProductionPlanQuantity = x.ProductPlanQuantity,
                        ProductPlanBacklog = x.ProductPlanQuantity - _db.WorkerOrders
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProducePlansProduct.ProducePlansProductId == x.ProducePlansProductId)
                            .Sum(y=>y.ProductWorkQuantity),
                        ProductionPlanMemo = x.ProducePlan.ProductionPlanMemo,
                        WorkerOrderProducePlans = x.ProducePlansProcesses
                            .Where(y=>y.IsDeleted == 0)
                            .Select(y=> new ProducePlanProcessInterface002
                            {
                                ProductId = y.ProductProcess.ProduceProductId,
                                ProductProcessId = y.ProductProcess.ProductProcessId,
                                ProducePlansProcessId = y.ProducePlansProcessId,
                                ProcessOrder = y.ProductProcess.ProcessOrder,
                                ProcessCode = y.ProductProcess.Process.Code,
                                ProcessName = y.ProductProcess.Process.Name,
                                ProcessPlanQuantity = y.ProcessPlanQuantity,
                                ProcessPlanBacklog = y.ProcessPlanQuantity - _db.ProcessProgresses.Where(z=>z.IsDeleted == 0).Where(z=>z.WorkOrderProducePlan.ProducePlansProcess.ProducePlansProcessId == y.ProducePlansProcessId).Select(z=>z.ProductionQuantity).Sum(),
                                ProcessCheck = true,
                                ProcessCheckResult = "미검사",
                                FacilityId = 0,
                                MoldId = 0,
                                WorkerId = "",
                                PartnerId = 0,
                                ProcessWorkQuantity = y.ProcessPlanQuantity,
                            }).OrderBy(y=>y.ProcessOrder).ToList(),
                        /*
                        ProducePlansProcesses = x.ProducePlansProcesses
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => new ProducePlanProcessInterface002
                            {
                                ProductProcessId = y.ProductProcess.ProductProcessId,
                                ProducePlansProcessId = y.ProducePlansProcessId,
                                ProcessOrder = y.ProductProcess.ProcessOrder,
                                ProcessCode = y.ProductProcess.Process.Code,
                                ProcessName = y.ProductProcess.Process.Name,
                                ProcessPlanQuantity = y.ProcessPlanQuantity,
                                ProcessPlanBacklog = y.ProcessPlanQuantity - _db.ProcessProgresses.Where(z => z.IsDeleted == 0).Where(z => z.WorkOrderProducePlan.ProducePlansProcess.ProducePlansProcessId == y.ProducePlansProcessId).Select(z => z.ProductionQuantity).Sum(),

                            }).OrderBy(y => y.ProcessOrder).ToList(),
                        */
                    })
                    .OrderBy(x=> x.ProducePlanId)
                    .ToArrayAsync();

                var res = _res.Where(x => _req.productionPlanStatus =="ALL"? true : x.ProductionPlanStatus == _req.productionPlanStatus);

                var Res = new Response<IEnumerable<ProducePlanPopupResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanPopupResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        public async Task<Response<IEnumerable<ProducePlanDetailResponse>>> GetProducePlansProducts(ProducePlanRequest002 param)
        {
            try
            {
                var res = await _db.ProducePlans
                    .Where(x=> x.ProducePlanId == param.producePlanId)

                    .Select(x => new ProducePlanDetailResponse
                    {
                        ProducePlanId = x.ProducePlanId,
                        ProductionPlanNo = x.ProductionPlanNo,
                        ProductionPlanStartDate = x.ProductionPlanStartDate.ToString("yyyy-MM-dd"),
                        ProductionPlanEndDate = x.ProductionPlanEndDate.ToString("yyyy-MM-dd"),
                        RegisterDate = x.RegisterDate.ToString("yyyy-MM-dd"),
                        ProductionPlanStatus = x.ProductionPlanStatus,
                        RegisterName = x.Register.FullName,

                        UserEmail = x.Register.Email,
                        UserPhoneNumber = x.Register.PhoneNumber,
                       // uploadFileUrl = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileUrl : "",
                      //  uploadFileName = x.UploadFiles.Count() > 0 ? x.UploadFiles.FirstOrDefault().FileName : "",

                        UploadFiles = x.UploadFiles.ToList(),
                        ProductionPlanMemo = x.ProductionPlanMemo,

                        ProducePlanProducts = x.ProducePlanProducts
                            .Where(y=> y.IsDeleted == 0)
                            .OrderBy(y=>y.Priority)
                            .Select(y => new ProducePlanProductInterface
                            {
                                producePlansProductId = y.ProducePlansProductId,
                                priority = y.Priority,
                                productionPlanDate = y.ProductionPlanDate.ToString("yyyy-MM-dd"),
                                productId = y.Product.Id,
                                productCode = y.Product.Code,
                                productName = y.Product.Name,
                                productStandard = y.Product.Standard,
                                productUnit = y.Product.Unit,
                                productOrderCount = _db.OrderProducts
                                    .Where(z=>z.OrderProductId == y.OrderProduct.OrderProductId).Select(z=>z.ProductOrderCount).FirstOrDefault(),
                                optimumStock = y.Product.OptimumStock,
                                inventory = _db.LotCounts.Where(z => z.Product.Id == y.Product.Id).Where(z => z.IsDeleted == 0).Select(z => (0 - z.DefectiveCount -z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                                productPlanQuantity = y.ProductPlanQuantity,
                                ProductClassification = y.Product.CommonCode.Name,
                                producePlanProcesses = y.ProducePlansProcesses
                                    .Where(z=>z.IsDeleted == 0)
                                    .Select(z=> new ProducePlanProcessInterface
                                    {
                                        Checked = false,
                                        producePlansProductId = y.ProducePlansProductId,
                                        producePlansProcessId = z.ProducePlansProcessId,
                                        productProcessId = z.ProductProcess.ProductProcessId,
                                        processId = z.ProductProcess.ProcessId,
                                        processCode = z.ProductProcess.Process.Code,
                                        processName = z.ProductProcess.Process.Name,
                                        processOrder = z.ProductProcess.ProcessOrder,
                                        productCode = _db.Products.Where(k => k.Id == z.ProductProcess.ProduceProductId).Select(k => k.Code).FirstOrDefault(),
                                        productClassification = _db.Products.Where(k => k.Id == z.ProductProcess.ProduceProductId).Select(k => k.CommonCode.Name).FirstOrDefault(),
                                        processPlanQuantity = z.ProcessPlanQuantity,
                                        productName = _db.Products.Where(k=>k.Id == z.ProductProcess.ProduceProductId).Select(k=>k.Name).FirstOrDefault(),
                                        productUnit = _db.Products.Where(k => k.Id == z.ProductProcess.ProduceProductId).Select(k => k.Unit).FirstOrDefault(),
                                        productStandard = _db.Products.Where(k => k.Id == z.ProductProcess.ProduceProductId).Select(k => k.Standard).FirstOrDefault(),
                                        inventory = _db.LotCounts.Where(k => k.Product.Id == z.ProductProcess.ProduceProductId).Where(z=>z.IsDeleted == 0).Select(k => (0 - k.DefectiveCount - k.ConsumeCount - k.OutOrderCount + k.StoreOutCount + k.ProduceCount + k.ModifyCount)).Sum(),

                                        ProcessInputItems = z.ProductProcess.Items
                                        .Where(j => j.IsDeleted == 0)
                                        .Select(j => new ProductItemInterface2
                                        {
                                            ProcessId = j.ProcessId,
                                            ProcessCode = _db.Processes.Where(k => k.Id == j.ProcessId).Select(k => k.Code).FirstOrDefault(),
                                            ProcessName = _db.Processes.Where(k => k.Id == j.ProcessId).Select(k => k.Name).FirstOrDefault(),
    
                                            ItemId = j.ProductId,
                                            ItemCode = j.Product.Code,
                                            ItemName = j.Product.Name,
                                            ItemClassification = j.Product.CommonCode.Name,
                                            ItemStandard = j.Product.Standard,
                                            ItemUnit = j.Product.Unit,
                                            Loss = (float)j.Loss,
                                            ProcessPlanQuantity = 0,
                                            RequiredQuantity = (float)j.Require,
                                            TotalRequiredQuantity = (float)(j.Loss * j.Require),
                                            TotalInputQuantity = 0,
                                            Inventory = _db.LotCounts.Where(k => k.Product.Id == j.Product.Id).Where(k => k.IsDeleted == 0).Select(k => (0 - k.DefectiveCount - k.ConsumeCount - k.OutOrderCount + k.StoreOutCount + k.ProduceCount + k.ModifyCount)).Sum(),
                                        }).ToList()

                                    }).OrderBy(z=>z.processOrder).ToList()
                            }).ToList()

                    })
                    .ToArrayAsync();

 
                var Res = new Response<IEnumerable<ProducePlanDetailResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanDetailResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #region 생산계획 등록
        public async Task<Response<IEnumerable<ProducePlanReponse002>>> CreateProducePlan(ProducePlanRequest003 param)
        {
            try
            {
                //생산계획번호규칙 : PP+날짜(YYYYMMDD)+순서(4자리), 예시) PP202204220001
                var _producePlanNo = await _db.ProducePlans.Where(x=> x.RegisterDate == Convert.ToDateTime(param.registerDate)).OrderByDescending(x => x.ProductionPlanNo).FirstOrDefaultAsync();
                int increase = 1;
                if (_producePlanNo != null)
                {
                    increase = Int32.Parse(_producePlanNo.ProductionPlanNo.Substring(10, 4)) + 1;
                }
                var date = Convert.ToDateTime(param.registerDate).ToString("yyyyMMdd");
                var _newProducePlanNo = $"PP{date}{increase:000#}";

                var _user = await _userManager.FindByIdAsync(param.registerId);

                var _newProducePlan = new ProducePlan
                {
                    ProductionPlanNo = _newProducePlanNo,
                    ProductionPlanStartDate = Convert.ToDateTime(param.productionPlanStartDate),//생산계획시작일
                    ProductionPlanEndDate = Convert.ToDateTime(param.productionPlanEndDate),//생산계획종료일
                    RegisterDate = Convert.ToDateTime(param.registerDate),//등록일
                    Register = _user,//등록자
                    ProductionPlanMemo = param.productionPlanMemo,//비고
                    UploadFiles = param.UploadFiles,//첨부파일
                    ProducePlanProducts = null,
                    IsDeleted = 0,
                    ProductionPlanStatus = "계획등록"
                };

                await _db.ProducePlans.AddAsync(_newProducePlan);
                await Save();


                var _producePlan = await _db.ProducePlans.Where(y => y.ProductionPlanNo == _newProducePlanNo).FirstOrDefaultAsync();
                var _newProducePlanProducts = new List<ProducePlansProduct>();


                if (param.producePlanProducts != null)
                {
                                    
                    foreach (var i in param.producePlanProducts)
                    {
                        var _producePlanProduct = new ProducePlansProduct
                        {
                            ProducePlan = _producePlan,
                            OrderProduct = _db.OrderProducts.Where(z => z.OrderProductId == i.orderProductId).FirstOrDefault(), //수주정보추가 
                            Priority = i.priority,//우선순위
                            ProductionPlanDate = Convert.ToDateTime(i.productionPlanDate),//계획일자
                            ProductPlanQuantity = i.productPlanQuantity, 
                            Product = _db.Products.Where(z=>z.Id ==i.productId).FirstOrDefault(),  
                        };
                        
                        List<ProducePlansProcess> _newProducePlanProcesses = new List< ProducePlansProcess > ();
                        foreach(var j in i.producePlanProcesses)
                        {
                            var _producePlanProcess = new ProducePlansProcess
                            {
                                IsDeleted = 0,
                                ProcessPlanQuantity = j.processPlanQuantity,
                                ProductProcess = _db.ProductProcesses.Where(x => x.ProductProcessId == j.productProcessId).FirstOrDefault(),
                                ProductProcessId = j.productProcessId
                            };
                            _newProducePlanProcesses.Add(_producePlanProcess);
                        }
                        _producePlanProduct.ProducePlansProcesses = _newProducePlanProcesses;
                        _newProducePlanProducts.Add(_producePlanProduct);
                    }
                }

                _producePlan.ProducePlanProducts = _newProducePlanProducts;
                _db.ProducePlans.Update(_producePlan);

                await Save();





                var Res = new Response<IEnumerable<ProducePlanReponse002>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanReponse002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 생산계획등록

        #region 생산계획 업데이트
        public async Task<Response<IEnumerable<ProducePlanReponse002>>> UpdateProducePlan(ProducePlanRequest003 param)
        {
            try
            {
                var _producePlan = await _db.ProducePlans
                    .Include(x=>x.ProducePlanProducts)
                    .ThenInclude(x => x.ProducePlansProcesses)
                    .Include(x => x.UploadFiles)
                    .Where(y => y.ProducePlanId == param.producePlanId).FirstOrDefaultAsync();

                _producePlan.ProductionPlanStartDate = Convert.ToDateTime(param.productionPlanStartDate);
                _producePlan.ProductionPlanEndDate = Convert.ToDateTime(param.productionPlanEndDate);
                _producePlan.RegisterDate = Convert.ToDateTime(param.registerDate);

                var _user = await _userManager.FindByIdAsync(param.registerId);
                _producePlan.Register = _user;
                _producePlan.ProductionPlanMemo = param.productionPlanMemo;

                if (_producePlan.UploadFiles != null)
                {
                    _db.UploadFiles.RemoveRange(_producePlan.UploadFiles);
                }

                _producePlan.UploadFiles = param.UploadFiles;

                _db.ProducePlans.Update(_producePlan);
                await Save();


                bool checkFlag = false;
                bool checkFlag2 = false;


                List<int> _deleteProducePlanProducts = new List<int>();
                List<ProducePlanProductInterface> _updateProducePlanProducts = new List<ProducePlanProductInterface>();
                List<ProducePlanProductInterface> _createProducePlanProducts = new List<ProducePlanProductInterface>();

                List<int> _deleteProductPlanProcesses = new List<int>();
                List<ProducePlanProcessInterface> _updateProductPlanProcesses = new List<ProducePlanProcessInterface>();


                foreach (var _orig in _producePlan.ProducePlanProducts)
                {
                    checkFlag = false;
                    checkFlag2 = false;
                    if (param.producePlanProducts!=null)
                    {
                        foreach (var _prod in param.producePlanProducts)
                        {
                            string x = _prod.producePlansProductId.ToString();

                            if (x.Contains("S"))
                                x = "0";

                            
                            if (Convert.ToInt32(x) == _orig.ProducePlansProductId)
                            {
                                _updateProducePlanProducts.Add(_prod);
                                var temp = await _db.ProducePlanProducts.Where(x => x.ProducePlansProductId == _orig.ProducePlansProductId).FirstOrDefaultAsync();

                                temp.Priority = _prod.priority;
                                temp.ProductPlanQuantity = _prod.productPlanQuantity;
                                temp.ProductionPlanDate = Convert.ToDateTime(_prod.productionPlanDate);

                                _db.ProducePlanProducts.Update(temp);

                                checkFlag = true;

                                foreach (var _origProc in _orig.ProducePlansProcesses)
                                {
                                    if(_prod.producePlanProcesses != null)
                                    {
                                        foreach (var _proc in _prod.producePlanProcesses)
                                        {
                                            if (_proc.producePlansProcessId == _origProc.ProducePlansProcessId)
                                            {
                                                _updateProductPlanProcesses.Add(_proc);
                                                checkFlag2 = true;

                                                var temp1 = await _db.ProducePlanProcesses.Where(x => x.ProducePlansProcessId == _origProc.ProducePlansProcessId).FirstOrDefaultAsync();
                                                temp1.ProcessPlanQuantity = _proc.processPlanQuantity;
                                                _db.ProducePlanProcesses.Update(temp1);
                                                await Save();
                                            }
                                        }

                                        if (!checkFlag2)
                                        {
                                            _deleteProductPlanProcesses.Add(_origProc.ProducePlansProcessId);
                                            var temp2 = await _db.ProducePlanProcesses.Where(x => x.ProducePlansProcessId == _origProc.ProducePlansProcessId).FirstOrDefaultAsync();
                                            temp2.IsDeleted = 1;
                                            _db.ProducePlanProcesses.Update(temp2);
                                            await Save();

                                        }
                                        checkFlag2 = false;
                                    }
                                }
                            }
                        }
                        if (!checkFlag)
                        {
                            _deleteProducePlanProducts.Add(_orig.ProducePlansProductId);
                            var temp3 = await _db.ProducePlanProducts.Where(x => x.ProducePlansProductId == _orig.ProducePlansProductId).FirstOrDefaultAsync();
                            temp3.IsDeleted = 1;
                            _db.ProducePlanProducts.Update(temp3);
                            await Save();
                        }

                        checkFlag = false;
                    }
             
                }

                if (param.producePlanProducts !=null)
                {
                    foreach (var _prod in param.producePlanProducts)
                    {
                        string x = _prod.producePlansProductId.ToString();

                        if (x.Contains("S"))
                            x = "0";

                        if (Convert.ToInt32(x) == 0)
                        {
                            _createProducePlanProducts.Add(_prod);

                            var _producePlanProduct = new ProducePlansProduct
                            {
                                ProducePlanId = _producePlan.ProducePlanId,
                                ProducePlan = _producePlan,
                                OrderProduct = _db.OrderProducts.Where(z => z.OrderProductId == _prod.orderProductId).FirstOrDefault(), //수주정보추가 
                                Priority = _prod.priority,//우선순위
                                ProductionPlanDate = Convert.ToDateTime(_prod.productionPlanDate),//계획일자
                                ProductPlanQuantity = _prod.productPlanQuantity,
                                Product = _db.Products.Where(z => z.Id == _prod.productId).FirstOrDefault(),
                            };


                            await _db.ProducePlanProducts.AddAsync(_producePlanProduct);
                            await Save();

                            var temp = await _db.ProducePlanProducts.OrderByDescending(x => x.ProducePlansProductId).FirstOrDefaultAsync();

                            if (_prod.producePlanProcesses != null)
                            {
                                foreach (var j in _prod.producePlanProcesses)
                                {
                                    var _producePlanProcess = new ProducePlansProcess
                                    {
                                        ProducePlansProductId = temp.ProducePlansProductId,
                                        IsDeleted = 0,
                                        ProcessPlanQuantity = j.processPlanQuantity,
                                        ProductProcess = _db.ProductProcesses.Where(x => x.ProductProcessId == j.productProcessId).FirstOrDefault(),
                                    };

                                    await _db.ProducePlanProcesses.AddAsync(_producePlanProcess);
                                    await Save();
                                }
                            }

                        }
                    }

                }



                var Res = new Response<IEnumerable<ProducePlanReponse002>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;

   
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanReponse002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 생산계획업데이트

        #region 생산계획 삭제
        public async Task<Response<IEnumerable<ProducePlanReponse002>>> DeleteProducePlan(ProducePlanRequest003 param)
        {
            try
            {

                foreach(int id in param.producePlanIds)
                {
                    var _producePlan = await _db.ProducePlans.Where(y => y.ProducePlanId == id).FirstOrDefaultAsync();

                    if (_producePlan != null)
                    {
                        var _deleteProducePlanProducts = await _db.ProducePlanProducts.Where(x => x.ProducePlan.ProducePlanId == id).ToArrayAsync();
                        if (_deleteProducePlanProducts != null)
                        {
                            foreach (var producePlanProduct in _deleteProducePlanProducts)
                            {
                                var _deleteProducePlanProcesses = await _db.ProducePlanProcesses.Where(x => x.ProducePlansProduct.ProducePlansProductId == producePlanProduct.ProducePlansProductId).ToArrayAsync();
                                if (_deleteProducePlanProcesses != null)
                                {
                                    foreach (var producePlanProcess in _deleteProducePlanProcesses)
                                    {
                                        producePlanProcess.IsDeleted = 1;
                                        _db.ProducePlanProcesses.Update(producePlanProcess);
                                    }
                                }

                                producePlanProduct.IsDeleted = 1;
                                _db.ProducePlanProducts.Update(producePlanProduct);
                            }
                        }
                        _producePlan.IsDeleted = 1;

                        _db.ProducePlans.Update(_producePlan);
                        await Save();
                    }
                }

                var Res = new Response<IEnumerable<ProducePlanReponse002>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanReponse002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 생산계획 삭제

        public async Task<Response<IEnumerable<ProducePlanReponse004>>> GetProducePlanProcesses(ProducePlanRequest004 req)
        {
            try
            {

                var res = await _db.ProducePlanProcesses
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProducePlansProduct.ProducePlansProductId == req.producePlansProductId)
                    .Select(x => new ProducePlanReponse004
                    {
                        producePlansProcessId = x.ProducePlansProcessId,

                        producePlansProductId = x.ProducePlansProduct.ProducePlansProductId,
                        productProcessId = x.ProductProcess.ProductProcessId,

                        processCode = x.ProductProcess.Process.Code,
                        processName = x.ProductProcess.Process.Name,
                        processOrder = x.ProductProcess.ProcessOrder,
                        processPlanQuantity = x.ProcessPlanQuantity,
                        productCode = x.ProductProcess.Product.Code,
                        productName = x.ProductProcess.Product.Name,
                        productStandard = x.ProductProcess.Product.Standard,
                        productUnit = x.ProductProcess.Product.Unit,
                        productClassification = x.ProductProcess.Product.CommonCode.Name,
                        inventory = _db.LotCounts.Where(z => z.Product.Id == x.ProductProcess.ProductId).Where(y=>y.IsDeleted == 0).Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                    })
                    .ToArrayAsync();

                var Res = new Response<IEnumerable<ProducePlanReponse004>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanReponse004>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        //공정등록
        public async Task<Response<IEnumerable<ProducePlanReponse004>>> CreateProducePlanProcesses(ProducePlanRequest004 param)
        {
            try
            {

                var _query = _db.ProductProcesses;
                var result = _query.ToQueryString();

                var _producePlanProducts = await _db.ProducePlanProducts.Where(y => y.ProducePlansProductId == param.producePlansProductId).FirstOrDefaultAsync();

                var _newProducePlanProcesses = new List<ProducePlansProcess>();
                if (param.ProducePlanProcesses != null)
                {
                    foreach (var i in param.ProducePlanProcesses)
                    {
                        var _producePlanProcess = new ProducePlansProcess
                        {
                            ProducePlansProduct = _producePlanProducts,
                            ProductProcess = _db.ProductProcesses.Where(z => z.ProductProcessId == i.productProcessId).FirstOrDefault(),
                            ProcessPlanQuantity = i.processPlanQuantity,//생산계획수량
                            IsDeleted = 0
                        };

                        _newProducePlanProcesses.Add(_producePlanProcess);
                    }

                    _producePlanProducts.ProducePlansProcesses = _newProducePlanProcesses;
                    _db.ProducePlanProducts.Update(_producePlanProducts);
                    await Save();
                }

                //var _producePlanProcesses = await _db.ProducePlanProcesses
                //    .Where(x => x.ProducePlansProduct.ProducePlansProductId == _producePlanProducts.ProducePlansProductId) //==param.producePlansProductId                    
                //    .Select(x => new ProducePlanReponse004
                //    {
                //        producePlansProductId = x.ProducePlansProduct.ProducePlansProductId,
                //        producePlansProcessId = x.ProducePlansProcessId,

                //        processOrder = x.ProductProcess.ProcessOrder,//공정순서
                //        processCode = x.ProductProcess.Process.Code,//공정코드
                //        processName = x.ProductProcess.Process.Name,//공정이름
                //        productCode = x.ProductProcess.Product.Code,//품목코드
                //        productClassification = x.ProductProcess.Product.CommonCode.Name,//품목구분
                //        productName = x.ProductProcess.Product.Name,//품목이름
                //        productStandard = x.ProductProcess.Product.Standard,//규격
                //        productUnit = x.ProductProcess.Product.Unit,//단위
                //        inventory = -1,//재고수량,미정
                //        processPlanQuantity = x.ProcessPlanQuantity,//생산계획수량
                //    }).ToListAsync();

                var Res = new Response<IEnumerable<ProducePlanReponse004>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanReponse004>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        //공정수정-안씀
        public async Task<Response<IEnumerable<ProducePlanReponse004>>> UpdateProducePlanProcesses(ProducePlanRequest004 param)
        {
            try
            {
                var _producePlanProducts = await _db.ProducePlanProducts.Where(y => y.ProducePlansProductId == param.producePlansProductId).FirstOrDefaultAsync();

                var _deleteProducePlanProcesses = await _db.ProducePlanProcesses.Where(x => x.ProducePlansProduct.ProducePlansProductId == param.producePlansProductId).ToArrayAsync();
                if (_deleteProducePlanProcesses != null)
                {
                    foreach (var producePlanProcess in _deleteProducePlanProcesses)
                    {
                        producePlanProcess.IsDeleted = 1;
                        _db.ProducePlanProcesses.Update(producePlanProcess);
                    }
                }

                var _updateProducePlanProcesses = new List<ProducePlansProcess>();
                if (param.ProducePlanProcesses != null)
                {
                    foreach (var i in param.ProducePlanProcesses)
                    {
                        var _producePlanProcess = new ProducePlansProcess
                        {
                            ProducePlansProduct = _producePlanProducts,
                            ProductProcess = _db.ProductProcesses.Where(z => z.ProductProcessId == i.productProcessId).FirstOrDefault(),
                            ProcessPlanQuantity = i.processPlanQuantity,//생산계획수량
                        };

                        _updateProducePlanProcesses.Add(_producePlanProcess);
                    }
                }

                _producePlanProducts.ProducePlansProcesses = _updateProducePlanProcesses;
                _db.ProducePlanProducts.Update(_producePlanProducts);
                await Save();

                //if (param.ProducePlanProcesses != null)
                //{
                //    foreach (var i in param.ProducePlanProcesses)
                //    {
                //        var producePlanProcess = await _db.ProducePlanProcesses.Where(x => x.ProducePlansProcessId == i.productProcessId).FirstOrDefaultAsync();
                //        producePlanProcess.IsDeleted = 1;
                //        _db.ProducePlanProcesses.Update(producePlanProcess);
                //    }
                //    await Save();
                //}

                var _producePlanProcesses = await _db.ProducePlanProcesses
                    .Where(x => x.ProducePlansProduct.ProducePlansProductId == _producePlanProducts.ProducePlansProductId)  //==param.producePlansProductId
                    .Select(x => new ProducePlanReponse004
                    {
                        producePlansProductId = x.ProducePlansProduct.ProducePlansProductId,
                        producePlansProcessId = x.ProducePlansProcessId,

                        processOrder = x.ProductProcess.ProcessOrder,//공정순서
                        processCode = x.ProductProcess.Process.Code,//공정코드
                        processName = x.ProductProcess.Process.Name,//공정이름
                        productCode = x.ProductProcess.Product.Code,//품목코드
                        productClassification = x.ProductProcess.Product.CommonCode.Name,//품목구분
                        productName = x.ProductProcess.Product.Name,//품목이름
                        productStandard = x.ProductProcess.Product.Standard,//규격
                        productUnit = x.ProductProcess.Product.Unit,//단위
                        inventory = -1,//재고수량,미정
                        processPlanQuantity = x.ProcessPlanQuantity,//생산계획수량
                    }).ToListAsync();

                var Res = new Response<IEnumerable<ProducePlanReponse004>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _producePlanProcesses
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanReponse004>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        //공정삭제
        public async Task<Response<IEnumerable<ProducePlanReponse004>>> DeleteProducePlanProcesses(ProducePlanRequest004 param)
        {
            try
            {
                if (param.ProducePlanProcesses != null)
                {
                    foreach (var i in param.ProducePlanProcesses)
                    {
                        var producePlanProcess = await _db.ProducePlanProcesses.Where(x => x.ProducePlansProcessId == i.producePlansProcessId).FirstOrDefaultAsync();
                        producePlanProcess.IsDeleted = 1;
                        _db.ProducePlanProcesses.Update(producePlanProcess);
                    }
                    await Save();
                }

                var Res = new Response<IEnumerable<ProducePlanReponse004>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = null
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanReponse004>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        // 제품상세내역 - 공정 및 투입품목 조회 


        //제품상세내역
        public async Task<Response<ProducePlanProductDetailResponse>> GetProducePlanProductDetail(ProducePlanProduceDetailRequest req)
        {
            try { 

                //신규 생성시 조회
                if(req.ProducePlansProductId == 0)
                {
                    var res1 = await _db.Products
                        .Where(x => x.Id == req.ProductId)
                        .Select(x => new ProducePlanProductDetailResponse
                        {
                            ProducePlansProductId = 0,
                            
                            ProducePlanProcesses = x.Processes
                                .Where(y=>y.IsDeleted == 0)
                                .Select(y => new ProducePlanProcessInterface
                                {
                                    producePlansProductId = 0,
                                    producePlansProcessId = 0,
                                    productProcessId = y.ProductProcessId,
                                    processCode = y.Process.Code,
                                    processName = y.Process.Name,
                                    processOrder = y.ProcessOrder,

                                    productCode = _db.Products.Where(z=>z.Id == y.ProduceProductId).Select(z=>z.Code).FirstOrDefault(),
                                    productClassification = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.CommonCode.Code).FirstOrDefault(), 
                                    processPlanQuantity = 0,
                                    productName = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Name).FirstOrDefault(),
                                    productUnit = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Unit).FirstOrDefault(),
                                    productStandard = _db.Products.Where(z => z.Id == y.ProduceProductId).Select(z => z.Standard).FirstOrDefault(),
                                   
                                    inventory = _db.LotCounts
                                        .Where(z => z.Product.Id == y.ProduceProductId)
                                        .Where(z => z.IsDeleted == 0)
                                        .Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount))
                                        .Sum(),
                                })
                                .OrderBy(y=>y.processOrder)
                                .ToList(),

                            ProducePlanProcessItems = _db.ProductItems
                                    .Where(y => y.IsDeleted == 0)
                                    .Where(y => x.Processes.Where(z => z.IsDeleted == 0).Select(z => z.ProductProcessId).Contains(y.ProductProcessId) ? true : false)
                                    .Select(y => new ProducePlanProcessItemInterface
                                    {
                                        processCode = y.ProductProcess.Process.Code,
                                        processName = y.ProductProcess.Process.Name,
                                        itemCode = y.Product.Code,
                                        itemClassification = y.Product.CommonCode.Name,
                                        itemName = y.Product.Name,
                                        itemStandard = y.Product.Standard,
                                        itemUnit = y.Product.Unit,
                                        requiredQuantity = (float)y.Require,
                                        LOSS = (float)y.Loss,
                                        totalRequiredQuantity = 0,
                                        processPlanQuantity = 0,
                                        totalInputQuantity = 0,
                                        inventory = _db.LotCounts
                                            .Where(z => z.Product.Id == y.Product.Id)
                                            .Where(z => z.IsDeleted == 0)
                                            .Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount))
                                            .Sum(),
                                   
                                    
                                    })
                                    .ToList()


                        }).FirstOrDefaultAsync();

                    var Res1 = new Response<ProducePlanProductDetailResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res1,
                    };

                    return Res1;
                }

                //신규생성시에는 ProducePlanProductID와 같이 조회.
                else
                {
                    var res = await _db.ProducePlanProducts
                        .Where(x => x.IsDeleted == 0)
                        .Where(x => x.ProducePlansProductId == req.ProducePlansProductId)
                        .Select(x => new ProducePlanProductDetailResponse
                        {
                            ProducePlansProductId = x.ProducePlansProductId,
                            ProducePlanProcesses = _db.ProducePlanProcesses
                                .Where(y=> y.ProducePlansProductId == x.ProducePlansProductId)
                                .Where(y => y.IsDeleted == 0)
                                .Select(y => new ProducePlanProcessInterface
                                {
                                    producePlansProductId = y.ProducePlansProductId,
                                    producePlansProcessId = y.ProducePlansProcessId,
                                    productProcessId = y.ProductProcess.ProductProcessId,
                                    processCode = y.ProductProcess.Process.Code,
                                    processName = y.ProductProcess.Process.Name,
                                    processOrder = y.ProductProcess.ProcessOrder,
                                    processPlanQuantity = y.ProcessPlanQuantity,

                                    productCode = _db.Products.Where(z => z.Id == y.ProductProcess.ProduceProductId).Select(z => z.Code).FirstOrDefault(),
                                    productClassification = _db.Products.Where(z => z.Id == y.ProductProcess.ProduceProductId).Select(z => z.CommonCode.Code).FirstOrDefault(),
                                    productName = _db.Products.Where(z => z.Id == y.ProductProcess.ProduceProductId).Select(z => z.Name).FirstOrDefault(),
                                    productUnit = _db.Products.Where(z => z.Id == y.ProductProcess.ProduceProductId).Select(z => z.Unit).FirstOrDefault(),
                                    productStandard = _db.Products.Where(z => z.Id == y.ProductProcess.ProduceProductId).Select(z => z.Standard).FirstOrDefault(),

                                    inventory = _db.LotCounts
                                        .Where(z => z.IsDeleted == 0)
                                        .Where(z => z.Product.Id == y.ProductProcess.Product.Id)
                                        .Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount))
                                        .Sum(),

                                })
                                .OrderBy(y=>y.processOrder)
                                .ToList(),

                            ProducePlanProcessItems = _db.ProductItems
                                .Where(y => y.IsDeleted == 0)
                                .Where(y => x.ProducePlansProcesses.Where(z => z.IsDeleted == 0).Select(z => z.ProductProcess.ProductProcessId).Contains(y.ProductProcessId) ? true : false)
                                .Select(y => new ProducePlanProcessItemInterface
                                {
                                    processCode = y.ProductProcess.Process.Code,
                                    processName = y.ProductProcess.Process.Name,
                                    itemCode = y.Product.Code,
                                    itemClassification = y.Product.CommonCode.Name,
                                    itemName = y.Product.Name,
                                    itemStandard = y.Product.Standard,
                                    itemUnit = y.Product.Unit,
                                    requiredQuantity = (float)y.Require,
                                    LOSS = (float)y.Loss,
                                    totalRequiredQuantity = 0,
                                    processPlanQuantity = 0,
                                    totalInputQuantity = 0,
                                    inventory = _db.LotCounts
                                        .Where(z => z.Product.Id == x.Product.Id)
                                        .Where(z => z.IsDeleted == 0)
                                        .Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount))
                                        .Sum(),
                                }).ToList()
                        }).FirstOrDefaultAsync();

                    var Res = new Response<ProducePlanProductDetailResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = res,
                    };

                    return Res;
                }
            }
            catch (Exception ex)
            {
                var Res = new Response<ProducePlanProductDetailResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }


        // 투입품목 조회 
        public async Task<Response<IEnumerable<ProducePlanReponse005>>> GetProductItems(ProducePlanRequest005 param)
        {
            try
            {
                var res = await _db.ProductItems
                    .Where(x => x.ProductProcessId == param.producePlansProductId)
                    .Select(x => new ProducePlanReponse005
                    {
                        processCode = x.ProductProcess.Process.Code,
                        processName = x.ProductProcess.Process.Name,
                        itemCode = x.Product.Code,
                        itemClassification = x.Product.CommonCode.Name,
                        itemName = x.Product.Name,
                        itemStandard = x.Product.Standard,
                        itemUnit = x.Product.Unit,
                        requiredQuantity = (float)x.Require,
                        LOSS = (float)x.Loss,
                        totalRequiredQuantity = 0,
                        processPlanQuantity = 0,
                        totalInputQuantity = 0,
                        inventory = _db.LotCounts.Where(z => z.Product.Id == x.Product.Id).Select(z => (0 - z.DefectiveCount -z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                    
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<ProducePlanReponse005>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res,
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProducePlanReponse005>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
 
        #endregion

        #region 2) 소요량산출
        public async Task<Response<GetRequiredAmountsResponse002>> GetRequiredAmounts(GetRequiredAmountsRequest001 param)
        {
            try
            {
                var res2 = await _db.ProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.ProducePlanId == param.producePlanId)
                    .Select(x => new 
                    {
                        producePlanId = x.ProducePlanId,
                        ProducePlanProducts = x.ProducePlanProducts
                            .Where(y => y.IsDeleted == 0)
                            .Select(y => new 
                            {
                                producePlansProductId = y.ProducePlansProductId,
                                productCode = y.Product.Code,
                                productName = y.Product.Name,
                                productStandard = y.Product.Standard,
                                productUnit = y.Product.Unit,
                                productPlanQuantity = y.ProductPlanQuantity,
                                ProductClassification = y.Product.CommonCode.Name,

                                ProducePlanProcesses = y.ProducePlansProcesses
                                    .Where(z=>z.IsDeleted == 0)
                                    .Select(z=> z.ProductProcess.Items
                                        .Where(k=>k.IsDeleted == 0)
                                        .Select(k=> new 
                                        {
                                            itemId = k.Product.Id,
                                            itemCode = k.Product.Code,
                                            itemClassification = k.Product.CommonCode.Name,
                                            itemName = k.Product.Name,
                                            itemStandard = k.Product.Standard,
                                            itemUnit = k.Product.Unit,
                                            requiredQuantity = k.Require,
                                            loss = k.Loss,
                                            totalInputQuantity = z.ProcessPlanQuantity * (k.Require + k.Loss),
                                          //  inventory = _db.LotCounts.Where(z => z.Product.Id == y.Product.Id).Select(z => (0 - z.DefectiveCount - z.ConsumeCount - z.OutOrderCount + z.StoreOutCount + z.ProduceCount + z.ModifyCount)).Sum(),
                                       
                                        }).ToList()
                                    ).ToList(),
                            }).ToList(),
                    })
                    .ToArrayAsync();

                GetRequiredAmountsResponse002 res = new GetRequiredAmountsResponse002();

                foreach (var orig in res2)
                {
                    var parent = new GetRequiredAmountsResponse002
                    {
                        producePlanId = orig.producePlanId,
                    };

                    res.producePlanId = orig.producePlanId;


                    List<ProducePlanProductInterface2> products = new List<ProducePlanProductInterface2>();
                    List<ProducePlanProcessItemInterface2> items = new List<ProducePlanProcessItemInterface2>();

                    foreach (var prod in orig.ProducePlanProducts)
                    {
                        var _prd = new ProducePlanProductInterface2
                        {
                            producePlansProductId = prod.producePlansProductId,
                            productCode = prod.productCode,
                            productName = prod.productName,
                            productStandard = prod.productStandard,
                            productUnit = prod.productUnit,
                            productPlanQuantity = prod.productPlanQuantity,
                            ProductClassification = prod.ProductClassification
                        };

                        foreach (var _proc in prod.ProducePlanProcesses)
                        {
                            foreach(var _item in _proc)
                            {
                                var item = new ProducePlanProcessItemInterface2
                                {
                                    itemCode = _item.itemCode,
                                    itemClassification = _item.itemClassification,
                                    itemName = _item.itemName,
                                    itemStandard = _item.itemStandard,
                                    itemUnit = _item.itemUnit,
                                    requiredQuantity = (float)_item.requiredQuantity,
                                    loss = (float)_item.loss,
                                    productPlanQuantity = prod.productPlanQuantity,
                                    totalInputQuantity = prod.productPlanQuantity * ((float)_item.requiredQuantity+ (float)_item.loss),
                                    inventory = _db.Inventory.Where(x=>x.Product.Id == _item.itemId).Select(x=>x.Total).Sum(),
                                };
                                items.Add(item);
                            }
                        }
                        products.Add(_prd);
                    }
                    res.ProducePlanProducts = products;
                    res.ProducePlanProductsInputItems = items;
                }
                

                var Res = new Response<GetRequiredAmountsResponse002>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };
                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<GetRequiredAmountsResponse002>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        //발주버튼이 있네 확인할 것!
        #endregion

        #region 4) 공정별 작업일보관리
        

        //공정조회
        public async Task<Response<IEnumerable<GetReportByProcessesResponse001>>> GetReportByProcesses(GetReportByProcessesRequest001 _req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(_req.workOrderStartDate))
                    .Where(x => x.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(_req.workOrderEndDate))
                    .Select(x => new GetReportByProcessesResponse001
                    {
                        workerOrderProducePlanId = x.WorkerOrderProducePlanId,

                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                       
                        processId = x.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Id).FirstOrDefault(),

                        processCode =  x.WorkerOrder.ProducePlansProduct != null? 
                            x.ProducePlansProcess.ProductProcess.Process.Code :
                            _db.WorkerOrderWithoutPlans.Where(y=>y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y=>y.ProductProcess.Process.Code).FirstOrDefault(),
                        processName = x.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Name :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Name).FirstOrDefault(),
                        processMemo = x.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Process.Memo :
                            _db.WorkerOrderWithoutPlans.Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y => y.ProductProcess.Process.Memo).FirstOrDefault(),
                    }).ToListAsync();


                var res4 = res.GroupBy(x => x.processCode).ToList();


                List<GetReportByProcessesResponse001> res5 = new List<GetReportByProcessesResponse001>();


                foreach (var r in res4)
                {
                    var _temp = new GetReportByProcessesResponse001 { };
                    _temp.workOrderDate = "2000-01-01";
                    foreach(var x in r)
                    {
                        _temp.processId = x.processId;
                        _temp.workOrderDate = Convert.ToDateTime(_temp.workOrderDate) <= Convert.ToDateTime(x.workOrderDate) ? x.workOrderDate : _temp.workOrderDate;
                        _temp.processMemo = x.processMemo;
                        _temp.processCode = x.processCode;
                        _temp.processName = x.processName;
                    }
                    res5.Add(_temp);
                }

                var res6 = res5.Where(x=> _req.processId == 0? true : x.processId == _req.processId).OrderByDescending(x => x.workOrderDate).ThenBy(x=>x.processCode);

                var Res = new Response<IEnumerable<GetReportByProcessesResponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res6
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<GetReportByProcessesResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        //작업지시목록
        public async Task<Response<IEnumerable<GetReportByProcessWorkOrdersResponse002>>> GetReportByProcessWorkOrders(GetReportByProcessesRequest001 _req)
        {
            try
            {
                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(_req.workOrderStartDate))
                    .Where(x => x.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(_req.workOrderEndDate))
                    .Select(x => new GetReportByProcessWorkOrdersResponse002
                    {
                        workerOrderId = x.WorkerOrder.WorkerOrderId,
                        workOrderNo = x.WorkerOrder.WorkOrderNo,
                        workOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        workOrderSequence = x.WorkerOrder.WorkOrderSequence,
                        IsOutSourcing = x.InOutSourcing == 1? true :false,
                        PartnerName = x.Partner.Name,
                        FacilitiesCode = x.Facility.Code,
                        FacilitiesName = x.Facility.Name,
                        MoldCode = x.Mold.Code,
                        MoldName = x.Mold.Name,
                        WorkerName = x.Register.FullName,
                        ProcessCheck = x.ProcessCheck == 1? true :false,
                        ProcessWorkQuantity = x.ProcessWorkQuantity,
                        
                        ProcessId = x.WorkerOrder.ProducePlansProduct != null? 
                            x.ProducePlansProcess.ProductProcess.Process.Id :
                            _db.WorkerOrderWithoutPlans.Where(y=>y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId).Select(y=>y.ProductProcess.ProcessId).FirstOrDefault(),
                       
                    }).ToListAsync();

                var res2 = res.Where(x => x.ProcessId == _req.processId).GroupBy(x => x.workerOrderId);


                List<GetReportByProcessWorkOrdersResponse002> Rst = new List<GetReportByProcessWorkOrdersResponse002>();

                foreach(var i in res2)
                {
                    var rst = new GetReportByProcessWorkOrdersResponse002();

                    foreach ( var j in i)
                    {
                        rst.workerOrderId = i.FirstOrDefault().workerOrderId;
                        rst.workOrderNo = i.FirstOrDefault().workOrderNo;
                        rst.workOrderDate = i.FirstOrDefault().workOrderDate;
                        rst.workOrderSequence = i.FirstOrDefault().workOrderSequence;

                        rst.IsOutSourcing = i.FirstOrDefault().IsOutSourcing;
                        rst.PartnerName = i.FirstOrDefault().PartnerName;

                        rst.FacilitiesCode = i.FirstOrDefault().FacilitiesCode;
                        rst.FacilitiesName = i.FirstOrDefault().FacilitiesName;

                        rst.MoldCode = i.FirstOrDefault().MoldCode;
                        rst.MoldName = i.FirstOrDefault().MoldName;

                        rst.ProcessCheck = i.FirstOrDefault().ProcessCheck;
                        rst.ProcessWorkQuantity = i.FirstOrDefault().ProcessWorkQuantity;
                        rst.WorkerName = i.FirstOrDefault().WorkerName;
                        rst.ProcessId = i.FirstOrDefault().ProcessId;
                    }

                    Rst.Add(rst);
                }


                var Res = new Response<IEnumerable<GetReportByProcessWorkOrdersResponse002>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = Rst
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<GetReportByProcessWorkOrdersResponse002>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        //작업상세내역
        public async Task<Response<GetReportByProcessWorkOrdersResponse005>> GetReportByProcessWorkOrderProducePlans(GetReportByProcessesRequest001 _req)
        {
            try
            {

                //1. 작업지시 LIST 부터 GET.
                var resWorkorder1 = await _db.WorkerOrderProducePlans
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkerOrder.WorkOrderDate >= Convert.ToDateTime(_req.workOrderStartDate))
                    .Where(x => x.WorkerOrder.WorkOrderDate <= Convert.ToDateTime(_req.workOrderEndDate))
                   // .Where(x => x.ProducePlansProcess.ProducePlansProcessId != 1)
                   // .Where(x => x.ProducePlansProcess.ProductProcess.ProcessId == _req.processId)
                    .Where(x => x.WorkerOrder.WorkerOrderId == _req.workerOrderId)
                    .Select(x => new GetReportByProcessWorkOrdersResponse003
                    {
                        WorkderOrderProducePlanId = x.WorkerOrderProducePlanId,
                        WorkerOrderId = x.WorkerOrder.WorkerOrderId,
                        WorkOrderNo = x.WorkerOrder.WorkOrderNo,
                        WorkOrderDate = x.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        WorkOrderSequence = x.WorkerOrder.WorkOrderSequence,
                        IsOutSourcing = x.InOutSourcing == 0 ? false : true,
                        
                        ProductCode = x.Product.Code,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductName = x.Product.Name,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,
                        ProductWorkQuantity = x.WorkerOrder.ProductWorkQuantity,
                        WorkOrderStatus = x.ProcessProgress.WorkStatus,
                        RegisterDate = x.WorkerOrder.RegisterDate.ToString("yyyy-MM-dd"),
                        RegisterName = x.WorkerOrder.Register.FullName,
                        ProductionPlanNo = x.WorkerOrder.ProducePlansProduct.ProducePlan.ProductionPlanNo ?? "[해당생산계획없음]",

                    }).ToListAsync();


                var res = new GetReportByProcessWorkOrdersResponse005
                {
                    ProcessId = _db.Processes.Where(x => x.Id == _req.processId).Select(x => x.Id).FirstOrDefault(),
                    ProcessCode = _db.Processes.Where(x => x.Id == _req.processId).Select(x => x.Code).FirstOrDefault(),
                    ProcessName = _db.Processes.Where(x=>x.Id == _req.processId).Select(x=>x.Name).FirstOrDefault(),
                    ProcessClassification = _db.Processes.Where(x => x.Id == _req.processId).Select(x => x.CommonCode.Name).FirstOrDefault(),
                    WorkerOrderProducePlans = resWorkorder1,
                };



                var Res = new Response<GetReportByProcessWorkOrdersResponse005>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<GetReportByProcessWorkOrdersResponse005>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<GetReportByProcessWorkOrdersResponse004>> GetReportByProcessWorkOrderProducePlansProcess(GetReportByProcessesRequest001 _req)
        {
            try
            {
                var res2 = await _db.WorkerOrderProducePlans
                    .Where(x => x.WorkerOrderProducePlanId == _req.workerOrderProducePlanId)
                    .Select(x => new GetReportByProcessWorkOrdersResponse004
                    {
                        IsOutSourcing = x.InOutSourcing == 0 ? false : true,
                        PartnerName = x.Partner.Name,
                        WorkStatus = x.ProcessProgress.WorkStatus,
                        FacilitiesCode = x.Facility.Code,
                        FacilitiesName = x.Facility.Name,
                        MoldCode = x.Mold.Code,
                        MoldName = x.Mold.Name,
                        WorkerName = x.Register.FullName,
                        ProcessWorkQuantity = x.ProcessWorkQuantity,
                        ProcessCheck = x.ProcessCheck == 0 ? false : true,

                        InputItems = x.WorkerOrder.ProducePlansProduct != null ?
                            x.ProducePlansProcess.ProductProcess.Items
                                .Where(y => y.IsDeleted == 0)
                                .Select(y => new GetReportByProcessWorkOrderProducePlanInputItem
                                {
                                    ItemId = y.Product.Id,
                                    ItemCode = y.Product.Code,
                                    ItemName = y.Product.Name,
                                    ItemClassification = y.Product.CommonCode.Name,
                                    ItemStandard = y.Product.Standard,
                                    ItemUnit = y.Product.Unit,
                                    LOSS = (float)y.Loss,
                                    RequiredQuantity = (float)y.Require,
                                    TotalRequiredQuantity = (float)(y.Loss + y.Require),
                                    WorkerName = x.Register.FullName,
                                    ProcessCheck = x.ProcessCheck == 1?true:false,
                                    Flag = "withPlan",
                                    ProcessWorkQuantity = x.ProcessWorkQuantity,
                                }).ToList() : _db.WorkerOrderWithoutPlans
                                .Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId)
                                .Select(y => y.ProductProcess.Items
                                    .Where(z => z.IsDeleted == 0)
                                    .Select(z => new GetReportByProcessWorkOrderProducePlanInputItem
                                    {

                                        ItemId = z.Product.Id,
                                        ItemCode = z.Product.Code,
                                        ItemName = z.Product.Name,
                                        ItemClassification = z.Product.CommonCode.Name,
                                        ItemStandard = z.Product.Standard,
                                        ItemUnit = z.Product.Unit,
                                        LOSS = (float)z.Loss,
                                        RequiredQuantity = (float)z.Require,
                                        TotalRequiredQuantity = (float)(z.Loss * z.Require),
                                        WorkerName = x.Register.FullName,
                                        ProcessCheck = x.ProcessCheck == 1 ? true : false,
                                        ProcessWorkQuantity = x.ProcessWorkQuantity,

                                        Flag = "WithoutPlan"
                                    }).ToList())
                                .FirstOrDefault()
                    }).FirstOrDefaultAsync();


                /*

                var res = await _db.WorkerOrderProducePlans
                    .Where(x => x.WorkerOrderProducePlanId == _req.workerOrderProducePlanId)
                    .Select(x => 
                        x.WorkerOrder.ProducePlansProduct != null?
                            x.ProducePlansProcess.ProductProcess.Items
                                .Where(y=>y.IsDeleted == 0)
                                .Select(y=> new GetReportByProcessWorkOrderProducePlanInputItem
                                {
                                    IsOutSourcing = x.InOutSourcing == 0 ? false : true,
                                    PartnerName = x.Partner.Name,
                                    WorkStatus = x.ProcessProgress.WorkStatus,
                                    FacilitiesCode = x.Facility.Code,
                                    FacilitiesName = x.Facility.Name,
                                    MoldCode = x.Mold.Code,
                                    MoldName = x.Mold.Name,

                                    ItemId = y.Product.Id,
                                    ItemCode = y.Product.Code,
                                    ItemName = y.Product.Name,
                                    ItemClassification = y.Product.CommonCode.Name,
                                    ItemStandard = y.Product.Standard,
                                    ItemUnit = y.Product.Unit,
                                    LOSS = y.Loss,
                                    RequiredQuantity = y.Require,
                                    TotalRequiredQuantity = y.Loss * y.Require,

                                    WorkerName = x.Register.FullName,
                                    ProcessWorkQuantity = x.ProcessWorkQuantity,
                                    ProcessCheck = x.ProcessCheck == 0 ? false : true,
                                    Flag = "withPlan",
                                }).ToList() :

                            _db.WorkerOrderWithoutPlans
                                .Where(y => y.WorkerOrderProducePlan.WorkerOrderProducePlanId == x.WorkerOrderProducePlanId)
                                .Select(y => y.ProductProcess.Items
                                    .Where(z=>z.IsDeleted == 0)
                                    .Select(z => new GetReportByProcessWorkOrderProducePlanInputItem
                                    {
                                        IsOutSourcing = x.InOutSourcing == 0 ? false : true,
                                        PartnerName = x.Partner.Name,
                                        WorkStatus = x.ProcessProgress.WorkStatus,
                                        FacilitiesCode = x.Facility.Code,
                                        FacilitiesName = x.Facility.Name,
                                        MoldCode = x.Mold.Code,
                                        MoldName = x.Mold.Name,

                                        ItemId = z.Product.Id,
                                        ItemCode = z.Product.Code,
                                        ItemName = z.Product.Name,
                                        ItemClassification = z.Product.CommonCode.Name,
                                        ItemStandard = z.Product.Standard,
                                        ItemUnit = z.Product.Unit,
                                        LOSS = z.Loss,
                                        RequiredQuantity = z.Require,
                                        TotalRequiredQuantity = z.Loss * z.Require,

                                        WorkerName = x.Register.FullName,
                                        ProcessWorkQuantity = x.ProcessWorkQuantity,
                                        ProcessCheck = x.ProcessCheck == 0 ? false : true,
                                        Flag = "WithoutPlan"
                                    }).ToList())
                                .FirstOrDefault()
                    ).FirstOrDefaultAsync();
                */

                var Res = new Response<GetReportByProcessWorkOrdersResponse004>()
                {
                    IsSuccess = true,
                    ErrorMessage = "" ,
                    Data = res2 
                };

                return Res;
            }
            catch(Exception ex)
            {
                var Res = new Response<GetReportByProcessWorkOrdersResponse004>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }

        }



        #endregion

        #region 5) 제품별 생산량 관리
        public async Task<Response<IEnumerable<GetProductionManageByProductsResponse001>>> GetProductionManageByProducts(GetProductionManageByProductsRequest001 _req)
        {
            try
            {

                var res = await _db.ProcessProgresses
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkOrderProducePlan.WorkerOrder.WorkOrderNo.Contains(_req.workOrderNo))
                    .Where(x => x.WorkEndDateTime >= Convert.ToDateTime(_req.workOrderStartDate))
                    .Where(x => x.WorkEndDateTime <= Convert.ToDateTime(_req.workOrderEndDate).AddDays(1))
                    .Where(x => _req.productId == 0 ? true : x.WorkOrderProducePlan.Product.Id == _req.productId)
                    .Where(x => _req.productClassification == "ALL" ? true : x.WorkOrderProducePlan.Product.CommonCode.Name.Contains(_req.productClassification))
                    .Where(x => x.WorkStatus == "작업완료")
                    .Where(x => x.WorkOrderProducePlan.Product != null)
                    .Where(x => x.ProductionQuantity>0)
                    .Select(x => new GetProductionManageByProductsResponse001
                    {
                        productCode = x.WorkOrderProducePlan.Product.Code,
                        productName = x.WorkOrderProducePlan.Product.Name,
                        productClassification = x.WorkOrderProducePlan.Product.CommonCode.Name,
                        productUnit = x.WorkOrderProducePlan.Product.Unit,
                        productStandard = x.WorkOrderProducePlan.Product.Standard,
                        workOrderNo = x.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,
                        workStartDateTime = x.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                        workEndDateTime = x.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        workerName = x.WorkOrderProducePlan.Register.FullName,
                        productLOT =  _db.Lots
                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessType == "P")
                            .Select(y => y.LotName)
                            .FirstOrDefault(),
                        downtime = _db.ProcessNotWork
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgressId == x.ProcessProgressId)
                            .Select(y => EF.Functions.DateDiffMinute(y.ShutdownStartDateTime, y.ShutdownEndDateTime)).Sum(),

                        processElapsedTime = EF.Functions.DateDiffMinute(x.WorkStartDateTime, x.WorkEndDateTime),
                        productionGoodQuantity = x.ProductionQuantity - _db.ProcessDefectives
                                                                            .Where(y => y.IsDeleted == 0)
                                                                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                                                                            .Select(y => y.DefectiveCount)
                                                                            .Sum(),
                        productionQuantity = x.ProductionQuantity,
                        productDefectiveQuantity = _db.ProcessDefectives
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                            .Select(y=>y.DefectiveCount)
                            .Sum()

                    }).ToListAsync();




                var Res = new Response<IEnumerable<GetProductionManageByProductsResponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<GetProductionManageByProductsResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion

        #region 6) 기간별 생산량 관리
        public async Task<Response<IEnumerable<GetProductionManageByPeriodsResponse001>>> GetProductionManageByPeriods(GetProductionManageByPeriodsRequest001 _req)
        {
            try
            {
                 var res = await _db.ProcessProgresses
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.WorkOrderProducePlan.WorkerOrder.WorkOrderNo.Contains(_req.workOrderNo))
                    .Where(x => x.WorkEndDateTime >= Convert.ToDateTime(_req.workOrderStartDate))
                    .Where(x => x.WorkEndDateTime <= Convert.ToDateTime(_req.workOrderEndDate).AddDays(1))
                    .Where(x => _req.productId == 0 ? true : x.WorkOrderProducePlan.Product.Id == _req.productId)
                    .Where(x => _req.productClassification == "ALL" ? true : x.WorkOrderProducePlan.Product.CommonCode.Name.Contains(_req.productClassification))
                    .Where(x => x.WorkStatus == "작업완료")
                    .Where(x => x.WorkOrderProducePlan.Product != null)
                    .Where(x => x.ProductionQuantity > 0)
                    .Select(x => new GetProductionManageByPeriodsResponse001
                    {
                        processProgressId = x.ProcessProgressId,
                        productCode = x.WorkOrderProducePlan.Product.Code,
                        productName = x.WorkOrderProducePlan.Product.Name,
                        productClassification = x.WorkOrderProducePlan.Product.CommonCode.Name,
                        productUnit = x.WorkOrderProducePlan.Product.Unit,
                        productStandard = x.WorkOrderProducePlan.Product.Standard,
                       
                        workOrderNo = x.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,
                        workStartDateTime = x.WorkStartDateTime.ToString("yyyy-MM-dd HH:mm"),
                        workEndDateTime = x.WorkEndDateTime.ToString("yyyy-MM-dd HH:mm"),
                        workerName = x.WorkOrderProducePlan.Register.FullName,

                        productLOT =  _db.Lots
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                            .Where(y => y.ProcessType == "P")
                            .Select(y => y.LotName).FirstOrDefault(),
                       
                        downtime = _db.ProcessNotWork
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y => y.ProcessProgressId == x.ProcessProgressId)
                            .Select(y=> EF.Functions.DateDiffMinute(y.ShutdownStartDateTime, y.ShutdownEndDateTime)).Sum(),

                        productDefectiveQuantity = _db.ProcessDefectives
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                            .Select(y=>y.DefectiveCount)
                            .Sum(),

                        processElapsedTime = EF.Functions.DateDiffMinute(x.WorkStartDateTime, x.WorkEndDateTime),
                        productionQuantity = x.ProductionQuantity,

                        productionGoodQuantity = x.ProductionQuantity - _db.ProcessDefectives
                                                                            .Where(y => y.IsDeleted == 0)
                                                                            .Where(y => y.ProcessProgress.ProcessProgressId == x.ProcessProgressId)
                                                                            .Select(y => y.DefectiveCount)
                                                                            .Sum()
                    }).ToListAsync();


                var Res = new Response<IEnumerable<GetProductionManageByPeriodsResponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<GetProductionManageByPeriodsResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion

        #region 7) 기간별 불량수량 관리
        public async Task<Response<IEnumerable<GetDefectiveManageByPeriodsResponse001>>> GetDefectiveManageByPeriods(GetDefectiveManageByPeriodsRequest001 _req)
        {
            try
            {

                //1. 기타
                var resEtc = await _db.EtcDefectivesDetails
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.EtcDefective.DefectiveDate >= Convert.ToDateTime(_req.defectiveStartDate))
                    .Where(x => x.EtcDefective.DefectiveDate <= Convert.ToDateTime(_req.defectiveEndDate))
                    .Where(x => _req.productId == 0 ? true : x.Lot.LotCounts.Select(y => y.Product.Id).Contains(_req.productId))
                    .Where(x => _req.productClassification == "ALL" ? true : x.Lot.LotCounts.Select(y => y.Product.CommonCode.Name).Contains(_req.productClassification))
                    .Where(x => _req.defectiveId == 0 ? true : x.EtcDefective.Defective.Id == _req.defectiveId)

                    .Select(x => new GetDefectiveManageByPeriodsResponse001
                    {
                        defectiveDate = x.EtcDefective.DefectiveDate.ToString("yyyy-MM-dd"),
                        defectiveCode = x.EtcDefective.Defective.Code,
                        defectiveName = x.EtcDefective.Defective.Name,

                        productCode = x.EtcDefective.Product.Code,
                        productClassification = x.EtcDefective.Product.CommonCode.Name,
                        productName = x.EtcDefective.Product.Name,
                        productUnit = x.EtcDefective.Product.Unit,
                        productStandard = x.EtcDefective.Product.Standard,

                        produceDefectiveQuantity = x.DefectiveQuantity,
                        type = "기타",
                        productLOT = x.Lot.LotName,
                        dateFlag = x.EtcDefective.DefectiveDate
                    }).ToListAsync();


                //2. 입고
                var resIn = await _db.StoreOutStoreProductDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => _req.productId == 0 ? true : x.StoreOutStoreProduct.OutStoreProduct.Product.Id == _req.productId)
                    .Where(x => x.StoreOutStoreProduct.Receiving.ReceivingDate >= Convert.ToDateTime(_req.defectiveStartDate))
                    .Where(x => x.StoreOutStoreProduct.Receiving.ReceivingDate <= Convert.ToDateTime(_req.defectiveEndDate))

                    .Where(x => _req.productClassification == "ALL" ? true : x.StoreOutStoreProduct.OutStoreProduct.Product.CommonCode.Name.Contains(_req.productClassification))
                    .Where(x => _req.defectiveId == 0 ? true : x.Defective.Id == _req.defectiveId)
                    .Select(x => new GetDefectiveManageByPeriodsResponse001
                    {
                        defectiveDate = x.StoreOutStoreProduct.Receiving.ReceivingDate.ToString("yyyy-MM-dd"),
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,

                        productCode = x.StoreOutStoreProduct.OutStoreProduct.Product.Code,
                        productClassification = x.StoreOutStoreProduct.OutStoreProduct.Product.CommonCode.Name,
                        productName = x.StoreOutStoreProduct.OutStoreProduct.Product.Name,
                        productUnit = x.StoreOutStoreProduct.OutStoreProduct.Product.Unit,
                        productStandard = x.StoreOutStoreProduct.OutStoreProduct.Product.Standard,
                        produceDefectiveQuantity = x.DefectiveQuantity,
                        type = "입고",
                        productLOT = x.Lot.LotName,
                        dateFlag = x.StoreOutStoreProduct.Receiving.ReceivingDate
                    }).ToListAsync();

                //3. 출하
                //3. 공정
                var resProc = await _db.ProcessDefectives
                    .Where(x=>x.IsDeleted == 0)
                    .Where(x => _req.productId == 0 ? true : x.ProcessProgress.WorkOrderProducePlan.Product.Id == _req.productId)
                    .Where(x => x.ProcessProgress.WorkEndDateTime >= Convert.ToDateTime(_req.defectiveStartDate))
                    .Where(x => x.ProcessProgress.WorkEndDateTime <= Convert.ToDateTime(_req.defectiveEndDate).AddHours(23))
                    .Where(x => _req.productClassification == "ALL" ? true : x.ProcessProgress.WorkOrderProducePlan.Product.CommonCode.Name.Contains(_req.productClassification))
                    .Where(x => _req.defectiveId == 0 ? true : x.Defective.Id == _req.defectiveId)
                    .Select(x => new GetDefectiveManageByPeriodsResponse001
                    {
                        defectiveDate = x.ProcessProgress.WorkEndDateTime.ToString("yyyy-MM-dd"),
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,

                        productCode = x.ProcessProgress.WorkOrderProducePlan.Product.Code,
                        productClassification = x.ProcessProgress.WorkOrderProducePlan.Product.CommonCode.Name,
                        productName = x.ProcessProgress.WorkOrderProducePlan.Product.Name,
                        productUnit = x.ProcessProgress.WorkOrderProducePlan.Product.Unit,
                        productStandard = x.ProcessProgress.WorkOrderProducePlan.Product.Standard,
                        produceDefectiveQuantity = x.DefectiveCount,
                        type = "공정",
                        productLOT = x.Lot.LotName,
                        dateFlag = x.ProcessProgress.WorkEndDateTime
                    }).ToListAsync();

                //4. 출하
                var resOut = await _db.OutOrderProductsDefectives
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => _req.productId == 0 ? true : x.OutOrderProduct.Product.Id == _req.productId)
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate >= Convert.ToDateTime(_req.defectiveStartDate))
                    .Where(x => x.OutOrderProduct.OutOrder.ShipmentDate <= Convert.ToDateTime(_req.defectiveEndDate))

                    .Where(x => _req.productClassification == "ALL" ? true : x.OutOrderProduct.Product.CommonCode.Name.Contains(_req.productClassification))
                    .Where(x => _req.defectiveId == 0 ? true : x.Defective.Id == _req.defectiveId)
                    .Select(x => new GetDefectiveManageByPeriodsResponse001
                    {
                        defectiveDate = x.OutOrderProduct.OutOrder.ShipmentDate.ToString("yyyy-MM-dd"),
                        defectiveCode = x.Defective.Code,
                        defectiveName = x.Defective.Name,

                        productCode = x.OutOrderProduct.Product.Code,
                        productClassification = x.OutOrderProduct.Product.CommonCode.Name,
                        productName = x.OutOrderProduct.Product.Name,
                        productUnit = x.OutOrderProduct.Product.Unit,
                        productStandard = x.OutOrderProduct.Product.Standard,
                        produceDefectiveQuantity = x.DefectiveQuantity,
                        type = "출고",
                        productLOT = x.Lot.LotName,
                        dateFlag = x.OutOrderProduct.OutOrder.ShipmentDate
                    }).ToListAsync();


                List<GetDefectiveManageByPeriodsResponse001> res = new List<GetDefectiveManageByPeriodsResponse001>();
                foreach(var i in resEtc)
                {
                    res.Add(i);
                }

                foreach (var i in resIn)
                {
                    res.Add(i);
                }

                foreach (var i in resProc)
                {
                    res.Add(i);
                }

                foreach (var i in resOut)
                {
                    res.Add(i);
                }

                var res2 = res.OrderBy(x => x.dateFlag);


                var Res = new Response<IEnumerable<GetDefectiveManageByPeriodsResponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res2
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<GetDefectiveManageByPeriodsResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion

        #region 8) 월별 생산량 관리
        public async Task<Response<IEnumerable<GetProductionManageByMonthResponse001>>> GetProductionManageByMonth(GetProductionManageByMonthRequest001 _req)
        {
            try
            {
                string year = _req.year.ToString();
                string startDate = _req.year.ToString() + "-01-01";
                string endDate = _req.year.ToString() + "-12-31";

                var res = await _db.ProcessProgresses
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => _req.productClassification == "ALL" ? true : x.WorkOrderProducePlan.Product.CommonCode.Name.Contains(_req.productClassification))
                    .Where(x => x.WorkOrderProducePlan.Product.Name.Contains(_req.searchInput) || x.WorkOrderProducePlan.Product.Code.Contains(_req.searchInput))
                    .Where(x => x.WorkEndDateTime >= Convert.ToDateTime(startDate))
                    .Where(x => x.WorkEndDateTime <= Convert.ToDateTime(endDate))
                    .Where(x => x.WorkOrderProducePlan.Product != null)
                    .Select(x => new GetProductionManageByMonthResponse001
                    {
                        productCode = x.WorkOrderProducePlan.Product.Code,
                        productClassification = x.WorkOrderProducePlan.Product.CommonCode.Name,
                        productName = x.WorkOrderProducePlan.Product.Name,
                        productStandard = x.WorkOrderProducePlan.Product.Standard,
                        productUnit = x.WorkOrderProducePlan.Product.Unit,
                        jan = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-01-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-02-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        feb = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-02-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-03-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        mar = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-03-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-04-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        apr = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-04-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-05-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        may = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-05-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-06-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        jun = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-06-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-07-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        july = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-07-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-08-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        aug = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-08-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-09-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        sep = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-09-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-10-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        oct = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-10-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-11-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        nov = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-11-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-12-01"))
                            .Select(y => y.ProductionQuantity).Sum(),

                        dec = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-12-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-12-31 23:59:00"))
                            .Select(y => y.ProductionQuantity).Sum(),


                        total = _db.ProcessProgresses
                            .Where(y => y.IsDeleted == 0)
                            .Where(y => y.WorkOrderProducePlan.Product.Id == x.WorkOrderProducePlan.Product.Id)
                            .Where(y => y.WorkEndDateTime >= Convert.ToDateTime(year + "-01-01"))
                            .Where(y => y.WorkEndDateTime <= Convert.ToDateTime(year + "-12-31 23:59:00"))
                            .Select(y => y.ProductionQuantity).Sum(),

                    })
                    .Distinct().ToArrayAsync();


                var grouped = res.GroupBy(x => x.productCode).Select(x => new GetProductionManageByMonthResponse001
                {
                    productCode = x.Key,
                    productClassification = x.FirstOrDefault().productClassification,
                    productName = x.FirstOrDefault().productName,
                    productStandard = x.FirstOrDefault().productStandard,
                    productUnit = x.FirstOrDefault().productUnit,
                    jan = x.Sum(y=>y.jan),
                    feb = x.Sum(y=>y.feb),
                    mar = x.Sum(y=>y.mar),
                    apr = x.Sum(y=>y.apr),
                    may = x.Sum(y=>y.may),
                    jun = x.Sum(y=>y.jun),
                    july = x.Sum(y=>y.july),
                    aug = x.Sum(y=>y.aug),
                    oct = x.Sum(y=>y.oct),
                    nov = x.Sum(y=>y.nov),
                    dec = x.Sum(y=>y.dec),
                    sep = x.Sum(y=>y.sep),
                    total = x.Sum(y=>y.total)
                });

                var ordered = grouped.OrderBy(x => x.productCode);



                var Res = new Response<IEnumerable<GetProductionManageByMonthResponse001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = ordered
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<GetProductionManageByMonthResponse001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion


        #region KPI 
        public async Task<Response<IEnumerable<KpiProductionByHourResponse>>> GetProductionByHour(KpiRequest _req)
        {
            try
            {
                var res = await _db.VW_KPI
                    .Where(x => x.Year == _req.Year)
                    .Where(x => _req.ProductId == 0? true : x.Product.Id == _req.ProductId)
                    .GroupBy(o => new
                    {
                        Month = o.WorkEndDateTime.Month,
                    })
                    .Select(x => new KpiProductionByHourResponse
                    {
                        Year = _req.Year,
                        Month = x.Key.Month,
                        ProductionQuantity = x.Sum(y => y.ProductionQuantity),
                        ElapseTime = x.Sum(y=>y.ElapseTime),
                        DefectiveQuantity = x.Sum(y=>y.DefectiveQuantity),
                        Eah = x.Sum(y => y.ProductionQuantity) / x.Sum(y => y.ElapseTime) * 60,
                        DefectiveEah = x.Sum(y => y.DefectiveQuantity) / x.Sum(y => y.ElapseTime) * 60,
                    })
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Month)
                    .ToListAsync();


                List<KpiProductionByHourResponse> list = new List<KpiProductionByHourResponse>();

                bool flag = false;
                for(int i = 0; i < 12; i++)
                {
                    flag = false;
                    var kpi = new KpiProductionByHourResponse
                    {
                        Year = _req.Year,
                        Month = 1 + i,
                        ProductionQuantity = 0,
                        ElapseTime = 0,
                        DefectiveQuantity = 0,
                        Eah = 0,
                        DefectiveEah = 0,
                    };

                    foreach(var q in res)
                    {
                        if(q.Month == i + 1)
                        {
                            list.Add(q);
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        list.Add(kpi);
                    }
                }

                var Res = new Response<IEnumerable<KpiProductionByHourResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = list
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<KpiProductionByHourResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        
        public async Task<Response<IEnumerable<KpiProductionByHourResponse>>> GetDefectiveByHour(KpiRequest _req)
        {
            try
            {
                var res = await _db.VW_KPI
                    .Where(x => x.Year == _req.Year)
                    .Where(x => _req.ProductId == 0 ? true : x.Product.Id == _req.ProductId)
                    .GroupBy(o => new
                    {
                        Month = o.WorkEndDateTime.Month,
                    }).Select(x => new KpiProductionByHourResponse
                    {
                        Year = x.FirstOrDefault().Year,
                        Month = x.Key.Month,
                        ProductionQuantity = x.Sum(y => y.ProductionQuantity),
                        ElapseTime = x.Sum(y => y.ElapseTime),
                        DefectiveQuantity = x.Sum(y => y.DefectiveQuantity),
                        Eah = x.Sum(y => y.ProductionQuantity) / x.Sum(y => y.ElapseTime) * 60,
                        DefectiveEah = x.Sum(y => y.DefectiveQuantity) / x.Sum(y => y.ElapseTime) * 60,
                    })
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Month)
                    .ToListAsync();

                var Res = new Response<IEnumerable<KpiProductionByHourResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<KpiProductionByHourResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<KpiReportResponse>> GetKpiByMonth(KpiRequest _req)
        {
            try
            {
                DateTime prevDate = Convert.ToDateTime((_req.Year.ToString("0000") + "-" + _req.Month.ToString("00") + "-01")).AddDays(-5);

                int prevYear = prevDate.Year;
                int prevMonth = prevDate.Month;

                var res = await _db.VW_KPI
                    .Where(x => x.Year == _req.Year)
                    .Where(x => x.Month == _req.Month)
                    .Where(x => x.Product.Id == _req.ProductId)
                    .Select(x => new KpiReportResponse
                    {
                        ProductUnit = x.Product.Unit,
                        ProductClassification = x.Product.CommonCode.Name,
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        ProductStandard = x.Product.Standard,
                        CurrentDefectiveEah = _db.VW_KPI.Where(y => y.Month == _req.Month).Where(y => y.Year == _req.Year).Where(y => y.Product.Id == _req.ProductId).Select(y => y.DefectiveQuantity).Sum(),
                        CurrentEah = _db.VW_KPI.Where(y => y.Month == _req.Month).Where(y => y.Year == _req.Year).Where(y => y.Product.Id == _req.ProductId).Select(y => y.ProductionQuantity).Sum(),
                        LastDefectiveEah = _db.VW_KPI.Where(y => y.Month == prevMonth).Where(y => y.Year == prevYear).Where(y => y.Product.Id == _req.ProductId).Select(y => y.DefectiveQuantity).Sum(),
                        LastEah = _db.VW_KPI.Where(y => y.Month == prevMonth).Where(y => y.Year == prevYear).Where(y => y.Product.Id == _req.ProductId).Select(y => y.ProductionQuantity).Sum(),
                        ElapseTime = _db.VW_KPI.Where(y => y.Month == _req.Month).Where(y => y.Year == _req.Year).Where(y => y.Product.Id == _req.ProductId).Select(y => y.ElapseTime).Sum(),
                        LastElapseTime = _db.VW_KPI.Where(y => y.Month == prevMonth).Where(y => y.Year == prevYear).Where(y => y.Product.Id == _req.ProductId).Select(y => y.ElapseTime).Sum()
                    })
                    .FirstOrDefaultAsync();

                if(res == null)
                {
                    var empty = await _db.Products.Where(x => x.Id == _req.ProductId)
                    .Select(x => new KpiReportResponse
                    {
                        ProductUnit = x.Unit,
                        ProductClassification = x.CommonCode.Name,
                        ProductCode = x.Code,
                        ProductName = x.Name,
                        ProductStandard = x.Standard,
                        CurrentDefectiveEah = 0,
                        CurrentEah = 0,
                        ElapseTime = 0,
                        LastDefectiveEah = 0,
                        LastEah = 0,
                        LastElapseTime = 0
                    })
                    .FirstOrDefaultAsync();

                    var Res2 = new Response<KpiReportResponse>()
                    {
                        IsSuccess = true,
                        ErrorMessage = "",
                        Data = empty
                    };

                    return Res2;

                }

                var Res = new Response<KpiReportResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<KpiReportResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        public async Task<Response<IEnumerable<KpiWorkListResponse>>> GetWorkByMonth(KpiRequest _req)
        {
            try
            {
                var res = await _db.VW_KPI
                    .Where(x => x.Year == _req.Year)
                    .Where(x => x.Month == _req.Month)
                    .Where(x => x.Product.Id == _req.ProductId)
                    .Select(x => new KpiWorkListResponse
                    {
                        DefectiveQuantity = x.DefectiveQuantity,
                        DownTime = x.DownTime,
                        WorkOrderDate = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderDate.ToString("yyyy-MM-dd"),
                        ElapseTime = x.ElapseTime,
                        ProductionQuantity = x.ProductionQuantity,
                        WorkOrderNo = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderNo,   
                        WorkSequence = x.ProcessProgress.WorkOrderProducePlan.WorkerOrder.WorkOrderSequence

                    }).ToListAsync();

                var Res = new Response<IEnumerable<KpiWorkListResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<KpiWorkListResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }


        #endregion KPI


        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
