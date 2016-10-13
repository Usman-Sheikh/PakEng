using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class Vendor
    {
        public int VendorID { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string CompanyTitle { get; set; }
    }
}