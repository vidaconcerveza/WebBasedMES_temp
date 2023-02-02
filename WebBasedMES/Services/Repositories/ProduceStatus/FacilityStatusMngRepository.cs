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
    public class FacilityStatusMngRepository : IFacilityStatusMngRepository
    {
        private readonly ApiDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public FacilityStatusMngRepository(ApiDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

     

        public async Task<Response<IEnumerable<FacilityStatusRes001>>> facilityStatusList(FacilityStatusReq001 req)
        {
            try
            {

                var query = $"exec [dbo].[생산현황모니터링_설비관리현황_설비관리현황] ";
                var paramteters = new List<string>();


                if (req.facilityStatusId != 0) paramteters.Add($"@facilityStatusId={req.facilityStatusId}");
                if (req.facilityId != 0) paramteters.Add($"@facilityId={req.facilityId}");
                if (req.inspectionId != 0) paramteters.Add($"@inspectionId={req.inspectionId}");



                query = query + string.Join(",", paramteters);
                var _result = await _db.생산현황모니터링_설비관리현황_설비관리현황.FromSqlRaw(query).ToListAsync();

                var Res = new Response<IEnumerable<FacilityStatusRes001>>()
                {
                    IsSuccess = true,
                    ErrorMessage = "",
                    Data = _result
                };

                return Res;

            }
            catch (Exception ex)
            {

                var Res = new Response<IEnumerable<FacilityStatusRes001>>()
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
