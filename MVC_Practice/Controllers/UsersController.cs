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
            if (user.Roles.Count == 0)
            {
                var result = UserManager.AddToRole(user.Id, "None");
            }
            return RoleManager.FindById(user.Roles.First().RoleId).Name;
        }

        public ActionResult Index()
        {
            ViewBag.roles = GetRoleNamesForUsers();

            return View(UserManager.Users);
        }

        [HttpPost]
        public async Task<ActionResult> Add(UserModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user = await UserManager.FindByNameAsync(user.UserName);
                    result = await UserManager.AddToRoleAsync(user.Id, "None");
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return HttpNotFound();
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            string[] role = { GetUserRole(user) };
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
            ViewBag.role = role;

            return View(user);
        }

        [HttpPost, ActionName("Update")]
        public async Task<ActionResult> UpdateConfirmed(UserUpdateModel newUser)
        {
            if (ModelState.IsValid)
            {
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
            return Redirect(Request.UrlReferrer.ToString());
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
            var rolesView = new SelectList(roles, "value", "name");

            var userRole = GetUserRole(user);
            rolesView.Single(x => x.Value == userRole).Selected = true;
            ViewBag.roles = rolesView;

            return View(user);
        }

        [HttpPost, ActionName("UpdateRole")]
        public async Task<ActionResult> UpdateRole(UserRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var roles = await UserManager.GetRolesAsync(model.UserId);
                var result = await UserManager.RemoveFromRolesAsync(model.UserId, roles.ToArray());
                if (result.Succeeded)
                {
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
                else
                {
                    return HttpNotFound();
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}