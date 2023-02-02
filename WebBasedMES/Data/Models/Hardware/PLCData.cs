using System;

namespace WebBasedMES.Data.Models.Hardware
{
    public class PLCData
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Protocol { get; set; }
        public string Data { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Block { get; set; }
    }
}
