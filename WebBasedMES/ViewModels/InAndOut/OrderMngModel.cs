using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels.BaseInfo;

namespace WebBasedMES.ViewModels.InAndOutMng.InAndOut
{


    public class OrderRequestCrud
    {
        public int orderId { get; set; }
        public int isDeprecated { get; set; }
        public int[] orderIdArray { get; set; }

        public string registerId { get; set; }
        public string orderEndDate { get; set; }
        public string orderNo { get; set; }
        public int orderStatusId { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
        public int uploadFileId { get; set; }
        public string orderDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string orderMemo { get; set; }
        public int isDeleted { get; set; }

        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }

    public class OrderProductRequestCrud
    {

        public int orderProductId { get; set; } //수주 id
        public int[] orderProductIdArray { get; set; } //수주 id
        public int orderId { get; set; } //수주 id
        public string SearchInput { get; set; }
        public string IsUsingStr { get; set; } //ALL,Y,N
        public int Id { get; set; }
        public int productId { get; set; } //품목-제품으로 취급
        //public int productStandardUnitId { get; set; } //기준단위 
        public string productStandardUnit { get; set; } //기준단위 
        public int productSupplyPrice { get; set; }//단위수량 
        public int productStandardUnitCount { get; set; }//단위수량 
        public string productTaxInfo { get; set; } //과세유형 
        
        public int productOrderCount { get; set; }//수주수량
        public int productSellPrice { get; set; }//판매단가
        public string orderProductMemo { get; set; } //비고

        public string productCode { get; set; }//품목코드
        public string productGubun { get; set; }//품목구분
        public string productNm { get; set; }//품목이름
        public int productTotalPrice { get; set; } // 총금액
        public int productTaxPrice { get; set; }

        public int[] Ids { get; set; } //다건 삭제 처리 위한.

        public OrderRequestCrud order { get; set; }
        public IEnumerable<OrderProductRequestCrud> orderProductArray { get; set; } //배열 insert을 위함.
        //public IEnumerable<OrderProductRequestCrud> OrderProducts { get; set; } //배열 insert을 위함.
        public int Partner { get; set; }
        public int isDeleted { get; set; }
    }

    public class OrderPopupRequest
    {
        public string SearchInput { get; set; }
        public string ShipmentStatus { get; set; }
    }

    public class OrderPopupResponse
    {
        public int OrderId { get; set; }
        public int OrderProductId { get; set; }
        public string OrderNo { get; set; }
        public string ShipmentStatus { get; set; }
        public string OrderDate { get; set; }
        public string RequestDeliveryDate { get; set; }

        public int ProductId { get; set; }
        public string PartnerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public string ProductStandardUnit { get; set; }
        public int ProductStandardUnitCount { get; set; }
        public int ProductOrderCount { get; set; }
        public int? ProductOrderCountRemain { get; set; }
        public string OrderProductMemo { get; set; }
        public int OptimumStock { get; set; }
        public int Inventory { get; set; }

        public IEnumerable<ProductProcessInterface2> ProductProcesses { get;set; }
    }


    public class OrderReq001
    {
        public int orderId { get; set; }

        public string orderStartDate { get; set; }
        public string orderEndDate { get; set; }
        public string orderNo { get; set; }
        public string orderStatus { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
    }

    public class OrderReq002
    {
        public string productIsUsingStr { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productClassification { get; set; }
        public string invenType { get; set; }
    }

    public class OrderReq003
    {
        public string searchInput { get; set; }
        public string userIsApprovedStr { get; set; }   //ALL,Y,N

    }
    public class OrderReq004
    {
        public string searchInput { get; set; }
        public string partnerStatusStr { get; set; }   //ALL,Y,N

    }
    public class OrderRes001
    {
        public int orderId { get; set; }

        public string orderNo { get; set; }
        public string orderDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string partnerName { get; set; }
        public string partnerTaxInfo { get; set; }
        public int orderProductCount { get; set; }
        public int orderSellPrice { get; set; }

        public double orderTaxPrice { get; set; }
        public double orderTotalPrice { get; set; }
        public string orderStatus { get; set; }

        public string registerName { get; set; }
        public string orderMemo { get; set; }

        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
        public Boolean orderIsUsing { get; set; }

    }

    public class OrderRes002
    {
        public int orderId { get; set; }
        public string orderNo { get; set; }
        public string orderDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string orderStatus { get; set; }
        public string registerId { get; set; }
        public string registerName { get; set; }

