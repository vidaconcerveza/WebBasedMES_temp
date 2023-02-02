using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.Barcode;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Data.Models.ProducePlan;

namespace WebBasedMES.Data.Models.Lots
{

    [Table("ProcessProgress")]
    public class ProcessProgress
    {
        [Key]
        public int ProcessProgressId { get; set; }
        public int WorkerOrderProducePlanId { get; set; } //2022-06-05 by mg. 추가 DB에 있는 컬럼명 그대로 가져옴
        public WorkerOrderProducePlan WorkOrderProducePlan { get; set; }

        public int ProductionQuantity { get; set; } = 0;
        public string WorkStatus { get; set; } = "작업대기";
        public DateTime WorkStartDateTime { get; set; }
        public DateTime WorkEndDateTime { get; set; }
        public int IsDeleted { get; set; }
        public IEnumerable<ProcessNotWork> ProcessNotWorks { get; set; }
        public BarcodeMaster Barcode { get; set; }
    }
    [Table("Lots")]
    public class LotEntity
    {
        [Key]
        public int LotId { get; set; }
        public ProcessProgress ProcessProgress { get; set; } 
        //public ProcessProgress Partner { get; set; } //공정진행현황 ProcessProgress
        public string LotName { get; set; }
        public string ProcessType { get; set; }
        public int IsDeleted { get; set; }
        public IEnumerable<LotCount> LotCounts { get; set; }
    }

    //수주 && 제품
    public class LotCount
    {
        [Key]
        public int LotCountId { get; set; }
        //[NotMapped]
        public Product Product { get; set; } //품목-제품으로 취급
        public LotEntity Lot { get; set; }
        public int LotId { get; set; }
        public int StoreOutCount { get; set; }// 입고수량
        public int OutOrderCount { get; set; }// 출고 수량
        public int ConsumeCount { get; set; }// 투입 수량
        public int ProduceCount { get; set; }// 생산 수량
        public int ModifyCount { get; set; }
        public int DefectiveCount { get; set; }

        public int IsDeleted { get; set; }
        

    }

}
