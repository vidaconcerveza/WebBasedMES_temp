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
    public class ProcessProgressStatusMngRepository : IProcessProgressStatusMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public ProcessProgressStatusMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

     
        
        public async Task<Response<IEnumerable<ProcessProgressStatusRes001>>> processProgressStatusList(ProcessProgressStatusReq001 req)
        {
            try
            {

                var query = $"exec [dbo].[생산현황모니터링_공정진행현황_공정진행현황] ";
                var paramteters = new List<string>();

                
                if (req.processProgressStatusId != 0) paramteters.Add($"@processProgressStatusId={req.processProgressStatusId}");
                if (req.productId != 0) paramteters.Add($"@productId={req.productId}");
                if (req.moldId != 0) paramteters.Add($"@moldId={req.moldId}");
                if (req.facilityId != 0) paramteters.Add($"@facilityId={req.facilityId}");
                


                query = query + string.Join(",", paramteters);
                var _result = await _db.생산현황모니터링_공정진행현황_공정진행현황.FromSqlRaw(query).ToListAsync();

                var Res = new Response<IEnumerable<ProcessProgressStatusRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<ProcessProgressStatusRes001>>()
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
