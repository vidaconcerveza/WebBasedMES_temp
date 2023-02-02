using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBasedMES.ViewModels.Process
{

    #region 비가동내역부터
    public class ProcessNotWorkQueryRequest
    {
        public string shutdownStartDate { get; set; }
        public string shutdownEndDate { get; set; }
        public int shutdownCodeId { get; set; }
        public int processId { get; set; }
    }

    public class ProcessNotWorkQueryResponse
    {

        public int shutdownCodeId { get; set; }
        public string shutdownStartDateTime { get; set; }
        public string shutdownEndDateTime { get; set; }
        public int downtime { get; set; }
        public string shutdownCode { get; set; }
        public string shutdownName { get; set; }
        public string processShutdownMemo { get; set; }
        public string workOrderNo { get; set; }
        public string workOrderDate { get; set; }
        public int processOrder { get; set; }
        public int processId { get; set; }
        public string processCode { get; set; }
        public string processName { get; set; }
        public bool isOutSourcing { get; set; }
        public string partnerName { get; set; }
        public string facilitiesCode { get; set; }
        public string facilitiesName { get; set; }
        public string moldCode { get; set; }
        public string moldName { get; set; }
        public string workerName { get; set; }
        public int productionQuantity { get; set; }
        public int processElapsedTime { get; set; }
        public string productLOT { get; set; }
    }

    #endregion 비가동내역부터


    public class ProductionManageByProcessRequest
    {
        public int ProcessId { get; set; }
        public string WorkStartDate { get; set; }
        public string WorkEndDate { get; set; }
    }

    public class ProductionManageByFacilityRequest
    {
        public string ProductLOT { get; set; }
        public int FacilityId { get; set; }
        public int MoldId { get; set; }
        public string WorkStartDate { get; set; }
        public string WorkEndDate { get; set; }
    }


    public class ProductionManageByFacilityResponse
    {
        public string RegisterDate { get; set; }
        public string WorkOrderNo { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductClassification { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int ProductionQuantity { get; set; }
        public int ProductionElapseTime { get; set; }
        public int ProductionDownTime { get; set; }
        public string WorkerName { get; set; }
        public string ProductLOT { get; set; }
    }

    public class ProductionManageByProcessProcessListResponse
    {
        public string WorkEndDateTime { get; set; }
        public string WorkStartDateTime { get; set; }
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string ProcessMemo { get; set; }
    }

    public class ProductionManageByProcessWorkOrderListResponse
    {
        public string WorkOrderNo { get; set; }
        public string WorkEndDate { get; set; }
        public int WorkOrderSequence { get; set; }
        public bool IsOutSourcing { get; set; }
        public string PartnerName { get; set; }
        public string FacilitiesCode { get; set; }
        public string FacilitiesName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string WorkerName { get; set; }
        public int ProcessWorkQuantity { get; set; }
        public int ProductionQuantity { get; set; }
        public bool ProcessCheck { get; set; }
        public int ProcessId { get; set; }
        public string WorkOrderDate { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public string ProductClassification { get; set; }
        public string ProductLOT { get; set; }
        public int ProductGoodQuantity { get; set; }
        public int ProductBadQuantity { get; set; }

    }

    public class DailyProductionRequest
    {
        public string WorkEndDateTime { get; set; }
        public string ProductClassification { get; set; }
        public int ProductId { get; set; }
    }

    public class DailyProductionResponse
    {
        public string WorkOrderNo { get; set; }
        public string WorkEndDateTime { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int ProductionQuantity { get; set; }
        public int ProductGoodQuantity { get; set; }
        public int ProductDefectiveQuantity { get; set; }
        public string ProductLOT { get; set; }

        public DateTime TimeFlag { get; set; }
    }

    public class DailyDefectiveRequest
    {
        public string DefectiveDate { get; set; }
        public int DefectiveId { get; set; }
        public string Type { get; set; }
    }

    public class DailyDefectiveSummaryResponse
    {
        public int TotalDefectiveQuantity { get; set; }
        public int StoreProductDefectiveQuantity { get; set; }
        public int EtcDefectiveQuantity { get; set; }
        public int OutOrderProductDefectiveQuantity { get; set; }
        public int ProcessDefectiveQuantity { get; set; }
    }

    public class DailyDefectiveDetailResponse
    {
        public string DefectiveDate { get; set; }
        public string DefectiveCode { get; set; }
        public string DefectiveName { get; set; }
        public int ProductDefectiveQuantity { get; set; }
        public string DefectiveUnit { get; set; }
        public string Type { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public string Number { get; set; } //입출고 넘버
        public string PartnerName { get; set; }
        public string RegisterDate { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkStartDateTime { get; set; }
        public string ProcessName { get; set; }
        public string ProductLOT { get; set; }
        public string Memo { get; set; }
    }

    public class DailyOutOrderRequest
    {
        public string ShipmentDate { get; set; }
        public int PartnerId { get; set; }
        public int ProductId { get; set; }
    }

    public class DailyOutOrderResponse
    {
        public string ShipmentNo { get; set; }
        public string ShipmentDate { get; set; }
        public string PartnerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public string ProductStandardUnit { get; set; }
        public int ProductStandardUnitCount { get; set; }
        public int ProductShipmentCount { get; set; }
        public int ProductSellPrice { get; set; }
        public int ProductSupplyPrice { get; set; }
        public int ProductTaxPrice { get; set; }
        public int ProductTotalPrice { get; set; }
        public string ProductLOT { get; set; }
        public string ShipmentProductMemo { get; set; }
        public string ProductStandard { get; set; }

    }


    public class LotInfoRequest
    {
        public string productLOT { get; set; }
    } 

    public class LotInfoResponse
    {
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int ProductWorkQuantity { get; set; }
        public int ProductionQuantity { get; set; }
        public int ProductGoodQuantity { get; set; }
        public int ProductDefectiveQuantity { get; set; }
        public int TotalElapsedTime { get; set; }
        public string WorkOrderStatus { get; set; }
        public string ProductLOT { get; set; }

        public IEnumerable<LotProcessInfo> LotProcessInfo { get; set; }

    }

    public class LotProcessInfo
    {
        public string WorkStartDateTime { get; set; }
        public string WorkEndDateTime { get; set; }
        public int ProcessOrder { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public bool IsOutSourcing { get; set; }
        public string PartnerName { get; set; }
        public string FacilitiesCode { get; set; }
        public string FacilitiesName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string WorkerName { get; set; }
        public int ProductionQuantity { get; set; }
        public int ProductGoodQuantity { get; set; }
        public int ProductDefectiveQuantity { get; set; }
        public string ProductLOT { get; set; }
    } 

    public class LotInfoInputItemResponse
    {
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string ItemCode { get; set; }
        public string ItemClassification { get; set; }
        public string ItemName { get; set; }
        public string ItemStandard { get; set; }
        public string ItemUnit { get; set; }
        public float RequiredQuantity { get; set; }
        public float LOSS { get; set; }
        public float TotalRequiredQuantity { get; set; }
        public int ProductionQuantity { get; set; }
        public int TotalInputQuantity { get; set; }
        public string ItemLOT { get; set; }
    }

    public class LotInfoProcessDefectiveResponse
    {
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public bool IsOutSourcing { get; set; }
        public string PartnerName { get; set; }
        public string DefectiveCode { get; set; }
        public string DefectiveName { get; set; }
        public int DefectiveQuantity { get; set; }
        public string DefectiveProductMemo { get; set; }
    }

    public class LotInfoProcessInspectionResponse
    {
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public bool IsOutSourcing { get; set; }
        public string PartnerName { get; set; }
        public string FacilitiesCode { get; set; }
        public string FacilitiesName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string WorkerName { get; set; }
        public int ProductionQuantity { get; set; }
        public bool ProcessCheck { get; set; }
        public string ProcessCheckResult { get; set; }
        public string WorkProcessMemo { get; set; }

    }

    public class LotInfoProductOutResponse
    {
        public string ShipmentNo { get; set; }
        public string ShipmentDate { get; set; }
        public string PartnerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public string ProductLOT { get; set; }
        public int Quantity { get; set; }
        public bool ProductShipmentCheck { get; set; }
        public string ProductShipmentCheckResult { get; set; }
        public string RegisterName { get; set; }
        public string ShipmentProductMemo { get; set; }

    }
}
