using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBasedMES.ViewModels.Lot
{

    //[Keyless]
    //public class procesureT1
    //{
    //    public int? productId { get; set; }
    //    public string? productCode { get; set; }
    //    public string? productName { get; set; }
    //    [NotMapped]
    //    public int[]? lotIdArray { get; set; }
    //    [NotMapped]
    //    public string? lotName { get; set; }
    //    [NotMapped]
    //    public int? processProgressId { get; set; }
    //    [NotMapped]
    //    public string? processType { get; set; }
    //    [NotMapped]
    //    public int? isDeleted { get; set; }
    //    [NotMapped]
    //    public int? partnerId { get; set; }
    //    [NotMapped]
    //    public string? LOT { get; set; }
    //}
    [Keyless]
    public class procesureT1
    {
        public int? productId { get; set; }
        //public int? uploadFileId { get; set; }
        //public string uploadFileName { get; set; }
        //public string uploadFileUrl { get; set; }

        public string? productCode { get; set; }
        public string? productClassification { get; set; }
        public string? productName { get; set; }
        public string? productStandard { get; set; }
        public string? productUnit { get; set; }
        public string? productTaxInfo { get; set; }
        public Boolean? productImportCheck { get; set; }
        public string? productMemo { get; set; }
        public Boolean? productIsUsing { get; set; }


    }
    public class LotRequestCrud
    {
        
        public int productId { get; set; }
        public string productIsUsingStr { get; set; }
        public int lotId { get; set; }
        public int[] lotIdArray { get; set; }
        public string lotName { get; set; }
        public int processProgressId { get; set; }
        public string processType { get; set; }
        public int isDeleted { get; set; }

        public string registerStartDate { get; set; }
        public string registerEndDate { get; set; }
        public int partnerId { get; set; }
        public string LOT { get; set; }
        public string InvenType { get; set; }
        public string docuNo { get; set; }

        public string selectType { get; set; }
        public string SearchStr { get; set; }
        public string TypeStr { get; set; }

    }

    public class LotResponse01
    {
        public int productId { get; set; }
        public int lotCountId { get; set; }
        public int lotId { get; set; }
        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; } //품목 코드
        public string productStandard { get; set; }//규격
        public string productUnit { get; set; }//단위
        public int optimumStock { get; set; }//단위
        public int inventory { get; set; }//단위
        public string productMemo { get; set; }//품목이름
        public Boolean productIsUsing { get; set; }

    }


}
