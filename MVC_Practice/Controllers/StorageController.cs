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
    public class StorageController : Controller
    {
        private DbModels context;

        public StorageController() : base()
        {
            context = new DbModels();

            var tabCreator = new TabCreator("Storage man");
            tabCreator.ChooseTab("Storages");
            ViewBag.tabs = tabCreator.GetTabs;
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
        public ActionResult Update(int? id)
        {
            var storage = context.Storages.Find(id);
            if (storage == null)
                return HttpNotFound();

            return View(storage);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(Storage model)
        {
            if (model == null || model.storageID == 0 || model.storageAddres == null)
                return HttpNotFound();

            using(var repo = new Repository<Storage>())
            {
                if (repo.Update(model))
                {
                    repo.Save();
                }
                else return HttpNotFound();
            }

            return RedirectToAction("Storages");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var storage = context.Storages.Find(id);
            if (storage == null)
                return HttpNotFound();

            return View(storage);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            var model = context.Storages.Find(id);
            if (model == null)
                return HttpNotFound();

            using(var repo = new Repository<Storage>())
            {
                if (await repo.DeleteAsync((int)id))
                {
                    repo.Save();
                }
                else return HttpNotFound();
            }

            return RedirectToAction("Storages");
        }
    }
}