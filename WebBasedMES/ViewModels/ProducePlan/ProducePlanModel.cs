using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels.BaseInfo;

namespace WebBasedMES.ViewModels.ProducePlan
{
    #region 생산계획관리-목록
    [Keyless]
    public class ProducePlanReponse001
    {
        /*Key값*/
        public int? producePlanId { get; set; }  //생산계획Id
        public string? registerId { get; set; }  //등록자id
        public int? uploadFileId { get; set; }   //첨부파일Id
        /*Key값 외*/
        public string? productionPlanNo { get; set; }    //생산계획번호
        public string? productionPlanStartDate { get; set; }
        public string? productionPlanEndDate { get; set; }
        public int? productionPlanProductCount { get; set; } //품목(수)
        public int? productionPlanTotalAmount { get; set; }  //총 수량
        public string? productionPlanStatus { get; set; }    //상태
        public string? registerDate { get; set; }            //등록일        
        public string? registerName { get; set; }            //등록자
        public string? productionPlanMemo { get; set; }      //비고
        public string? uploadFileName { get; set; }          //첨부파일
        public string? uploadFileUrl { get; set; }
    }

    public class ProducePlanRequest001
    {
        /*Key값*/
        public string registerId { get; set; } = "";
        public int productId { get; set; }

        /*Key값 외*/
        public string productionPlanNo { get; set; }    //생산계획번호
        public string registerStartDate { get; set; }   //등록일
        public string registerEndDate { get; set; }     //등록일
        public string productionPlanStatus { get; set; }   //상태
        public string productCode { get; set; }         //품목코드
        public string productName { get; set; }         //품목이름        
        public string registerName { get; set; }        //등록자
        public string userNo { get; set; }              //등록자id
        public string userFullName { get; set; }        //등록자이름
    }


    public class ProducePlanPopupRequest
    {
        public string searchInput { get; set; }
        public string productionPlanStatus { get; set; }

    }

    public class ProducePlanPopupResponse
    {
        public int ProducePlanId { get; set; }
        public int ProductPlansProductId { get; set; }
        public string ProductionPlanNo { get; set; }
        public string ProductionPlanStatus { get; set; }
        public int Priority { get; set; }
        public string ProductionPlanDate { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int ProductionPlanQuantity { get; set; }
        public int ProductPlanBacklog { get; set; }
        public string ProductionPlanMemo { get; set; }

        public IEnumerable<ProducePlanProcessInterface002> ProducePlansProcesses { get; set; }
        public IEnumerable<ProducePlanProcessInterface002> WorkerOrderProducePlans { get; set; }

    }

    #endregion

    #region 생산계획관리-목록-상세내역
    [Keyless]
    public class ProducePlanReponse002
    {
        /*Key값*/
        public int producePlanId { get; set; }          //생산계획Id
        public int producePlansProductId { get; set; }  //생산계획마스터아이템Id
        /*Key값 외*/
        public int priority { get; set;}                    //우선순위
        public string productionPlanDate { get; set; }      //계획일자
        public string productCode { get; set; }             //품목코드
        public string productClassification { get; set; }   //품목구분
        public string productName { get; set; }             //품목이름
        public string productStandard { get; set; }         //규격
        public string productUnit { get; set; }             //단위
        public int productionPlanQuantity { get; set; }     //계획수량
    }

    public class ProducePlanDetailResponse
    {
        public int ProducePlanId { get; set; }
        public string ProductionPlanNo { get; set; }
        public string ProductionPlanStartDate { get; set; }
        public string ProductionPlanEndDate { get; set; }
        public string ProductionPlanStatus { get; set; }
        public string RegisterDate { get; set; }
        public string RegisterName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public string ProductionPlanMemo { get; set; }
        public string UploadFileName { get; set; }
        public string UploadFileUrl { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public IEnumerable<ProducePlanProductInterface> ProducePlanProducts { get; set; }
    }

    public class ProducePlanRequest002
    {
        public int producePlanId { get; set; }
    }
    #endregion

    #region 생산계획관리-품목등록/수정
    [Keyless]
    public class ProducePlanReponse003
    {
        /*Key값*/
        public int producePlanId { get; set; }  //생산계획Id
        public string pegisterId { get; set; }  //등록자id
        public int uploadFileId { get; set; }   //첨부파일Id
        /*Key값 외*/
        public string productionPlanNo { get; set; }    //생산계획번호
        public string productionPlanStartDate { get; set; }
        public string productionPlanEndDate { get; set; }
        public int productionPlanProductCount { get; set; } //품목(수)
        public int productionPlanTotalAmount { get; set; }  //총 수량
        public string productionPlanStatus { get; set; }    //상태
        public string registerDate { get; set; }            //등록일        
        public string registerName { get; set; }            //등록자
        public string productionPlanMemo { get; set; }      //비고
        public string uploadFileName { get; set; }          //첨부파일
        public IEnumerable<ProducePlanProductInterface> ProducePlanProducts { get; set; }   //품목목록
    }
    public class ProducePlanRequest003
    {
        /*Key값*/
        public int producePlanId { get; set; }
        public string registerId { get; set; }
        public int uploadFileId { get; set; }
        public int[] producePlanIds { get; set; }

