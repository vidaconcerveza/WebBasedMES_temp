using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InAndOutMng.InAndOut;
using WebBasedMES.ViewModels.Lot;

namespace WebBasedMES.Services.Repositories.Monitor
{
    public interface IMonitorManageRepository
    {
        Task<Response<IEnumerable<ProcessProgressRecordResponse>>> GetProcessProgressRecord();
        Task<Response<IEnumerable<ProcessOperationRecordResponse>>> GetProcessOperationRecord();
        Task<Response<IEnumerable<ProductionRecordByContractResponse>>> GetProductionRecordByContract();
        Task<Response<IEnumerable<ProductionRecordByPlanResponse>>> GetProductionRecordByPlan();
        Task<Response<IEnumerable<FacilityManageRecordResponse>>> GetFacilityManageRecord();
        Task<Response<IEnumerable<MoldManageRecordInspectionResponse>>> GetMoldManageRecordInspection();
        Task<Response<IEnumerable<MoldManageRecordCleaningResponse>>> GetMoldManageRecordWash();

        Task<Response<IEnumerable<FacilityStatus>>> GetFacilityOperationRecord();
        Task<Response<IEnumerable<FacilityStatus>>> GetFacilityTempRecord();
        Task<Response<bool>> GetFacilityOperationRecordSave();
        Task Save ();
    }
}