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
    public class UsersController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }
        private ApplicationRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); }
        }

        private string[] GetRoleNamesForUsers()
        {
            var users = UserManager.Users.ToList();
            var roleIds = users.Select(x => 
            {
                if (x.Roles.Count > 0)
                {
                    return x.Roles.ToList().First().RoleId;
                }
                else return "None";
            });
            return roleIds.Select(x => 
            {
                if (x == "None")
                    return x;
                return RoleManager.FindById(x).Name;
            }).ToArray();
        }

        private string GetUserRole(ApplicationUser user)
        {
            string role = null;
            if (user.Roles.Count > 0)
            {
                role = RoleManager.FindById(user.Roles.First().RoleId).Name;
            }
            return role;
        }

        public ActionResult Index()
        {
            ViewBag.roles = GetRoleNamesForUsers();

            return View(UserManager.Users);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            string[] role = { GetUserRole(user) };
            if (role.First() == null)
                role[0] = "None";
            ViewBag.role = role;

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var result = await UserManager.DeleteAsync(user);
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
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            string[] role = { GetUserRole(user) };
            if (role.First() == null)
                role[0] = "None";
            ViewBag.role = role;

            return View(user);
        }

        [HttpPost, ActionName("Update")]
        public async Task<ActionResult> UpdateConfirmed(ApplicationUser newUser)
        {
            if(newUser == null || newUser.UserName == null || newUser.Email == null)
                return Redirect(Request.UrlReferrer.ToString());
            
            var user = await UserManager.FindByIdAsync(newUser.Id);
            if (user == null)
                return HttpNotFound();
            user.UserName = newUser.UserName;
            user.Email = newUser.Email;
            var result = await UserManager.UpdateAsync(user);
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
        public async Task<ActionResult> UpdateRole(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            var roles = RoleManager.Roles.ToList().Select(x => new
            {
                name = x.Name,
                value = x.Name
            }).ToList();
            roles.Add(new { name = "None", value = "None" });
            var rolesView = new SelectList(roles, "value", "name");

            var userRole = GetUserRole(user);
            if (userRole == null)
                userRole = "None";
            rolesView.Single(x => x.Value == userRole).Selected = true;
            ViewBag.roles = rolesView;

            return View(user);
        }

        [HttpPost, ActionName("UpdateRole")]
        public async Task<ActionResult> UpdateRole(UserRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var roleNames = RoleManager.Roles.Select(x => x.Name).ToArray();
                var result = await UserManager.RemoveFromRolesAsync(model.UserId, roleNames);
                result = await UserManager.AddToRoleAsync(model.UserId, model.RoleName);
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