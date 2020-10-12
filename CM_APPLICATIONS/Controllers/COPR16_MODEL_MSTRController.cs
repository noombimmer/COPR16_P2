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
using CM_APPLICATIONS.Models;

namespace CM_APPLICATIONS.Controllers
{
    public class COPR16_MODEL_MSTRController : Controller
    {
        private COPR16Entities db = new COPR16Entities();

        // GET: COPR16_MODEL_MSTR
        public async Task<ActionResult> Index()
        {
            return View(await db.COPR16_MODEL_MSTR.ToListAsync());
        }

        // GET: COPR16_MODEL_MSTR/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MODEL_MSTR cOPR16_MODEL_MSTR = await db.COPR16_MODEL_MSTR.FindAsync(id);
            if (cOPR16_MODEL_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_MODEL_MSTR);
        }

        // GET: COPR16_MODEL_MSTR/Create
        public ActionResult Create()
        {
            ItemsModel model = new ItemsModel(db);

            model.cOPR16_MODEL_MSTR = new COPR16_MODEL_MSTR();
            return View(model);
        }

        // POST: COPR16_MODEL_MSTR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MODEL_ID,BRAND_ID,MODEL_DESC,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_MODEL_MSTR cOPR16_MODEL_MSTR)
        {
            if (ModelState.IsValid)
            {
                db.COPR16_MODEL_MSTR.Add(cOPR16_MODEL_MSTR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cOPR16_MODEL_MSTR);
        }

        // GET: COPR16_MODEL_MSTR/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MODEL_MSTR cOPR16_MODEL_MSTR = await db.COPR16_MODEL_MSTR.FindAsync(id);
            if (cOPR16_MODEL_MSTR == null)
            {
                return HttpNotFound();
            }
            ItemsModel model = new ItemsModel(db);

            model.cOPR16_MODEL_MSTR = cOPR16_MODEL_MSTR;
            //return View(cOPR16_MODEL_MSTR);
            return View(model);
        }

        // POST: COPR16_MODEL_MSTR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MODEL_ID,BRAND_ID,MODEL_DESC,ADATE,CRE_BY,MOD_BY,MOD_DATE,FLGACT")] COPR16_MODEL_MSTR cOPR16_MODEL_MSTR)
        {
            if (ModelState.IsValid)
            {
                cOPR16_MODEL_MSTR.MOD_BY = System.Environment.UserName.ToLower();
                cOPR16_MODEL_MSTR.MOD_DATE = System.DateTime.Now;
                db.Entry(cOPR16_MODEL_MSTR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cOPR16_MODEL_MSTR);
        }

        // GET: COPR16_MODEL_MSTR/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MODEL_MSTR cOPR16_MODEL_MSTR = await db.COPR16_MODEL_MSTR.FindAsync(id);
            if (cOPR16_MODEL_MSTR == null)
            {
                return HttpNotFound();
            }
            return View(cOPR16_MODEL_MSTR);
        }
        // GET: COPR16_MODEL_MSTR/Delete/5
        public async Task<ActionResult> Deactive(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MODEL_MSTR cOPR16_MODEL_MSTR = await db.COPR16_MODEL_MSTR.FindAsync(id);
            if (cOPR16_MODEL_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_MODEL_MSTR.FLGACT = false;
            cOPR16_MODEL_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_MODEL_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: COPR16_MODEL_MSTR/Delete/5
        public async Task<ActionResult> Active(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COPR16_MODEL_MSTR cOPR16_MODEL_MSTR = await db.COPR16_MODEL_MSTR.FindAsync(id);
            if (cOPR16_MODEL_MSTR == null)
            {
                return HttpNotFound();
            }
            cOPR16_MODEL_MSTR.FLGACT = true;
            cOPR16_MODEL_MSTR.MOD_BY = System.Environment.UserName.ToLower();
            cOPR16_MODEL_MSTR.MOD_DATE = System.DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        // POST: COPR16_MODEL_MSTR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            COPR16_MODEL_MSTR cOPR16_MODEL_MSTR = await db.COPR16_MODEL_MSTR.FindAsync(id);
            db.COPR16_MODEL_MSTR.Remove(cOPR16_MODEL_MSTR);
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
