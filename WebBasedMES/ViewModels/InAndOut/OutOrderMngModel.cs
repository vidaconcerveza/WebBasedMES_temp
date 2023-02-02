using System;
using System.Collections.Generic;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels.BaseInfo;

namespace WebBasedMES.ViewModels.InAndOutMng.InAndOut
{
    public class OutOrderRequestCrud
    {
        public int outOrderId { get; set; }
        public int isDeleted { get; set; }
        public int[] outOrderIdArray { get; set; }
        public string shipmentDate { get; set; }
        public string shipmentNo { get; set; }
        public int partnerId { get; set; }
        public string registerId { get; set; }
        public string shipmentMemo { get; set; }
        public int uploadFileId { get; set; }

        public IEnumerable<UploadFile> UploadFiles { get; set; }

    }
    public class OutOrderProductRequestCrud
    {
        public int outOrderProductId { get; set; } //출고 마스터 아이템 id
        public int isDeleted { get; set; }
        public int[] outOrderProductIdArray { get; set; }
        public int outOrderId { get; set; } //출고 id
        public int productId { get; set; } // 품목 id
        public int orderProductId { get; set; } // 수주마스터아이템 id
        public int quantity { get; set; }
        public int productShipmentCount { get; set; }
        public int productSellPrice { get; set; }
        public int productSupplyPrice { get; set; }
        public int productTaxPrice { get; set; }
        public int ProductTotalPrice { get; set; }
        public string? shipmentProductMemo { get; set; }
        public string? productLOT { get; set; } // LOT

        public IEnumerable<OutOrderProductStockInterface> OutOrderProductStocks { get; set; }
        public IEnumerable<OutOrderProductDefectiveInterface> OutOrderProductDefectives { get; set; }
        public IEnumerable<OutOrderProductRequestCrud> outOrderProductArray { get; set; } //배열 insert을 위함.
    }

    public class OutOrderReq001
    {
        public int outOrderId { get; set; }
        public int outOrderProductId { get; set; }
        public int orderProductId { get; set; }
        public string shipmentStartDate { get; set; }
        public string shipmentEndDate { get; set; }
        public string shipmentNo { get; set; }
        public string productLOT { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
    }
    
    public class OutOrderReq002
    {
        public int outOrderProductDefectiveId { get; set; }
        public int outOrderProductId { get; set; } 
        public int[] outOrderProductIds { get; set; }
        public int defectiveId { get; set; }
        public int[] outOrderProductDefectiveIdArray { get; set; }
        public int defectiveQuantity { get; set; }
        public string lotName { get; set; }
        public IEnumerable<OutOrderReq002> defectiveArray { get; set; } //배열 insert을 위함.
    }

    public class OutOrderReq003
    {
        public int partnerId { get; set; }
        public string shipmentStatus { get; set; } //출고여부
        public string searchInput { get; set; }//검색조건:검색 text
        public string requestType { get; set; }

    }
    //출고관리 메인화면 수주목록
    public class OutOrderReq004
    {
        public int orderId { get; set; }

        public string orderStartDate { get; set; }
        public string orderEndDate { get; set; }
        public string orderNo { get; set; }
        public string orderStatus { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
    }
    public class OutOrderReq005
    {
        public int outOrderProductStockId { get; set; }
        public int outOrderProductId { get; set; } //출고 마스터 아이템 id
        public int isDeleted { get; set; }
        public int isSelected { get; set; }
        public int productId { get; set; } // 품목 id
        public int quantity { get; set; }

        public string productShipmentCheckResult { get; set; }
        public string productLOT { get; set; }
        public IEnumerable<OutOrderReq005> outOrderProductsStockArray { get; set; } //배열 insert을 위함.
        //public OutOrderReq005[] outOrderProductsStockArray { get; set; } //배열 insert을 위함.
    }


    public class OutOrderReq006
    {
        public int outOrderProductStockId { get; set; }
        public int[] outOrderProductStockIdArray { get; set; }
        public int outOrderProductId { get; set; } //출고 마스터 아이템 id
        public int isDeleted { get; set; }
        public int isSelected { get; set; }
        public int productId { get; set; } // 품목 id
        public int quantity { get; set; }
        public string ProductShipmentCheckResult { get; set; }
        public string productLOT { get; set; }
        public IEnumerable<OutOrderReq006> outOrderProductsStockArray { get; set; } //배열 insert을 위함.
        //public OutOrderReq005[] outOrderProductsStockArray { get; set; } //배열 insert을 위함.
    }