        /*Key값 외*/
        public string productionPlanStartDate { get; set; } //생산계획 시작일*
        public string productionPlanEndDate { get; set; }   //생산계획 종료일*
        public string registerDate { get; set; }            //등록일
        public string registerName { get; set; }            //등록자
        public string productionPlanMemo { get; set; }      //비고
        public string uploadFileName { get; set; }          //첨부파일(uploadFileName)
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public IEnumerable<ProducePlanProductInterface> producePlanProducts { get; set; }   //품목목록
    }


    


    #endregion

    #region 생산계획관리-공정등록
    [Keyless]
    public class ProducePlanReponse004
    {
        /*Key값*/
        public int? producePlansProductId { get; set; }
        public int? producePlansProcessId { get; set; }
        public int productProcessId { get; set; }
        /*Key값 외*/
        public int? processOrder { get; set; }               //공정순서
        public string? processCode { get; set; }             //공정코드
        public string? processName { get; set; }             //공정이름
        public string? productCode { get; set; }             //품목코드
        public string? productClassification { get; set; }   //품목구분
        public string? productName { get; set; }             //품목이름
        public string? productStandard { get; set; }         //규격
        public string? productUnit { get; set; }             //단위
        public int? inventory { get; set; }                  //재고수량
        public int? processPlanQuantity { get; set; }        //생산계획수량*
    }
    public class ProducePlanRequest004
    {
        public int producePlansProductId { get; set; }
        public int[] producePlansProductIds { get; set; }
        public IEnumerable<ProducePlanProcessInterface> ProducePlanProcesses { get; set; }   //공정목록
    }
    #endregion

    #region 생산계획관리-투입품목목록
    [Keyless]
    public class ProducePlanReponse005
    {
        public string? processCode { get; set; }         //공정코드
        public string? processName { get; set; }         //공정이름
        public string? itemCode { get; set; }            //품목코드
        public string? itemClassification { get; set; }  //품목구분
        public string? itemName { get; set; }            //품목이름
        public string? itemStandard { get; set; }        //규격
        public string? itemUnit { get; set; }            //단위
        public float? requiredQuantity { get; set; }       //소요량
        public float? LOSS { get; set; }                   //LOSS
        public float? totalRequiredQuantity { get; set; }  //총소요량
        public int? processPlanQuantity { get; set; }    //계획수량
        public int? inventory { get; set; }              //재고수량
        public int? totalInputQuantity { get; set; }     //총투입수량
    }
    public class ProducePlanRequest005
    {
        public int producePlansProcessId { get; set; }
        public int producePlansProductId { get; set; }
        public int productProcessId { get; set; }
    }
    #endregion

    #region 인터페이스
    public class ProducePlanProductInterface
    {
        public object producePlansProductId { get; set; } 
        public int orderProductId { get; set; }         //수주정보에서 가져오거나 아래의 ProductId에서 가져오거
        public int productId { get; set; }              //ProductId=>등록하기위하여 추가
        /*Key값 외*/
        public int priority { get; set; }                   //우선순위*
        public string productionPlanDate { get; set; }      //계획일자*                
        public string productCode { get; set; }             //품목코드*
        public string ProductClassification { get; set; }   //품목구분
        public string productName { get; set; }             //품목이름
        public string productStandard { get; set; }         //규격
        public string productUnit { get; set; }             //단위        
        public int productOrderCount { get; set; } = 0;     //수주수량 : 수주정보에서 가져왔을때만 정보 있음
        public int optimumStock { get; set; }               //적정재고
        public int inventory { get; set; }                  //현재고 : Inventorys내의 storeCount
        public int productPlanQuantity { get; set; }        //계획수량*>스토리보드상에 위에선 productionPlanQuantity라고 되어있음 확인할
        public IEnumerable<ProducePlanProcessInterface> producePlanProcesses { get; set; }   //공정목록
    }

    public class ProducePlanProcessInterface
    {
        /*Key값*/
        public int producePlansProcessId { get; set; }
        public int? producePlansProductId { get; set; }
        public int productProcessId { get; set; }
        /*Key값 외*/

