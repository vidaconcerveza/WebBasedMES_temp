using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.Quality
{

    public class StoreCheckDefectiveReq001
    {
        
        public int receivingId { get; set; }
        public string receivingNo { get; set; }
        public string receivingStartDate { get; set; }
        public string receivingEndDate { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
        public int defectiveId { get; set; }
        public string productLOT { get; set; }
        public int storeOutStoreProductId { get; set; }
    }
    [Keyless]
    public class StoreCheckDefectiveRes001
    {
        public int storeOutStoreProductId { get; set; }
       
        public string registerId { get; set; }
        public string receivingNo { get; set; }
        public string receivingDate { get; set; }
        public string partnerName { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productStandardUnit { get; set; } //구매단위
        public int productReceivingCount { get; set; }//입고수량
        public int productGoodQuantity { get; set; } //양품수량
        public int productDefectiveQuantity { get; set; } //불량수량
        public string productLOT { get; set; }
        public bool productImportCheck { get; set; } //수입검사여부
        public int productImportCheckResult { get; set; } //수입검사결과
        public string registerName { get; set; }
        public string receivingProductMemo { get; set; } //비고

        public DateTime flag;
    }
    [Keyless]
    public class StoreCheckDefectiveRes002
    {
       
        public int storeOutStoreProductId { get; set; }
        public string registerId { get; set; }
        public string receivingNo { get; set; }
        public string receivingDate { get; set; }
        public string partnerCode { get; set; }
        public string partnerName { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//품목단위
        public string productStandardUnit { get; set; } //구매단위
        public int productStandardUnitCount { get; set; } //기준단위기준수량
        public int productReceivingCount { get; set; }//입고수량
        public int productGoodQuantity { get; set; } //양품수량
        public int productDefectiveQuantity { get; set; } //불량수량
        public string productLOT { get; set; }
        public string outStoreNo { get; set; }
        public bool productImportCheck { get; set; } //수입검사여부
        public int productImportCheckResult { get; set; } //수입검사결과
        public string registerName { get; set; }
        public string receivingProductMemo { get; set; } //비고
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
    }
    [Keyless]
    public class StoreCheckDefectiveRes003
    {
        public string defectiveCode { get; set; }
        public string defectiveName { get; set; }
        public int defectiveQuantity { get; set; }
        public string defectiveUnit { get; set; }
        public string defectiveProductMemo { get; set; }
    }
 }
