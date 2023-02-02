using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.ProduceStatus
{

    public class FacilityStatusReq001
    {
        public int facilityStatusId { get; set; }
        public int facilityId { get; set; }
        public int inspectionId { get; set; }
        public string currentDate { get; set; }   //현재 날짜
        
    }
    [Keyless]
    public class FacilityStatusRes001
    {
        public int? facilityStatusId { get; set; }
        public int? facilityId { get; set; }
        public int? inspectionId { get; set; }
        
        public string? facilitiesCode { get; set; }      //설비코드
        public string? facilitiesClassification { get; set; } //설비구분
        public string? facilitiesName { get; set; }      //설비이름
        public int? inspectionPeriod { get; set; } //점검 주기
        public int? inspectionBlankingCount { get; set; } //점검 타발 수
        public string? recentInspectionDate { get; set; }   //최근 점검일
        public int? inspectionAfterBlankingCount { get; set; } //점검 후 타발 수
        public string? nextInspectionDate { get; set; }   //다음 점검일

    }
 }
