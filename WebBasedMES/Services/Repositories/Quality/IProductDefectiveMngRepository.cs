using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Quality;

namespace WebBasedMES.Services.Repositories.Quality
{
    public interface IProductDefectiveMngRepository
    {
        // 1) 제품별 불량현황 메인화면
        Task<Response<IEnumerable<ProductDefectiveRes001>>> productDefectiveMstList(ProductDefectiveReq001 productDefectiveReq001);
       

        Task Save ();
    }
}