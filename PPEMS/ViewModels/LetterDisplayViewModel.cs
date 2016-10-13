using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPEMS.ViewModels
{
    public class PaymentLetterViewModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string ClientCompanyName { get; set; }
        public string ClientAddress { get; set; }
        public string Attention { get; set; }
        public string Subject { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public string SenderDesignation { get; set; }
    }
    public class DrawingSubmissionViewModel
    {
        public string ReferenceNo { get; set; }
        public string ClientCompanyName { get; set; }
        public string ClientAddress { get; set; }
        public string Attention { get; set; }
        public string Subject { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public string SenderDesignation { get; set; }
        public int Id { get; set; }
    }

    public class VendorInquiryViewModel
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string VendorCompanyName { get; set; }
        public string VendorAddress { get; set; }
        public string Attention { get; set; }
        public string Subject { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public string SenderDesignation { get; set; }
    }
    public class OrderPurchaseViewModel
    {
        public int Id { get; set; }
        public string NTN { get; set; }
        public string PO { get; set; }
        public string AcceptedBy { get; set; }
        public string ReferenceNo { get; set; }
        public string VendorCompanyName { get; set; }
        public string VendorAddress { get; set; }
        public string Attention { get; set; }
        public string Subject { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public string SenderDesignation { get; set; }
    }

    public class ChallanViewModel
    {
        public int ProjectID { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string Remarks { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string ClientCompany { get; set; }
        public string ProjectTitle { get; set; }
        public string ChallanNo { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
    }
}