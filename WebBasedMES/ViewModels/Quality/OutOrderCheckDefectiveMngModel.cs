using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.Quality
{

    public class OutOrderCheckDefectiveReq001
    {

        public int outOrderId { get; set; }
        public int outOrderProductId { get; set; }
        public int orderProductId { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
        public int defectiveId { get; set; }
        public string shipmentNo { get; set; }
        public string shipmentStartDate { get; set; }
        public string shipmentEndDate { get; set; }
        public string productLOT { get; set; }
        public int productShipmentCheckResult { get; set; }

    }
    [Keyless]
    public class OutOrderCheckDefectiveRes001
    {
        //public int outOrderCheckDefectiveId { get; set; }
        public string orderNo { get; set; }

        public int outOrderProductId { get; set; }
        //public int productId { get; set; }
        //public int partnerId { get; set; }
        //public string registerId { get; set; }
       

        public string shipmentNo { get; set; }
        public string shipmentDate { get; set; }
        public string partnerName { get; set; }
        public string partnerCode { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//단위
        public int quantity { get; set; } //수량
        public int productDefectiveQuantity { get; set; } //불량수량
        public string productLOT { get; set; }
        public bool productShipmentCheck { get; set; }
        public int productShipmentCheckResult { get; set; }
        public string registerName { get; set; }
        public string shipmentProductMemo { get; set; }

        public DateTime dateFlag { get; set; }

    }
    [Keyless]
    public class OutOrderCheckDefectiveRes002
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
        public int productDefectiveQuantity { get; set; } //불량수량
        public string productLOT { get; set; }
        public bool productShipmentCheck { get; set; }
        public int productShipmentCheckResult { get; set; }
        public string registerName { get; set; }
        public string shipmentProductMemo { get; set; }
        //public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
        public string orderNo { get; set; }

    }

    [Keyless]
    public class OutOrderCheckDefectiveRes003
    {
        public int outOrderProductDefectiveId { get; set; }
        public int outOrderProductId { get; set; }
        public string defectiveCode { get; set; }
        public string defectiveName { get; set; }
        public int defectiveQuantity { get; set; }
        public string defectiveProductMemo { get; set; }
    }
}
