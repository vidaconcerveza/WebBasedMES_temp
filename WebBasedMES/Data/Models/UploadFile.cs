using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.BaseInfo;

namespace WebBasedMES.Data.Models
{
    public class UploadFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileStamp { get; set; }
        //public bool IsDeleted { get; set; }

        //public Notice Notice { get; set; }
       // public Facility Facility { get; set; }
        //public Mold Mold { get; set; }

       // public Facility Facility { get; set; }
       // public Notice Notice { get; set; }
    }
}
