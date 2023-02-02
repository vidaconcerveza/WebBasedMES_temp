using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.Quality
{

    public class ProductDefectiveReq001
    {
        public int productDefectiveId { get; set; }
        public int productId { get; set; }
        public int defectiveId { get; set; }
        public string defectiveStartDate { get; set; }
        public string defectiveEndDate { get; set; }
        public string productLOT { get; set; }
        public string type { get; set; }

    }
    [Keyless]
    public class ProductDefectiveRes001
    {
        //public int productDefectiveId { get; set; }
        public int? productId { get; set; }
        public int? defectiveId { get; set; }
        //public string registerId { get; set; }
        public string? productCode { get; set; } //품목 코드
        public string? productClassification { get; set; }//품목구분
        public string? productName { get; set; }//품목이름
        public string? productStandard { get; set; }//품목규격
        public string? productUnit { get; set; }//단위
        public string? productLOT { get; set; }
        public string? type { get; set; }//발생유형
        public string? defectiveCode { get; set; } //불량 코드
        public string? defectiveName { get; set; } //불량 유형
        public int? productDefectiveQuantity { get; set; } //불량수량
        public string? productStandardUnit { get; set; }//기준단위
        public string? defectiveProductMemo { get; set; }
        public string? registerName { get; set; }
        public string? defectiveDate { get; set; }

    }
    
 }
