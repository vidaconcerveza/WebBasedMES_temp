using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.Barcode;

namespace WebBasedMES.Data.Models.BaseInfo
{
    public class Defective
    {
        public int Id { get; set; }
        public CommonCode CommonCode { get; set; } 
        public string Code { get; set; }
        public string Name { get; set; } //불량유형
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public ApplicationUser Creator { get; set; } //최초만 하자..
        public DateTime CreateDate { get; set; } //최초만 하래 이것도...
        public string Memo { get; set; }
        public string LOT { get; set; }
        public BarcodeMaster Barcode { get; set; }

    }
}
