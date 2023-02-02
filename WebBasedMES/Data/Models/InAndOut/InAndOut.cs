using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.Barcode;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Data.Models.Lots;

namespace WebBasedMES.Data.Models
{


    //public class InAndOutCommonRequest
    //{
    //    public int SortId { get; set; }
    //    public Boolean IsUsing { get; set; }
    //}
    //public class InAndOutCommonResponse
    //{
    //    public int CommonId { get; set; }
    //    public int SortId { get; set; }
    //    public string Code { get; set; }
    //    public string Memo { get; set; }
    //    public string Name { get; set; }
    //    public Boolean IsUsing { get; set; }

    //}
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public Partner Partner { get; set; } //거래처 Partners
        //public UploadFile UploadFile { get; set; } //첨부파일 UploadFiles
        public ApplicationUser Register { get; set; } // 등록자
        public string OrderNo { get; set; } //수주 번호
        public DateTime OrderDate { get; set; } //등록일
        public DateTime RequestDeliveryDate { get; set; } //납품요청일
        public string OrderMemo { get; set; } //비고
        public IEnumerable<OrderProduct> OrderProducts { get; set; } //품목 Products
        public int IsDeleted { get; set; } //활성,비활성
        public string OrderFinish { get; set; }
        public IEnumerable<UploadFile> UploadFiles { get; set; }
    }

    //수주 && 제품
    public class OrderProduct
    {
        [Key]
        public int OrderProductId { get; set; }
        public Order Order { get; set; } //수주
        public int ProductId { get; set; } //상품 아이디

        //  [NotMapped]
        public Product Product { get; set; } //품목-제품으로 취급

        //public CommonCode ProductStandardUnit { get; set; } //기준단위[]
        public string ProductStandardUnit { get; set; } //기준단위[]
        public int ProductStandardUnitCount { get; set; }//기준단위수량
        public string ProductTaxInfo { get; set; } //과세유형
        public int ProductOrderCount { get; set; }//수주수량
        public int ProductSellPrice { get; set; }//판매단가
        public int ProductSupplyPrice { get; set; }//공급가액
        public int ProductTaxPrice { get; set; }//세액
        public int ProductTotalPrice { get; set; }//총금액
        public string OrderProductMemo { get; set; }//총금액[]
        //public int IsDeprecated { get; set; } //활성,비활성
        public int IsDeleted { get; set; } //활성,비활성
        public string OrderStatus { get; set; } = "수주등록";

    }


    public class OutStore
    {
        [Key]
        public int OutStoreId { get; set; }
        public Partner Partner { get; set; } //거래처 Partners
        //public UploadFile UploadFile { get; set; } //첨부파일 UploadFiles
        public IEnumerable<UploadFile> UploadFiles { get; set; }
        public ApplicationUser Register { get; set; } // 등록자
        public string OutStoreNo { get; set; } //발주 번호
        public DateTime OutStoreDate { get; set; } //발주일
        public DateTime RequestDeliveryDate { get; set; } //납품요청일
        public string OutStoreMemo { get; set; } //비고
        public IEnumerable<OutStoreProduct> OutStoreProducts { get; set; } //발주아이템
        public int IsDeleted { get; set; } //활성,비활성
        public string OutStoreFinish { get; set; }

    }
    //발주 && 제품
    public class OutStoreProduct
    {
        [Key]
        public int OutStoreProductId { get; set; }
        public OutStore OutStore { get; set; } //발주
                                               //  [NotMapped]
        public Product Product { get; set; } //품목-제품으로 취급
        public string ProductStandardUnit { get; set; } //기준단위[]
        public int ProductStandardUnitCount { get; set; }//기준단위수량
        public string ProductTaxInfo { get; set; } //과세유형
        public int ProductOutStoreCount { get; set; }//발주수량
        public int ProductBuyPrice { get; set; }//구매단가
        public int ProductSupplyPrice { get; set; }//공급가액
        public int ProductTaxPrice { get; set; }//세액
        public int ProductTotalPrice { get; set; }//총금액
        public string OutStoreProductMemo { get; set; }//비고
        //public int IsDeprecated { get; set; } //활성,비활성
        public int IsDeleted { get; set; } //활성,비활성
        public string OutStoreStatus { get; set; } = "입고대기";
        public IEnumerable<StoreOutStoreProduct> StoreOutStoreProducts { get; set; }

    }



    public class Store
    {
        [Key]
        public int ReceivingId { get; set; }
        public Partner Partner { get; set; } //거래처 Partners
        public IEnumerable<UploadFile> UploadFiles { get; set; } //첨부파일 UploadFiles
        public ApplicationUser Register { get; set; } // 등록자       
        public DateTime ReceivingDate { get; set; } //입고일
        public string ReceivingNo { get; set; } //입고번호
        public string ReceivingMemo { get; set; } //비고
        public IEnumerable<StoreOutStoreProduct> StoreOutStoreProducts { get; set; } //입고-발주목록
        public int IsDeleted { get; set; } //활성,비활성

    }

    public class StoreOutStoreProduct //입고-발주품목
    {
        public int StoreOutStoreProductId { get; set; }
        public Store Receiving { get; set; } //입고
        public OutStoreProduct OutStoreProduct { get; set; } //발주&&제품
        public LotEntity Lot { get; set; }
        public string LotName { get; set; } // Lot
        public int ProductReceivingCount { get; set; } //입고수량
        public int ProductBuyPrice { get; set; } //구매단가
        public int ProductSupplyPrice { get; set; }//공급가액
        public int ProductTaxPrice { get; set; }//세액
        public int ProductTotalPrice { get; set; }//총금액
        public int ProductImportCheckResult { get; set; } = 3; //수입검사결과  
        public string ReceivingProductMemo { get; set; } //비고
        public int IsDeleted { get; set; } //활성,비활성
        public IEnumerable<StoreOutStoreProductDefective> StoreOutStoreProductDefectives { get; set; }
        public IEnumerable<StoreOutStoreProductInspectionType> storeOutStoreProductInspections { get; set; }


        public string ProductTaxInfo { get; set; }
        public string ProductStandardUnit { get; set; }
        public int ProductStandardUnitCount { get; set; }

      //  public int BarcodeIssueCount { get; set; } = 0;
      //  public int BarcodeReIssueCount { get; set; } = 0;
        public BarcodeMaster Barcode { get; set; }
    }

    public class StoreOutStoreProductDefective
    {
        [Key]
        public int StoreOutStoreProductDefectiveId { get; set; }
        public StoreOutStoreProduct StoreOutStoreProduct { get; set; } //입고-발주품목
        public Defective Defective { get; set; } //불량유형
        public int DefectiveQuantity { get; set; } //불량수량
        public string DefectiveUnit { get; set; } //기준단위
        public int IsDeleted { get; set; } //활성,비활성
        public string DefectiveMemo { get; set; }
        public LotEntity Lot { get; set; }
    }

    public class StoreOutStoreProductInspectionType
    {
        [Key]
        public int StoreOutStoreProductInspectionId { get; set; }
        public StoreOutStoreProduct StoreOutStoreProduct { get; set; } //입고-발주품목
        public InspectionType InspectionType { get; set; } //검사항목
        public int InspectionResult { get; set; } = 3; //검사 결과   //
        public string InspectionResultText { get; set; } = "미검사";//검사 결과
        public int IsDeleted { get; set; } //활성,비활성
    }

    public class OutOrder
    {
        [Key]
        public int OutOrderId { get; set; }
        public Partner Partner { get; set; } //거래처 Partners
        public IEnumerable<UploadFile> UploadFiles { get; set; } //첨부파일 UploadFiles
        public ApplicationUser Register { get; set; } // 등록자       
        public string ShipmentNo { get; set; } //출고번호
        public DateTime ShipmentDate { get; set; } //출고일
        public string ShipmentMemo { get; set; } //출고상태
        public IEnumerable<OutOrderProduct> OutOrderProducts { get; set; } //입고-발주목록
        public int IsDeleted { get; set; }
    }

    public class OutOrderProduct  //출고-수주품목
    {
        [Key]
        public int OutOrderProductId { get; set; }
        public OutOrder OutOrder { get; set; } //출고
        public int ProductId { get; set; } // 상품ID
        public Product Product { get; set; } //품목 //[NotMapped]
        public OrderProduct OrderProduct { get; set; } //수주-품목 목록
        public int Quantity { get; set; } //출고수량
        public int ProductShipmentCount { get; set; } //출고수량
        public int ProductSellPrice { get; set; } //출고수량
        public int ProductSupplyPrice { get; set; } //공급가액
        public int ProductTaxPrice { get; set; } //세액
        public int ProductTotalPrice { get; set; } //총 금액
        public string ShipmentProductMemo { get; set; } //비고
        public int IsDeleted { get; set; }

      //  public int BarcodeIssueCount { get; set; }
      //  public int BarcodeReIssueCount { get; set; }

        public BarcodeMaster Barcode { get; set; }
        public IEnumerable<OutOrderProductStock> OutOrderProductStock { get; set; }
        //public LotEntity Lot { get; set; }
    }
    //[Table("OutOrderProductsDefectives")]
    public class OutOrderProductDefective  //출고-출고 마스터 아이템 불량 유형
    {
        [Key]
        public int OutOrderProductDefectiveId { get; set; }
        public OutOrderProduct OutOrderProduct { get; set; }
        public Defective Defective { get; set; }
        public int DefectiveQuantity { get; set; }
        public int IsDeleted { get; set; } //활성,비활성

        public LotEntity Lot { get; set; }
        public string LotName { get; set; }

        public string DefectiveProductMemo { get; set; }
    }

    public class OutOrderProductStock  //출고-출고 마스터 아이템 품목 재고
    {
        [Key]
        public int OutOrderProductStockId { get; set; }
        public OutOrderProduct OutOrderProduct { get; set; }
        public LotEntity Lot { get; set; }
        public string LotName { get; set; }
        public string ProductShipmentCheckResult { get; set; }
        public int IsDeleted { get; set; } //활성,비활성

        public BarcodeMaster Barcode { get; set; }
    }


}
