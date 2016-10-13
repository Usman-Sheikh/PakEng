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
using PagedList.EntityFramework;

namespace PPEMS.Controllers
{
    [Authorize(Roles = "admin")]
    public class VendorsController : BaseSecurityController
    {

        public async Task<ActionResult> Index(int? page, string search)
        {

            if (search == null)
            {
                var list = await db.Vendors.OrderBy(i => i.VendorID).ToPagedListAsync(page ?? 1, 20);
                return View(list);
            }
            else
            {
                var list = await db.Vendors.OrderBy(i => i.VendorID).Where(s => s.Name.Contains(search)).ToPagedListAsync(page ?? 1, 20);
                return View(list);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                var find = await db.Vendors.Where(v => v.CompanyTitle.ToLower() == vendor.CompanyTitle.ToLower()).FirstOrDefaultAsync();
                if (find != null)
                {
                    ModelState.AddModelError("", "Vendor already exists with same company title");
                    return View(vendor);
                }
                db.Vendors.Add(vendor);
                await db.SaveChangesAsync();
                ModelState.AddModelError("", "Challan created successfully");
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Input fields are not valid");
                return View(vendor);
            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendor);
        }


    }
}
