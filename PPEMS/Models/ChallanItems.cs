using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class ChallanItems
    {
        public int ChallanItemsID { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }

        public int ChallanID { get; set; }
        public virtual Challan Challan { get; set; }
    }
}