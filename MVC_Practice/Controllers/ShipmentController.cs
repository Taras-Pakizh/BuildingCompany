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
    public class ShipmentController : Controller
    {
        private DbModels context;

        private SelectList storages;

        public ShipmentController() : base()
        {
            context = new DbModels();
            
            storages = new SelectList(context.Storages, "storageID", "storageAddres");
        }

        public ActionResult Storages()
        {
            return View(context.Storages);
        }

        public ActionResult OpenStorage(int? id)
        {
            var storage = context.Storages.Find(id);
            if (storage == null)
                return HttpNotFound();

            ViewBag.resources = context.Resources_Storages.Where(x => x.storageID == id);

            return View(storage);
        }

        public ActionResult OpenOrder(int? id)
        {
            var order = context.DeliveryOrders.Find(id);
            if (order == null)
                return HttpNotFound();

            ViewBag.storages = storages;
            ViewBag.items = context.DeliverysContents.Where(x => x.deliveryOrderID == id);

            return View(order);
        }

        public ActionResult OpenItem(int? id)
        {
            var item = context.DeliverysContents.Find(id);
            if (item == null)
                return HttpNotFound();

            ViewBag.storages = storages;

            return View(item);
        }

        [HttpPost, ActionName("Add")]
        public ActionResult AddOrderShipment(OrderShipmentView model)
        {
            if (ModelState.IsValid)
            {
                if (context.DeliveryOrders.Find(model.orderID) == null ||
                    context.Storages.Find(model.storageID) == null)
                    return HttpNotFound();
                var orderID = new SqlParameter("@deliveryID", model.orderID);
                var storageID = new SqlParameter("@storageID", model.storageID);
                var date = new SqlParameter("@date", model.date);
                context.Database.SqlQuery(null, "proc_AllDeliveryToStorage @deliveryID, @storageID, @date", orderID, storageID, date);

                return RedirectToAction("Open", "Purchase", new { id = model.orderID });
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("Add")]
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

        public ActionResult Shipments()
        {
            return View(context.ShipmentToStorages);
        }

    }
}