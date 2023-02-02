using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;

namespace WebBasedMES.Services.Repositories.InAndOut
{
    public interface IInvenMngRepository
    {

        Task<Response<IEnumerable<InvenMngModelRes0001>>> getProgressList(InvenMngModelReq0001 InvenMngModelReq0001);

        Task Save ();
    }
}