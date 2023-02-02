using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.SystemManage
{
    public class NoticeManageRequest
    {
        public int Id { get; set; }
        public int[] Ids { get; set; }
        public string Uuid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IEnumerable<UploadFile> uploadFiles { get; set; }

        public string SearchStr { get; set; }
        public string TypeStr { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class NoticeManageResponse
    {
        public int Id { get; set; }
        public string Creator { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreateOn { get; set; }
        public IEnumerable<UploadFile> uploadFiles { get; set; }
    }

}
