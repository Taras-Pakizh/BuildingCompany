using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MVC_Practice.Models;

using MVC_Practice.Repository;

namespace MVC_Practice.Controllers
{
    [Authorize(Roles = "admin, HR")]
    public class EmployeeOrdersController : Controller
    {
        MyModels context;

        private SelectList orders;
        private SelectList employees;

        public EmployeeOrdersController() : base()
        {
            context = new MyModels();

            orders = new SelectList(context.OrderTypes, "orderTypeID", "orderName");

            var employeeSelectList = context.Employees.Select(x => new
            {
                id = x.employeeID,
                value = x.firstname + " " + x.lastname
            });

            employees = new SelectList(employeeSelectList, "id", "value");
        }

        public ActionResult Index()
        {
            ViewBag.orders = orders;
            ViewBag.employees = employees;

            return View(context.EmployeeOrders);
        }

        [HttpPost]
        public ActionResult Add(EmployeeOrder order)
        {
            order.orderID = context.EmployeeOrders.Max(x => x.orderID) + 1;
            order.orderDescription = order.orderDescription.Trim();
            if (order.orderDescription.Length > 50)
                order.orderDescription = order.orderDescription.Substring(0, 49);
            using(var repository = new Repository<EmployeeOrder>())
            {
                if (repository.Add(order))
                {
                    repository.Save();
                }
                else return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Open(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var model = context.EmployeeOrders.Find((int)id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Update()
        {
            return View();
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed()
        {
            return View();
        }
    }
}