using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProduceStatus;

namespace WebBasedMES.Services.Repositories.ProduceStatus
{
    public interface IFacilityWorkStatusMngRepository
    {
        // 1) 설비가동현황 메인화면
        Task<Response<IEnumerable<FacilityWorkStatusRes001>>> facilityWorkStatusList(FacilityWorkStatusReq001 facilityWorkStatusReq001);
       

        Task Save ();
    }
}