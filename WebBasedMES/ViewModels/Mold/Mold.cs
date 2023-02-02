using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels.InspectionRepair;

namespace WebBasedMES.ViewModels.Mold
{
    public class MoldRequest
    {
        public int MoldId { get; set; }
        public int[] MoldIds { get; set; }
        public int CommonCode { get; set; } //금형구분
        public string Code { get; set; }
        public string Name { get; set; }
        public string Standard { get; set; }
        public string Material { get; set; } //재질
        public int Weight { get; set; }
        public int Price { get; set; }
        public string MoldCreateDate { get; set; }
        public string RegisterDate { get; set; }
        public string Owener { get; set; }

        public int PartnerId { get; set; }
        public string RegisterId { get; set; }

        public int GuranteeCount { get; set; } = 0;
        public int StartCount { get; set; } = 0;
        public int CurrentCount { get; set; } = 0; //카운터로 계산하기. 

        public int CleaningCycle { get; set; }

        public string Status { get; set; } = "사용"; //비사용,수리,폐기,기타
        public string Memo { get; set; }
        public bool IsDeleted { get; set; }

        public UploadFile UploadFile { get; set; } //설비사진
        public IEnumerable<UploadFile> UploadFiles { get; set; }

        //금형 점검...
        public bool DailyInspection { get; set; } = true;
        public int DailyInspectionThreshold { get; set; }
        public bool RegularInspection { get; set; } = true;
        public int RegularInspectionThreshold { get; set; }

        //금형폐기
        public string DiscardDate { get; set; }
        public string DiscardRegistererName { get; set; }
        public string DiscardRegisterer { get; set; }
        public string DiscardReason { get; set; }

        public bool AutoCode { get; set; }


    }

    public class MoldResponse
    {
        public int Id { get; set; }
        public int MoldId { get; set; }
        public string CommonCode { get; set; } //금형구분 
        public string CommonCodeName { get; set; }
        public string Code { get; set; }
        public string MoldCode { get; set; }
        public string Name { get; set; }
        public string MoldName { get; set; }
        public string Standard { get; set; }
        public string Material { get; set; } //재질
        public int Weight { get; set; }
        public int Price { get; set; }
        public string MoldCreateDate { get; set; }
        public string RegisterDate { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public int PartnerId { get; set; }
        public string RegisterId { get; set; }
        public string RegisterName { get; set; }

        public int GuranteeCount { get; set; } = 0;
        public int StartCount { get; set; } = 0;
        public int CurrentCount { get; set; } = 0; //카운터로 계산하기. 

        public int CleaningCycle { get; set; }
        public string CleaningCycleName { get; set; }

        public string Status { get; set; } = "사용"; //비사용,수리,폐기,기타
        public string Memo { get; set; }
        public bool IsDeleted { get; set; }

        public UploadFile UploadFile { get; set; } //설비사진
        public string Picture { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
            
        //금형 점검...
        public bool DailyInspection { get; set; } = true;
        public int DailyInspectionThreshold { get; set; }
        public bool RegularInspection { get; set; } = true;
        public int RegularInspectionThreshold { get; set; }

        //금형폐기
        public string DiscardDate { get; set; }
        public string DiscardRegistererName { get; set; }
        public string DiscardRegisterer { get; set; }
        public string DescardReason { get; set; }

        public string CurrentCleaningDate { get; set; }


        public string MoldLocationArea { get; set; }
        public string MoldLocationRow { get; set; }
        public string MoldLocationColumn { get; set; }
        public string MoldLocationComment { get; set; }
        public int MoldLocationIsUsing { get; set; }

        public string MoldDraftRegisterDate { get; set; }
        public string MoldDraftEditedDate { get; set; }

    }

    public class MoldRequest002
    {
        public string SearchInput { get; set; }
        public string MoldStatus { get; set; }
        public string inspectionType { get; set; }

        public string IsUsingStr { get; set; }
        public string SearchMoldPositionInput { get; set; }
        public int MoldId { get; set; }
    }

    public class MoldResponse002
    {
        public int MoldId { get; set; }
        public string MoldCode { get; set; }
        public string MoldClassification { get; set; }
        public string MoldName { get; set; }
        public int MoldGuaranteeCount { get; set; }
        public int MoldCurrentCount { get; set; }
        public int MoldStartCount { get; set; }
        public string RegisterDate { get; set; }
        public string MoldDiscardDate { get; set; }
        public string PartnerName { get; set; }
        public string MoldMemo { get; set; }
        public string MoldStatus { get; set; }
        public string MoldStandard { get; set; }
        public int MoldWeight { get; set; }
        public string MoldMaterial { get; set; }

        public IEnumerable<MoldInspectionItemInterface> MoldInspectionItems { get; set; }

    }

}
