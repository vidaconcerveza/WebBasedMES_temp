using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBasedMES.Data.Models.BaseInfo;

namespace WebBasedMES.Data.Models.InspectionRepair
{
    public class RepairAsk
    {
        [Key]
        public int RepairAskId { get; set; }
        public string RepairAskNo { get; set; }
        public DateTime RegisterDate { get; set; }
        public ApplicationUser Register { get; set; }
        
        //MOLD or Facility    
        public string RepairTarget { get; set; }
        public Facility Facility { get; set; }
        public WebBasedMES.Data.Models.Mold.Mold Mold { get; set; }

        public string RepairAskMemo { get; set; }
        public string RepairResult { get; set; }
        public int IsDeleted { get; set; }
        public string IsOutSourcing { get; set; }
        public RepairLog RepairLog { get; set; }
       // public RepairLog RepairLog { get; set; }
    }

    public class RepairLog
    {
        [Key]
        public int RepairLogId { get; set; }
        //public RepairAsk RepairAsk { get; set; }
        public DateTime RepairFinishDate { get; set; }
        public Partner Partner { get; set; }
        //CommonCode 참고...
        public int RepairClassification { get; set; }
        public int RepairType { get; set; }
        public int RepairResult { get; set; }


        public string WorkerName { get; set; }
        public string CauseOfRepair { get; set; }
        public string CommentOfRepair { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }

    }
}
