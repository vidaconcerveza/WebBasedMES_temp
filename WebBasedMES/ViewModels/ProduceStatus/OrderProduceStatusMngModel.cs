using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.ProduceStatus
{

    public class OrderProduceStatusReq001
    {
        public int orderProduceStatusId { get; set; }
        public int orderId { get; set; }
        public int productId { get; set; }
        public int partnerId { get; set; }
        public string currentDate { get; set; }   //현재 날짜
        
    }
    [Keyless]
    public class OrderProduceStatusRes001
    {
        public int? orderProduceStatusId { get; set; }
        public int? orderId { get; set; }
        public int? productId { get; set; }
        public string? currentDate { get; set; }   //현재 날짜
        public string? orderDate { get; set; }   //수주일
        public string? deliveryDate { get; set; }   //납품일
        public string? orderNo { get; set; }   //수주번호
        public string? partnerCode { get; set; }
        public string? partnerName { get; set; }
        public int? totalProductionRate { get; set; }//총 생산률
        public string? productName { get; set; }
        public int? productionRate { get; set; } // 생산률
        public int? orderProductCount { get; set; } //수주품목수
        public int? completedCount { get; set; } //완료수
       
    }
    
}
