using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models
{
    public class CommonCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public DateTime CreateDate { get; set; }
        public ApplicationUser Creator { get; set; }
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public SortCode SortCode { get; set; }
        
    }
}
