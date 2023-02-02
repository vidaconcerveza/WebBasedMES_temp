using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Mold;

namespace WebBasedMES.Services.Repositories.MoldManage
{
    public interface IMoldRepository
    {
        //Mold
        Task<Response<IEnumerable<MoldResponse002>>> GetMoldsBySearch(MoldRequest002 _req);
        Task<Response<IEnumerable<MoldResponse>>> GetMoldMasterList(MoldRequest002 _req);

        Task<Response<MoldResponse>> GetMold (int id);
        Task<Response<IEnumerable<MoldResponse>>> GetMolds (int code);
        Task<Response<IEnumerable<MoldResponse>>> CreateMold (MoldRequest facility);
        Task<Response<IEnumerable<MoldResponse>>> UpdateMold (MoldRequest facility, int id);
        //Task<Response<IEnumerable<string>>> UpdateMolds (List<MoldResponse> facility);
        Task<Response<IEnumerable<MoldResponse>>> DeleteMolds (int[] id);


        Task<Response<MoldGroupResponse>> GetMoldGroup (MoldGroupRequest req);
        Task<Response<IEnumerable<MoldGroupResponse>>> GetMoldGroups (MoldGroupRequest req);
        Task<Response<IEnumerable<MoldGroupInterface>>> GetMoldGroupDetail (MoldGroupRequest req);
        Task<Response<bool>> CreateMoldGroup (MoldGroupRequest req);
        Task<Response<bool>> UpdateMoldGroup (MoldGroupRequest req);
        Task<Response<bool>> DeleteMoldGroups (MoldGroupRequest req);



        Task<Response<IEnumerable<MoldCleaningResponse>>> GetMoldCleanings(MoldCleaningRequest req);
        Task<Response<bool>> UpdateMoldCleaing(MoldCleaningRequest req);


        Task<Response<IEnumerable<MoldDraftResponse>>> GetMoldDrafts(MoldDraftRequest req);
        Task<Response<bool>> UpdateMoldDraft(MoldDraftRequest req);


        Task<Response<MoldLocationResponse>> GetMoldLocation(MoldLocationRequest req);
        Task<Response<bool>> UpdateMoldLocation(MoldLocationRequest req);


        Task<Response<IEnumerable<MoldUsageListResponse>>> GetMoldUsageList (MoldUsageRecordRequest req);
        Task<Response<IEnumerable<MoldUsageRecordResponse>>> GetMoldUsageRecords(MoldUsageRecordRequest req);





        Task Save ();
    }
}