        public int processId { get; set; }
        public int processOrder { get; set; }               //공정순서
        public string processCode { get; set; }             //공정코드
        public string processName { get; set; }             //공정이름
        public string productCode { get; set; }             //품목코드
        public string productClassification { get; set; }   //품목구분
        public string productName { get; set; }             //품목이름
        public string productStandard { get; set; }         //규격
        public string productUnit { get; set; }             //단위
        public int inventory { get; set; }                  //재고수량
        public int processPlanQuantity { get; set; }        //생산계획수량*

        public IEnumerable<ProductItemInterface2> ProcessInputItems { get; set; }
        public bool Checked { get; set; } = false;
    }

    public class ProducePlanProcessInterface002
    {
        public int ProductProcessId { get; set; }
        public int ProducePlansProcessId { get; set; }
        public int ProcessOrder { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public int ProcessPlanQuantity { get; set; }
        public int ProcessPlanBacklog { get; set; }
        public bool ProcessCheck { get; set; }
        public string ProcessCheckResult { get; set; }
        public int FacilityId { get; set; }
        public int MoldId  { get; set; }
        public string  WorkerId { get; set; }
        public int PartnerId { get; set; }
        public int ProcessWorkQuantity  { get; set; }
        public int ProductId { get; set; }
    }
    public class ProducePlanProduceDetailRequest
    {
        public int ProductId { get; set; } 
        public int ProducePlansProductId { get; set; }
    }

    public class ProducePlanProductDetailResponse
    {
        public int ProducePlansProductId { get; set; } = 0;
        public IEnumerable<ProducePlanProcessInterface> ProducePlanProcesses { get; set; }   //공정목록
        public IEnumerable<ProducePlanProcessItemInterface> ProducePlanProcessItems { get; set; }
    }

    public class ProducePlanProcessItemInterface
    {
        public string? processCode { get; set; }         //공정코드
        public string? processName { get; set; }         //공정이름
        public string? itemCode { get; set; }            //품목코드
        public string? itemClassification { get; set; }  //품목구분
        public string? itemName { get; set; }            //품목이름
        public string? itemStandard { get; set; }        //규격
        public string? itemUnit { get; set; }            //단위
        public float? requiredQuantity { get; set; }       //소요량
        public float? LOSS { get; set; }                   //LOSS
        public int? totalRequiredQuantity { get; set; }  //총소요량
        public int? processPlanQuantity { get; set; }    //계획수량
        public int? inventory { get; set; }              //재고수량
        public int? totalInputQuantity { get; set; }     //총투입수량
    }



    #endregion

    #region 소요량산출
    [Keyless]
    public class GetRequiredAmountsResponse001
    {
        public int? productItemId { get; set; }
        
        public string? itemCode { get; set; }
        public string? itemClassification { get; set; }
        public string? itemName { get; set; }
        public string? itemStandard { get; set; }
        public string? itemUnit { get; set; }
        public int? optimumStock { get; set; }
        public int? inventory { get; set; }
        public int? totalInputQuantity { get; set; }
    }

    public class GetRequiredAmountsResponse002
    {
        public int producePlanId { get; set; }
        public IEnumerable<ProducePlanProductInterface2> ProducePlanProducts { get; set; }
        public IEnumerable<ProducePlanProcessItemInterface2> ProducePlanProductsInputItems { get; set; }
    }
    public class ProducePlanProductInterface2
    {
        /*Key값*/
        public int producePlansProductId { get; set; }   //return용
        public int productId { get; set; }              //ProductId=>등록하기위하여 추가
        /*Key값 외*/
        public string productCode { get; set; }             //품목코드*
        public string ProductClassification { get; set; }   //품목구분
        public string productName { get; set; }             //품목이름
        public string productStandard { get; set; }         //규격
        public string productUnit { get; set; }             //단위        
        public int productPlanQuantity { get; set; }        //계획수량*>스토리보드상에 위에선 productionPlanQuantity라고 되어있음 확인할
    }
    public class ProducePlanProcessItemInterface2
    {
        public string? itemCode { get; set; }            //품목코드
        public string? itemClassification { get; set; }  //품목구분
        public string? itemName { get; set; }            //품목이름
        public string? itemStandard { get; set; }        //규격
        public string? itemUnit { get; set; }            //단위
        public int OptimumStock { get; set; }
        public float? requiredQuantity { get; set; }       //소요량
        public float loss { get; set; }
        public int productPlanQuantity { get; set; }
        public int? inventory { get; set; }              //재고수량
        public float? totalInputQuantity { get; set; }     //총투입수량
    }


