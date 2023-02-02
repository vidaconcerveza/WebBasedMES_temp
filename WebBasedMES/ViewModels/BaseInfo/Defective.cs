using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.ViewModels.BaseInfo
{
    public class DefectiveResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } //불량유형
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string Creator { get; set; } //최초만 하자..
        public string CreateDate { get; set; } //최초만 하래 이것도...
        public string Memo { get; set; }
    }


    public class DefectiveRequest
    {
        public int Id { get; set; }
        public int[] Ids { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } //불량유형
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string Creator { get; set; } //최초만 하자..
        public string CreateDate { get; set; } //최초만 하래 이것도...
        public string Memo { get; set; }
        public string IsUsingStr { get; set; }//검색조건:사용 여부
        public string SearchInput { get; set; }//검색조건:검색 text
        public string DefectiveIsUsing { get; set; }
        public string SearchStr { get; set; }
        public string Uuid { get; set; }
        public bool AutoCode { get; set; }

    }


    public class DefectivePopupRequest
    {
        public string SearchInput { get; set; }
        public string DefectiveIsUsing { get; set; }
    }

    public class DefectivePopupResponse
    {
        public int DefectiveId { get; set; }
        public string DefectiveCode { get; set; }
        public string DefectiveName { get; set; }
        public string DefectiveMemo { get; set; }
        public string DefectiveIsUsing { get; set; }
    }

    public class DownTimeRequest
    {
        public string SearchStr { get; set; }
        public string IsUsing { get; set; }
    }

}