    //public class OutOrderReq005_1
    //{
    //    public int outOrderProductId { get; set; } //출고 마스터 아이템 id
    //    public int isSelected { get; set; }
    //    public string productShipmentCheckResult { get; set; }
    //    public int quantity { get; set; }
    //}
    public class OutOrderRes001
    {
        public int outOrderId { get; set; }

        public string shipmentNo { get; set; }
        public string shipmentDate { get; set; }
        public string partnerName { get; set; }
        public string partnerTaxInfo { get; set; }
        public int shipmentSellPrice { get; set; }
        public int shipmentProductCount { get; set; }
        public int shipmentSupplyPrice { get; set; }
        public int shipmentTaxPrice { get; set; }
        public int shipmentTotalPrice { get; set; }
        public string registerName { get; set; }
        public string shipmentMemo { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
    }

    public class OutOrderRes002
    {
        public int outOrderProductId { get; set; }
        public int outOrderId { get; set; }
        public int orderProductId { get; set; }
        public int productId { get; set; }
        public string orderNo { get; set; } //수주 번호
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//규격
        public string productUnit { get; set; }//단위
        public string productStandardUnit { get; set; } //기준단위
        public string productTaxInfo { get; set; } //과세유형 
        public int productOrderCount { get; set; }//수주수량
        public int productShipmentCount { get; set; }//출고수량
        public int productSellPrice { get; set; }//판매단가
        public int productSupplyPrice { get; set; }//공급가액
        public int productTaxPrice { get; set; } //세액
        public int productTotalPrice { get; set; } // 총금액
        public string shipmentProductMemo { get; set; } //비고

        public string ModelName { get; set; }
        public string PartnerName { get; set; }

    }

    public class OutOrderRes003
    {
        public int outOrderId { get; set; }
        public string shipmentNo { get; set; }
        public string shipmentDate { get; set; }
        public int partnerId { get; set; }
        public string partnerCode { get; set; }
        public string partnerName { get; set; }
        public string partnerTaxInfo { get; set; }
        public string contactName { get; set; }
        public string telephoneNumber { get; set; }
        public string faxNumber { get; set; }
        public string contactEmail { get; set; }
        public string registerId { get; set; }
        public string registerName { get; set; }
        public string shipmentMemo { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }

        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }
    public class OutOrderRes004
    {
        public int outOrderProductId { get; set; }
        public int outOrderId { get; set; }
        public int orderProductId { get; set; }
        public int productId { get; set; }
        public string orderNo { get; set; } //수주 번호
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//규격
        public string productUnit { get; set; }//단위
        public string productTaxInfo { get; set; } //과세유형 
        public int productOrderCount { get; set; }//수주수량
        public int productOrderCountRemain { get; set; }//수주잔량
        public int quantity { get; set; }//수량
        public string productStandardUnit { get; set; } //기준단위
        public int productStandardUnitCount { get; set; } //기준단위수량
        public int productShipmentCount { get; set; }//출고수량
        public int productSellPrice { get; set; }//판매단가
        public int productSupplyPrice { get; set; }//공급가액
        public int productTaxPrice { get; set; } //세액
        public int productTotalPrice { get; set; } // 총금액
        public string shipmentProductMemo { get; set; } //비고
        public IEnumerable<OutOrderProductStockInterface> outOrderProductStocks { get; set; }
        public IEnumerable<OutOrderProductDefectiveInterface> outOrderProductDefectives { get; set; }
    }
    public class OutOrderRes005
    {
        public int outOrderProductDefectiveId { get; set; }
        public int defectiveId { get; set; }
        public string productLOT { get; set; }
        public string defectiveCode { get; set; }
        public string defectiveName { get; set; }
        public int defectiveQuantity { get; set; }
        public string defectiveProductMemo { get; set; }
    }

