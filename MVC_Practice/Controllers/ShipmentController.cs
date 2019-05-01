using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CourseworkBD.DAL.DbContext;
using CourseworkBD.DAL.Models;
//using MVC_Practice.Models.DbModels;
using MVC_Practice.Repository;
using MVC_Practice.Models.ViewModels;

using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MVC_Practice.Controllers
{
    [Authorize(Roles = "admin, Storage man")]
    public class ShipmentController : Controller
    {
        private CourseworkDBContext context;

        private SelectList storages;

        //private TabCreator tabCreator;

        public ShipmentController() : base()
        {
            context = new CourseworkDBContext();
            
            storages = new SelectList(context.Storages, "storageID", "storageAddres");

            var tabCreator = new TabCreator("Storage man");
            
            string action = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");

            switch (action)
            {
                case "OpenOrder":
                    tabCreator.ChooseTab("Purchases");
                    break;
                case "OpenItem":
                    tabCreator.ChooseTab("Purchases");
                    break;
                case "Delete":
                    tabCreator.ChooseTab("Shipment to storages");
                    break;
                case "OpenShipmentToStorage":
                    tabCreator.ChooseTab("Shipment to storages");
                    break;
                case "ShipmentToStorages":
                    tabCreator.ChooseTab("Shipment to storages");
                    break;
                case "Update":
                    tabCreator.ChooseTab("Shipment to storages");
                    break;
            }
            ViewBag.tabs = tabCreator.GetTabs;
        }

        [HttpGet]
        public ActionResult OpenOrder(int? id)
        {
            var order = context.Orders.Find(id);
            if (order == null)
                return HttpNotFound();


            var items = context.DeliveryContents.Where(x => x.Order.Id == id).ToList();
            
            var itemsCheck = new List<bool>();

            foreach(var item in items)
            {
                var shipments = context.ShipmentToStorages.Where(x => x.contentID == item.Id).ToList();
                if (shipments.Count() == 0 || shipments.Sum(s => s.resourceAmount) < item.Amount)
                {
                    itemsCheck.Add(false);
                }
                else itemsCheck.Add(true);
            }

            ViewBag.storages = storages;
            ViewBag.items = items;
            ViewBag.isItemsStored = itemsCheck;

            var count = context.ShipmentToStorages.Where(x => x.DeliverysContent.Order.Id == id).Count();
            if (count == 0)
                ViewBag.isAllNotStored = true;
            else ViewBag.isAllNotStored = false;

            return View(order);
        }

        [HttpGet]
        public ActionResult OpenItem(int? id)
        {
            var item = context.DeliveryContents.Find(id);
            if (item == null)
                return HttpNotFound();

            ViewBag.storages = storages;
            ViewBag.min = 1;
            ViewBag.max = item.Amount;
            if(context.ShipmentToStorages.Count(x=>x.contentID == id) > 0)
            {
                ViewBag.max -= context.ShipmentToStorages.Where(x => x.contentID == id).Sum(y => y.resourceAmount);
            }

            return View(item);
        }

        [HttpPost, ActionName("AddOrder")]
        public ActionResult AddOrderShipment(OrderShipmentView model)
        {
            if (ModelState.IsValid)
            {
                if (context.Orders.Find(model.orderID) == null ||
                    context.Storages.Find(model.storageID) == null)
                    return HttpNotFound();
                
                var rows = context.Database.ExecuteSqlCommand("proc_AllDeliveryToStorage @deliveryID, @storageID, @date",
                    new SqlParameter("@deliveryID", model.orderID),
                    new SqlParameter("@storageID", model.storageID),
                    new SqlParameter("@date", ((DateTime)(model.date)).ToShortDateString())
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
                    contentID = model.contentID,
                    resourceAmount = model.amount,
                    shipmentDate = model.date,
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
                var orderID = context.DeliveryContents.Find(model.contentID).Order.Id;
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

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var model = context.ShipmentToStorages.Find(id);
            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (context.ShipmentToStorages.Find(id) == null)
                return HttpNotFound();

            using(var repo = new Repository<ShipmentToStorage>())
            {
                if (await repo.DeleteAsync((int)id))
                {
                    repo.Save();
                }
                else return HttpNotFound();
            }

            return RedirectToAction("ShipmentToStorages");
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            var model = context.ShipmentToStorages.Find(id);
            if (model == null)
                return HttpNotFound();

            var list = context.ShipmentToStorages
                .Where(x => x.contentID == model.contentID && x.shipmentToStorageID != model.shipmentToStorageID);

            ViewBag.min = 1;
            ViewBag.max = context.DeliveryContents.Find(model.contentID).Amount;
            if(list.Count() > 0)
            {
                ViewBag.max -= list.Sum(x => x.resourceAmount);
            }

            return View(model);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(ShipmentToStorage model)
        {
            if (model == null || model.contentID == 0 || model.shipmentToStorageID == 0 || model.shipmentDate == null || model.resourceAmount == 0)
                return HttpNotFound();

            using (var repo = new Repository<ShipmentToStorage>())
            {
                if (repo.Update(model))
                {
                    repo.Save();
                }
                else return HttpNotFound();
            }

            return RedirectToAction("ShipmentToStorages");
        }
    }
}