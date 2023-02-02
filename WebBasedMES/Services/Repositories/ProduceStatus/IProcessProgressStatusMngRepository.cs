using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProduceStatus;

namespace WebBasedMES.Services.Repositories.ProduceStatus
{
    public interface IProcessProgressStatusMngRepository
    {
        // 1) 공정 진행현황 메인화면
        Task<Response<IEnumerable<ProcessProgressStatusRes001>>> processProgressStatusList(ProcessProgressStatusReq001 processProgressStatusReq001);
        Task Save ();
    }
}