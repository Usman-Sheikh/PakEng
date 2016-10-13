using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class InvoiceItems
    {
        public int InvoiceItemsID { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int Rate { get; set; }
        public int Amount { get; set; }
        public string ItemType { get; set; }
        public int InvoiceID { get; set; }
        public virtual Invoice Invioce { get; set; }
    }
}