using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models.BaseInfo
{
    public class InspectionType
    {
        public int Id { get; set; }
        public CommonCode CommonCode { get; set; } 
        public string Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } //검사유형 
        public bool IsUsing { get; set; } = true;
        public string InspectionItem { get; set; }
        public string InspectionMethod { get; set; }
        public string InspectionStandard { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Memo { get; set; }

    }
}
