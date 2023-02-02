using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.Mold
{
    public class MoldCleaningRequest
    {
        public int MoldId { get; set; }

        public IEnumerable<MoldCleaningListRequest> MoldCleaningList { get; set; }
    }

    public class MoldCleaningResponse
    {
        public int MoldId { get; set; }
        public int MoldCleaningId { get; set; }
        public int CleaningClassification { get; set; }
        public string CleaningClassificationName { get; set; }
        public int CleaningType { get; set; }
        public string CleaningTypeName { get; set; }
        public int GauranteeCount { get; set; }
        public int StartCount { get; set; }
        public int CurrentCount { get; set; }
        public string WorkerId { get; set; }
        public string WorkerName { get; set; }
        public string Memo { get; set; }
        public string CleaningDate { get; set; }
    }

    public class MoldCleaningListRequest
    {
        public int MoldId { get; set; }
        public int MoldCleaningId { get; set; }
        public int CleaningClassification { get; set; }
        public int CleaningType { get; set; }
        public int GuaranteeCount { get; set; }
        public int StartCount { get; set; }
        public int CurrentCount { get; set; }
        public string WorkerId { get; set; }
        public string WorkerName { get; set; }
        public string Memo { get; set; }
        public string CleaningDate { get; set; }
    }


}
