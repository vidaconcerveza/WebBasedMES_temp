using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Mold;

namespace WebBasedMES.Services.Repositories.MoldManage
{
    public interface IPreventiveMaintenanceRepository
    {
        //예방보존 기준정보

        Task<Response<PreventiveMaintenanceFacilityResponse>> GetPreventiveMaintnanceFacility(PreventiveMaintenanceFacilityRequest req);
        Task<Response<IEnumerable<PreventiveMaintenanceFacilityResponse>>> GetPreventiveMaintnanceFacilitys(PreventiveMaintenanceFacilityRequest req);
        Task<Response<bool>> UpdatePreventiveMaintnanceFacilitys(PreventiveMaintenanceFacilityRequest req);
        Task<Response<bool>> CreatePreventiveMaintnanceFacilitys(PreventiveMaintenanceFacilityRequest req);
        Task<Response<bool>> DeletePreventiveMaintnanceFacilitys(PreventiveMaintenanceFacilityRequest req);


        Task<Response<PreventiveMaintenanceMoldResponse>> GetPreventiveMaintnanceMold(PreventiveMaintenanceMoldRequest req);
        Task<Response<IEnumerable<PreventiveMaintenanceMoldResponse>>> GetPreventiveMaintnanceMolds(PreventiveMaintenanceMoldRequest req);
        Task<Response<bool>> UpdatePreventiveMaintnanceMolds(PreventiveMaintenanceMoldRequest req);
        Task<Response<bool>> CreatePreventiveMaintnanceMolds(PreventiveMaintenanceMoldRequest req);
        Task<Response<bool>> DeletePreventiveMaintnanceMolds(PreventiveMaintenanceMoldRequest req);



        Task<Response<IEnumerable<PreventiveMaintenanceFacilityResponse>>> GetPreventiveMaintenanceManageFacilitys(PreventiveMaintenanceFacilityRequest req);
        Task<Response<IEnumerable<PreventiveMaintenanceMoldResponse>>> GetPreventiveMaintenanceManageMolds(PreventiveMaintenanceMoldRequest req);

        //예방보존 
        Task Save ();
    }
}