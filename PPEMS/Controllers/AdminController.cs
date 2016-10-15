using PPEMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using PagedList.EntityFramework;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace PPEMS.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : BaseSecurityController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }









        /// Projects functionality
        public async Task<ActionResult> ProjectsList(int? page, string search)
        {
            if (search == null)
            {
                var list = await db.Projects.OrderBy(i => i.ProjectID).ToPagedListAsync(page ?? 1, 20);
                return View(list);
            }
            else
            {
                var list = await db.Projects.OrderBy(i => i.ProjectID).Where(s => s.Title.Contains(search)).ToPagedListAsync(page ?? 1, 20);
                return View(list);
            }

        }
        public ActionResult CreateProject()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProject(Project pro)
        {
            if (ModelState.IsValid)
            {
                var find = await db.Projects.Where(p => p.Title.ToLower() == pro.Title.ToLower() || p.ReferenceNo.ToLower() == pro.ReferenceNo.ToLower()).FirstOrDefaultAsync();
                if (find != null)
                {
                    ModelState.AddModelError("", "Project already exist for same title or refrence #");
                    return View(pro);
                }
                db.Projects.Add(pro);
                await db.SaveChangesAsync();
                ModelState.AddModelError("", "Project Created Successfully");
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Input fields are not valid");
                return View(pro);
            }
        }
        public async Task<ActionResult> ProjectDetails(int? id)
        {
            var find = await db.Projects.Where(i => i.ProjectID == id).FirstOrDefaultAsync();
            return View(find);
        }
        /// Projects functionality End












        /// Quotations
        public ActionResult QuotationsCreate()
        {
            ViewBag.Prolist = db.Projects.OrderBy(o => o.ProjectID).ToList().Select(s => new SelectListItem { Value = s.ProjectID.ToString(), Text = s.Title }).ToList();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> QuotationsCreate(Quotation quo)
        {
            ViewBag.Prolist = db.Projects.OrderBy(o => o.ProjectID).ToList().Select(s => new SelectListItem { Value = s.ProjectID.ToString(), Text = s.Title }).ToList();
            if (ModelState.IsValid)
            {
                var find = await db.Quotations.Where(p => p.ReferenceNo.ToLower() == quo.ReferenceNo.ToLower()).FirstOrDefaultAsync();
                if (find != null)
                {
                    ModelState.AddModelError("", "Quotation already exist for refrence #");
                    return View(quo);
                }
                db.Quotations.Add(quo);
                db.SaveChanges();
                ModelState.AddModelError("", "Quotation Created Successfully");
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Input fields are not valid");
                return View(quo);
            }
        }
        /// Quotations End







        /// Quotations Details
        public ActionResult QuotationsDetailCreate()
        {
            ViewBag.Pro = db.Projects.OrderBy(o => o.ProjectID).ToList().Select(s => new SelectListItem { Value = s.ProjectID.ToString(), Text = s.Title }).ToList();
            return View();
        }
        public ActionResult SelectQuo(int? Project)
        {
            ViewBag.Quolist = db.Quotations.OrderBy(o => o.QuotationID).Where(q => q.ProjectID == Project).ToList().Select(s => new SelectListItem { Value = s.QuotationID.ToString(), Text = s.ReferenceNo }).ToList();
            return PartialView("~/Views/Admin/_PartialQuo.cshtml");
        }
        public ActionResult SelectQuoDetailF(int? Quotation)
        {
            var id = db.QuotationsDetails.Where(i => i.QuotationID == Quotation).FirstOrDefault();
            if (id != null)
            {
                QuotationDetails model = new QuotationDetails();
                model.QuotationID = id.QuotationID;
                ViewBag.QuotionList = db.QuotationsDetails.OrderBy(o => o.QuotationDetailsID).Where(i => i.QuotationID == model.QuotationID).ToList();
                return PartialView("_PartialQuoDetailsForm", model);
            }
            else
            {
                QuotationDetails model = new QuotationDetails();
                model.QuotationID = Convert.ToInt32(Quotation);
                ViewBag.QuotionList = null;
                return PartialView("_PartialQuoDetailsForm", model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult QuotationsDetailCreate(QuotationDetails quod)
        {
            if (ModelState.IsValid)
            {
                db.QuotationsDetails.Add(quod);
                db.SaveChanges();
                ViewBag.QuoID = quod.QuotationID;
                var list = db.QuotationsDetails.OrderBy(o => o.QuotationDetailsID).Where(i => i.QuotationID == quod.QuotationID).ToList();
                return PartialView("_PartialListQuoDetail", list);
            }
            else
            {
                //return Json(false);
                return View();
            }
        }
        /// Quotations Details End
        




        public FileResult QuoItem(int id,string ReportType)
        {
            LocalReport lr = new LocalReport();
            lr.ReportPath= Server.MapPath("~/Content/QuoItemReport.rdlc");

            ReportDataSource rd = new ReportDataSource();
            rd.Name = "QtoItemDetail";
            rd.Value = db.QuotationsDetails.Where(i => i.QuotationID == id).ToList();

            lr.DataSources.Add(rd);
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                "",
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        public FileResult InvItem(int id, string ReportType)
        {
            LocalReport lr = new LocalReport();
            lr.ReportPath = Server.MapPath("~/Content/InvoiceItem.rdlc");

            ReportDataSource rd = new ReportDataSource();
            rd.Name = "InvoiceItem";
            rd.Value = db.InvoiceItems.Where(i => i.InvoiceID == id).ToList();

            lr.DataSources.Add(rd);
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                "",
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        public FileResult ChallanItem(int id, string ReportType)
        {
            LocalReport lr = new LocalReport();
            lr.ReportPath = Server.MapPath("~/Content/ChallanItem.rdlc");

            ReportDataSource rd = new ReportDataSource();
            rd.Name = "ChallanItem";
            rd.Value = db.ChallanItems.Where(i => i.ChallanID == id).ToList();

            lr.DataSources.Add(rd);
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                "",
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }

    }
}