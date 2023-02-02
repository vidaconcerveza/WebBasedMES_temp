using System;

namespace WebBasedMES.ViewModels.Hardware
{
    public class PLCDataRequest
    {
        public string Uid { get; set; }
        public string Block { get; set; }
    }

    public class PLCDataResponse
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Protocol { get; set; }
        public string Data { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Block { get; set; }
    }
}
