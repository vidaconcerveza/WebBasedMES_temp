using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.ProduceStatus
{

    public class ProducePlanProduceStatusReq001
    {
        public int producePlanProduceStatusId { get; set; }
        public int producePlanId { get; set; }
        public int productId { get; set; }
        public string currentDate { get; set; }   //현재 날짜
        
    }
    [Keyless]
    public class ProducePlanProduceStatusRes001
    {
        public int? producePlanProduceStatusId { get; set; }
        public int? producePlanId { get; set; }
        public int? productId { get; set; }
        public string? currentDate { get; set; }   //현재 날짜
        public string? productionPlanStartDate { get; set; }   //계획시작일
        public string? productionPlanEndDate { get; set; }   //계획종료일
        public string? productionPlanNo { get; set; }   //계획번호
        public int? totalProductionRate { get; set; }//총 생산률
        public string? productName { get; set; }
        public int? productionRate { get; set; } // 생산률
        public int? productionPlanProductCount { get; set; } //생산계획 품목수
        public int? completedCount { get; set; } //완료수
       
    }
    
}
