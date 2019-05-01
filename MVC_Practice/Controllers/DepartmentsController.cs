using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using CourseworkBD.DAL.DbContext;
using CourseworkBD.DAL.Models;
//using MVC_Practice.Models.DbModels;
using MVC_Practice.Repository;

namespace MVC_Practice.Controllers
{
    [Authorize(Roles ="admin, HR")]
    public class DepartmentsController : Controller
    {
        CourseworkDBContext context;

        public DepartmentsController() : base()
        {
            context = new CourseworkDBContext();

            var tabCreator = new TabCreator("HR");
            tabCreator.ChooseTab("Departments");
            ViewBag.tabs = tabCreator.GetTabs;
        }

        public ActionResult Index()
        {
            return View(context.Departments);
        }

        [HttpPost]
        public ActionResult Add(Department department)
        {
            //department.departmentID = context.Departments.Max(x => x.departmentID) + 1;
            using (var repository = new Repository<Department>())
            {
                if (repository.Add(department))
                {
                    repository.Save();
                }
                else return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var department = context.Departments.Find(id);
            if (department == null)
                return HttpNotFound();
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using (var repository = new Repository<Department>())
            {
                if (await repository.DeleteAsync((int)id))
                {
                    await repository.SaveAsync();
                }
                else return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            var department = context.Departments.Find(id);
            if (department == null)
                return HttpNotFound();
            return View(department);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(Department department)
        {
            if (department == null)
                return HttpNotFound();
            using (var repository = new Repository<Department>())
            {
                if (repository.Update(department))
                {
                    repository.Save();
                }
                else repository.Save();
            }
            return RedirectToAction("Index");
        }
    }
}