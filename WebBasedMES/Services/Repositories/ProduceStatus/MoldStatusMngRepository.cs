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
    public class MoldStatusMngRepository : IMoldStatusMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public MoldStatusMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

     

        public async Task<Response<IEnumerable<MoldStatusRes001>>> moldInspectionList(MoldStatusReq001 req)
        {
            try
            {

                var query = $"exec [dbo].[생산현황모니터링_금형관리현황_정기점검대상목록] ";
                var paramteters = new List<string>();


                if (req.moldStatusId != 0) paramteters.Add($"@moldStatusId={req.moldStatusId}");
                if (req.moldId != 0) paramteters.Add($"@moldId={req.moldId}");
                if (req.inspectionId != 0) paramteters.Add($"@inspectionId={req.inspectionId}");



                query = query + string.Join(",", paramteters);
                var _result = await _db.생산현황모니터링_금형관리현황_정기점검대상목록.FromSqlRaw(query).ToListAsync();

                var Res = new Response<IEnumerable<MoldStatusRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<MoldStatusRes001>>()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message.ToString(),
                    Data = null
                };

                return Res;
            }
        }

        public async Task<Response<IEnumerable<MoldStatusRes002>>> moldCleanList(MoldStatusReq001 req)
        {
            try
            {

                var query = $"exec [dbo].[생산현황모니터링_금형관리현황_세척대상목록] ";
                var paramteters = new List<string>();


                if (req.moldStatusId != 0) paramteters.Add($"@moldStatusId={req.moldStatusId}");
                if (req.moldId != 0) paramteters.Add($"@moldId={req.moldId}");
                if (req.cleanId != 0) paramteters.Add($"@cleanId={req.cleanId}");



                query = query + string.Join(",", paramteters);
                var _result = await _db.생산현황모니터링_금형관리현황_세척대상목록.FromSqlRaw(query).ToListAsync();

                var Res = new Response<IEnumerable<MoldStatusRes002>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<MoldStatusRes002>>()
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
