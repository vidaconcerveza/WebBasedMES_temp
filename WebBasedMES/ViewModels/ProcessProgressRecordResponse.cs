using System.Collections.Generic;

namespace WebBasedMES.ViewModels
{
    public class ProcessProgressRecordResponse
    {
        public string ProductName { get; set; }
        public int PlanWorkQuantity { get; set; }
        public string WorkOrderNo { get; set; }

        public IEnumerable<ProcessProgressRecordList> ProcessList { get; set; }
    }


    public class ProcessProgressRecordList
    {
        public int ProcessOrder { get; set; }
        public string ProcessName { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessStatus { get; set; }
        public string FacilityName { get; set; }
        public string MoldName { get; set; }
        public int OrderQuantity { get; set; }
        public int ProductionQuantity { get; set; }

        public int ProductionRatio { get; set; }
        public int GoodQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public string WorkStartDateTime { get; set; }
        public string WorkEndDateTime { get; set; }

        public string WorkStatus { get; set; }
    }


    public class ProcessOperationRecordResponse
    {
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string WorkStatus { get; set; }
        public string FacilityName { get; set; }
        public string MoldName { get; set; }
        public string ProductName { get; set; }
        public int OrderQuantity { get; set; }
        public int ProductionQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public int GoodQuantity { get; set; }
        public int Ratio { get; set; }

        public IEnumerable<ProcessOperationWork> ProcessOperationWorks { get; set; }

    }

    public class ProcessOperationWork
    {
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string WorkStatus { get; set; }
        public string FacilityName { get; set; }
        public string MoldName { get; set; }
        public string ProductName { get; set; }
        public int OrderQuantity { get; set; }
        public int ProductionQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public int GoodQuantity { get; set; }
        public int Ratio { get; set; }
    }

    public class ProductionRecordByContractResponse
    {
        public string ShipmentDate { get; set; }
        public string OrderDate { get; set; }
        public string OrderNo { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
        public int FinishProductCount { get; set; }
        public int OrderProductCount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int FinishRatio { get; set; }
        public int OrderProductQuantity { get; set; }

        public IEnumerable<ProductionRecordByContractProduct> ProductionRecordByContractProducts { get; set; }
    }

    public class ProductionRecordByContractProduct
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int FinishRatio { get; set; }
    }



    public class ProductionRecordByPlanResponse
    {
        public string PlanStartDate { get; set; }
        public string PlanEndDate { get; set; }
        public string PlanNo { get; set; }
        public int PlanProductCount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProductionQuantity { get; set; }
        public int ProductionPlanQuantity { get; set; }

        public IEnumerable<ProductionRecordByContractProduct> ProductionRecordByPlanProducts { get; set; }

    }


    public class FacilityManageRecordResponse
    {
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string FacilityClassification { get; set; }
        public string InspectionPeriod { get; set; }
        public string CurrentInspectionDate { get; set; }
        public string NextInspectionDate { get; set; }
        public bool InspectionAlert { get; set; }

    }

    public class MoldManageRecordInspectionResponse
    {
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string MoldClassification { get; set; }
        public int GuaranteeCount { get; set; }
        public int CurrentCount { get; set; }
        public string InspectionPeriod { get; set; }
        public int InspectionCount { get; set; }
        public string CurrentInspectionDate { get; set; }

        public int AfterInspectionCount { get; set; }
        public string NextInspectionDate { get; set; }
        public bool InspectionAlert { get; set; }

    }

    public class MoldManageRecordCleaningResponse
    {
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string MoldClassification { get; set; }
        public int GuaranteeCount { get; set; }
        public int CurrentCount { get; set; }
        public string WashPeriod { get; set; }
        public int WashCount { get; set; }
        public string CurrentWashDate { get; set; }
        public int AfterWashCount { get; set; }
        public string NextWashDate { get; set; }
        public bool WashAlert { get; set; }

    }



    public class FacilityOperationRecordResponse
    {
        public string FacilityName { get; set; }
        public string MoldName { get; set; }
        public bool FacilityStatus { get; set; }
        public int ProductionTarget { get; set; }
        public int ProductionCurrent { get; set; }
        public int Smp { get; set; }

        public int MainMotorAmp { get; set; }
        public int Ton { get; set; }

        public string Slide { get; set; }
        public string SlideAmp { get; set; }
        public string SlideUL { get; set; }
        public string SlideDL { get; set; }
    }

    public class FacilityStatus
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string Production_Current { get; set; }
        public string Production_Total { get; set; }
        public string Production_Target { get; set; }
        public string Production_Mold { get; set; }

        //Press
        public string MotorSpm { get; set; }
        public bool OnOffStatus { get; set; }
        public int TonValue { get; set; }
        public string Amp_Slide { get; set; }
        public int Amp_MainMotor { get; set; }
        public string Slide_Value { get; set; }
        public string Slide_UL { get; set; }
        public string Slide_LL { get; set; }
        public bool StartStop { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }


        public string D500 { get; set; }
        public string D501 { get; set; }
        public string D502 { get; set; }
        public string D503 { get; set; }
        public string D504 { get; set; }
        public string MoldName { get; set; }
        public int MaxAmp { get; set; }
        public int MaxTon { get; set; }

        public string Temp { get; set; }
        public string Humid { get; set; }
        public string UpdateDate { get; set; }

        public string StatusColor { get; set; }
        public int DefectiveQuantity { get; set; }
    }
}
