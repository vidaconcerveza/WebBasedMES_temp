using System;
using System.ComponentModel.DataAnnotations;
using WebBasedMES.Data.Models.Lots;

namespace WebBasedMES.Data.Models.Barcode
{
    public class BarcodeMaster
    {
        [Key]
        public int BarcodeMasterId { get; set; }
        public DateTime BarcodeIssueDate { get; set; }
        public int BarcodeIssueCount { get; set; } = 0;
        public int BarcodeReIssueCount { get; set; }= 0;
    }
}
