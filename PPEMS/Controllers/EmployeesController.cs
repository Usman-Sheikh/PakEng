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
    public class EmployeesController : BaseSecurityController
    {

        public async Task<ActionResult> Index(int? page, string search)
        {
            if (search == null)
            {
                var list = await db.Employee.OrderBy(i => i.EmployeeID).ToPagedListAsync(page ?? 1, 20);
                return View(list);
            }
            else
            {
                var list = await db.Employee.OrderBy(i => i.EmployeeID).Where(s => s.Name.Contains(search)).ToPagedListAsync(page ?? 1, 20);
                return View(list);
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var check = await db.Employee.Where(e => e.Name.ToLower() == employee.Name.ToLower() && e.ContactNo == employee.ContactNo).FirstOrDefaultAsync();
                if (check == null)
                {
                    db.Employee.Add(employee);
                    await db.SaveChangesAsync();
                    var getdate = DateTime.Now.ToString("Y");
                    var attlist = db.Attendance.ToList();
                    var getAttDateList = attlist.Where(a => a.AttendanceDate.ToString("Y") == getdate).Select(s => s.AttendanceDate).Distinct().ToList();
                    if (getAttDateList.Count > 0)
                    {
                        var findemp = await db.Employee.FirstOrDefaultAsync(e => e.Address.ToLower() == employee.Address
                        && e.ContactNo == employee.ContactNo
                        && e.Designation.ToLower() == employee.Designation.ToLower()
                        && e.Name.ToLower() == employee.Name.ToLower());
                        foreach (var item in getAttDateList)
                        {
                            var attCreate = new Attendance
                            {
                                AttendanceDate = item,
                                EmployeeID = findemp.EmployeeID,
                                MarkAttendance = "N/A"
                            };
                            db.Attendance.Add(attCreate);
                        }
                        await db.SaveChangesAsync();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Employe Already exists for same name and contact");
                    return View(employee);
                }
            }

            return View(employee);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }
    }
}
