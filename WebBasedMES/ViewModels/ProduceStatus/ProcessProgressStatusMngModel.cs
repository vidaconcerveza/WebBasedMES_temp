using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.ProduceStatus
{

    public class ProcessProgressStatusReq001
    {
        public int processProgressStatusId { get; set; }
        public int productId { get; set; }
        public int moldId { get; set; }
        public int facilityId { get; set; }
        public string currentDate { get; set; }   //현재 날짜
        
    }
    [Keyless]
    public class ProcessProgressStatusRes001
    {
        public int? processProgressStatusId { get; set; }
        public int? productId { get; set; }
        public int? moldId { get; set; }
        public int? facilityId { get; set; }
        public string? currentDate { get; set; }   //현재 날짜
        public string? productName { get; set; }
        public int? totalProductionRate { get; set; }//총 생산률
        public int? totalGoalQuantity { get; set; } //총 목표수량
        public int? totalProductionQuantity { get; set; } //총 생산수량
        public string? workNo { get; set; } //작업번호
        public string? status { get; set; } //작업번호
        public string? facilitiesName { get; set; }      //설비명
        public string? moldName { get; set; }      //금형이름
        public int? goalQuantity { get; set; } // 목표량
        public int? productionQuantity { get; set; } // 생산량
        public int? productionRate { get; set; } // 생산률
        public int? productGoodQuantity { get; set; } // 양품
        public int? productDefectiveQuantity { get; set; } // 불량품
        public string? productionStartDate { get; set; } //생산시작일
        public string? productionEndDate { get; set; } //생산완료일

    }
    
}
