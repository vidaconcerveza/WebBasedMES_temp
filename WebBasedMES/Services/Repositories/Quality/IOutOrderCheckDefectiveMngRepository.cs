using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public interface IOutOrderCheckDefectiveMngRepository
    {
        // 1) 출하검사 불량현황 메인화면
        Task<Response<IEnumerable<OutOrderCheckDefectiveRes001>>> outOrderCheckDefectiveMstList(OutOrderCheckDefectiveReq001 outOrderCheckDefectiveReq001);
        // 2) 메인화면에서 목록 중 하나 더블클릭 팝업
        Task<Response<OutOrderCheckDefectiveRes002>> outOrderCheckDefectiveMstPop(OutOrderCheckDefectiveReq001 outOrderCheckDefectiveReq001);
        // 2) 상세내역
        Task<Response<IEnumerable<OutOrderCheckDefectiveRes003>>> outOrderCheckDefectiveList(OutOrderCheckDefectiveReq001 outOrderCheckDefectiveReq001);

        Task Save ();
    }
}