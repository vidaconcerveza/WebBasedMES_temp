using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.Hardware;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Hardware;

namespace WebBasedMES.Services.Repositories.HardwareManage
{
    public interface IHardwareRepository
    {
        Task<Response<IEnumerable<PLCDataResponse>>> GetAllData();
        Task<Response<IEnumerable<PLCDataResponse>>> GetPLCAllData(string uid, string block);
        Task<Response<PLCDataResponse>> GetPLCData(string uid, string block);
        Task<Response<bool>> CreateData(PLCDataResponse data, int id);
        Task<Response<bool>> UpdateData(PLCDataResponse data, int id);
        Task<Response<bool>> DeleteData(int id);
    }
}
