using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models;
using WebBasedMES.ViewModels.ProducePlan;

namespace WebBasedMES.ViewModels.BaseInfo
{
    //2022.06.24
    public class ProductResponse
    {
        public int Id { get; set; }
        public int CommonCode { get; set; }
        public string CommonCodeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Standard { get; set; }
        public int OptimumStock { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;
        public UploadFile UploadFile { get; set; }
        public string Picture { get; set; }
        public IEnumerable<ProductProcessInterface> ProductProcesses { get; set; }
        public IEnumerable<ProductItemInterface> ProductItems { get; set; }
        public int ProcessCount { get; set; }
        public int ItemCount { get; set; }
        public string TaxType { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public bool ImportCheck { get; set; } // 수입검사
        public bool ExportCheck { get; set; } // 출고검사


        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }

    }



    public class ProductRequest
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int[] ProductIds { get; set; }
        public int CommonCode { get; set; }
        public string CommonCodeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Standard { get; set; }
        public int OptimumStock { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public string TaxType { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public bool ImportCheck { get; set; } // 수입검사
        public bool ExportCheck { get; set; } // 출고검사


        public UploadFile UploadFile { get; set; }
        public IEnumerable<ProductProcessInterface> ProductProcesses { get; set; }
        public IEnumerable<ProductItemInterface> ProductItems { get; set; }

        public string SearchStr { get; set; }
        public string TypeStr { get; set; }
        public string IsUsingStr { get; set; }
        public bool AutoCode { get;set; }

        //태영 전용
        public int PartnerId { get; set; }
        public int ModelId { get; set; }
    }

    public class ProductProcessInterface
    {
        public int ProductProcessId { get; set; }
        public int ProcessId { get; set; }
        public int ProductId { get; set; }
        public int PartnerId { get; set; }
        public int ProduceProuctId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
        public int ProcessOrder { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessType { get; set; }
        public string ProcessName { get; set; }
        public string CommonCodeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public bool IsFinal { get; set; }
        public string Memo { get; set; }
        public bool IsOutSourcing { get; set; }

        public string ProduceProductType { get; set; }
        public string ProduceProductName { get; set; }
        public string ProduceProductStandard { get; set; }
        public string ProduceProductUnit { get; set; }

    }

    public class ProductItemInterface
    {
        public int ProductItemsId { get; set; }
        public int ProcessOrder { get; set; }
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }


        //품목
        public int ProductId { get; set; }
        //public string ItemClassify { get; set; }
        public string ProductCommonCodeName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }


        public string Standard { get; set; }
        public string Unit { get; set; }
        public int Require { get; set; }
        public int Loss { get; set; }
        public string Memo { get; set; }
        public int Priority { get; set; }


        public string CommonCodeName { get; set; }
        public int ProductProcessId { get; set; }



        //이전 쓰던...
        public string Code { get; set; }
        public string Name { get; set; }
        public string ItemClassify { get; set; }


        // public string ProductCode { get; set; }
        //      public string InputUnitName { get; set; }
        //     public string InputUnitCode { get; set; }
        //     public string InputUnit { get; set; }
    }




    public class BomResponse
    {
        public int Id { get; set; }
        public int CommonCode { get; set; }
        public string CommonCodeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Standard { get; set; }
        public int OptimumStock { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;
        public UploadFile UploadFile { get; set; }
        public string Picture { get; set; }
        public IEnumerable<BomProductProcessInterface> ProductProcesses { get; set; }
        public IEnumerable<BomProductItemInterface> ProductItems { get; set; }
        public int ProcessCount { get; set; }
        public int ItemCount { get; set; }

        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
    }

    public class BomResponseSortByProcess
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCommonCodeName { get; set; }
        public string ProductUnit { get; set; }
        public bool ProductIsUsing { get; set; }
        public string ProductStandard { get; set; }
        public IEnumerable<BomProductProcessResponseByProcess> ProductProcesses { get; set; }

        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }

        public int ProcessCount { get; set; }


    }

    public class BomProductProcessResponseByProcess
    {
        //생산공정 정보
        public int ProductProcessId { get; set; }

        public int ProcessOrder { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessCommonCodeName { get; set; }
        public string ProcessName { get; set; }
        public bool IsUsing { get; set; }
        public bool IsFinal { get; set; }
        public bool IsOutSourcing { get; set; }
        public string Memo { get; set; }


        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }


        //생산품목
        public int ProductProducedId { get; set; }
        public string ProductProducedCode { get; set; }
        public string ProductProducedName { get; set; }
        public string ProductProducedCommonCodeName { get; set; }
        public string ProductProducedStandard { get; set; }
        public string ProductProducedUnit { get; set; }

        //해당 생산 공정에서의 투입 아이템
        public IEnumerable<BomProductItemReponseByProcess> ProductItems { get; set; }

    }

    public class BomProductItemReponseByProcess
    {
        public int ProductItemId { get; set; }
        public int ProductProcessId { get; set; }

        public int ProcessId { get; set; }
        public int ProcessOrder { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }


        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductCommonCodeName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public float Require { get; set; }
        public float Loss { get; set; }
        public string Memo { get; set; }
        public int Priority { get; set; }
    }




