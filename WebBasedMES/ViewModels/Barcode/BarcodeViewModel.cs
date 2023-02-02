namespace WebBasedMES.ViewModels.Barcode
{
    public class InBarcodeRequest
    {
        public string ReceivingStartDate { get; set; }
        public string ReceivingEndDate { get; set; }
        public string ReceivingNo { get; set; }
        public int PartnerId { get; set; }
        public int ProductId { get; set; }
        public string ProductLOT { get; set; }
        public int StoreOutStoreProductId { get; set; }
        public int BarcodeIssueCount { get; set; }
        public int BarcodeReIssueCount { get; set; }
        public string InvenType { get; set; }
        public bool ReIssue { get; set; }
    }

    public class InBarcodeResponse
    {
        public int StoreOutStoreProductId { get; set; }
        public int ReceivingId { get; set; }
        public int OutStoreProductId { get; set; }
        public int ProductId { get; set; }
        public string OutStoreNo { get; set; } 
        public string ProductCode { get; set; } 
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public string ProductStandard { get; set; }
        public string ProductStandardUnit { get; set; } 
        public int ProductStandardUnitCount { get; set; }
        public string ProductTaxInfo { get; set; } 
        public int ProductReceivingCount { get; set; }
        public int ProductBuyPrice { get; set; }
        public int ProductSupplyPrice { get; set; }
        public int ProductTaxPrice { get; set; } 
        public int ProductTotalPrice { get; set; } 
        public string ProductLOT { get; set; } 
        public string ReceivingProductMemo { get; set; }
        public string BarcodeIssueDate { get; set; }
        public int? BarcodeIssueCount { get; set; }
        public int? BarcodeReIssueCount { get; set; }
        public string PartnerName { get; set; }
        public string ReceivingDate { get; set; }

    }
    public class OutBarcodeRequest
    {
        public string ShipmentStartDate { get; set; }
        public string ShipmentEndDate { get; set; }
        public string OrderNo { get; set; }
        public string ShipmentNo { get; set; }
        public int PartnerId { get; set; }
        public int ProductId { get; set; }
        public string ProductLOT { get; set; }
        public int OutOrderProductStockId { get; set; }
        public int BarcodeIssueCount { get; set; }
        public int BarcodeReIssueCount { get; set; }
        public string InvenType { get; set; }

        public bool ReIssue { get; set; }

    }

    public class OutBarcodeResponse
    {
        public int OutOrderId { get; set; }
        public int OutOrderProductStockId { get; set; }
        public string OrderNo { get; set; }
        public string ShipmentNo { get; set; }
        public string ShipmentDate { get; set; }
        public string PartnerName { get; set; }
        public string ProductCode { get; set; } //품목 코드
        public string ProductClassification { get; set; }//품목구분
        public string ProductName { get; set; }//품목이름
        public string ProductStandard { get; set; }//규격
        public string ProductUnit { get; set; }//단위
        public string ProductTaxInfo { get; set; }//단위
        public string ProductStandardUnit { get; set; } //기준단위
        public int ProductStandardUnitCount { get; set; } //기준단위수량
        public int ProductShipmentProductCount { get; set; } //출고수량
        public int ProductSellPrice { get; set; }//판매단가
        public int ProductSupplyPrice { get; set; } //공급가액
        public int ProductTaxPrice { get; set; }
        public int ProductTotalPrice { get; set; }
        public string ProductLOT { get; set; } //기준단위수량
        public string BarcodeIssueDate { get; set; }

        public int BarcodeIssueCount { get; set; }
        public int BarcodeReIssueCount { get; set; }

    }


    public class ProcessBarcodeRequest
    {
        public string WorkStartDate { get; set; }
        public string WorkEndDate { get; set; }
        public int ProductId { get; set; }
        public int ProcessId { get; set; }
        public int FacilityId { get; set; }
        public int MoldId { get; set; }
        public string ProductLOT { get; set; }
        public int ProcessProgressId { get; set; }
        public int BarcodeIssueCount { get; set; }
        public int BarcodeReIssueCount { get; set; }
        public bool ReIssue { get; set; }

    }

    public class ProcessBarcodeResponse
    {
        public int ProcessProgressId { get; set; }
        public string WorkEndDate { get; set; }
        public string WorkStartDate { get; set; }
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductClassification { get; set; }
        public string ProductUnit { get; set; }
        public int ProductionCount { get; set; }
        public string ProductLOT { get; set; }
        public string BarcodeIssueDate { get; set; }
        public int BarcodeIssueCount { get; set; }
        public int BarcodeReIssueCount { get; set; }
    }

    public class BarcodeMasterRequest
    {
        public int ProductId { get; set; }
        public int PartnerId { get; set; }
        public int ProcessId { get; set; }
        public int FacilityId { get; set; }
        public int DefectiveId { get; set; }
        public int MoldId { get; set; }
        public string SearchString { get; set; }
        public string Type { get; set; }
        public bool ReIssue { get; set; }

        public int BarcodeIssueCount { get; set; }
        public int BarcodeReIssueCount { get; set; }

    }
    public class BarcodeMasterResponse
    {
        public int ProductId { get; set; }
        public int PartnerId { get; set; }
        public int ProcessId { get; set; }
        public int FacilityId { get; set; }
        public int DefectiveId { get; set; }
        public int MoldId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; }
        public string Type { get; set; }
        public string Standard { get; set; }
        public string Unit { get; set; }
        public string Memo { get; set; }
        public string IsUsing { get; set; }
        public string BarcodeIssueDate { get; set; }
        public int BarcodeIssueCount { get; set; }
        public int BarcodeReIssueCount { get; set; }
    }



}
