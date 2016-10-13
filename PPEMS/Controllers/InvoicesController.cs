using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PPEMS.Models;
using System.Threading.Tasks;

namespace PPEMS.Controllers
{
    [Authorize(Roles = "admin")]
    public class InvoicesController : BaseSecurityController
    {

        public ActionResult Create()
        {
            ViewBag.ProjectID = db.Projects.OrderBy(o => o.ProjectID).Select(s => new SelectListItem { Value = s.ProjectID.ToString(), Text = s.Title }).ToList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Invoice invoice)
        {
            ViewBag.ProjectID = db.Projects.OrderBy(o => o.ProjectID).Select(s => new SelectListItem { Value = s.ProjectID.ToString(), Text = s.Title }).ToList();
            if (ModelState.IsValid)
            {
                var find = await db.Invoice.Where(i => i.RefNo.ToLower() == invoice.RefNo.ToLower()).FirstOrDefaultAsync();
                if (find != null)
                {
                    ModelState.AddModelError("", "Invoice already exists for same refrence #");
                    return View(invoice);
                }
                db.Invoice.Add(invoice);
                await db.SaveChangesAsync();
                ModelState.AddModelError("", "Invoice Created Successfully");
                return View();
            }
            {
                ModelState.AddModelError("", "Input fields are not valid");
                return View(invoice);
            }
        }


        ///// Add Items to invoice
        public ActionResult AddItem()
        {
            ViewBag.Pro = db.Projects.OrderBy(o => o.ProjectID).Select(s => new SelectListItem { Value = s.ProjectID.ToString(), Text = s.Title }).ToList();
            return View();
        }
        public ActionResult SelectInvoice(int? Project)
        {
            ViewBag.Invoice = db.Invoice.OrderBy(o => o.InvoiceID).Where(i => i.ProjectID == Project).Select(s => new SelectListItem { Value = s.InvoiceID.ToString(), Text = s.BillNo }).ToList();
            return PartialView("_PartialInvoiceLoad");
        }

        public ActionResult InvoiceItemForm(int? Invoice)
        {
            var id = db.InvoiceItems.Where(i => i.InvoiceID == Invoice).FirstOrDefault();
            if (id != null)
            {
                InvoiceItems invoiceItems = new InvoiceItems();
                invoiceItems.InvoiceID = id.InvoiceID;
                ViewBag.ItemList = db.InvoiceItems.OrderBy(o => o.InvoiceItemsID).Where(i => i.InvoiceID == invoiceItems.InvoiceID).ToList();
                return PartialView("_PartialInvoiceForm", invoiceItems);
            }
            else
            {
                InvoiceItems invoiceItems = new InvoiceItems();
                invoiceItems.InvoiceID = Convert.ToInt32(Invoice);
                ViewBag.ItemList = null;
                return PartialView("_PartialInvoiceForm", invoiceItems);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddItem(InvoiceItems invoiceItems)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceItems.Add(invoiceItems);
                db.SaveChanges();
                ViewBag.InvId = invoiceItems.InvoiceID;
                var list = db.InvoiceItems.OrderBy(o => o.InvoiceItemsID).Where(i => i.InvoiceID == invoiceItems.InvoiceID).ToList();
                return PartialView("_PartialInvoiceItemsList", list);
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong try again later");
                return RedirectToAction("AddItem");
            }
        }
        ///// Add Items to invoice End
    }
}
