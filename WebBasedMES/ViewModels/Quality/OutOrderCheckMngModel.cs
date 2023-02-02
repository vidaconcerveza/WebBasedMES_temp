using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.Quality
{

    public class OutOrderCheckReq001
    {

        public int outOrderId { get; set; }
        public int outOrderProductId { get; set; }
        public int orderProductId { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
        public string shipmentNo { get; set; }
        public string shipmentStartDate { get; set; }
        public string shipmentEndDate { get; set; }
        public string productLOT { get; set; }
        public int productShipmentCheckResult { get; set; }

    }
    [Keyless]
    public class OutOrderCheckRes001
    {
        public int outOrderProductId { get; set; }
        //public int productId { get; set; }
        //public int partnerId { get; set; }
        public string shipmentNo { get; set; }
        public string shipmentDate { get; set; }
        public string partnerCode { get; set; }
        public string partnerName { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//단위
        public int quantity { get; set; } //수량
        public string productLOT { get; set; }
        public int productShipmentCheckResult { get; set; }
        public string registerName { get; set; }
        public string shipmentProductMemo { get; set; }

        public DateTime dateFlag { get; set; }

    }
    
 }
