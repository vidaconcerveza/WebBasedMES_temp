using System.Collections.Generic;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.InspectionRepair
{
    public class FacilityInspectionsResponse
    {
        public int FacilityInspectionId { get; set; }
        public string RegisterDate { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityType { get; set; }
        public string FacilityName { get; set; }
        public string RegisterName { get; set; }
        public string InspectionContents { get; set; }
        public string FacilityMemo { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }

    public class FacilityInspectionResponse
    {
        public int FacilityInspectionId { get; set; }
        public string RegisterDate { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityType { get; set; }
        public string FacilityName { get; set; }
        public string FacilityMemo { get; set; }

        public string RegisterName { get; set; }
        public string RegisterId { get; set; }
        public string InspectionContents { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }

        public IEnumerable<FacilityInspectionItemInterface> FacilityInspectionItems { get; set; }

    }
    public class FacilityInspectionRequest
    {
        public int FacilityInspectionId { get; set; }
        public int[] FacilityInspectionIds { get; set; }
        public string RegisterStartDate { get; set; }
        public string RegisterEndDate { get; set; }
        public int FacilityId { get; set; }
        public string InspectionType { get; set; }
    }


    //Create/update
    public class FacilityInspectionCreateUpdateRequest
    {
        public int FacilityInspectionId { get; set; }
        public string RegisterDate { get; set; }
        public string RegisterId { get; set; }
        public int FacilityId { get; set; }
        public string InspectionContents { get; set; }
        public string InspectionType { get; set; }
        public string FacilityMemo { get; set; }

        public IEnumerable<FacilityInspectionItemInterface> FacilityInspectionItems { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }

    }

    public class FacilityInspectionItemInterface
    {
        public int FacilityInspectionItemId { get; set; }
        public int FacilityInspectionId { get; set; }
        public int InspectionItemId { get; set; }
        public string InspectionCode { get; set; }
        public string InsepctionName { get; set; }
        public string InspectionPeriod { get; set; }
        public int InspectionCountCriteria { get; set; }
        public string InspectionItem { get; set; }
        public string InspectionJudgement { get; set; }
        public string InspectionMethod { get; set; }
        public string InspectionResult { get; set; }
        public string CauseOfError { get; set; }
        public string ErrorManagementResult { get; set; }
       
    }

}
