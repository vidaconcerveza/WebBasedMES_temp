using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProduceStatus;

namespace WebBasedMES.Services.Repositories.ProduceStatus
{
    public interface IOrderProduceStatusMngRepository
    {
        // 1) 수주대비 생산현황 메인화면
        Task<Response<IEnumerable<OrderProduceStatusRes001>>> orderProduceStatusList(OrderProduceStatusReq001 orderProduceStatusReq001);
       

        Task Save ();
    }
}