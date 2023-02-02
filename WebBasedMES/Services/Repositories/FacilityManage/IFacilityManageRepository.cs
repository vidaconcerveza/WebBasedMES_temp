using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.FacilityManage;

namespace WebBasedMES.Services.Repositories.FacilityManage
{
    public interface IFacilityManageRepository
    {
        Task<Response<FacilityBaseInfoResponse>> GetFacilityBaseInfo(FacilityBaseInfoRequest req);
        Task<Response<IEnumerable<FacilityBaseInfoResponse>>> GetFacilityBaseInfos(FacilityBaseInfoRequest req);
        Task<Response<bool>> CreateFacilityBaseInfo(FacilityBaseInfoRequest req);
        Task<Response<bool>> UpdateFacilityBaseInfo(FacilityBaseInfoRequest req);
        Task<Response<bool>> DeleteFacilityBaseInfo(FacilityBaseInfoRequest req);

        Task<Response<FacilityControlResponse>> GetFacilityControl(FacilityControlRequest req);
        Task<Response<IEnumerable<FacilityControlResponse>>> GetFacilityControls(FacilityControlRequest req);
        Task<Response<bool>> UpdateFacilityControl(FacilityControlRequest req);
        Task<Response<bool>> CreateFacilityControl(FacilityControlRequest req);
        Task<Response<bool>> DeleteFacilityControl(FacilityControlRequest req);


        //ErrorLog
        Task<Response<IEnumerable<FacilityErrorLogResponse>>> GetFacilityErrorLog(FacilityErrorLogRequest req);


        //
        Task<Response<IEnumerable<FacilityOperationResponse>>> GetFacilityOperations(FacilityOperationRequest req);
        Task<Response<FacilityOperationResponse>> GetFacilityOperation(FacilityOperationRequest req);
        Task<Response<bool>> UpdateFacilityOperation(FacilityOperationRequest req);
        Task<Response<bool>> CreateFacilityOperation(FacilityOperationRequest req);
        Task<Response<bool>> DeleteFacilityOperation(FacilityOperationRequest req);


    }
}
