using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public interface IOutStoreMngRepository
    {

        //////new 품목 선택 팝업
        //Task<Response<IEnumerable<OutStoreRes005>>> productList(OutStoreReq002 OutStoreReq001);
        Task<Response<IEnumerable<OutStoreRes001>>> outStoreMstList(OutStoreReq001 OutStoreReq001);
        ////2) 수주등록 등록/수정 버튼 클릭 팝업 리스트
        Task<Response<IEnumerable<OutStoreRes003>>> outStoreProductList(OutStoreReq001 OutStoreReq001);
        Task<Response<OutStoreRes002>> outStoreMstPop(OutStoreReq001 OutStoreReq001);
        Task<Response<IEnumerable<OutStoreRes004>>> outStoreProductListPop(OutStoreReq001 OutStoreReq001);
        //////new) 등록자 팝업
        ////Task<Response<IEnumerable<OutStoreRes006>>> userList(OutStoreReq003 OutStoreReq003);
        //////new) 거래처
        ////Task<Response<IEnumerable<OutStoreRes007>>> partnerList(OutStoreReq004 OutStoreReq004);


        ////outStore crud
        Task<Response<int>> CreateOutStore(OutStoreRequestCrud outStoreRequest);
        Task<Response<bool>> UpdateOutStore(OutStoreRequestCrud outStoreRequest);
        Task<Response<bool>> DeleteOutStore(OutStoreRequestCrud outStoreRequest);
        ////outStore crud

        ////outStoreProduct crud
        Task<Response<bool>> CreateOutStoreProduct(OutStoreProductRequestCrud OutStoreProductRequest);
        Task<Response<bool>> DeleteOutStoreProduct(OutStoreProductRequestCrud OutStoreProductRequest);
        Task<Response<bool>> UpdateOutStoreProduct(OutStoreProductRequestCrud OutStoreProductRequest);


        Task<Response<bool>> OutStoreProductEditSave(OutStoreProductRequestCrud OutStoreProductRequestCrud);

        Task Save ();
    }
}