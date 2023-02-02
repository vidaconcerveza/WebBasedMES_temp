using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models
{
    public class BaseInfoCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;
    }
}
