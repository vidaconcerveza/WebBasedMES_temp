using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models.Mold
{
    public class MoldDraft
    {
        public int Id { get; set; }
        public Mold Mold { get; set; }
        public DateTime RegisterDate { get; set; }

        public ApplicationUser Register { get; set; }
        public UploadFile UploadFile { get; set; }
        public string DraftClassification { get; set; }
        public string Memo { get; set; }
        public bool IsDeleted { get; set; }
    }

}
