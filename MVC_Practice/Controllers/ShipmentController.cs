using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVC_Practice.Models.DbModels;
using MVC_Practice.Repository;
using MVC_Practice.Models.ViewModels;

using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MVC_Practice.Controllers
{
    [Authorize(Roles = "admin, Storage man")]
    public class ShipmentController : Controller
    {
        private DbModels context;

        private SelectList storages;

        public ShipmentController() : base()
        {
            context = new DbModels();
            
            storages = new SelectList(context.Storages, "storageID", "storageAddres");
        }

        [HttpGet]
        public ActionResult Storages()
        {
            return View(context.Storages);
        }

        [HttpGet]
        public ActionResult OpenStorage(int? id)
        {
            var storage = context.Storages.Find(id);
            if (storage == null)
                return HttpNotFound();

            ViewBag.resources = context.Resources_Storages.Where(x => x.storageID == id && x.resourceAmount != 0);

            return View(storage);
        }

        [HttpGet]
        public ActionResult OpenOrder(int? id)
        {
            var order = context.DeliveryOrders.Find(id);
            if (order == null)
                return HttpNotFound();

            
            var items = context.DeliverysContents.Where(x => x.deliveryOrderID == id);
            
            var itemsCheck = new List<bool>();

            foreach(var item in items)
            {
                var shipments = context.ShipmentToStorages.Where(x => x.contentID == item.contentID);
                if (shipments.Count() == 0 || shipments.Sum(s => s.resourceAmount) < item.contentAmount)
                {
                    itemsCheck.Add(false);
                }
                else itemsCheck.Add(true);
            }

            ViewBag.storages = storages;
            ViewBag.items = items;
            ViewBag.isItemsStored = itemsCheck;

            var count = context.ShipmentToStorages.Where(x => x.DeliverysContent.DeliveryOrder.deliveryOrderID == id).Count();
            if (count == 0)
                ViewBag.isAllNotStored = true;
            else ViewBag.isAllNotStored = false;

            return View(order);
        }

        [HttpGet]
        public ActionResult OpenItem(int? id)
        {
            var item = context.DeliverysContents.Find(id);
            if (item == null)
                return HttpNotFound();

            ViewBag.storages = storages;
            ViewBag.min = 1;
            ViewBag.max = item.contentAmount - context.ShipmentToStorages.Where(x => x.contentID == id).Sum(y => y.resourceAmount);

            return View(item);
        }

        [HttpPost, ActionName("AddOrder")]
        public ActionResult AddOrderShipment(OrderShipmentView model)
        {
            if (ModelState.IsValid)
            {
                if (context.DeliveryOrders.Find(model.orderID) == null ||
                    context.Storages.Find(model.storageID) == null)
                    return HttpNotFound();
                
                var rows = context.Database.ExecuteSqlCommand("proc_AllDeliveryToStorage @deliveryID, @storageID, @date",
                    new SqlParameter("@deliveryID", model.orderID),
                    new SqlParameter("@storageID", model.storageID),
                    new SqlParameter("@date", model.date.ToString())
                );

                if (rows == 0)
                    return HttpNotFound();
                return RedirectToAction("Index", "Purchase");
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("AddItem")]
        public ActionResult AddItemShipment(ShipmentToStorageView model)
        {
            if (ModelState.IsValid)
            {
                var shipment = new ShipmentToStorage()
                {
                    contentID = model.contentID, resourceAmount = model.amount, shipmentDate = model.date,
                    resourceStorageID = context.Resources_Storages.Where(
                        x=>x.resourceID == model.resourceID && 
                        x.storageID == model.storageID).First().resourceStorageID
                };
                using(var repo = new Repository<ShipmentToStorage>())
                {
                    if (repo.Add(shipment))
                    {
                        repo.Save();
                    }
                    else
                    {
                        HttpNotFound();
                    }
                }
                var orderID = context.DeliverysContents.Find(model.contentID).DeliveryOrder.deliveryOrderID;
                return RedirectToAction("OpenOrder", new { id = orderID });
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> AddStorage(Storage model)
        {
            using (var repository = new Repository<Storage>())
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
        public ActionResult ShipmentToStorages()
        {
            return View(context.ShipmentToStorages);
        }

        [HttpGet]
        public ActionResult OpenShipmentToStorage(int? id)
        {
            var shipment = context.ShipmentToStorages.Find(id);
            if (shipment == null)
                return HttpNotFound();

            return View(shipment);
        }
    }
}