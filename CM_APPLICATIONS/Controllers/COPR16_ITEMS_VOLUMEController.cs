using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CM_APPLICATIONS;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_ITEMS_VOLUMEController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_ITEMS_VOLUME
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_ITEMS_VOLUME.ToListAsync());
        }

        // GET: COPR16_ITEMS_VOLUME/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_ITEMS_VOLUME cOPR16_ITEMS_VOLUME = await db.COPR16_ITEMS_VOLUME.FindAsync(id);
            if (cOPR16_ITEMS_VOLUME == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_ITEMS_VOLUME);
        }

        // GET: COPR16_ITEMS_VOLUME/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COPR16_ITEMS_VOLUME/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RUNNING_ID,ITEM_ID,BUSI_DATE,QTY")] COPR16_ITEMS_VOLUME cOPR16_ITEMS_VOLUME)
        {
            if (ModelState.IsValid)
            {
                cOPR16_ITEMS_VOLUME.RUNNING_ID = Guid.NewGuid();
                db.COPR16_ITEMS_VOLUME.Add(cOPR16_ITEMS_VOLUME);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_ITEMS_VOLUME);
        }

        // GET: COPR16_ITEMS_VOLUME/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_ITEMS_VOLUME cOPR16_ITEMS_VOLUME = await db.COPR16_ITEMS_VOLUME.FindAsync(id);
            if (cOPR16_ITEMS_VOLUME == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_ITEMS_VOLUME);
        }

        // POST: COPR16_ITEMS_VOLUME/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RUNNING_ID,ITEM_ID,BUSI_DATE,QTY")] COPR16_ITEMS_VOLUME cOPR16_ITEMS_VOLUME)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOPR16_ITEMS_VOLUME).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_ITEMS_VOLUME);
        }

        // GET: COPR16_ITEMS_VOLUME/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_ITEMS_VOLUME cOPR16_ITEMS_VOLUME = await db.COPR16_ITEMS_VOLUME.FindAsync(id);
            if (cOPR16_ITEMS_VOLUME == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_ITEMS_VOLUME);
        }

        // POST: COPR16_ITEMS_VOLUME/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            COPR16_ITEMS_VOLUME cOPR16_ITEMS_VOLUME = await db.COPR16_ITEMS_VOLUME.FindAsync(id);
            db.COPR16_ITEMS_VOLUME.Remove(cOPR16_ITEMS_VOLUME);
            await db.SaveChangesAsync();
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
