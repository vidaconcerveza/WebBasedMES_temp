using System.Collections.Generic;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.InspectionRepair
{
    public class RepairAskRequest
    {
        public int RepairAskId { get; set; }
        public int[] RepairAskIds { get; set; }
        public string RepairTarget { get; set; }
        public string RepairAskNo { get; set; }
        public int MoldId { get; set; }
        public int FacilityId { get; set; }
        public string RegisterStartDate { get; set; }
        public string RegisterEndDate { get; set; }
        public int RepairResult { get; set; }
    }

    public class RepairAsksResponse
    {
        public int RepairAskId { get; set; }
        public string RepairAskNo { get; set; }
        public string RegisterDate { get; set; }
        public int FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityType { get; set; }
        public string FacilityName { get; set; }
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldyType { get; set; }
        public string MoldName { get; set; }
        public string IsOutSourcing { get; set; }
        public string RegisterId { get; set; }
        public string RegisterName { get; set; }
        public string RepairAskMemo { get; set; }
        public string RepairResult { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }

    }

    public class RepairAskCreateUpdateRequest
    {
        public int RepairAskId { get; set; }
        public string RegisterDate { get; set; }
        public string RepairTarget { get; set; }
        public int FacilityId { get; set; }
        public int MoldId { get; set; }
        public string IsOutSourcing { get; set; }
        public string RegisterId { get; set; }
        public string RepairAskMemo { get; set; }
    }


    //Repair

    public class RepairLogRequest
    {
        public int RepairAskId { get; set; }
        public int RepairLogId { get; set; }
        public int[] RepairLogIds { get; set; }
        public string RepairTarget { get; set; }
        public string RepairAskNo { get; set; }
        public int MoldId { get; set; }
        public int FacilityId { get; set; }
        public string RegisterStartDate { get; set; }
        public string RegisterEndDate { get; set; }
    }

    public class RepairLogResponse
    {
        public int RepairAskId { get; set; }
        public int RepairLogId { get; set; }
        public string RepairAskNo { get; set; }
        public string RegisterDate { get; set; }
        public int FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityType { get; set; }
        public string FacilityName { get; set; }
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldType { get; set; }
        public string MoldName { get; set; }

        public string IsOutSourcing { get; set; }
        public string RegisterName { get; set; }
        public string RepairAskMemo { get; set; }

        public string RepairFinishDate { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string WorkerName { get; set; }
        public int RepairClassification { get; set; }
        public int RepairType { get; set; }
        public int RepairResult { get; set; }
        public string RepairResultText { get; set; }
        public string CauseOfRepair { get; set; }
        public string CommentOfRepair { get; set; }


        public IEnumerable<UploadFile> UploadFiles { get; set; } 

    }

    public class RepairLogCreateUpdateRequest
    {
        public int RepairLogId { get; set; }
        public int RepairAskId { get; set; }
        public string RepairAskNo { get; set; }
        public string RegisterDate { get; set; }
        public int FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityType { get; set; }
        public string FacilityName { get; set; }
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldyType { get; set; }
        public string MoldName { get; set; }

        public string IsOutSourcing { get; set; }
        public string RegisterName { get; set; }
        public string RepairAskMemo { get; set; }

        public string RepairFinishDate { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string WorkerName { get; set; }
        public int RepairClassification { get; set; }
        public int RepairType { get; set; }
        public int RepairResult { get; set; }

        public string CauseOfRepair { get; set; }
        public string CommentOfRepair { get; set; }

        public IEnumerable<UploadFile> UploadFiles { get; set; }

    }



    public class RepairLogsResponse
    {
        public int RepairAskId { get; set; }
        public string RepairAskNo { get; set; }
        public string RegisterDate { get; set; }
        public int FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityType { get; set; }
        public string FacilityName { get; set; }
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldType { get; set; }
        public string MoldName { get; set; }
        public string IsOutSourcing { get; set; }
        public string RegisterId { get; set; }
        public string RegisterName { get; set; }
        public string RepairAskMemo { get; set; }
        public string RepairResult { get; set; }
        public string RepairType { get; set; }
        public string RepairClassification { get; set; }
        public string CauseOfRepair { get; set; }
        public string CommentOfRepair { get; set; }
    }


}
