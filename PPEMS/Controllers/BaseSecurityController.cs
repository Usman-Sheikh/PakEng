using Microsoft.AspNet.Identity.Owin;
using PPEMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPEMS.Controllers
{
    [Authorize]
    public class BaseSecurityController : Controller
    {
        internal ApplicationSignInManager _signInManager;
        internal ApplicationUserManager _userManager;
        internal PPEMSContext db;

        public BaseSecurityController()
        {
            db = new PPEMSContext();
        }

        public BaseSecurityController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}