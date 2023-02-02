using System.Collections.Generic;
using System.Threading.Tasks;
using WebBasedMES.ViewModels;
using WebBasedMES.ViewModels.Process;

namespace WebBasedMES.Services.Repositories.ProcessManage
{
    public interface IProcessRepository
    {

        #region 생산공정관리
        Task<Response<IEnumerable<WorkOrderProducePlanResponse001>>> GetWorkOrderProducePlans(WorkOrderProducePlanRequest001 param);
        Task<Response<WorkOrderProducePlanResponse002>> GetWorkOrderProducePlan(WorkOrderProducePlanRequest001 param);
        Task<Response<IEnumerable<ProcessProgressResponse001>>> GetProcessProgresses(ProcessProgressRequest001 param);
        Task<Response<IEnumerable<ProcessDefectiveResponse001>>> GetProcessDefectives(ProcessDefectiveRequest001 param);
        Task<Response<IEnumerable<ProcessNotWorkResponse001>>> GetProcessNotWorks(ProcessNotWorkRequest001 param);
        Task<Response<IEnumerable<ProductItemsResponse002>>> GetProductItems(ProductItemsRequest001 param);


        //작업시작버튼클릭
        Task<Response<EventResult>> EventWorkStart(ProcessNotWorkRequest001 param);
        //작업중지조회
        Task<Response<IEnumerable<ProcessNotWorkResponse002>>> GetNotWorks(ProcessNotWorkRequest002 param);
        //작업중지저장
        Task<Response<EventResult>> EventWorkStop(ProcessNotWorkRequest002 param); 
        //비가동수정조회
        Task<Response<ProcessNotWorkResponse003>> GetNotWork(ProcessNotWorkRequest003 param);
        //비가동수정저장
        Task<Response<EventResult>> SaveNotWork(ProcessNotWorkRequest003 param);

        Task<Response<bool>> DeleteNotWorks(ProcessNotWorkRequest002 param);

        //불량버튼조회
        Task<Response<ProcessDefectiveResponse002>> GetDefective(ProcessDefectiveRequest002 param);
        Task<Response<IEnumerable<ProcessDefectiveResponse001>>> GetDefectives(ProcessDefectiveRequest002 param);
        //불량버튼저장
        Task<Response<EventResult>> CreateDefective(ProcessDefectiveRequest002 param);

        //불량수정조회
        //Task<Response<ProcessDefectiveResponse003>> GetProcessDefective(ProcessDefectiveRequest003 param);
        //불량수정저장
        Task<Response<EventResult>> UpdateDefective(ProcessDefectiveRequest003 param);
        Task<Response<EventResult>> DeleteDefectives(ProcessDefectiveRequest003 param);


        //투입품목수정조회
        Task<Response<IEnumerable<ProductItemsResponse002>>> GetProductItems(ProductItemsRequest002 param);
        Task<Response<ProductItemsResponse003>> GetProductItem(ProductItemsRequest002 param);

        //투입품목수정저장

        Task<Response<EventResult>> UpdateProductItems(ProductItemsResponse003 param);

        //Task<Response<EventResult>> SaveProductItems(ProductItemsRequest002 param);


        //작업종료버튼조회
        Task<Response<WorkStopResponse001>> GetWorkStop(WorkStopRequest001 param);
        //작업종료저
        Task<Response<EventResult>> SaveWorkStop(WorkStopRequest001 param);

        //작업수정

        Task<Response<WorkStopResponse001>> GetWorkEdit(WorkStopRequest001 param);
        //작업수정
        Task<Response<EventResult>> SaveWorkEdit(WorkStopRequest001 param);

        Task<Response<IEnumerable<ProcessNotWorkTypeResponse>>> GetProcessNotWorkType();
        Task<Response<IEnumerable<ProcessProgressSelectResponse>>> GetProcessProgressSelect(ProcessProgressSelectRequest param);

        #endregion 생산공정관리

        // 비가동내역
        Task<Response<IEnumerable<ProcessNotWorkQueryResponse>>> GetProcessNotWorksQuery(ProcessNotWorkQueryRequest param);


        Task Save();
    }
}
