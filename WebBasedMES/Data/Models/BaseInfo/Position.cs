using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsUsing { get; set; } = true;
        public string Memo { get; set; }
    }
}
