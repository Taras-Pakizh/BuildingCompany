using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVC_Practice.Models.Identity;
using MVC_Practice.Models.ViewModels;
using MVC_Practice.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;

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
            var tabCreator = new TabCreator("admin");
            ViewBag.tabs = tabCreator.GetTabs;

            return View();
        }

        [Authorize(Roles = "admin, HR")]
        public ActionResult HR()
        {
            var tabCreator = new TabCreator("HR");
            ViewBag.tabs = tabCreator.GetTabs;

            return View();
        }

        [Authorize(Roles = "admin, Storage man")]
        public ActionResult StorageMan()
        {
            var tabCreator = new TabCreator("Storage man");
            ViewBag.tabs = tabCreator.GetTabs;

            return View();
        }

        [Authorize(Roles = "admin, None")]
        public ActionResult None()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public void Generate(string htmlValue)
        {
            var css = PdfGenerator.ParseStyleSheet(System.IO.File.ReadAllText(Server.MapPath("~/Content/bootstrap.min.css")));
            var msPDF = PdfSharpConvert(htmlValue, css);

            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=myPdf.pdf");
            Response.BinaryWrite(msPDF.ToArray());

            Response.End();
        }

        public static MemoryStream PdfSharpConvert(string html, CssData css)
        {
            using (var file = new MemoryStream())
            {
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4, 25, css);
                pdf.Save(file);
                return file;
            }
        }
    }
}