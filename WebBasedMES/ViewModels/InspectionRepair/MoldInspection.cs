using System.Collections.Generic;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.InspectionRepair
{
    public class MoldInspectionsResponse
    {
        public int MoldInspectionId { get; set; }
        public string RegisterDate { get; set; }
        public string MoldCode { get; set; }
        public string MoldType { get; set; }
        public string MoldName { get; set; }
        public string RegisterName { get; set; }
        public string InspectionContents { get; set; }
        public string MoldMemo { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }

    public class MoldInspectionResponse
    {
        public int MoldInspectionId { get; set; }
        public string RegisterDate { get; set; }
        public string MoldCode { get; set; }
        public string MoldType { get; set; }
        public string MoldName { get; set; }
        public string RegisterName { get; set; }
        public string RegisterId { get; set; }
        public string InspectionContents { get; set; }
        public string MoldPartnerName { get; set; }
        public int MoldWeight { get; set; }
        public string MoldStandard { get; set; }
        public string MoldMaterial { get; set; }
        public string MoldMemo { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }

        public IEnumerable<MoldInspectionItemInterface> MoldInspectionItems { get; set; }

    }
    public class MoldInspectionRequest
    {
        public int MoldInspectionId { get; set; }
        public int[] MoldInspectionIds { get; set; }
        public string RegisterStartDate { get; set; }
        public string RegisterEndDate { get; set; }
        public int MoldId { get; set; }
        public string InspectionType { get; set; }
    }


    //Create/update
    public class MoldInspectionCreateUpdateRequest
    {
        public int MoldInspectionId { get; set; }
        public string RegisterDate { get; set; }
        public string RegisterId { get; set; }
        public int MoldId { get; set; }
        public string InspectionContents { get; set; }
        public string InspectionType { get; set; }

        public IEnumerable<MoldInspectionItemInterface> MoldInspectionItems { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }

    }

    public class MoldInspectionItemInterface
    {
        public int MoldInspectionItemId { get; set; }
        public int MoldInspectionId { get; set; }
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
