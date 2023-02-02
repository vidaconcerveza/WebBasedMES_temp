using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public interface IOutOrderCheckMngRepository
    {
        // 1) 출하검사현황 메인화면
        Task<Response<IEnumerable<OutOrderCheckRes001>>> outOrderCheckMstList(OutOrderCheckReq001 outOrderCheckReq001);
       

        Task Save ();
    }
}