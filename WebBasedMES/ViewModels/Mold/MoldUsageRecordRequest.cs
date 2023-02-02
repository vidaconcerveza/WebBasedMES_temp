namespace WebBasedMES.ViewModels.Mold
{
    public class MoldUsageRecordRequest
    {
        public int MoldId { get; set; }
        public string MoldStatusStr { get; set; }
        public string WorkStartDate { get; set; }
        public string WorkEndDate { get; set; }
    }

    public class MoldUsageRecordResponse
    {
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public string ProductStandard { get; set; }
        public int ProduceQuantity { get; set; }
        public int ElapsedTime { get; set; }
        public int DownTime { get; set; }

    }

    public class MoldUsageListResponse
    {
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldName { get; set; }
        public string MoldClassification { get; set; }
        public int GuaranteeCount { get; set; }
        public int CurrentCount { get; set; }
        public string CurrentWorkDate { get; set; }
        public string Status { get; set; }
    }
}
