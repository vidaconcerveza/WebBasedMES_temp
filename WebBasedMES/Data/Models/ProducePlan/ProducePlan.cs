using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Data.Models.Lots;

namespace WebBasedMES.Data.Models.ProducePlan
{
    public class ProducePlan
    {
        [Key]
        public int ProducePlanId { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public ApplicationUser Register { get; set; }
        public string ProductionPlanNo { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime ProductionPlanStartDate { get; set; }
        public DateTime ProductionPlanEndDate { get; set; }
        public string ProductionPlanStatus { get; set; } = "계획등록";
        public string ProductionPlanMemo { get; set; }
        public int IsDeleted { get; set; } = 0;

        public IEnumerable<ProducePlansProduct> ProducePlanProducts { get; set; }
    }

    //https://docs.microsoft.com/ko-kr/aspnet/core/data/ef-mvc/complex-data-model?view=aspnetcore-6.0 오류난부분 이걸로 참조.
    public class ProducePlansProduct
    {        
        [Key]        
        public int ProducePlansProductId { get; set; } //ProducePlansProduct 와 ProducePlansProductId 이름이 같아야 오류가 안남...
        public ProducePlan ProducePlan { get; set; }
        public int ProducePlanId { get; set; }
        public Product Product { get; set; }        
        public OrderProduct OrderProduct { get; set; }
        public int Priority { get; set; }
        public DateTime ProductionPlanDate { get; set; }
        public int ProductPlanQuantity { get; set; }
        public int IsDeleted { get; set; }
        public IEnumerable<ProducePlansProcess> ProducePlansProcesses { get; set; }
        public IEnumerable<WorkerOrder> WorkerOrders { get; set; }
    }

    public class ProducePlansProcess
    {
        [Key]
        public int ProducePlansProcessId { get; set; }
        //[ForeignKey("ProducePlansProductId")] //이부분만 왜 이걸 넣어야하는지 모르겠음. 다른방법 있는지 확인해볼 것. >> 2022-06-05 해결함 class명과 PK 명의 명명규칙 같이게할 것. 클래스네임Id
        public ProducePlansProduct ProducePlansProduct { get; set; }
        public int ProducePlansProductId { get; set; } 
     //   [NotMapped]
        public ProductProcess ProductProcess { get; set; }
        public int ProductProcessId { get; set; }
        public int ProcessPlanQuantity { get; set; }
        public int? IsDeleted { get; set; }

        public IEnumerable<WorkerOrderProducePlan> WorkerOrderProducePlans { get; set; }
    }

    public class WorkerOrder
    {
        [Key]
        public int WorkerOrderId { get; set; }        
        public ProducePlansProduct ProducePlansProduct { get; set; }
      //  [NotMapped]
        public Product Product { get; set; }
        public string WorkOrderNo { get; set; }
        public DateTime WorkOrderDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public ApplicationUser Register { get; set; }
        public int ProductWorkQuantity { get; set; }
        public int WorkOrderSequence { get; set; }
        public string? WorkOrderMemo { get; set; } //현재 DB에 없음 2022-06-05 기준
        public int IsDeleted { get; set; }
        public IEnumerable<WorkerOrderProducePlan> WorkerOrderProducePlans { get; set; }
    }

    public class WorkerOrderProducePlan
    {
        [Key]
        public int WorkerOrderProducePlanId { get; set; }
        public WorkerOrder WorkerOrder { get; set; }
        public ProducePlansProcess ProducePlansProcess { get; set; }

        // [NotMapped]
        public Product Product { get; set; }
        public Partner Partner { get; set; }
        public Facility Facility { get; set; }
        //public int ProductProcessId { get; set; }
        public WebBasedMES.Data.Models.Mold.Mold Mold { get; set; }
        public ApplicationUser Register { get; set; }
        public int InOutSourcing { get; set; }
        public int ProcessWorkQuantity { get; set; } = 0;
        public int ProcessElapsedTime { get; set; } = 0;
        public int ProcessCheck { get; set; } = 0;
        public int ProcessCheckResult { get; set; } = 3;
        public string WorkProcessMemo { get; set; } 
        public int IsDeleted { get; set; }
        public ProcessProgress ProcessProgress { get; set; }

    }

    public class WorkerOrderWithoutPlan
    {
        [Key]
        public int WorkOrderWithoutPlanId { get; set; }
        public ProductProcess ProductProcess { get; set; }
        public WorkerOrderProducePlan WorkerOrderProducePlan { get;set;}
    }

    [Keyless]
    public class VW_KPI
    {
        public DateTime WorkEndDateTime { get; set; }
        public Product Product { get; set; }
        public int ProductionQuantity { get; set; }
        public ProcessProgress ProcessProgress { get; set; }
        public int ElapseTime { get; set; }
        public int DefectiveQuantity { get; set; }
        public int DownTime { get; set; } 
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
