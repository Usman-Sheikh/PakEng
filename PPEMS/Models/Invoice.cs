using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public string Attention { get; set; }
        public string Subject { get; set; }
        public string BillNo { get; set; }
        public DateTime Date { get; set; }
        public string NTN { get; set; }
        public string RefNo { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Sender { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }
    }
}