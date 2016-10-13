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
    public class PaymentsController : BaseSecurityController
    {

        // GET: Payments
        public async Task<ActionResult> Index()
        {
            var employee = await db.Employee.ToListAsync();
            return View(employee);
        }

        public async Task<ActionResult> PaymentList(int? id)
        {
            var em = await db.Payment.Where(i => i.EmployeeID == id).ToListAsync();
            return View(em);
        }
        
        public async Task<ActionResult> Create()
        {
            ViewBag.EmployeeID = await db.Employee.OrderBy(o => o.EmployeeID).Select(s => new SelectListItem { Value = s.EmployeeID.ToString(), Text = s.Name }).ToListAsync(); /*new SelectList(db.Employee, "EmployeeID", "Name");*/
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.PaymentDate = DateTime.Now;
                db.Payment.Add(payment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = await db.Employee.OrderBy(o => o.EmployeeID).Select(s => new SelectListItem { Value = s.EmployeeID.ToString(), Text = s.Name }).ToListAsync();
            return View(payment);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payment.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", payment.EmployeeID);
            return View(payment);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", payment.EmployeeID);
            return View(payment);
        }
    }
}
