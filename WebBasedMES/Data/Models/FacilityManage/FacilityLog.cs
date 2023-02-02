using System;
using System.ComponentModel.DataAnnotations;

namespace WebBasedMES.Data.Models.FacilityManage
{
    public class FacilityStatus
    {
        [Key]
        public int Id { get; set; }
        public string UID { get; set; }
        public DateTime UpdateTime { get; set; }
        public string CW010 { get; set; }
        public string CW012 { get; set; }
        public string CW030 { get; set; }
        public string DW500 { get; set; }
        public string DW501 { get; set; }
        public string DW502 { get; set; }
        public string DW503 { get; set; }
        public string DW504 { get; set; }
        public string DW2921 { get; set; }
        public string DW4000 { get; set; }
        public string DW4020 { get; set; }
        public string FW004 { get; set; }
        public string FW050 { get; set; }
        public string LW056 { get; set; }
        public string LW058 { get; set; }
        public string LW059 { get; set; }
        public string LW060 { get; set; }
        public string LW000 { get; set; }
        public string DW4101 { get; set; }
        public string DW4102 { get; set; }
        public string MW900 { get; set; }
        public string MW000 { get; set; }
        public string MW090 { get; set; }
        public string PW000 { get; set; }
        public string PW010 { get; set; }
        public string PW020 { get; set; }
        public string PW030 { get; set; }
        public string DW0020 { get; set; }
    }

    public class FacilityStatusLog
    {
        [Key]
        public int Id { get; set; }
        public string UID { get; set; }
        public DateTime CreateTime { get; set; }
        public string CW010 { get; set; }
        public string CW012 { get; set; }
        public string CW030 { get; set; }
        public string DW500 { get; set; }
        public string DW501 { get; set; }
        public string DW502 { get; set; }
        public string DW503 { get; set; }
        public string DW504 { get; set; }
        public string DW2921 { get; set; }
        public string DW4000 { get; set; }
        public string DW4020 { get; set; }
        public string FW004 { get; set; }
        public string FW050 { get; set; }
        public string LW056 { get; set; }
        public string LW058 { get; set; }
        public string LW059 { get; set; }
        public string LW060 { get; set; }
        public string LW000 { get; set; }
        public string DW4101 { get; set; }
        public string DW4102 { get; set; }
        public string MW900 { get; set; }
        public string MW000 { get; set; }
        public string MW090 { get; set; }
        public string PW000 { get; set; }
        public string PW010 { get; set; }
        public string PW020 { get; set; }
        public string PW030 { get; set; }
        public string DW0020 { get; set; }
    }


    public class FacilityErrorCode
    {
        [Key]
        public int Id { get; set; }
        public string ErrorCode { get; set; }
        public string Description { get; set; }

    }

    public class FacilityErrorLog
    {
        [Key]
        public int FacilityErrorLogId { get; set; }
        public string FacilityErrorCodeId { get; set; }
        public DateTime ErrorOccuredDate { get; set; }
        public string UID { get; set; }
        public string BottomDeadPoint { get; set; }
        public string Slide { get; set; }
        public string Ton { get; set; }
    }

    public class MoldStatusLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string UID { get; set; }
        public string D500 { get; set; }
        public string D501 { get; set; }
        public string D502 { get; set; }
        public string D503 { get; set; }
        public string D504 { get; set; }
        public string ErrorRate { get; set; }
        public string MoldProductivity { get; set; }
    }



    public class FacilityStatus_Inspection
    {
        [Key]
        public int Id { get; set; }
        public string UID { get; set; }
        public DateTime UpdateTime { get; set; }
        public string DW037 { get; set; }
        public string DW038 { get; set; }
        public string LW067 { get; set; }
        public string DW040 { get; set; }
        public string DW4663 { get; set; }
        public string DW4669 { get; set; }
        public string DW4522 { get; set; }


        public string DW4530 { get; set; }
        public string DW4531 { get; set; }
        public string DW4532 { get; set; }
        public string DW4533 { get; set; }
        public string DW4534 { get; set; }
        public string DW4535 { get; set; }
        public string DW4536 { get; set; }
        public string DW4537 { get; set; }
        public string DW4538 { get; set; }
        public string DW4539 { get; set; }
    }
}
