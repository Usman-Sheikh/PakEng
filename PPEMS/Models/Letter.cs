using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPEMS.Models
{
    public class Letter
    {
        public int LetterID { get; set; }
        public string ReferenceNo { get; set; }
        //[RegularExpression(@"^((0[1-9])|(1[0-2]))\/((0[1-9])|(1[0-9])|(2[0-9])|(3[0-1]))\/(\d{4})$", ErrorMessage = "Date is not valid")]
        public string Date { get; set; }
        public string Attention { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string PONo { get; set; }
        public string Sender { get; set; }
        public string SenderDesignation { get; set; }
        public string Attachments { get; set; }
        public string Payment { get; set; }
        public string DeliveryAt { get; set; }
        // [RegularExpression(@"^((0[1-9])|(1[0-2]))\/((0[1-9])|(1[0-9])|(2[0-9])|(3[0-1]))\/(\d{4})$", ErrorMessage = "Date is not valid")]
        public string DeliveryTime { get; set; }
        public string AcceptedBy { get; set; }
        public int? ProjectID { get; set; }
        public int? VendorID { get; set; }
        public string LetterType { get; set; }
        public string NTN { get; set; }
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,3})$", ErrorMessage = "Email is not a valid e-mail address.")]
        public string Email { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}