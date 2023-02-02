using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Lot;

namespace WebBasedMES.Services.Repositories.Lots
{
    public interface ILotCountMngRepository
    {

        //LotCount crud
        Task<Response<bool>> CreateLotCount(LotCountRequestCrud lotCountRequest);
        Task<Response<bool>> UpdateLotCount(LotCountRequestCrud lotCountRequest);
        Task<Response<bool>> DeleteLotCount(LotCountRequestCrud lotCountRequest);
        ////LotCount crud

        Task<Response<bool>> editEvent(LotCountRequestCrud lotCountRequest); //수정이벤트
        //Task<Response<IEnumerable<procesureT1>>> procesureT1s(LotCountRequestCrud lotRequest); //입출고원장리스트
        Task<Response<IEnumerable<procesureT1>>> productList(OrderReq002 OrderReq002); //품목 리스트 테스트
        Task Save ();
    }
}