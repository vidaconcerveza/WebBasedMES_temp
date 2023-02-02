using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.InAndOutMng.InAndOut
{


    public class StoreRequestCrud
    {
        public int outStoreId { get; set; }
        public int receivingId { get; set; } 
        public int isDeleted { get; set; }
        public int[] receivingIdArray { get; set; }
        public string receivingDate { get; set; }
        public string receivingNo { get; set; }
        public int partnerId { get; set; }
        public string registerId { get; set; }
        public string receivingMemo { get; set; }
        public int uploadFileId { get; set; } 

        public IEnumerable<UploadFile> UploadFiles { get; set; }

    }

    public class StoreOutStoreProductRequestCrud
    {
        public StoreRequestCrud store { get; set; }
        public int storeOutStoreProductId { get; set; } //입고 마스터 아이템 id
        public int isDeleted { get; set; }
        public int[] storeOutStoreProductIdArray { get; set; }
        public int receivingId { get; set; } //입고 id
        public int outStoreId { get; set; } // 발주 id
        public int outStoreProductId { get; set; } // 발주마스터아이템 id
       
        public string outStoreNo { get; set; } //발주 번호
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//품목단위
        public string productStandardUnit { get; set; } //구매단위
        public int productStandardUnitCount { get; set; }//구매단위 기준수량 
        public string productTaxInfo { get; set; } //과세유형 
        public int productOutStoreCount { get; set; } //발주수량
        public int productReceivingCount { get; set; }//입고수량
        public int productBuyPrice { get; set; }//구매단가
        public int productSupplyPrice { get; set; }//공급가액
        public int productTaxPrice { get; set; } //세액
        public int productTotalPrice { get; set; } // 총금액
        public string productLOT { get; set; } // LOT
        public int productGoodQuantity { get; set; } //양품수량
        public int productDefectiveQuantity { get; set; } //불량수량
        public bool productImportCheck { get; set; } //수입검사여부
        public string productImportCheckResult { get; set; } //수입검사결과
        public string receivingProductMemo { get; set; } //비고

        public IEnumerable<StoreOutStoreProductInspectionInterface> storeOutStoreProductInspections { get; set; }
        public IEnumerable<StoreOutStoreProductDefectiveInterface> storeOutStoreProductDefectives { get; set; }
        public IEnumerable<StoreOutStoreProductRequestCrud> storeOutStoreProductArray { get; set; } //배열 insert을 위함.

    }

    public class StoreOutStoreProductInspectionInterface
    {
        public int storeOutStoreProductInspectionId { get; set; }
        public int storeOutStoreProductId { get; set; }
        public int InspectionTypeId { get; set; }
        public string inspectionCode { get; set; }
        public int inspectionId { get; set; }
        public string inspectionStandard { get; set; }
        public string inspectionMethod { get; set; }
        public string inspectionResult { get; set; }
        public string inspectionResultText { get; set; }
        public string inspectionName { get; set; }
    }

    public class StoreOutStoreProductDefectiveInterface
    {
        public int storeOutStoreProductDefectiveId { get; set; }
        public int storeOutStoreProductId { get; set; }
        public int defectiveId { get; set; }
        public int defectiveQuantity { get; set; }
        public string defectiveUnit { get; set; }
        public string defectiveProductMemo { get; set; }
        public string defectiveMemo { get; set; }
        public string defectiveName { get; set; }
        public string defectiveCode { get; set; }
        public bool Checked { get; set; }

    }


    public class StoreReq001
    {
        public int receivingId { get; set; }
        public string receivingStartDate { get; set; }
        public string receivingEndDate { get; set; }
        public string receivingNo { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
        public string LOT { get; set; }
        public string invenType { get; set; }
        //public int LOT { get; set; }
    }
    public class StoreReq002
    {
        public string defectiveIsUsing { get; set; }
        public string inspectionIsUsing { get; set; }
        public string searchInput { get; set; }//검색조건:검색 text
    }
    public class StoreReq003
    {
        public int storeoutStoreProductId { get; set; } //입고 마스터 아이템 id
        public int defectiveId { get; set; }
        public int[] storeOutStoreProductDefectiveIdArray { get; set; }
        public int defectiveQuantity { get; set; }
        public string defectiveUnit { get; set; }
        public IEnumerable<StoreReq003> defectiveArray { get; set; } //배열 insert을 위함.
    }
    public class StoreReq004
    {
        public int storeoutStoreProductId { get; set; } //입고 마스터 아이템 id
        public int inspectionTypeId { get; set; }
        public int[] storeOutStoreProductInspectionIdArray { get; set; }
        public int inspectionResult { get; set; }
        public string inspectionResultText { get; set; }
        public IEnumerable<StoreReq004> inspectionArray { get; set; } //배열 insert을 위함.
    }

    public class StoreReq005
    {
        public int partnerId { get; set; }
        public string taskStatus { get; set; } //진행상태
        public string searchInput { get; set; }//검색조건:검색 text
        public string invenType { get; set; }
    }

    public class StoreRes001
    {
        public int receivingId { get; set; }

        public string receivingNo { get; set; }
        public string receivingDate { get; set; }
        public string partnerName { get; set; }
        public string partnerTaxInfo { get; set; }
        public int receivingProductCount { get; set; }
        public int receivingSupplyPrice { get; set; }
        public int receivingTaxPrice { get; set; }
        public int receivingTotalPrice { get; set; }
        public string registerName { get; set; }
        public string receivingMemo { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
        public int[] productIds { get; set; }

        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }

    public class StoreRes002
    {
        public int storeOutStoreProductId { get; set; }
        public int receivingId { get; set; }
        public int outStoreProductId { get; set; }
        public int productId { get; set; }
        public string outStoreNo { get; set; } //발주 번호
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productStandardUnit { get; set; } //구매단위
        public string productTaxInfo { get; set; } //과세유형 
        public int productReceivingCount { get; set; }//입고수량
        public int productBuyPrice { get; set; }//구매단가
        public int productSupplyPrice { get; set; }//공급가액
        public int productTaxPrice { get; set; } //세액
        public int productTotalPrice { get; set; } // 총금액
        public string productLOT { get; set; } // LOT
        public int productGoodQuantity { get; set; } //양품수량
        public int productDefectiveQuantity { get; set; } //불량수량
        public int productImportCheck { get; set; } //수입검사여부
        public string productImportCheckResult { get; set; } //수입검사결과
        public string receivingProductMemo { get; set; } //비고
        public int productStandardUnitCount { get; set; }

        public string ModelName { get; set; }
        public string PartnerName { get; set; }

    }

    public class StoreRes003
    {
        public int receivingId { get; set; }
        public string receivingDate { get; set; }
        public int partnerId { get; set; }
        public string partnerCode { get; set; }
        public string partnerName { get; set; }
        public string contactName { get; set; }
        public string telephoneNumber { get; set; }
        public string faxNumber { get; set; }
        public string contactEmail { get; set; }
        public string partnerTaxInfo { get; set; }
        public string registerId { get; set; }
        public string registerName { get; set; }
        public string receivingMemo { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }

    public class StoreRes004
    {
        public int storeOutStoreProductId { get; set; }
        public int receivingId { get; set; }
        public int outStoreProductId { get; set; }
        public int productId { get; set; }

        public string outStoreNo { get; set; } //발주 번호
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//품목단위
        public string productStandardUnit { get; set; } //구매단위
        public int productStandardUnitCount { get; set; }//구매단위 기준수량 
        public string productTaxInfo { get; set; } //과세유형 
        public int productOutStoreCount { get; set; } //발주수량
        public int productReceivingCount { get; set; }//입고수량
        public int productBuyPrice { get; set; }//구매단가
        public int productSupplyPrice { get; set; }//공급가액
        public int productTaxPrice { get; set; } //세액
        public int productTotalPrice { get; set; } // 총금액
        public string productLOT { get; set; } // LOT
        public int productGoodQuantity { get; set; } //양품수량
        public int productDefectiveQuantity { get; set; } //불량수량
        public bool productImportCheck { get; set; } //수입검사여부
        public string productImportCheckResult { get; set; } //수입검사결과
        public string receivingProductMemo { get; set; } //비고

        public IEnumerable<StoreOutStoreProductInspectionInterface> storeOutStoreProductInspections { get; set; }
        public IEnumerable<StoreOutStoreProductDefectiveInterface> storeOutStoreProductDefectives { get; set; }

    }
    public class StoreRes005
    {
        public int defectiveId { get; set; }
        public string defectiveCode { get; set; } 
        public string defectiveMemo { get; set; }
        public string defectiveName { get; set; }
        public bool defectiveIsUsing { get; set; }

    }
    public class StoreRes006
    {
        
        public int storeOutStoreProductInspectionTypeId { get; set; }
        public int inspectionTypeId { get; set; }
        public string inspectionCode { get; set; }
        public string inspectionName { get; set; }
        public string inspectionStandard { get; set; }
        public string inspectionMethod { get; set; }
        public int inspectionResult { get; set; }
        public string inspectionResultText { get; set; }
    }
    public class StoreRes007
    {
        public int storeOutStoreProductDefectiveId { get; set; }
        public int defectiveId { get; set; }
        public string defectiveCode { get; set; }
        public string defectiveName { get; set; }
        public int defectiveQuantity { get; set; }
        public string defectiveUnit { get; set; }
        public string defectiveMemo { get; set; }
    }
    public class StoreRes008
    {
        public int partnerId { get; set; }
        public int outStoreId { get; set; }
        public string outStoreNo { get; set; }
        public string outStoreDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string partnerName { get; set; }
        public string partnerTaxInfo { get; set; }
        public int outStoreProductCount { get; set; }
        public int receivingCompletedCount { get; set; }
        public string outStoreStatus { get; set; }
    }
    public class StoreRes008_1
    {
        public int OutStoreProductId { get; set; }
        public int productReceivingCount { get; set; }
        public int ProductOutStoreCount { get; set; }
        public int receivingCompletedCount { get; set; }
    }
    public class StoreRes008_2
    {
        public int outStoreId { get; set; }
        public int receivingCompletedCount { get; set; }
    }

    public class StoreRes009
    {
        public int outStoreId { get; set; }
        public string outStoreNo { get; set; }
        public string outStoreDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string outStoreStatus { get; set; }
        public string registerId { get; set; }
        public string registerName { get; set; }

        public int partnerId { get; set; }
        public string partnerName { get; set; }
        public string contactName { get; set; }
        public string contactEmail { get; set; }
        public string partnerCode { get; set; }
        public string telephoneNumber { get; set; }
        public string outStoreMemo { get; set; }

        public string partnerTaxInfo { get; set; }
        public string faxNumber { get; set; }
        public string outStoreFinish { get; set; }

        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }

    public class StoreRes010
    {
        public string taskStatus { get; set; }
        public int outStoreProductId { get; set; }
        public int outStoreId { get; set; }
        public int productId { get; set; }

        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
        public string productStandardUnit { get; set; }
        public int productStandardUnitCount { get; set; }
        public int productInCount { get; set; }
        public string productTaxInfo { get; set; }
        public int productOutStoreCount { get; set; }
        public int productBuyPrice { get; set; }

        public int productSupplyPrice { get; set; }
        public double productTaxPrice { get; set; }
        public double productTotalPrice { get; set; }
        public string outStoreProductMemo { get; set; }
        
        public Boolean productImportCheck { get; set; }
        //public int uploadFileId { get; set; }
        //public string uploadFileName { get; set; }
        //public string uploadFileUrl { get; set; }
    }
    public class StoreRes011
    {
        public int receivingId { get; set; }

        public string receivingNo { get; set; }
        public string receivingDate { get; set; }
        public string partnerName { get; set; }
        public string partnerTaxInfo { get; set; }
        public int receivingProductCount { get; set; }
        public int receivingSupplyPrice { get; set; }
        public int receivingTaxPrice { get; set; }
        public int receivingTotalPrice { get; set; }
        public string registerName { get; set; }
        public string receivingMemo { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
        public string tempLot { get; set; }
        public int[] productIds { get; set; }
    }
    public class StoreRes012
    {
        public int storeOutStoreProductId { get; set; }
        public int receivingId { get; set; }
        public int outStoreProductId { get; set; }
        public int productId { get; set; }
        public string outStoreNo { get; set; } //발주 번호
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productUnit { get; set; }
        public string productStandard { get; set; }//품목규격
        public string productStandardUnit { get; set; } //구매단위
        public string productTaxInfo { get; set; } //과세유형 
        public int productReceivingCount { get; set; }//입고수량
        public int productBuyPrice { get; set; }//구매단가
        public int productSupplyPrice { get; set; }//공급가액
        public int productTaxPrice { get; set; } //세액
        public int productTotalPrice { get; set; } // 총금액
        public string productLOT { get; set; } // LOT
        //public int productGoodQuantity { get; set; } //양품수량
        //public int productDefectiveQuantity { get; set; } //불량수량
        //public int productImportCheck { get; set; } //수입검사여부
        //public int productImportCheckResult { get; set; } //수입검사결과
        public string receivingProductMemo { get; set; } //비고

    }
    [Keyless]
    public class StoreRes013
    {
        public int? outStoreProductId { get; set; }
        public int? outStoreId { get; set; }
        

        public string? outStoreNo { get; set; }
        public string? taskStatus { get; set; }
        public string? outStoreDate { get; set; }
        public string? requestDeliveryDate { get; set; }

        public string? partnerName { get; set; }
        public int partnerId { get; set; }


        public int? productId { get; set; }
        public string? productCode { get; set; }
        public string? productClassification { get; set; }
        public string? productName { get; set; }
        public string? productStandard { get; set; }
        public string? productUnit { get; set; }
        public string? productStandardUnit { get; set; }
        public string productTaxInfo { get; set; }
        public int? productOutStoreCount { get; set; }
        public int? productReceivingCount { get; set; }
        public string? outStoreProductMemo { get; set; }
        public bool productImportCheck { get; set; }
        public int productBuyPrice { get; set; }
        public int productStandardUnitCount { get; set; }
        public IEnumerable<StoreOutStoreProductDefectiveInterface> storeOutStoreProductDefectives { get; set; }
        public IEnumerable<StoreOutStoreProductInspectionInterface> storeOutStoreProductInspections { get; set; }
    }
    public class StoreRes014
    {
        public int storeOutStoreProductId { get; set; }
        public int receivingId { get; set; }
        public int outStoreProductId { get; set; }
        public int productId { get; set; }
        public string receivingNo { get; set; } //입고 번호
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productStandardUnit { get; set; } //구매단위
        public string productTaxInfo { get; set; } //과세유형 
        public int productReceivingCount { get; set; }//입고수량
        public int productBuyPrice { get; set; }//구매단가
        public int productSupplyPrice { get; set; }//공급가액
        public int productTaxPrice { get; set; } //세액
        public int productTotalPrice { get; set; } // 총금액
        public string productLOT { get; set; } // LOT
        //public int productGoodQuantity { get; set; } //양품수량
        //public int productDefectiveQuantity { get; set; } //불량수량
        //public int productImportCheck { get; set; } //수입검사여부
        //public int productImportCheckResult { get; set; } //수입검사결과
        public string receivingProductMemo { get; set; } //비고

    }
}
