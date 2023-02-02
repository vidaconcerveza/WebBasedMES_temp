using WebBasedMES.Data.Models.Mold;

namespace WebBasedMES.ViewModels.Mold
{
    public class MoldLocationRequest
    {
        public int MoldId { get; set; }
        public string MoldLocationArea { get; set; }
        public string MoldLocationRow { get; set; }
        public string MoldLocationColumn { get; set; }
        public string MoldLocationComment { get; set; }
        public int MoldLocationIsUsing { get; set; }
    }

    public class MoldLocationResponse
    {
        public int MoldId { get; set; }
        public string MoldLocationArea { get; set; }
        public string MoldLocationRow { get; set; }
        public string MoldLocationColumn { get; set; }
        public string MoldLocationComment { get; set; }
        public int MoldLocationIsUsing { get; set; }

    }
}
