using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.ProducePlan
{
    //조회
    [Keyless]

    public class WorkOrderRequest
    {
        public int WorkerOrderId { get; set; }
    }

    public class WorkOrderResponse
    {
        public int WorkerOrderId { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }           //작업지시일*
        public string WorkOrderStatus { get; set; }         //상태
        public int WorkOrderSequence { get; set; }     
        public int ProducePlansProductId { get; set; }
        public string ProductionPlanNo { get; set; }

        public int ProductId { get; set; }
        public string ProductCode { get; set; }             //품목코드*
        public string ProductClassification { get; set; }   //품목구분
        public string ProductName { get; set; }             //품목이름
        public string ProductStandard { get; set; }         //규격
        public string ProductUnit { get; set; }             //단위
        
        public int ProductPlanQuanity { get; set; }         //계획수량
        public int ProductPlanBacklog { get; set; }
        public int ProductWorkQuantity { get; set; }
        public string RegisterDate { get; set; }  
        public string RegisterId { get; set; }    
        public string RegisterName { get; set; }            //등록자
        public string WorkOrderMemo { get; set; }           //비고
       //계획잔량

        public IEnumerable<WorkerOrderProducePlansInterface> WorkerOrderProducePlans { get; set; }
    }

    public class WorkOrderReponse001
    {
        /*Key*/
        public int? workerOrderId { get; set; }            //작업지시Id
        //public string registerId { get; set; }              //등록자Id
        //public int productId { get; set; }              //품목Id
        //public int producePlanId { get; set; }           //생산계획Id
        /*Key외*/
        public string? workOrderNo { get; set; }             //작업지시번호*
        public string? workOrderDate { get; set; }           //작업지시일*
        //public bool? workOrderProcessCheck { get; set; }     //작업지시공정검사여부*
        public string? workOrderStatus { get; set; }         //상태
        public int? workOrderSequence { get; set; }          //작업순서*
        public int? productWorkQuantity { get; set; }        //품목-작업지시 수량*
        public string? registerDate { get; set; }            //등록일*    
        public string? registerName { get; set; }            //등록자
        public string? workOrderMemo { get; set; }           //비고
        public string? productCode { get; set; }             //품목코드*
        public string? productClassification { get; set; }   //품목구분
        public string? productName { get; set; }             //품목이름
        public string? productStandard { get; set; }         //규격
        public string? productUnit { get; set; }             //단위
        public int? productPlanQuanity { get; set; }         //계획수량
        public int? productPlanBacklog { get; set; }         //계획잔량
        public string? productionPlanNo { get; set; }           //생산계획번호

        public string facilityName { get; set; }
        public string processName { get; set; }
    }
    public class WorkOrderRequest001
    {
        /*Key*/
        public int productId { get; set; }
        public string registerId { get; set; }

        /*Key외*/
        public string registerStartDate { get; set; }   //등록일
        public string registerEndDate { get; set; }     //등록일
        public string workOrderNo { get; set; }         //작업지시번호
        public string workOrderStatus { get; set; }     //상태
        public string productCode { get; set; }         //품목코드
        public string productName { get; set; }         //품목이름
        public string registerName { get; set; }        //등록자
        public string userNo { get; set; }              //등록자코드
        public string userFullName { get; set; }        //이름

    }
    //등록/수정
    public class WorkOrderReponse002
    {

    }
    public class WorkOrderRequest002
    {
        /*Key*/
        public int[] workerOrderIds { get; set; }
        public int workerOrderId { get; set; }
        public int producePlansProductId { get; set; }
        public int productId { get; set; }
        public string registerId { get; set; }

        /*Key외*/
        public string workOrderDate { get; set; }           //작업지시일*
        public int workOrderSequence { get; set; }          //작업순서*
        public string productionPlanNo { get; set; }        //생산계획번호

        public int productPlanQuantity { get; set; }        //계획수량
        public int productPlanBacklog { get; set; }         //계획잔량
        public int productWorkQuantity { get; set; }        //지시수량*
        public string workOrderStatus { get; set; }         //상태
        public string registerDate { get; set; }            //등록일
        public string registerName { get; set; }            //등록자
        public string workOrderMemo { get; set; }           //비고
        public IEnumerable<WorkerOrderProducePlansInterface> workerOrderProducePlans { get; set; }
    }

    public class WorkerOrderProducePlansInterface
    {
        public int workerOrderProducePlanId { get; set; }
        public int producePlansProcessId { get; set; }
        public int? processId { get; set; }
        public int processOrder { get; set; }
        public string processCode { get; set; }
        public string processName { get; set; }
        public bool isOutSourcing { get; set; }
        public int? partnerId { get; set; } = 0;
        public string partnerName { get; set; }
        public int? facilityId { get; set; } = 0;
        public string facilitiesCode { get; set; }
        public string facilitiesName { get; set; }

        public int? moldId { get; set; } = 0;
        public string moldName { get; set; }
        public string moldCode { get; set; }
        public string workerId { get; set; }
        public string workerName { get; set; }
        public int processPlanQuantity { get; set; }
        public int processPlanBacklog { get; set; }

        public bool processCheck { get; set; }
        public int processWorkQuantity { get; set; }
        public int productionQuantity { get; set; }
        public int productDefectiveQuantity { get; set; }
        public int productGoodQuantity { get; set; }
        public int processElapsedTime { get; set; }
        public string processCheckResult { get; set; }
        public int productId { get; set; }
    }

    //삭제
    public class WorkOrderReponse003
    {

    }
    public class WorkOrderRequest003
    {
        public int workerOrderId { get; set; }
    }

    //공정목록삭제
    public class WorkOrderReponse004
    {

    }
    public class WorkOrderRequest004
    {
        public int workOrderProducePlanId { get; set; }
    }

    //목록-공정목록
    [Keyless]
    public class WorkOrderResponse005
    {
        /*Key*/
        //public int workerOrderId { get; set; }
        //public int workOrderProducePlanId { get; set; }
        //public int producePlanProcessId { get; set; }
        //public int productId { get; set; }
        //public int partnerId { get; set; }
        //public int facilityId { get; set; }
        //public int moldId { get; set; }
        //public string registerId { get; set; }
        /*Key외*/
        public int? processOrder { get; set; }           //공정순서
        public string? processCode { get; set; }         //공정코드
        public string? processName { get; set; }         //공정이름
        public int? isOutSourcing { get; set; }         //자체/외주    
        public string? partnerName { get; set; }         //거래처
        public string? facilitiesCode { get; set; }      //설비코드
        public string? facilitiesName { get; set; }      //설비이름
        public string? moldCode { get; set; }            //금형코드
        public string? moldName { get; set; }            //금형이름
        public string? workerName { get; set; }           //작업자
        public int? processPlanQuantity { get; set; }    //계획수량
        public int? processPlanBacklog { get; set; }     //계획잔량
        public int? processWorkQuantity { get; set; }    //지시수량
        public int? productionQuanitity { get; set; }    //생산수량
        public int? productGoodQuantity { get; set; }    //양품
        public int? productDefectiveQuantity { get; set; } //불량품
        public int? processElapsedTime { get; set; }     //소요시간(분)
        public int? processCheck { get; set; }        //공정검사여부
        public int? processCheckResult { get; set; }  //공정검사결과
    }
    public class WorkOrderRequest005
    {
        public int workerOrderId { get; set; }
    }
    #region 인터페이스
    public class WorkOrderProducePlansInterface
    {
        /*Key*/
        public int workerOrderId { get; set; }
        public int producePlansProcessId { get; set; }
        public int productProcessId { get; set; } //공정순서,공정쪽 가져오기 위하여

        public int productId { get; set; }
        public int partnerId { get; set; }
        public int facilityId { get; set; }
        public int moldId { get; set; }
        public string registerId { get; set; }

        /*Key외*/
        public int inOutSourcing { get; set; } //자체/외주
        public int processWorkQuantity { get; set; } //지시수량
        public int processElapsedTime { get; set; } //공정검사여부
        public int processCheckResult { get; set; } //공정검사결과
    }

    #endregion

}
