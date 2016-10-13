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
    public class ChallansController : BaseSecurityController
    {
        public ActionResult Index()
        {
            var challans = db.Challans.Include(c => c.Project);
            return View(challans.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.ProjectID = db.Projects.OrderBy(o => o.ProjectID).Select(s => new SelectListItem { Value = s.ProjectID.ToString(), Text = s.Title }).ToList();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Challan challan)
        {
            ViewBag.ProjectID = await db.Projects.OrderBy(o => o.ProjectID).Select(s => new SelectListItem { Value = s.ProjectID.ToString(), Text = s.Title }).ToListAsync();
            if (ModelState.IsValid)
            {
                var check = await db.Challans.Where(i => i.ChallanNo == challan.ChallanNo).CountAsync();
                if (check > 0)
                {
                    ModelState.AddModelError("", "Challan alrady exixts on this no");
                    return View(challan);
                }
                db.Challans.Add(challan);
                await db.SaveChangesAsync();
                ModelState.AddModelError("", "Challan created successfully");
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Input fields are not valid");
                return View(challan);
            }
        }



        public ActionResult AddItem()
        {
            ViewBag.Pro = db.Projects.OrderBy(o => o.ProjectID).ToList().Select(s => new SelectListItem { Value = s.ProjectID.ToString(), Text = s.Title }).ToList();
            return View();
        }
        public ActionResult ChallanSelect(int? Project)
        {
            ViewBag.Challan = db.Challans.OrderBy(o => o.ChallanID).Where(i => i.ProjectID == Project).Select(s => new SelectListItem { Value = s.ChallanID.ToString(), Text = s.ChallanNo }).ToList();
            return PartialView("_PartialChallanSelect");
        }

        public ActionResult ChallaItemForm(int? Challan)
        {
            var id = db.ChallanItems.Where(i => i.ChallanID == Challan).FirstOrDefault();
            if (id != null)
            {
                ChallanItems challanItem = new ChallanItems();
                challanItem.ChallanID = id.ChallanID;
                ViewBag.ItemList = db.ChallanItems.OrderBy(o => o.ChallanItemsID).Where(i => i.ChallanID == challanItem.ChallanID).ToList();
                return PartialView("_PartialChallaItemForm", challanItem);
            }
            else
            {
                ChallanItems challanItem = new ChallanItems();
                challanItem.ChallanID = Convert.ToInt32(Challan);
                ViewBag.ItemList = null;
                return PartialView("_PartialChallaItemForm", challanItem);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddItem(ChallanItems item)
        {
            if (ModelState.IsValid)
            {
                db.ChallanItems.Add(item);
                db.SaveChanges();
                ViewBag.ChallanId = item.ChallanID;
                var list = db.ChallanItems.OrderBy(o => o.ChallanItemsID).Where(i => i.ChallanID == item.ChallanID).ToList();
                return PartialView("_PartialChallanItemsList", list);
            }
            else
            {
                //return Json(false);
                return View();
            }
        }

    }
}
