using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Process;

namespace WebBasedMES.Services.Repositories.ProcessManage
{
    public interface IProcessStatusRepository
    {

        // 비가동내역
        Task<Response<IEnumerable<ProcessNotWorkQueryResponse>>> GetProcessNotWorksQuery(ProcessNotWorkQueryRequest res);

        //공정별생산량관리
        Task<Response<IEnumerable<ProductionManageByProcessProcessListResponse>>> GetProductionManageByProcesses(ProductionManageByProcessRequest res);
        Task<Response<IEnumerable<ProductionManageByProcessWorkOrderListResponse>>> GetProductionManageByProcess(ProductionManageByProcessRequest res);

        //일일생산현황
        Task<Response<IEnumerable<DailyProductionResponse>>> GetDailyProductions(DailyProductionRequest param);

        //일일불량현황 SUMMARY
        Task<Response<DailyDefectiveSummaryResponse>> GetDailyDefectiveSummary(DailyDefectiveRequest param);

        //일일불량현황 DETAIL
        Task<Response<IEnumerable<DailyDefectiveDetailResponse>>> GetDailyDefective(DailyDefectiveRequest param);
        Task<Response<IEnumerable<DailyOutOrderResponse>>> GetDailyOutOrder(DailyOutOrderRequest param);





        Task<Response<LotInfoResponse>> GetLotInfo(LotInfoRequest param);
        Task<Response<IEnumerable<LotInfoInputItemResponse>>> GetLotInputItems(LotInfoRequest param);
        Task<Response<IEnumerable<LotInfoProcessDefectiveResponse>>> GetLotProcessDefective(LotInfoRequest param);
        Task<Response<IEnumerable<LotInfoProcessInspectionResponse>>> GetLotProcessInspection(LotInfoRequest param);
        Task<Response<IEnumerable<LotInfoProductOutResponse>>> GetLotOutProduct(LotInfoRequest param);
        Task<Response<IEnumerable<ProductionManageByFacilityResponse>>> GetProductionManageByFacility(ProductionManageByFacilityRequest _req);


        Task Save();
    }
}
