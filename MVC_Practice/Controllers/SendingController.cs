﻿using System;
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
    public class SendingController : Controller
    {
        private CourseworkDBContext context;

        private SelectList _projectStages;
        private SelectList _storages;

        public SendingController() : base()
        {
            context = new CourseworkDBContext();

            var stagesCheckbox = context.Projects.Select(x => new
            {
                id = x.Id,
                value = "Project: " + x.Name
            });

            _projectStages = new SelectList(stagesCheckbox, "id", "value");
            _storages = new SelectList(context.Storages, "storageID", "storageAddres");

            var tabCreator = new TabCreator("Storage man");
            tabCreator.ChooseTab("Sendings");
            ViewBag.tabs = tabCreator.GetTabs;
        }

        [HttpGet]
        public ActionResult Sendings()
        {
            return View(context.StorageShipments);
        }
        
        [HttpGet]
        public ActionResult OpenSending(int? id)
        {
            var model = context.StorageShipments.Find(id);
            if (model == null)
                return HttpNotFound();
            return View(model);
        }

        [HttpGet]
        public ActionResult Add(int? id)
        {
            var model = context.Resources_Storages.Find(id);
            if (model == null)
                return HttpNotFound();

            ViewBag.stages = _projectStages;
            ViewBag.min = 1;
            ViewBag.max = model.resourceAmount;

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(StorageShipment model)
        {
            using(var repo = new Repository<StorageShipment>())
            {
                if (repo.Add(model))
                {
                    repo.Save();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("OpenStorage", "Storage", 
                new
                {
                    id = context.Resources_Storages.Find(model.resourceStorageID).storageID
                }
            );
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            var model = context.StorageShipments.Find(id);
            if (model == null)
                return HttpNotFound();

            _projectStages.Single(x => x.Value == model.projectID.ToString()).Selected = true;
            ViewBag.stages = _projectStages;

            ViewBag.min = 1;
            ViewBag.max = model.Resources_Storages.resourceAmount + model.resourceAmount;

            return View(model);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(StorageShipment model)
        {
            using(var repo = new Repository<StorageShipment>())
            {
                if (repo.Update(model))
                {
                    repo.Save();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Sendings");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var model = context.StorageShipments.Find(id);
            if (model == null)
                return HttpNotFound();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            using(var repo = new Repository<StorageShipment>())
            {
                if(await repo.DeleteAsync((int)id))
                {
                    repo.Save();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return RedirectToAction("Sendings");
        }


    }
}