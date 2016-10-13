using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PPEMS.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var roles = ApplicationRoleManager.Create(HttpContext.GetOwinContext());

            if (!await roles.RoleExistsAsync("admin"))
            {
                await roles.CreateAsync(new IdentityRole { Name = "admin" });
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> Contact(string name,string email, string comments)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p> <p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("pakenggeneering@gmail.com"));
                message.From = new MailAddress(email);
                message.Subject = "User Message";
                message.Body = string.Format(body, name, email, comments);
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
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}