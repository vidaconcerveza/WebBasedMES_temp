using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebBasedMES.ViewModels.Quality
{

    public class FaultyReq001
    {

        public int etcDefectiveId { get; set; }
        public int faultyId { get; set; }
        public int[] faultyIdArray { get; set; }
        public string defectiveStartDate { get; set; }
        public string defectiveEndDate { get; set; }
        public string defectiveDate { get; set; }
        public int defectiveId { get; set; }
        public int productId { get; set; }
        public int productStandardUnit { get; set; }
        public int productStandardUnitCount { get; set; }
        public int defectiveQuantity { get; set; } //불량수량
        public string productLOT { get; set; }
        public string LotName { get; set; }
        public string ProductMemo { get; set; }
    }

    /*
    public class FaultyRes001
    {

        public int? productId { get; set; }
        public int? defectiveId { get; set; }
        public int? etcDefectiveId { get; set; }
        //public int? faultyId { get; set; }
        public string? defectiveDate { get; set; }
        public string? defectiveName { get; set; }
        public string? defectiveCode { get; set; }
        public string? productCode { get; set; } //품목 코드
        public string? productClassification { get; set; }//품목구분
        public string? productName { get; set; }//품목이름
        public string? productStandard { get; set; }//품목규격
        public string? productUnit { get; set; }//품목단위
        public string? productStandardUnit { get; set; } //기준단위
        public int? productDefectiveQuantity { get; set; } //불량수량
        public string? productLOT { get; set; } // LOT
        public string? defectiveProductMemo { get; set; } //비고
    }
    */

    [Keyless]
    public class FaultyRes001
    {
        public int etcDefectiveDetailId { get; set; }
        public int productId { get; set; }
        public int defectiveId { get; set; }
        public int? etcDefectiveId { get; set; }
        //public int? faultyId { get; set; }
        public string defectiveDate { get; set; }
        public string defectiveName { get; set; }
        public string defectiveCode { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//품목단위
        public string productStandardUnit { get; set; } //기준단위
        public int productDefectiveQuantity { get; set; } //불량수량
        public string productLOT { get; set; } // LOT
        public string defectiveProductMemo { get; set; } //비고
        public DateTime flag { get; set; }
    }




    [Keyless]
    public class FaultyRes002
    {
        public int faultyId { get; set; }
        public int defectiveId { get; set; }
        public string defectiveDate { get; set; }
        public string defectiveCode { get; set; }
        public string defectiveName { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//품목단위
        public string defectiveProductMemo { get; set; } //비고
    }
    [Keyless]
    public class FaultyRes003
    {
        public int faultyId { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//품목단위
        public string productLOT { get; set; } // LOT
        public int inventory { get; set; }
        public int isSelected { get; set; }
        public string productStandardUnit { get; set; } //기준단위
        public int productStandardUnitCount { get; set; } //기준단위기준수량
        public int defectiveQuantity { get; set; } //불량수량
    }


    [Keyless]
    public class FaultyLotResponse
    {
        public int etcDefectiveDetailId { get; set; }
        public int LotId { get; set; }
        public string productCode { get; set; } //품목 코드
        public string productClassification { get; set; }//품목구분
        public string productName { get; set; }//품목이름
        public string productStandard { get; set; }//품목규격
        public string productUnit { get; set; }//품목단위
        public int inventory { get; set; }
        public string productLOT { get; set; }
        public bool IsRegistered { get; set; }
        public int productStandardUnitId { get; set; }
        public string productStandardUnit { get; set; }
        public int productStandardUnitCount { get; set; }
        public int DefectiveQuantity { get; set; }
    }


    public class CreateEtcDefectiveRequest
    {
        [Key]
        public int etcDefectiveId { get; set; }
        public string defectiveDate { get; set; }
        public int productId { get; set; }
        public int defectiveId { get; set; }
        public int lotId { get; set; }
        public string productMemo { get; set; }

        public IEnumerable<EtcDefRequest> etcDefectiveDetails { get; set; }
    }

    public class EtcDefRequest
    {

        public int etcDefectiveDetailId { get; set; }
        public int lotId { get; set; }
        public int productStandardUnitId { get; set; }
        public int productStandardUnitCount { get; set; }
        public int defectiveQuantity { get; set; } 
    }



    /*
    public class UpdateEtcDefectiveRequest
    {
        public int EtcDefectiveId { get; set; }
        public string DefectiveDate { get; set; }
        public int ProductId { get; set; }
        public int DefectiveId { get; set; }
        public string ProductMemo { get; set; } 
        //public int ProductStandardUnitId { get; set; }
        //public int ProductStandardUnitCount { get; set; }
        //public int DefectiveQuantity { get; set; } 
        public IEnumerable<EtcDefRequest> EtcDefectiveDetails { get; set; }


    }
    */

    [Keyless]
    public class EtcDefectiveResponse
    {
        public int EtcDefectiveId { get; set; }
        public int DefectiveId { get; set; }
        public string DefectiveDate { get; set; }
        public string DefectiveCode { get;set; }

        public string DefectiveName { get;set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public string ProductMemo { get; set; }

        public IEnumerable<EtcDefectiveDetailResponse> EtcDefectiveDetails { get; set; }

    }

    [Keyless]
    public class EtcDefectiveDetailResponse
    {
        public int EtcDefectiveDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public string ProductLot { get; set; }
        public int Inventory { get; set; } = 0;
        public int ProductStandardUnitId { get; set; }
        public string ProductStandardUnit { get; set; }
        public int ProductStandardUnitCount { get; set; }
        public int DefectiveQuantity { get; set; }
        public bool IsRegistered { get; set; }
    }



}