    public class OutOrderRes006
    {
       
       
        public int orderProductId { get; set; }
        public string orderNo { get; set; } //수주 번호
        public string orderDate { get; set; } //등록일
        public string requestDeliveryDate { get; set; }//납품요청일
        public string partnerName { get; set; }//거래처
        public string partnerTaxInfo { get; set; } //과세정보
        public string shipmentStatus { get; set; } //출고여부
        public int productId { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//규격
        public string productUnit { get; set; }//단위
        public string productStandardUnit { get; set; } //기준단위
        public int productOrderCount { get; set; } //수주수량
        public int productOrderCountRemain { get; set; }//수주잔량
        public int productSellPrice { get; set; }//판매단가
        public bool productShipmentCheck { get; set; }
        public int Inventory { get; set; }
        public int OptimumStock { get; set; }
        public IEnumerable<ProductProcessInterface2> producePlanProcesses { get; set; }
        public IEnumerable<OutOrderProductStockInterface> outOrderProductStocks { get; set; }
        public IEnumerable<OutOrderProductDefectiveInterface> outOrderProductDefectives { get; set; }


    }

    public class OutOrderRes007
    {
        public int orderId { get; set; }
        public string orderNo { get; set; }
        public string orderDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string partnerName { get; set; }
        public string partnerTaxInfo { get; set; }
        public int orderProductCount { get; set; }
        public int shipmentCompletedCount { get; set; } //출고완료 품목수    
        public string orderStatus { get; set; } //수주상태
        public string orderMemo { get; set; }
    }
    public class OutOrderRes008
    {
        public int orderId { get; set; }
        public string orderNo { get; set; }
        public string orderDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string orderStatus { get; set; }
        public string registerId { get; set; }
        public string registerName { get; set; }
        public int partnerId { get; set; }
        public string partnerCode { get; set; }
        public string partnerName { get; set; }
        public string contactName { get; set; }
        public string telephoneNumber { get; set; }
        public string faxNumber { get; set; }
        public string contactEmail { get; set; }
        public string partnerTaxInfo { get; set; }
        public string orderMemo { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
        public string shipmentManageMemo {get; set;}

        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }
    public class OutOrderRes009
    {
        public int outOrderId { get; set; }

        public string shipmentNo { get; set; }
        public string shipmentDate { get; set; }
        public string partnerName { get; set; }
        public string partnerTaxInfo { get; set; }
        public int shipmentProductCount { get; set; }
        public int shipmentSupplyPrice { get; set; }
        public int shipmentTaxPrice { get; set; }
        public int shipmentTotalPrice { get; set; }
        public string registerName { get; set; }
        public string shipmentMemo { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
    }
    public class OutOrderRes010
    {
        public int outOrderId { get; set; }
        public int outOrderProductId { get; set; }

        public string shipmentNo { get; set; }
        public string shipmentDate { get; set; }
        public string partnerName { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//규격
        public string productUnit { get; set; }//단위
        public string productTaxInfo { get; set; }//단위
        public string productStandardUnit { get; set; } //기준단위
        public int productStandardUnitCount { get; set; } //기준단위수량
        public int productShipmentProductCount { get; set; } //출고수량
        public int productSellPrice { get; set; }//판매단가
        public int productSupplyPrice { get; set; } //공급가액
        public int productTaxPrice { get; set; }
        public int productTotalPrice { get; set; }
        public string productLOT { get; set; } //기준단위수량
    }

    public class OutOrderRes011
    {
        public int lotId { get; set; }
        public int inventory { get; set; }
        public int lotCountId { get; set; }
        public int outOrderProductId { get; set; }
        public int productId { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//규격
        public string productUnit { get; set; }//단위
        public int productStandardUnitCount { get; set; } //기준단위수량
        public string productLOT { get; set; } //기준단위수량
        public Boolean productShipmentCheck { get; set; }
        public Boolean productShipmentCheckResult { get; set; }
        public int quantity { get; set; }
        public int isSelected { get; set; }
    }


    public class ProductDetailRequest
    {
       
        public int[] ProductIds { get; set; }
    }

    public class ProductDetailResponse
    {
        public int ProductId { get; set; }
        public IEnumerable<OutOrderProductStockInterface> Stocks { get; set; }
    }
}

