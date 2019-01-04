using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MVC_Practice.Models;

namespace MVC_Practice.Controllers
{
    public class EmployeesController : Controller
    {
        private MyModels context;

        private SelectList positions;
        private SelectList departments;
        
        public EmployeesController():base()
        {
            context = new MyModels();
            positions = new SelectList(context.Positions, "positionID", "positionName");
            departments = new SelectList(context.Departments, "departmentID", "dname");
        }

        // GET: Employees
        public ActionResult Index()
        {
            ViewBag.positions = positions;
            ViewBag.departments = departments;

            return View("~/Views/Employees/EmployeePanel.cshtml", context.Employees);
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            employee.employeeID = context.Employees.Max(x => x.employeeID) + 1;
            context.Employees.Add(employee);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var employee = context.Employees.Find(id);
            if (employee == null)
                return HttpNotFound();
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            var remove = context.Employees.Find(id);
            if (remove == null)
                return HttpNotFound();
            context.Employees.Remove(remove);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            var employee = context.Employees.Find(id);
            if (employee == null)
                return HttpNotFound();

            positions.Single(x => x.Value == employee.Position1.positionID.ToString()).Selected = true;
            departments.Single(x => x.Value == employee.Department1.departmentID.ToString()).Selected = true;

            ViewBag.positions = positions;
            ViewBag.departments = departments;

            return View(employee);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(Employee employee)
        {
            if (employee == null)
                return HttpNotFound();
            context.Entry(employee).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}