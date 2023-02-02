using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.Barcode;
using WebBasedMES.Data.Models.BaseInfo;



namespace WebBasedMES.Data.Models.Mold
{
    public class Mold
    {
        public int Id { get; set; }
        public CommonCode CommonCode { get; set; } //금형구분 
        public string Code { get; set; }
        public string Name { get; set; }
        public string Standard { get; set; }
        public string Material { get; set; } //재질
        public int Weight { get; set; }
        public int Price { get; set; }
        public DateTime MoldCreateDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public Partner Owener { get; set; }
        public ApplicationUser Registerer { get; set; }

        public int GuranteeCount { get; set; } = 0;
        public int StartCount { get; set; } = 0;
        public int CurrentCount { get; set; } = 0; //카운터로 계산하기. 
        
        public CommonCode CleaningCycle { get; set; }

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
        public DateTime DiscardDate { get; set; }
        public ApplicationUser DiscardRegisterer { get; set; }
        public string DiscardReason { get; set; }

        public IEnumerable<MoldCleaning> MoldCleanings { get; set; }
        public IEnumerable<MoldDraft> MoldDrafts { get; set; }

        public MoldLocation MoldLocation { get; set; }

        public BarcodeMaster Barcode { get; set; }


    }
}
