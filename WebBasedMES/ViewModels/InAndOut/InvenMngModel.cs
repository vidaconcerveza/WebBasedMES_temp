using Microsoft.EntityFrameworkCore;

namespace WebBasedMES.ViewModels.InAndOutMng.InAndOut
{

    public class InvenMngModelReq0001
    {
        public string workOrderNo { get; set; }
        public string workOrderStartDate { get; set; }
        public string workOrderEndDate { get; set; }
        public int processId { get; set; }
        public int productId { get; set; }
        public string productLOT { get; set; }
    }

    [Keyless]
    public class InvenMngModelRes0001
    {
        public int? processId { get; set; }
        public int? productId { get; set; }

        public int? WorkerOrderProducePlanId { get; set; }
        public string? productCode { get; set; }
        public string? productClassification { get; set; }
        public string? productName { get; set; }

        public string? productStandard { get; set; }
        public string? productUnit { get; set; }
        public string? workOrderNo { get; set; }
        public string? workOrderDate { get; set; }
        public string? processCode { get; set; }
        public string? processName { get; set; }
        public string? facilitiesCode { get; set; }
        public string? facilitiesName { get; set; }

        public float? requiredQuantity { get; set; }
        public float LOSS { get; set; }
        public int? totalRequiredQuantity { get; set; }
        public int? productionQuantity { get; set; }
        public int? totalInputQuantity { get; set; }
        public string productLOT { get; set; }
        
    }

   
}
