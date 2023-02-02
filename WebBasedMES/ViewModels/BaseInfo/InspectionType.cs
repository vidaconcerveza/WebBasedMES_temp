using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.ViewModels.BaseInfo
{
    public class InspectionTypeResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } //검사유형 
        public string Class { get; set; } //금형 OR 설비
        public string Type { get; set; } //일상 OR 정기
        public bool IsUsing { get; set; } = true;
        public string InspectionItem { get; set; }
        public string InspectionMethod { get; set; }
        public string InspectionStandard { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Memo { get; set; }




    }

    public class InspectionTypeRequest
    {
        public int Id { get; set; }
        public int[] Ids { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } //검사유형 
        public string Class { get; set; } //금형 OR 설비
        public string Type { get; set; } //일상 OR 정기
        public bool IsUsing { get; set; } = true;
        public string InspectionItem { get; set; }
        public string InspectionMethod { get; set; }
        public string InspectionStandard { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Memo { get; set; }

        public string SearchInput { get; set; } = "";
        public string TypeStr { get; set; } = "ALL";
        public string IsUsingStr { get; set; } = "ALL";
        public bool AutoCode { get; set; }

    }
}