        public int partnerId { get; set; }
        public string partnerName { get; set; }
        public string contactName { get; set; }
        public string contactEmail { get; set; }
        public string partnerCode { get; set; }
        public string telephoneNumber { get; set; }
        public string orderMemo { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
        public string partnerTaxInfo { get; set; }
        public string faxNumber { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public Boolean orderIsUsing { get; set; }

    }

    public class OrderRes003
    {
        public int orderProductId { get; set; }
        public int orderId { get; set; }
        public int productId { get; set; }

        public string shipmentStatus { get; set; }
        public string productCode { get; set; }

        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
        public string productStandardUnit { get; set; }

        public string productTaxInfo { get; set; }
        public int productOrderCount { get; set; }
        public int productOrderRemain { get; set; }
        public int productSellPrice { get; set; }
        public int productSupplyPrice { get; set; }
        public double productTaxPrice { get; set; }
        public double productTotalPrice { get; set; }
        public string orderProductMemo { get; set; }
        public bool productShipmentCheck { get; set; }
        
        public string ModelName { get; set; }
        public string PartnerName { get; set; }
        public IEnumerable<OutOrderProductStockInterface> outOrderProductStocks { get; set; }
        public IEnumerable<OutOrderProductDefectiveInterface> outOrderProductDefectives { get; set; }


    }
    public class OrderRes004
    {
        public int orderProductId { get; set; }
        public int orderId { get; set; }
        public int productId { get; set; }

        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }

        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
        public string productStandardUnit { get; set; }
        public int productStandardUnitCount { get; set; }

        public string productTaxInfo { get; set; }
        public int productOrderCount { get; set; }
        public int productSellPrice { get; set; }
        public int productSupplyPrice { get; set; }
        public double productTaxPrice { get; set; }
        public double productTotalPrice { get; set; }
        public string orderProductMemo { get; set; }

    }
    [Keyless]
    public class OrderRes005
    {

        public int? productId { get; set; }
        public int? uploadFileId { get; set; }
        public string? uploadFileName { get; set; }
        public string? uploadFileUrl { get; set; }

        public string? productCode { get; set; }
        public string? productClassification { get; set; }
        public string? productName { get; set; }
        public string? productStandard { get; set; }
        public string? productUnit { get; set; }
        public string? productTaxInfo { get; set; }

        public int? productBuyPrice { get; set; }
        public int? productSellPrice { get; set; }
        public bool productImportCheck { get; set; }
        public bool productShipmentCheck { get; set; }
        public string? productMemo { get; set; }


        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }

        public Boolean? productIsUsing { get; set; }
        //public int? isUsing { get; set; }
        public Boolean? isDeleted { get; set; }

        public IEnumerable<OutOrderProductStockInterface> outOrderProductStocks { get; set; }
        public IEnumerable<OutOrderProductDefectiveInterface> outOrderProductDefectives { get;set; }

    }

    public class OutOrderProductStockInterface
    {
        public int LotCountId { get; set; }
        public int outOrderProductsStockId { get; set; }
        public int productId { get; set; }
        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
        public string productLOT { get; set; }
        public int inventory { get; set; }
        public bool isSelected { get; set; }
        public bool Checked { get; set; }
        public int quantity { get; set; }
        public int tempQuantity { get; set; }
        public bool productShipmentCheck { get; set; }
        public string productShipmentCheckResult { get; set; }
        public string tempId { get; set; }
    }

    public class OutOrderProductDefectiveInterface
    {
        public int outOrderProductDefectiveId { get; set; }
        public int defectiveId { get; set; }
        public string productLOT { get; set; }
        public string defectiveCode { get; set; }
        public string defectiveName { get; set; }
        public int defectiveQuantity { get; set; }
        public string defectiveProductMemo { get; set; }
        public int productId { get; set; }
    }

    public class OrderRes006
    {
        public string userId { get; set; }

        public string userNo { get; set; }
        public string userFullName { get; set; }
        public string userRole { get; set; }
        public string userDepartment { get; set; }
        public string userPosition { get; set; }
        public string userEmail { get; set; }
        public string userPhoneNumber { get; set; }
        public Boolean userIsApproved { get; set; }

    }

    public class OrderRes007
    {
        public int partnerId { get; set; }

        public string partnerCode { get; set; }
        public string partnerClassification { get; set; }
        public string partnerName { get; set; }
        public string businessNumber { get; set; }
        public string presidentName { get; set; }
        public string telephoneNumber { get; set; }
        public string faxNumber { get; set; }
        public string partnerMemo { get; set; }
        public string contactName { get; set; }
        public string contactEmail { get; set; }
        public string partnerTaxInfo { get; set; }
        public Boolean partnerStatus { get; set; }

        public bool Group_Buy { get; set; }
        public bool Group_Sell { get; set; }
        public bool Group_Finance { get; set; }
        public bool Group_Etc { get; set; }

    }
}
