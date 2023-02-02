using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.Quality
{

    public class ProcessCheckReq001
    {
        public int workOrderId { get; set; }            //작업지시Id
        public int processId { get; set; }
        public int facilityId { get; set; }
        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderStartDate { get; set; }   //작업지시일
        public string workOrderEndDate { get; set; }     //작업지시일
        public string productLOT { get; set; }
        public int processCheckResult { get; set; } //공정검사결과
        public string WorkerName { get; set; }

       
    }
    [Keyless]
    public class ProcessCheckRes001
    {
        public int workerOrderProducePlanId { get; set; }
        public int processId { get; set; }
        public int workOrderId { get; set; }
        public int facilityId { get; set; }
        public int moldId { get; set; }
        //public string? registerId { get; set; }
        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderDate { get; set; }//작업지시일
        public string? processCode { get; set; }//공정코드
        public string? processName { get; set; }//공정이름
        public int isOutSourcing { get; set; }         //자체/외주
        public string partnerName { get; set; }         //거래처
        public string facilitiesCode { get; set; }      //설비코드
        public string facilitiesName { get; set; }      //설비이름
        public string moldCode { get; set; }            //금형코드
        public string moldName { get; set; }            //금형이름
        public string workerName { get; set; }           //작업자
        public int productionQuantity { get; set; }    //생산수량
        public int processCheckResult { get; set; } //공정검사결과
        public string productLOT { get; set; }
        public string workProcessMemo { get; set; }
        public DateTime flag { get; set; }
    }
 }
