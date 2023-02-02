using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models
{
    public class BusinessInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string President { get; set; }
        public string IndustryType { get; set; }
        public string BusinessType { get; set; }
        public string ContactNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string BusinessNumber { get; set;}
        public string CorporationNumber { get; set; }
        public string TaxRegistrationId { get; set; }
        public string Address { get; set; }
        public string Introduce { get; set; }
        public string LogoUrl { get; set; }
        public bool Option1 { get; set; }
        public bool Option2 { get; set; }
        public bool Option3 { get; set; }

        public string CrtfcKey { get; set; }
        // public UploadFile UploadFile { get; set; }

    }
}
