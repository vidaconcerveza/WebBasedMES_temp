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
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public class FaultyMngRepository : IFaultyMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public FaultyMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

        #region 불량 조회 팝업
        public async Task<Response<IEnumerable<DefectivePopupResponse>>> GetDefectives(DefectivePopupRequest _req)
        {
            try
            {
                var res = await _db.Defectives
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _req.DefectiveIsUsing == "ALL" ? true : _req.DefectiveIsUsing == "Y" ? x.IsUsing == true : x.IsUsing == false)
                    .Where(x => x.Code.Contains(_req.SearchInput) || x.Name.Contains(_req.SearchInput))
                    .Select(x => new DefectivePopupResponse
                    {
                        DefectiveCode = x.Code,
                        DefectiveId = x.Id,
                        DefectiveIsUsing = x.IsUsing ? "사용" : "비사용",
                        DefectiveMemo = x.Memo,
                        DefectiveName = x.Name
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<DefectivePopupResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch(Exception ex)
            {
                var Res = new Response<IEnumerable<DefectivePopupResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 불량 조회 팝업

        #region 기타불량 품목 조회 팝업
        public async Task<Response<IEnumerable<ProductPopupResponse>>> GetProducts(ProductPopupRequest _req)
        {
            try
            {
                var res = await _db.Products
                    .Where(x => x.IsDeleted == false)
                    .Where(x => _req.ProductIsUsing == "ALL" ? true : _req.ProductIsUsing == "Y" ? x.IsUsing == true : x.IsUsing == false)
                    .Where(x => _req.ProductClassification == "ALL"? true : x.CommonCode.Name == _req.ProductClassification )
                    .Where(x => x.Code.Contains(_req.SearchInput) || x.Name.Contains(_req.SearchInput))
                    .Select(x => new ProductPopupResponse
                    {
                        ProductId = x.Id,
                        ProductClassification = x.CommonCode.Name,
                        ProductCode = x.Code,
                        ProductName = x.Name,

                        ProductStandard = x.Standard,
                        ProductTaxType = x.TaxType,
                        ProductUnit = x.Unit,
                        UploadFileName = x.UploadFile != null ? x.UploadFile.FileUrl : "",
                        
                        ProductIsUsing = x.IsUsing ? "사용" : "비사용",
                        ProductMemo = x.Memo,
                    })
                    .ToListAsync();

                var Res = new Response<IEnumerable<ProductPopupResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<ProductPopupResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 기타불량 품목 조회 팝업

        #region 기타불량 마스터 조회
        public async Task<Response<IEnumerable<FaultyRes001>>> faultyMstList(FaultyReq001 _req)
        {
            try
            {

                var res = await _db.EtcDefectives
                    .Include(x=>x.Product).ThenInclude(x=>x.CommonCode)
                    .Include(x=>x.EtcDefectivesDetails)
                    .Include(x=>x.Defective)
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.DefectiveDate >= Convert.ToDateTime(_req.defectiveStartDate))
                    .Where(x => x.DefectiveDate <= Convert.ToDateTime(_req.defectiveEndDate))
                    .Where(x => _req.defectiveId == 0 ? true : x.Defective.Id == _req.defectiveId)
                    .Where(x => _req.productId == 0 ? true : x.Product.Id == _req.productId)
                    .OrderBy(x => x.DefectiveDate)
                    .Select(x =>
                        new FaultyRes001
                        {
                            etcDefectiveId = x.EtcDefectiveId,
                            defectiveId = x.Defective == null? 0 :x.Defective.Id,
                            defectiveCode = x.Defective == null ?"": x.Defective.Code ,
                            defectiveDate = x.DefectiveDate.ToString("yyyy-MM-dd"),
                            defectiveName = x.Defective == null ? "" :x.Defective.Name,
                            productId = x.Product.Id,
                            productCode = x.Product.Code,
                            productClassification = x.Product.CommonCode.Name,
                            productName = x.Product.Name,
                            productStandard = x.Product.Standard,
                            productUnit = x.Product.Unit,

                            productStandardUnit = String.Join(",",x.EtcDefectivesDetails.Where(x => x.IsDeleted == 0).Select(y => y.ProductStandardUnit.Name).ToList()),
                            productDefectiveQuantity = x.EtcDefectivesDetails.Where(x=>x.IsDeleted == 0).Select(y=>y.DefectiveQuantity).Sum(),
                            productLOT = String.Join(",", x.EtcDefectivesDetails.Where(x=>x.IsDeleted == 0).Select(y=>y.Lot.LotName).ToList()),
                            defectiveProductMemo = x.EtcDefectiveMemo,
                            flag = x.DefectiveDate
                        }
                    )
                    .ToListAsync();

                var filteredRes = res.Where(x => x.productLOT.Contains(_req.productLOT));


                var Res = new Response<IEnumerable<FaultyRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = filteredRes
                };

                return Res;

            }
            catch (Exception ex)
            {
                var Res = new Response<IEnumerable<FaultyRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion 기타불량 마스터 조회

        #region 기타불량 BY ID 
        public async Task<Response<EtcDefectiveResponse>> getEtcDefective(FaultyReq001 _req)
        {
            try
            {
                var res = await _db.EtcDefectives
                    .Where(x => x.EtcDefectiveId == _req.etcDefectiveId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new EtcDefectiveResponse
                    {
                        EtcDefectiveId = x.EtcDefectiveId,
                        DefectiveDate = x.DefectiveDate.ToString("yyyy-MM-dd"),

                        DefectiveId = x.Defective.Id,
                        DefectiveCode = x.Defective.Code,
                        DefectiveName = x.Defective.Name,
                        ProductMemo = x.EtcDefectiveMemo,

                        ProductClassification = x.Product.CommonCode.Name,
                        ProductCode = x.Product.Code,
                        ProductName = x.Product.Name,
                        ProductId = x.Product.Id,
                        ProductStandard = x.Product.Standard,
                        ProductUnit = x.Product.Unit,

                        EtcDefectiveDetails = x.EtcDefectivesDetails.Where(y => y.IsDeleted == 0).Select(y => new EtcDefectiveDetailResponse
                        {
                            EtcDefectiveDetailId = y.EtcDefectiveDetailId,
                            ProductCode = x.Product.Code,
                            ProductName = x.Product.Name,
                            ProductId = x.Product.Id,
                            ProductClassification = x.Product.CommonCode.Name,
                            ProductStandard = x.Product.Standard,
                            ProductUnit = x.Product.Unit,
                            ProductLot = y.Lot.LotName,
                            ProductStandardUnit = y.ProductStandardUnit.Name,
                            ProductStandardUnitCount = y.ProductStandardUnitCount,
                            ProductStandardUnitId = y.ProductStandardUnit.Id,
                            DefectiveQuantity = y.DefectiveQuantity,
                            IsRegistered = true,
                        })
                    }).FirstOrDefaultAsync();

                var Res = new Response<EtcDefectiveResponse>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res
                };

                return Res;
            }
            catch (Exception ex)
            {

                var Res = new Response<EtcDefectiveResponse>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        #endregion

        #region 기타불량 CREATE LOT 검색 
        public async Task<Response<IEnumerable<FaultyLotResponse>>> faultyMstPop(FaultyReq001 _req)
        {
            try
            {

                //LOT NAME 아직 할당되지 않는 것들은 제외. 
                //INV가 0 이상일때만 필터링. 
                // -된다는것 자체가 문제가 있는거라.. 

                var res = await _db.LotCounts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Lot.LotName.Contains(_req.productLOT))
                    .Where(x => x.Lot.LotName != "")
                    .Where(x => _req.productId == 0 ? true : x.Product.Id == _req.productId)
                    .Select(x => new FaultyLotResponse
                    {
                        LotId = x.Lot.LotId,
                        productCode = x.Product.Code,
                        productClassification = x.Product.CommonCode.Name,
                        productName = x.Product.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,
                        productLOT = x.Lot.LotName,
                        productStandardUnitId = 0,
                        inventory = (x.StoreOutCount - x.OutOrderCount + x.ProduceCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                        IsRegistered = false
                    })
                    .ToListAsync();

                var res2 = res.GroupBy(x => x.productLOT)
                    .Select(x => new FaultyLotResponse
                    {
                        LotId = x.FirstOrDefault().LotId,   //나중에 LOT 이름으로 대표 ID만 가져옴, 어차피 GROUPBY라.. 상관없네요.
                        productCode = x.FirstOrDefault().productCode,
                        productClassification = x.FirstOrDefault().productClassification,
                        productName = x.FirstOrDefault().productName,
                        productStandard = x.FirstOrDefault().productStandard,
                        productUnit = x.FirstOrDefault().productUnit,
                        productLOT = x.FirstOrDefault().productLOT,
                        inventory = x.Select(y => y.inventory).Sum(),
                        IsRegistered = x.FirstOrDefault().IsRegistered,
                        productStandardUnitId = x.FirstOrDefault().productStandardUnitId
                    }).OrderBy(x => x.productLOT).ToList();

                var res3 = res2.Where(x => x.inventory >= 0);

                var Res = new Response<IEnumerable<FaultyLotResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = res3
                };

                return Res;
            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<FaultyLotResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 기타불량 CREATE LOT 검색 

        #region 기타불량 UPDATE POPUP - LOT 검색 API
        public async Task<Response<IEnumerable<FaultyLotResponse>>> EtcDefectiveUpdateSearchLot(FaultyReq001 _req)
        {
            try
            {

                //1. 최초 등록했을때의 LOT
                var _beforeData = await _db.EtcDefectivesDetails
                    .Include(x=>x.Lot)
                    .Where(x => x.EtcDefectiveId == _req.etcDefectiveId)
                    .Where(x => x.IsDeleted == 0)
                    .Select(x => new FaultyLotResponse
                    {
                        etcDefectiveDetailId = x.EtcDefectiveDetailId,
                        LotId = x.Lot.LotId,
                        productCode = x.Lot.LotCounts.FirstOrDefault().Product.Code,
                        productClassification = x.Lot.LotCounts.FirstOrDefault().Product.CommonCode.Name,
                        productName = x.Lot.LotCounts.FirstOrDefault().Product.Name,
                        productStandard = x.Lot.LotCounts.FirstOrDefault().Product.Standard,
                        productUnit = x.Lot.LotCounts.FirstOrDefault().Product.Unit,
                        productLOT = x.Lot.LotName,
                        inventory = _db.LotCounts
                            .Where(y=>y.IsDeleted == 0)
                            .Where(y=>y.Lot.LotName == x.Lot.LotName)
                            .Select(y => y.StoreOutCount - y.OutOrderCount + y.ProduceCount - y.ConsumeCount + y.ModifyCount - y.DefectiveCount)
                            .Sum() + x.DefectiveQuantity,
                        IsRegistered = true,
                        productStandardUnitId = x.ProductStandardUnit.Id,
                        productStandardUnit = x.ProductStandardUnit.Name,
                        productStandardUnitCount = x.ProductStandardUnitCount,
                        DefectiveQuantity = x.DefectiveQuantity,
                    })
                    .ToListAsync();

                //처음 조회된 LOT 제외한 LOT들을 검색하자.
                var res = await _db.LotCounts
                    .Where(x => x.IsDeleted == 0)
                    .Where(x => x.Lot.LotName.Contains(_req.productLOT))
                    .Where(x => x.Lot.LotName != "")
                    .Where(x => _req.productId == 0 ? true : x.Product.Id == _req.productId)
                    .Select(x => new FaultyLotResponse
                    {
                        etcDefectiveDetailId = 0,
                        LotId = x.Lot.LotId,
                        productCode = x.Product.Code,
                        productClassification = x.Product.CommonCode.Name,
                        productName = x.Product.Name,
                        productStandard = x.Product.Standard,
                        productUnit = x.Product.Unit,
                        productLOT = x.Lot.LotName,
                        inventory = (x.StoreOutCount - x.OutOrderCount + x.ProduceCount - x.ConsumeCount + x.ModifyCount - x.DefectiveCount),
                        IsRegistered = false,
                        productStandardUnitId = 0,
                        productStandardUnit = "",
                        productStandardUnitCount = 0,
                        DefectiveQuantity = 0,
                    })
                    .ToListAsync();

                var res2 = res.GroupBy(x => x.productLOT)
                    .Select(x => new FaultyLotResponse
                    {
                        LotId = x.FirstOrDefault().LotId,
                        productCode = x.FirstOrDefault().productCode,
                        productClassification = x.FirstOrDefault().productClassification,
                        productName = x.FirstOrDefault().productName,
                        productStandard = x.FirstOrDefault().productStandard,
                        productUnit = x.FirstOrDefault().productUnit,
                        productLOT = x.FirstOrDefault().productLOT,
                        inventory = x.Select(y => y.inventory).Sum(),
                        IsRegistered = x.FirstOrDefault().IsRegistered,
                        productStandardUnit = x.FirstOrDefault().productStandardUnit,
                        productStandardUnitCount = x.FirstOrDefault().productStandardUnitCount,
                        DefectiveQuantity = x.FirstOrDefault().DefectiveQuantity,
                        productStandardUnitId = x.FirstOrDefault().productStandardUnitId
                    }).ToList();

                var res3 = res2.Where(x => x.inventory >= 0);




                List<FaultyLotResponse> resf = new List<FaultyLotResponse>();
                
                foreach(var i in _beforeData)
                {
                    resf.Add(i);
                }

                bool flag = false;
                foreach(var i in res3)
                {
                    flag = false;
                    foreach (var j in _beforeData)
                    {
                        if(j.productLOT == i.productLOT)
                        {
                            flag = true;
                        }
                    }

                    if(!flag)
                    {
                        resf.Add(i);
                    }
                }




                var Res = new Response<IEnumerable<FaultyLotResponse>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = resf
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<FaultyLotResponse>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }




        #endregion

        #region 기타불량 CREATE
        public async Task<Response<bool>> CreateEtcDefective(CreateEtcDefectiveRequest _req)
        {
            try
            {
                var _product = await _db.Products.Where(y => y.Id == _req.productId).FirstOrDefaultAsync();
                var _defective = await _db.Defectives.Where(y => y.Id == _req.defectiveId).FirstOrDefaultAsync();

                List<EtcDefectivesDetail> etcDetails = new List<EtcDefectivesDetail>();

                foreach (EtcDefRequest x in _req.etcDefectiveDetails)
                {
                    var lot1 = await _db.Lots.Where(y => y.LotId == x.lotId).FirstOrDefaultAsync();

                    await _db.Lots.AddAsync(new LotEntity
                    {
                        LotName = lot1.LotName,
                        ProcessType = "E",
                        IsDeleted = 0,
                    });

                    await Save();

                    var lot2 = await _db.Lots.OrderBy(x=>x.LotId).LastOrDefaultAsync();

                    await _db.LotCounts.AddAsync(new LotCount
                    {
                        Product = _product,
                        DefectiveCount = x.defectiveQuantity,
                        Lot = lot2,
                        IsDeleted = 0,
                        ConsumeCount = 0,
                        ModifyCount = 0,
                        StoreOutCount = 0,
                        OutOrderCount = 0,
                        ProduceCount = 0
                    });

                    await Save();
                    var _unit = await _db.CommonCodes.Where(z => z.Id == x.productStandardUnitId).FirstOrDefaultAsync();

                    var etcDef = new EtcDefectivesDetail
                    {
                        DefectiveQuantity = x.defectiveQuantity,
                        ProductStandardUnit = _unit != null? _unit : _db.CommonCodes.FirstOrDefault(),
                        ProductStandardUnitCount = x.productStandardUnitCount,
                        IsDeleted = 0,
                        Lot = lot2
                    };
                    etcDetails.Add(etcDef);
                }


                await _db.EtcDefectives.AddRangeAsync(new EtcDefective
                {
                    Defective = _defective,
                    Product = _product,
                    DefectiveDate = Convert.ToDateTime(_req.defectiveDate),
                    EtcDefectiveMemo = _req.productMemo,
                    EtcDefectivesDetails = etcDetails
                });
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
        #endregion 기타불량 CREATE

        #region 기타불량 업데이트
        public async Task<Response<bool>> UpdateEtcDefective(CreateEtcDefectiveRequest _req)
        {
            try
            {
                var _product = await _db.Products.Where(y => y.Id == _req.productId).FirstOrDefaultAsync();
                var _defective = await _db.Defectives.Where(y => y.Id == _req.defectiveId).FirstOrDefaultAsync();

                var _origin = await _db.EtcDefectives
                    .Include(x=>x.EtcDefectivesDetails)
                    .Where(x => x.EtcDefectiveId == _req.etcDefectiveId)
                    .FirstOrDefaultAsync();

                if(_req.defectiveDate != "" && _req.defectiveDate != null)
                {
                    _origin.DefectiveDate = Convert.ToDateTime(_req.defectiveDate);
                }

                _origin.EtcDefectiveMemo = _req.productMemo;
                _origin.Defective = await _db.Defectives.Where(y => y.Id == _req.defectiveId).FirstOrDefaultAsync();
                _origin.Defective = _defective !=null? _defective : _origin.Defective;

                _db.EtcDefectives.Update(_origin);
                await Save();



                List<int> _deleteDetail = new List<int>();
                List<EtcDefRequest> _updateDetail = new List<EtcDefRequest>();
                List<EtcDefRequest> _createDetail = new List<EtcDefRequest>();

                bool checkFlag = false;
                foreach (var _orig in _origin.EtcDefectivesDetails)
                {
                    checkFlag = false;
                    foreach (var _detail in _req.etcDefectiveDetails)
                    {
                        if (_detail.etcDefectiveDetailId == _orig.EtcDefectiveDetailId)
                        {
                            _updateDetail.Add(_detail);
                            checkFlag = true;
                        }
                    }
                    if (!checkFlag)
                    {
                        _deleteDetail.Add(_orig.EtcDefectiveDetailId);
                    }
                }



                foreach (var _detail in _req.etcDefectiveDetails)
                {
                    if (_detail.etcDefectiveDetailId == 0)
                    {
                        _createDetail.Add(_detail);
                    }
                }

                //DELETE
                foreach(var _item in _deleteDetail)
                {
                    var _detail = await _db.EtcDefectivesDetails.Include(x=>x.Lot).Where(x => x.EtcDefectiveDetailId == _item).FirstOrDefaultAsync();
                    _detail.IsDeleted = 1;
                    _db.EtcDefectivesDetails.Update(_detail);

                    var _lot = await _db.Lots.Where(x => x.LotId == _detail.Lot.LotId).FirstOrDefaultAsync();
                    _lot.IsDeleted = 1;
                    _db.Lots.Update(_lot);

                    var _lotCnt = await _db.LotCounts
                        .Where(x => x.LotId == _lot.LotId)
                        .FirstOrDefaultAsync();

                    _lotCnt.IsDeleted = 1;
                    _db.LotCounts.Update(_lotCnt);

                    await Save();
                }



                foreach (var _item in _updateDetail)
                {
                    var _detail = await _db.EtcDefectivesDetails.Include(x => x.Lot).Where(x => x.EtcDefectiveDetailId == _item.etcDefectiveDetailId).FirstOrDefaultAsync();
                    var _lot = await _db.Lots.Where(x => x.LotId == _detail.Lot.LotId).FirstOrDefaultAsync();
                    var _lotCnt = await _db.LotCounts.Where(x => x.Lot == _lot).FirstOrDefaultAsync();

                    var _unit =await _db.CommonCodes.Where(y => y.Id == _item.productStandardUnitId).FirstOrDefaultAsync();

                    _detail.DefectiveQuantity = _item.defectiveQuantity;
                    _detail.ProductStandardUnit = _unit != null ? _unit : _db.CommonCodes.FirstOrDefault();
                    _detail.ProductStandardUnitCount = _item.productStandardUnitCount;

                    _lotCnt.DefectiveCount = _item.defectiveQuantity;

                    _db.EtcDefectivesDetails.Update(_detail);
                    _db.LotCounts.Update(_lotCnt);

                    await Save();
                }



                List<EtcDefectivesDetail> etcDetails = new List<EtcDefectivesDetail>();
                foreach (EtcDefRequest x in _createDetail)
                {
                    var lot1 = await _db.Lots.Where(y => y.LotId == x.lotId).FirstOrDefaultAsync();

                    await _db.Lots.AddAsync(new LotEntity
                    {
                        LotName = lot1.LotName,
                        ProcessType = "E",
                        IsDeleted = 0,
                    });

                    await Save();

                    var lot2 = await _db.Lots.OrderBy(x => x.LotId).LastOrDefaultAsync();

                    await _db.LotCounts.AddAsync(new LotCount
                    {
                        Product = _product,
                        DefectiveCount = x.defectiveQuantity,
                        Lot = lot2,
                        IsDeleted = 0,
                        ConsumeCount = 0,
                        ModifyCount = 0,
                        StoreOutCount = 0,
                        OutOrderCount = 0,
                        ProduceCount = 0
                    });
                     
                    await Save();

                    var _unit = await _db.CommonCodes.Where(z => z.Id == x.productStandardUnitId).FirstOrDefaultAsync();

                    var etcDef = new EtcDefectivesDetail
                    {

                        EtcDefectiveId = _origin.EtcDefectiveId,
                        DefectiveQuantity = x.defectiveQuantity,
                        ProductStandardUnit = _unit != null ? _unit : _db.CommonCodes.FirstOrDefault(),
                        ProductStandardUnitCount = x.productStandardUnitCount,
                        IsDeleted = 0,
                        Lot = lot2
                    };
                    _db.EtcDefectivesDetails.Add(etcDef);
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
        #endregion 기타불량 업데이트

        #region 기타불량 삭제
        public async Task<Response<bool>> DeleteEtcDefective(FaultyReq001 req)
        {
            try
            {

                foreach (int item in req.faultyIdArray)
                {

                    var _items1 = await _db.EtcDefectives.Where(x => x.EtcDefectiveId == item).FirstOrDefaultAsync();

                    _items1.IsDeleted = 1;
                    _db.EtcDefectives.Update(_items1);


                    var _items = await _db.EtcDefectivesDetails.Include(x => x.Lot).Where(x => x.EtcDefective.EtcDefectiveId == item).ToArrayAsync();

                    if (_items != null)
                    {
                        foreach (var i in _items)
                        {
                            i.IsDeleted = 1;
                            _db.EtcDefectivesDetails.Update(i);

                            var _lot = await _db.Lots.Where(x => x.LotId == i.Lot.LotId).FirstOrDefaultAsync();
                            var _lotCnt = await _db.LotCounts.Where(x => x.Lot == _lot).FirstOrDefaultAsync();

                            if (_lot != null)
                            {
                                _lot.IsDeleted = 1;
                                _db.Lots.Update(_lot);
                            }

                            if (_lotCnt != null)
                            {
                                _lotCnt.IsDeleted = 1;
                                _db.LotCounts.Update(_lotCnt);
                            }

                        }
                    }


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
                    ErrorMessage = "",
                    Data = false,
                };
                return Res;
            }
        }

        #endregion 기타불량 삭제

        #region 미사용 API 
        public async Task<Response<IEnumerable<FaultyRes003>>> faultyProductList(FaultyReq001 faultyReq001)
        {
            try
            {

                var query = $"exec [dbo].[품질관리_기타불량등록_등록수정화면_목록조회] ";
                var paramteters = new List<string>();

                if (faultyReq001.productLOT != null || faultyReq001.productLOT != "") paramteters.Add($"@productLOT='{faultyReq001.productLOT}'");


                query = query + string.Join(",", paramteters);
                var _result = await _db.품질관리_기타불량등록_등록수정화면_목록조회.FromSqlRaw(query).ToListAsync();

                var Res = new Response<IEnumerable<FaultyRes003>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<FaultyRes003>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }
        #endregion 미사용 API 
        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

       


    }
}
