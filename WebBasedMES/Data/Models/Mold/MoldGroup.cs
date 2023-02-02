using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.BaseInfo;

namespace WebBasedMES.Data.Models.Mold
{
    public class MoldGroup
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsAuto { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<MoldGroupElement> MoldGroupList { get; set; }

        public string Flag { get; set; }
    }

    public class MoldGroupElement
    {
        public int Id { get; set; }
        public MoldGroup MoldGroup { get; set; }
        public Facility Facility { get; set; }
        public Mold Mold { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
    }


}
