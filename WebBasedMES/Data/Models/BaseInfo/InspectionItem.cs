using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models.BaseInfo
{
    public class InspectionItem
    {
        public int Id { get; set; }
        public CommonCode CommonCode { get; set; }  //점검주기
        public string Code { get; set; }
        public string Name { get; set; } //검사유형 
        public string Classify { get; set; } //금형 OR 설비
        public string Type { get; set; } //일상 OR 정기

        public string InspectionType { get; set; }
        public int InspectionCount { get; set; }
        public string InspectionItems { get; set; }
        public string JudgeStandard { get; set; }
        public string JudgeMethod { get; set; }
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string Memo { get; set; }
    
        public ApplicationUser Creator { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
