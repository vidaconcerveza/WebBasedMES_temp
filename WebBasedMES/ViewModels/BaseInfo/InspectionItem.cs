using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.ViewModels.BaseInfo
{
    public class InspectionItemResponse
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } //검사유형 
        public bool IsUsing { get; set; } = true;
        public string InspectionItem { get; set; }
        public string InspectionStandard { get; set; }
        public string Memo { get; set; }

        public int CommonCode { get; set; }
        public string CommonCodeName { get; set; }

        public string Classify { get; set; }
        public string InspectionType { get; set; }
        public string InspectionCount { get; set; }
        public string JudgeStandard { get; set; }
        public string JudgeMethod { get; set; }
        public string Creator { get; set; }
        public string CreateDateTime { get; set; }

    }

    public class InspectionItemRequest
    {
        public int Id { get; set; }
        public int[] Ids { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } //검사유형 
        public bool IsUsing { get; set; } = true;
        public string InspectionItem { get; set; }
        public string InspectionStandard { get; set; }
        public string Memo { get; set; }
        public int CommonCode { get; set; }
        public string CommonCodeName { get; set; }
        public string Classify { get; set; }
        public string InspectionType { get; set; }
        public int InspectionCount { get; set; }
        public string JudgeStandard { get; set; }
        public string JudgeMethod { get; set; }
        public string Creator { get; set; }
        public string CreateDateTime { get; set; }
        public string SearchInput { get; set; } = "";
        public string TypeStr { get; set; } = "ALL";
        public string IsUsingStr { get; set; } = "ALL";
        public bool AutoCode { get; set; }

    }
}
