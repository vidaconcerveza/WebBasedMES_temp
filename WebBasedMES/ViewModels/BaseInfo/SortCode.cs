using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.BaseInfo
{
    public class SortCodeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsUsing { get; set; }
        public string CreateDate { get; set; }
        public string RegisterId { get; set; }
        public string RegisterName { get; set; }
        public string Memo { get; set; }
        public string Uuid { get; set; }
       // public IEnumerable<CommonCode> CommonCodes { get; set; }

        public IEnumerable<CommonCodeResponse> CommonCodes { get; set; }
    }

    public class SortCodeRequest
    {
        public int Id { get; set; }
        public int[] Ids { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }

        public bool AutoCode { get; set; }

        public bool IsUsing { get; set; }
        public string RegisterId { get; set; }
        public string CreateDate { get; set; }
        public string SearchStr { get; set; }
        public string IsUsingStr { get; set; }
    }


}
