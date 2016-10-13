using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class QuotationDetails
    {
        public int QuotationDetailsID { get; set; }
        //[Required,DataType(DataType.Text)]
        public string Description { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }
        public int UnitCost { get; set; }
        public int QuantityCost { get; set; }

        public int QuotationID { get; set; }
        public virtual Quotation Quotation { get; set; }
    }
}