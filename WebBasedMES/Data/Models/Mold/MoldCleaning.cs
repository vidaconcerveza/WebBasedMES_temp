using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.BaseInfo;

namespace WebBasedMES.Data.Models.Mold
{
    public class MoldCleaning
    {
        public int Id { get; set; }
        public Mold Mold { get; set; }
        public DateTime CleaningDate { get; set; }
        public string Memo { get; set; }
        public bool IsDeleted { get; set; }
        public int CleaningClassification { get; set; }
        public int CleaningType { get; set; }
        public int GuaranteeCount { get; set; }
        public int StartCount { get; set; }
        public int CurrentCount { get; set; }
        public ApplicationUser Worker { get; set; }

    }

}
