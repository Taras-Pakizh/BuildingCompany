﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

using CourseworkBD.DAL.DbContext;
using CourseworkBD.DAL.Models;
//using MVC_Practice.Models.DbModels;

//using MVC_Practice.Models.Identity;
using MVC_Practice.Repository;

using Microsoft.AspNet.Identity.Owin;

namespace MVC_Practice.Controllers
{
    [Authorize(Roles = "admin, HR")]
    public class EmployeesController : Controller
    {


        private CourseworkDBContext context;

        private SelectList positions;
        private SelectList departments;
        
        public EmployeesController():base()
        {
            context = new CourseworkDBContext();
           
            positions = new SelectList(context.Positions, "positionID", "positionName");
            departments = new SelectList(context.Departments, "Id", "Name");

            var tabCreator = new TabCreator("HR");
            tabCreator.ChooseTab("Employees");
            ViewBag.tabs = tabCreator.GetTabs;
        }

        public ActionResult Index()
        {
            ViewBag.positions = positions;
            ViewBag.departments = departments;

            return View("~/Views/Employees/EmployeePanel.cshtml", context.Employees);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Employee employee, int positionID, int DepartmentId)
        {
            
            using(var repository = new Repository<Employee>())
            {
                employee.Department = repository.context.Departments.Find(DepartmentId);
                employee.Position = repository.context.Positions.Find(positionID);
                if (repository.Add(employee))
                {
                    await repository.SaveAsync();
                }
                else
                {
                    return HttpNotFound();
                }
            }

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
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using (var repository = new Repository<Employee>())
            {
                if(await repository.DeleteAsync((int)id))
                {
                    await repository.SaveAsync();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            var employee = context.Employees.Find(id);
            if (employee == null)
                return HttpNotFound();

            positions.Single(x => x.Value == employee.Position.positionID.ToString()).Selected = true;
            departments.Single(x => x.Value == employee.Department.Id.ToString()).Selected = true;

            ViewBag.positions = positions;
            ViewBag.departments = departments;

            return View(employee);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(Employee employee, int positionID, int DepartmentId)
        {
            if (employee == null)
                return HttpNotFound();
            using(var repository = new Repository<Employee>())
            {
                employee.Department = repository.context.Departments.Find(DepartmentId);
                employee.Position = repository.context.Positions.Find(positionID);
                if (repository.Update(employee))
                {
                    repository.Save();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Index");
        }
    }
}