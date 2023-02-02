using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Barcode;
using WebBasedMES.ViewModels.Mold;

namespace WebBasedMES.Services.Repositories.BarcodeManage
{
    public interface IBarcodeRepository
    {
        //예방보존 기준정보

        Task<Response<IEnumerable<InBarcodeResponse>>> GetInBarcodeIssueList(InBarcodeRequest req);
        Task<Response<InBarcodeResponse>> GetInBarcodeIssueData(InBarcodeRequest req);
        Task<Response<bool>> UpdateInBarcodeIssue(InBarcodeRequest req);


        Task<Response<IEnumerable<OutBarcodeResponse>>> GetOutBarcodeIssueList(OutBarcodeRequest req);
        Task<Response<OutBarcodeResponse>> GetOutBarcodeIssueData(OutBarcodeRequest req);
        Task<Response<bool>> UpdateOutBarcodeIssue(OutBarcodeRequest req);


        Task<Response<IEnumerable<ProcessBarcodeResponse>>> GetProcessBarcodeIssueList(ProcessBarcodeRequest req);
        Task<Response<ProcessBarcodeResponse>> GetProcessBarcodeIssueData(ProcessBarcodeRequest req);
        Task<Response<bool>> UpdateProcessBarcodeIssue(ProcessBarcodeRequest req);



        Task<Response<IEnumerable<BarcodeMasterResponse>>> GetMasterBarcodeIssueList(BarcodeMasterRequest req);
        Task<Response<BarcodeMasterResponse>> GetMasterBarcodeIssueData(BarcodeMasterRequest req);
        Task<Response<bool>> UpdateMasterBarcodeIssue(BarcodeMasterRequest req);

        Task Save ();
    }
}