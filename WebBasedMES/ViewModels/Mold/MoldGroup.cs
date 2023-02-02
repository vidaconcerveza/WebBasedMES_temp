using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.Mold
{
    public class MoldGroupRequest
    {
        public int MoldGroupId { get; set; }
        public int[] MoldGroupIds { get; set; }
        public string MoldGroupCode { get; set; } 
        public string MoldGroupName { get; set; }
        public bool AutoCode { get; set; } = false;
        public bool IsAuto { get; set; }
        public bool IsUsing { get; set; }
        public string Memo { get; set; }
        public IEnumerable<MoldGroupInterface> MoldGroupElements { get; set; }
    }

    public class MoldGroupResponse
    {
        public int MoldGroupId { get; set; }
        public string MoldGroupCode { get; set; } 
        public string MoldGroupName { get; set; }
        public bool AutoCode { get; set; } = false;
        public bool IsAuto { get; set; }
        public bool IsUsing { get; set; }
        public string Memo { get; set; }

        public IEnumerable<MoldGroupInterface> MoldGroupElements { get; set; }

    }

    public class MoldGroupInterface
    {
        public int MoldGroupElementId { get; set; }
        public int FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityType { get; set; }
        public string FacilityName { get; set; }
        public string FacilityStandard { get; set; }
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldType { get; set; }
        public string MoldName { get; set; }
        public string MoldStandard { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
    }
}
