using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.BaseInfo
{
    public class ItemRequest
    {
        public int Id { get; set; }
        public string CommonCode { get; set; } //Type
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string TaxType { get; set; }
        public string Standard { get; set; }

        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public int OptimumStock { get; set; }
        public bool ImportCheck { get; set; }
        public bool ExportCheck { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public UploadFile UploadFile { get; set; }
    }

    public class ItemResponse
    {
        public int Id { get; set; }
        public int CommonCode { get; set; } //Type
        public string CommonCodeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string TaxType { get; set; }
        public string Standard { get; set; }

        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public int OptimumStock { get; set; }
        public bool ImportCheck { get; set; }
        public bool ExportCheck { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public UploadFile UploadFile { get; set; }
        public string Picture { get; set; }
    }
}
