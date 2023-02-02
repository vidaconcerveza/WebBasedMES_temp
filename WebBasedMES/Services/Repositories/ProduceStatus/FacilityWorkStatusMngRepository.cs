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
    public class FacilityWorkStatusMngRepository : IFacilityWorkStatusMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public FacilityWorkStatusMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

     

        public async Task<Response<IEnumerable<FacilityWorkStatusRes001>>> facilityWorkStatusList(FacilityWorkStatusReq001 req)
        {
            try
            {

                var query = $"exec [dbo].[생산현황모니터링_설비가동현황_설비가동현황] ";
                var paramteters = new List<string>();

               
                if (req.facilityWorkStatusId != 0) paramteters.Add($"@facilityWorkStatusId={req.facilityWorkStatusId}");
                if (req.facilityId != 0) paramteters.Add($"@facilityId={req.facilityId}");
                if (req.moldId != 0) paramteters.Add($"@moldId={req.moldId}");
             


                query = query + string.Join(",", paramteters);
                var _result = await _db.생산현황모니터링_설비가동현황_설비가동현황.FromSqlRaw(query).ToListAsync();

                var Res = new Response<IEnumerable<FacilityWorkStatusRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<FacilityWorkStatusRes001>>()
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
