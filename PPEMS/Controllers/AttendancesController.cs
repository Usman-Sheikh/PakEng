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
    public class AttendancesController : BaseSecurityController
    {
        public ActionResult Index()
        {
            var attendance = db.Attendance.Include(a => a.Employee);
            return View(attendance.ToList());
        }

        public ActionResult Create()
        {
            //ViewBag.EmployeeID = new SelectList(db.Employee, "EmployeeID", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                var date = DateTime.Now.ToString("d");
                var get = await db.Attendance.ToListAsync();
                if (get != null)
                {
                    var check = get.Where(a=>a.AttendanceDate.ToString("d")==date.ToString()).FirstOrDefault();
                    if (check!=null)
                    {
                        ModelState.AddModelError("", "Attendacne Already Exists for today pleasy go to Mark attendance");
                        return View();
                    }
                }
                var empList = db.Employee.Select(s => s.EmployeeID).ToList();
                foreach (var item in empList)
                {
                    var att = new Attendance
                    {
                        AttendanceDate = DateTime.Now.Date,
                        EmployeeID = item,
                        MarkAttendance = "A"
                    };
                    db.Attendance.Add(att);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("FetchAttnedance");
            }

            ViewBag.EmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", attendance.EmployeeID);
            return View(attendance);
        }





        public async Task<ActionResult> MarkAttnedance()
        {
            var list = await db.Attendance.Select(s => s.AttendanceDate).Distinct().ToListAsync();
            List<SelectListItem> dl = new List<SelectListItem>();
            foreach (var item in list)
            {
                dynamic n = item.Date.ToString("d");
                dl.Add(new SelectListItem { Value = n, Text = n });
            }
            ViewBag.list = dl;
            return View();
        }
        public async Task<ActionResult> GetEmp(DateTime? AttendanceDate)
        {
            var list = await db.Attendance.Where(a => a.AttendanceDate == AttendanceDate).ToListAsync();
            //if(list)
            return View("_PartialGetEmp", list);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> MarkAttnedance(IEnumerable<int> Id)
        {
            try
            {
                db.Attendance.Where(a => Id.Contains(a.AttendanceID)).ToList().ForEach(a => a.MarkAttendance = "P");
                await db.SaveChangesAsync();
                return RedirectToAction("FetchAttnedance");
            }
            catch
            {
                ModelState.AddModelError("", "Some thing Wend Wrong");
                return RedirectToAction("FetchAttnedance");
            }
        }


        public async Task<ActionResult> FetchAttnedance()
        {
            var list = await db.Attendance.Select(s => s.AttendanceDate).Distinct().ToListAsync();
            List<string> dl = new List<string>();
            dynamic n = null;
            foreach (var item in list)
            {
                n = item.Date.ToString("Y");
                dl.Add(n);
            }

            var c = dl.Distinct().ToList();

            //List<SelectListItem> li = new List<SelectListItem>();
            //foreach(var item in c)
            //{
            //    li = new SelectListItem { Value = c, Text = c };
            //}
            ViewBag.list = new SelectList(c);
            return View();
        }
        public async Task<ActionResult> GetAtt(string Month)
        {
            var list = await db.Attendance.ToListAsync();
            var dateGet = list.Where(a => a.AttendanceDate.ToString("Y") == Month).ToList();
            //// Date Distinct
            List<string> date = new List<string>();
            foreach (var item in dateGet)
            {
                var s = item.AttendanceDate.ToString("d");
                date.Add(s);
            }
            ViewBag.Date = date.Distinct().ToList();
            //// Date Distinct End

            //// Name Distinct
            List<string> name = new List<string>();
            foreach (var item in dateGet)
            {
                var s = item.Employee.Name;
                name.Add(s);
            }
            var getatte = list.Where(a => a.AttendanceDate.ToString("Y") == Month && name.Contains(a.Employee.Name)).ToList();
            //getatte.d
            ViewBag.Name = name.Distinct().ToList();
            //// Name Distinct End

            return View("_PartialGetAtt", getatte);
        }





        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendance.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", attendance.EmployeeID);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employee, "EmployeeID", "Name", attendance.EmployeeID);
            return View(attendance);
        }

    }
}
