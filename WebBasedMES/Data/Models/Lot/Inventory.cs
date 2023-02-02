using Microsoft.EntityFrameworkCore;
using WebBasedMES.Data.Models.BaseInfo;

namespace WebBasedMES.Data.Models.Lot
{
    [Keyless]
    public class Inventory
    {
        public string LotName { get; set; }
        public Product Product { get; set; }
        public int StoreOutCount { get; set; }
        public int OutOrderCount { get; set; }
        public int ConsumeCount { get; set; }
        public int ProduceCount { get; set; }
        public int ModifyCount { get; set; }
        public int DefectiveCount { get; set; }
        public int Total { get; set; }

    }
}
