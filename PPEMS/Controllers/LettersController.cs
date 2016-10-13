using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PPEMS.Models;
using PPEMS.ViewModels;
using PagedList.EntityFramework;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;

namespace PPEMS.Controllers
{
    [Authorize(Roles = "admin")]
    public class LettersController : BaseSecurityController
    {

        public ActionResult Index()
        {
            return View();
        }




        /// Letter Creation
        public ActionResult PaymentRequest()
        {
            ViewBag.Project = db.Projects.OrderBy(o => o.ProjectID).Select(p => new SelectListItem { Value = p.ProjectID.ToString(), Text = p.Title }).ToList();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> PaymentRequest(Letter letter)
        {
            ViewBag.Project = db.Projects.OrderBy(o => o.ProjectID).Select(p => new SelectListItem { Value = p.ProjectID.ToString(), Text = p.Title }).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    letter.Body = HttpUtility.HtmlEncode(letter.Body);
                    letter.Date = DateTime.Now.Date.ToString("d");
                    letter.LetterType = "Payment";
                    db.Letters.Add(letter);
                    await db.SaveChangesAsync();
                    var print = db.Letters.Where(l => l.ReferenceNo == letter.ReferenceNo).FirstOrDefault();
                    var profin = db.Projects.Where(p => p.ProjectID == letter.ProjectID).FirstOrDefault();
                    var LetterPrint = new PaymentLetterViewModel
                    {
                        Attention = print.Attention,
                        Body = HttpUtility.HtmlDecode(print.Body),
                        ClientAddress = profin.ClientAddress,
                        ClientCompanyName = profin.ClientCompanyName,
                        Date = print.Date,
                        ReferenceNo = print.ReferenceNo,
                        Sender = print.Sender,
                        SenderDesignation = print.SenderDesignation,
                        Subject = print.Subject,
                    };


                    /////// Email Send
                    var body = "<p>Refrence #: {0}<br/>Send By:{1}</p> <p>Message:</p><br/><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(letter.Email));
                    message.From = new MailAddress("pakenggeneering@gmail.com");
                    message.Subject = letter.Subject;
                    message.Body = string.Format(body, letter.ReferenceNo, letter.Sender, letter.Body);
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "pakenggeneering@gmail.com",
                            Password = "Az@123456"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                    /////// Email Send End


                    //return PartialView("_PartialPaymentLetter", LetterPrint);
                    ModelState.AddModelError("", "Letter sent successfully");
                    return View();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(letter);
                }
            }
            else
            {
                ModelState.AddModelError("", "Fields are invalid");
                return View(letter);
            }
        }
        public ActionResult DrawingSubmission()
        {
            ViewBag.Project = db.Projects.OrderBy(o => o.ProjectID).Select(p => new SelectListItem { Value = p.ProjectID.ToString(), Text = p.Title }).ToList();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DrawingSubmission(Letter letter, HttpPostedFileBase fileUpload)
        {
            ViewBag.Project = db.Projects.OrderBy(o => o.ProjectID).Select(p => new SelectListItem { Value = p.ProjectID.ToString(), Text = p.Title }).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    if (fileUpload == null)
                    {
                        ModelState.AddModelError("", "file is requires");
                        return View(letter);
                    }
                    if (fileUpload.ContentLength > 25 * 1024 * 1024)
                    {
                        ModelState.AddModelError("", "Max length of file will be 25 mb");
                        return View(letter);
                    }
                    var filename = Path.GetFileName(fileUpload.FileName);
                    var extension = Path.GetExtension(filename).ToLower();
                    var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/FileUpload/", filename));
                    fileUpload.SaveAs(path);
                    letter.FileName = filename;
                    letter.FilePath = "~/Content/FileUpload/" + filename;
                    letter.Body = HttpUtility.HtmlEncode(letter.Body);
                    letter.Date = DateTime.Now.Date.ToString("d");
                    letter.LetterType = "Drawing";
                    db.Letters.Add(letter);
                    await db.SaveChangesAsync();
                    var print = db.Letters.Where(l => l.ReferenceNo == letter.ReferenceNo).FirstOrDefault();
                    var profin = db.Projects.Where(p => p.ProjectID == letter.ProjectID).FirstOrDefault();
                    var LetterPrint = new DrawingSubmissionViewModel
                    {
                        Attention = print.Attention,
                        Body = HttpUtility.HtmlDecode(print.Body),
                        ClientAddress = profin.ClientAddress,
                        ClientCompanyName = profin.ClientCompanyName,
                        Date = print.Date,
                        ReferenceNo = print.ReferenceNo,
                        Sender = print.Sender,
                        SenderDesignation = print.SenderDesignation,
                        Subject = print.Subject,
                    };


                    /////// Email Send
                    var body = "<p>Refrence #: {0}<br/>Send By:{1}</p> <p>Message:</p><br/><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(letter.Email));
                    message.From = new MailAddress("pakenggeneering@gmail.com");
                    message.Subject = letter.Subject;
                    message.Body = string.Format(body, letter.ReferenceNo, letter.Sender, letter.Body);
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "pakenggeneering@gmail.com",
                            Password = "Az@123456"
                        };
                        if (fileUpload.ContentLength > 0)
                        {
                            message.Attachments.Add(new Attachment(fileUpload.InputStream, Path.GetFileName(fileUpload.FileName)));
                        }
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                    /////// Email Send End
                    ModelState.AddModelError("", "Letter sent successfully");
                    return View();
                    //return PartialView("_PartialDrawingSubmission", LetterPrint);
                }
                catch (Exception ex)
                {
                    //ViewBag.Project = db.Projects.OrderBy(o => o.ProjectID).Select(p => new SelectListItem { Value = p.ProjectID.ToString(), Text = p.Title }).ToList();
                    ModelState.AddModelError("", ex.Message);
                    return View(letter);
                }
            }
            //ViewBag.Project = db.Projects.OrderBy(o => o.ProjectID).Select(p => new SelectListItem { Value = p.ProjectID.ToString(), Text = p.Title }).ToList();
            else
            {
                ModelState.AddModelError("", "Fields are invalid");
                return View(letter);
            }
        }
        public ActionResult VendorInquiry()
        {
            ViewBag.Vendor = db.Vendors.OrderBy(o => o.VendorID).Select(p => new SelectListItem { Value = p.VendorID.ToString(), Text = p.CompanyTitle }).ToList();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> VendorInquiry(Letter letter)
        {
            ViewBag.Vendor = db.Vendors.OrderBy(o => o.VendorID).Select(p => new SelectListItem { Value = p.VendorID.ToString(), Text = p.CompanyTitle }).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    letter.Body = HttpUtility.HtmlEncode(letter.Body);
                    letter.Date = DateTime.Now.Date.ToString("d");
                    letter.LetterType = "VendorInquiry";
                    db.Letters.Add(letter);
                    await db.SaveChangesAsync();
                    var print = db.Letters.Where(l => l.ReferenceNo == letter.ReferenceNo).FirstOrDefault();
                    var vendorfin = db.Vendors.Where(p => p.VendorID == letter.VendorID).FirstOrDefault();
                    var LetterPrint = new VendorInquiryViewModel
                    {
                        Attention = print.Attention,
                        Body = HttpUtility.HtmlDecode(print.Body),
                        VendorAddress = vendorfin.Address,
                        VendorCompanyName = vendorfin.CompanyTitle,
                        Date = print.Date,
                        ReferenceNo = print.ReferenceNo,
                        Sender = print.Sender,
                        SenderDesignation = print.SenderDesignation,
                        Subject = print.Subject,
                    };

                    /////// Email Send
                    var body = "<p>Refrence #: {0}<br/>Send By:{1}</p> <p>Message:</p><br/><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(letter.Email));
                    message.From = new MailAddress("pakenggeneering@gmail.com");
                    message.Subject = letter.Subject;
                    message.Body = string.Format(body, letter.ReferenceNo, letter.Sender, letter.Body);
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "pakenggeneering@gmail.com",
                            Password = "Az@123456"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                    /////// Email Send End

                    ModelState.AddModelError("", "Letter sent successfully");
                    return View();
                }
                catch (Exception ex)
                {
                    //ViewBag.Vendor = db.Vendors.OrderBy(o => o.VendorID).Select(p => new SelectListItem { Value = p.VendorID.ToString(), Text = p.CompanyTitle }).ToList();
                    ModelState.AddModelError("", ex.Message);
                    return View(letter);
                }
            }
            //ViewBag.Vendor = db.Vendors.OrderBy(o => o.VendorID).Select(p => new SelectListItem { Value = p.VendorID.ToString(), Text = p.CompanyTitle }).ToList();
            else
            {
                ModelState.AddModelError("", "Fields are invalid");
                return View(letter);
            }
        }
        public ActionResult PurchaseOrder()
        {
            ViewBag.Vendor = db.Vendors.OrderBy(o => o.VendorID).Select(p => new SelectListItem { Value = p.VendorID.ToString(), Text = p.CompanyTitle }).ToList();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> PurchaseOrder(Letter letter)
        {
            ViewBag.Vendor = db.Vendors.OrderBy(o => o.VendorID).Select(p => new SelectListItem { Value = p.VendorID.ToString(), Text = p.CompanyTitle }).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    letter.Body = HttpUtility.HtmlEncode(letter.Body);
                    letter.Date = DateTime.Now.Date.ToString("d");
                    letter.LetterType = "PurchaseOrder";
                    db.Letters.Add(letter);
                    await db.SaveChangesAsync();
                    var print = db.Letters.Where(l => l.ReferenceNo == letter.ReferenceNo).FirstOrDefault();
                    var orderPur = db.Vendors.Where(p => p.VendorID == letter.VendorID).FirstOrDefault();
                    var LetterPrint = new OrderPurchaseViewModel
                    {
                        Attention = print.Attention,
                        Body = HttpUtility.HtmlDecode(print.Body),
                        VendorAddress = orderPur.Address,
                        VendorCompanyName = orderPur.CompanyTitle,
                        Date = print.Date,
                        PO = print.PONo,
                        ReferenceNo = print.ReferenceNo,
                        Sender = print.Sender,
                        SenderDesignation = print.SenderDesignation,
                        Subject = print.Subject,
                        NTN = print.NTN
                    };

                    /////// Email Send
                    var body = "<p>Refrence #: {0}<br/>Send By:{1}</p> <p>Message:</p><br/><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(letter.Email));
                    message.From = new MailAddress("pakenggeneering@gmail.com");
                    message.Subject = letter.Subject;
                    message.Body = string.Format(body, letter.ReferenceNo, letter.Sender, letter.Body);
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "pakenggeneering@gmail.com",
                            Password = "Az@123456"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                    /////// Email Send End

                    //return PartialView("_PartialPurchaseOrder", LetterPrint);
                    ModelState.AddModelError("", "Letter sent successfully");
                    return View();
                }
                catch (Exception ex)
                {
                    ViewBag.Vendor = db.Vendors.OrderBy(o => o.VendorID).Select(p => new SelectListItem { Value = p.VendorID.ToString(), Text = p.CompanyTitle }).ToList();
                    ModelState.AddModelError("", ex.Message);
                    return View(letter);
                }
            }
            else
            {
                ModelState.AddModelError("", "Fields are invalid");
                return View(letter);
            }
        }
        /// Letter Creation End




        /// Letter Listing
        public async Task<ActionResult> PaymentsLetters(int? page, string search)
        {
            var list = from l in db.Letters
                       where l.LetterType == "Payment"
                       join j in db.Projects on l.ProjectID equals j.ProjectID
                       select new PaymentLetterViewModel
                       {
                           Id = l.LetterID,
                           ReferenceNo = l.ReferenceNo,
                           Date = l.Date,
                           ClientCompanyName = j.ClientCompanyName,
                           Sender = l.Sender,
                       };
            if (search != null)
            {
                var li = await list.OrderBy(i => i.Id).Where(s => s.ReferenceNo.Contains(search)).ToPagedListAsync(page ?? 1, 20);
                return View(li);
            }
            else
            {
                var li = await list.OrderBy(i => i.Id).ToPagedListAsync(page ?? 1, 20);
                return View(li);
            }

        }
        public async Task<ActionResult> DrawingLetters(int? page, string search)
        {
            var list = from l in db.Letters
                       where l.LetterType == "Drawing"
                       join j in db.Projects on l.ProjectID equals j.ProjectID
                       select new DrawingSubmissionViewModel
                       {
                           Id = l.LetterID,
                           ReferenceNo = l.ReferenceNo,
                           Date = l.Date,
                           ClientCompanyName = j.ClientCompanyName,
                           Sender = l.Sender,
                       };
            if (search != null)
            {
                var li = await list.OrderBy(i => i.Id).Where(s => s.ReferenceNo.Contains(search)).ToPagedListAsync(page ?? 1, 20);
                return View(li);
            }
            else
            {
                var li = await list.OrderBy(i => i.Id).ToPagedListAsync(page ?? 1, 20);
                return View(li);
            }
        }
        public async Task<ActionResult> InquiryLetters(int? page, string search)
        {
            var list = from l in db.Letters
                       where l.LetterType == "VendorInquiry"
                       join j in db.Vendors on l.VendorID equals j.VendorID
                       select new VendorInquiryViewModel
                       {
                           Id = l.LetterID,
                           ReferenceNo = l.ReferenceNo,
                           Date = l.Date,
                           VendorCompanyName = j.CompanyTitle,
                           Sender = l.Sender,
                       };
            if (search != null)
            {
                var li = await list.OrderBy(i => i.Id).Where(s => s.ReferenceNo.Contains(search)).ToPagedListAsync(page ?? 1, 20);
                return View(li);
            }
            else
            {
                var li = await list.OrderBy(i => i.Id).ToPagedListAsync(page ?? 1, 20);
                return View(li);
            }
        }
        public async Task<ActionResult> PurchaseOrderLetters(int? page, string search)
        {
            var list = from l in db.Letters
                       where l.LetterType == "PurchaseOrder"
                       join j in db.Vendors on l.VendorID equals j.VendorID
                       select new OrderPurchaseViewModel
                       {
                           Id = l.LetterID,
                           ReferenceNo = l.ReferenceNo,
                           Date = l.Date,
                           VendorCompanyName = j.CompanyTitle,
                           Sender = l.Sender,
                           NTN = l.NTN,
                           PO = l.PONo
                       };
            if (search != null)
            {
                var li = await list.OrderBy(i => i.Id).Where(s => s.ReferenceNo.Contains(search)).ToPagedListAsync(page ?? 1, 20);
                return View(li);
            }
            else
            {
                var li = await list.OrderBy(i => i.Id).ToPagedListAsync(page ?? 1, 20);
                return View(li);
            }
        }
        /// Letter Listing End




        /// Letter View
        public ActionResult PaymentsLettersView(int? id)
        {
            var print = db.Letters.Where(l => l.LetterID == id).FirstOrDefault();
            var profin = db.Projects.Where(p => p.ProjectID == print.ProjectID).FirstOrDefault();
            var LetterPrint = new PaymentLetterViewModel
            {
                Attention = print.Attention,
                Body = HttpUtility.HtmlDecode(print.Body),
                ClientAddress = profin.ClientAddress,
                ClientCompanyName = profin.ClientCompanyName,
                Date = print.Date,
                ReferenceNo = print.ReferenceNo,
                Sender = print.Sender,
                SenderDesignation = print.SenderDesignation,
                Subject = print.Subject
            };
            return View(LetterPrint);
        }
        public ActionResult QuotationLettersView(int? id)
        {
            var print = db.Quotations.Where(l => l.QuotationID == id).FirstOrDefault();
            //var LetterPrint = new PaymentLetterViewModel
            //{
            //    Attention = print.Attention,
            //    Body = HttpUtility.HtmlDecode(print.Body),
            //    ClientAddress = profin.ClientAddress,
            //    ClientCompanyName = profin.ClientCompanyName,
            //    Date = print.Date,
            //    ReferenceNo = print.ReferenceNo,
            //    Sender = print.Sender,
            //    SenderDesignation = print.SenderDesignation,
            //    Subject = print.Subject
            //};
            return View(print);
        }
        public ActionResult invoiceLettersView(int? id)
        {
            var print = db.Invoice.Where(l => l.InvoiceID == id).FirstOrDefault();
            //var LetterPrint = new PaymentLetterViewModel
            //{
            //    Attention = print.Attention,
            //    Body = HttpUtility.HtmlDecode(print.Body),
            //    ClientAddress = profin.ClientAddress,
            //    ClientCompanyName = profin.ClientCompanyName,
            //    Date = print.Date,
            //    ReferenceNo = print.ReferenceNo,
            //    Sender = print.Sender,
            //    SenderDesignation = print.SenderDesignation,
            //    Subject = print.Subject
            //};
            return View(print);
        }
        public ActionResult DrawingLettersView(int? id)
        {
            var print = db.Letters.Where(l => l.LetterID == id).FirstOrDefault();
            var profin = db.Projects.Where(p => p.ProjectID == print.ProjectID).FirstOrDefault();
            var LetterPrint = new DrawingSubmissionViewModel
            {
                Attention = print.Attention,
                Body = HttpUtility.HtmlDecode(print.Body),
                ClientAddress = profin.ClientAddress,
                ClientCompanyName = profin.ClientCompanyName,
                Date = print.Date,
                ReferenceNo = print.ReferenceNo,
                Sender = print.Sender,
                SenderDesignation = print.SenderDesignation,
                Subject = print.Subject
            };
            return View(LetterPrint);
        }
        public ActionResult InquiryLettersView(int? id)
        {
            var print = db.Letters.Where(l => l.LetterID == id).FirstOrDefault();
            var profin = db.Vendors.Where(p => p.VendorID == print.VendorID).FirstOrDefault();
            var LetterPrint = new VendorInquiryViewModel
            {
                Attention = print.Attention,
                Body = HttpUtility.HtmlDecode(print.Body),
                VendorAddress = profin.Address,
                VendorCompanyName = profin.CompanyTitle,
                Date = print.Date,
                ReferenceNo = print.ReferenceNo,
                Sender = print.Sender,
                SenderDesignation = print.SenderDesignation,
                Subject = print.Subject
            };
            return View(LetterPrint);
        }
        public ActionResult PurchaseOrderLettersView(int? id)
        {
            var print = db.Letters.Where(l => l.LetterID == id).FirstOrDefault();
            var profin = db.Vendors.Where(p => p.VendorID == print.VendorID).FirstOrDefault();
            var LetterPrint = new OrderPurchaseViewModel
            {
                Attention = print.Attention,
                Body = HttpUtility.HtmlDecode(print.Body),
                VendorAddress = profin.Address,
                VendorCompanyName = profin.CompanyTitle,
                Date = print.Date,
                ReferenceNo = print.ReferenceNo,
                Sender = print.Sender,
                PO = print.PONo,
                SenderDesignation = print.SenderDesignation,
                Subject = print.Subject,
                NTN = print.NTN
            };
            return View(LetterPrint);
        }
        /// Letter View End




        /// Partial Letter Lists
        public ActionResult PaymentLattersList(int id)
        {
            var list = from l in db.Letters
                       where l.LetterType == "Payment" && l.ProjectID == id
                       join j in db.Projects on l.ProjectID equals j.ProjectID
                       select new PaymentLetterViewModel
                       {
                           Id = l.LetterID,
                           ReferenceNo = l.ReferenceNo,
                           Date = l.Date,
                           ClientCompanyName = j.ClientCompanyName,
                           Sender = l.Sender,
                       };
            return PartialView(list);
        }
        public ActionResult DrawingSubmissionList(int id)
        {
            var list = from l in db.Letters
                       where l.LetterType == "Drawing" && l.ProjectID == id
                       join j in db.Projects on l.ProjectID equals j.ProjectID
                       select new DrawingSubmissionViewModel
                       {
                           Id = l.LetterID,
                           ReferenceNo = l.ReferenceNo,
                           Date = l.Date,
                           ClientCompanyName = j.ClientCompanyName,
                           Sender = l.Sender,
                       };
            return PartialView(list);
        }
        public ActionResult QuotationLetterList(int id)
        {
            var list = db.Quotations.Where(i => i.ProjectID == id).ToList();
            return PartialView(list);
        }
        public ActionResult InvoiceLetterList(int id)
        {
            var list = db.Invoice.Where(i => i.ProjectID == id).ToList();
            return PartialView(list);
        }
        /// Partial Letter Lists End


        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Letter letter = db.Letters.Find(id);
        //    if (letter == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(letter);
        //}

        //public ActionResult Create()
        //{
        //    ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title");
        //    ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Name");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "LetterID,ReferenceNo,Date,Attention,Subject,Body,Sender,Attachments,Payment,DeliveryAt,DeliveryTime,AcceptedBy,ProjectID,VendorID")] Letter letter)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Letters.Add(letter);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", letter.ProjectID);
        //    ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Name", letter.VendorID);
        //    return View(letter);
        //}
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Letter letter = db.Letters.Find(id);
        //    if (letter == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", letter.ProjectID);
        //    ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Name", letter.VendorID);
        //    return View(letter);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "LetterID,ReferenceNo,Date,Attention,Subject,Body,Sender,Attachments,Payment,DeliveryAt,DeliveryTime,AcceptedBy,ProjectID,VendorID")] Letter letter)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(letter).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Title", letter.ProjectID);
        //    ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Name", letter.VendorID);
        //    return View(letter);
        //}

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Letter letter = db.Letters.Find(id);
        //    if (letter == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(letter);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Letter letter = db.Letters.Find(id);
        //    db.Letters.Remove(letter);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

    }
}
