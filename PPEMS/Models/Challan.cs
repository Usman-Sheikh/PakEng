using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class Challan
    {
        public int ChallanID { get; set; }
        public string ChallanNo { get; set; }
        //[RegularExpression(@"^((0[1-9])|(1[0-2]))\/((0[1-9])|(1[0-9])|(2[0-9])|(3[0-1]))\/(\d{4})$",ErrorMessage ="Date is not valid")]
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public string Note { get; set; }
        public string Remarks { get; set; }
        public string SentBy { get; set; }
        public string RecievedBy { get; set; }
        public virtual Project Project { get; set; }
        public int ProjectID { get; set; }

    }
}