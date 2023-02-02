using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.Barcode;

namespace WebBasedMES.Data.Models.BaseInfo
{
    public class Facility
    {
        public int Id { get; set; }
        public CommonCode CommonCode { get; set; } //설비구분.... 
        public int CommonCodeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Standard { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        // public Partner Agent { get; set; }
        public string Agent { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Price { get; set; }
        public string Uid { get; set; }
        public string MaxCurrent { get; set; } = "0";
        public string MaxTon { get; set; } = "0";
        public string Memo { get; set; }
        public bool DailyInspection { get; set; } = true;
        public bool PeriodicalInspection { get; set; } = true;
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
       // public UploadFile UploadFile { get; set; } //설비사진 
       // public int UploadFileId { get; set; }

        public string ImageUrl { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public BarcodeMaster Barcode { get; set; }


    }
}
