using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GuessAnimal.Models;

namespace GuessAnimal.Controllers
{
    public class AnimalDetailsController : Controller
    {
        private AnimalEntities db = new AnimalEntities();

        // GET: AnimalDetails
        public ActionResult Index()
        {
            var animalDetails = db.AnimalDetails.Include(a => a.Animal);
            return View(animalDetails.ToList());
        }

        public ActionResult Question()
        {
            return View("Question");
        }

        // GET: AnimalDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }
            return View(animalDetail);
        }

        // GET: AnimalDetails/Create
        public ActionResult Create()
        {
            ViewBag.AnimalId = new SelectList(db.Animals, "Id", "Name");
            return View();
        }

        // POST: AnimalDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FactId,Facts,AnimalId")] AnimalDetail animalDetail)
        {
            if (ModelState.IsValid)
            {
                db.AnimalDetails.Add(animalDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnimalId = new SelectList(db.Animals, "Id", "Name", animalDetail.AnimalId);
            return View(animalDetail);
        }

        // GET: AnimalDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalId = new SelectList(db.Animals, "Id", "Name", animalDetail.AnimalId);
            return View(animalDetail);
        }

        // POST: AnimalDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FactId,Facts,AnimalId")] AnimalDetail animalDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animalDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnimalId = new SelectList(db.Animals, "Id", "Name", animalDetail.AnimalId);
            return View(animalDetail);
        }

        // GET: AnimalDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }
            return View(animalDetail);
        }

        // POST: AnimalDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            db.AnimalDetails.Remove(animalDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
