using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.Barcode;
using WebBasedMES.Data.Models.Lots;

namespace WebBasedMES.Data.Models.BaseInfo
{
    public class Process
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public CommonCode CommonCode { get; set; }
        public int CommonCodeId { get; set; }
        public string Name { get; set; }
        public bool FacilityUse { get; set; }
        public bool ProcessInspection { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
        //public Product Product { get; set; }
        public IEnumerable<ProcessDownTimeType> ProcessDownTimeTypes { get; set; }
        public IEnumerable<ProcessFacility> ProcessFacilitys { get; set; }
        public IEnumerable<ProcessDefective> ProcessDefectives { get; set; }
        public IEnumerable<ProcessDefective_Master> ProcessDefectives_Master { get; set; }
        public bool IsDeleted { get; set; }
        public BarcodeMaster Barcode { get; set; }

    }

    public class ProcessDownTimeType
    {
        public int Id { get; set; }
        public CommonCode DownTime { get; set; }
        public int DownTimeId { get; set; }
        public bool IsUsing { get; set; }
        public Process Process { get; set; }
        public int ProcessId { get; set; }
        public string Memo { get; set; }
    }

    public class ProcessFacility
    {
        public int Id { get; set; }
        public Facility Facility { get; set; }
        public int FacilityId { get; set; }
        public bool IsUsing { get; set; }
        public Process Process { get; set; }
        public int ProcessId { get; set; }
        public string Memo { get; set; }

    }

    public class ProcessDefective_Master
    {
        public int Id { get; set; }
        public Process Process { get; set; }
        public int ProcessId { get; set; }
        public bool IsUsing { get; set; }
        public Defective Defective { get; set; }
        public int DefectiveId { get; set; }
        public string Memo { get; set; }
    }


    public class ProcessDefective
    {
        [Key]
        public int ProcessDefectiveId { get; set; }
        public int DefectiveCount { get; set; }
        public ProcessProgress ProcessProgress { get; set; }
        public int IsDeleted { get; set; }
        public Defective Defective { get; set; }
        public string DefectiveProductMemo { get; set; }
        public string LotName { get; set; }
        //public bool IsUsing { get; set; }
        public LotEntity Lot { get; set; }

    }

    public class ProcessNotWork
    {
        [Key]
        public int ProcessNotWorkId { get; set; }
        public ProcessProgress ProcessProgress { get; set; }
        public int ProcessProgressId { get; set; }
        public int IsDeleted { get; set; }
        public int ShutdownCodeId { get; set; }
        public DateTime ShutdownStartDateTime { get; set; }
        public DateTime ShutdownEndDateTime { get; set; }
        public string ProcessShutdownMemo { get; set; }
    }
}
