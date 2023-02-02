using System;
using System.ComponentModel.DataAnnotations;
using WebBasedMES.Data.Models.BaseInfo;

namespace WebBasedMES.Data.Models
{
    public class FacilityBaseInfo
    {
        [Key]
        public int FacilityBaseInfoId { get; set; }
        public Facility Facility { get; set; }
        public WebBasedMES.Data.Models.Mold.Mold  Mold { get; set; }

        public double BottomDeadPoint { get; set; }
        public double Slide { get; set; }
        public int TON { get; set; }
        public int MoldPositionSensor { get; set; }
        public ApplicationUser Register { get; set; }
        public DateTime RegisterDate { get; set; }
        //public DateTime UpdateDate { get; set; }
        public int IsDeleted { get; set; }
        public string Memo { get; set; }
       // public FacilityControl FacilityControl { get; set; }

    }

    public class FacilityControl
    {
        [Key]
        public int FacilityControlId { get; set; }
        public ApplicationUser Register { get; set; }
        public DateTime RegisterDate { get; set; }
        public double BottomDeadPointUL { get; set; }
        public double BottomDeadPointDL { get; set; }
        public double SlideUL { get; set; }
        public double SlideDL { get; set; }
        public int TonUL { get; set; }
        public int TonDL { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Facility Facility { get; set; }
        public WebBasedMES.Data.Models.Mold.Mold Mold { get; set; }

    }
}
