using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBasedMES.Data.Models.BaseInfo;


namespace WebBasedMES.Data.Models.InspectionRepair
{
    public class FacilityInspection
    {
        [Key]
        public int FacilityInspectionId { get; set; }
        public DateTime RegisterDate { get; set; }
        public ApplicationUser Register { get; set; }
        public Facility Facility { get; set; }
        public string Contents { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public IEnumerable<FacilityInspectionItem> FacilityInspectionItems { get; set; }
        public int IsDeleted { get; set; }
        public string Type { get; set; }

        public string Memo { get; set; }

    }

    public class FacilityInspectionItem
    {
        [Key]
        public int FacilityInspectionItemId { get; set; }
        public FacilityInspection FacilityInspection { get; set; }
        public InspectionItem InspectionItem { get; set; }
        public string InspectionResult { get; set; }
        public string CauseOfError { get; set; }
        public string ErrorManagementResult { get; set; }
    }


    public class MoldInspection
    {
        [Key]
        public int MoldInspectionId { get; set; }
        public DateTime RegisterDate { get; set; }
        public ApplicationUser Register { get; set; }
        public WebBasedMES.Data.Models.Mold.Mold Mold { get; set; }
        public string Contents { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public IEnumerable<MoldInspectionItem> MoldInspectionItems { get; set; }
        public int IsDeleted { get; set; }
        public string Type { get; set; }
        public string Memo { get; set; }

    }

    public class MoldInspectionItem
    {
        [Key]
        public int MoldInspectionItemId { get; set; }
        public MoldInspection MoldInspection { get; set; }
        public InspectionItem InspectionItem { get; set; }
        public string InspectionResult { get; set; }
        public string CauseOfError { get; set; }
        public string ErrorManagementResult { get; set; }
    }

}