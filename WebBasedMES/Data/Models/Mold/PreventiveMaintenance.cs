using System.ComponentModel.DataAnnotations;
using WebBasedMES.Data.Models.BaseInfo;

namespace WebBasedMES.Data.Models.Mold
{
    public class PreventiveMaintenanceFacility
    {
        [Key]
        public int PreventiveMaintenanceFacilityId { get; set; }
        public Facility Facility { get; set; }
        public int RegularInspectionPeriod { get; set; }
        public int IsDeleted { get; set; } = 0;

    }

    public class PreventiveMaintenanceMold
    {
        [Key]
        public int PreventiveMaintenanceMoldId { get; set; }
        public Mold Mold { get; set; }
        public int RegularInspectionCount { get; set; }
        public int RegularInspectionPeriod { get; set; }
        public int MoldCleaningCount { get; set; }
        public int MoldCleaningPeriod { get; set; }
        public int IsDeleted { get; set; } = 0;

    }
}
