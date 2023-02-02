using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.ViewModels.InspectionRepair;

namespace WebBasedMES.ViewModels.BaseInfo
{
    public class FacilityRequest
    {
        public int Id { get; set; }
        public int[] Ids { get; set; }
        public int CommonCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        //설비구분
        public string SearchInput { get; set; }
        public string IsUsingStr { get; set; } //ALL,Y,N
        public string Type { get; set; }
        public string TypeStr { get; set; }

        public string Standard { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Agent { get; set; }
        public string PurchaseDate { get; set; }
        public int Price { get; set; }
        public string MaxCurrent { get; set; }
        public string MaxTon { get; set; }
        public string Memo { get; set; }
        public bool DailyInspection { get; set; }
        public bool PeriodicalInspection { get; set; }
        public string Uid { get; set; }
        public bool IsUsing { get; set; }
        public bool IsDeleted { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public bool AutoCode { get; set; }
    }

    public class FacilityResponse
    {
        public int Id { get; set; }
        public int CommonCode { get; set; }
        public string CommonCodeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        //설비구분
        public string Type { get; set; }
        public string Standard { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Agent { get; set; }
        public string PurchaseDate { get; set; }
        public int Price { get; set; }
        public string MaxCurrent { get; set; }
        public string MaxTon { get; set; }
        public string Memo { get; set; }
        public bool DailyInspection { get; set; } = true;
        public bool PeriodicalInspection { get; set; } = true;
        public string Uid { get; set; }

        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string ImageUrl { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public string Picture { get; set; }

    }

    public class FacilityPopupRequest
    {
        public string FacilitiesClassification { get; set; }
        public string SearchInput { get; set; }
        public string FacilitiesIsUsing { get; set; }
        public string inspectionType { get; set; }
    }

    public class FacilityPopupResponse
    {
        public int FacilityId { get; set; }
        public string FacilitiesCode { get; set; }
        public string FacilitiesClassification { get; set; }
        public string FacilitiesName { get; set; }
        public string FacilitiesStandard { get; set; }
        public string ElectricCurrentMax { get; set; }
        public string TonMax { get; set; }
        public string PurchaseDate { get; set; }
        public string FacilitiesMemo { get; set; }
        public bool FacilitiesIsUsing { get; set; }
        public int PeriodicalInspection { get; set; }

        public IEnumerable<FacilityInspectionItemInterface> FacilityInspectionItems { get; set; }


    }
}