    public class GetRequiredAmountsRequest001
    {
        public int producePlanId { get; set; }
    }
    #endregion

    #region 공정별 작업일보관리
    [Keyless]
    public class GetReportByProcessesResponse001
    {
        public int workerOrderProducePlanId { get; set; }
        public int workerOrderId { get; set; }
        public string workOrderDate { get; set; }
        public int processId { get; set; }
        public string processCode { get; set; }
        public string processName { get; set; }
        public string processMemo { get; set; }
    }
    public class GetReportByProcessesRequest001
    {
        public int workerOrderId { get; set; }
        public int workerOrderProducePlanId { get; set; }
        public int processId { get; set; }
        public string workOrderStartDate { get; set; }
        public string workOrderEndDate { get; set; }
    }

    //미사용
    [Keyless]
    public class GetReportByProcessWorkOrdersResponse001
    {
        public int? workerOrderId { get; set; }
        public int? registerId { get; set; }

        public string? workOrderNo { get; set; }
        public string? workOrderDate { get; set; }
        public int? workOrderSequence { get; set; }
        public string? productCode { get; set; }
        public string? productClassification { get; set; }
        public string? productName { get; set; }
        public string? productStandard { get; set; }
        public string? productUnit { get; set; }
        public int? productWorkQuantity { get; set; }
        public string? workOrderStatus { get; set; }
        public string? registerDate { get; set; }
        public string? registerName { get; set; }
        public string? productionPlanNo { get; set; }
        public bool IsOutSourcing { get; set; }
    }

    public class GetReportByProcessWorkOrdersResponse002
    {
        public int workerOrderId { get; set; }
        public int WorkderOrderProducePlanId { get; set; }
        public string workOrderNo { get; set; }
        public string workOrderDate { get; set; }
        public int workOrderSequence { get; set; }
        public bool IsOutSourcing { get; set; }
        public string PartnerName { get; set; }
        public string FacilitiesCode { get; set; }
        public string FacilitiesName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string WorkerName { get; set; }
        public bool ProcessCheck { get; set; }
        public int? ProcessWorkQuantity { get; set; }
        public int ProcessId { get; set; }
    }

    public class GetReportByProcessWorkOrdersResponse003
    {
        public int WorkerOrderId { get; set; }
        public int WorkderOrderProducePlanId { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public int WorkOrderSequence { get; set; }
        public bool IsOutSourcing { get; set; }

        public string  ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int ProductWorkQuantity { get; set; }
        public string WorkOrderStatus { get; set; }
        public string RegisterDate { get; set; }
        public string RegisterName { get; set; }
        public string ProductionPlanNo { get; set; }
    }

    public class GetReportByProcessWorkOrdersResponse004
    {
        public bool IsOutSourcing { get; set; }
        public string PartnerName { get; set; }
        public string WorkStatus { get; set; }
        public string FacilitiesCode { get; set; }
        public string FacilitiesName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public IEnumerable<GetReportByProcessWorkOrderProducePlanInputItem> InputItems { get; set; }
        public string WorkerName { get; set; }
        public int ProcessWorkQuantity { get; set; }
        public bool ProcessCheck { get; set; }
    }

    public class GetReportByProcessWorkOrdersResponse005
    {
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessClassification { get; set; }
        public string ProcessName { get; set; }
        public IEnumerable<GetReportByProcessWorkOrdersResponse003> WorkerOrderProducePlans { get; set; }
       // public IEnumerable<GetReportByProcessWorkOrdersResponse004> WorkOrderDetailList { get; set; }

    }

    public class GetReportByProcessWorkOrderProducePlanInputItem
    {
        public bool IsOutSourcing { get; set; }
        public string PartnerName { get; set; }
        public string WorkStatus { get; set; }
        public string FacilitiesCode { get; set; }
        public string FacilitiesName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string ItemCode { get; set; }
        public string ItemClassification { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemStandard { get; set; }
        public string ItemUnit { get; set; }
        public float RequiredQuantity { get; set; }
        public float LOSS { get; set; }
        public float TotalRequiredQuantity { get; set; }
        public string WorkerName { get; set; }
        public int? ProcessWorkQuantity { get; set; }
        public bool ProcessCheck { get; set; }
        public string Flag { get; set; }
    }


    public class GetReportByProcessWorkOrdersRequest001
    {
        public int workOrderId { get; set; }
    }
    [Keyless]
    public class GetReportByProcessWorkOrderProducePlansResponse001
    {
        public int? inOutSourcing { get; set; }
        public string? partnerName { get; set; }
        public string? workOrderStatus { get; set; }        
        public string? facilitiesCode { get; set; }
        public string? facilitiesName { get; set; }
        public string? moldCode { get; set; }
        public string? moldName { get; set; }

