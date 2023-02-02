using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.ProduceStatus
{

    public class FacilityWorkStatusReq001
    {
        public int facilityWorkStatusId { get; set; }
        public int facilityId { get; set; }
        public int moldId { get; set; }
       
        
    }
    [Keyless]
    public class FacilityWorkStatusRes001
    {
        public int? facilityWorkStatusId { get; set; }
        public int? facilityId { get; set; }
        public int? moldId { get; set; }
        public string? status { get; set; } //상태
        public string? facilitiesName { get; set; }      //설비이름
        public string? moldName { get; set; }
        public int? isWorking { get; set; }
        public int? spm { get; set; } //SPM
        public int? goalProductionQuantity { get; set; } // 목표생산량
        public int? currentProductionQuantity { get; set; } // 현재생산량
        public int? temperatureCurrent { get; set; } // 현재온도
        public int? humidityCurrent { get; set; } // 현재습도
        public int? temperatureUpper { get; set; } //온도 상한값
        public int? temperatureLower { get; set; }//온도 하한값
        public int? motorElectricCurrent { get; set; } // 메인모터전류
        public int? tonCurrent { get; set; } //TON
        public int? slideCurrent { get; set; } //슬라이드 현재값
        public int? slideElectricCurrent { get; set; } //슬라이드 전류
        public int? slideUpper { get; set; } //슬라이드 상한값
        public int? slideLower { get; set; } //슬라이드 하한값
    }
 }
