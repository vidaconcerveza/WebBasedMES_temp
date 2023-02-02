using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.ProducePlan;

namespace WebBasedMES.Services.Repositories.ProducePlanManage
{
    public interface IWorkOrderRepository
    {
        Task<Response<IEnumerable<WorkOrderReponse001>>> GetWorkOrders(WorkOrderRequest001 param);
        Task<Response<IEnumerable<WorkOrderReponse001>>> GetWorkOrdersMain(WorkOrderRequest001 param);
        Task<Response<IEnumerable<WorkOrderReponse001>>> CreateWorkOrder(WorkOrderRequest002 param);
        Task<Response<IEnumerable<WorkOrderReponse001>>> UpdateWorkOrder(WorkOrderRequest002 param);
        Task<Response<IEnumerable<WorkOrderReponse001>>> DeleteWorkOrder(WorkOrderRequest002 param);
        Task<Response<WorkOrderResponse>> GetWorkOrderDetail(WorkOrderRequest _req);
        Task<Response<IEnumerable<WorkOrderResponse005>>> GetWorkOrderProducePlans(WorkOrderRequest005 param);
        Task Save();
    }
}
