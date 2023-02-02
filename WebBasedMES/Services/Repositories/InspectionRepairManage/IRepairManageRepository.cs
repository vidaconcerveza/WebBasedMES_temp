using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InspectionRepair;

namespace WebBasedMES.Services.Repositories.InspectionRepairManage
{
    public interface IRepairManageRepository
    {
        //1. 수리 요청
        Task<Response<IEnumerable<RepairAsksResponse>>> GetRepairAsks(RepairAskRequest req);
        Task<Response<RepairAsksResponse>> GetRepairAsk(RepairAskRequest req);
        Task<Response<bool>> CreateRepairAsk(RepairAskCreateUpdateRequest req);
        Task<Response<bool>> UpdateRepairAsk(RepairAskCreateUpdateRequest req);
        Task<Response<bool>> DeleteRepairAsk(RepairAskRequest req);

        //3. 수리 일지
        Task<Response<RepairLogResponse>> GetRepairLog(RepairLogRequest req);
        Task<Response<bool>> CreateUpdateRepairLog(RepairLogCreateUpdateRequest req);

        //2. 설비 수리 일지
        //수리현황... 
        Task<Response<IEnumerable<RepairLogsResponse>>> GetRepairLogs(RepairAskRequest req);


        Task Save();
    }
}
