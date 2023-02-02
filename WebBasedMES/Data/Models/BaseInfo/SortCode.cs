using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models
{
    public class SortCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public ApplicationUser Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<CommonCode> CommonCodes { get; set; }
    }
}
