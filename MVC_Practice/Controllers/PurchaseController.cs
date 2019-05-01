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
    [Authorize(Roles = "admin, Storage man")]
    public class PurchaseController : Controller
    {
        private CourseworkDBContext context;

        private SelectList suppliers;
        private SelectList resources;

        public PurchaseController() : base()
        {
            context = new CourseworkDBContext();

            suppliers = new SelectList(context.Suppliers, "Id", "Name");
            resources = new SelectList(context.Resources, "Id", "Name");

            var tabCreator = new TabCreator("Storage man");
            tabCreator.ChooseTab("Purchases");
            ViewBag.tabs = tabCreator.GetTabs;
        }

        public ActionResult Index()
        {
            ViewBag.suppliers = suppliers;

            var isStoraged = new List<bool>();

            foreach(var order in context.Orders.ToList())
            {
                isStoraged.Add(true);
                foreach (var item in order.Content.ToList())
                {
                    var shipments = context.ShipmentToStorages.Where(x => x.contentID == item.Id);
                    if (shipments.Count() == 0 || shipments.Sum(x => x.resourceAmount) < item.Amount)
                    {
                        isStoraged[isStoraged.Count - 1] = false;
                        break;
                    }
                }
            }

            ViewBag.isStoraged = isStoraged;

            return View(context.Orders);
        }

        [HttpGet]
        public async Task<ActionResult> Open(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            var order = await context.Orders.FindAsync(id);
            var items = context.DeliveryContents.Where(x => x.Order.Id == (int)id);

            ViewBag.items = items;
            ViewBag.resources = resources;

            return View(order);
        }

        [HttpPost]
        public async Task<ActionResult> Add(DeliveryOrder model, int Supplier_Id)
        {
            using(var repository = new Repository<DeliveryOrder>())
            {
                model.Supplier = repository.context.Suppliers.Find(Supplier_Id);
                if (repository.Add(model))
                {
                    await repository.SaveAsync();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Open", new { id = context.Orders.Max(x=>x.Id) });
        }

        [HttpPost]
        public async Task<ActionResult> AddItem(Content model, int Resource_Id, int DeliveryOrder_Id)
        {
            using(var repository = new Repository<Content>())
            {
                model.Resource = repository.context.Resources.Find(Resource_Id);
                model.Order = repository.context.Orders.Find(DeliveryOrder_Id);
                if (repository.Add(model))
                {
                    await repository.SaveAsync();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Open", new { id = DeliveryOrder_Id });
        }

        [HttpPost]
        public async Task<ActionResult> AddSupplier(Supplier model)
        {
            using (var repository = new Repository<Supplier>())
            {
                if (repository.Add(model))
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

        [HttpPost]
        public async Task<ActionResult> AddResource(Resource model)
        {
            using (var repository = new Repository<Resource>())
            {
                if (repository.Add(model))
                {
                    await repository.SaveAsync();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            var order = context.Orders.Find(id);
            if (order == null)
                return HttpNotFound();

            suppliers.Single(x => x.Value == order.Supplier.Id.ToString()).Selected = true;

            ViewBag.suppliers = suppliers;

            return View(order);
        }

        [HttpGet]
        public ActionResult UpdateItem(int? id)
        {
            var item = context.DeliveryContents.Find(id);
            if (item == null)
                return HttpNotFound();

            resources.Single(x => x.Value == item.Resource.Id.ToString()).Selected = true;

            ViewBag.resources = resources;

            return View(item);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(DeliveryOrder model, int Supplier_Id)
        {
            if (ModelState.IsValid)
            {
                using (var repository = new Repository<DeliveryOrder>())
                {
                    model.Supplier = repository.context.Suppliers.Find(Supplier_Id);
                    if (repository.Update(model))
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
            return HttpNotFound();
        }

        [HttpPost, ActionName("UpdateItem")]
        public ActionResult UpdateItemConfirmed(Content model, int Resource_Id, int DeliveryOrder_Id)
        {
            if (ModelState.IsValid)
            {
                using(var repository = new Repository<Content>())
                {
                    model.Resource = repository.context.Resources.Find(Resource_Id);
                    model.Order = repository.context.Orders.Find(DeliveryOrder_Id);
                    if (repository.Update(model))
                    {
                        repository.Save();
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                return RedirectToAction("Open", new { id = model.Order.Id });
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var order = context.Orders.Find(id);
            if (order == null)
                return HttpNotFound();
            ViewBag.orders = context.DeliveryContents.Where(x => x.Order.Id == id);
            return View(order);
        }

        [HttpGet]
        public ActionResult DeleteItem(int? id)
        {
            var item = context.DeliveryContents.Find(id);
            if (item == null)
                return HttpNotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using(var repository = new Repository<DeliveryOrder>())
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

        [HttpPost, ActionName("DeleteItem")]
        public async Task<ActionResult> DeleteItemConfirmed(int? id)
        {
            if (id == null)
                return HttpNotFound();
            int? returnId = null;
            using (var repository = new Repository<Content>())
            {
                if (await repository.DeleteAsync((int)id))
                {
                    returnId = context.DeliveryContents.Find(id).Order.Id;
                    await repository.SaveAsync();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Open", new { id = returnId});
        }

        [HttpGet]
        public ActionResult OpenPDF(int? id)
        {
            var model = context.Orders.Find(id);
            if (model == null)
                return HttpNotFound();
            return View(model);
        }
    }
}