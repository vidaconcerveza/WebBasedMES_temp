using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public interface IOrderMngRepository
    {

        //new 품목 선택 팝업
        Task<Response<IEnumerable<OrderRes005>>> productList(OrderReq002 OrderReq001);
        //new 1) 수주등록 메인 화면 :목록
        Task<Response<IEnumerable<OrderRes001>>> orderMstList(OrderReq001 OrderReq001);
        //2) 수주등록 등록/수정 버튼 클릭 팝업 리스트
        Task<Response<IEnumerable<OrderRes003>>> orderProductList(OrderReq001 OrderReq001);
        //new) 2) 수주등록 등록/수정 버튼 팝업 -수주정보
        Task<Response<OrderRes002>> orderMstPop(OrderReq001 OrderReq001);
        //new) 2) 수주등록 등록/수정 버튼 팝업 -품목리스트
        Task<Response<IEnumerable<OrderRes004>>> orderProductListPop(OrderReq001 OrderReq001);
        //new) 등록자 팝업
        Task<Response<IEnumerable<OrderRes006>>> userList(OrderReq003 OrderReq003);
        //new) 거래처
        Task<Response<IEnumerable<OrderRes007>>> partnerList(OrderReq004 OrderReq004);


        //order crud
        Task<Response<IEnumerable<OrderPopupResponse>>> GetOrdersPopupBySearch(OrderPopupRequest req);
        Task<Response<int>> CreateOrder(OrderRequestCrud orderRequest);
        Task<Response<bool>> UpdateOrder(OrderRequestCrud orderRequest);
        Task<Response<bool>> DeleteOrder(OrderRequestCrud orderRequest);
        ////order crud

        ////orderProduct crud
        Task<Response<bool>> CreateOrderProduct(OrderProductRequestCrud OrderProductRequest);
        Task<Response<bool>> DeleteOrderProduct(OrderProductRequestCrud OrderProductRequest);
        Task<Response<bool>> UpdateOrderProduct(OrderProductRequestCrud OrderProductRequest);


        Task<Response<bool>> OrderProductEditSave(OrderProductRequestCrud OrderProductRequestCrud);


        Task<Response<bool>> OutOrderFinishEvent(OrderProductRequestCrud req);

        Task Save ();
    }
}