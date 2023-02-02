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
using WebBasedMES.ViewModels.ProduceStatus;

namespace WebBasedMES.Services.Repositories.ProduceStatus
{
    public class OrderProduceStatusMngRepository : IOrderProduceStatusMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrderProduceStatusMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

     

        public async Task<Response<IEnumerable<OrderProduceStatusRes001>>> orderProduceStatusList(OrderProduceStatusReq001 req)
        {
            try
            {

                var query = $"exec [dbo].[생산현황모니터링_수주대비생산현황_수주대비생산현황] ";
                var paramteters = new List<string>();


                if (req.orderProduceStatusId != 0) paramteters.Add($"@orderProduceStatusId={req.orderProduceStatusId}");
                if (req.orderId != 0) paramteters.Add($"@orderId={req.orderId}");
                if (req.productId != 0) paramteters.Add($"@productId={req.productId}");
                if (req.partnerId != 0) paramteters.Add($"@partnerId={req.partnerId}");



                query = query + string.Join(",", paramteters);
                var _result = await _db.생산현황모니터링_수주대비생산현황_수주대비생산현황.FromSqlRaw(query).ToListAsync();

                var Res = new Response<IEnumerable<OrderProduceStatusRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<OrderProduceStatusRes001>>()
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
