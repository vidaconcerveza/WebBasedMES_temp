using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebBasedMES.Data.Models.Barcode;

namespace WebBasedMES.Data.Models.BaseInfo
{
    // [Keyless]
    public class Product
    {
        public int Id { get; set; }
        public CommonCode CommonCode { get; set; }
        public Partner Partner { get; set; }
        public CommonCode Model { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Standard { get; set; }
        public int OptimumStock { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Item Item { get; set; }//ItemId 추가
        public UploadFile UploadFile { get; set; }
        public IEnumerable<ProductProcess> Processes { get; set; }

        //  public IEnumerable<ProductItem> Items { get; set; }
        public string TaxType { get; set; } = "과세";
        public int BuyPrice { get; set; } = 0;
        public int SellPrice { get; set; } = 0;
        public bool ImportCheck { get; set; } = true;// 수입검사
        public bool ExportCheck { get; set; } = true; // 출고검사

        public BarcodeMaster Barcode { get; set; }

    }

    //  [Keyless]
    public class ProductProcess
    {
        [Key]
        public int ProductProcessId { get; set; }
        // [NotMapped]
        public Process Process { get; set; }
        //[ForeignKey("ProductId")]
        //[NotMapped]
        //여기에 있는 애는 1:N관계일때 매칭시켜주는 친구에요.
        public Product Product { get; set; }
        public int ProductId { get; set; }


        public Partner Partner { get; set; }

        public int ProcessId { get; set; } = 0;
        public int PartnerId { get; set; } = 0;
        public int ProduceProductId { get; set; } = 0;
        public int ProcessOrder { get; set; }
        public int IsDeleted { get; set; }

        public bool IsOutSourcing { get; set; } = false;
        public bool IsFinal { get; set; } = false;
        public string Memo { get; set; } = "";
        public IEnumerable<ProductItem> Items { get; set; }
    }
    //   [Keyless]
    public class ProductItem
    {
        [Key]
        public int ProductItemId { get; set; }
        //   [ForeignKey("ProductProcessId")]
        //  [NotMapped]
        public ProductProcess ProductProcess { get; set; }
        public int ProductProcessId { get; set; }
        //  [NotMapped]
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int ProcessId { get; set; }
        public double Loss { get; set; }
        public double Require { get; set; }
        public int Priority { get; set; }
        public string Memo { get; set; }
        public int IsDeleted { get; set; }
        public int ProcessOrder { get; set; }

        //    public int ProcessNumber { get; set; }

        //     public Process Process { get; set; }
        //    public Item Item { get; set; }
        //    public CommonCode InputUnit { get; set; }
    }

    public class ProductProduce
    {
        [Key]
        public int ProductProduceId { get; set; }
        //   [NotMapped]
        public ProductProcess ProductProcess { get; set; }
        //   [NotMapped]
        public Product Product { get; set; }

        public int IsDeleted { get; set; }

    }
}
