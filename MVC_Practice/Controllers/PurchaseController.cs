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
    [Authorize(Roles = "admin, Storage man")]
    public class PurchaseController : Controller
    {
        private DbModels context;

        private SelectList suppliers;
        private SelectList resources;

        public PurchaseController() : base()
        {
            context = new DbModels();

            suppliers = new SelectList(context.Suppliers, "supplierID", "supplierName");
            resources = new SelectList(context.Resources, "resourceID", "resourceName");
        }

        public ActionResult Index()
        {
            ViewBag.suppliers = suppliers;

            var isStoraged = new List<bool>();

            foreach(var order in context.DeliveryOrders)
            {
                foreach(var item in order.DeliverysContents)
                {
                    var shipments = context.ShipmentToStorages.Where(x => x.contentID == item.contentID);
                    if (shipments.Count() == 0 || shipments.Sum(x => x.resourceAmount) < item.contentAmount)
                    {
                        isStoraged.Add(false);
                        break;
                    }
                }
                isStoraged.Add(true);
            }

            ViewBag.isStoraged = isStoraged;

            return View(context.DeliveryOrders);
        }

        [HttpGet]
        public async Task<ActionResult> Open(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            var order = await context.DeliveryOrders.FindAsync(id);
            var items = context.DeliverysContents.Where(x => x.deliveryOrderID == (int)id);

            ViewBag.items = items;
            ViewBag.resources = resources;

            return View(order);
        }

        [HttpPost]
        public async Task<ActionResult> Add(DeliveryOrder model)
        {
            using(var repository = new Repository<DeliveryOrder>())
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
            return RedirectToAction("Open", new { id = context.DeliveryOrders.Max(x=>x.deliveryOrderID) });
        }

        [HttpPost]
        public async Task<ActionResult> AddItem(DeliverysContent model)
        {
            using(var repository = new Repository<DeliverysContent>())
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
            return RedirectToAction("Open", new { id = model.deliveryOrderID });
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
            var order = context.DeliveryOrders.Find(id);
            if (order == null)
                return HttpNotFound();

            suppliers.Single(x => x.Value == order.supplierID.ToString()).Selected = true;

            ViewBag.suppliers = suppliers;

            return View(order);
        }

        [HttpGet]
        public ActionResult UpdateItem(int? id)
        {
            var item = context.DeliverysContents.Find(id);
            if (item == null)
                return HttpNotFound();

            resources.Single(x => x.Value == item.resourceID.ToString()).Selected = true;

            ViewBag.resources = resources;

            return View(item);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(DeliveryOrder model)
        {
            if (ModelState.IsValid)
            {
                using (var repository = new Repository<DeliveryOrder>())
                {
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
        public ActionResult UpdateItemConfirmed(DeliverysContent model)
        {
            if (ModelState.IsValid)
            {
                using(var repository = new Repository<DeliverysContent>())
                {
                    if (repository.Update(model))
                    {
                        repository.Save();
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                return RedirectToAction("Open", new { id = model.deliveryOrderID });
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var order = context.DeliveryOrders.Find(id);
            if (order == null)
                return HttpNotFound();
            ViewBag.orders = context.DeliverysContents.Where(x => x.deliveryOrderID == id);
            return View(order);
        }

        [HttpGet]
        public ActionResult DeleteItem(int? id)
        {
            var item = context.DeliverysContents.Find(id);
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
            using (var repository = new Repository<DeliverysContent>())
            {
                if (await repository.DeleteAsync((int)id))
                {
                    returnId = context.DeliverysContents.Find(id).deliveryOrderID;
                    await repository.SaveAsync();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Open", new { id = returnId});
        }
    }
}