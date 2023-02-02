using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.Mold
{
    public class MoldDraftRequest
    {
        public int MoldId { get; set; }
        public IEnumerable<MoldDraftListRequest> MoldDraftList { get; set; }
    }

    public class MoldDraftResponse
    {
        public int MoldId { get; set; }
        public int MoldDraftId { get; set; }
        public string DraftClassification { get; set; }
        public string RegisterId { get; set; }
        public string RegisterName { get; set; }
        public string Memo { get; set; }
        public string RegisterDate { get; set; }
        public UploadFile UploadFile { get; set; }
    }

    public class MoldDraftListRequest
    {
        public int MoldId { get; set; }
        public int MoldDraftId { get; set; }
        public string DraftClassification { get; set; }
        public string RegisterId { get; set; }
        public string RegisterName { get; set; }
        public string Memo { get; set; }
        public string RegisterDate { get; set; }
        public UploadFile UploadFile { get; set; }

    }
}
