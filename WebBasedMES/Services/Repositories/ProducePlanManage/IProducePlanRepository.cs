using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProducePlan;

namespace WebBasedMES.Services.Repositories.ProducePlanManage
{
    public interface IProducePlanRepository
    {


        Task<Response<IEnumerable<ProducePlanReponse001>>> GetProducePlans(ProducePlanRequest001 param);
        Task<Response<IEnumerable<ProducePlanPopupResponse>>> GetProducePlansPopup(ProducePlanPopupRequest req);
        Task<Response<IEnumerable<ProducePlanDetailResponse>>> GetProducePlansProducts(ProducePlanRequest002 param);
        Task<Response<IEnumerable<ProducePlanReponse002>>> CreateProducePlan(ProducePlanRequest003 param);
        Task<Response<IEnumerable<ProducePlanReponse002>>> UpdateProducePlan(ProducePlanRequest003 param);
        Task<Response<IEnumerable<ProducePlanReponse002>>> DeleteProducePlan(ProducePlanRequest003 param);
        Task<Response<IEnumerable<ProducePlanReponse004>>> GetProducePlanProcesses(ProducePlanRequest004 param);
        Task<Response<IEnumerable<ProducePlanReponse004>>> CreateProducePlanProcesses(ProducePlanRequest004 param);
        Task<Response<IEnumerable<ProducePlanReponse004>>> UpdateProducePlanProcesses(ProducePlanRequest004 param);
        Task<Response<IEnumerable<ProducePlanReponse004>>> DeleteProducePlanProcesses(ProducePlanRequest004 param);
        Task<Response<IEnumerable<ProducePlanReponse005>>> GetProductItems(ProducePlanRequest005 param);
        Task<Response<ProducePlanProductDetailResponse>> GetProducePlanProductDetail(ProducePlanProduceDetailRequest req);

        Task<Response<GetRequiredAmountsResponse002>> GetRequiredAmounts(GetRequiredAmountsRequest001 param);


        Task<Response<IEnumerable<GetReportByProcessWorkOrdersResponse002>>> GetReportByProcessWorkOrders(GetReportByProcessesRequest001 _req);
        Task<Response<IEnumerable<GetReportByProcessesResponse001>>> GetReportByProcesses(GetReportByProcessesRequest001 param);
       // Task<Response<IEnumerable<GetReportByProcessWorkOrderProducePlansResponse001>>> GetReportByProcessWorkOrderProducePlans(GetReportByProcessWorkOrderProducePlansRequest001 param);
        Task<Response<GetReportByProcessWorkOrdersResponse005>> GetReportByProcessWorkOrderProducePlans(GetReportByProcessesRequest001 param);
        Task<Response<GetReportByProcessWorkOrdersResponse004>> GetReportByProcessWorkOrderProducePlansProcess(GetReportByProcessesRequest001 _req);

        Task<Response<IEnumerable<GetProductionManageByProductsResponse001>>> GetProductionManageByProducts(GetProductionManageByProductsRequest001 param);
        Task<Response<IEnumerable<GetProductionManageByPeriodsResponse001>>> GetProductionManageByPeriods(GetProductionManageByPeriodsRequest001 param);
        Task<Response<IEnumerable<GetDefectiveManageByPeriodsResponse001>>> GetDefectiveManageByPeriods(GetDefectiveManageByPeriodsRequest001 param);
        Task<Response<IEnumerable<GetProductionManageByMonthResponse001>>> GetProductionManageByMonth(GetProductionManageByMonthRequest001 param);
        Task<Response<IEnumerable<KpiProductionByHourResponse>>> GetProductionByHour(KpiRequest param);
        Task<Response<IEnumerable<KpiProductionByHourResponse>>> GetDefectiveByHour(KpiRequest param);
        Task<Response<KpiReportResponse>> GetKpiByMonth(KpiRequest param);
        Task<Response<IEnumerable<KpiWorkListResponse>>> GetWorkByMonth(KpiRequest param);

        Task Save();
    }
}
