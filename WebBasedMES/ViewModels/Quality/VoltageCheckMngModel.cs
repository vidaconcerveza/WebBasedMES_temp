using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.Quality
{

    public class VoltageCheckReq001
    {

        public int voltageCheckId { get; set; }
        public int workOrderId { get; set; }            //작업지시Id
        public int processId { get; set; }
        public int facilityId { get; set; }
        public int moldId { get; set; }

        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderStartDate { get; set; }   //작업지시일
        public string workOrderEndDate { get; set; }     //작업지시일
        public string productLOT { get; set; }

    }
    [Keyless]
    public class VoltageCheckRes001
    {
        public int voltageCheckId { get; set; }
        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderDate { get; set; }//작업지시일
        public string processCode { get; set; }//공정코드
        public string processName { get; set; }//공정이름
        public string facilitiesCode { get; set; }      //설비코드
        public string facilitiesName { get; set; }      //설비이름
        public string facilitiesUID { get; set; }
        public string moldCode { get; set; }            //금형코드
        public string moldName { get; set; }            //금형이름
        public string productLOT { get; set; }
        public string spm { get; set; }
        public string motorElectricCurrent { get; set; }
        public string slideElectricCurrent { get; set; }
        public DateTime dateFlag { get; set; }
        public DateTime WorkStartDateTime { get; set; }
        public int processId { get; set; }

    }

    public class BottomDeadPointResponse
    {
        public int voltageCheckId { get; set; }
        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderDate { get; set; }//작업지시일
        public string processCode { get; set; }//공정코드
        public string processName { get; set; }//공정이름
        public string facilitiesCode { get; set; }      //설비코드
        public string facilitiesName { get; set; }      //설비이름
        public string facilitiesUID { get; set; }
        public string moldCode { get; set; }            //금형코드
        public string moldName { get; set; }            //금형이름
        public string productLOT { get; set; }
        public string spm { get; set; }
        public string motorElectricCurrent { get; set; }

        public string bottomDeadPoint { get; set; }
        public string bottomDeadPoint_UL { get; set; }
        public string bottomDeadPoint_LL { get; set; }
        public DateTime dateFlag { get; set; }
        public DateTime WorkStartDateTime { get; set; }
        public int processId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productClassification { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
    }

    public class SlideCurrentResponse
    {
        public int voltageCheckId { get; set; }
        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderDate { get; set; }//작업지시일
        public string processCode { get; set; }//공정코드
        public string processName { get; set; }//공정이름
        public string facilitiesCode { get; set; }      //설비코드
        public string facilitiesName { get; set; }      //설비이름
        public string facilitiesUID { get; set; }
        public string moldCode { get; set; }            //금형코드
        public string moldName { get; set; }            //금형이름
        public string productLOT { get; set; }

        public string slideElectricCurrent { get; set; }
        public string slideValue { get; set; }
        public string slideValue_UL { get; set; }
        public string slideValue_LL { get; set; }
        public DateTime dateFlag { get; set; }
        public DateTime WorkStartDateTime { get; set; }
        public int processId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productClassification { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
    }

    public class TonCheckResponse
    {
        public int voltageCheckId { get; set; }
        public string workOrderNo { get; set; } //작업지시번호
        public string workOrderDate { get; set; }//작업지시일
        public string processCode { get; set; }//공정코드
        public string processName { get; set; }//공정이름
        public string facilitiesCode { get; set; }      //설비코드
        public string facilitiesName { get; set; }      //설비이름
        public string facilitiesUID { get; set; }
        public string moldCode { get; set; }            //금형코드
        public string moldName { get; set; }            //금형이름
        public string productLOT { get; set; }
        public string spm { get; set; }
        public string motorElectricCurrent { get; set; }

        public string tonValue { get; set; }
        public string tonValue_UL { get; set; }
        public string tonValue_LL { get; set; }
        public DateTime dateFlag { get; set; }
        public DateTime WorkStartDateTime { get; set; }
        public int processId { get; set; }

        public string productCode { get; set; }
        public string productName { get; set; }
        public string productClassification { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
    }


    public class VoltageInspectionRequest
    {
        public int VoltageInspectionId { get; set; }
        public int[] VoltageInspectionIds { get; set; }
        public string InspectionStartDate { get; set; }
        public string InspectionEndDate { get; set; }
        public int ProductId { get; set; }
        public string Lot { get; set; }


        public string InspectionDate { get; set; }
        public string Memo { get; set; }
        public string Ton { get; set; }
        public string LoadCell { get; set; }

        public string MainMotorAmp { get; set; }
        public string MainMotorOverAmp { get; set; }
        public string SlideAmp { get; set; }
        public string SlideMotorOverAmp { get; set; }
        public string MotorSpm { get; set; }
        public string PressMaxSpm { get; set; }
        public string Slp { get; set; }

        public string A1Speed { get; set; }
        public string A1Slp { get; set; }

        public string A2Speed { get; set; }
        public string A2Slp { get; set; }

        public string A3Speed { get; set; }
        public string A3Slp { get; set; }

        public string A4Speed { get; set; }
        public string A4Slp { get; set; }

        public string A5Speed { get; set; }
        public string A5Slp { get; set; }

    }

    public class VoltageInspectionResponse
    {
        public int VoltageInspectionId { get; set; }
        public string InspectionStartDate { get; set; }
        public string InspectionEndDate { get; set; }
        public int ProductId { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductClassification { get; set; }

        public string Lot { get; set; }


        public string InspectionDate { get; set; }
        public string Memo { get; set; }
        public string Ton { get; set; }
        public string LoadCell { get; set; }

        public string MainMotorAmp { get; set; }
        public string MainMotorOverAmp { get; set; }
        public string SlideAmp { get; set; }
        public string SlideMotorOverAmp { get; set; }
        public string MotorSpm { get; set; }
        public string PressMaxSpm { get; set; }
        public string Slp { get; set; }

        public string A1Speed { get; set; }
        public string A1Slp { get; set; }

        public string A2Speed { get; set; }
        public string A2Slp { get; set; }

        public string A3Speed { get; set; }
        public string A3Slp { get; set; }

        public string A4Speed { get; set; }
        public string A4Slp { get; set; }

        public string A5Speed { get; set; }
        public string A5Slp { get; set; }
    }

    public class InspectionDataResponse
    {
        public string MainMotorAmp { get; set; }
        public string MainMotorOverAmp { get; set; }
        public string SlideAmp { get; set; }
        public string SlideMotorOverAmp { get; set; }
        public string MotorSpm { get; set; }
        public string PressMaxSpm { get; set; }
        public string Slp { get; set; }

        public string A1Speed { get; set; }
        public string A1Slp { get; set; }

        public string A2Speed { get; set; }
        public string A2Slp { get; set; }

        public string A3Speed { get; set; }
        public string A3Slp { get; set; }

        public string A4Speed { get; set; }
        public string A4Slp { get; set; }

        public string A5Speed { get; set; }
        public string A5Slp { get; set; }

        public string UpdateTime { get; set; }
    }

    public class TempHumidRequest
    {
        public int FacilityId { get; set; }
    }

    public class TempHumidResponse
    {
        public string Date { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string FacilityType { get; set; }
        public string Memo { get; set; }
        public string Temp { get; set; }
        public string Humid { get; set; }
        public string Temp_UL { get; set; }
        public string Temp_LL { get; set; }
    }
}
