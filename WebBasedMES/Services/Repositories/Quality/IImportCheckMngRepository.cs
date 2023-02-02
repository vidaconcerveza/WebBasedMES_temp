using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public interface IImportCheckMngRepository
    {
        // 1) 수입검사현황 메인화면
        Task<Response<IEnumerable<ImportCheckRes001>>> importCheckMstList(ImportCheckReq001 importCheckReq001);
        // 2) 메인화면에서 목록 중 하나 더블클릭 팝업
        Task<Response<ImportCheckRes002>> importCheckMstPop(ImportCheckReq001 importCheckReq001);
        // 2) 상세내역
        Task<Response<IEnumerable<ImportCheckRes003>>> importCheckInspectionList(ImportCheckReq001 importCheckReq001);

        Task Save ();
    }
}