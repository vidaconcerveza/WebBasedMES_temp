using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.Quality
{

    public class ProcessCheckDefectiveReq001
    {

        public int workerOrderProducePlanId { get; set; }            //작업지시Id
        public int processId { get; set; }
        public string registerId { get; set; }
        public int defectiveId { get; set; }
        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderStartDate { get; set; }   //작업지시일
        public string workOrderEndDate { get; set; }     //작업지시일
        public string productLOT { get; set; }
        public string workerName { get; set; }
        public int processCheckResult { get; set; }
       

    }
    [Keyless]
    public class ProcessCheckDefectiveRes001
    {
        public int workerOrderProducePlanId { get; set; }
        //public int processId { get; set; }
        //public int moldId { get; set; }
        //public string registerId { get; set; }
        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderDate { get; set; }//작업지시일
        public string processCode { get; set; }//공정코드
        public string processName { get; set; }//공정이름
        public int isOutSourcing { get; set; }         //자체/외주
        public string partnerName { get; set; }         //거래처
        public string facilitiesCode { get; set; }      //설비코드
        public string facilitiesName { get; set; }      //설비이름
        public string moldCode { get; set; }            //금형코드
        public string moldName { get; set; }            //금형이름
        public string workerName { get; set; }           //작업자
        public int productionQuantity { get; set; }    //생산수량
        public int productGoodQuantity { get; set; } //양품수량
        public int productDefectiveQuantity { get; set; } //불량수량
        public bool processCheck { get; set; }
        public int processCheckResult { get; set; } //공정검사결과
        public string productLOT { get; set; }
        public string workProcessMemo { get; set; }
        public DateTime dateFlag { get; set; }
    }
    [Keyless]
    public class ProcessCheckDefectiveRes002
    {
        public int workerOrderProducePlanId { get; set; }
        public int processId { get; set; }
        public int moldId { get; set; }
        public int productId { get; set; }
        public int facilityId { get; set; }
        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderDate { get; set; }//작업지시일
        public string workStartDateTime { get; set; }   
        public string workEndDateTime { get; set; }     
        public string processCode { get; set; }//공정코드
        public string processName { get; set; }//공정이름
        public int isOutSourcing { get; set; }         //자체/외주
        public string partnerName { get; set; }         //거래처
        public string facilitiesCode { get; set; }      //설비코드
        public string facilitiesName { get; set; }      //설비이름
        public string moldCode { get; set; }            //금형코드
        public string moldName { get; set; }            //금형이름
        public string workerName { get; set; }           //작업자
        public bool processCheck { get; set; }
        public int processCheckResult { get; set; } //공정검사결과
        public string productLOT { get; set; }
        public string workProcessMemo { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//품목단위
        public int productionQuantity { get; set; }    //생산수량
        public int productGoodQuantity { get; set; } //양품수량
        public int productDefectiveQuantity { get; set; } //불량수량
    }
    [Keyless]
    public class ProcessCheckDefectiveRes003
    {
        public int processDefectiveId { get; set; }
        public int workerOrderProducePlanId { get; set; }
        public int defectiveId { get; set; }
        public string defectiveCode { get; set; }
        public string defectiveName { get; set; }
        public int defectiveQuantity { get; set; }
        public string defectiveProductMemo { get; set; }
    }
 }
