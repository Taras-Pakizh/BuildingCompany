using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVC_Practice.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC_Practice.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        private ApplicationRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); }
        }

        
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("admin"))
                {
                    return RedirectToAction("Admin");
                }
                else if (User.IsInRole("HR"))
                {
                    return RedirectToAction("HR");
                }
                else if(User.IsInRole("Storage man"))
                {
                    return RedirectToAction("StorageMan");
                }
                else if (User.IsInRole("None"))
                {
                    return RedirectToAction("None");
                }
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

        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "admin, HR")]
        public ActionResult HR()
        {
            return View();
        }

        [Authorize(Roles = "admin, Storage man")]
        public ActionResult StorageMan()
        {
            return View();
        }

        [Authorize(Roles = "admin, None")]
        public ActionResult None()
        {
            return View();
        }
    }
}