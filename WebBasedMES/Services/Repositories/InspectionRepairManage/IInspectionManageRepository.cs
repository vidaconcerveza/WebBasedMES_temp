using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.InspectionRepair;

namespace WebBasedMES.Services.Repositories.InspectionRepairManage
{
    public interface IInspectionManageRepository
    {
        //1. 설비일상점검 리스트 조회
        Task<Response<IEnumerable<FacilityInspectionsResponse>>> GetFacilityInspections(FacilityInspectionRequest req);
        Task<Response<FacilityInspectionResponse>> GetFacilityInsepction(FacilityInspectionRequest req);
        Task<Response<bool>> CreateFacilityInspection(FacilityInspectionCreateUpdateRequest req);
        Task<Response<bool>> UpdateFacilityInspection(FacilityInspectionCreateUpdateRequest req);
        Task<Response<bool>> DeleteFacilityInspection(FacilityInspectionRequest req);

        Task<Response<IEnumerable<FacilityInspectionItemInterface>>> GetFacilityInspectionItems(FacilityInspectionRequest req);



        Task<Response<IEnumerable<MoldInspectionsResponse>>> GetMoldInspections(MoldInspectionRequest req);
        Task<Response<MoldInspectionResponse>> GetMoldInsepction(MoldInspectionRequest req);
        Task<Response<bool>> CreateMoldInspection(MoldInspectionCreateUpdateRequest req);
        Task<Response<bool>> UpdateMoldInspection(MoldInspectionCreateUpdateRequest req);
        Task<Response<bool>> DeleteMoldInspection(MoldInspectionRequest req);
        Task<Response<IEnumerable<MoldInspectionItemInterface>>> GetMoldInspectionItems(MoldInspectionRequest req);


        Task Save();
    }
}
