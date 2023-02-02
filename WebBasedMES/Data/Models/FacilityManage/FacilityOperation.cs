using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBasedMES.Data.Models.BaseInfo;
using WebBasedMES.Data.Models.Lots;

namespace WebBasedMES.Data.Models.FacilityManage
{
    public class FacilityOperation
    {
        [Key]
        public int FacilityOperationId { get; set; }
        public DateTime Date { get; set; }
        public Facility Facility { get; set; }
        public WebBasedMES.Data.Models.Mold.Mold Mold { get; set; }
        public Product Product { get; set; }
        public int ProductionQuantity { get; set; }
        public int ElapsedTime { get; set; }
        public ApplicationUser Worker { get; set; }
        public ApplicationUser Register { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Memo { get; set; }
        public LotEntity Lot { get; set; }
        public IEnumerable<FacilityOperationInputItem> FacilityOperationInputItems { get; set; }
        public int IsDeleted { get; set; }
    }

    public class FacilityOperationInputItem
    {
        [Key]
        public int FacilityOperationtInputItemId {get;set;}
        public Product Product { get; set; }
        public double RequireQuantity { get; set; }
        public double LOSS { get; set; }
        public IEnumerable<FacilityOperationInputItemStock> FacilityOperationInputItemStocks { get; set; }
        public int IsDeleted { get; set; }
        public FacilityOperation FacilityOperation { get; set; }
    }

    public class FacilityOperationInputItemStock
    {
        [Key]
        public int FacilityOperationProductInputItemStockId {get;set;}
        public LotEntity Lot { get; set; }
        public int InputQuantity { get; set; }
        public int IsDeleted { get; set; }

        public FacilityOperationInputItem FacilityOperationInputItem { get; set; }
    }
}
