using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public interface IProcessCheckMngRepository
    {
        // 1) 공정검사현황 메인 화면
        Task<Response<IEnumerable<ProcessCheckRes001>>> processCheckMstList(ProcessCheckReq001 processCheckReq001);
       

        Task Save ();
    }
}