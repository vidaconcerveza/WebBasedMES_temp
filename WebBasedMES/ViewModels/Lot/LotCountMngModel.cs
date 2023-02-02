using System.Collections.Generic;

namespace WebBasedMES.ViewModels.Lot
{


    public class LotCountRequestCrud
    {

        public IEnumerable<LotCountRequestCrud> LotCountArray { get; set; } //배열 insert을 위함.
        public int lotCountId { get; set; }
        public int[] lotCountIdArray { get; set; }
        public int lotId { get; set; }
        public int productId { get; set; }
        public string processType { get; set; }
        public int storeOutCount { get; set; }// 입고수량
        public int outOrderCount { get; set; }// 출고 수량
        public int consumeCount { get; set; }// 투입 수량
        public int produceCount { get; set; }// 생산 수량
        public int isDeleted { get; set; }

        public int isSelected { get; set; }
        public int quantity { get; set; }
        public int inventory { get; set; }
    }
    public class LotCountResponse01
    {
        public int productId { get; set; }
        public int lotCountId { get; set; }
        public int lotId { get; set; }
        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productName { get; set; } //품목 코드
        public string productStandard { get; set; }//규격
        public string productLOT { get; set; }//단위
        public string productUnit { get; set; }//단위
        public int inventory { get; set; }//단위
        public int isSelected { get; set; }//단위
        public int quantity { get; set; }

    }


    public class LotCountResponse02 //입출고원장 메뉴 화면, 메인화면
    {
        public int productId { get; set; }
        public int lotCountId { get; set; }
        public int partnerId { get; set; }

        public string registerDate { get; set; }
        public string type { get; set; }
        public string details { get; set; }
        public string productCode { get; set; }
        public string productClassification { get; set; }
        public string productUnit { get; set; } 
        public string productName { get; set; } 
        public string productStandard { get; set; }
        public string partnerName { get; set; } 
        public string productLOT { get; set; }
        public int priorInventory { get; set; }
        public int receivingQuantity { get; set; }
        public int shipmentQuantity { get; set; }
        public int modifyQuantity { get; set; }
        public int inputQuantity { get; set; }
        public int defectiveQuantity { get; set; }
        public int inventory { get; set; }


        public string partnerCode { get; set; }
        public string docuNo { get; set; }
        public string productTaxInfo { get; set; }
        public int count { get; set; }
        public int unitPrice { get; set; }
        public int supplyPrice { get; set; }
        public int taxPrice { get; set; }
        public int totalPrice { get; set; }

        public string productStandardUnit { get; set; }
        public int productStandardUnitCount { get; set; }
       
    }


}
