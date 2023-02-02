using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.ViewModels.Bom
{
    public class ProductProcessRes
    {
        public int productProcessId { get; set; }
        public int productId { get; set; }
        public int processOrder { get; set; }
        public int processId { get; set; }
        public int partnerId { get; set; }
       
    }

    public class ProductProcessReq
    {
        public int[] productProcessIdArray { get; set; }
        public int isDeprecated { get; set; }
        public int productProcessId { get; set; }
        public int productId { get; set; }
        public int processOrder { get; set; }
        public int processId { get; set; }
        public int partnerId { get; set; }
       
    }
    public class ProductItemRes
    {
        public int productItemId { get; set; }
        public int productId { get; set; }
        public int productProcessId { get; set; }
        public float require { get; set; }
        public float loss { get; set; }
        public string memo { get; set; }
        public int priority { get; set; }
    }

    public class ProductItemReq
    {
        public int[] productItemsIdArray { get; set; }
        public int isDeprecated { get; set; }
        public int productItemsId { get; set; }
        public int productId { get; set; }
        public int productProcessId { get; set; }
        public int require { get; set; }
        public int loss { get; set; }
        public string memo { get; set; }
        public int priority { get; set; }
    }
    public class ProductProduceRes
    {
       
        public int productProduceId { get; set; }
        public int productId { get; set; }
        public int productProcessId { get; set; }

    }
    public class ProductProduceReq
    {
        public int isDeprecated { get; set; }
        public int productProduceId { get; set; }
        public int productId { get; set; }
        public int ProductProcessId { get; set; }
        public int[] productProducesIdArray { get; set; }

    }

    public class ProductInputItemRequest
    {
        public int ProductId { get; set; }
    }

    public class ProductInputItemResponse
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductClassification { get; set; }
        public string ProductStandard { get; set; }
        public string ProductUnit { get; set; }
        public double RequireQuantity { get; set; }
        public double LOSS { get; set; }
        public double TotalRequire { get; set; }
        
        public IEnumerable<ProductInputItemStock> ProductInputItemStocks { get; set; }
    }

    public class ProductInputItemStock
    {
        public int LotId { get; set; }
        public string LotName { get; set; }
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public string ProductStandard { get; set; }
        public int StockCount { get; set; }
        public bool IsSelected { get; set; }
        public int InputQuantity { get; set; }
    }
}
