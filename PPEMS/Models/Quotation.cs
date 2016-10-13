using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class Quotation
    {
        public int QuotationID { get; set; }
        [Required]
        public string ReferenceNo { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Attention { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Sender { get; set; }

        public virtual Project Project { get; set; }
        public int ProjectID { get; set; }
    }
}