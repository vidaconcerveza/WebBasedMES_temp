using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProduceStatus;

namespace WebBasedMES.Services.Repositories.ProduceStatus
{
    public interface IProducePlanProduceStatusMngRepository
    {
        // 1) 계획대비 생산현황 메인화면
        Task<Response<IEnumerable<ProducePlanProduceStatusRes001>>> producePlanProduceStatusList(ProducePlanProduceStatusReq001 producePlanProduceStatusReq001);
       

        Task Save ();
    }
}