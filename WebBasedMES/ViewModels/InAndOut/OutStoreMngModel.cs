using System;
using System.Collections.Generic;
using WebBasedMES.Data.Models;

namespace WebBasedMES.ViewModels.InAndOutMng.InAndOut
{


    public class OutStoreRequestCrud
    {
        public int outStoreId { get; set; }
        public int isDeleted { get; set; }
        public int[] outStoreIdArray { get; set; }

        public string registerId { get; set; }
        public string outStoreEndDate { get; set; }
        public string outStoreNo { get; set; }
        public int outStoreStatusId { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
        public int uploadFileId { get; set; }
        public string outStoreDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string outStoreMemo { get; set; }
        public Boolean outStoreIsUsing { get; set; }

        public IEnumerable<UploadFile> UploadFiles { get; set; }

    }

    public class OutStoreProductRequestCrud
    {
        public int outStoreProductId { get; set; } //발주 id
        public int[] outStoreProductIdArray { get; set; }
        public int outStoreId { get; set; } //발주 id
        public string SearchInput { get; set; }
        public string IsUsingStr { get; set; } //ALL,Y,N
        public int Id { get; set; }
        public int productId { get; set; } //품목-제품으로 취급
        public string productStandardUnit { get; set; } //기준단위 
        public int productSupplyPrice { get; set; }//단위수량 
        public int productStandardUnitCount { get; set; }//단위수량 
        public string productTaxInfo { get; set; } //과세유형 

        public int productOutStoreCount { get; set; }//발주수량
        public int productBuyPrice { get; set; }//구매단가
        public string outStoreProductMemo { get; set; } //비고

        public string productCode { get; set; }//품목코드
        public string productGubun { get; set; }//품목구분
        public string productNm { get; set; }//품목이름
        public int productTotalPrice { get; set; } // 총금액
        public int productTaxPrice { get; set; }

        public OutStoreRequestCrud outStore { get; set; }
        public int isDeleted { get; set; }
        public int[] Ids { get; set; } //다건 삭제 처리 위한.
        public IEnumerable<OutStoreProductRequestCrud> outStoreProductArray { get; set; } //배열 insert을 위함.
        public int Partner { get; set; }

        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }

    public class OutStoreReq001
    {
        public int outStoreId { get; set; }
        public int outStoreProductId { get; set; }
        public string outStoreStartDate { get; set; }
        public string outStoreEndDate { get; set; }
        public string outStoreNo { get; set; }
        public string outStoreStatus { get; set; }
        public int partnerId { get; set; }
        public int productId { get; set; }
    }

    public class OutStoreReq002
    {
        public string productIsUsingStr { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productClassification { get; set; }
    }

    public class OutStoreReq003
    {
        public string searchInput { get; set; }
        public string userIsApprovedStr { get; set; }   //ALL,Y,N

    }
    public class OutStoreReq004
    {
        public string searchInput { get; set; }
        public string partnerStatusStr { get; set; }   //ALL,Y,N

    }
    public class OutStoreRes001
    {
        public int outStoreId { get; set; }

        public string outStoreNo { get; set; }
        public string outStoreDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string partnerName { get; set; }
        public string partnerTaxInfo { get; set; }
        public int outStoreProductCount { get; set; }
        public double outStoreSupplyPrice { get; set; }
        public double outStoreTaxPrice { get; set; }
        public double outStoreTotalPrice { get; set; }
        public string outStoreStatus { get; set; }
        public string registerName { get; set; }
        public string outStoreMemo { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }
    }

    public class OutStoreRes002
    {
        public int outStoreId { get; set; }
        public string outStoreNo { get; set; }
        public string outStoreDate { get; set; }
        public string requestDeliveryDate { get; set; }
        public string outStoreStatus { get; set; }
        public string registerId { get; set; }
        public string registerName { get; set; }

        public int partnerId { get; set; }
        public string partnerName { get; set; }
        public string contactName { get; set; }
        public string contactEmail { get; set; }
        public string partnerCode { get; set; }
        public string telephoneNumber { get; set; }
        public string outStoreMemo { get; set; }

        public string partnerTaxInfo { get; set; }
        public string faxNumber { get; set; }

        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }

        public IEnumerable<UploadFile> UploadFiles { get; set; }

    }

    public class OutStoreRes003
    {
        public int outStoreProductId { get; set; }
        public int outStoreId { get; set; }
        public int productId { get; set; }

        public string taskStatus { get; set; }
        public string productCode { get; set; }

        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
        public string productStandardUnit { get; set; }

        public string productTaxInfo { get; set; }
        public int productOutStoreCount { get; set; }
        public int productBuyPrice { get; set; }
        public int productSupplyPrice { get; set; }
        public int productTaxPrice { get; set; }
        public int productTotalPrice { get; set; }
        public string outStoreProductMemo { get; set; }
        public Boolean productImportCheck { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }



    }
    public class OutStoreRes004
    {
        public int outStoreProductId { get; set; }
        public int outStoreId { get; set; }
        public int productId { get; set; }

        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
        public string productStandardUnit { get; set; }
        public int productStandardUnitCount { get; set; }

        public string productTaxInfo { get; set; }
        public int productOutStoreCount { get; set; }
        public int productBuyPrice { get; set; }
        
        public int productSupplyPrice { get; set; }
        public double productTaxPrice { get; set; }
        public double productTotalPrice { get; set; }
        public string outStoreProductMemo { get; set; }

        public Boolean productImportCheck { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }

    }

    public class OutStoreRes005
    {
        public int productId { get; set; }
        public int uploadFileId { get; set; }
        public string uploadFileName { get; set; }
        public string uploadFileUrl { get; set; }

        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; }
        public string productStandard { get; set; }
        public string productUnit { get; set; }
        public string productTaxInfo { get; set; }
        public Boolean productImportCheck { get; set; }
        public string productMemo { get; set; }
        public Boolean productIsUsing { get; set; }


    }

    public class OutStoreRes006
    {
        public string userId { get; set; }

        public string userNo { get; set; }
        public string userFullName { get; set; }
        public string userRole { get; set; }
        public string userDepartment { get; set; }
        public string userPosition { get; set; }
        public string userEmail { get; set; }
        public string userPhoneNumber { get; set; }
        public Boolean userIsApproved { get; set; }

    }

    public class OutStoreRes007
    {
        public int partnerId { get; set; }

        public string partnerCode { get; set; }
        public string partnerGroup { get; set; }
        public string partnerName { get; set; }
        public string businessNumber { get; set; }
        public string presidentName { get; set; }
        public string telephoneNumber { get; set; }
        public string faxNumber { get; set; }
        public string partnerMemo { get; set; }
        public Boolean partnerStatus { get; set; }

    }
}
