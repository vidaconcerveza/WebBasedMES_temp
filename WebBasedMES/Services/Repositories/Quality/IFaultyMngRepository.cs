using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.BaseInfo;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public interface IFaultyMngRepository
    {
        // 1) 기타 불량등록 메인 화면 :목록
        Task<Response<IEnumerable<FaultyRes001>>> faultyMstList(FaultyReq001 faultyReq001);
        // 2) 등록/수정화면 기타불량등록 master 정보
        Task<Response<IEnumerable<FaultyLotResponse>>> faultyMstPop(FaultyReq001 faultyReq001);
        // 2) 등록/수정화면 목록
        Task<Response<IEnumerable<FaultyRes003>>> faultyProductList(FaultyReq001 faultyReq001);

        ////EtcDefective crud
        Task<Response<EtcDefectiveResponse>> getEtcDefective(FaultyReq001 _req);
        Task<Response<bool>> CreateEtcDefective(CreateEtcDefectiveRequest req);
        Task<Response<bool>> UpdateEtcDefective(CreateEtcDefectiveRequest faultyReq001);
        Task<Response<bool>> DeleteEtcDefective(FaultyReq001 faultyReq001);
        Task<Response<IEnumerable<FaultyLotResponse>>> EtcDefectiveUpdateSearchLot(FaultyReq001 _req);

        ////EtcDefective crud
        Task<Response<IEnumerable<DefectivePopupResponse>>> GetDefectives(DefectivePopupRequest req);
        Task<Response<IEnumerable<ProductPopupResponse>>> GetProducts(ProductPopupRequest req);

        Task Save ();
    }
}