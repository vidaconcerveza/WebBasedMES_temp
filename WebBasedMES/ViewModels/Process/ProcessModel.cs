using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBasedMES.ViewModels.Process
{
    //공정진행현황
    [Keyless]
    public class WorkOrderProducePlanResponse001
    {
        /*Key*/
        public int? workerOrderProducePlanId { get; set; }
        public int workerOrderId { get; set; }
        /*Key외*/
        public string? workOrderNo { get; set; } //작업지시번호
        public string? workOrderDate { get; set; }//작업지시일
        public int? workOrderSequence { get; set; }//작업순서
        public string? productCode { get; set; }//제품코드
        public string? productClassification { get; set; }//제품 구분
        public string? productName { get; set; }//제품이름
        public string? productStandard { get; set; }//규격
        public string? productUnit { get; set; }//단위
        public int? productWorkQuantity { get; set; }//지시수량
        public int? productGoodQuantity { get; set; }//양품
        public int? productDefectiveQuantity { get; set; }//불량품
        public int? productionQuantity { get; set; }
        public int? totalElapsedTime { get; set; }//소요시간(분)
        public string? workOrderStatus { get; set; }//상태
        public bool workOrderProcessCheck { get; set; }

        ////ProcessProgress
        //public int productionQuantity { get; set; } //생산수량
        //public int workStatus { get; set; }//작업상태
        //public string workStartDateTime { get; set; }//작업시작
        //public string workEndDateTime { get; set; }//작업완료

        //public int processOrder { get; set; }//공정순서
        //public string processCode { get; set; }//공정코드
        //public string processName { get; set; }//공정이름
        //public int isOutSourcing { get; set; }         //자체/외주
        //public string partnerName { get; set; }         //거래처
        //public string facilitiesCode { get; set; }      //설비코드
        //public string facilitiesName { get; set; }      //설비이름
        //public string moldCode { get; set; }            //금형코드
        //public string moldName { get; set; }            //금형이름
        //public string workerName { get; set; }           //작업자
        //public int processWorkQuantity { get; set; }    //지시수량
        //public int productionQuanitity { get; set; }    //생산수량
        //public int processElapsedTime { get; set; }     //소요시간(분)
        //public int processCheck { get; set; }        //공정검사여부
        //public int processCheckResult { get; set; }  //공정검사결과
        //public string LOT { get; set; }
        //public string workProcessMemo { get; set; }
    }
    public class WorkOrderProducePlanRequest001
    {
        /*Key*/
        public int productId { get; set; }

        /*Key외*/
        public string workOrderStartDate { get; set; }   //작업지시일
        public string workOrderEndDate { get; set; }     //작업지시일
        public string workOrderNo { get; set; }         //작업지시번호
        public string workOrderStatus { get; set; }     //상태
        public string productCode { get; set; }         //품목코드
        public string productName { get; set; }         //품목이름

        public int workerOrderId { get; set; }
    }



    public class WorkOrderProducePlanResponse002
    {
        public int WorkerOrderId { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int ProductWorkQuantity { get; set; }
        public bool WorkOrderProcessCheck { get; set; }

        
        public IEnumerable<ProcessProgressResponse002> ProcessProgresses { get; set; }
    }

    public class ProcessProgressResponse002
    {
        public int ProcessProgressId { get; set; }
        public string WorkStatus { get; set; }
        public string WorkStartDateTime { get; set; }
        public string WorkEndDateTime { get; set; }
        public int ProcessOrder { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public bool IsOutSourcing { get; set; }
        public string PartnerName { get; set; }
        public string FacilitiesCode { get; set; }
        public string FacilitiesName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string WorkerName { get; set; }
        public int ProcessWorkQuantity { get; set; }
        public int ProductionQuantity { get; set; }
        public int ProductGoodQuantity { get; set; }
        public int ProductDefectiveQuantity { get; set; }
        public bool ProcessCheck { get; set; }
        public string ProcessCheckResult { get; set; }
        public string ProductLOT { get; set; }
        public string WorkProcessMemo { get; set; }
        public int ElapsedTime { get; set; }
    }


    //조회
    [Keyless]
    public class ProcessProgressResponse001
    {
        /*Key*/
        //public int processProgressId { get; set; }
        public string workStatus { get; set; }//작업상태
        public string workStartDateTime { get; set; }//작업시작
        public string workEndDateTime { get; set; }//작업완료

        /*Key외*/
        //public int workOrderSequence { get; set; }//작업순서

        //public string workOrderStatus { get; set; }//상태



        public int processOrder { get; set; }//공정순서
        public string processCode { get; set; }//공정코드
        public string processName { get; set; }//공정이름
        public bool isOutSourcing { get; set; }         //자체/외주

        public string workerName { get; set; }

        public int processWorkQuantity { get; set; }    //지시수량
       // public int productWorkQuantity { get; set; }//지시수량
        public int productionQuantity { get; set; }//생산수량
        public int productGoodQuantity { get; set; }//양품
        public int productDefectiveQuantity { get; set; }//불량품

        public string partnerName { get; set; }         //거래처
       
      //  public string facilitiesCode { get; set; }      //설비코드
      //  public string facilitiesName { get; set; }      //설비이름
       // public string moldCode { get; set; }            //금형코드
      //  public string moldName { get; set; }            //금형이름
        //public int? productionQuantity { get; set; }    //생산수량
        public bool processCheck { get; set; }        //공정검사여부
        public string processCheckResult { get; set; }  //공정검사결과
        public string productLOT { get; set; }
        public string workProcessMemo { get; set; }
    }

    public class ProcessProgressRequest001
    {
        public int workerOrderId { get; set; }
        public int workerOrderProducePlanId { get; set; }
        ///*Key*/
        //public int productId { get; set; }

        ///*Key외*/
        //public string workOrderStartDate { get; set; }   //작업지시일
        //public string workOrderEndDate { get; set; }     //작업지시일
        //public string workOrderNo { get; set; }         //작업지시번호
        //public string workOrderStatus { get; set; }     //상태
        //public string productCode { get; set; }         //품목코드
        //public string productName { get; set; }         //품목이름
    }

    //비가동조회
    [Keyless]
    public class ProcessNotWorkResponse001
    {
        public int? processNotWorkId { get; set; }

        public string? shutdownStartDateTime { get; set; }   //시작일시-확인
        public string? shutdownEndDateTime { get; set; }     //종료일시-확인
        public int? downtime { get; set; }                   //소요시간(분)
        public string? processCode { get; set; }             //공정코드
        public string? processName { get; set; }             //공정이름
        public string? shutdownCode { get; set; }            //비가동코드
        public string? shutdownName { get; set; }            //비가동유형
        public string? processShutdownMemo { get; set; }     //비고-확인
    }
    public class ProcessNotWorkRequest001
    {
        public int workerOrderId { get; set; }
        public int processProgressId { get; set; }//공정진행현황Id
        public int[] processProgressIdArray { get; set; }
    }

    [Keyless]
    public class ProcessNotWorkResponse002
    {
        public int processNotWorkId { get; set; }
        public string shutdownStartDateTime { get; set; }   //시작일시-확인
        public string shutdownEndDateTime { get; set; }     //종료일시-확인
        public int downtime { get; set; }                   //소요시간(분)
        public int processProgressId { get; set; }
        public string processCode { get; set; }             //공정코드
        public string processName { get; set; }             //공정이름
        public string shutdownCode { get; set; }            //비가동코드
        public string shutdownName { get; set; }            //비가동유형
        public string processShutdownMemo { get; set; }     //비고-확인

    }
    public class ProcessNotWorkRequest002
    {
        public int processProgressId { get; set; }
        public int[] processProgressIdArray { get; set; }
        public int shutdownCodeId { get; set; }
        public string processShutdownMemo { get; set; }
        public int[] processNotWorkIdArray { get; set; }

    }

    [Keyless]
    public class ProcessNotWorkResponse003
    {
        public int processNotWorkId { get; set; }
        public int processProgressId { get; set; }
        public string shutdownStartDateTime { get; set; }
        public string shutdownEndDateTime { get; set; }
        public string processCode { get; set; }
        public string processName { get; set; }
        public int shutdownCodeId { get; set; }
        public string shutdownCode { get; set; }
        public string shutdownName { get; set; }
        public string processShutdownMemo { get; set; }
    }
    public class ProcessNotWorkRequest003
    {
        public int processId { get; set; }
        public int processNotWorkId { get; set; }
        public int shutdownCodeId { get; set; }
        public int processProgressId { get; set; }

        public string shutdownStartDateTime { get; set; }
        public string shutdownEndDateTime { get; set; }
        public string processCode { get; set; }
        public string processName { get; set; }
        public string shutdownCode { get; set; }
        public string shutdownName { get; set; }
        public string processShutdownMemo { get; set; }
    }

    [Keyless]
    //불량유형조회
    public class ProcessDefectiveResponse001
    {
        public int processProgressId { get; set; }
        public int processDefectiveId { get; set; }
        public string processCode { get; set; }     //공정코드
        public string processName { get; set; }     //공정이름
        public string defectiveCode { get; set; }   //불량코드
        public string defectiveName { get; set; }   //불량유형
        public int defectiveQuantity { get; set; }  //불량수량-확인
        public string defectiveProductMemo { get; set; } //비고-확인
    }
    public class ProcessDefectiveRequest001
    {
        public int workerOrderId { get; set; }
        public int processProgressId { get; set; }//공정진행현황Id
    }

    [Keyless]
    public class ProcessDefectiveResponse002
    {
        public int? processProgressId { get; set; }
        public int processDefectiveId { get; set; }
        public int? defectiveId { get; set; }
        public string? defectiveCode { get; set; }
        public string? defectiveName { get; set; }
        public int? defectiveQuantity { get; set; }
        public string? defectiveProductMemo { get; set; }

        public string processCode { get; set; }
        public string processName { get; set; }
    }
    public class ProcessDefectiveRequest002
    {        
        public int processDefectiveId { get; set; }
        public int processProgressId { get; set; }
        public int defectiveId { get; set; }
        public int defectiveQuantity { get; set; }
        public string defectiveProductMemo { get; set; }
    }

    [Keyless]
    public class ProcessDefectiveResponse003
    {
        public int? defectiveId { get; set; }

        public string? processCode { get; set; }
        public string? processName { get; set; }
        public string? defectiveCode { get; set; }
        public string? defectiveName { get; set; }
        public int? defectiveQuantity { get; set; }
        public string? defectiveProductMemo { get; set; }
    }
    public class ProcessDefectiveRequest003
    {
        public int processProgressId { get; set; }
        public int processId { get; set; }
        public int processDefectiveId { get; set; }
        public int defectiveId { get; set; }

        public int defectiveQuantity { get; set; }
        public string defectiveProductMemo { get; set; }

        public int[] processDefectiveIdArray { get; set; }
    }

    //투입품목
    [Keyless]
    public class ProductItemsResponse001
    {        
        public int? productItemId { get; set; }

        public string? processCode { get; set; }         //공정코드
        public string? processName { get; set; }         //공정이름
        public string? itemCode { get; set; }            //품목코드
        public string? itemClassification { get; set; }  //품목구분
        public string? itemName { get; set; }            //품목이름
        public string? itemStandard { get; set; }        //규격
        public string? itemUnit { get; set; }            //단위
        public int? requiredQuantity { get; set; }       //소요량
        public int? LOSS { get; set; }                   //LOSS
        public int? totalRequiredQuantity { get; set; }  //총소요량 = 소요량+LOSS
        public int? productionQuantity { get; set; }     //생산수량
        public int? totalInputQuantity { get; set; }     //총투입수량
        public string? itemLOT { get; set; }             //LOT
    }
    public class ProductItemsRequest001
    {

        public int workerOrderId { get; set; }
        public int processProgressId { get; set; }//공정진행현황Id
    }

    [Keyless]
    public class ProductItemsResponse002
    {
        public int processProgressId { get; set; }
        public string processCode { get; set; }
        public string processName { get; set; }
        public int itemId { get; set; }
        public string itemCode { get; set; }
        public string itemClassification { get; set; }
        public string itemName { get; set; }
        public string itemStandard { get; set; }
        public string itemUnit { get; set; }
        public string itemLOT { get; set; }
        //public string[] itemLOTs { get; set; }
        public int inventory { get; set; }
        public int totalInputQuantity { get; set; }
        public float totalRequiredQuantity { get; set; }
        public float requiredQuantity { get; set; }
        public int productionQuantity { get; set; }
        public float LOSS { get; set; }
    }

    public class ProductItemsResponse003
    {

        public int processProgressId { get; set; }
        public int itemId { get; set; }

        public string processCode { get; set; }
        public string processName { get; set; }
        
        public string itemCode { get; set; }
        public string itemClassification { get; set; }
        public string itemName { get; set; }
        public string itemStandard { get; set; }
        public string itemUnit { get; set; }
        public float requiredQuantity { get; set; }
        public float LOSS { get; set; }
        public float totalRequiredQuantity { get; set; }
        public int productionQuantity { get; set; }
        public int totalInputQuantity { get; set; }
        public IEnumerable<ProductItemStockInterface> productItemStocks { get; set; }
    }

    public class ProductItemStockInterface
    {
        public int processProgressId { get;set; }
        public int itemId { get; set; }
        public int lotCountId { get; set; }
        public string itemCode { get; set; }
        public string itemClassification { get; set; }
        public string itemName { get; set; }
        public string itemStandard { get; set; }
        public string itemUnit { get; set; }
        public string itemLOT { get; set; }
        public bool isSelected { get; set; }
        public int inputQuantity { get; set; }

        public int inventory { get; set; }
    }

    /*
    public class ProductItemInputQuantityInterface
    {
        public int inputQuantity { get; set; }
    }
    */


    public class ProductItemsRequest002
    {
        public int processProgressId { get; set; }
        public int itemId { get; set; }
        public string itemLOT { get; set; }
        public IEnumerable<ProductItemsInterface> ProductItems { get; set; }
    }

    public class ProductItemsInterface
    {
        public bool iselected { get; set; }
        public int lotId { get; set; }
        public int inputQuantity { get; set; }
    }

    [Keyless]
    public class WorkStopResponse001
    {
        public int processProgressId { get; set; }
        public string workEndDateTime { get; set; } 
        public string workStartDateTime { get; set; }
        public string workStatus { get; set; }
        public int productionQuantity { get; set; } 
        public int processCheckResult { get; set; } 
        public string workProcessMemo { get; set; } 
    }

    public class WorkStopRequest001
    {
        public int processProgressId { get; set; }
        public string workEndDateTime { get; set; }
        public string workStartDateTime { get; set; }
        public int productionQuantity { get; set; }
        public int processCheckResult { get; set; }
        public string workProcessMemo { get; set; }
        public string workStatus { get; set; }
    }


    public class ProcessNotWorkTypeResponse
    {
        public int shutdownCodeId { get; set; }
        public string shutdownName { get; set; }
        public string shutdownCode { get; set; }
    }

    public class ProcessProgressSelectRequest
    {
        public int WorkerOrderId { get; set; }
    }

    public class ProcessProgressSelectResponse
    {
        public int processProgressId { get; set; }
        public int processOrder { get; set; }
        public string processName { get; set; }
        public string processCode { get; set; }
    }

    #region 인터페이스
    public class WorkOrderProducePlansInterface
    {
        /*Key*/
        public int workOrderId { get; set; }
        public int producePlansProcessId { get; set; }

        public int productId { get; set; }
        public int partnerId { get; set; }
        public int facilityId { get; set; }
        public int moldId { get; set; }
        public int registerId { get; set; }

        /*Key외*/
        public int inOutSourcing { get; set; } //자체/외주
        public int processWorkQuantity { get; set; } //지시수량
        public int processElapsedTime { get; set; } //공정검사여부
        public int processCheckResult { get; set; } //공정검사결과

        public ProcessProgress ProcessProgress { get; set; } //공정진행현황

        //LOT,비고
    }
    public class ProcessProgress
    {
        public int productionQuantity { get; set; } //생산수량
        public int workStatus { get; set; }//작업상태
        public string workStartDateTime { get; set; }//작업시작
        public string workEndDateTime { get; set; }//작업완료
    }

    [Keyless]
    public class Items
    {
        public string itemCode { get; set; }
        public string itemClassification { get; set; }
        public string itemName { get; set; }
        public string itemStandard { get; set; }
        public string itemUnit { get; set; }
        public string itemLOT { get; set; }
        public int inventory { get; set; }
        public int isSelected { get; set; } //투입
        public int inputQuantity { get; set; }
    }
    #endregion







    #region 이벤트리턴용
    [Keyless]
    public class EventResult
    {        
        public string result { get; set; }
    }
    #endregion
}
