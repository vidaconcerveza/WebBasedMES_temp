using System.Collections.Generic;

namespace WebBasedMES.ViewModels.FacilityManage
{
    public class FacilityBaseInfoRequest
    {
        public int FacilityBaseInfoId { get; set; }
        public int[] FacilityBaseInfoIds { get; set; }
        public int FacilityId { get; set; }
        public int MoldId { get; set; }
        public int MoldPositionSensor { get; set; }
        public double BottomDeadPoint { get; set; }
        public double Slide { get; set; }
        public int Ton { get; set; }
        public string RegisterId { get; set; }
        public string RegisterDate { get; set; }
        public string Memo { get; set; }
        public double BottomDeadPointUL { get; set; }
        public double SlideUL { get; set; }
        public int TonUL { get; set; }
        public double BottomDeadPointDL { get; set; }
        public double SlideDL { get; set; }
        public int TonDL { get; set; }
    }

    public class FacilityBaseInfoResponse
    {
        public int FacilityBaseInfoId { get; set; }
        public int[] FacilityBaseInfoIds { get; set; }
        public int FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string FacilityType { get; set; }
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string MoldType { get; set; }
        public int MoldPositionSensor { get; set; }
        public double BottomDeadPoint { get; set; }
        public double Slide { get; set; }
        public int Ton { get; set; }
        public string RegisterId { get; set; }
        public string RegisterName { get; set; }
        public string RegisterDate { get; set; }
        public string Memo { get; set; }
        public double BottomDeadPointUL { get; set; }
        public double SlideUL { get; set; }
        public int TonUL { get; set; }
        public double BottomDeadPointDL { get; set; }
        public double SlideDL { get; set; }
        public int TonDL { get; set; }

        public string RegisterName2 { get; set; }

        public string Temp { get; set; }
        public string Humid { get; set; }

        public string FacilityMemo { get; set; }

        public string UpdateDate { get; set; }

    }

    public class FacilityErrorLogRequest
    {
        public int FacilityId { get; set; }
        public int MoldId { get; set; }
        public string LogStartDate { get; set; }
        public string LogEndDate { get; set; }
    }

    public class FacilityErrorLogResponse
    {
        public int FacilityErrorLogId { get; set; }
        public string CreateDateTime { get; set; }
        public string FacilityCode { get; set; }

        public string FacilityName { get; set; }
        public string MoldName { get; set; }
        public string MoldCode { get; set; }
        public string BDP { get; set; }
        public string SLIDE { get; set; }
        public string TON { get; set; }
        public string ErrorNote { get; set; }

        public string Temp { get; set; }
        public string Humid { get; set; }

    }


    public class FacilityOperationRequest
    {

        //기본 조회
        public int FacilityOperationId { get; set; }
        public int[] FacilityOperationIds { get; set; }

        public string WorkStartDate { get; set; }
        public string WorkEndDate { get; set; }
        public int FacilityId { get; set; }
        public int MoldId { get; set; }
        public string ProductLOT { get; set; }
        public int ProductId { get; set; }

        //UPDATE/CREATE
        public string Date { get; set; }
        public string RegisterDate { get; set; }
        public string RegisterId { get; set; }
        public string WorkerId { get; set; }
        public int ElapsedTime { get; set; }
        public int ProductionQuantity { get; set; }
        public string Memo { get; set; }

        public IEnumerable<OperationInputItem> ProductInputItems { get; set; }
    }

    public class FacilityOperationResponse
    {
        public int FacilityOperationId { get; set; }
        public int FacilityId { get; set; }
        public int MoldId { get; set; }
        public int ProductId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductClassification { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }

        public string ProductLOT { get; set; }
        public string Date { get; set; }
        public string RegisterDate { get; set; }
        public string RegisterId { get; set; }
        public string RegisterName { get; set; }
        public string WorkerId { get; set; }
        public string WorkerName { get; set; }
        public int ElapsedTime { get; set; }
        public int ProductionQuantity { get; set; }
        public string Memo { get; set; }

        public string Temp { get; set; }
        public string Humid { get; set; }
        public IEnumerable<OperationInputItem> ProductInputItems { get; set; }

    }

    public class OperationInputItem
    {
        public int FacilityOperationInputItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductClassification { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int ProductionQuantity { get; set; }
        public double RequireQuantity { get; set; }
        public double LOSS { get; set; }
        public double TotalRequire { get; set; }
        public string ProductLOT { get; set; }

        public IEnumerable<OperationInputItemStock> ProductInputItemStocks { get; set; }
    }

    public class OperationInputItemStock
    {
        public string LotName { get; set; }
        public int InputQuantity { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductClassification { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int StockCount { get; set; }
        public bool IsSelected { get; set; }
    }


    public class FacilityControlRequest
    {
        public int FacilityControlId { get; set; }
        public int[] FacilityControlIds { get; set; }
        public int FacilityId { get; set; }
        public int MoldId { get; set; }
        public int MoldPositionSensor { get; set; }
        public double BottomDeadPoint { get; set; }
        public double Slide { get; set; }
        public int Ton { get; set; }
        public string RegisterId { get; set; }
        public string RegisterDate { get; set; }
        public string Memo { get; set; }
        public double BottomDeadPointUL { get; set; }
        public double SlideUL { get; set; }
        public int TonUL { get; set; }
        public double BottomDeadPointDL { get; set; }
        public double SlideDL { get; set; }
        public int TonDL { get; set; }
    }


    public class FacilityControlResponse
    {
        public int FacilityControlId { get; set; }
        public int[] FacilityControlIds { get; set; }
        public int FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string FacilityType { get; set; }
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string MoldType { get; set; }
        public int MoldPositionSensor { get; set; }
        public double BottomDeadPoint { get; set; }
        public double Slide { get; set; }
        public int Ton { get; set; }
        public string RegisterId { get; set; }
        public string RegisterName { get; set; }
        public string RegisterDate { get; set; }
        public string Memo { get; set; }
        public double BottomDeadPointUL { get; set; }
        public double SlideUL { get; set; }
        public int TonUL { get; set; }
        public double BottomDeadPointDL { get; set; }
        public double SlideDL { get; set; }
        public int TonDL { get; set; }

        public string RegisterName2 { get; set; }

        public string Temp { get; set; }
        public string Humid { get; set; }

        public string FacilityMemo { get; set; }

        public string UpdateDate { get; set; }

    }



}