    public class BomProductProcessInterface
    {
        public int ProductId { get; set; }
        public int ProductProcessId { get; set; }
        public int PartnerId { get; set; }
        public int ProduceProductId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }

        public bool IsFinal { get; set; }
        public string Memo { get; set; }
        public bool IsOutSourcing { get; set; }

        public int ProcessOrder { get; set; }
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessType { get; set; }
        public string ProcessName { get; set; }

        public string ProduceProductType { get; set; }
        public string ProduceProductName { get; set; }
        public string ProduceProductCode { get; set; }
        public string ProduceProductStandard { get; set; }
        public string ProduceProductUnit { get; set; }

    }

    public class BomProductItemInterface
    {
        public int ProductItemsId { get; set; }
        public int ProcessOrder { get; set; }
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }


        //품목
        public int ProductId { get; set; }
        //public string ItemClassify { get; set; }
        public string ProductCommonCodeName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }


        public string Standard { get; set; }
        public string Unit { get; set; }
        public float Require { get; set; }
        public float Loss { get; set; }
        public float TotalRequire { get; set; }
        public string Memo { get; set; }
        public int Priority { get; set; }
        public int ProductProcessId { get; set; }

    }



    public class BomUpdateRequest
    {
        public int ProductId { get; set; }
        public IEnumerable<ProductProcessUpdate> ProductProcesses { get; set; }
        public IEnumerable<ProductItemUpdate> ProductItems { get; set; }

    }

    public class ProductProcessUpdate
    {
        public int ProductProcessId { get; set; }
        public int ProcessOrder { get; set; }
        public int ProcessId { get; set; }
        public int PartnerId { get; set; }
        public int ProduceProductId { get; set; }
        public bool IsOutSourcing { get; set; }
        public bool IsFinal { get; set; }
        public string Memo { get; set; }

    }

    public class ProductItemUpdate
    {
        public int ProductProcessId { get; set; }
        public int ProductItemId { get; set; }
        public int ProcessOrder { get; set; }
        public int ProcessId { get; set; }
        public int ProductId { get; set; }
        public float Require { get; set; }
        public float Loss { get; set; }
        public string Memo { get; set; }
        public int Priority { get; set; } = 1;

        public string picture { get; set; }
    }

    public class InputProducedProductResponse
    {
        public int ProductId { get; set; }
        public int CommonCode { get; set; }
        public string CommonCodeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Standard { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;

        public string Picture { get; set; }

        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }

    }

    public class ProductPopupRequest
    {
        public string SearchInput { get; set; }
        public string ProductIsUsing { get; set; }
        public string ProductClassification { get; set; }

        public string RequestType { get; set; }
    }

    public class ProductPopupResponse
    {
        
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductClassification { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public string ProductTaxInfo { get; set; }
        
        public string ProductMemo { get; set; }
        public string ProductIsUsing { get; set; }
        public string? UploadFileName { get; set; }
        public string? UploadFileUrl { get; set; }

        public int Inventory { get; set; }
        public int OptimumStock { get; set; }

        public int ProductOrderCount { get; set; } = 0;
        public int OrderId { get; set; } = 0;
        public string ProductTaxType { get; set; }

        public IEnumerable<ProductProcessInterface2> ProducePlanProcesses { get; set; }
        public IEnumerable<ProducePlanProcessInterface002> WorkerOrderProducePlans { get; set; }

        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
    }


    public class ProductProcessInterface2
    {
        public int ProductProcessId { get; set; }
        public int ProductId { get; set; }
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public int ProcessOrder { get; set; }
        public string ProductClassification { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public int Inventory { get; set; } = 0;
        public int ProcessPlanQuantity { get; set; } = 0;
        public IEnumerable<ProductItemInterface2> ProcessInputItems { get; set; }
    }

    public class ProductItemInterface2
    {
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemClassification { get; set; }
        public string ItemName { get; set; }
        public string ItemStandard { get; set; }
        public string ItemUnit { get; set; }
        public float RequiredQuantity { get; set; }
        public float Loss { get; set; }
        public float TotalRequiredQuantity { get; set; }
        public int ProcessPlanQuantity { get; set; }
        public int Inventory { get; set; }
        public int TotalInputQuantity { get; set; }
    }


    public class ProcessNotWorkPopupRequest
    {
        public string SearchInput { get; set; }
        public string ShutdownIsUsing { get; set; }
    }

    public class ProcessNotWorkPopupResponse
    {
        public int shutdownCodeId { get; set; }
        public string shutdownCode { get; set; }
        public string shutdownName { get; set; }
        public string shutdownMemo { get; set; }
        public bool shutdownIsUsing { get; set; }

    }



    public class BomUpdateInterface
    {
        public string ProductCode { get; set; }
        public string ProcessCode { get; set; }
        public int ProcessOrder { get; set; }
        public string ProducedProductCode { get; set; }
        public int Cavity { get; set; }
        public string InputItemCode { get; set; }
        public double Loss { get; set; }
        public double Require { get; set; }
    }






}
