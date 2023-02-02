using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProduceStatus;

namespace WebBasedMES.Services.Repositories.ProduceStatus
{
    public interface IFacilityStatusMngRepository
    {
        // 1) 설비관리현황 메인화면
        Task<Response<IEnumerable<FacilityStatusRes001>>> facilityStatusList(FacilityStatusReq001 facilityStatusReq001);
       

        Task Save ();
    }
}