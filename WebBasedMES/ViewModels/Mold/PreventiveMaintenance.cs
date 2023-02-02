namespace WebBasedMES.ViewModels.Mold
{
    public class PreventiveMaintenanceFacilityRequest
    {
        public int PreventiveMaintenanceFacilityId { get; set; }
        public int[] PreventiveMaintenanceFacilityIds { get; set; }
        public int FacilityId { get; set; }
        public string IsUsingStr { get; set; }
        public int RegularInspectionPeriod { get; set; }
        

    }
    public class PreventiveMaintenanceFacilityResponse
    {
        public int PreventiveMaintenanceFacilityId { get; set; }
        public int FacilityId { get; set; }
        public string FacilityClassification { get; set; }
        public string FacilityType { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public int IsRegularInspectionPeriod { get; set; }
        public int RegularInspectionPeriod { get; set; }
        public string RegularInspectionPeriodName { get; set; }
        public string FacilityMemo { get; set; }
        public int IsUsing { get; set; }
        public string MoldStatus { get; set; }

        public string LastInspectionDate { get; set; }
        public string NextInspectionDate { get; set; }

    }

    public class PreventiveMaintenanceMoldRequest
    {
        public int PreventiveMaintenanceMoldId { get; set; }
        public int[] PreventiveMaintenanceMoldIds { get; set; }
        public int MoldId { get; set; }
        public string MoldStatusStr { get; set; }
        public int RegularInspectionCount { get; set; }
        public int RegularInspectionPeriod { get; set; }
        public int MoldCleaningCount { get; set; }
        public int MoldCleaningPeriod { get; set; }
        public string IsUsingStr { get; set; }

    }

    public class PreventiveMaintenanceMoldResponse
    {
        public int PreventiveMaintenanceMoldId { get; set; }
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldClassification { get; set; }
        public string MoldType { get; set; }
        public string MoldName { get; set; }
        public int IsRegularInspectionPeriod { get; set; }
        public int RegularInspectionCount { get; set; }
        public int RegularInspectionPeriod { get; set; }
        public string RegularInspectionPeriodName { get; set; }
        public int MoldCleaningCount { get; set; }
        public int MoldCleaningPeriod { get; set; }
        public string MoldCleaningPeriodName { get; set; }
        public string MoldMemo { get; set; }
        public string MoldStatus { get; set; }
        public string LastInspectionDate { get; set; }
        public string NextInspectionDate { get; set; }

        public string LastCleaningDate { get; set; }
        public string NextCleaningDate { get; set; }
        public int MoldCount { get; set; }
        public int CountAfterInspection { get; set; }
        public int CountAfterCleaning { get; set; }

    }



}
