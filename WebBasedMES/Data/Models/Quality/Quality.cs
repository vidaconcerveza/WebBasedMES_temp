using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Data.Models.Lots;

namespace WebBasedMES.Data.Models
{
    public class EtcDefective
    {
        [Key]
        public int EtcDefectiveId { get; set; }
        // [NotMapped]
        public Product Product { get; set; }
        public Defective Defective { get; set; } //불량유형
                                                 //  public string LotName { get; set; }
        public DateTime DefectiveDate { get; set; }
        public int IsDeleted { get; set; } //활성,비활성
        public string EtcDefectiveMemo { get; set; }
        public IEnumerable<EtcDefectivesDetail> EtcDefectivesDetails { get; set; }
    }

    public class EtcDefectivesDetail
    {
        [Key]
        public int EtcDefectiveDetailId { get; set; }
        public EtcDefective EtcDefective { get; set; }
        public int EtcDefectiveId { get; set; }
        public LotEntity Lot { get; set; }
        public CommonCode ProductStandardUnit { get; set; }
        public int ProductStandardUnitCount { get; set; }
        public int DefectiveQuantity { get; set; } //불량수량

        public int IsDeleted { get; set; } //활성,비활성


    }


    public class VoltageInspection
    {
        [Key]
        public int VoltageInspectionId { get; set; }
        public DateTime InspectionDate { get; set; }
        public Product Product { get; set; }
        public string Lot { get; set; }
        public string Memo { get; set; }
        public string Ton { get; set; }
        public string MainMotorAmp { get; set; }
        public string MainMotorOverAmp { get; set; }
        public string SlideAmp { get; set; }
        public string SlideMotorOverAmp { get; set; }
        public string MotorSpm { get; set; }
        public string PressMaxSpm { get; set; }
        public string Slp { get; set; }
        public string LoadCellTon { get; set; }
        public string A1Speed { get; set; }
        public string A1Slp { get; set; }
        public string A2Speed { get; set; }
        public string A2Slp { get; set; }
        public string A3Speed { get; set; }
        public string A3Slp { get; set; }
        public string A4Speed { get; set; }
        public string A4Slp { get; set; }
        public string A5Speed { get; set; }
        public string A5Slp { get; set; }
        public bool IsDeleted { get; set; }
    }

}
