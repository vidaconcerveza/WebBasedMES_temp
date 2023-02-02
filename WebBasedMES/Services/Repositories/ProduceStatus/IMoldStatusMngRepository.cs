using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProduceStatus;

namespace WebBasedMES.Services.Repositories.ProduceStatus
{
    public interface IMoldStatusMngRepository
    {
        // 1) 금형관리현황 메인화면 정기점검 대상목록
        Task<Response<IEnumerable<MoldStatusRes001>>> moldInspectionList(MoldStatusReq001 moldStatusReq001);
        // 1) 금형관리현황 메인화면 세척대상 대상목록
        Task<Response<IEnumerable<MoldStatusRes002>>> moldCleanList(MoldStatusReq001 moldStatusReq001);

        Task Save ();
    }
}