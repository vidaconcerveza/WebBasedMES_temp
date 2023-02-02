using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public interface IOutOrderMngRepository
    {

        //new 1) 출고등록 메인 화면 :목록
        Task<Response<IEnumerable<OutOrderRes001>>> outOrderMstList(OutOrderReq001 outOrderReq001);
        //new) 1) 출고등록 메인화면 상세내역 리스트
        Task<Response<IEnumerable<OutOrderRes002>>> outOrderProductList(OutOrderReq001 outOrderReq001);
        //new) 2) 출고등록 등록/수정 버튼 팝업 -출고정보
        Task<Response<OutOrderRes003>> outOrderMstPop(OutOrderReq001 outOrderReq001);
        //new) 2) 출고등록 등록/수정 버튼 팝업 -품목리스트
        Task<Response<IEnumerable<OutOrderRes004>>> outOrderProductListPop(OutOrderReq001 outOrderReq001);
        //new) 2) 출고등록 등록/수정 버튼 팝업 -불량유형리스트
        Task<Response<IEnumerable<OutOrderRes005>>> outOrderProductDefectiveListPop(OutOrderReq002 outOrderReq002);
        //new) 3) 2)에서 수주목록에 수주정보 추가버튼 클릭 팝업
        Task<Response<IEnumerable<OutOrderRes006>>> orderProductListPop(OutOrderReq003 outOrderReq003);
        Task<Response<IEnumerable<ProductDetailResponse>>> orderProductLotList(ProductDetailRequest req);
        Task<Response<IEnumerable<OutOrderRes006>>> orderProductListPopProcessList(OutOrderReq002 outOrderRequest);


        ////OutOrder crud
        Task<Response<int>> CreateOutOrder(OutOrderRequestCrud outOrderRequest);
        Task<Response<int>> UpdateOutOrder(OutOrderRequestCrud outOrderRequest);
        Task<Response<bool>> DeleteOutOrder(OutOrderRequestCrud outOrderRequest);
        ////OutOrder crud

        ////outOrderProduct crud
        //Task<Response<bool>> CreateOutOrderProduct(OutOrderProductRequestCrud outOrderProductRequest);
        Task<Response<bool>> CreateOutOrderProductLot(OutOrderProductRequestCrud outOrderProductRequest);
        Task<Response<bool>> CreateOutOrderProductforProduct(OutOrderProductRequestCrud outOrderProductRequest);
        Task<Response<bool>> UpdateOutOrderProduct(OutOrderProductRequestCrud outOrderProductRequest);
        Task<Response<bool>> UpdateOutOrderProductforProduct(OutOrderProductRequestCrud outOrderProductRequest);
        Task<Response<bool>> DeleteOutOrderProduct(OutOrderProductRequestCrud outOrderProductRequest);
        //Task<Response<bool>> StoreOutStoreProductEditSave(StoreOutStoreProductRequestCrud storeOutStoreProductRequest);
        ////outOrderProduct crud
       
        // 불량유형 추가버튼 팝업 저장버튼 event
        Task<Response<bool>> CreateOutOrderProductDefective(OutOrderReq002 outOrderReq002);
        //new) 2) 출고등록 등록/수정 버튼 팝업 -불량유형리스트 삭제
        Task<Response<bool>> DeleteOutOrderProductDefective(OutOrderReq002 outOrderReq002);
        //new) 2) 출고등록 등록/수정 버튼 팝업 -불량유형리스트  수정
        Task<Response<bool>> UpdateOutOrderProductDefective(OutOrderReq002 outOrderReq002);
        // 품목재고 추가버튼 팝업 저장버튼 event
        Task<Response<bool>> CreateOutOrderProductStock(OutOrderReq005 outOrderReq005);

        // 출고 마스터 아이템 품목 재고
        Task<Response<bool>> CreateOutOrderProductsStock2(OutOrderReq006 outOrderReq006);
        // 추가버튼 팝업 저장버튼 event 삭제
        Task<Response<bool>> DeleteOutOrderProductsStock(OutOrderReq006 outOrderReq006);
        // 추가버튼 팝업 저장버튼 event  수정
        Task<Response<bool>> UpdateOutOrderProductsStock(OutOrderReq006 outOrderReq006);
    

        //출고관리
        //1) 출고관리 메인화면 수주목록
        Task<Response<IEnumerable<OutOrderRes007>>> orderMstList(OutOrderReq004 outOrderReq004);
        //출고관리
        //1) 출고관리 메인화면 수주목록 클릭, 수주마스터 정보
        Task<Response<OutOrderRes008>> orderMstPop(OutOrderReq004 outOrderReq004);
        //출고관리
        //1) 수주품목 클릭, 출고마스터리스트
        Task<Response<IEnumerable<OutOrderRes001>>> outOrderMstList2(OutOrderReq001 outOrderReq001);

        //출고현황
        //1) 출고현황 메인화면(품목별)
        Task<Response<IEnumerable<OutOrderRes010>>> outOrderProductList2(OutOrderReq001 outOrderReq001);

        //출고 = 품목상새내역-품목재고
        Task<Response<IEnumerable<OutOrderRes011>>> outOrderProductListLOT(OutOrderReq001 outOrderReq001);


        Task Save ();
    }
}