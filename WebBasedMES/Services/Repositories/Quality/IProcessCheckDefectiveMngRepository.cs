using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public interface IProcessCheckDefectiveMngRepository
    {
        // 1) 공정검사 불량현황 메인화면
        Task<Response<IEnumerable<ProcessCheckDefectiveRes001>>> processCheckDefectiveMstList(ProcessCheckDefectiveReq001 processCheckDefectiveReq001);
        // 2) 메인화면에서 목록 중 하나 더블클릭 팝업
        Task<Response<ProcessCheckDefectiveRes002>> processCheckDefectiveMstPop(ProcessCheckDefectiveReq001 processCheckDefectiveReq001);
        // 2) 상세내역
        Task<Response<IEnumerable<ProcessCheckDefectiveRes003>>> processCheckDefectiveList(ProcessCheckDefectiveReq001 processCheckDefectiveReq001);

        Task Save ();
    }
}