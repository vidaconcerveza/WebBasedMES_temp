using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.ViewModels.BaseInfo
{
    public class CommonCodeRequest
    {
        public int Id { get; set; }
        public string SortCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Uuid { get; set; }
        public string CreateDate { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
        public bool AutoCode { get; set; }

        public string IsUsingStr { get; set; }
        public string SearchStr { get; set; }

    }
    public class CommonCodeResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string CreateDate { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
        public bool AutoCode { get; set; }
        public string SortCode { get; set; }
        public string SortCodeName { get; set; }
        public string RegisterName { get; set; }
    }
}
