using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;
using WebBasedMES.Data.Models.BaseInfo;

namespace WebBasedMES.ViewModels.BaseInfo
{
    public class ProcessResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CommonCodeName { get; set; }
        public int CommonCode { get; set; }
        public string Name { get; set; }
        public bool FacilityUse { get; set; }
        public bool ProcessInspection { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
        public IEnumerable<ProcessDownTimeInterface> ProcessDownTimeTypes { get; set; }
        public IEnumerable<ProcessFacilityInterface> ProcessFacilitys { get; set; }
        public IEnumerable<ProcessDefectiveInterface> ProcessDefectives { get; set; }
    }

    public class ProcessDownTimeInterface
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
        public bool Checked { get; set; }
    }

    public class ProcessFacilityInterface
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
        public bool Checked { get; set; }

    }

    public class ProcessDefectiveInterface
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
        public bool Checked { get; set; }

    }

    public class ProcessRequest
    {
        public int Id { get; set; }
        public int[] Ids { get; set; }
        public string Code { get; set; }
        public int CommonCode { get; set; }
        public string Name { get; set; }
        public bool FacilityUse { get; set; }
        public bool ProcessInspection { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }
        public IEnumerable<ProcessDownTimeInterface> ProcessDownTimeTypes { get; set; }
        public IEnumerable<ProcessFacilityInterface> ProcessFacilitys { get; set; }
        public IEnumerable<ProcessDefectiveInterface> ProcessDefectives { get; set; }

        public string TypeStr { get; set; }
        public string SearchStr { get; set; }
        public string IsUsingStr { get; set; }
        public bool AutoCode { get; set; }
    }


    public class ProcessPopupRequest
    {
        public string SearchInput { get; set; }
        public string ProcessIsUsing { get; set; }
    }

    public class ProcessPopupResponse
    {
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessClassification { get; set; }
        public string ProcessName { get; set; }
        public bool ProcessIsFacilities { get; set; }
        public bool ProcessCheck { get; set; }
        public string ProcessMemo { get; set; }
        public bool ProcessIsUsing { get; set; }
    }
}
