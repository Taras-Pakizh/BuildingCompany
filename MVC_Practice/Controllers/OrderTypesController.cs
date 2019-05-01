using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CourseworkBD.DAL.DbContext;
using CourseworkBD.DAL.Models;
//using MVC_Practice.Models.DbModels;
using MVC_Practice.Repository;
using System.Threading.Tasks;

namespace MVC_Practice.Controllers
{
    [Authorize(Roles = "admin, HR")]
    public class OrderTypesController : Controller
    {
        CourseworkDBContext context;

        public OrderTypesController() : base()
        {
            context = new CourseworkDBContext();

            var tabCreator = new TabCreator("HR");
            tabCreator.ChooseTab("Order types");
            ViewBag.tabs = tabCreator.GetTabs;
        }

        public ActionResult Index()
        {
            return View(context.OrderTypes);
        }

        [HttpPost]
        public ActionResult Add(OrderType orderType)
        {
            //orderType.orderTypeID = context.OrderTypes.Max(x => x.orderTypeID) + 1;
            using (var repository = new Repository<OrderType>())
            {
                if (repository.Add(orderType))
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
            var orderType = context.OrderTypes.Find(id);
            if (orderType == null)
                return HttpNotFound();
            return View(orderType);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using (var repository = new Repository<OrderType>())
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
            var orderType = context.OrderTypes.Find(id);
            if (orderType == null)
                return HttpNotFound();
            return View(orderType);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(OrderType orderType)
        {
            if (orderType == null)
                return HttpNotFound();
            using (var repository = new Repository<OrderType>())
            {
                if (repository.Update(orderType))
                {
                    repository.Save();
                }
                else repository.Save();
            }
            return RedirectToAction("Index");
        }
    }
}