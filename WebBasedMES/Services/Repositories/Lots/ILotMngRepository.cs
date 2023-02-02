using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Lot;

namespace WebBasedMES.Services.Repositories.Lots
{
    public interface ILotMngRepository
    {
        //Lot crud
        Task<Response<bool>> CreateLot(LotRequestCrud lotRequest);
        Task<Response<bool>> UpdateLot(LotRequestCrud lotRequest);
        Task<Response<bool>> DeleteLot(LotRequestCrud lotRequest);
        ////Lot crud
        Task<Response<IEnumerable<LotResponse01>>> invenList(LotRequestCrud lotRequest);
        Task<Response<IEnumerable<LotCountResponse01>>> getLots(LotRequestCrud lotRequest);
        Task<Response<IEnumerable<LotCountResponse02>>> getLotCounts(LotRequestCrud lotRequest); //입출고원장리스트

        

        Task Save ();
    }
}