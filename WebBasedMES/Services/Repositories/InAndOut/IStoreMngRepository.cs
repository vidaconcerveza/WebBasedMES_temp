using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public interface IStoreMngRepository
    {

        //new 1) 입고등록 메인 화면 :목록
        Task<Response<IEnumerable<StoreRes001>>> storeMstList(StoreReq001 storeReq001);
        //2) 입고등록 등록/수정 버튼 클릭 팝업 리스트
        Task<Response<IEnumerable<StoreRes002>>> storeOutStoreProductList(StoreReq001 storeReq001);
        //new) 2) 입고등록 등록/수정 버튼 팝업 -입고정보
        Task<Response<StoreRes003>> storeMstPop(StoreReq001 storeReq001);
        //new) 2) 입고등록 등록/수정 버튼 팝업 -품목리스트
        Task<Response<IEnumerable<StoreRes004>>> storeOutStoreProductListPop(StoreReq001 storeReq001);
        //new) 2)등록/수정 버튼 클릭화면 -품목 상세내역- 추가버튼 - 입고등록 팝업
        Task<Response<IEnumerable<StoreRes013>>> outStoreProductListPop(StoreReq005 storeReq005);

        //Store crud
        Task<Response<int>> CreateStore(StoreRequestCrud storeRequest);
        Task<Response<bool>> UpdateStore(StoreRequestCrud storeRequest);
        Task<Response<bool>> DeleteStore(StoreRequestCrud storeRequest);
        ////outStore crud

        //storeOutStoreProduct crud
        Task<Response<bool>> CreateStoreOutStoreProduct(StoreOutStoreProductRequestCrud storeOutStoreProductRequest);
        Task<Response<bool>> UpdateStoreOutStoreProduct(StoreOutStoreProductRequestCrud storeOutStoreProductRequest);
        Task<Response<bool>> DeleteStoreOutStoreProduct(StoreOutStoreProductRequestCrud storeOutStoreProductRequest);
        Task<Response<bool>> StoreOutStoreProductEditSave(StoreOutStoreProductRequestCrud storeOutStoreProductRequest);

        //new) 불량유형 추가버튼 팝업
        Task<Response<IEnumerable<StoreRes005>>> defectiveList(StoreReq002 storeReq002);
        //new) 검사항목 추가버튼 팝업
        Task<Response<IEnumerable<StoreRes006>>> inspectionList(StoreReq002 storeReq002);
        // 불량유형 추가버튼 팝업 저장버튼 event
        Task<Response<bool>> CreateStoreOutStoreProductDefective(StoreReq003 storeReq003);
        // 검사항목 추가버튼 팝업 저장버튼 event
        Task<Response<bool>> CreateStoreOutStoreProductInspection(StoreReq004 storeReq004);
        //new) 2) 입고등록 등록/수정 버튼 팝업 -불량유형리스트
        Task<Response<IEnumerable<StoreRes007>>> storeOutStoreProductDefectiveListPop(StoreReq003 storeReq003);
        //new) 2) 입고등록 등록/수정 버튼 팝업 -검사항목리스트
        Task<Response<IEnumerable<StoreRes006>>> storeOutStoreProductInspectionTypeListPop(StoreReq004 storeReq004);
        //new) 2) 입고등록 등록/수정 버튼 팝업 -불량유형리스트 삭제
        Task<Response<bool>> DeleteStoreOutStoreProductDefective(StoreReq003 storeReq003);
        //new) 2) 입고등록 등록/수정 버튼 팝업 -검사항목리스트 삭제
        Task<Response<bool>> DeleteStoreOutStoreProductInspection(StoreReq004 storeReq004);



        //3)입고관리 조회
        Task<Response<IEnumerable<StoreRes008>>> getOutStoreMsts(OutStoreReq001 OutStoreReq001);
        Task<Response<StoreRes009>> getStoreOutStore(OutStoreReq001 OutStoreReq001);
        Task<Response<IEnumerable<StoreRes010>>> getStoreOutStoreItems(OutStoreReq001 OutStoreReq001);
        Task<Response<IEnumerable<StoreRes011>>> storeMstList2(OutStoreReq001 OutStoreReq001);
        Task<Response<IEnumerable<StoreRes011>>> storeMstList3(StoreReq001 storeReq001);
        Task<Response<IEnumerable<StoreRes012>>> storeOutStoreProductList2(StoreReq001 storeReq001);
        Task<Response<IEnumerable<StoreRes014>>> storeOutStoreProductList3(StoreReq001 storeReq001);
        //마감처리
        Task<Response<bool>> eventOutStoreFinish(StoreRequestCrud req);
        //3)입고관리 조회

        Task Save ();
    }
}