using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WebBasedMES.ViewModels.ProduceStatus
{

    public class MoldStatusReq001
    {
        public int moldStatusId { get; set; }
        public int moldId { get; set; }
        public int inspectionId { get; set; }
        public int cleanId { get; set; }
        public string currentDate { get; set; }   //현재 날짜
        
    }
    [Keyless]
    public class MoldStatusRes001
    {
        public int? moldStatusId { get; set; }
        public int? moldId { get; set; }
        public int? inspectionId { get; set; }
        public string? moldCode { get; set; }      //금형코드
        public string? moldClassification { get; set; } //금형구분
        public string? moldName { get; set; }      //금형이름
        public int? moldGuaranteeCount { get; set; } //보증타발수
        public int? moldCurrentCount { get; set; } //누적타발수
        public int? inspectionPeriod { get; set; } //점검 주기
        public int? inspectionBlankingCount { get; set; } //점검 타발 수
        public string? recentInspectionDate { get; set; }   //최근 점검일
        public int? inspectionAfterBlankingCount { get; set; } //점검 후 타발 수
        public string? nextInspectionDate { get; set; }   //다음 점검일

    }
    [Keyless]
    public class MoldStatusRes002
    {
        public int? moldStatusId { get; set; }
        public int? moldId { get; set; }
        public int? cleanId { get; set; }
        public string? moldCode { get; set; }      //금형코드
        public string? moldClassification { get; set; } //금형구분
        public string? moldName { get; set; }      //금형이름
        public int? moldGuaranteeCount { get; set; } //보증타발수
        public int? moldCurrentCount { get; set; } //누적타발수
        public int? cleanPeriod { get; set; } //세척 주기
        public int? cleanBlankingCount { get; set; } //세척 타발 수
        public string? recentCleanDate { get; set; }   //최근 세척일
        public int? cleanAfterBlankingCount { get; set; } //세척 후 타발 수
        public string? nextCleanDate { get; set; }   //다음 세척일

    }
}
