using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.Barcode;

namespace WebBasedMES.Data.Models.BaseInfo
{
    public class Partner
    {
        public int Id { get; set; }
        public CommonCode CommonCode { get; set; }
        public string PartnerType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string President { get; set; }
        public string BusinessNumber { get; set; }
        public string PresidentNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Address { get; set; }
        public string BusinessType { get; set; }
        public string BusinessClass { get; set; }
        public string Memo { get; set; }
        public bool Group_Buy { get; set; }
        public bool Group_Sell { get; set; }
        public bool Group_Finance { get; set; }
        public bool Group_Etc { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string TaxInfo { get; set; } = "과세";
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public BarcodeMaster Barcode { get; set; }


    }
}
