using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public interface IStoreCheckDefectiveMngRepository
    {
        // 1) 수입검사 불량현황 메인화면
        Task<Response<IEnumerable<StoreCheckDefectiveRes001>>> storeCheckDefectiveMstList(StoreCheckDefectiveReq001 storeCheckDefectiveReq001);
        // 2) 메인화면에서 목록 중 하나 더블클릭 팝업
        Task<Response<StoreCheckDefectiveRes002>> storeCheckDefectiveMstPop(StoreCheckDefectiveReq001 storeCheckDefectiveReq001);
        // 2) 상세내역
        Task<Response<IEnumerable<StoreCheckDefectiveRes003>>> storeCheckDefectiveList(StoreCheckDefectiveReq001 storeCheckDefectiveReq001);

        Task Save ();
    }
}