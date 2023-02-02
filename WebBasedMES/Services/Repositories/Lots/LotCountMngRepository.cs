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
using WebBasedMES.Services.Repositories.InAndOut;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Lot;

namespace WebBasedMES.Services.Repositories.Lots
{
    public class LotCountMngRepository : ILotCountMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public LotCountMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        public async Task<Response<bool>> CreateLotCount(LotCountRequestCrud lotCountRequest)
        {
            try
            {

                var _product = await _db.Products.Where(x => x.Id == lotCountRequest.productId).FirstOrDefaultAsync();
                var _lot = await _db.Lots.Where(x => x.LotId == lotCountRequest.lotId).FirstOrDefaultAsync();
                var _lotCount = new LotCount();
                {
                    _lotCount.Product = _product;
                    _lotCount.Lot = _lot;
                    _lotCount.StoreOutCount = lotCountRequest.storeOutCount;
                    _lotCount.OutOrderCount = lotCountRequest.outOrderCount;
                    _lotCount.ConsumeCount = lotCountRequest.consumeCount;
                    _lotCount.ProduceCount = lotCountRequest.produceCount;
                    _lotCount.IsDeleted = lotCountRequest.isDeleted;
                };



                var result = await _db.LotCounts.AddAsync(_lotCount);
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

        public async Task<Response<bool>> UpdateLotCount(LotCountRequestCrud lotCountRequest)
        {
            try
            {

                var _product = await _db.Products.Where(x => x.Id == lotCountRequest.productId).FirstOrDefaultAsync();
                var _lot = await _db.Lots.Where(x => x.LotId == lotCountRequest.lotId).FirstOrDefaultAsync();
                var _lotCount = new LotCount()

                {
                    LotCountId = lotCountRequest.lotCountId,
                    Lot = _lot,
                    StoreOutCount = lotCountRequest.storeOutCount,
                    OutOrderCount = lotCountRequest.outOrderCount,
                    ConsumeCount = lotCountRequest.consumeCount,
                    ProduceCount = lotCountRequest.produceCount,
                    IsDeleted = lotCountRequest.isDeleted,
                };
                _db.LotCounts.Update(_lotCount);

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
        public async Task<Response<bool>> DeleteLotCount(LotCountRequestCrud lotCountRequest)
        {
            try
            {
                foreach (int item in lotCountRequest.lotCountIdArray)
                {
                    var _lotCounts = await _db.LotCounts.FindAsync(item);
                    _lotCounts.IsDeleted = 1;
                    _db.LotCounts.Update(_lotCounts);
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


        public async Task<Response<bool>> editEvent(LotCountRequestCrud lotCountRequest)
        {
            try
            {
                foreach (LotCountRequestCrud item in lotCountRequest.LotCountArray)
                {
                    if (item.isSelected == 1)
                    {
                        var _lotCounts = await _db.LotCounts.FindAsync(item.lotCountId);

                        if (_lotCounts != null)
                        {
                            _lotCounts.ModifyCount = _lotCounts.ModifyCount + item.quantity - item.inventory;
                            _db.LotCounts.Update(_lotCounts);
                        }

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

        //public async Task<Response<IEnumerable<procesureT1>>> procesureT1s(LotCountRequestCrud lotRequest)
        //{
        //    try
        //    {
        //        var query = $"exec [dbo].[테스트1] ";
        //        var paramteters = new List<string>();
        //        if (lotRequest.productId != 0) paramteters.Add($"@productId={lotRequest.productId}");
        //        query = query + string.Join(",", paramteters);
        //        var _result = await _db.procesureT1s.FromSqlRaw(query).ToListAsync();
        //        var Res = new Response<IEnumerable<procesureT1>>()
        //        {
        //            IsSuccess = true,
        //            ErrorMessage = "",
        //            Data = _result
        //        };

        //        return Res;

        //    }
        //    catch (Exception ex)
        //    {

        //        var Res = new Response<IEnumerable<procesureT1>>()
        //        {
        //            IsSuccess = false,
        //            ErrorMessage = ex.Message.ToString(),
        //            Data = null
        //        };

        //        return Res;
        //    }
        //}
        //product list 
        public async Task<Response<IEnumerable<procesureT1>>> productList(OrderReq002 orderReq002)
        {
            try
            {
                var query = $"exec [dbo].[테스트1] ";
                var paramteters = new List<string>();
                if (orderReq002.productClassification != null || orderReq002.productClassification != "") paramteters.Add($"@productClassification='{orderReq002.productClassification}'");
                if (orderReq002.productIsUsingStr != null || orderReq002.productIsUsingStr != "") paramteters.Add($"@productIsUsingStr='{orderReq002.productIsUsingStr}'");
                query = query + string.Join(",", paramteters);
                var _result = await _db.procesureT1s.FromSqlRaw(query).ToListAsync();
                var Res = new Response<IEnumerable<procesureT1>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<procesureT1>>()
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
