using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MVC_Practice.Models.DbModels;

using MVC_Practice.Repository;

namespace MVC_Practice.Controllers
{
    [Authorize(Roles = "admin, HR")]
    public class EmployeeOrdersController : Controller
    {
        DbModels context;

        private SelectList orders;
        private SelectList employees;

        public EmployeeOrdersController() : base()
        {
            context = new DbModels();

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
            //order.eOrderID = context.EmployeeOrders.Max(x => x.eOrderID) + 1;
            order = order.Validate();
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
        public ActionResult Delete(int? id)
        {
            var order = context.EmployeeOrders.Find(id);
            if (employees == null)
                return HttpNotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using (var repository = new Repository<EmployeeOrder>())
            {
                if (await repository.DeleteAsync((int)id))
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
            var order = context.EmployeeOrders.Find(id);
            if (order == null)
                return HttpNotFound();

            orders.Single(x => x.Value == order.orderTypeID.ToString()).Selected = true;
            employees.Single(x => x.Value == order.employeeID.ToString()).Selected = true;

            ViewBag.orders = orders;
            ViewBag.employees = employees;

            return View(order);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(EmployeeOrder order)
        {
            if (order == null)
                return HttpNotFound();
            order = order.Validate();
            using (var repository = new Repository<EmployeeOrder>())
            {
                if (repository.Update(order))
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