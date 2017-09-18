using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RenameFiles.Models;

namespace RenameFiles.Controllers
{
    public class RenameFilesController : Controller
    {
        private renamefilesEntities db = new renamefilesEntities();

        // GET: /RenameFiles/
        public ActionResult Index()
        {
            return View(db.CompanyStamp_tab.ToList());
        }

        // GET: /RenameFiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyStamp_tab companystamp_tab = db.CompanyStamp_tab.Find(id);
            if (companystamp_tab == null)
            {
                return HttpNotFound();
            }
            return View(companystamp_tab);
        }

        // GET: /RenameFiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /RenameFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,FilePath,FileName,FileNumber")] CompanyStamp_tab companystamp_tab)
        {
            if (ModelState.IsValid)
            {
                db.CompanyStamp_tab.Add(companystamp_tab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companystamp_tab);
        }

        // GET: /RenameFiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyStamp_tab companystamp_tab = db.CompanyStamp_tab.Find(id);
            if (companystamp_tab == null)
            {
                return HttpNotFound();
            }
            return View(companystamp_tab);
        }

        // POST: /RenameFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,FilePath,FileName,FileNumber")] CompanyStamp_tab companystamp_tab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companystamp_tab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companystamp_tab);
        }

        // GET: /RenameFiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyStamp_tab companystamp_tab = db.CompanyStamp_tab.Find(id);
            if (companystamp_tab == null)
            {
                return HttpNotFound();
            }
            return View(companystamp_tab);
        }

        // POST: /RenameFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyStamp_tab companystamp_tab = db.CompanyStamp_tab.Find(id);
            db.CompanyStamp_tab.Remove(companystamp_tab);
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