        public string? itemCode { get; set; }
        public string? itemClassification { get; set; }
        public string? itemName { get; set; }
        public string? itemStandard { get; set; }
        public string? itemUnit { get; set; }
        public int? requiredQuantity { get; set; }
        public int? LOSS { get; set; }
        public int? totalRequiredQuantity { get; set; }
        public string? workerName { get; set; }
        public int? processWorkQuantity { get; set; }
        public int? processCheck { get; set; }
    }
    public class GetReportByProcessWorkOrderProducePlansRequest001
    {
        public int workOrderId { get; set; }
    }
    #endregion

    #region 제품별 생산량 관리
    [Keyless]
    public class GetProductionManageByProductsResponse001
    {
        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
        public string workOrderNo { get; set; }
        public string workStartDateTime { get; set; }
        public string workEndDateTime { get; set; }
        public string workerName { get; set; }
        public int productionQuantity { get; set; }
        public int productionGoodQuantity { get; set; }
        public int productDefectiveQuantity { get; set; }
        public int processElapsedTime { get; set; }
        public int downtime { get; set; }
        public string productLOT { get; set; }
    }
    public class GetProductionManageByProductsRequest001
    {
        public int workOrderId { get; set; }
        public int productId { get; set; }
        public int commonCodeId { get; set; }

        public string workOrderStartDate { get; set; }
        public string workOrderEndDate { get; set; }
        public string productClassification { get; set; }
        public string workOrderNo { get; set; }
    }
    #endregion

    #region 기간별 생산량 관리
    [Keyless]
    public class GetProductionManageByPeriodsResponse001
    {
        public int processProgressId { get; set; }
        public string workStartDateTime { get; set; }
        public string workEndDateTime { get; set; }
        public string workOrderNo { get; set; }
        public string workerName { get; set; }
        public int processElapsedTime { get; set; }
        public int? downtime { get; set; }
        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }                
        public int productionQuantity { get; set; }
        public int productionGoodQuantity { get; set; }
        public int productDefectiveQuantity { get; set; }                
        public string productLOT { get; set; }
    }
    public class GetProductionManageByPeriodsRequest001
    {
        public int workOrderId { get; set; }
        public int productId { get; set; }
        public int commonCodeId { get; set; }

        public string workOrderStartDate { get; set; }
        public string workOrderEndDate { get; set; }
        public string productClassification { get; set; }
        public string workOrderNo { get; set; }
    }
    #endregion

    #region 기간별 불량수량 관리
    [Keyless]
    public class GetDefectiveManageByPeriodsResponse001
    {
        public string defectiveDate { get; set; }
        public string defectiveCode { get; set; }
        public string defectiveName { get; set; }
        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
        public int produceDefectiveQuantity { get; set; }
        public string type { get; set; }        
        public string productLOT { get; set; }
        public DateTime dateFlag { get; set; }
    }
    public class GetDefectiveManageByPeriodsRequest001
    {        
        public int productId { get; set; }
        public int defectiveId { get; set; }
                
        public string defectiveStartDate { get; set; }
        public string defectiveEndDate { get; set; }
        public string productClassification { get; set; }
    }
    #endregion

    #region 월별 생산량 관리
    [Keyless]
    public class GetProductionManageByMonthResponse001
    {
        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
		public int jan { get; set; }
        public int feb { get; set; }
        public int mar { get; set; }
        public int apr { get; set; }
        public int may { get; set; }
        public int jun { get; set; }
        public int july { get; set; }
        public int aug { get; set; }
        public int sep { get; set; }
        public int oct { get; set; }
        public int nov { get; set; }
        public int dec { get; set; }
        public int total { get; set; }
    }
    public class GetProductionManageByMonthRequest001
    {
        public string searchInput { get; set; }
        public string productClassification { get; set; }
        public int year { get; set; }
    }


    public class KpiRequest
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int ProductId { get; set; }

    }

    public class KpiProductionByHourResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int ProductionQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public int ElapseTime { get; set; }
        public int Eah { get; set; }
        public int DefectiveEah { get; set; }

    }

    public class KpiReportResponse
    {
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int LastEah { get; set; }
        public int CurrentEah { get; set; }
        public int LastDefectiveEah { get; set; }
        public int CurrentDefectiveEah { get; set; }
        public int ElapseTime { get; set; }
        public int LastElapseTime { get; set; }

    }

    public class KpiWorkListResponse
    {
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public int WorkSequence { get; set; }
        public int ElapseTime { get; set; }
        public int DownTime { get; set; }
        public int ProductionQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
    }
    #endregion
}
