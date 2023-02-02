using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models.BaseInfo
{
    //  [Keyless]
    public class Item
    {
        public int Id { get; set; }
        public CommonCode CommonCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Standard { get; set; }
        public string TaxType { get; set; }
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
}
