using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Practice.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace MVC_Practice.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private ApplicationRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); }
        }
        
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ApplicationRole role)
        {

            IdentityResult result = await RoleManager.CreateAsync(new ApplicationRole()
            {
                Name = role.Name,
                Description = role.Description
            });
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
                return HttpNotFound();
            return View(role);


        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            var result = await RoleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Update(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
                return HttpNotFound();
            return View(role);
        }

        [HttpPost, ActionName("Update")]
        public async Task<ActionResult> UpdateConfirmed(ApplicationRole newRole)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(newRole.Id);
                if (role == null)
                    return HttpNotFound();
                role.Name = newRole.Name;
                role.Description = newRole.Description;
                var result = await RoleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

       
    }
}