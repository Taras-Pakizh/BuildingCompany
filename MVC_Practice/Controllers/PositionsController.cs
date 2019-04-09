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
    public class PositionsController : Controller
    {
        DbModels context;

        public PositionsController() : base()
        {
            context = new DbModels();

        }

        public ActionResult Index()
        {
            return View(context.Positions);
        }

        [HttpPost]
        public ActionResult Add(Position position)
        {
            //position.positionID = context.Positions.Max(x => x.positionID) + 1;
            using(var repository = new Repository<Position>())
            {
                if (repository.Add(position))
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
            var position = context.Positions.Find(id);
            if (position == null)
                return HttpNotFound();
            return View(position);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using(var repository = new Repository<Position>())
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
            var position = context.Positions.Find(id);
            if (position == null)
                return HttpNotFound();
            return View(position);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult UpdateConfirmed(Position position)
        {
            if (position == null)
                return HttpNotFound();
            using(var repository = new Repository<Position>())
            {
                if (repository.Update(position))
                {
                    repository.Save();
                }
                else repository.Save();
            }
            return RedirectToAction("Index");
        }
    }
}