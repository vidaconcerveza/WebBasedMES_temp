using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ApplicationUser Creator { get; set; }
        public DateTime CreateOn { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }


}
